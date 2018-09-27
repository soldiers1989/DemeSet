var selectType = "1";
$(function () {
    $(".j-order-status").find("li").each(function () {
        $(this).find("a").on("click", function () {
            RemoveClass(this);
        });
    });
    MyOrderList("1");
})


function OrderClick(event) {
    var orderNum = $(event).attr("orderNum");
    var orderAmount = $(event).attr("orderAmount");
    $(".wdzf-header-nav").show();
    $(".wdzf-header-nav").html(" <dl><dt>实付：¥ " + parseFloat(orderAmount) + "</dt><dd><a href=\"javascript:;\" id='" + orderNum + "' orderPrice='" + orderAmount + "' onclick=orderToPay(this)>立即支付</a></dd></dl>");
}

function orderToPay(event) {
    var orderNum = $(event).attr("id");
    var orderAmount = $(event).attr("orderPrice");
    var parment = new Object();
    parment.OrderNo = orderNum;
    parment.openid = $("#openid").val();
    parment.OrderAmount = orderAmount;//hiTotalPrice;
    $.StandardPost('MyPay', parment);
}

function MyOrderList(sheetState) {
    if (sheetState != "1") {
        $(".wdzf-header-nav").html("");
        $(".wdzf-header-nav").hide();
    }
    var obj = new Object();
    obj.State = sheetState;
    topevery.ajaxwx({
        url: "api/Sheet/WcChatSheetList",
        data: JSON.stringify(obj),
        async: true
    }, function (data) {
        if (data.Success) {
            if (data.Result.length > 0) {
                $("#orderList").html(template("orderList_html", data));
                $("#orderList").show();
                $(".zanwu").hide();
            } else {
                $("#orderList").hide();
                $(".zanwu").show();
                $(".wdzf-header-nav").html("");
            }

        } else {
            $("#orderList").hide();
            $(".zanwu").show();
            $(".wdzf-header-nav").html("");
        }
    });
}

function RemoveClass(event) {
    $(".j-order-status").find("li").each(function () {
        $(this).find("a").removeClass("hover");
    });
    $(event).addClass("hover");
    selectType = $(event).attr("name");
    //加载订单列表
    MyOrderList($(event).attr("name"));
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

//转订单详情页
function toOrderDetail(event) {
    var orderno = $(event).attr("name");
    window.location.href = "/jjs/PersonalCenter/OrderDetail?orderno=" + orderno;
}

//撤销订单
function OrderCancel(event) {
    layer.confirm("确认要取消吗，取消后不能恢复", { title: "取消确认" }, function (index) {
        var obj = new Object();
        obj.OrderNo = $(event).attr("id");
        obj.State = 3;
        topevery.ajaxwx({
            url: "api/Sheet/EnditSheetState",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                layer.msg(info.Message);
                MyOrderList(selectType);
            } else {
                layer.msg(info.Message);
            }
        });
    })
}