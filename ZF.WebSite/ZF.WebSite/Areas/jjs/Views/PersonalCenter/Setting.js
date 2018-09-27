topevery.ajaxwx({
    type: "GET",
    url: "api/Home/GetUserInfoByTicket"
}, function (data) {
    if (data.Success) {
        var object = new Object();
        object.NickNamw = data.Result.NickNamw;
        object.HeadImage = data.Result.HeadImage;
        $("#LoginIndex").html(template("question_common1", object));
    }
});