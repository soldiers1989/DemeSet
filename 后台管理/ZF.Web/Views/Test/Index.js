$(function () {
    var getPostDataUrl = "api/User/GetList";
    var addUrl = "Test/UserAddOrEdit";
    var deleteUrl = "api/User/Delete";
    //var exportUrl = "Test/Export";
    //var exportdbfUrl = "Test/ExportDbf";
    var importUrl = "Test/ImpRept";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '登录名', '用户名', '是否管理员', '手机号码',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'LoginName', index: 'LoginName', width: 100, align: "center" },
            { name: 'UserName', index: 'UserName', width: 100, align: "center" },
            { name: 'IsAdmin', index: 'IsAdmin', width: 100, align: "center", formatter: topevery.isYesOrNo },
            { name: 'Phone', index: 'Phone', width: 100, align: "center" },
               { name: '', index: '', width:300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 450);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 450);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    //$(".export_btn").bindExportBtn(exportUrl);
    //$(".exportdbf_btn").bindExportBtn(exportdbfUrl);
   //$(".import_btn").btnIfram(importUrl);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});