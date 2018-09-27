$(function () {
    var getPostDataUrl = "api/UseDescription/GetList";
    var addUrl = "UseDescription/AddOrEdit";
    var deleteUrl = "api/UseDescription/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '所属类别', '所属小类', '时间',''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'BigClassName', index: '', width: 100, align: "center" },
            { name: 'ClassName', index: '', width: 100, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center" },
             { name: '', index: '', width: 400, align: "center" },
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
    topevery.BindSelect("QueryBigClassId", "Common/GetBigClassIdList?Code=zllb", "--全部--");
    $("#QueryBigClassId").change(function () {
        topevery.BindSelect("QueryClassId", "Common/DataDictionary?dataTypeId=" + $("#QueryBigClassId").val(), "--全部--");
    });
});