$(function () {
    var flag = false;
    var subjectId = $('#QSubjectId').val();
    var projectId = $('#QProjectId').val();
    var courseId = $('#QCourseId').val();
    var chapterId = $('#QChapterId').val();



    Init();
    function Init() {
        topevery.BindTree("treeDemo1", "Common/SubjectKnowledgePointLists?subjectId=" + subjectId, onClick2, onClick2, false, null, 0);
        topevery.BindSelect('QSubjectClassId', 'Common/SubjectClassList?subjectId=' + subjectId, '全部')
        topevery.BindSelect("QSubjectType", "Common/QuestionType", "全部");
        GetSelectQuestinList();
    }

    $('#querySelectQuestion').on('click', function () {
        GetSelectQuestinList();
    });

   
    function GetSelectQuestinList() {
        var knowlegePointIds = $('#knowledgeIds').val();
        var subjectClass = $('#QSubjectClassId').children('option:selected').val();
        var subjectType = $('#QSubjectType').children('option:selected').val();
        var subjectId = $('#QSubjectId').children('option:selected').val();

        var obj = JSON.stringify({CourseId:courseId, KnowledgePointId: knowlegePointIds, SubjectClassId: subjectClass, SubjectType: subjectType, SubjectId: subjectId });

        var grid = $('#questionList');
        var url = 'api/SubjectBigQuestion/GetBigQuestionExceptExistInChapter'
        if (!flag) {
            grid.jgridInit({
                url: url,
                colNames: ['试题编码', '试题标题', '难度等级', '使用次数',''],
                colModel: [
                   { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
                   { name: 'QuestionTitle', width: 80, align: "center" },
                   //{ name: 'QuestionContent', width: 150, align: "center" },
                   //{ name: 'RightAnswer', width: 40, align: "center" },
                   {
                       name: 'DifficultLevel', index: 'DifficultLevel', width: 40, align: "center", formatter: function (data) {

                           return data == 1 ? "简单" : data == 2 ? "容易" : data == 3 ? "困难" : "";
                       }
                   },
                   { name: 'UseTimes', width: 40, align: "center" },
                   { name: '', width: 200, align: "center" },
                ],
                postData: topevery.form2Json("selectQuestionFrom"),
                height: 'auto',
            });
            flag = true;
        } else {
            grid.jqGrid('setGridParam', {
                url: url, page: 1, postData: topevery.form2Json("selectQuestionFrom")
            }).trigger("reloadGrid");
        }
    }

    

    $('#addChapterSubject').on('click', function () {
        var ids = $('#questionList').jqGrid('getGridParam', 'selarrrow');
        console.log(ids);
        if (ids==undefined|| ids.length == 0) {
            layer.alert("请选择试题");
            return;
        }
        var arr=[];
        for (var i = 0; i < ids.length;i++) {
            var rowId = ids[i];
            var rowData = $('#questionList').jqGrid('getRowData', rowId);
            arr.push(rowData.Id);
        }
        layer.confirm('确认添加?', { title: "确认" }, function () {
            topevery.ajax({
                url: 'api/CourseSubject/AddCourseSubjectList?courseId=' + courseId + "&chapterId=" + chapterId + "&strQuestionIds=" + arr.join(','),
            }, function (data) {
                if (data.Result.Success) {
                    $(".layui-layer-close").click();//关闭窗口
                    //添加成功,加载数据
                    $('#selectedQuestionData').trigger("reloadGrid");
                }
            });
        })
    });
});





//获取当前选择节点及其所有子节点id,拼接成字符串
function getAllSubId(nodes) {
    var ids = '';
    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].children != null && nodes[i].children.length > 0) {
            ids += nodes[i].id + ",";
            var subNodes = nodes[i].children;
            ids += getAllSubId(subNodes);
        } else {
            ids += nodes[i].id + ",";
        }
    }
    return ids;
}


function onClick2(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo1"),
    nodes = zTree.getSelectedNodes(),
    v = "";
    nodes.sort(function compare(a, b) { return a.id - b.id; });
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
    }
    if (v.length > 0) v = v.substring(0, v.length - 1);
    var cityObj = $("#ParentKnoledgeId");
    cityObj.attr("value", v);
    var ids = getAllSubId(nodes);
    console.log(ids);
    $("#knowledgeIds").val("" + ids);
}

function showMenu2() {
    $("#menuContent1").slideDown("fast");
    $("body").bind("mousedown", onBodyDown2);
}

function hideMenu2() {
    $("#menuContent1").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown2);
}

function onBodyDown2(event) {
    if (!(event.target.id == "menuBtn" || event.target.id == "menuContent1" || $(event.target).parents("#menuContent1").length > 0)) {
        hideMenu2();
    }
}