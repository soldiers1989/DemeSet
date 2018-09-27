function setSmallquestion(id) {
    var addUrl = "SubjectSmallquestion/Index?BigQuestionId=" + id;
    topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "小题列表",
            skin: 'layui-layer-rim', //加上边框
            area: [1200 + 'px', 650 + 'px'], //宽高
            content: data
        });
    }, true);
}
$(function () {
    var header = $(".content-wrapper").outerHeight(true)
                      - $(".nav-header").outerHeight(true) - 3
                      - $(".content-header").outerHeight(true)
                      - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                      - 31;
    $(".KnowledgePointTree").height(header);
    var getPostDataUrl = "api/SubjectBigQuestion/GetList";
    var editUrl = "SubjectBigQuestion/AddOrEdit";
    var deleteUrl = "api/SubjectBigQuestion/Delete";
    var grid = $("#tblData");
    var subjectId = "";
    var dataType = false;
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: false,
        rowNum: 10,
        colNames: ['序号', '试题标题', '试题内容', '所属科目', '试题所属知识点', '题型', '难度等级', '正确答案', '参考答案', '试题状态', '正确答案', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'QuestionTitle', index: 'QuestionTitle', width: 390, align: "left" },
            { name: 'QuestionContent', index: 'QuestionContent', width: 100, align: "left", hidden: true },
            { name: 'SubjectName', index: 'SubjectName', width: 80, align: "center" },
            { name: 'KnowledgePointName', index: 'KnowledgePointName', width: 80, align: "center" },
            { name: 'ClassName', index: 'SubjectClassId', width: 70, align: "center" },
             { name: 'DifficultLevel', index: 'DifficultLevel', width: 50, align: "center", formatter: topevery.DifficultLevel },
            { name: 'RightAnswer', index: 'RightAnswer', width: 100, align: "center", hidden: true },
            { name: 'ConsultAnswer', index: 'ConsultAnswer', width: 100, align: "center", hidden: true },
            { name: 'State', index: 'State', width: 60, align: "center", formatter: topevery.State },
            {
                name: 'RightAnswer', index: 'RightAnswer', width: 100, align: "center", formatter: function (a, b, c) {
                    if (a) {
                        return a.replace("1", "A").replace("2", "B").replace("3", "C").replace("4", "D").replace("5", "E").replace("6", "F").replace("7", "G").replace("8", "H");
                    }
                    return a;
                }
            },
             {
                 name: '', index: '', width: 70, align: "center", formatter: function (a, b, c) {
                     if (c.SubjectType === 7) {
                         return "<a onclick=\"setSmallquestion('" + c.Id + "')\">设置小题</a>";
                     }
                     return "";
                 }
             },
              { name: '', index: '', width: 100, align: "center", formatter: topevery.State },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    topevery.BindSelect("QueryDifficultLevel", "Common/DifficultLevelList", "--全部--");
    $(".add_btn").on("click", function () {
        if ($("#QueryLevelId").val()) {
            var addUrl = "SubjectBigQuestion/AddOrEdit?SubjectId=" + $("#QuerySubjectIdTree").val() + "&KnowledgePointId=" + $("#QueryKnowledgePointId").val();
            topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: "新增",
                    skin: 'layui-layer-rim', //加上边框
                    area: [1200 + 'px', 650 + 'px'], //宽高
                    content: data
                });
            }, true);
        } else {
            layer.alert("请先选择知识点!");
        }
    });

    $(".edit_btn").bindEditBtnDx(editUrl, grid, 1200, 650);
    $(".del_btn").bindDelBtnDx(deleteUrl, grid, function (rowData) {
        var id = rowData.Id;
        var flag = true;
        topevery.ajax({
            url: 'api/SubjectBigQuestion/IfUse',
            data: JSON.stringify({ Id: id }),
            async: false
        }, function (data) {
            if (data.Result) {
                layer.alert("该试题已被使用,不能删除", 2);
                flag = false;
            }
        });
        return flag;
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    topevery.BindTree("treeDemo1", "Common/SubjectKnowledgePointTree", beforeClick, "", "", "", 1);
    topevery.BindSelect("QuerySubjectId", "Common/SubjectList", "--全部--");
    function beforeClick(data) {
        $("#QueryProjectClassId").val("");
        $("#QueryProjectId").val("");
        if (data.type === "4" || data.type === "5") {
            subjectId = data.subjectId;
            $("#QuerySubjectIdTree").val(subjectId);
            $("#QuerySubjectId").val(data.subjectId);
            $("#QueryKnowledgePointId").val(data.id);
            $("#QueryLevelId").val(data.level);

            topevery.BindSelect("QuerySubjectClassId", "Common/SubjectClassList?SubjectId=" + subjectId, "--请选择--");

            $(".query_btn").click();
        } else if (data.type === "3") {
            $("#QuerySubjectId").val(data.subjectId);
            $("#QueryKnowledgePointId").val("");
            $("#QueryLevelId").val("");

            topevery.BindSelect("SubjectClassId", "Common/SubjectClassList?SubjectId=" + subjectId, "--请选择--");
            $(".query_btn").click();
        } else {
            if (data.type === "1") {
                $("#QueryProjectClassId").val(data.id);
            } else if (data.type === "2") {
                $("#QueryProjectId").val(data.id);
            }
            subjectId = "";
            $("#QuerySubjectId").val("");
            $("#QuerySubjectIdTree").val("");
            $("#QueryKnowledgePointId").val("");
            $("#QueryLevelId").val("");
            $(".query_btn").click();
        }
    }
});

