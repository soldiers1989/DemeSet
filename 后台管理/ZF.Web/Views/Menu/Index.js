$(function () {
    var header = $(".content-wrapper").outerHeight(true)
                      - $(".nav-header").outerHeight(true) - 3
                      - $(".content-header").outerHeight(true)
                      - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                      - 31;
    $(".Tree").height(header);
    var getPostDataUrl = "api/Menu/GetList";
    var addUrl = "Menu/AddOrEdit";
    var deleteUrl = "api/Menu/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Sort",
        sortorder: "asc",
        colNames: ['序号', '菜单名称', '样式', '路径', '描述', '排序号', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'MenuName', index: 'MenuName', width: 100, align: "center" },
            { name: 'Class', index: 'Class', width: 60, align: "center" },
            { name: 'Url', index: 'Url', width: 100, align: "center" },
            { name: 'Description', index: 'Description', width: 100, align: "center" },
            { name: 'Sort', index: 'Sort', width: 60, align: "center" },
             { name: '', index: '', width: 150, align: "center" },
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
    topevery.BindSelect("QueryModuleId", "Common/MenuModuleList", "--全部--");
    topevery.BindTree("treeDemo", "Common/MenuModuleTree", onClickTree);
    function onClickTree(data) {
        if (data.level === 0) {
            $("#QueryModuleId").val(data.id);
            $(".query_btn").click();
        }
    }
});