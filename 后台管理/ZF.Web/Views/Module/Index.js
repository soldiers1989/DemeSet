$(function () {
    var getPostDataUrl = "api/Module/GetList";
    var addUrl = "Module/ModuleAddOrEdit";
    var deleteUrl = "api/Module/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Sort",
        sortorder: "asc",
        colNames: ['序号', '模块名称', '样式', '排序号',''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'ModuleName', index: 'ModuleName', width: 100, align: "center" },
            { name: 'Class', index: 'Class', width: 100, align: "center" },
             { name: 'Sort', index: 'Sort', width: 100, align: "center" },
              { name: '', index: '', width: 200, align: "center" },
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