$(function () {
    var urlList = [
        "api/DataType/GetList", //查询
        "DataType/AddOrEdit",//新增或修改
        "api/DataType/Delete"//删除
    ];
    var grid = $("#tblData");
    grid.jgridInit({
        url: urlList[0],
        sortname: "Sort",
        sortorder: "asc",
        colNames: ['编码', '分类名称', '分组代码', '说明', '排序号', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 100, align: "center", hidden: true },
            { name: 'DataTypeName', index: 'DataTypeName', width: 100, align: "center" },
            { name: 'DataTypeCode', index: 'DataTypeCode', width: 100, align: "center" },
            { name: 'Desc', index: 'Desc', width: 100, align: "center" },
            { name: 'Sort', index: 'Sort', width: 60, align: "center" },
               { name: '', index: '', width: 250, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(urlList[1], 600, 500);
    $(".edit_btn").bindEditBtn(urlList[1], grid, 600, 500);
    $(".del_btn").bindDelBtn(urlList[2], grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: urlList[0], page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});