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

$(function () {
    var parament = $("#hiparament").val();
    topevery.ajax({
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
                $("#pathval").show();
                $("#pathval").html("课程：" + handeout + "需要邮寄讲义,请选择需邮寄的地址");
                getDeliveryAddress();
            }
            $("#amount").text(amount + "元");
            $("#amount").attr("name", amount);
            $("#orderno").text(orderno);
            $("#hiOrderNO").val(orderno);
            $(".CartCount").html($("#hicartcount").val());
        }
    });
    $("#zfbdiv").on("click", function () {
        $(this).addClass("active");
        $("#wxdiv").removeClass("active");
    });
    $("#wxdiv").on("click", function () {
        $(this).addClass("active");
        $("#zfbdiv").removeClass("active");
    });
   


});

function OrderClick() {
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
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNO").val();
    obj.InvoiceHeader = $("#txtInvoiceHeader").val();
    obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
    obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
    topevery.ajax({
        url: "api/Sheet/EnditiSheetElectronicInvoice",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            //电子发票写入成功之后转到支付
            var url = "OnlinePayment" + "?OrderNo=" + $("#orderno").text() + "&OrderAmount=" + $("#amount").attr("name") + "&type=" + type;
            topevery.ajaxToThis({ type: "get", url: url, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: type == 0 ? "支付宝" : "微信",
                    skin: 'layui-layer-rim', //加上边框
                    area: [350 + 'px', ystyle + 'px'], //宽高
                    content: data
                });
            }, true);
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
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
            }
        });
    }
 
}

function getDeliveryAddress() {
    debugger
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
    layer.confirm("确定删除吗?", { title: "删除确认" }, function (index) {
        topevery.ajax({
            url: "api/DeliveryAddress/DelDeliveryAddress",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                layer.msg(info.Message, { time: 1500, icon: 1 });
                getDeliveryAddress();
            } else {
                layer.msg(info.Message, { time: 1500, icon: 2 })
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
            layer.msg(info.Message, { time: 1500, icon: 1 });
            enditiSheetDeliveryAddRess($(event).attr("name"));
            getDeliveryAddress();
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 })
        }

    });
}
//编辑
function alertUpdateAddressDiagByoverseas(event) {
    layer.open({
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
    layer.open({
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
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}
