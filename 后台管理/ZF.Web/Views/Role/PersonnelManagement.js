$(function () {
    var getPostDataUrl = "api/RoleUser/GetList";
    var addUrl = "Role/RoleUserAddOrEdit?RoleId=" + $("#RoleId").val();
    var deleteUrl = "api/RoleUser/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Id",
        colNames: ['序号', '用户名称', '登录名', '手机号码', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'UserName', index: 'UserName', width: 100, align: "center" },
            { name: 'LoginName', index: 'LoginName', width: 100, align: "center" },
            { name: 'Phone', index: 'Phone', width: 100, align: "center" },
            { name: '', index: '', width: 300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 800, 650);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl,
            page: 1,
            postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});
