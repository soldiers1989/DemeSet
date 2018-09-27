$(function () {
    var getPostDataUrl = "api/BaseTag/GetList";
    var addUrl = "BaseTag/AddOrEdit";
    var deleteUrl = "api/BaseTag/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '模块编码', '标签名称', '备注',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 80, align: "center", hidden: true },
            { name: 'ModelCode', index: 'ModelCode', width: 30, align: "center" },
            { name: 'TagName', index: 'TagName', width: 40, align: "center" },
            { name: 'Remark', width: 50, align: "center" },
            { name: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 500, 400);
    $(".edit_btn").bindEditBtn(addUrl, grid, 500, 400);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});