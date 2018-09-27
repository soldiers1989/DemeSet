$("#Sumbit").click(function () {
    Login();
});

$("#LoginName,#PassWord").keydown(function (event) {
    if (event.keyCode === 13) {
        Login();
    }
});



function Login() {
    var data = topevery.serializeObject($("form"));
    
    console.log(navigator.userAgent);

    ///判断是手机登陆还是电脑登陆
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
       "SymbianOS", "Windows Phone",
       "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    if (flag) {
        data.LoginType = "PC";
    } else {
        data.LoginType = "Phone";
    }


    if (data.LoginName === "") {
        topevery.msg("用户名不能为空", 2);
        return;
    }
    if (data.PassWord === "") {
        topevery.msg("密码不能为空", 2);
        return;
    }



    var url = "LoginIn";
    data.PassWord = md5(data.PassWord);
    debugger;
    //data.PassWord = md5('123456');
    topevery.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        loading: false
    }, function (row) {
        if (row.Success) {
            topevery.msg(row.Message, 1);
            location.href = "/";
        } else {
            topevery.msg(row.Message, 2);
        }
    }, true);
}