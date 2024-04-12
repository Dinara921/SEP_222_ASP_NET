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
            type: "POST",
            url: "Gym/AdminOrUser",
            contentType: "application/json", 
            data: JSON.stringify(formData), 
            success: function (response) 
            {
                var userType = response.userType;
                var user_id = response.user_id;

                window.localStorage.setItem('user_id', user_id)
                console.log(window.localStorage.setItem('user_id', user_id))
            
                if (userType === "Admin") {
                    window.localStorage.setItem('userType', 'Admin');
                    window.location.href = '/adminPanel.html';
                } else if (userType === "Client") {
                    window.localStorage.setItem('userType', 'Client');
                    window.location.href = '/clientPanel.html';
                } else if (userType === "Trainer") {
                    window.localStorage.setItem('userType', 'Trainer');
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
