$(function () {
    var getPostDataUrl = "api/PaperGroupRelation/GetList1";

    var grid = $("#tblData1");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Id",
        pager: "#pager1",
        height: 450,
        colNames: ['序号', '试卷编号', '试卷名称', '时长', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'PaperId', index: 'PaperId', width: 120, align: "center", hidden: true },
            { name: 'PaperName', index: 'PaperName', width: 120, align: "center" },
            { name: 'TestTime', index: 'TestTime', width: 50, align: "center" },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom1")
    });
    $(".query_btn1").on("click", function () {
        $("#tblData1").jqGrid('setGridParam', {
            url: getPostDataUrl,
            page: 1,
            postData: topevery.form2Json("selectFrom1")
        }).trigger("reloadGrid");
    });
});
$(".add_btn1").click(function () {
    debugger;
    var addCoursePaper = "api/PaperGroupRelation/AddOrEdit";
    var data = $("#tblData1").jqGrid('getGridParam', 'selarrrow');
    var ids = [];
    if (data.length > 0) {
        //遍历访问这个集合  
        $(data).each(function (index, id) {
            //由id获得对应数据行  
            var rowData = $("#tblData1").jqGrid('getRowData', id);
            ids.push(rowData.PaperId);
        });
        topevery.ajax({
            url: addCoursePaper,
            data: JSON.stringify({
                "PaperIds": ids.join(),
                PaperGroupId: $("#PaperGroupId1").val()
            })
        }, function (data) {
            var icon = 1;
            var message = "保存失败";
            if (data.Success) {
                icon = data.Result.Success === true ? 1 : 2;
                if (data.Result.Success) {
                    $("#tblData1").trigger("reloadGrid");
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
    } else {
        layer.alert("请选择一条记录!");
    }
});