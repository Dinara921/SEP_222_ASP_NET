$(document).ready(function () {       
    $("#category").change(function () {                   
        var category = $(this).val();
        $.ajax({
            type: "GET",
            url: "Music/GetAllOrCategoryMusic",
            data: { category: category },
            success: function (data) { 
                $("#Grid_block").empty(); 

                var tableHtml = '<table class="table table-bordered table-light">' +
                                    '<thead class="thead-dark">' +
                                        '<tr>' +
                                            '<th>ID</th>' +
                                            '<th>Название</th>' +
                                            '<th>Категория</th>' +
                                            '<th>Длительность</th>' +
                                            '<th>Действия</th>' +
                                        '</tr>' +
                                    '</thead>' +
                                    '<tbody>';
                
                $.each(data, function(index, track) 
                {
                    tableHtml += '<tr>' +
                                    '<td>' + track.id + '</td>' +
                                    '<td>' + track.name + '</td>' +
                                    '<td>' + track.category + '</td>' +
                                    '<td>' + track.duration + '</td>' +
                                    '<td>' +
                                        '<button class="btn btn-primary edit-track" data-trackid="' + track.id + '"><i class="fas fa-pencil-alt"></i></button>' +
                                        '<button class="btn btn-danger delete-track" data-trackid="' + track.id + '"><i class="fas fa-trash"></i></button>' +
                                    '</td>' +
                                '</tr>';
                });

                tableHtml += '</tbody></table>';

                $("#Grid_block").append(tableHtml);

                $(".delete-track").click(function () 
                {
                    var trackId = $(this).data("trackid");
                    deleteTrack(trackId);
                });


                $(".edit-track").click(function ()  
                {
                    var trackId = $(this).data("trackid");
                    var trackName = $(this).closest("tr").find("td:nth-child(2)").text(); 
                    var trackCategory = $(this).closest("tr").find("td:nth-child(3)").text(); 
                    var trackDuration = $(this).closest("tr").find("td:nth-child(4)").text(); 
        
                    EditTrack(trackId, trackName, trackCategory, trackDuration); 
                });
            }
        });
    });

});


function refreshTrackList() {
    $.ajax({
        type: "GET",
        url: "Music/GetAllOrCategoryMusic",
        data: { category: $("#category").val() },
        success: function (data) {
            $("#Grid_block").empty(); 
            var tableHtml = '<table class="table table-bordered table-light">' +
                                '<thead class="thead-dark">' +
                                    '<tr>' +
                                        '<th>ID</th>' +
                                        '<th>Название</th>' +
                                        '<th>Категория</th>' +
                                        '<th>Длительность</th>' +
                                        '<th>Действия</th>' +
                                    '</tr>' +
                                '</thead>' +
                                '<tbody>';
            $.each(data, function(index, track) 
            {
                tableHtml += '<tr>' +
                                '<td>' + track.id + '</td>' +
                                '<td>' + track.name + '</td>' +
                                '<td>' + track.category + '</td>' +
                                '<td>' + track.duration + '</td>' +
                                '<td>' +
                                    '<button class="btn btn-primary edit-track" data-trackid="' + track.id + '"><i class="fas fa-pencil-alt"></i></button>' +
                                    '<button class="btn btn-danger delete-track" data-trackid="' + track.id + '"><i class="fas fa-trash"></i></button>' +
                                '</td>' +
                            '</tr>';
            });
            tableHtml += '</tbody></table>';
            $("#Grid_block").append(tableHtml);

            $(".delete-track").click(function () 
            {
                var trackId = $(this).data("trackid");
                deleteTrack(trackId);
            }); 
        },
        error: function (xhr, status, error) 
        {
            console.log("Error");
        }
    });
}

function deleteTrack(trackId) 
{
    $.ajax({
        type: "GET",
        url: "Music/DeleteMusic",
        data: { id: trackId },
        success: function (response) 
        {
            refreshTrackList();
        },
        error: function (xhr, status, error) 
        {
            console.log("Error");
        }
    });
    
}

function EditTrack(trackId, name, category, duration) 
{
    var requestData = {
        id: trackId,
        name: name,
        category: category,
        duration: duration
    };
    $.ajax({
        type: "POST",
            headers: 
            {
                'Content-Type': 'application/json'
            },
        url: "Music/AddOrEditMusic",
        data: JSON.stringify(requestData),
        success: function (response) 
        {
            showModal();
            refreshTrackList();
        },
        error: function (xhr, status, error)
         {
            console.log("Error");
        }
    });
}

function showModal() 
{
    console.log('show');
    $("#editModal").modal("show");    
}