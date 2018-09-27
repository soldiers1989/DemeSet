$(function () {
    var getPostDataUrl = "api/Project/GetList";
    var addUrl = "Project/AddOrEdit";
    var deleteUrl = "api/Project/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        sortorder: "asc",
        colNames: ['序号', '项目名称', '所属项目分类', '排序号', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'ProjectName', index: 'ProjectName', width: 100, align: "center" },
            { name: 'ProjectClassName', index: 'ProjectClassId', width: 100, align: "center" },
                { name: 'OrderNo', index: 'OrderNo', width: 100, align: "center" },
            { name: '', index: '', width: 300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 500);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    topevery.BindSelect("QueryProjectClassId", "Common/ProjectClassificationList", "--全部--");
});