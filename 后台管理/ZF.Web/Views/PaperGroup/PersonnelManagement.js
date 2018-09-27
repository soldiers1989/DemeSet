$(function() {
    var getPostDataUrl = "api/PaperGroupRelation/GetList";
    var addUrl = "PaperGroup/PaperGroupAddOrEdit?PaperGroupId=" + $("#PaperGroupId").val();
    var deleteUrl = "api/PaperGroupRelation/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Id",
        colNames: ['序号', '试卷名称', '时长', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'PaperName', index: 'PaperName', width: 120, align: "center" },
            { name: 'TestTime', index: 'TestTime', width: 50, align: "center" },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 800, 650);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function() {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl,
            page: 1,
            postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});
