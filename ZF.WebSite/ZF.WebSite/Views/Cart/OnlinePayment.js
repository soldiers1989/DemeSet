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
});

$(function () {
    var type = $("#OrderType").val();
    var obj = new Object();
    obj.PayType = type;
    //微信支付
    if (obj.PayType != 0) {
        obj.OrderNo = $("#OrderNo").val();
        obj.OrderAmount = $("#OrderAmount").val();
        obj.CardNo = $("#CardNo").val();
        topevery.ajax({
            url: "api/WxPay/GetPayUrl",
            data: JSON.stringify(obj)
        }, function (data) {
            var dataInfo = data.Result;
            if (data.Success) {
                obj = new Object();
                obj.PayType = type;
                obj.TwoCode = dataInfo.Message
            }
            $("#PayHtml").html(template("orderList_html", obj));
        });
    } else { //支付宝支付
        $("#PayHtml").html(template("orderList_html", obj));
    }
    $("#SheetNO").html($("#OrderNo").val());
    $("#price").html($("#OrderAmount").val());
    type = type == 0 ? "支付宝" : "微信";
    $("#payType").html(type);
    if (type == "支付宝") {
        aliPayment();
    }
  
})


function aliPayment() {
    var obj = new Object();
    //订单编号
    obj.OrderNo = $("#OrderNo").val();
    //金额
    obj.OrderAmount = $("#OrderAmount").val();
    //取得支付宝平台地址
    $("#IsPay").show()
    $("#aliPay").hide();
    // $.StandardPost('OnlinePaymentAli', obj);
    var url = 'OnlinePaymentAli?OrderNo=' + encodeURI(obj.OrderNo) + "&OrderAmount=" + encodeURI(obj.OrderAmount);

    openUrl(url);

    //var tempWindow = window.open('', '_blank', ''); //打开一个新的空白窗口
    //tempWindow.location.href = url;  //对新打开的页面进行重定向
    //$.ajax({
    //    type: "get",
    //    async:false,
    //    success: function () {
    //        //1展示悬浮窗
    //        //2打开新窗口
    //        window.location.href = url

    //    }
    //})


}


function openUrl(url) {
    var a = $('<a href="' + url + '" target="_blank"></a>')[0];
    var e = document.createEvent('MouseEvents');
    e.initEvent('click', true, true);
    a.dispatchEvent(e);
}