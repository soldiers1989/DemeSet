$(function () {
    var getPostDataUrl = "api/SysSet/GetList";
    var addUrl = "SysSet/AddOrEdit";
    var deleteUrl = "api/SysSet/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '名称', '参数名称', '参数值', '备注',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 100, align: "center" },
            { name: 'ArguName', index: 'ArguName', width: 100, align: "center" },
            { name: 'ArguValue', index: 'ArguValue', width: 200, align: "center" },
             { name: 'Remark', index: 'Remark', width: 200, align: "center" },
               { name: '', index: '', width: "100", align: "center" },
        ],
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1
        }).trigger("reloadGrid");
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 500);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
});