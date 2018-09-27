topevery.ajaxwx({
    type: "GET",
    url: "api/Home/GetUserInfoByTicket"
}, function (data) {
    if (data.Success) {
        var object = new Object();
        object.NickNamw = data.Result.NickNamw;
        object.HeadImage = data.Result.HeadImage;
        object.QQ = data.Result.QQ;
        object.Email = data.Result.Email;
        object.TelphoneNum = data.Result.TelphoneNum;
        $("#LoginIndex").html(template("question_common1", object));
    }
});