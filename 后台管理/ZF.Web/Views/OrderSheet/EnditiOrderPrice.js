
var getPostDataUrl = "api/OrderSheet/GetList";
(function ($) {
    $.ToView = function (id,price) {        
        layer.open({
            type: 2,
            title: "修改订单金额",
            skin: 'layui-layer-rim', //加上边框
            area: [600 + 'px', 400 + 'px'], //宽高
            content: "OrderSheet/SetPrice?OrderNo=" + id + "&price=" + price,
            end: function () {
                $("#tblData").jqGrid('setGridParam', {
                    url: getPostDataUrl, page: 1, postData: JSON.parse(topevery.extend(topevery.form2Json("selectFrom")))
                }).trigger("reloadGrid");
            }
        });
    }
})(jQuery);

$(function () {

    var addUrl = "OrderSheet/AddOrEdit";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '课程名称', '订单号', '下单用户', '下单时间', '订单状态', '订单金额', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'CourseName', index: '', width: 120, align: "center", hidden: true },
            { name: 'OrderNo', index: 'OrderNo', width: 120, align: "center" },
            { name: 'RegisterUserName', index: '', width: 90, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            {
                name: 'State', index: '', width: 60, align: "center", formatter: function (a, b, c) {
                    var e = "";
                    if (c.State === "已付款") {
                        e = "<a style='color:#00a65a;'>" + c.State + "</a>";
                    } else if (c.State === "已取消") {
                        e = "<a style='color:#e27552;'>" + c.State + "</a>";
                    } else if (c.State === "已撤销") {
                        e = "<a style='color:red;'>" + c.State + "</a>";
                    } else {
                        e = "<a style='color:#000'>" + c.State + "</a>";
                    }
                    return e;
                }
            },
             { name: 'OrderAmount', index: 'OrderAmount', width: 60, align: "center" },
            {
                name: '', index: '', width: 40, align: "center", formatter: function (a, b, c) {
                    var e = "";
                    var f = "<button type=\"button\" onclick=\"$.ToView('" + c.OrderNo + "','" + c.OrderAmount + "')\" class=\"btn-xs  btn-primary\">编辑</button>";
                    return e + " " + f;
                }
            },
             { name: '', index: '', width: 150, align: "center" },
        ],
        postData: JSON.parse(topevery.extend(topevery.form2Json("selectFrom")))
    });

    function loadHtml1(data) {
        $(".content-wrapper").html(data);
    }
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: JSON.parse(topevery.extend(topevery.form2Json("selectFrom")))
        }).trigger("reloadGrid");
    });
});