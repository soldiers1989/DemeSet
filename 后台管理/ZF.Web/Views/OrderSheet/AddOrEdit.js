$(function () {
    topevery.ajax({
        url: "Common/SelectRegisterUser",
        data: JSON.stringify({
        })
    }, function (data) {
        $("#RegisterUserId").select2({
            data: data,
            placeholder: '请选择',
            allowClear: true
        })
    });

    topevery.ajax({
        url: "Common/InstitutionsList",
        data: JSON.stringify({
        })
    }, function (data) {
        $("#InstitutionsId").select2({
            data: data,
            placeholder: '请选择',
            allowClear: true
        })
    });
    //Date picker

    $("#return").bind("click", function () {
        topevery.ajax({ type: "get", url: "OrderSheet/Index", dataType: "html" }, loadHtml, true);
    });
    function loadHtml(data) {
        $(".content-wrapper").html(data);
    }
    var grid = $("#tblData1");
    grid.jgridInit({
        url: "",
        pager: "#pager1",
        caption: "已选择选择课程/套餐",
        height: "200",
        colNames: ['序号', '课程/套餐名称', '标签', '原价', '优惠价', '数量', '合计', '课程类型(0:课程 1：套餐)', '有效日期', '类型', '操作'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'CourseName', index: '', width: 120, align: "center" },
            { name: 'CourseTag', index: '', width: 50, align: "center", hidden: true },
            { name: 'Price', index: '', width: 50, align: "center" },
            { name: 'FavourablePrice', index: '', width: 50, align: "center" },
            { name: 'Number', index: '', width: 50, align: "center" },
            { name: 'Total', index: '', width: 50, align: "center" },
            { name: 'CourseType', index: '', width: 50, align: "center", hidden: true },
            { name: 'ValidityEndDate', index: '', width: 70, align: "center" },
            { name: 'TypeName', index: '', width: 70, align: "center" },
            {
                name: 'edit',
                index: '',
                width: 100,
                align: "center",
                formatter: function (a, b, c) {
                    var f = "<button type=\"button\" onclick=\"removeAll(" + b.rowId + ")\" class=\"btn-xs btn-danger\">移除全部</button>";
                    var g = "<button type=\"button\" onclick=\"remove(" + b.rowId + ",'" + c.Id + "','" + c.Number + "')\" class=\"btn-xs btn-danger\">移除部分</button>";
                    return f + "  " + g;
                }
            },
        ],
    });

    var getPostDataUrl = "api/CourseInfo/GetPackcourseInfoInfoList";
    var grid1 = $("#tblData2");
    grid1.jgridInit({
        url: getPostDataUrl,
        pager: "#pager2",
        caption: "可选择课程/套餐",
        height: "200",
        sortname: "CourseName",
        colNames: ['序号', '课程/套餐名称', '标签', '原价', '优惠价', '课程类型(0:课程 1：套餐)', '有效日期', '类型', '操作'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'CourseName', index: '', width: 120, align: "center" },
            { name: 'CourseTag', index: '', width: 50, align: "center", hidden: true },
            { name: 'Price', index: '', width: 50, align: "center" },
            { name: 'FavourablePrice', index: '', width: 50, align: "center" },
            { name: 'CourseType', index: '', width: 50, align: "center", hidden: true },
            { name: 'ValidityEndDate', index: '', width: 50, align: "center" },
            { name: 'TypeName', index: '', width: 50, align: "center" },
            {
                name: '',
                index: '',
                width: 100,
                align: "center",
                formatter: function (a, b, c) {
                    var f = "<button type=\"button\" class=\"btn-xs  btn-primary\" onclick=\"numberChoose('" + b.rowId + "','" + c.Id + "')\">添加</button>";
                    return f;
                }
            },
        ],
        postData: topevery.form2Json("selectFrom")
    });

    $(".query_btn1").on("click", function () {
        $("#tblData2").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    $("#sumbit").bind("click", function () {
        var registerUserId = "";
        if ($('#RegisterUserId').select2('data').length > 0)
            registerUserId = $('#RegisterUserId').select2('data')[0].id;
        var institutionsId = "";
        if ($('#InstitutionsId').select2('data').length > 0)
            institutionsId = $('#InstitutionsId').select2('data')[0].id;
        var orderAmount = $("#OrderAmount").val();
        var factPayAmount = $("#FactPayAmount").val();
        var payTime = $("#PayTime").val();
        var tradeNo = $("#TradeNo").val();
        var remark = $("#Remark").val();
        var data = $("#tblData1").jqGrid('getRowData');
        if (!registerUserId) {
            layer.alert("下单用户不能为空!");
            return;
        }
        if (!orderAmount) {
            layer.alert("订单金额不能为空!");
            return;
        }
        if (!factPayAmount) {
            layer.alert("实际支付金额不能为空!");
            return;
        }
        if (!payTime) {
            layer.alert("实际支付时间不能为空!");
            return;
        }
        if (!tradeNo) {
            layer.alert("交易编号不能为空!");
            return;
        }
        if (data.length === 0) {
            layer.alert("课程/套餐不能为空!");
            return;
        }
        topevery.ajax({
            url: "api/OrderSheet/AddOrEdit",
            data: JSON.stringify({
                registerUserId: registerUserId,
                InstitutionsId: institutionsId,
                orderAmount: orderAmount,
                factPayAmount: factPayAmount,
                payTime1: payTime,
                tradeNo: tradeNo,
                remark: remark,
                RegiestInput: [],
                data: data
            })
        }, function (data) {
            var message = "提交失败";
            var icon = 1;
            if (data.Success) {
                message = data.Result.Message;
                icon = data.Result.Success === true ? 1 : 2;
                if (data.Result.Success) {
                    layer.msg(message, {
                        icon: icon,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 5
                    });
                    $("#return").click();
                } else {
                    layer.msg(message, {
                        icon: icon,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 5
                    });
                }
            }
        });
    });
});
var index;
//添加课程数量选择页面  rowindex   选中行 addCourse  保存按钮回调函数  id  课程编号
function numberChoose(rowIndex, id) {
    topevery.ajax({ type: "get", url: "OrderSheet/NumberChoose?rowIndex=" + rowIndex + "&Callback=addCourse" + "&Id=" + id, dataType: "html" }, function (data) {
        index = layer.open({
            type: 1,
            title: "输入数量",
            skin: 'layui-layer-rim', //加上边框
            area: [300 + 'px', 180 + 'px'], //宽高
            content: data
        });
    }, true);
}


$("#primary").click(function () {
    var data = $("#tblData2").jqGrid('getGridParam', 'selarrrow');
    for (var i = 0; i < data.length; i++) {
        addCourse("", data[i], 1);
    }
});

//回调函数  保存选中的课程到课程列表中
function addCourse(id, rowIndex, number) {
    var isok = true;
    var data = $("#tblData2").jqGrid('getRowData', rowIndex);
    var rowData = $("#tblData1").jqGrid("getRowData");
    var ids = $("#tblData1").jqGrid('getDataIDs');
    for (var i = 0; i < rowData.length; i++) {
        if (rowData[i].Id == id) {
            isok = false;
            var updaterow = $("#tblData1").jqGrid('getRowData', ids[i]);
            updaterow.Number = parseFloat(number) + parseFloat(updaterow.Number);
            updaterow.Total = parseFloat(updaterow.Number) * parseFloat(rowData[i].FavourablePrice);
            $("#tblData1").jqGrid("setRowData", ids[i], updaterow, false);
        }
    }
    if (isok) {
        //获得当前最大行号（数据编号）
        var rowid = Math.max.apply(Math, ids);
        if (rowid == "-Infinity") {
            rowid = 0;
        }
        //获得新添加行的行号（数据编号）
        var newrowid = rowid + 1;
        var dataRow = {
            Id: data.Id,
            CourseName: data.CourseName,
            CourseTag: data.CourseTag,
            Price: data.Price,
            FavourablePrice: data.FavourablePrice,
            Number: number,
            Total: parseFloat(number) * parseFloat(data.FavourablePrice),
            ValidityEndDate: data.ValidityEndDate,
            TypeName: data.TypeName,
            CourseType: data.CourseType,
            edit: "",
        }
        $("#tblData1").jqGrid("addRowData", newrowid, dataRow, "first");
    }
    AmountCalculate();
    layer.close(index);
}
//通过rowIndex  删除全部课程数量
function removeAll(rowIndex) {
    $("#tblData1").jqGrid("delRowData", rowIndex);
    AmountCalculate();
}

//删除部分课程
function remove(rowIndex, id, number1) {
    topevery.ajax({ type: "get", url: "OrderSheet/NumberChoose?rowIndex=" + rowIndex + "&Callback=deleteCourse" + "&Id=" + id + "&number1=" + number1, dataType: "html" }, function (data) {
        index = layer.open({
            type: 1,
            title: "输入数量",
            skin: 'layui-layer-rim', //加上边框
            area: [300 + 'px', 180 + 'px'], //宽高
            content: data
        });
    }, true);
}

///删除部分数量选择页面  rowindex   选中行 addCourse  保存按钮回调函数  id  课程编号
function deleteCourse(id, rowIndex, number) {
    var updaterow = $("#tblData1").jqGrid('getRowData', rowIndex);
    if (parseFloat(updaterow.Number) === parseFloat(number)) {
        $("#tblData1").jqGrid("delRowData", rowIndex);
    } else {
        updaterow.Number = parseFloat(updaterow.Number) - parseFloat(number);
        updaterow.Total = parseFloat(updaterow.Number) * parseFloat(updaterow.FavourablePrice);
        $("#tblData1").jqGrid("setRowData", rowIndex, updaterow, false);
    }
    AmountCalculate();
    layer.close(index);
}

///计算总计金额  优惠后金额
function AmountCalculate() {
    var totalAmount = 0;
    var afterDiscount = 0;
    var data = $("#tblData1").jqGrid('getRowData');
    for (var i = 0; i < data.length; i++) {
        totalAmount += parseFloat(data[i].Price) * parseFloat(data[i].Number);
        afterDiscount += parseFloat(data[i].Total);
    }
    $("#TotalAmount").html(totalAmount);
    $("#AfterDiscount").html(afterDiscount);
    $("#OrderAmount").val(afterDiscount);
}