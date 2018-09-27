$(function () {
    var getPostDataUrl = "api/SmsendLog/GetList";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname:"SendTime",
        colNames: ['序号', '手机号', '验证码', '发送时间', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'TelphoneNum', index: 'TelphoneNum', width: 100, align: "center" },
            { name: 'Code', index: 'Code', width: 60, align: "center" },
            { name: 'SendTime', index: 'SendTime', width: 100, align: "center" },
               { name: '', index: '', width: 250, align: "center" },
        ],
        multiselect:false,
        postData: topevery.form2Json("selectFrom")
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    var wait = 60;
    $("#sendSms").on("click", function () {
 
        var data = new Object();
        data.PhoneNumbers = $("#PhoneNumber").val();
        topevery.ajax({
            url: "api/SmsendLog/SendSmsInfo",
            data: JSON.stringify(data)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                layer.msg(info.Message.split('!')[0], {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 5000,
                    anim: 5
                });
                $("#FailureTime").val(info.Message.split('!')[1]);
            } else {
                layer.msg(info.Message, {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 5000,
                    anim: 5
                });
            }
           
        });
        time(this);
    })

    function time(event) {
        if (wait == 0) {
            $(event).removeAttr("disabled");
            $(event).val("免费获取验证码");
            wait = 60;
        } else {
            $(event).attr("disabled", "disabled");
            wait--;
            $(event).val(wait + "秒后可以重发");
            setTimeout(function () { time(event) }, 1000);
        }
    }


    //判断验证码是否失效
    $("#btnlogin").on("click", function () {
        var failureTime = $("#FailureTime").val();
        var start = new Date(failureTime.replace("-", "/").replace("-", "/"));
        var myDate = new Date();
        //获取当前年
        var year = myDate.getFullYear();
        //获取当前月
        var month = myDate.getMonth() + 1;
        //获取当前日
        var date = myDate.getDate();
        var h = myDate.getHours();       //获取当前小时数(0-23)
        var m = myDate.getMinutes();     //获取当前分钟数(0-59)
        var s = myDate.getSeconds();
        var now = year + '-' + p(month) + "-" + p(date) + " " + p(h) + ':' + p(m) + ":" + p(s);
        var end = new Date(now.replace("-", "/").replace("-", "/"));
        if (end > start) {
            
        } else {
            
        }
    })


});

function p(s) {
    return s < 10 ? '0' + s : s;
}
