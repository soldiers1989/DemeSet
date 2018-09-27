$(function () {
    getInfo();
    $("#userSavefp").on("click", function () {
        var invoiceHeaderTime = $("#invoiceHeaderTime").val();
        if (invoiceHeaderTime >= 1) {
            layer.msg("订单完成已超过一个月,不允许再补开电子发票", { time: 2500, icon: 1 });
            return;
        }

        //保存用户电子发票信息
        var obj = new Object();
        obj.OrderNo = $("#hiOrderNoMx").val();
        obj.InvoiceHeader = $("#txtInvoiceHeader").val();
        obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
        obj.InvoicePhone = $("#txtInvoicePhone").val();
        //if ($("#qyRadio").prop("checked")) {
           
        //}
        obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
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


        topevery.ajax({
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
    });
    $(document).on("change", ".invoice", function () {
        if ($(this).is(":checked")) {
            $(".electronicInvoice").show();
            $("#goodssavefp").show();
        } else {
            $(".electronicInvoice").hide();
            $("#goodssavefp").hide();
        }
    })
})

function getInfo() {
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNoMx").val();
    topevery.ajax({
        url: "api/Sheet/OrderSheetDetail",
        data: JSON.stringify(obj)
    }, function (data) {
        if (data.Success) {
            if (data.Result.InvoiceState == 1 || (data.Result.InvoiceHeader || data.Result.InvoiceMailbox || data.Result.TaxpayerIdentificationNumber)) {
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
            $(".mc").html(template("orderDetail_html", data.Result));

        }
    });
}

//function personal(type) {
//    switch (type) {
//        case 1:
//            $("#divpersonal").hide();
//            break;
//        case 2:
//            $("#divpersonal").show();
//            break;
//    }
//}