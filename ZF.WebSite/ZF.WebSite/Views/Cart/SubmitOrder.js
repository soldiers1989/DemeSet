var hiTotalPrice = 0;
var cardList = "";
var arr = [];
//已选择学习卡
var selectedCard = [];
var userprice = 0.0;
var scrollFunc = function (e) {
    e = e || window.event;
    if (e.wheelDelta) {  //判断浏览器IE，谷歌滑轮事件               
        if (e.wheelDelta > 0) {
            //当滑轮向上滚动时执行的代码段  consignee-scrollbar
            $(".ui-scrollbar-item-consignee").css({ "top": "2px" });
            $(".consignee-scrollbar").css({ "top": "0px" });
        }
        if (e.wheelDelta < 0) {
            //当滑轮向下滚动时执行的代码段   
            $(".ui-scrollbar-item-consignee").css({ "top": "70px" });
            $(".consignee-scrollbar").css({ "top": "-126px" });
        }
    } else if (e.detail) {  //Firefox滑轮事件  
        if (e.detail < 0) {
            //当滑轮向上滚动时执行的代码段   
            $(".ui-scrollbar-item-consignee").css({ "top": "2px" });
            $(".consignee-scrollbar").css({ "top": "0px" });
        }
        if (e.detail > 0) {
            //当滑轮向下滚动时执行的代码段  
            $(".ui-scrollbar-item-consignee").css({ "top": "70px" });
            $(".consignee-scrollbar").css({ "top": "126px" });
        }
    }
}
var tDeliveryIsEndit = false;
$(function () {
    hiTotalPrice = 0;
    cardList = "";
    arr = [];
    selectedCard = [];

    if (topevery.GetCookie("cartToSubmitOrder")) {
        topevery.DelCookie("cartToSubmitOrder")
    }
    getOrderInfo();
    $("#zfbdiv").on("click", function () {
        $(this).addClass("active");
        $("#wxdiv").removeClass("active");
    });
    $("#wxdiv").on("click", function () {
        $(this).addClass("active");
        $("#zfbdiv").removeClass("active");
    });

    //是否邮寄讲义
    $("#inDiag").click(function () {
        if ($(this).prop("checked")) {
            $("#pathshow").show();
        } else {
            $("#pathshow").hide();
        }
    })
});

function getOrderInfo() {
    userprice = 0.0;
    hiTotalPrice = 0;
    cardList = "";
    arr = [];
    selectedCard = [];

    var parament = $("#hiparament").val();
    var paramentInfo = JSON.parse($("#hiparament").val());
    var toUrl = "api/Sheet/SheetAddToList";
    if (paramentInfo.cookieSubmit == "yes") {
        toUrl = "api/Sheet/SheetAddToListNoLogin";
    }

    topevery.ajax({
        url: toUrl,
        data: parament
    }, function (data) {
        if (data.Success) {
            $("#datas").html(template("datainfo", data));
            var orderno, emailnotes, handeout, deliveryaddressid;
            var favourable = 0.0;
            var amount = 0.0;
            $.each(data.Result, function (i, item) {
                favourable = favourable + item.FavourablePrice;
      
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
                GetDiscountCard(item.CourseId, orderno);
            });
            amount = favourable;
            if (emailnotes) {
                $("#handoutIsShow").show();
                $("#pathval").html("课程：" + handeout + "需要邮寄讲义,请选择需邮寄的地址");
                getDeliveryAddress();
            } else {
                $("#handoutIsShow").hide();
            }
            ShowDiscountCard(arr);

            $("#amount").attr("name", amount);
            userprice = amount;
            $("#orderno").text("订单号：" + orderno);
            $("#hiOrderNO").val(orderno);
            $(".CartCount").html($("#hicartcount").val());
            $("#hiAmount").val(amount);
            $('#discountCardArea').html(cardList);

            //重新计算价格
            setTimeout(function () {
                CalculationPrice();
                //计算购物车数量
                parent.setUserInfo();
            }, 200);
        }
    });
}

function OrderClick() {
    if (!$("#pathshow").is(":hidden") && !tDeliveryIsEndit && !$("#handoutIsShow").prop("checked")) {
        parent.layer.msg("请选择讲义收件地址！");
        return
    }
    var discourseCard = "";
    var arrUnique = selectedCard.unique();
    for (var i = 0; i < arrUnique.length; i++) {
        if (!discourseCard) {
            discourseCard = arrUnique[i];
        } else {
            discourseCard += "," + arrUnique[i];
        }
    }
    var obj = new Object();
    obj.DiscountCard = discourseCard;
    obj.OrderNo = $("#hiOrderNO").val();
    ////提交卡卷
    //topevery.ajax({
    //    url: 'api/Sheet/EnditDiscountCard',
    //    data: JSON.stringify(obj)
    //}, function (data) {
    //    if (data.Success) {
    //        if (data.Result.Success) {
    //            if ($("#invoice").prop("checked")) {
    //                invoiceIsTrue();
    //            } else {
    //                invoiceIsFalse();
    //            }
    //        } else {
    //            parent.layer.msg("学习卡异常！");
    //        }
    //    } else {
    //        parent.layer.msg("学习卡异常！");
    //    }
    //});
    if ($("#invoice").prop("checked")) {
        invoiceIsTrue();
    } else {
        invoiceIsFalse();
    }
}

//需要开发票
function invoiceIsTrue() {
    var type;
    var ystyle;
    if ($("#zfbdiv").hasClass("active")) {
        type = 0;
        ystyle = 300;
    } else if ($("#wxdiv").hasClass("active")) {
        type = 1;
        ystyle = 560;
    }
    //保存用户电子发票信息
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNO").val();
    obj.InvoiceHeader = $("#txtInvoiceHeader").val();
    obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
    obj.InvoicePhone = $("#txtInvoicePhone").val();
    if (!obj.InvoiceMailbox || !obj.InvoicePhone) {
        parent.layer.msg("邮箱和手机号不能为空！");
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

    var cardNo = "";
    if ($('input[name="checkbox3"]:checked').length > 0) {
        $('input[name="checkbox3"]:checked').each(function () {
            cardNo += $(this).attr("cardcode") + "," + $(this).attr("Count") + ";";
        });
    }
    obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
    topevery.ajax({
        url: "api/Sheet/EnditiSheetElectronicInvoice",
        data: JSON.stringify(obj),
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            //电子发票写入成功之后转到支付
            var url = "OnlinePayment" + "?OrderNo=" + $("#hiOrderNO").val() + "&OrderAmount=" + userprice + "&type=" + type + "&cardNo=" + cardNo;

            if (type == 0) {
                //var urls = 'OnlinePaymentAli?OrderNo=' + encodeURI($("#hiOrderNO").val());
                //window.location.href = urls;
                //订单编号
                var obj = new Object();
                //订单编号
                obj.CardNo = cardNo.trim(";");
                obj.OrderNo = $("#hiOrderNO").val();
                obj.OrderAmount = userprice;
                //取得支付宝平台地址
                topevery.ajax({
                    url: "api/AliPay/GetAliPayUrl",
                    data: JSON.stringify(obj)
                }, function (data) {
                    //$("#alyPlay").append(data.Result.response.Body)
                    var bdhtml = "<div id=\"alyPlay\" style=\"display:none\">" + data.Result.response.Body + "</div>";
                    $("#alpqy").html(bdhtml);
                    //document.write(bdhtml);
                });

            } else {
                parent.layer.open({
                    type: 2,
                    title: '微信支付',
                    shadeClose: false,
                    shade: [0.7, '#000'], //0.7透明度的白色
                    maxmin: true, //开启最大化最小化按钮
                    area: [400 + 'px', ystyle + 'px'],
                    content: url,
                    btn: ['支付成功', '返回'],
                    skin: 'demo-class',
                    yes: function (index, layero) {
                        parent.layer.close(index);
                        setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
                    },
                    btn2: function (index, layero) {
                        var obj = new Object();
                        obj.OrderNo = $("#hiOrderNO").val();
                        topevery.ajax({
                            url: "api/Sheet/OrderSheetIsPay",
                            data: JSON.stringify(obj)
                        }, function (data) {
                            var info = data.Result;
                            if (info.Success) {
                                parent.layer.close(index);
                                setTimeout(function () { getOrderInfo() }, 200);
                            } else {
                                parent.layer.close(index);
                                setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
                            }
                        });
                        return false;
                    }, end: function () {
                        var obj = new Object();
                        obj.OrderNo = $("#hiOrderNO").val();
                        topevery.ajax({
                            url: "api/Sheet/OrderSheetIsPay",
                            data: JSON.stringify(obj)
                        }, function (data) {
                            var info = data.Result;
                            if (info.Success) {
                                parent.layer.close();
                                setTimeout(function () { getOrderInfo() }, 200);
                            } else {
                                parent.layer.close();
                                setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
                            }
                        });
                    }
                });

            }
        } else {
            parent.layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}

//不需要开发票
function invoiceIsFalse() {
    debugger;
    var type;
    var ystyle;
    if ($("#zfbdiv").hasClass("active")) {
        type = 0;
        ystyle = 300;
    } else if ($("#wxdiv").hasClass("active")) {
        type = 1;
        ystyle = 560;
    }
    var cardNo = "";
    if ($('input[name="checkbox3"]:checked').length > 0) {
        $('input[name="checkbox3"]:checked').each(function () {
            cardNo += $(this).attr("cardcode") + "," + $(this).attr("Count") + ";";
        });
    }
    //电子发票写入成功之后转到支付
    var url = "OnlinePayment" + "?OrderNo=" + $("#hiOrderNO").val() + "&OrderAmount=" + userprice + "&type=" + type + "&cardNo=" + cardNo;
    if (type == 0) {
        debugger;
        //var urls = 'OnlinePaymentAli?OrderNo=' + encodeURI($("#hiOrderNO").val());
        //parent.window.location.href = urls;
        var obj = new Object();
        //订单编号
        obj.OrderNo = $("#hiOrderNO").val();
        obj.CardNo = cardNo.trim(";");
        obj.OrderAmount = userprice;
        //取得支付宝平台地址
        topevery.ajax({
            url: "api/AliPay/GetAliPayUrl",
            data: JSON.stringify(obj)
        }, function (data) {
            //$("#alyPlay").append(data.Result.response.Body)
            var bdhtml = "<div id=\"alyPlay\" style=\"display:none\">" + data.Result.response.Body + "</div>";
            $("#alpqy").html(bdhtml);
            //document.write(bdhtml);
        });
    } else {
        parent.layer.open({
            type: 2,
            title: '微信支付',
            shadeClose: false,
            shade: [0.7, '#000'], //0.7透明度的白色
            maxmin: true, //开启最大化最小化按钮
            area: [400 + 'px', ystyle + 'px'],
            content: url,
            btn: ['支付成功', '返回'],
            skin: 'demo-class',
            yes: function (index, layero) {
                parent.layer.close(index);
                setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
            },
            btn2: function (index, layero) {
                var obj = new Object();
                obj.OrderNo = $("#hiOrderNO").val();
                topevery.ajax({
                    url: "api/Sheet/OrderSheetIsPay",
                    data: JSON.stringify(obj)
                }, function (data) {
                    var info = data.Result;
                    if (info.Success) {
                        parent.layer.close(index);
                        setTimeout(function () { getOrderInfo() }, 200);
                    } else {
                        parent.layer.close(index);
                        setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
                    }
                });
                return false;
            }, end: function () {
                var obj = new Object();
                obj.OrderNo = $("#hiOrderNO").val();
                topevery.ajax({
                    url: "api/Sheet/OrderSheetIsPay",
                    data: JSON.stringify(obj)
                }, function (data) {
                    var info = data.Result;
                    if (info.Success) {
                        parent.layer.close();
                        setTimeout(function () { getOrderInfo() }, 200);
                    } else {
                        parent.layer.close();
                        setTimeout(function () { window.parent.location.href = "/LearningCenter/Index" }, 200);
                    }
                });
            }
        });

    }

}


//获取最近一次电子发票信息
function getElectronicInvoiceInfo() {
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNO").val();
    if ($("#electronicInvoiceInfo").attr("class") == "collapse") {
        topevery.ajax({
            url: "api/AliPay/GetElectronicInvoiceByUserIds"
        }, function (data) {
            if (data.Result != null) {
                var info = data.Result;
                $("#txtInvoiceHeader").val(info.InvoiceHeader);
                $("#txtInvoiceMailbox").val(info.InvoiceMailbox);
                $("#txtTaxpayerIdentificationNumber").val(info.TaxpayerIdentificationNumber);
                $("#txtInvoicePhone").val(info.InvoicePhone);
            }
        });
    }
}

function getDeliveryAddress() {
    topevery.ajax({
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
            if (data.Result.length > 4) {
                //给页面绑定滑轮滚动事件  Firefox
                if ($("#overflowShow").addEventListener) {//firefox  
                    $("#overflowShow").addEventListener('DOMMouseScroll', scrollFunc, false);
                }
                //滚动滑轮触发scrollFunc方法 ie 谷歌  
                window.onmousewheel = $("#overflowShow").onmousewheel = scrollFunc;
                $(".ui-scrollbar-bg").show();
                $(".ui-scrollbar-item-consignee").show();
            } else {
                $(".ui-scrollbar-bg").hide();
                $(".ui-scrollbar-item-consignee").hide();
            }
            $("#consignee-list").html(template("path_html", data.Result))
        }
    });
}
//删除地址
function alertDelAddressDiag(event) {
    var obj = new Object();
    obj.Id = $(event).attr("id");
    parent.layer.confirm("确定删除吗?", {
        title: "删除确认"
    }, function (index) {
        topevery.ajax({
            url: "api/DeliveryAddress/DelDeliveryAddress",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                parent.layer.msg(info.Message, {
                    time: 1500, icon: 1
                });
                getDeliveryAddress();
            } else {
                parent.layer.msg(info.Message, {
                    time: 1500, icon: 2
                })
            }

        });
    });
}
//修改默认地址
function makeAddressAllDefaultByoverseas(event) {
    var obj = new Object();
    obj.Id = $(event).attr("name");
    topevery.ajax({
        url: "api/DeliveryAddress/EnditiDefaultAddress",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            parent.layer.msg(info.Message, {
                time: 1500, icon: 1
            });
            enditiSheetDeliveryAddRess($(event).attr("name"));
            getDeliveryAddress();
        } else {
            parent.layer.msg(info.Message, {
                time: 1500, icon: 2
            })
        }

    });
}
//编辑
function alertUpdateAddressDiagByoverseas(event) {
    parent.layer.open({
        type: 2,
        title: '地址新增编辑',
        shadeClose: true,
        maxmin: false, //开启最大化最小化按钮
        area: ['600px', '500px'],
        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
        content: 'HandOutPathAddOrEnditi?id=' + $(event).attr("name"),
        end: function () {
            getDeliveryAddress();
        }
    });

}
function alertAddAddressDiag() {
    parent.layer.open({
        type: 2,
        title: '地址新增编辑',
        shadeClose: true,
        maxmin: false, //开启最大化最小化按钮
        area: ['600px', '500px'],
        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
        content: 'HandOutPathAddOrEnditi',
        end: function () {
            getDeliveryAddress();
        }
    });
}
//添加修改样式
function enditiClass(event) {
    $(".consignee-item").removeClass("item-selected");
    $(event).addClass("item-selected");
    var handoutId = $(event).attr("name");
    enditiSheetDeliveryAddRess(handoutId);
}

//修改送货地址
function enditiSheetDeliveryAddRess(id) {
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNO").val();
    obj.HandOutId = id;
    topevery.ajax({
        url: "api/Sheet/EnditiSheetDeliveryAddRess",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            tDeliveryIsEndit = true;
        } else {
            parent.layer.msg(info.Message, {
                time: 1500, icon: 2
            });
        }
    });
}

$("#invoice").change(function () {
    if ($(this).is(':checked')) {
        $("#electronicInvoiceInfo").show();
    } else {
        $("#electronicInvoiceInfo").hide();
    }
});

//function personal(type) {
//    debugger
//    switch (type) {
//        case 1:
//            $("#divpersonal").hide();
//            break;
//        case 2:
//            $("#divpersonal").show();
//            break;
//    }
//}

//以下为学习卡以及满减活动流程
///获取可用学习卡s

function GetDiscountCard(courseid, orderNo) {
    arr = [];
    topevery.ajax({
        url: 'api/UserDiscountCard/GetUseCard?courseId=' + courseid + "&orderNo=" + orderNo,
        async: false
    }, function (data) {
        if (data.Success && data.Result) {
            var list = data.Result;
            for (var i = 0; i < list.length; i++) {
                arr.push(list[i])
            }
        }
    })
}
function ShowDiscountCard(arr) {
    var uniqueArr = arr.unique();
    for (var j = 0; j < uniqueArr.length; j++) {
        //cardList += "<input  checked=\"checked\" type=\"checkbox\" data=\"" + uniqueArr[j].Amount + "\" onclick=\"checkCard(this)\" cardCode=\"" + uniqueArr[j].CardCode + "\">" + uniqueArr[j].CardName + "：<span style=\"min-width:0px;\">" + uniqueArr[j].Amount + "元</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        cardList += " <dt><input type=\"checkbox\" checked=\"checked\" data=\"" + uniqueArr[j].Amount + "\" onclick=\"checkCard(this)\" Count=\"" + uniqueArr[j].Count + "\" cardCode=\"" + uniqueArr[j].CardCode + "\" name=\"checkbox3\"> " + uniqueArr[j].CardName + "：</dt> <dd>" + uniqueArr[j].Amount + " 元</dd> <div class=\"clear\"></div> ";
    }
}

Array.prototype.unique = function () {
    var res = [];
    var json = {
    };
    for (var i = 0; i < this.length; i++) {
        if (typeof this[i] == 'object') {
            if (!json[JSON.stringify(this[i])]) {
                res.push(this[i]);
                json[JSON.stringify(this[i])] = 1;
            }
        } else {
            if (!json[this[i]]) {
                res.push(this[i]);
                json[this[i]] = 1;
            }
        }
    }
    return res;
}


function checkCard(event) {
    CalculationPrice();
}


var discountNum = 0.0;
//计算金额
function CalculationPrice() {
    var priceSum = 0.0;
    var orderSum = 0.0;
    var checkedIsTrue = false;
    selectedCard = [];
    priceSum = parseFloat($("#hiAmount").val()).toFixed(2);
    orderSum = priceSum;

    GetBestDiscountNum(priceSum);

    //选择学习卡后计算价格
    if (priceSum > 0) {
        var discount = 0.0;
        $("#discountCardArea").find("dt").each(function () {
            if ($(this).find("input[type='checkbox']").prop("checked")) {
                var cardcode = $(this).find("input[type='checkbox']").attr("cardCode");
                selectedCard.push(cardcode);
                var value = $(this).find("input[type='checkbox']").attr("data");
                discount = discount + parseFloat(value);
                priceSum = (priceSum - parseFloat(value)).toFixed(2);
                hiTotalPrice = priceSum;
            }
            //    if ($(this).prop("checked")) {
            //        selectedCard.push($(this).attr("cardCode"));
            //        var value = $(this).attr("data");
            //        priceSum = (priceSum - parseFloat(value)).toFixed(2);
            //        hiTotalPrice = priceSum;
            //}
        });
        $("#discount").html(discount);
    }
    var newprice = priceSum - discountNum < 0 ? 0 : priceSum - discountNum;

    $("#totalPrice").html(newprice);
    hiTotalPrice = newprice;

    //$("#amount").text(newprice + "元");
    $("#amount").attr("name", newprice);
    userprice = newprice;
    $("#sumPrice").html(orderSum);
}

//获取最优满减
function GetBestDiscountNum(priceSum) {
    $('#preferential').html("0");
    discountNum = 0.0;
    topevery.ajax({
        url: 'api/PurchaseDiscount/GetGetBestDiscountNum?priceSum=' + priceSum,
        async: false
    }, function (data) {
        if (data.Success && data.Result) {
            discountNum = data.Result.MinusNum;
            $('#preferential').html("(满" + data.Result.TopNum + "立减" + data.Result.MinusNum + ")&nbsp;&nbsp;" + discountNum + '元');
        }
    });
}
