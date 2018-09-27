$(function () {
    GetUserInfo();
});

function saveUserPhoto() {
    $("#formid").ajaxSubmit({
        type: 'post',
        url: "/User/UserFliePhoto",
        dataType: "json",
        success: function (data) {
            if (data) {
                var obj = new Object();
                obj.Id = $("#hiuid").val();
                obj.HeadImage = data;
                obj.RegiesterType = "4";
                topevery.ajax({
                    url: "api/Account/ModifyUserPhone",
                    data: JSON.stringify(obj)
                }, function (data) {
                    var info = data.Result;
                    if (!info.Success) {
                        layer.msg(info.Message, { time: 1000, icon: 2 });
                        GetUserInfo();
                    } else {
                        layer.msg(info.Message, { time: 1000, icon: 1 });
                        GetUserInfo();
                        newSetUserInfo();
                    }
                });
            } else {
                layer.msg("修改失败", { time: 1000, icon: 2 });
                GetUserInfo();
            }
           
        }
    });
    return false;//阻止页面跳转
}

//获取验证码
function getPhoneCode(event) {
    var args = new Object();
    args.PhoneNumbers = $("#telphoneNum").val();
    if (!args.PhoneNumbers) {
        layer.msg("手机号不能为空", { time: 1000, icon: 2 });
        return;
    }
    topevery.ajax({
        url: "api/Account/SendSmsInfo",
        data: JSON.stringify(args)
    }, function (data) {
        var into = data.Result;
        if (!into.Success) {
            layer.alert("验证码发送失败");
        }
    });

    topevery.timeCode(event);
}
//绑定
function bindTel(event) {
    var Id = event;
    var passrowd = $("#telpassword").val();
    var obj = new Object();
    obj.Id = Id;
    obj.Code = $("#txtphoneyzm").val();
    obj.TelphoneNum = $("#telphoneNum").val();
    obj.RegiesterType = "3";
    if (!obj.Code || !obj.TelphoneNum ) {
        layer.msg("手机号,验证码以及登录密码不能为空");
        return;
    }
    else {
        if (passrowd) {
            if (!topevery.verifyPwdLength(passrowd)) {
                layer.msg("密码长度必须在8到16之间且包含数字和字母");
                return
            }
        }
        obj.LoginPwd = md5(passrowd);
        var istrue = false;
        //验证手机号是否存在
        topevery.ajax({
            url: "api/Account/ModifyUserPhoneCheck",
            data: JSON.stringify(obj),
            async:false
        }, function (data) {
            var info = data.Result;
            istrue = info;
            if (info) {
                layer.msg("手机号已存在", { time: 1000, icon: 2 });
            } 
        });
        if (istrue) {
            return;
        }
    }
    topevery.ajax({
        url: "api/Account/ModifyUserPhone",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (!info.Success) {
            layer.msg(info.Message, { time: 1000, icon: 2 });
        } else {
            layer.msg(info.Message, { time: 1000, icon: 1 });
            setTimeout(function () {
                topevery.SetCookie("userToken", info.ModelId);
                //topevery.DelCookie("userToken"); // 删除 cookie
                parent.location.href = "/Home/Index";
            }, 1000);
        }
    });
}

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

function nickEdit(editType,event) {
    switch (editType) {
        case 1:
            $(event).hide();
            $("#nickShowBox").hide();
            $("#nickEditBox").show();
            $("#nikeSave").show();
            break;
        case 2:
            $(event).hide();
            $("#emailShowBox").hide();
            $("#emailEditBox").show();
            $("#qqShowBox").hide();
            $("#qqEditBox").show();
            $("#infoSave").show();
            break;
        case 3:
            $(event).hide();
            $("#phoneShowBox").hide();
            $("#phoneEditBox").show();
            $("#savePhone").show();
            break;
    }
}

//保存
function infoSave(id, editType) {
    switch (editType) {
        case 1:
            var nike = $("#inputNick").val();
            if (!nike) {
                layer.msg("昵称不能为空", { time: 1000, icon: 2 });
                return;
            }
            saveNike(id, editType, nike);
            break;
        case 2:
            var qq = $("#txtQQ").val();
            var email = $("#txtEmail").val();
            saveQQandEmail(id, qq, email);
            break;
        case 3:
            $("#phoneShowBox").hide();
            $("#phoneEditBox").show();
            bindTel(id);
            break;
    }
}


function saveQQandEmail(id,qq,email) {
    var obj = new Object();
    obj.Id = id;
    obj.QQ = qq;
    obj.Email = email;
    obj.RegiesterType = "2";
    topevery.ajax({
        url: "api/Account/ModifyUserPhone",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (!info.Success) {
            layer.msg(info.Message, { time: 1000, icon: 2 });
        } else {
            layer.msg(info.Message, { time: 1000, icon: 1 });
            GetUserInfo();
            newSetUserInfo();
        }
    });
}

//保存昵称
function saveNike(id, ediTypem, nike) {
    var obj = new Object();
    obj.Id = id;
    obj.NickNamw = nike;
    obj.RegiesterType = "1";
    topevery.ajax({
        url: "api/Account/ModifyUserPhone",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (!info.Success) {
            layer.msg(info.Message, { time: 1000, icon: 2 });
        } else {
            layer.msg(info.Message, { time: 1000, icon: 1 });
            GetUserInfo();
            newSetUserInfo();
        }
    });
}


function newSetUserInfo() {
    var object = new Object();
    object.userToken = topevery.GetCookie("userToken");
    object.CartCount = 0;
    if (object.userToken && object.userToken !== "") {
        topevery.ajax({
            type: "GET",
            url: "api/Home/GetUserInfoByTicket"
        }, function (data) {
            if (data.Success) {
                object.NickNamw = data.Result.NickNamw;
                object.CartCount = data.Result.CartCount;
                object.LearnCount = data.Result.LearnCount;
                object.HeadImage = data.Result.HeadImage;
                //if (data.Result.SubjectId) {
                //    topevery.SetCookie("subjectId", data.Result.SubjectId);
                //    topevery.SetCookie("subjectName", data.Result.SubjectName);
                //}
                $("#LoginIndex").html(template("question_common1", object));
            }
        });
    } else {
        $("#LoginIndex").html(template("question_common1", object));
    }
}