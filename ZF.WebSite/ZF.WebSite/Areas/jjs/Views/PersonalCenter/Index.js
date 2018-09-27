$(function () {
    setUserInfo();
    if (!topevery.IsValueAddedWebApp) {
        $("#wddd,#wdkj,#shdz,#zzfw,#user-nav").show();
    } else {
        $(".user-nav dl").css("width", "50%");
    }
})

function setUserInfo() {
    var object = new Object();
    object.userToken = topevery.GetCookie("userToken");
    object.CartCount = 0;
    if (object.userToken && object.userToken !== "") {
        topevery.ajaxwx({
            type: "GET",
            url: "api/Home/GetUserInfoByTicket"
        }, function (data) {
            if (data.Success) {
                object.NickNamw = data.Result.NickNamw;
                object.HeadImage = data.Result.HeadImage;
                object.SubjectName = data.Result.SubjectName;
                object.SubjectId = data.Result.SubjectId;
                $(".user-logobg").html(template("question_common1", object));
            }
        });
    } else {
        $("#LoginIndex").html(template("question_common1", object));
    }
}