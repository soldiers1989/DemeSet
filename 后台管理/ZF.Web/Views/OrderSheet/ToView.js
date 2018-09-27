$(function () {
    getInfo();
    $(document).on("change", ".invoice", function () {
        if ($(this).is(":checked")) {
            $(".electronicInvoice").show();
        } else {
            $(".electronicInvoice").hide();
        }
    })
})

function getInfo() {
    var obj = new Object();
    obj.OrderNo = $("#hiOrderNoMx").val();
    topevery.ajax({
        url: "/api/OrderSheet/OrderSheetDetail",
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