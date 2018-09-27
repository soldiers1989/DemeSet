
var tDeliveryIsEndit = false;
$(document).ready(function () {
   
    var hand = topevery.getUrlParam("type");
    //从添加地址页过来
    if (hand && hand == "hand") {
        var hiparament = topevery.getUrlParam("hiparament");
        var wxJsApiParam = topevery.getUrlParam("wxJsApiParam");
        $("#hiparament").val(decodeURI(hiparament));
        $("#wxJsApiParam").val(decodeURI(wxJsApiParam));
    }
    payInfo();
    //是否开发票
    $("#openfp").on("click", function () {
        if ($(this).prop("checked")) {
            $(".container").show();
        } else {
            $(".container").hide();
        }
    })
});

//初始化信息
function payInfo() {
    var parament = $("#hiparament").val();
    topevery.ajaxwx({
        url: "api/Sheet/SheetAddToList",
        data: parament
    }, function (data) {
        if (data.Success) {
            $("#datas").html(template("datainfo", data));
            var amount, orderno, emailnotes, handeout, deliveryaddressid;
            $.each(data.Result, function (i, item) {
                amount = item.OrderAmount;
                orderno = item.OrderNo;
                //是否邮寄讲义
                if (item.EmailNotes === 1) {
                    emailnotes = true;
                    if (!handeout) {
                        handeout = item.CourseName;
                    } else {
                        handeout += "," + item.CourseName;
                    }
                }
            });
            if (emailnotes) {
                $("#pathshow").show();
                $("#pathshow").html("<p>课程需要邮寄讲义,请选择需邮寄的地址<a href=\"javascript:;\" onclick=\"alertAddAddressDiag()\" >+新增地址</a></p> ");
                getDeliveryAddress();
            }
            $("#amount").html("应付总额：¥" + amount);
            $("#amount").attr("name", amount);
            $("#hiOrderNO").val(orderno);
        }
    });
 
}
function getDeliveryAddress() {
    topevery.ajaxwx({
        url: "api/DeliveryAddress/GetList",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            if (data.Result.length > 0) {
                $.each(data.Result, function (i, item) {
                    if (item.DefaultAddress === 1) {
                        enditiSheetDeliveryAddRess(item.Id);
                    }
                });
            }
            $("#consignee-list").html(template("path_html", data.Result))
        }
    });
}

//添加修改样式
function enditiClass(event) {
    $("#consignee-list").find("div").each(function () {
        if ($(this).attr("class") == "jrzf-list07") {
            $(this).removeClass("jrzf-list07");
            $(this).addClass("jrzf-list06");
        }
    });
    $(event).attr("class", "jrzf-list07");
    var handoutId = $(event).attr("name");
    enditiSheetDeliveryAddRess(handoutId);
}

//修改送货地址
function enditiSheetDeliveryAddRess(id) {
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNO").val();
    obj.HandOutId = id;
    topevery.ajaxwx({
        url: "api/Sheet/EnditiSheetDeliveryAddRess",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            tDeliveryIsEndit = true;
        } else {
            parent.layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}


//支付
function OrderClick() {
    if (!$("#pathshow").is(":hidden") && !tDeliveryIsEndit) {
        layer.msg("请选择讲义收件地址！");
        return
    }

    var type;
    var ystyle;
    if ($("#zfbdiv").hasClass("active")) {
        type = 0;
        ystyle = 300;
    } else if ($("#wxdiv").hasClass("active")) {
        type = 1;
        ystyle = 500;
    }
    //保存用户电子发票信息
    if ($("#openfp").prop("checked")) {
        var obj = new Object();
        obj.OrderNo = $("#hiOrderNO").val();
        obj.InvoiceHeader = $("#txtInvoiceHeader").val();
        obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
        obj.InvoicePhone = $("#txtInvoicePhone").val();
        if (!obj.InvoiceMailbox || !obj.InvoicePhone) {
            layer.msg("邮箱和手机号不能为空！");
            return
        }

        if (!topevery.verifyPhone(obj.InvoicePhone)) {
            layer.msg("手机格式不正确！");
            return
        }
        if (!topevery.verifyEmail(obj.InvoiceMailbox)) {
            layer.msg("邮箱格式不正确！");
            return
        }

        //$("#consignee-lis").find("div").each(function () {
        //    if ($(this).attr("class") == "jrzf-list07") {
        //    }
        //});
        obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
        topevery.ajaxwx({
            url: "api/Sheet/EnditiSheetElectronicInvoice",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                //电子发票写入成功之后转到支付
                if (typeof WeixinJSBridge == "undefined") {
                    if (document.addEventListener) {
                        document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
                    } else if (document.attachEvent) {
                        document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
                        document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
                    }
                } else {
                    onBridgeReady();
                }

            } else {
                layer.msg(info.Message, { time: 1500, icon: 2 });
            }
        });
    } else {
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
            } else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
                document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
            }
        } else {
            onBridgeReady();
        }
    }
}

function onBridgeReady() {
    var wxJsApiParam = $("#wxJsApiParam").val();
    var obj = JSON.parse(wxJsApiParam);
    WeixinJSBridge.invoke(
        'getBrandWCPayRequest', {
            "appId": obj.appId,
            "timeStamp": obj.timeStamp,
            "nonceStr": obj.nonceStr,
            "package": obj.package,
            "signType": obj.signType,
            "paySign": obj.paySign
        },
     function (res) {
         if (res.err_msg == "get_brand_wcpay_request:ok") {
             //        // 支付成功后的回调函数
             //        //修改订单状态
             var parment = new Object();
             parment.OrderNo = $("#hiOrderNO").val();
             parment.OrderAmount = $("#amount").attr("name");
             parment.State = 0;
             parment.PayType = "648a1ab9-4575-49d9-bc29-0b5bb5e3006a";
             topevery.ajaxwx({
                 url: "api/Account/EnditSheetStates",
                 data: JSON.stringify(parment),
                 async:false
             }, function (data) {
                 var dataInfo = data.Result;
                 if (dataInfo.Success) {
                     layer.msg(dataInfo.Message);
                     window.location.href = "/jjs/MyCourse/Index";
                 } else {
                     layer.msg(dataInfo.Message);
                     payInfo();
                 }
             });

         }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
     }
    );
}



function alertAddAddressDiag() {
    var hiparament = $("#hiparament").val();
    var wxJsApiParam = $("#wxJsApiParam").val();
    window.location.href = "/jjs/PersonalCenter/MyHandOutPathEdit?hiparament=" + encodeURI(hiparament) + "&wxJsApiParam=" + encodeURI(wxJsApiParam);
}

$.extend({
    StandardPost: function (url, args) {
        var form = $("<form method='post'></form>"),
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



