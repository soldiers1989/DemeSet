$(function () {
    if (!topevery.GetCookie("SubjectId")) {
        location.href = "/jjs/Subject/Index";
    }
    //绑定滚动图片
    topevery.ajaxwx({
        url: "api/BaseDanye/GetSlideSettingList",
        data: JSON.stringify({}),
        async: false
    }, function (data) {
        if (data.Success) {
            $("#fsD1").html(template("fsD1_html", data));
            if (topevery.IsValueAddedWebApp) {
                $("#AppLinkAddress").attr("href", "#");
                $("#AppLinkAddress1").attr("href", "#");
                $(".index-nav dl").css("width", "33%");
                $(".zzfw").show();
            } else {
                $("#zzfw").show();
                $(".index-mstd,.index-tjkc,.line").show();
                $(".zzfw").hide();
                $(".line1").hide();
            }
            Qfast.add('widgets', { path: "/Areas/jjs/resources/js/top/terminator2.2.min.js", type: "js", requires: ['fx'] });
            Qfast(false, 'widgets', function () {
                K.tabs({
                    id: 'fsD1',   //焦点图包裹id
                    conId: "D1pic1",  //** 大图域包裹id
                    tabId: "D1fBt",
                    tabTn: "a",
                    conCn: '.fcon', //** 大图域配置class
                    auto: 1,   //自动播放 1或0
                    effect: 'fade',   //效果配置
                    eType: 'click', //** 鼠标事件
                    pageBt: true,//是否有按钮切换页码
                    bns: ['.prev', '.next'],//** 前后按钮配置class
                    interval: 2000  //** 停顿时间
                })
            })
        }
    });
})
var Rows1 = 4;
var CourseType = 1;
$(".ulli >li").click(function () {
    CourseType = $(this).attr("CourseType");
    $(".ulli >li").removeClass("active");
    $(this).addClass("active");
    CourseInfoAll();
});
CourseInfoAll(
    function () {
        if (!$("#PopularCourses").html()) {
            $(".ulli >li")[2].click();
        }
    });

function CourseInfoAll(callback) {
    //绑定畅销好课
    topevery.ajaxwx({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ IsFree: 5, Page: 1, Rows: Rows1, IsRecommend: 1, SubjectId: topevery.GetCookie("SubjectId"), CourseType: CourseType, Type: 0, IsValueAdded: 0 })
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses").html(template("PopularCourses_html", data.Result));
            if (callback) {
                callback();
            }
        }
    });
}

CourseInfoAll1();
function CourseInfoAll1() {
    //绑定增值服务
    topevery.ajaxwx({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ IsFree: 5, Page: 1, Rows: Rows1, IsRecommend: 1, SubjectId: topevery.GetCookie("SubjectId"), CourseType: 0, Type: 0, IsValueAdded: 1 })
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses1").html(template("PopularCourses1_html", data.Result));
            //if (data.Result.Rows.length > 0) {
            //    $(".zzfw").show();
            //}
        }
    });
}



var Rows = 3;
//AfficheHelp();
function AfficheHelp() {
    //绑定资讯
    topevery.ajaxwx({
        url: "api/BaseDanye/GetAfficheHelp",
        data: JSON.stringify({ IsIndex: 0, Type: 0, Page: 1, Rows: Rows })
    }, function (data) {
        if (data.Success) {
            var Result = data.Result;
            $("#Affiche").html(template("Affiche_html", Result));
        }
    });
}

//topevery.ajaxToThis({
//    type: "Post",
//    url: "/jjs/PersonalCenter/GetJsapiTicketPublic",
//    data: JSON.stringify({ "findUrl": window.location.href }),
//    dataType: "html"
//}, function (data) {
//    var row = JSON.parse(data);
//    wx.config({
//        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
//        appId: row.appId, // 必填，公众号的唯一标识
//        timestamp: parseInt(row.timestamp), // 必填，生成签名的时间戳
//        nonceStr: row.nonceStr, // 必填，生成签名的随机串
//        signature: row.signature, // 必填，签名，见附录1
//        jsApiList: [
//     'checkJsApi',
//     'onMenuShareTimeline',
//     'onMenuShareAppMessage',
//     'onMenuShareQQ',
//     'onMenuShareWeibo',
//     'onMenuShareQZone',
//     'hideMenuItems',
//     'showMenuItems',
//     'hideAllNonBaseMenuItem',
//     'showAllNonBaseMenuItem',
//     'translateVoice',
//     'startRecord',
//     'stopRecord',
//     'onVoiceRecordEnd',
//     'playVoice',
//     'onVoicePlayEnd',
//     'pauseVoice',
//     'stopVoice',
//     'uploadVoice',
//     'downloadVoice',
//     'chooseImage',
//     'previewImage',
//     'uploadImage',
//     'downloadImage',
//     'getNetworkType',
//     'openLocation',
//     'getLocation',
//     'hideOptionMenu',
//     'showOptionMenu',
//     'closeWindow',
//     'scanQRCode',
//     'chooseWXPay',
//     'openProductSpecificView',
//     'addCard',
//     'chooseCard',
//     'openCard'
//        ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
//    });
//});

//document.querySelector('#sys').onclick = function () {
//    wx.scanQRCode({
//        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
//        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
//        success: function (res) {
//            var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
//            var code = "";
//            try {
//                code = result.split("?code=")[1];
//                if (result.split("?code=")[0].indexOf("http://zgrskspx.class.com.cn") === -1) {
//                    layer.msg("扫描错误,请扫描正确的图书二维码!");
//                    return;
//                }
//            } catch (e) {
//                layer.msg("扫描错误,请扫描正确的图书二维码!");
//                return;
//            }
//            if (code) {
//                if (code.length == 15) {
//                    topevery.ajaxwx({
//                        url: "api/CourseAppraise/CheckCourseCode?code=" + code + "&IsValueAdded=" + topevery.IsValueAddedWebApp,
//                        async: false
//                    }, function (data) {
//                        if (data.Success) {
//                            var info = data.Result;
//                            if (info.Success) {
//                                window.location.href = "/jjs/ValueAdded/Index?Message=" + info.ModelId;
//                            } else {
//                                window.location.href = "/jjs/ValueAdded/Message?Message=" + info.Message + "&code=" + code;
//                            }
//                        }
//                    });
//                } else {
//                    location.href = result;
//                }
//            }
//        }
//    })
//};
//名师团队
GetTeacherList();
function GetTeacherList() {
    topevery.ajaxwx({
        url: 'api/CourseOnTeacher/GetList',
        data: JSON.stringify({ Page: 1, Rows: 6, IsFamous: 1 })
    }, function (data) {
        if (data.Success) {
            var Result = data.Result;
            for (var i = 0; i < Result.Rows.length; i++) {
                Result.Rows[i].Synopsis = topevery.delHtmlTag(Result.Rows[i].Synopsis);
            }
            $('#scrollDiv').html(template('teacherinfo_html', data.Result));
            $("#scrollDiv").Scroll({ line: 1, speed: 500, timer: 7000, up: "but_up", down: "but_down" });
        }
    })
}