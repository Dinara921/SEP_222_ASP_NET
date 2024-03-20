$(document).ready(function () 
    {
        $("#myButton1").click(function () 
        {                   
            var city='<table border="1">';
            city+='<thead><tr><td>ID</td><td>Name</td></tr></thead>';
            city+='<tr><td>1</td><td>Almaty</td></tr>';
            city+='<tr><td>2</td><td>Astana</td></tr>';
            city+='<tr><td>3</td><td>Qostanay</td></tr>';
            city+='<tr><td>4</td><td>Qaraganda</td></tr>';
            city+='</table>';

            $("#city").html(city)     
        });

        $("#myButton2").click(function () 
        {                   
            $.ajax({
                       type: "GET",
                       url: "test/GetCityAll",
                       success: function (data) 
                       {
                           console.warn(data);
                           let html = '';
                           html += "<table border='1' cellpadding='1' cellspacing='1' width='500'>";
                           html += "<tr bgcolor='#ffd400'>";
                           html += "<td class='text-center'>id</td>"
                           html += "<td class='text-center'>name</td>";
                           html += "</tr>";
                           $.each(data, function (i, item) 
                           {
                                html += "<tr><td>" + item.id + "</td>" +
                               "<td>" + item.name + "</td></tr>";
                           });
                           html += "</table >";
                           $("#city").html(html);
                        },
                           error: function () 
                           {
                                console.log("Error");
                           }
                  });
        });
        $("#myButton3").click(function () 
                {
                    $.ajax({
                           type: "GET",
                           url: "test/GetCityAll",
                           success: function (data) 
                           {
                               $("#city").empty();
                               var selectOptions = "";
                               $.each(data, function (index, row) 
                               {
                                    selectOptions += "<option value='" + row.id + "'>" + row.name + "</option>";
                               });
                               var select = "<select>" + selectOptions + "</select>";
                               $("#city").append(select);
                            },
                            error: function () 
                            {
                                console.log("Error");
                            }
                        });
                        
        });      
        function showModal()
        {
            $("#myModal").modal("show");
        }

        function createCity() 
        {
            var MyData = 
            {
                "id": 4,
                "name": "Aktobe"
            };
            $.ajax
                ({
                    type: "POST",
                    headers: 
                    {
                        'Content-Type': 'application/json'
                    },
                    url: "test/createCity",
                    data: JSON.stringify(MyData),
                    success: function (data) 
                    {
                        console.log(data);
                    },
                    error: function () 
                    {
                        console.log("error")
                    }
                });
        }

        function confirm() 
        {
        
            $.confirm({
                title: 'Are you sure to delete book!',
                content: 'Deletion!',
                buttons: 
                {
                    confirm: function () 
                    {
                        //$.alert('go to delete');
        
                    },
                    cancel: function () 
                    {
                        //$.alert('cancel delete!');
                    }
                }
            });
        }
});
