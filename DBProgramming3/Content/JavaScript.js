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

/*$(".deleteLink").click(function (e) {

    e.preventDefault();

    var link = $(this);
    var actionUrl = link.attr('action');

    $.ajax({
        url: actionUrl,
        type: "DELETE",//type of posting the data
        success: function (data) {
            alert(data.message);
            location.reload();
        }
  });

});*/