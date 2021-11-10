$(function () {
    presearch = null
    $('#livesearchtextbox').keyup(function () {
        let textboxsearch = $('#livesearchtextbox')
        let value = textboxsearch.val()
        let url = textboxsearch.data('request-url')
        if (value != presearch) {
            LiveSearch(value, url)
            presearch = value
        }
    });

    var elements = $('.form-control')
    elements.each(function () {
        $(this).change(function () {
            let url = $('#livesearchtextbox').data('request-url')
            let value = $('#livesearchtextbox').val()
            LiveSearch(value, url)
        });
    });
});

function LiveSearch(value, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            search: value,
            type: $("#typeselect :selected").text(),
            color: $("#colorselect :selected").text(),
            gender: $("#genderselect :selected").text(),
            brand: $("#brandselect :selected").text()
        },
        datatype: "html",
        success: function (data) {
            // Insert the returned search results html into the result element
            $('#con').empty();
            $('#con').html(data);
        }
    });
}