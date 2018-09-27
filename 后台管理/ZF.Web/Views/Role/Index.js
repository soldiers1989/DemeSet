$(function () {
    var getPostDataUrl = "api/Role/GetList";
    var addUrl = "Role/AddOrEdit";
    var deleteUrl = "api/Role/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '角色名称', '描述', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'RoleName', index: 'RoleName', width: 100, align: "center" },
            { name: 'Description', index: 'Description', width: 100, align: "center" },
            {
                name: '', index: '', width: 100, align: "center", formatter: function (a, b, c) {
                    var f = "<button type=\"button\" onclick=\"ToView('" + c.Id + "')\" class=\"btn-xs  btn-primary\">分配资源</button>";
                    var e = "<button type=\"button\" onclick=\"PersonnelManagement('" + c.Id + "')\" class=\"btn-xs  btn-primary\">人员管理</button>";
                    return e + " " + f;
                }
            },
            { name: '', index: '', width: 300, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 450);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 450);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl,
            page: 1,
            postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});
//分配资源
function ToView(id) {
    topevery.ajax({ type: "get", url: "Role/AllocatingResources?RoleId=" + id, dataType: "html" }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}
//人员管理
function PersonnelManagement(id) {
    topevery.ajax({ type: "get", url: "Role/PersonnelManagement?RoleId=" + id, dataType: "html" }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}