var loginType = 0;
$(function () {
    //获取微信扫码二维码
    //getWikiLoginCode();
    $(".u-loginbtn").on("click", function () {
        debugger;
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
        }
        //else if (loginType == 1) {
        //    if (!inputIsValid($("#txtphoneyzm").val())) {
        //        showError(4);
        //        return
        //    }
        //    obj.Code = $("#txtphoneyzm").val();
        //    obj.PcLoginType = 1;
        //}
        obj.LoginPwd = "f1efd26c7cdb6298e76d516b263762dd";
        obj.RegiesterType = "web";
        obj.RegisterIp = $("#hiUserIp").val();
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
                //if (topevery.GetCookie("userToken")) {
                //    topevery.ajax({
                //        url: "api/MyCollection/UpdateSubject?subjectId=" + topevery.GetCookie("SubjectId")
                //    });
                //}
                $(".layui-layer-close").click();
                var cookieCart = topevery.GetCookie("cartInfoList");
                if (cookieCart) {
                    //有加入购物车的情况下写入购物车
                    var obj = new Object()
                    obj.CourseId = cookieCart;
                    if (topevery.GetCookie("userToken")) {
                        topevery.ajax({
                            url: 'api/Cart/CartAdd',
                            data: JSON.stringify(obj),
                            async: false
                        }, function (data) {
                            if (data.Success) {
                                debugger
                                if (data.Result.Success) {
                                    topevery.DelCookie("cartInfoList"); // 删除 cookie
                                    parent.setNum();
                                    //购物车加入成功后跳转支付页面
                                    var isToOrder = topevery.GetCookie("cartToSubmitOrder");
                                    if (isToOrder) {
                                        if (data.Result.Message) {
                                            var obj = JSON.parse(isToOrder);
                                            obj.Id = data.Result.Message;
                                            obj.cookieSubmit = "yes";
                                            //$.StandardPost('/Cart/IframSubmitOrder', obj);
                                            parent.location.href = "/Cart/IframSubmitOrder?parment=" + encodeURIComponent(JSON.stringify(obj));
                                        } else {
                                            parent.location.href = "/Home/Index";
                                        }
                                        //window.location.href = "/Cart/IframSubmitOrder?parment=" + obj;
                                    } else {
                                        parent.location.href = "/Home/Index";
                                    }
                                }
                            } else {
                                parent.location.href = "/Home/Index";
                            }
                        });
                    }
                } else {
                    parent.location.href = "/Home/Index";
                }
            }
        });
    });

    $.extend({
        StandardPost: function (url, args) {
            var form = $("<form method='post' target='_blank'></form>"),
                input;
            $(document.body).append(form);
            //document.body.appendChild(form);
            form.attr({ "action": url });
            $.each(args, function (key, value) {
                input = $("<input type='hidden'>");
                input.attr({ "name": key });
                input.val(value);
                form.append(input);
            });
            form.submit();
        }
    })

    //注册
    $("#changepage").on("click", function () {
            index = parent.layer.open({
                type: 2,
                title: "用户注册",
                shadeClose: false,
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '660px'], //宽高
                content: "/Login/Regist"
            });
    });

    //忘记密码
    $(".forgetpwd").on("click", function () {
        index = parent.layer.open({
            type: 2,
            title: "找回密码",
            skin: 'layui-layer-rim', //加上边框
            area: ['600px', '600px'], //宽高
            content: "/Login/ForgetPassword"
        });
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
        var obj = new WxLogin({
            self_redirect: true,
            id: "twoCode",
            appid: "wx21f0f32ef6a82902",
            scope: "snsapi_login",
            redirect_uri: "http%3A%2F%2F205c8u6006.imwork.net%2FLogin%2FWikiLogin&response",
            state: "2"
        });
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