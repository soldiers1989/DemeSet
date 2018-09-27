
var isok = false;
$(function () {
    var subjectId = $('#subjectId').val();
    var chapterId = "";
    Initialize();

    function Initialize() {

        topevery.BindSelect("QuerySubjectType", "Common/QuestionType", "全部");

        topevery.BindTree("courseChapterTree", "Common/GetChpaterVideoTree?CourseId=" + $('#courseId').val(), beforeClick, "");

        //根据科目动态绑定知识点树
        topevery.BindTree("treeDemo", "Common/SubjectKnowledgePointLists?subjectId=" + subjectId, onClick, onClick, false, null, 0);
        //根据项目动态绑定试题分类
        topevery.BindSelect('QuerySubjectClassId', 'Common/SubjectClassList?subjectId=' + subjectId, '全部')
        GetExistQuestionList();
    }


    $('.query_btn').on('click', function () {
        var courseId = $('#courseId').val();
        var chapterId = $('#QueryChapterId').val();

        GetExistQuestionList();
    });
    function beforeClick(data) {
        console.log(data);
        if (data.type == "2") {
            $('#QueryChapterId').val(data.id);
        }
        //if (data.children == null) {
        //    $('#QueryParentChapterId').val('');
        //    $('#QueryChapterId').val(data.id);
        //}else{
        //    $('#QueryParentChapterId').val('');
        //    $('#QueryChapterId').val(data.id);
        //}
        $('#chapterName').text(data.name);
            GetExistQuestionList();
    }


    $('.add_btn').on('click', function () {
        var courseId = $('#courseId').val();
        var projectId = $('#projectId').val();
        var subjectId = $('#subjectId').val();
        var chapterId = $('#QueryChapterId').val();

        if (chapterId == null || chapterId == undefined || chapterId == '') {
            layer.alert('请选择视频节点进行试题添加');
            return;
        }

        topevery.ajax({ type: 'get', url: 'CourseSubject/Add?projectId=' + projectId + '&subjectId=' + subjectId + '&courseId=' + courseId + "&chapterId=" + chapterId, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: '添加章节试题',
                skin: 'layui-layer-rim', //加上边框
                area: [900 + 'px',700 + 'px'], //宽高
                content: data
            });
        }, true)
    })
    $('#return_btn').on('click',function () {
        topevery.ajax({ url: "CourseInfo/CourseContent", type: 'get', dataType: 'html' }, function (data) {
            $('.content-wrapper').html(data);
        });
    });

});



function GetExistQuestionList() {
    var knowlegePointIds = $('#knowledgePointIds').val();
    var subjectClass = $('#QuerySubjectClassId').children('option:selected').val();//试题类型
    var subjectType = $('#QuerySubjectType').children('option:selected').val();
    var subjectId = $('#subjectId').val();
    var chapterId = $('#QueryChapterId').val();
    var parentId = $('#QueryParentChapterId').val();
    var obj = JSON.stringify({ KnowledgePointId: knowlegePointIds, SubjectClassId: subjectClass, SubjectType: subjectType, SubjectId: subjectId, ChapterId: chapterId, ParentId: parentId });

    var grid = $("#selectedQuestionData");

    var getPostDataUrl = "api/CourseSubject/GetExistBigQuestionWithSmallQuestion";
    if (!isok) {
        grid.jgridInit({
            url: getPostDataUrl,

            colNames: ['编码', '试题标题', '难度等级', '使用次数',''], //列头
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
                { name: 'UseTimes',index:'UseTimes', width: 40, align: "center" },
                { name: '', width: 200, align: "center" },
            ],
            postData: topevery.form2Json('selectExistQuestionFrom'),
            pager: '#pager1',
            height: '500'
        });
        isok = true;
    } else {
        grid.jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectExistQuestionFrom")
        }).trigger("reloadGrid");
    }
}


$(".remove_btn").bindDelBtn('api/CourseSubject/Delete', $("#selectedQuestionData"));






var arrSelectedQuestionId = [];

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

function onClick(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
    nodes = zTree.getSelectedNodes(),
    v = "";
    nodes.sort(function compare(a, b) { return a.id - b.id; });
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
    }
    if (v.length > 0) v = v.substring(0, v.length - 1);
    var cityObj = $("#ParentId");
    cityObj.attr("value", v);
    var ids = getAllSubId(nodes);
    console.log(ids);
    $("#knowledgePointIds").val("" + ids);
}

function showMenu() {
    $("#menuContent").slideDown("fast");
    $("body").bind("mousedown", onBodyDown);
}

function hideMenu() {
    $("#menuContent").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown);
}

function onBodyDown(event) {
    if (!(event.target.id == "menuBtn" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        hideMenu();
    }
}

