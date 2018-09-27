$(function () {
    var securityCode = topevery.getQueryString("Code");
    if (securityCode) {
        // 验证防伪码是否有效
        var obj = new Object();
        obj.Code = securityCode;
        topevery.ajaxwx({
            url: "api/CourseSecurityCode/VerifySecurityCode",
            data: JSON.stringify(obj)
        }, function (data) {
            if (data.Success) {
                if (data.Result.Id === 1) {
                    data.Result.TitleName = "人事社经济师增值服务";
                    data.Result.tp = "gz";
                } else if (data.Result.Id === 0) {
                    data.Result.TitleName = "人事社经济师课堂";
                    data.Result.tp = "zzfw";
                }
                $(".smts-box").html(template("security_html", data.Result))
            } else {
                $(".smts-box").html("服务错误");
            }
        });

        //var obj = new Object();
        //obj.Success = true;
        //obj.Message = securityCode;
        //$(".smts-font").html(template("security_html", obj))
    }
})