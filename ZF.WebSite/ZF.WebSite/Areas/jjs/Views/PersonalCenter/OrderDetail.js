$(function () {
    getInfo();
    $(document).on("change", ".jdradio001", function () {
        if ($(this).is(":checked")) {
            $(".container").show();
            $("#goodssavefp").show();
        } else {
            $(".container").hide();
            $("#goodssavefp").hide();
        }
    })

    //切换个人与企业发票信息
    //$(".tabs").find("li").each(function () {
    //    $(this).click(function () {
    //        $(".tabs").find("li").removeClass("active");
    //        $(this).addClass("active");
    //        if ($(this).attr("id") == "lione") {
    //            $("#divpersonal").hide();
    //        } else {
    //            $("#divpersonal").show();
    //        }
    //    })
    //})
})

function getInfo() {
    var orderno = topevery.getUrlParam("orderno");
    var obj = new Object();
    obj.OrderNo = orderno;
    topevery.ajaxwx({
        url: "api/Sheet/OrderSheetDetail",
        data: JSON.stringify(obj)
    }, function (data) {
        if (data.Success) {
            debugger
            if (data.Result.InvoiceState == 1 || (data.Result.InvoiceHeader || data.Result.InvoiceMailbox || data.Result.TaxpayerIdentificationNumber) || (data.Result.State == 2 || data.Result.State == 3)) {
                $("#electronicInvoice").hide();
            }
            else {
                $("#electronicInvoice").show();
                $("#txtInvoiceHeader").val(data.Result.InvoiceHeader);
                $("#txtInvoiceMailbox").val(data.Result.InvoiceMailbox);
                $("#txtTaxpayerIdentificationNumber").val(data.Result.TaxpayerIdentificationNumber);
                $("#txtInvoicePhone").val(data.Result.InvoicePhone);
            }
            $("#invoiceHeaderTime").val(data.Result.InvoiceHeaderTime)
            $("#detailinfo").html(template("orderDetail_html", data.Result));

        }
    });
}

function personal(type) {
    switch (type) {
        case 1:
            $("#divpersonal").hide();
            break;
        case 2:
            $("#divpersonal").show();
            break;
    }
}

function addInvoiceInfo() {
    var invoiceHeaderTime = $("#invoiceHeaderTime").val();
    if (invoiceHeaderTime >= 1) {
        layer.msg("订单完成已超过一个月,不允许再补开电子发票", { time: 2500, icon: 1 });
        return;
    }
    //保存用户电子发票信息
    var obj = new Object();
    obj.OrderNo = topevery.getUrlParam("orderno");
    obj.InvoiceHeader = $("#txtInvoiceHeader").val();
    obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
    obj.InvoicePhone = $("#txtInvoicePhone").val();
    //if (!$("#divpersonal").is("hidden")) {
    //}
    obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
    if (!obj.InvoiceMailbox || !obj.InvoicePhone) {
        parent.layer.msg("邮箱和手机号不能为空！", { time: 1500, icon: 2 });
        return
    }

    topevery.ajaxwx({
        url: "api/Sheet/EnditiSheetElectronicInvoice",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            getInfo();
            layer.msg("保存成功", { time: 1500, icon: 1 });
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}