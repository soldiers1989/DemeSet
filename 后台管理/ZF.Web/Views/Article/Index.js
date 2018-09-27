$(function () {
    var getPostDataUrl = "api/Article/GetList";
    var addUrl = "Article/AddOrEdit";
    var deleteUrl = "api/Article/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '标题', '封面', '维护时间', '是否删除', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 100, align: "center", hidden: true },
            { name: 'ArticleTitle', index: 'ArticleTitle', width: 150, align: "center" },
            { name: 'ArticleImage', index: 'ArticleImage', width: 80, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 80, align: "center" },
            { name: 'IsDelete', index: 'IsDelete', width: 50, align: "center", formatter: function (value) { return parseInt(value)==1?"是":"否"} },
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

    $('input[type="radio"]').click(function () {
        $('#IsDelete').val($(this).val());
        $('.query_btn').click();
    })
});