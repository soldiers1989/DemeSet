
var index = 0;
$(function () {
    $("#sumbitForm").bootstrapValidator({
        message: '输入的值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            TelphoneNum: {
                message: '用户名验证失败',
                validators: {
                    notEmpty: {
                        message: '手机号不能为空'
                    }
                }
            },
            LoginPwd: {
                validators: {
                    notEmpty: {
                        message: '密码不能为空'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var $form = $(e.target);
        var bv = $form.data('bootstrapValidator');
        if (bv.isValid()) {
            var pwdVal = $("#LoginPwd").val();
            $("#LoginPwd").val(md5(pwdVal));
            var info = topevery.serializeObject($("#sumbitForm"));
            topevery.ajax({
                url: "api/Account/Login",
                data: JSON.stringify(info)
            }, function (data) {
                var info = data.Result;
                if (!info.Success) {
                    layer.msg(info.Message, {
                        icon: 2,
                        title: false, //不显示标题
                        offset: 'rb',
                        time: 3000, //10秒后自动关闭
                        anim: 2
                    });
                    $("#LoginPwd").val("");
                    bv.resetForm();
                } else {
                    ////先删后写
                    //$.cookie("userToken", null);
                    //var option = new Object();
                    //option.path = "/";
                    //topevery.cookieHelper("userToken", info.Ticket, option);
                    //var token = $.cookie("userToken");
                    topevery.SetCookie("userToken", info.Ticket);

                    if (topevery.getQueryString("RefUrl")) {
                        location.href = topevery.getQueryString("RefUrl");
                    } else {
                        location.href = "/Home/Index";
                    }
                }
            });
        }
    });
    //忘记密码
    $("#RetrievePassword").on("click", function () {
        //topevery.ajaxToThis({ type: "get", url: "ForgetPassword", dataType: "html" }, function (data) {
        //    index = layer.open({
        //        type: 1,
        //        title: "找回密码",
        //        skin: 'layui-layer-rim', //加上边框
        //        area: ['600px', '620px'], //宽高
        //        content: data,
        //        end: function () {
        //            location.reload();
        //        }
        //    });
        //}, true);
        layer.open({
            type: 2,
            title: '找回密码',
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: ['600px', '620px'],
            content: 'ForgetPassword',
            end: function () {
                layer.msg("操作成功，请重新登录", { time: 1000, icon: 1 });
                location.reload();
            }
        });
    })
});


function UserRegist() {
    layer.open({
        type: 2,
        title: '用户注册',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['600px', '530px'],
        content: 'UserLogin',
        end: function () {
            layer.msg("操作成功，请重新登录", { time: 1000, icon: 1 });
            location.reload();
        }
    });


}