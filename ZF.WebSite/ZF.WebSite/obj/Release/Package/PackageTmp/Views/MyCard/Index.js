$(function () {

    GetCard(false);
    
    ///根据状态获取学习卡 1：未使用 2:已使用 3:已过期
    function GetCard(state) {
        var contentHtml = "";
        var IfUse = 0//是否使用
        var ifExpair = 0;//是否过期
        var url = "";
        if (state=="1") {
            IfUse = 0;
        } else if(state=="2") {
            IfUse = 1;
        } else if (state == "3") {
            IfUse = 0;
            ifExpair = 1;
        }
        topevery.ajax({
            url: 'api/UserDiscountCard/GetList',
            data: JSON.stringify({IfUse:IfUse,IfExpair:ifExpair,Type:0})
        }, function (data) {
            if (data.Success ) {
                $("#bindCardInfo").html(template("card_html", data.Result))
            } 
        });
    }

    /////领取学习卡
    //$('#btn_getCard').click(function () {
    //    topevery.ajax({
    //        url: "api/UserDiscountCard/IfExistAndExpair?CardId="+$.trim($('#cardCode').val()),
    //    }, function (data) {
    //        if (data.Result&&data.Result.Success) {
    //            AddCard();
    //        } else {
    //            layer.msg(data.Result.Message);
    //            $("#cardCode").focus();
    //        }
    //    });
    //});


    //function AddCard() {
    //    topevery.ajax({
    //        url: "api/UserDiscountCard/AddCard",
    //        data: JSON.stringify({ CardId: $.trim($('#cardCode').val())})
    //    }, function (data) {
    //        if (data.Success&&data.Result) {
    //            //显示添加的学习卡
    //            GetCard(false);
    //            layer.msg(data.Result.Message);
    //        }
    //    })
    //}

    //$('.j-tab').click(function () {
    //    var value = $(this).attr("data");
    //    $('.j-tab').each(function () {
    //        $(this).removeClass('current');
    //    });
    //    $(this).addClass("current");

    //    //if (value == "1") {
    //    //    //可用学习卡
    //    //    GetCard(false)  
    //    //} else if (value == "2") {
    //    //    //已使用学习卡
    //    //    GetCard(true);
    //    //} else if (value == "3") {
    //    //    //已过期学习卡
    //    //    GetCard();
    //    //}
    //    GetCard(value);
    //})
})

//获取 URL参数
function GetUrlParam(paramName) {
    var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
    return sValue ? sValue[1] : sValue;
}
