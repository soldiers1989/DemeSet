$(function () {
    getInfo();
    $("#userSavefp").on("click", function () {
        debugger
        //保存用户电子发票信息
        var obj = new Object();
        obj.OrderNo = $("#hiOrderNoMx").val();
        obj.InvoiceHeader = $("#txtInvoiceHeader").val();
        obj.InvoiceMailbox = $("#txtInvoiceMailbox").val();
        obj.TaxpayerIdentificationNumber = $("#txtTaxpayerIdentificationNumber").val();
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
            }
            $(".mc").html(template("orderDetail_html", data.Result));

        }
    });
}