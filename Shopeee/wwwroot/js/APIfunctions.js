function postToFacebook() {
    var facebookMessage = $('#facebookMessageInput').val();
    const FacebookPageId = "293533752446566";
    const FacebookPageToken = "EAAMs4FMOuNYBAN2QQEwiXRtFtVi0VgZBLvn9bUWGDmtDquKGUHduIQdspE4CEyeAYvWhrlxMrbkeCoCA5I5WLGlNvkcVLulRnZC5GxNOvnjhN2x93rQ5SZBrpq3tMhExMsiRh1EpNkNH0arZColiNBuJwKJk0qLNZB8nLZBZBTLVLrDiBhAA02O";
    const FacebookApi = "https://graph.facebook.com/";
    const postReqUrl = FacebookApi + FacebookPageId + "/feed?message=" + facebookMessage + "&access_token=" + FacebookPageToken;
    if (facebookMessage) {
        $.ajax({
            url: postReqUrl,
            type: "POST",
            success: function (data, textStatus, jqXHR) { },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus + " " + errorThrown);
            }
        });
    }
}

function getCurrentCurrencies() {
    // set endpoint and your API key
    endpoint = 'convert';
    access_key = 'b7c0512d350e1639e736b99a3f1c5503';

    // define from currency, to currency, and amount
    var from = 'ILS';
    var to = $('#to_currency').val();
    
    amount = '10';

    // execute the conversion using the "convert" endpoint:
    $.ajax({
        url: 'https://api.exchangeratesapi.io/v1/' + endpoint + '?access_key=' + access_key + '&from=' + from + '&to=' + to + '&amount=' + amount,
        dataType: 'jsonp',
        success: function (json) {

            // access the conversion result in json.result
            alert(json.result);

        }
    });
}