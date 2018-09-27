$(function () {
    var type = $("#OrderType").val();
    var obj = new Object();
    obj.PayType = type;
    $("#PayHtml").html(template("orderList_html", obj));

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
    window.open( "OnlinePaymentAli?OrderNo=" + obj.OrderNo + "&OrderAmount=" + obj.OrderAmount);
}