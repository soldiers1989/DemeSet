$(function () {
    GetUserInfo();

});


function GetUserInfo() {
    //获取用户信息
    topevery.ajax({
        url: "api/Register/GetOne"
    }, function (data) {
        if (data.Success) {
            $(".pd2").html(template("userInfo_html", data.Result));
        }
    });
}

function userEnditi(event) {
    var obj = new Object();
    obj.Id = $(event).attr("id");
    obj.TelphoneNum = $("#TelphoneNum").val();
    obj.NickNamw = $("#NickNanw").val();
    //更改用户信息
    topevery.ajax({
        url: "api/Register/UpdateOne",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            layer.msg(info.Message, { time: 1000, icon: 1 });
            GetUserInfo();
        } else {
            layer.msg(info.Message, { time: 1000, icon: 2 });
        }
    });
}