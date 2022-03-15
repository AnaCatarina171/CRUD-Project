$(".formAddElement").submit(function (e) {

    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            alert(data.message);
            location.reload();
        }
    });
});

$("#deleteLink").click(function (e) {

    e.preventDefault();
    $.ajax({
        url: "<where to post>",
        type: "DELETE",//type of posting the data
        data: "<what to post>",
        success: function (data) {
            alert("State successfuly deleted");
            location.reload();
        },
        error: function(xhr, ajaxOptions, thrownError){
            //what to do in error
        },
        timeout : 15000//timeout of the ajax call
  });

});