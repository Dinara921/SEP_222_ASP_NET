$(document).ready(function () {
    $("#category1, #category2").change(function () {
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
                        '</div>' +
                        '</td>' +
                        '</tr>';
                });


                tableHtml += '</tbody></table>';

                $("#Grid_block").append(tableHtml);


                $(".register-training").click(function () {
                    var $row = $(this).closest("tr");
                    var trainingId = $row.find("td:eq(0)").text();
                    $('#training_id').val(trainingId)
                    $('#registerModal').modal('show');
                });

            }
        });
    });

    $("#zapis").click(function () {
        $("#myModal").modal("show");
        var user_id = window.localStorage.getItem('user_id')
        console.log(user_id)
        $.ajax({
            type: "GET",
            url: "Gym/GetTrainingAttendance",
            data: { user_id: user_id },
            success: function (response) {
                $("#gridContainer").empty();
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


    $("#closeButton").click(function () {
        $("#myModal").modal("hide");
    });

    $(document).on("click", ".delete-tr", function () {
        var TrainingID = $(this).data("trainingid")
        deleteTrainingAttendance(TrainingID);
    });

    function deleteTrainingAttendance(TrainingID) {
        $.ajax({
            type: "GET",
            url: "Gym/DeleteTrainingAttendance",
            data: { id: TrainingID },
            success: function (response) {
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
function refreshZapis(user_id) {
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
                '<th>Время тренировки</th>' +
                '<th>Статус занятий</th>' +
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
                    '<td>' + training.time + '</td>' +
                    '<td>' + training.status + '</td>' +
                    '<td>' + training.quantity + '</td>' +
                    '</tr>';
            });

            tableHtml += '</tbody></table>';

            $("#Grid_block").append(tableHtml);

            $(".delete-training").click(function () {
                var trainingId = $(this).data("trainingId");
                confirmDel(trainingId);
            });
        },
        error: function (xhr, status, error) {
            console.log("Error");
        }
    });
}

$(document).ready(function () {

    $("#RegisterModalBtn").click(function () {
        $("#registerModal").show();
    });

    $(".close").click(function () {
        $("#registerModal").hide();
    });

    $(".closeBtn").click(function () {
        $("#registerModal").hide();
    });

    $("#saveBtn").click(function () {
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




