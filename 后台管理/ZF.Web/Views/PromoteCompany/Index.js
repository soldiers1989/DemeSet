$(function () {
    var getPostDataUrl = "api/PromoteCompany/GetList";
    var addUrl = "PromoteCompany/AddOrEdit";
    var deleteUrl = "api/PromoteCompany/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "AddTime",
        sortorder: "asc",
        colNames: ['序号', '公司名称', '联系人', '提成比例(%)', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 100, align: "center" },
            { name: 'TheContact', index: 'TheContact', width: 100, align: "center" },
                { name: 'CommissionRatio', index: 'CommissionRatio', width: 100, align: "center" },
            { name: '', index: '', width: 300, align: "center" },
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