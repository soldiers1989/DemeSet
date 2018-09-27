$(function () {
    $(".fwm_anniu").on("click", function () {
        var code = $("#securityCode").val();
        if (!code) {
            parent.layer.msg("请输入防伪码", { time: 1500, icon: 2 });
            return;
        }
        topevery.ajax({
            url: "api/CourseAppraise/CheckCourseCode?code=" + code,
            async: false
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                $("#showmsg").html("成功领取增值服务，在增值服务中使用");
                $(".yanzheng").show();
                $(".fwm_box").hide();
                $(".yz_cuowu").hide();
            } else {
                if (info.Id === -1) {
                    $(".yanzheng").show();
                    $(".fwm_box").hide();
                    $(".yz_cuowu").hide();
                    $("#showmsg").html("该防伪码已经领取过增值服务，不能重复领取");
                } else {
                    $(".yanzheng").hide();
                    $(".fwm_box").hide();
                    $(".yz_cuowu").show();
                }
            }
        });
    })
})