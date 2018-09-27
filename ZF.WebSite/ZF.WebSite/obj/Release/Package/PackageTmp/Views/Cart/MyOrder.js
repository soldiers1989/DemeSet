var selectType = "";
$(function () {
    $(".j-order-status").find("a").each(function () {
        $(this).on("click", function () {
            RemoveClass(this);
        });
    });
    MyOrderList("");
});
//订单列表
function MyOrderList(sheetState) {
    var obj = new Object();
    obj.State = sheetState;
    obj.Page = $("#PageIndex").val();
    obj.Rows = $("#Rows").val();
    obj.Sidx = " OrderNo ";
    obj.Sord = " desc ";
    topevery.ajax({
        url: "api/Sheet/PageSheetList",
        data: JSON.stringify(obj),
        async:true
    }, function (data) {
        if (data.Success && data.Result.Rows.length > 0) {
            $("#tdTitle").show();
            $("#isInfo").show();
            $("#isNoInfo").hide();
            console.log(data.Result);
            $("#orderList").html(template("orderList_html", data.Result));
            if (data.Result.Records > 0) {
                $('.M-box1').show();
                $('.M-box1').pagination({
                    pageCount: Math.ceil(data.Result.Records / parseInt($("#Rows").val())),
                    jump: true,
                    coping: true,
                    homePage: '首页',
                    endPage: '末页',
                    prevContent: '上页',
                    nextContent: '下页',
                    callback: function (api) {
                        $("#PageIndex").val(api.getCurrent());
                        MyOrderList1(sheetState);
                    }
                });
             
            } 
        } else {
            $("#tdTitle").hide();
            $("#isInfo").hide();
            $("#isNoInfo").show();
        }
    });

}
function MyOrderList1(sheetState) {
    var obj = new Object();
    obj.State = sheetState;
    obj.Page = $("#PageIndex").val();
    obj.Rows = $("#Rows").val();
    obj.Sidx = " OrderNo ";
    obj.Sord = " desc ";
    topevery.ajax({
        url: "api/Sheet/PageSheetList",
        data: JSON.stringify(obj),
        async: true
    }, function (data) {
        if (data.Success && data.Result.Rows.length > 0) {
            $("#orderList").html(template("orderList_html", data.Result));
        } 
    });
}

function RemoveClass(event) {
    $(".j-order-status").find("a").each(function () {
        $(this).removeClass("current");
    });
    $(event).addClass("current");
    selectType = $(event).attr("name");

    // 获取分页器实例对象
    $("#PageIndex").val("1");
    //加载订单列表
    MyOrderList($(event).attr("name"));
}

//撤销订单
function OrderCancel(event) {
    layer.confirm("确认要取消吗，取消后不能恢复", { title: "取消确认" }, function (index) {
        var obj = new Object();
        obj.OrderNo = $(event).attr("id");
        obj.State = 3;
        topevery.ajax({
            url: "api/Sheet/EnditSheetState",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                layer.msg(info.Message, { time: 1000, icon: 1 });
                MyOrderList(selectType);
            } else {
                layer.msg(info.Message, { time: 1000, icon: 2 });
            }
        });
    })
}

//重新下单
function ReOrder(event) {
    var obj = new Object();
    obj.OrderNo = $(event).attr("id");
    topevery.ajax({
        url: "api/Sheet/ReOrder",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            layer.msg(info.Message, { time: 1000, icon: 1 });
            MyOrderList(selectType);
        } else {
            layer.msg(info.Message, { time: 1000, icon: 2 });
        }
    });
}

//付款
function ToPayMent(orderNo) {
    var parment = new Object();
    //订单编号
    parment.OrderNo = orderNo;
    debugger
    $.StandardPost('IframSubmitOrder', parment);
}

//删除订单
function DelOrder(event) {
    layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
        var obj = new Object();
        obj.Id = $(event).attr("id");
        topevery.ajax({
            url: "api/Sheet/DelSheet",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                layer.msg(info.Message, { time: 1000, icon: 1 });
                MyOrderList(selectType);
            } else {
                layer.msg(info.Message, { time: 1000, icon: 2 });
            }
        });
    })
}

$.extend({
    StandardPost: function (url, args) {
        var form = $("<form method='post' target='_blank'></form>"),
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