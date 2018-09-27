$(function () {
    var getPostDataUrl = "api/SubjectSmallquestion/GetList";
    var editUrl = "SubjectSmallquestion/AddOrEdit";
    var deleteUrl = "api/SubjectSmallquestion/Delete";
    var grid = $("#tblData1");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: false,
        pager: "#pager1",
        height:"430",
        colNames: ['序号', '试题标题', '试题内容', '试题类型', '正确答案', '参考答案', '试题状态', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'QuestionTitle', index: 'QuestionTitle', width: 300, align: "left" },
            { name: 'QuestionContent', index: 'QuestionContent', width: 80, align: "center", hidden: true },
            { name: 'SubjectType', index: 'SubjectType', width: 80, align: "center", formatter: topevery.SubjectType },
            { name: 'RightAnswer', index: 'RightAnswer', width: 100, align: "center", formatter: topevery.RightAnswer, hidden: true },
            { name: 'ConsultAnswer', index: 'ConsultAnswer', width: 100, align: "center", hidden: true },
            { name: 'State', index: 'State', width: 100, align: "center", formatter: topevery.State },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom1")
    });
    topevery.BindSelect("QuerySmallquestionSubjectType", "Common/QuestionTypeRemoveSeven", "--全部--");

    $(".add_btn1").on("click", function () {
        var addUrl = "SubjectSmallquestion/AddOrEdit?BigQuestionId=" + $("#QueryBigQuestionId").val();
        topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "新增",
                skin: 'layui-layer-rim', //加上边框
                area: [1000 + 'px', 650 + 'px'], //宽高
                content: data
            });
        }, true);
    });

    $(".edit_btn1").bindEditBtnDx(editUrl, grid, 1000, 650);
    $(".del_btn1").bindDelBtnDx(deleteUrl, grid);
    $(".query_btn1").on("click", function () {
        $("#tblData1").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom1")
        }).trigger("reloadGrid");
    });
});

