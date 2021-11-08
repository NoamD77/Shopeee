function postToFacebook() {
    var facebookMessage = document.getElementById("facebookMessageInput").value
    var postToFBRoute = "https://localhost:44319/ShoppingCarts/PostToFacebook"
        $.ajax({
            type: "POST",
            url: postToFBRoute,
            data: {
                facebookmassage: facebookMessage
            },
            datatype: "html",
            success: () => alert("Posted in Shopeee Page"),
            error: () => alert("Failed to post in Shopee Page")
        });
}

function ChangeRate() {
    var newRate = document.getElementById("toCurrencySelect").value
    var link = "https://localhost:44319/ShoppingCarts/ChangeRate"
    $.ajax({
        type: "POST",
        url: link,
        data: {
            new_Rate: newRate,
        },
        datatype: "html",
        success: () => {
            alert("Success")
            location.reload()
        },
        error: () => { alert("Failed") }
    });
}