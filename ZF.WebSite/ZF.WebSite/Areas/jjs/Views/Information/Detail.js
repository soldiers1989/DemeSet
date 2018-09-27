$(function () {
    var id = topevery.getQueryString("Id");
    //绑定资讯详情
    topevery.ajax({
        url: "api/BaseDanye/GetAfficheHelpView",
        data: JSON.stringify({ Id: id })
    }, function (data) {
        if (data.Success) {
            document.getElementsByTagName('title')[0].innerText = data.Result.Title;
            $("#newsDetails").html(template("newsDetails_html", data.Result));
            
        }
    });
})