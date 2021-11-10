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

    presearchcity = null
    $('#citysearchtextbox').keyup(function () {
        let search = $('#livesearchtextbox').val()
        let city = $('#citysearchtextbox').val()
        let url = $('#livesearchtextbox').data('request-url')
        if (city != presearchcity) {
            LiveSearch(search, city, url)
            presearchcity = city
        }
    });

    var elements = $('.form-control')
    elements.each(function () {
        $(this).change(function () {
            let url = $('#livesearchtextbox').data('request-url')
            let value = $('#livesearchtextbox').val()
            let city = $('#citysearchtextbox').val()
            LiveSearch(value, city, url)
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

function LiveSearch(search, city, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            search: search,
            city: city,
            area: $("#areaselect :selected").text()
        },
        datatype: "html",
        success: function (data) {
            // Insert the returned search results html into the result element
            $('#con').empty()
            $('#con').html(data)
        }
    });
}