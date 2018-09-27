var selectType = 0;
$(function () {
    $(".j-order-status").find("a").each(function () {
        $(this).on("click", function () {
            RemoveClass(this);
        });
    });
    getInfo(selectType);
})


function RemoveClass(event) {
    $(".j-order-status").find("a").each(function () {
        $(this).removeClass("current");
    });
    $(event).addClass("current");
    selectType = $(event).attr("name");
    //加载需邮寄信息
    getInfo(selectType);
}


function getInfo(infoType) {
    var obj = new Object();
    obj.InvoiceState = infoType;
    topevery.ajax({
        url: "api/OrderSheet/GetElectronicInvoiceInfoByOrderSheet",
        data:JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            $("#electronicInvoiceHtml").html(template("electronicInvoice_jshtml", data))
        }
    });
}

//处理电子发票
function setElectronicInvoice(event) {
    var obj = new Object();
    obj.Id = $(event).attr("name");
    topevery.ajax({
        url: "api/OrderSheet/EnditiElectronicInvoice",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            layer.msg(info.Message, { time: 1500, icon: 1 });
            getInfo(selectType);
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}
$(".report_btn").click(function () {
    window.location.href = "OrderSheet/Export?type=0";
});
$(".report_btn1").click(function () {
    window.location.href = "OrderSheet/Export?type=1";
});