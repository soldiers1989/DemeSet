$(function () {
    topevery.BindSelect("QueryState", "Common/QuestionState", "");
    var getPostDataUrl = "api/SlideSetting/GetList";
    var addUrl = "SlideSetting/AddOrEditCourse";
    var deleteUrl = "api/SlideSetting/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        colNames: ['序号', '链接地址', '状态', '排序号', '上传时间', '备注', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'LinkAddress', index: 'LinkAddress', width: 200, align: "center" },
            { name: 'State', index: 'State', width: 100, align: "center", formatter: topevery.State },
            { name: 'OrderNo', index: 'OrderNo', width: 60, align: "center" },
            { name: 'CreateTime', index: 'CreateTime', width: 80, align: "center", formatter: topevery.dataTimeView },
            { name: 'Remark', index: 'Remark', width: 100, align: "center" },
            { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 550);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 550);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});