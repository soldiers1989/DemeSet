function setCode() {
    var code = $("#securityCode").val();
    if (!code) {
        parent.layer.msg("请输入防伪码", { time: 1500, icon: 2 });
        return;
    }
    topevery.ajaxwx({
        url: "api/CourseAppraise/CheckCourseCode?code=" + code + "&IsValueAdded=" + topevery.IsValueAddedWebApp,
        async: false
    }, function (data) {
        if (data.Success) {
            var info = data.Result;
            if (info.Success) {
                window.location.href = "/jjs/ValueAdded/Index?Message=" + info.ModelId;
            } else {
                window.location.href = "/jjs/ValueAdded/Message?Message=" + info.Message + "&code=" + code;
            }
        }
    });
}