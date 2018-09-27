$(function () {
    var getPostDataUrl = "api/Subject/GetList";
    var addUrl = "Subject/AddOrEdit";
    var deleteUrl = "api/Subject/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        sortorder: "asc",
        colNames: ['科目编码', '科目名称', '所属项目分类', '所属项目', '是否经济基础', '排序号', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'SubjectName', index: 'SubjectName', width: 100, align: "center" },
            { name: 'ProjectClassName', index: 'ProjectClassName', width: 100, align: "center" },
            { name: 'ProjectName', index: 'ProjectName', width: 100, align: "center" },
              { name: 'IsEconomicBase', index: 'IsEconomicBase', width: 100, align: "center", formatter: topevery.IsEconomicBase },
            { name: 'OrderNo', index: 'OrderNo', width: 40, align: "center" },
           { name: '', index: '', width: 200, align: "center" },
            //{ name: 'AddTime', index: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 500, 550);
    $(".edit_btn").bindEditBtn(addUrl, grid, 500, 550);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    topevery.BindSelect("QueryProjectClassId", "Common/ProjectClassificationList", "--全部--");
    
    $('#QueryProjectClassId').change(function () {
        var projectClassId = $(this).children('option:selected').val();
        topevery.BindSelect("QueryProjecttId", "Common/ProjectList?ProjectClassId=" + projectClassId, "--全部--");
    })

});