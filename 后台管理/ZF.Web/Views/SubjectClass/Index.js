$(function () {
    var getPostDataUrl = "api/SubjectClass/GetList";
    var addUrl = "SubjectClass/AddOrEdit";
    var deleteUrl = "api/SubjectClass/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        colNames: ['序号', '题型名称', '题型所属项目', '试题表现形式', '排序号', '', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'ClassName', index: 'ClassName', width: 80, align: "center" },
            { name: 'ProjectName', index: 'ProjectId', width: 100, align: "center" },
            { name: 'BigTypeName', index: '', width: 100, align: "center" },
            { name: 'OrderNo', index: 'OrderNo', width: 100, align: "center" },
            { name: '', index: '', width: 150, align: "center" },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 600);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 600);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    topevery.BindSelect("QueryProjectId", "Common/ProjectIdSelect", "--全部--");
});