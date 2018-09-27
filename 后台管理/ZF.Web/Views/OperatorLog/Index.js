$(function () {
    var getPostDataUrl = "api/OperatorLog/GetList";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OperatorDate",
        multiselect:false,
        colNames: ['模块名称', '操作人', '操作时间', '操作类型', '操作者IP', '操作内容',''], //列头
        colModel: [
            { name: 'ModuleName', index: 'ModuleName', width: 70, align: "center" },
            { name: 'OperatorName', index: 'OperatorName', width: 80, align: "center" },
            { name: 'OperatorDate', index: 'OperatorDate', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: 'OperatorTypeName', index: 'OperatorTypeName', width: 60, align: "center" },
            { name: 'OperatorIp', index: 'OperatorIp', width: 80, align: "center" },
            { name: 'Remark', index: 'Remark', width: 300, align: "center" },
              { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    topevery.BindSelect("OperatorType", "Common/OperatorType", "--全部--");
    topevery.BindSelect("ModuleType", "Common/ModuleList", "--全部--");
});