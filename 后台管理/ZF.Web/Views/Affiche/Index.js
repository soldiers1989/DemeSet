$(function () {
    var getPostDataUrl = "api/AfficheHelp/GetList";
    var addUrl = "Affiche/AddOrEdit";
    var deleteUrl = "api/AfficheHelp/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '标题', '所属类别', '所属小类', '是否首页显示', '是否置顶', '时间', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 100, align: "center", hidden: true },
            { name: 'Title', index: 'Title', width: 150, align: "center" },
            { name: 'DataTypeName', index: 'DataTypeName', width: 80, align: "center" },
            { name: 'Name', index: '', width: 80, align: "center" },
            { name: 'IsIndex', index: 'IsIndex', width: 50, align: "center", formatter: topevery.isYesOrNo },
            { name: 'IsTop', index: 'IsTop', width: 50, align: "center", formatter: topevery.isYesOrNo },
            { name: 'AddTime', index: 'AddTime', width: 70, align: "center" },
             { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddSkip(addUrl);
    $(".edit_btn").bindEditSkip(addUrl, grid);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    ///获取所属列表
    topevery.BindSelect("QueryBigClassId", "Common/GetBigClassIdList?Code=zxgl", "--全部--");
    $("#QueryBigClassId").change(function () {
        topevery.BindSelect("QueryClassId", "Common/DataDictionary?dataTypeId=" + $("#QueryBigClassId").val(), "--全部--");
    });
});