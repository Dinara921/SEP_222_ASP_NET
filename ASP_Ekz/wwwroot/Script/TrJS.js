$(document).ready(function () {
    $("#search").click(function () {
        var fio = $("#tb_search").val();
        if (fio == "")
            fio = "all";
        refreshTrainerList(fio);
    });

    function refreshTrainerList(fio) {
        if(fio == ""|| fio == null)
        fio="all"
        $.ajax({
            type: "GET",
            url: "Gym/GetAllOrTrainerFio",
            data: { fio: fio },
            success: function (data) {
                $("#Grid_block").empty();

                var tableHtml = '<table class="table table-bordered table-light">' +
                    '<thead class="thead-dark">' +
                    '<tr>' +
                    '<th>ID</th>' +
                    '<th>Тренер</th>' +
                    '<th>Дата рождения</th>' +
                    '<th>Номер</th>' +
                    '<th>E-mail</th>' +
                    '<th>Пол</th>' +
                    '<th>Статус</th>' +
                    '<th>Тренировки</th>' +
                    '</tr>' +
                    '</thead>' +
                    '<tbody>';

                $.each(data, function (index, trainer) {
                    tableHtml += '<tr>' +
                        '<td>' + trainer.id + '</td>' +
                        '<td>' + trainer.fio + '</td>' +
                        '<td>' + trainer.dateBirth + '</td>' +
                        '<td>' + trainer.number + '</td>' +
                        '<td>' + trainer.email + '</td>' +
                        '<td>' + trainer.gender + '</td>' +
                        '<td>' + trainer.category + '</td>' +
                        '<td>' + trainer.special + '</td>' +
                        '<td>' +
                        '<button class="btn btn-primary edit-trainer" data-trainerId="' + trainer.id + '"><i class="fas fa-pencil-alt"></i></button>' +
                        '<button class="btn btn-danger delete-trainer" data-trainerId="' + trainer.id + '"><i class="fas fa-trash"></i></button>' +
                        '</td>' +
                        '</tr>';
                });

                tableHtml += '</tbody></table>';

                $("#Grid_block").append(tableHtml);

                $(".delete-trainer").click(function () 
                {
                    var $row = $(this).closest("tr");
                    var trainerId = $row.find("td:eq(0)").text();
                    confirmDel(trainerId);
                });

                $(".edit-trainer").click(function () 
                {
                    var $row = $(this).closest("tr");
                    var trainerId = $row.find("td:eq(0)").text();
                    var trainerFio = $row.find("td:eq(1)").text();
                    var trainerDateBirth = $row.find("td:eq(2)").text();
                    var trainerNumber = $row.find("td:eq(3)").text();
                    var trainerEmail = $row.find("td:eq(4)").text();
                    var trainerGender = $row.find("td:eq(5) select").val();
                    var trainerCategory = $row.find("td:eq(6) select").val();
                    var trainerSpecial = $row.find("td:eq(7) select").val();

                    $("#trainerId").val(trainerId);
                    $("#trainerFio").val(trainerFio);
                    $("#trainerDateBirth").val(trainerDateBirth);
                    $("#trainerNumber").val(trainerNumber);
                    $("#trainerEmail").val(trainerEmail);
                    $("#trainerGender").val(trainerGender);
                    $("#trainerCategory").val(trainerCategory);
                    $("#trainerSpecial").val(trainerSpecial);

                    $("#AddModal").modal("show");
                });
            },
            error: function (xhr, status, error) {
                console.log("Error");
            }
        });
    }

    function deleteTrainer(trainerId) {
        console.log(trainerId);
        $.ajax({
            type: "GET",
            url: "Gym/DeleteTrainer",
            data: { id: trainerId },
            success: function (response) {
                refreshTrainerList();
            },
            error: function (xhr, status, error) {
                console.log("Error");
            }
        });
    }

    function confirmDel(trainerId) 
    {
        $('#confirmModalDel').modal('show').on('click', '#confirmSaveButton', function () {
            $('#confirmModalDel').modal('hide');
            console.log(trainerId);
            deleteTrainer(trainerId);
        });
    }

    function showModal() {
        console.log('show');
        $("#AddModal").modal("show");
    }
    $('#saveButton').click(function () {
        confirmSave();
    });

    function confirmSave() {
        $('#confirmModal').modal('show');
    }

    function AddOrEditTraining(trainerId, trainerFio, trainerDateBirth, trainerNumber, trainerGender, trainerCategory, trainerSpecial, trainerEmail, trainerPwd, trainerRole_id) {
        refreshTrainerList()
        var requestData = 
        {
            trainer_id: trainerId,
            fio: trainerFio,
            dateBirth: trainerDateBirth,
            number: trainerNumber,
            gender: trainerGender,
            category_id: trainerCategory,
            special_id: trainerSpecial,
            email: trainerEmail,
            pwd: trainerPwd,
            role_id: trainerRole_id
        };

        $.ajax({
            type: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            url: "Gym/AddOrUpdateTrainer",
            data: JSON.stringify(requestData),
            success: function (response) {
                refreshTrainerList();
            },
            error: function (xhr, status, error) {
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

    $("#add").click(function () {
        showModal();
    });

    function confirmSave() {
        var trainerId = $('#trainerId').val();
        var trainerFio = $('#trainerFio').val();
        var trainerDateBirth = $('#trainerDateBirth').val();
        var trainerNumber = $('#trainerNumber').val();
        var trainerGender = $('#trainerGender').val();
        var trainerCategory = $('#trainerCategory').val();
        var trainerSpecial = $('#trainerSpecial').val();
        var trainerEmail = $('#trainerEmail').val();
        var trainerPwd = $('#trainerPwd').val();
        var trainerRole_id = $('#trainerRole_id').val();

        $('#confirmModal').modal('show').on('click', '#confirmSaveButton', function () {
            $('#confirmModal').modal('hide');
            $('#AddModal').modal('hide');
            AddOrEditTraining(trainerId, trainerFio, trainerDateBirth, trainerNumber, trainerGender, trainerCategory, trainerSpecial, trainerEmail, trainerPwd, trainerRole_id);
        });
    }

    $('#closeButton').click(function () {
        console.log('close');
        $('#AddModal').modal('hide');
    });

    $('#cancelSaveButton').click(function () {
        $('#confirmModal').modal('hide');
    });

});


