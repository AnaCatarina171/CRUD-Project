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

$(".formDeleteElement").submit(function (e) {

    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            window.location.href = (data.redirectUrl);
            alert(data.message);
        }
    });
});

$(".formEditElement").submit(function (e) {

    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            window.location.href = (data.redirectUrl);
            alert(data.message);
        }
    });
});

function createPaging(page, top, totalItems, baseURL, searchTerm, cmbSearch) {

    var _HTMLPaging = [];

    var pages = parseInt(totalItems / top);

    if (totalItems % top != 0) {
        pages++;
    }

    var i = 0;
    for (i = 0; i < pages; i++) {
        var activeClass = '';
        if (page == (i + 1)) {
            activeClass = 'active';
        }

        _HTMLPaging.push('<li class="page-item ' + activeClass + '"><a class="page-link" href="' + baseURL
            + '?cmbSearch=' + cmbSearch + '&searchTerm=' + searchTerm
            + '&top=' + top + '&page=' + (i + 1).toString() + '">' + (i + 1).toString() + '</a ></li >');
    }

    $(".pagination").html(_HTMLPaging.join(''));
}

function isStillOpen() {
    dateClosed = document.getElementById("inputDateClosed");

    isOpen = document.getElementById("cboStillOpen").checked;

    if (isOpen) {

        dateClosed.value = "";
    }
}

/*function createInterval() {
    var x = setInterval(function () {
        createInterval();
    }, 5000);

    var isTrue = true;
    while (isTrue) {
        // do something

        //isTrue = false;
    }
}*/