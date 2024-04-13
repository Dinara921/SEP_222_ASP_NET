$(document).ready(function () 
{
    $("#category1, #category2").change(function () 
    {      
        console.log($(this).val()); 
        var category = $(this).val();
        refreshTrainingList();
        $.ajax({
            type: "GET",
            url: "Gym/GetAllOrTrainerIdOrHallId",
            data: { hallOrTrId: category },
            success: function (data) {
                $("#Grid_block").empty();

                var tableHtml = '<table class="table table-bordered table-light">' +
                    '<thead class="thead-dark">' +
                    '<tr>' +
                    '<th>ID</th>' +
                    '<th>Тренер</th>' +
                    '<th>Зал</th>' +
                    '<th>Тип тренировок</th>'+
                    '<th>Время</th>' +
                    '<th>Формат занятий</th>' +
                    '<th>Количество в группе</th>' +
                    '</tr>' +
                    '</thead>' +
                    '<tbody>';

                $.each(data, function (index, training) 
                {
                    tableHtml += '<tr>' +
                        '<td>' + training.id + '</td>' +
                        '<td>' + training.trainer + '</td>' +
                        '<td>' + training.hall + '</td>' +
                        '<td>' + training.special + '</td>' +
                        '<td>' + training.time + '</td>' +
                        '<td>' + training.status + '</td>' +
                        '<td>' + training.quantity + '</td>' +
                        '<td>' +
                        '<div class="btn-group">' +
                        '<button class="btn btn-primary edit-training" data-trainingId="' + training.id + '"><i class="fas fa-pencil-alt"></i></button>' +
                        '<button class="btn btn-danger delete-training" data-trainingId="' + training.id + '"><i class="fas fa-trash"></i></button>' +
                        '<button class="btn btn-success register-training" data-trainingId="' + training.id + '"><i class="fas fa-plus"></i></button>' +  
                        '</div>' +
                        '</td>' +
                        '</tr>';
                });


                tableHtml += '</tbody></table>';

                $("#Grid_block").append(tableHtml);

                $(".delete-training").click(function () {
                    var $row = $(this).closest("tr");
                    var trainingId = $row.find("td:eq(0)").text();
                    confirmDel(trainingId);
                });

                $(".register-training").click(function () 
                {
                    var $row = $(this).closest("tr");
                    var trainingId = $row.find("td:eq(0)").text();
                    $('#training_id').val(trainingId)
                    $('#registerModal').modal('show'); 
                });

                $(".edit-training").click(function () {
                    var $row = $(this).closest("tr");
                    var trainingId = $row.find("td:eq(0)").text();
                    var trainingTrainer = $row.find("td:eq(1)").text();
                    var trainingTime = $row.find("td:eq(4)").text();
                    var trainingHall = $row.find("td:eq(6)").text();
                    var trainingStatus = $row.find("td:eq(3)").text();
                    var trainingQuantity = $row.find("td:eq(2)").text();

                    console.log("ID тренировки:", trainingId);
                    console.log("Тренер:", trainingTrainer);
                    console.log("Время :", trainingTime);
                    console.log("Тип тренировки:", trainingStatus);
                    console.log("Зал:", trainingHall);
                    console.log("Количество в группе:", trainingQuantity);

                    $("#trainingId").val(trainingId);
                    $("#trainingTrainer").val(trainingTrainer);
                    $("#trainingTime").val(trainingTime);
                    $("#trainingStatus").val(trainingStatus);
                    $("#trainingHall").val(trainingHall);
                    $("#trainingQuantity").val(trainingQuantity);
                    $('#AddModal').modal('show');

                    
                });
            }
        });
    });

    $("#add").click(function () 
    {
        showModal();
    });

    $('#saveButton').click(function () 
    {
        confirmSave();
    });

    $('#closeButton').click(function () 
    {
        console.log('close')
        $('#AddModal').modal('hide');
    });

    $('#cancelSaveButton').click(function () 
    {
        $('#confirmModal').modal('hide');
    });

    $("#zapis").click(function () {
        $("#myModal").modal("show");
        var user_id = window.localStorage.getItem('user_id')
        console.log(user_id)
        $.ajax({
            type: "GET",
            url: "Gym/GetTrainingAttendance",
            data: { user_id: user_id },
            success: function (response) 
            {
                $("#gridContainer").empty();
                console.log(response)
                if (response.length > 0) 
                {
                    var tableHtml = '<table class="table table-bordered table-light">' +
                        '<thead class="thead-dark">' +
                        '<tr>' +
                        '<th>ID</th>' +
                        '<th>Тип тренировок</th>' +
                        '<th>Время</th>' +
                        '<th>Формат тренировок</th>' +
                        '<th>Количество в группе</th>' +
                        '<th>Зал</th>' +
                        '<th>Тренер</th>' +
                        '<th>Клиент</th>' +
                        '<th>Действия</th>' +
                        '</tr>' +
                        '</thead>' +
                        '<tbody>';

                    $.each(response, function (index, item) 
                    {
                        tableHtml += '<tr>' +
                            '<td>' + item.trainingID + '</td>' +
                            '<td>' + item.trainingType + '</td>' +
                            '<td>' + item.trainingTime + '</td>' +
                            '<td>' + item.trainingFormat + '</td>' +
                            '<td>' + item.maxCapacity + '</td>' +
                            '<td>' + item.hall + '</td>' +
                            '<td>' + item.trainer + '</td>' +
                            '<td>' + item.clientName + '</td>' +
                            '<td>' +
                            '<button class="btn btn-danger delete-tr" data-trainingID="' + item.trainingID + '"><i class="fas fa-trash"></i></button>' +
                            '</td>' +
                            '</tr>';
                    });
                    console.log(tableHtml)
                    tableHtml += '</tbody></table>'

                    $("#gridContainer").html(tableHtml)
                } else {
                    $("#gridContainer").html("<p>Нет доступных тренировок.</p>")
                }
            },
            error: function () {
                alert("An error occurred while fetching training details.")
            }
        });
    });


    $("#closeButton").click(function () 
    {
        $("#myModal").modal("hide");
    });

    $(document).on("click", ".delete-tr", function () 
    {
        var TrainingID = $(this).data("trainingid")
        deleteTrainingAttendance(TrainingID);
    });

    function deleteTrainingAttendance(TrainingID) 
    {
        $.ajax({
            type: "GET",
            url: "Gym/DeleteTrainingAttendance",
            data: { id: TrainingID },
            success: function (response) 
            {
                var user_id = window.localStorage.getItem('user_id')
                console.log("Запись удачно удалена")
                refreshZapis(user_id)
                alert("Запись успешно удалена.")
            },
            error: function () {
                alert("An error occurred while deleting training.")
            }
        });
    }

});
function refreshZapis(user_id)
{
    $.ajax({
        type: "GET",
        url: "Gym/GetTrainingAttendance",
        data: { user_id: user_id },
        success: function (response) {
            $("#gridContainer").empty()
            console.log(response)
            if (response.length > 0) {
                var tableHtml = '<table class="table table-bordered table-light">' +
                    '<thead class="thead-dark">' +
                    '<tr>' +
                    '<th>ID</th>' +
                    '<th>Тип тренировок</th>' +
                    '<th>Время</th>' +
                    '<th>Формат тренировок</th>' +
                    '<th>Количество в группе</th>' +
                    '<th>Зал</th>' +
                    '<th>Тренер</th>' +
                    '<th>Клиент</th>' +
                    '<th>Действия</th>' +
                    '</tr>' +
                    '</thead>' +
                    '<tbody>';

                $.each(response, function (index, item) {
                    tableHtml += '<tr>' +
                        '<td>' + item.trainingID + '</td>' +
                        '<td>' + item.trainingType + '</td>' +
                        '<td>' + item.trainingTime + '</td>' +
                        '<td>' + item.trainingFormat + '</td>' +
                        '<td>' + item.maxCapacity + '</td>' +
                        '<td>' + item.hall + '</td>' +
                        '<td>' + item.trainer + '</td>' +
                        '<td>' + item.clientName + '</td>' +
                        '<td>' +
                        '<button class="btn btn-danger delete-tr" data-trainingID="' + item.trainingID + '"><i class="fas fa-trash"></i></button>' +
                        '</td>' +
                        '</tr>';
                });
                console.log(tableHtml)
                tableHtml += '</tbody></table>'

                $("#gridContainer").html(tableHtml)
            } else {
                $("#gridContainer").html("<p>Нет доступных тренировок.</p>")
            }
        },
        error: function () {
            alert("An error occurred while fetching training details.")
        }
    });
}
function refreshTrainingList() {
    $.ajax({
        type: "GET",
        url: "Gym/GetAllOrTrainerIdOrHallId",
        data: { hallOrTrId: $(".category").val() },
        success: function (data) {
            $("#Grid_block").empty();

            var tableHtml = '<table class="table table-bordered table-light">' +
                '<thead class="thead-dark">' +
                '<tr>' +
                '<th>ID</th>' +
                '<th>Тренер</th>' +
                '<th>Зал</th>' +
                '<th>Тип тренировок</th>' +
                '<th>Время</th>' +
                '<th>Формат занятий</th>' +
                '<th>Количество в группе</th>' +
                '</tr>' +
                '</thead>' +
                '<tbody>';

            $.each(data, function (index, training) {
                tableHtml += '<tr>' +
                    '<td>' + training.id + '</td>' +
                    '<td>' + training.trainer + '</td>' +
                    '<td>' + training.hall + '</td>' +
                    '<td>' + training.special + '</td>' +
                    '<td>' + training.time + '</td>' +
                    '<td>' + training.status + '</td>' +
                    '<td>' + training.quantity + '</td>' +
                    '<td>' +
                    '<div class="btn-group">' +
                    '<button class="btn btn-primary edit-training" data-trainingId="' + training.id + '"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger delete-training" data-trainingId="' + training.id + '"><i class="fas fa-trash"></i></button>' +
                    '<button class="btn btn-success register-training" data-trainingId="' + training.id + '"><i class="fas fa-plus"></i></button>' +
                    '</div>' +
                    '</td>' +
                    '</tr>';
            });

            tableHtml += '</tbody></table>';

            $("#Grid_block").append(tableHtml);

            $(".delete-training").click(function () 
            {
                var $row = $(this).closest("tr");
                var trainingId = $row.find("td:eq(0)").text();
                confirmDel(trainingId);
            });
        },
        error: function (xhr, status, error) {
            console.log("Error");
        }
    });
}

function deleteTraining(trainingId) 
{
    $.ajax({
        type: "GET",
        url:"Gym/DeleteTraining",
        data: { training_id: trainingId }, 
        success: function (response) 
        {
            refreshTrainingList();
        },
        error: function (xhr, status, error) 
        {
            console.log("Error");
        } 
    });
}

function confirmSave() {
    $('#confirmModal').modal('show');
}

function confirmDel(trainingId) {
    console.log(trainingId);
    $('#confirmModalDel').modal('show').on('click', '#confirmSaveButton', function () {
        $('#confirmModalDel').modal('hide');
        $('#AddModal').modal('hide');
        deleteTraining(trainingId);
    });
}

function showModal() {
    console.log('show');
    $("#AddModal").modal("show");
}

function confirmSave() {
    var trainingId = $('#trainingId').val()
    var trainingTrainer = $('#trainingTrainer').val()
    var trainingTime = $('#trainingTime').val()
    var trainingStatus = $('#trainingStatus').val()
    var trainingHall = $('#trainingHall').val()
    var trainingQuantity = $('#trainingQuantity').val()

    $('#confirmModal').modal('show').on('click', '#confirmSaveButton', function () {
        $('#confirmModal').modal('hide');
        $('#AddModal').modal('hide');
        
        AddOrEditTraining(trainingId, trainingTrainer, trainingTime, trainingStatus, trainingHall, trainingQuantity);
    });
}

function AddOrEditTraining(trainingId, trainingTrainer, trainingTime, trainingStatus, trainingHall, trainingQuantity) {
    refreshTrainingList()
    requestData =
    {
        id: trainingId,
        trainer_id: trainingTrainer,
        timeT_id: trainingTime,
        status_id: trainingStatus,
        hall_id: trainingHall,
        max_capacity: trainingQuantity
    };

    $.ajax({
        type: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        url: "Gym/AddOrUpdateTraining",
        data: JSON.stringify(requestData),
        success: function (response) 
        {
            if (response.result === 0) 
            {
                alert("Тренировка успешно добавлена или обновлена");
            } else if (response.result === -1) 
            {
                alert("Этот временной интервал уже занят для тренировки");
            } else if (response.result === -2) 
            {
                alert("Тренер недоступен в указанное время");
            } else if (response.result === -3) 
            {
                alert("Произошла ошибка при обработке вашего запроса");
            } else {
                alert("Произошла неизвестная ошибка");
            }
        },
        error: function (xhr, status, error) 
        {
            console.log("Error");
            var errorMessage = "Произошла ошибка при обработке запроса.";
            if (xhr.status === 400) {
                errorMessage = xhr.responseText;
            } else if (xhr.status === 500) {
                errorMessage = "Произошла внутренняя ошибка сервера.";
            }
            $("#error-message").text(errorMessage).show();
        }
    });
}

$(document).ready(function () 
{
    $("#trainerButton").click(function () 
    {
        window.location.href = "trainerInf.html";
    });

    $("#clientButton").click(function () 
    {
        window.location.href = "clientInf.html";
    });
})

$(document).ready(function () 
{

    $("#RegisterModalBtn").click(function () 
    {
        $("#registerModal").show();
    });

    $(".close").click(function () 
    {
        $("#registerModal").hide();
    });

    $(".closeBtn").click(function () 
    {
        $("#registerModal").hide();
    });

    $("#saveBtn").click(function () 
    {
        var client_id = $("#client_id").val();
        var training_id = $("#training_id").val();

        if (confirm("Вы уверены, что хотите записаться на тренировку?")) {
            $.ajax({
                type: "GET",
                url: "Gym/TrainingAttendance",
                data: {
                    client_id: client_id,
                    training_id: training_id
                },
                success: function (response) {
                    if (response.result === 0) {
                        alert("Вы успешно записались на тренировку!");
                    } else if (response.result === -1) {
                        alert("Ошибка: тренировка не существует!");
                    } else if (response.result === -2) {
                        alert("Ошибка: все места на тренировке заняты!");
                    } else if (response.result === -3) {
                        alert("Ошибка: места на тренировке ограничены и уже заняты!");
                    } else if (response.result === -4) {
                        alert("Ошибка при обработке запроса!");
                    }
                    $('#registerModal').modal('hide');
                },
                error: function (xhr, status, error) {
                    console.log("Ошибка:", error);
                }
            });
        }
    });
});



