$(function () {
    var getPostDataUrl = "api/ProjectClassification/GetList";
    var addUrl = "ProjectClassification/AddOrEdit";
    var deleteUrl = "api/ProjectClassification/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        sortorder: "asc",
        colNames: ['序号', '项目分类名称', '排序号', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'ProjectClassName', index: 'ProjectClassName', width: 100, align: "center" },
            { name: 'OrderNo', index: 'OrderNo', width: 100, align: "center" },
            { name: '', index: '', width: 300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 400);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 400);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});