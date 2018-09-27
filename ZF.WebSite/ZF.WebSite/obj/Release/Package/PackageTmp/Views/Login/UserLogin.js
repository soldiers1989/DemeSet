var loginType = 0;
$(function () {
    //获取微信扫码二维码
    getWikiLoginCode();

    $(".u-loginbtn").on("click", function () {
        if (!inputIsValid($("#tel").val())) {
            showError(1);
            return
        }
        //登录
        var obj = new Object();
        obj.TelphoneNum = $("#tel").val();
        if (loginType == 0) {
            if (!inputIsValid($("#pwd").val())) {
                showError(2);
                return
            }
            obj.LoginPwd = md5($("#pwd").val());
            obj.PcLoginType = 0;
        } else if (loginType == 1) {
            if (!inputIsValid($("#txtphoneyzm").val())) {
                showError(4);
                return
            }
            obj.Code = $("#txtphoneyzm").val();
            obj.PcLoginType = 1;
            obj.RegiesterType = "web";
            obj.RegisterIp = $("#hiUserIp").val();
        }
        obj.LoginType = 0;
        topevery.ajax({
            url: "api/Account/Login",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (!info.Success) {
                showError(3, info.Message);
                $("#LoginPwd").val("");
            } else {
                topevery.SetCookie("userToken", info.Ticket);
                if (topevery.GetCookie("userToken")) {
                    topevery.ajax({
                        url: "api/MyCollection/UpdateSubject?subjectId=" + topevery.GetCookie("subjectId")
                    });
                }
                $(".layui-layer-close").click();
                if (topevery.getQueryString("RefUrl")) {
                    parent.location.href = topevery.getQueryString("RefUrl");
                } else {
                    parent.location.href = "/Home/Index";
                }
            }
        });
    });

    //注册
    $("#changepage").on("click", function () {
        topevery.ajaxToThis({ type: "get", url: "/Login/Regist", dataType: "html" }, function (data) {
            index = parent.layer.open({
                type: 1,
                title: "用户注册",
                shadeClose: false,
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '660px'], //宽高
                content: data
            });
        }, true);

    });

    //忘记密码
    $(".forgetpwd").on("click", function () {
        topevery.ajaxToThis({ type: "get", url: "/Login/ForgetPassword", dataType: "html" }, function (data) {
            index = parent.layer.open({
                type: 1,
                title: "找回密码",
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '600px'], //宽高
                content: data
            });
        }, true);
    });

    //密码登录
    $("#pwd_login").on("click", function () {
        loginType = 0;
        $(".m-tel-login").show();
        $(".sm-login").hide();
        $(this).addClass("z-sel").siblings().removeClass("z-sel");
        $(".code").hide();
        $("#div_pwd").show();
      
    });

    //手机验证码登录
    $("#yzm_login").on("click", function () {
        loginType = 1;
        $(".m-tel-login").show();
        $(".sm-login").hide();
        $(this).addClass("z-sel").siblings().removeClass("z-sel");
        $("#div_pwd").hide();
        $(".code").show();
    });
    //扫码登录
    $("#sm_login").on("click", function () {
        $(this).addClass("z-sel").siblings().removeClass("z-sel");
        $(".m-tel-login").hide();
        $(".sm-login").show();
    });
    var wait = 30;
    //获取验证码
    $("#btngetyzm").on("click", function () {
        if (!inputIsValid($("#tel").val())) {
            showError(1);
            return
        }
        showError(99);
        //获取短信验证码
        var args = new Object();
        args.PhoneNumbers = $("#tel").val();
        topevery.ajax({
            url: "api/Account/SendSmsInfo",
            data: JSON.stringify(args)
        }, function (data) {
            var into = data.Result;
            if (!into.Success) {
                layer.alert("验证码发送失败");
            }
        });
        time(this);
    });

    function time(event) {
        if (wait == 0) {
            $(event).removeAttr("disabled");
            $(event).css({ "background-color": "#f32426" });
            $(event).val("重新获取校验码");
            wait = 30;
        } else {
            $(event).attr("disabled", "disabled");
            $(event).css({ "background-color": "" });
            wait--;
            $(event).val(wait + "秒后重新获取");
            setTimeout(function () { time(event) }, 1000);
        }
    }
})

function getWikiLoginCode() {
    topevery.ajax({
        url: "api/Account/CreateWikiTwoDimensionalCode"
    }, function (data) {
        var into = data.Result;
        if (!into.Success) {
        } else {
            $("#wikiLoginImage").attr("src", into.Message);
        }
    });
}

function showError(type,msg) {
    switch (type) {
        case 1:
            $("#div_phone").addClass("err");
            $(".m-error-box").show();
            $(".m-error-box").css({ "display": "inline-flex" });
            $(".ferrorhead").html("手机号不能为空");
            break;
        case 2:
            $("#div_pwd").addClass("err");
            $(".m-error-box").show();
            $(".m-error-box").css({ "display": "inline-flex" });
            $(".ferrorhead").html("密码不能为空");
            break;
        case 3:
            $(".m-error-box").show();
            $(".m-error-box").css({ "display": "inline-flex" });
            $(".ferrorhead").html(msg);
            break;
        case 4:
            $("#div_yz").addClass("err");
            $(".m-error-box").show();
            $(".m-error-box").css({ "display": "inline-flex" });
            $(".ferrorhead").html("验证码不能为空");
            break;
        default:
            $("#div_phone").removeClass("err");
            $("#div_pwd").removeClass("err");
            $("#div_yz").removeClass("err");
            $(".m-error-box").css({ "display": "" });
            $(".m-error-box").hide();
            $(".ferrorhead").html("");
            break;
    }
  
}

function inputIsValid(inputval) {
    if (!inputval) {
        return false;
    } else {
        return true;
    }
}