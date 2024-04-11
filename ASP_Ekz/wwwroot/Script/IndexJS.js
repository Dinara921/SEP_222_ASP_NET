$(document).ready(function () {
    $('#loginForm').submit(function (event) {
        event.preventDefault();

        var email = $('#exampleInputEmail1').val();
        var pwd = $('#exampleInputPassword1').val();

        var formData = {
            email: email,
            pwd: pwd
        };

        $.ajax({
            type: "GET",
            url: "Gym/AdminOrUser",
            data: formData,
            success: function (response) {
                if (response === "Admin") {
                    window.location.href = '/adminPanel.html';
                } else if (response === "Client") {
                    window.location.href = '/clientPanel.html';
                } else if (response === "Trainer") {
                    window.location.href = '/trainerPanel.html';
                } else {
                    alert("Unknown user type or invalid credentials.");
                }
            },
            error: function () {
                alert("An error occurred while processing your request.");
            }
        });
    });
});
