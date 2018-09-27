topevery.ajaxToThis({
    type: "Post",
    url: "/jjs/PersonalCenter/GetJsapiTicketPublic",
    data: JSON.stringify({ "findUrl": window.location.href }),
    dataType: "html"
}, function (data) {
    var row = JSON.parse(data);
    wx.config({
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: row.appId, // 必填，公众号的唯一标识
        timestamp: parseInt(row.timestamp), // 必填，生成签名的时间戳
        nonceStr: row.nonceStr, // 必填，生成签名的随机串
        signature: row.signature, // 必填，签名，见附录1
        jsApiList: ['checkJsApi', 'scanQRCode'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
});

document.querySelector('#sys').onclick = function () {
    wx.scanQRCode({
        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
        success: function (res) {
            var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
            var code = "";
            try {
                code = result.split("?code=")[1];
                if (result.split("?code=")[0].indexOf("http://zgrskspx.class.com.cn") === -1) {
                    layer.msg("扫描错误,请扫描正确的图书二维码!");
                    return;
                }

            } catch (e) {
                layer.msg("扫描错误,请扫描正确的图书二维码!");
                return;
            }
            if (code) {
                if (code.length == 15) {
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
                } else {
                    location.href = result;
                }
            }
        }
    });
}