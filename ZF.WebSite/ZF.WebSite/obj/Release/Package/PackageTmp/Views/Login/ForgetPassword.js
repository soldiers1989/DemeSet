$(function () {
    $("#submitForget").bootstrapValidator({
        message: '输入的值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            phoneNum: {
                message: '用户名验证失败',
                validators: {
                    notEmpty: {
                        message: '手机号不能为空'
                    },
                    regexp: {
                        regexp: /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[5-9]{1})|(18[0-9]{1}))+\d{8})$/,
                        message: '请输入正确的手机号'
                    }, callback: {
                        callback: function (value, validator, $field) {
                            var istrue = true;
                            if (validator) {
                                var args = new Object();
                                args.TelphoneNum = value;
                                topevery.ajax({
                                    url: "api/Account/CheckIfMobileDuplicatedIsTrue",
                                    data: JSON.stringify(args),
                                    async: false
                                }, function (data) {
                                    istrue = data.Result;
                                });
                            }

                            if (istrue) {
                                return {
                                    valid: true
                                }
                            } else {
                                return {
                                    valid: false,
                                    message: '手机号码不存在'
                                }
                            }
                        }
                    }
                }
            },
            idcode: {
                validators: {
                    notEmpty: {
                        message: '校验码不能为空'
                    },
                    callback: {
                        callback: function (value, validator, $field) {
                            if ($("#phoneNum").val() == "") {
                                return {
                                    valid: false,       // or true
                                    message: '请输入手机号'
                                }
                            } else {
                                var istrue = true;
                                var bv = $("#submitForget").data('bootstrapValidator');
                                if (validator) {
                                    //验证短信验证码
                                    var args = new Object();
                                    args.Code = $("#idcode").val();
                                    args.TelphoneNum = $("#phoneNum").val();
                                    topevery.ajax({
                                        url: "api/Account/CheckRegisterMobileVerificationCode",
                                        data: JSON.stringify(args),
                                        async: false
                                    }, function (data) {
                                        istrue = data.Result;
                                    });
                                }
                                if (istrue) {
                                    return {
                                        valid: true
                                    }
                                } else {
                                    return {
                                        valid: false,       // or true
                                        message: '验证码错误'
                                    }
                                }
                            }
                        }
                    }
                }
            },
            password: {
                validators: {
                    notEmpty: {
                        message: '密码不能为空'
                    },
                    regexp: {
                        regexp: /^(?![^a-zA-Z]+$)(?!\D+$).{8,16}$/

                    },
                    identical: {
                        field: 'passwordConfirm',
                        message: '两次输入的密码不一致'
                    }
                }
            },
            passwordConfirm: {
                validators: {
                    notEmpty: {
                        message: '确认密码不能为空'
                    },
                    identical: {
                        field: 'password',
                        message: '两次输入的密码不一致'
                    }
                }
            }

        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var $form = $(e.target);
        var bv = $form.data('bootstrapValidator');
        if (bv.isValid()) {
            var info = topevery.serializeObject($("#submitForget"));
            var datainfo = new Object();
            datainfo.LoginPwd = md5(info.password);
            datainfo.TelphoneNum = info.phoneNum;

            topevery.ajax({
                url: "api/Account/ForgetPassword",
                data: JSON.stringify(datainfo)
            }, function (data) {
                var info = data.Result;
                if (!info.Success) {
                    parent.parent.layer.msg("重置失败", { time: 1000, icon: 1 });
                    bv.resetForm();
                } else {
                    setTimeout(function () {
                        parent.layer.msg("重置成功，请重新登录", { time: 1500, icon: 1 })
                    }, 200);
                    layer.closeAll('page')
                }
            });
        } else {
            parent.alyet.alert("req");
        }

    });
    var wait = 30;
    //获取验证码
    $("#loadingButton").on("click", function () {
        var bv = $("#submitForget").data('bootstrapValidator');
        if (bv.isValidField("phoneNum")) {
            //获取短信验证码
            var args = new Object();
            args.PhoneNumbers = $("#phoneNum").val();
            topevery.ajax({
                url: "api/Account/SendSmsInfo",
                data: JSON.stringify(args)
            }, function (data) {
                var into = data.Result;
                if (!into.Success) {
                    parent.layer.alert("验证码发送失败");
                }
            });
            time(this);

        }
    });

    function time(event) {
        if (wait == 0) {
            $(event).removeAttr("disabled");
            $(event).text("重新获取校验码");
            wait = 30;
        } else {
            $(event).attr("disabled", "disabled");
            wait--;
            $(event).text(wait + "秒后重新获取");
            setTimeout(function () { time(event) }, 1000);
        }
    }

})
