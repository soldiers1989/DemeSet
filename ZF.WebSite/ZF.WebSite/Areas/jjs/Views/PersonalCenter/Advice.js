$(function () {
    var Code = topevery.getQueryString("Code");
    //绑定资讯详情
    topevery.ajax({
        url: "api/PublicInfo/GetContentInfo",
        data: JSON.stringify({ Code: Code })
    }, function (data) {
        if (data.Success) {
            if (Code === "gywm") {
                data.Result.Title = "关于我们";
            }
            if (Code === "lxwm") {
                data.Result.Title = "联系我们";
            }
            if (Code === "fltk") {
                data.Result.Title = "法律条款";
            }
            $("#newsDetails").html(template("newsDetails_html", data.Result));
        }
    });
})
