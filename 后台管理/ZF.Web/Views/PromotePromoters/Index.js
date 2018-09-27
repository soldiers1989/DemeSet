$(function () {
    var getPostDataUrl = "api/PromotePromoters/GetList";
    var addUrl = "PromotePromoters/AddOrEdit";
    var deleteUrl = "api/PromotePromoters/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "AddTime",
        sortorder: "asc",
        colNames: ['序号', '推广员名称', '所属公司', '联系方式', '推广码', '提成比例(%)', '推广地址', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 100, align: "center" },
            { name: 'CompanyName', index: 'CompanyName', width: 100, align: "center" },
            { name: 'Contact', index: 'Contact', width: 100, align: "center" },
            { name: 'PromotionCode', index: 'PromotionCode', width: 100, align: "center" },
            { name: 'CommissionRatio', index: 'CommissionRatio', width: 100, align: "center" },
               { name: 'PromotionCodeUrl', index: 'PromotionCodeUrl', width: 190, align: "center" },
            { name: '', index: '', width: 200, align: "center" },
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

    topevery.BindSelect("QueryCompanyId", "Common/CompanyList", "--全部--");
});