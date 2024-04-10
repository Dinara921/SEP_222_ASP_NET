$(document).ready(function () {       
    $("#category").change(function () 
    {                   
        var category = $(this).val();
        $.ajax({
            type: "GET",
            url: "Music/GetAllOrCategoryMusic",
            data: { category: category },
            success: function (data) 
            { 
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
                    console.log(trackId);
                    confirmDel(trackId);
                    // var trackId = $(this).data("trackid");
                    // deleteTrack(trackId);
                });


                $(".edit-track").click(function ()  
                { 
                    var trackId = $(this).data("trackid");
                    var trackName = $(this).closest("tr").find("td:nth-child(2)").text(); 
                    var trackCategory = $(this).closest("tr").find("td:nth-child(3)").text(); 
                    var trackDuration = $(this).closest("tr").find("td:nth-child(4)").text(); 
                    
                    $("#trackId").val(trackId);
                    $("#trackName").val(trackName);
                    $("#trackCategory").val(trackCategory);
                    $("#trackDuration").val(trackDuration);
                    showModal();
                    AddOrEditTrack(trackId, trackName, trackCategory, trackDuration); 
                });
            }
        });
    });
     
    $("#add").click(function () 
    {
        showModal();
    });

    $('#saveButton').click(function() 
    {
        confirmSave(); 
    });

    $('#closeButton').click(function() 
    {
        console.log('close')
        $('#AddModal').modal('hide');  
    });

    $('#cancelSaveButton').click(function() 
    {
        $('#confirmModal').modal('hide');
    });

    $("#your-button-id").click(function () 
    {
        uploadFile(); 
    });
});

function refreshTrackList() 
{
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
                confirmDel();
                // var trackId = $(this).data("trackid");
                // deleteTrack(trackId);
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

function confirmSave() 
{
    $('#confirmModal').modal('show');
}

function confirmDel() 
{
    $('#confirmModalDel').modal('show');
}

function showModal() 
{
    console.log('show');
    $("#AddModal").modal("show");    
}

function confirmSave() 
{
    var trackId = $('#trackId').val();
    var trackName = $('#trackName').val();
    var trackCategory = $('#trackCategory').val();
    var trackDuration = $('#trackDuration').val();

    $('#confirmModal').modal('show').on('click', '#confirmSaveButton', function() 
    {
        $('#confirmModal').modal('hide');
        $('#AddModal').modal('hide');
        AddOrEditTrack(trackId, trackName, trackCategory, trackDuration);
        Null()
    });
}

function confirmDel(trackId) 
{
    $('#confirmModal').modal('show').on('click', '#confirmSaveButton', function() 
    {
        $('#confirmModal').modal('hide');
        console.log(trackId);
        deleteTrack(trackId);
    });
    
}

function AddOrEditTrack(trackId, name, category_id, duration) 
{
    requestData = 
    {
        id: trackId,
        name: name,
        category_id: category_id,
        duration: duration
    };

    $.ajax({
        type: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        url: "Music/AddOrEditMusic",
        data: JSON.stringify(requestData),
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

function uploadFile() 
{
    var formatSelect = document.getElementById('format-select');
    if (!formatSelect) 
    {
        console.error('Элемент с идентификатором "format-select" не найден.');
        return;
    }

    var categorySelect = document.getElementById('category');
    if (!categorySelect) 
    {
        console.error('Элемент с идентификатором "category" не найден.');
        return;
    }

    var selectedFormat = formatSelect.value;
    var selectedCategory = categorySelect.value;

    window.location.href = "music/DownloadFormatMusic/" + selectedFormat + "/" + selectedCategory;
}