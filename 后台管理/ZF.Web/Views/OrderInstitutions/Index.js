$(function () {
    var getPostDataUrl = "api/OrderInstitutions/GetList";
    var addUrl = "OrderInstitutions/AddOrEdit";
    var deleteUrl = "api/OrderInstitutions/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Name",
        sortorder: "asc",
        colNames: ['序号', '机构名称', '机构联系人', '机构联系方式', '微信二维码', ''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 100, align: "center" },
            { name: 'Contact', index: 'Contact', width: 100, align: "center" },
             { name: 'ContactPhone', index: 'ContactPhone', width: 100, align: "center" },
            {
                name: 'Url',
                index: 'Url',
                width: 100,
                align: "center",
                formatter: function (a, b, c) {
                    var e = "<a href=\"" + c.Url + "\">下载</a>";
                    return e;
                }
            },
              { name: '', index: '', width: 200, align: "center" },
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

});