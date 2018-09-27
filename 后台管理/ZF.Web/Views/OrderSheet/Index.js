(function ($) {
    $.revocation = function (id) {
        layer.confirm("确认要撤销吗，撤销后不能恢复", { title: "撤销确认" }, function (index) {
            topevery.ajax({
                url: "api/OrderSheet/Revocation",
                data: JSON.stringify({
                    "Ids": id
                })
            }, function (data) {
                var icon = 1;
                var message = "撤销失败";
                if (data.Success) {
                    icon = data.Result.Success === true ? 1 : 2;
                    if (data.Result.Success) {
                        $("#tblData").trigger("reloadGrid");
                    }
                    message = data.Result.Message;
                } else {
                    message = data.Error;
                }
                layer.msg(message, {
                    icon: icon,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            });
        });
    },
    $.ToView = function (id) {
        /*layer弹出一个html页面或者html片段*/
        //topevery.ajax({ type: "get", url: "OrderSheet/ToView?OrderNo=" + id, dataType: "html" }, function (data) {
        layer.open({
            type: 2,
            title: "订单产品信息",
            skin: 'layui-layer-rim', //加上边框
            area: [1200 + 'px', 590 + 'px'], //宽高
            content: "OrderSheet/ToView?OrderNo=" + id
        });
        //}, true);
    }
})(jQuery);

$(function () {
    var getPostDataUrl = "api/OrderSheet/GetList";
    var addUrl = "OrderSheet/AddOrEdit";
    var pladdUrl = "OrderSheet/PlAddOrEdit";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '课程名称', '订单号', '下单机构', '下单用户', '下单时间', '订单状态', '订单金额', '实际支付金额', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'CourseName', index: '', width: 120, align: "center", hidden: true },
            { name: 'OrderNo', index: 'OrderNo', width: 120, align: "center" },
            { name: 'InstitutionsName', index: '', width: 90, align: "center" },
            { name: 'RegisterUserName', index: '', width: 90, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            {
                name: 'State',
                index: '',
                width: 70,
                align: "center",
                formatter: function (a, b, c) {
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
            { name: 'FavourablePrice', index: 'FavourablePrice', width: 60, align: "center" },
            { name: 'FactPayAmount', index: 'FactPayAmount', width: 60, align: "center" },
            {
                name: '',
                index: '',
                width: 60,
                align: "center",
                formatter: function (a, b, c) {
                    var e = "";
                    if (c.OrderType === "后台" && c.State !== "已撤销") {
                        e = "<button type=\"button\" onclick=\"$.revocation('" + c.Id + "')\" class=\"btn-xs  btn-primary\">撤销</button>";
                    }
                    var f = "<button type=\"button\" onclick=\"$.ToView('" + c.OrderNo + "')\" class=\"btn-xs  btn-primary\">查看</button>";
                    return e + " " + f;
                }
            },
            { name: '', index: '', width: 150, align: "center" },
        ],
        postData: JSON.parse(topevery.extend(topevery.form2Json("selectFrom"), {}))
    });

    //topevery.ajax({
    //    url: "Common/Select2CourseList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $('#CourseId').select2({
    //        data: data,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $(".box"),
    //        allowClear: true
    //    })
    //    $('#CourseId').val('-1').trigger('change');
    //});

    function loadHtml1(data) {
        $(".content-wrapper").html(data);
    }

    $(".add_btn").on("click", function () {
        topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, loadHtml1, true);
    });

    $(".pladd_btn").on("click", function () {
        topevery.ajax({ type: "get", url: pladdUrl, dataType: "html" }, loadHtml1, true);
    });
    

    topevery.ajax({
        url: "Common/InstitutionsList",
        data: JSON.stringify({
        })
    }, function (data) {
        $("#InstitutionsId").select2({
            data: data,
            allowClear: true,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
        })
        $('#InstitutionsId').val('-1').trigger('change');
        //$("#InstitutionsId").val("").select2();
    });

    $(".query_btn").on("click", function () {
        var InstitutionsId = "";
        if ($('#InstitutionsId').select2('data').length > 0)
            InstitutionsId = $('#InstitutionsId').select2('data')[0].id;
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: JSON.parse(topevery.extend(topevery.form2Json("selectFrom"), { InstitutionsId: InstitutionsId }))
        }).trigger("reloadGrid");
    });
    //订单状态
    topevery.BindSelect("QueryState", "Common/OrderState", "--全部--");
    ////支付方式
    topevery.BindSelect("QueryPayType", "Common/DataDictionary?dataTypeId=1c417bf9-e952-405c-9a41-bc732bcccf37", "--全部--");
    ////下单终端
    //topevery.BindSelect("QueryOrderType", "Common/OrderType", "--全部--");

});