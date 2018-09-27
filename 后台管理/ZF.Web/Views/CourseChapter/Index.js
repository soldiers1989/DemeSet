
var parentId = "";

var courseId = $('#CourseIdParam').val();
var chapterId = "";
var dataLevel = 0;
$(function () {
    //加载课程章节树
    var children ;
    topevery.BindTree("chapterTree", "Common/CourseChapterTree?CourseId=" + courseId, beforeClick, "");

    function beforeClick(data) {
        parentId=chapterId = data.id;
        children = data.children;
        //确定该章节节点为几级节点
        dataLevel = parseInt(data.level);
        console.log('clickTree', parentId);
    }

    $('.del_btn').on('click', function () {
        console.log('delete',parentId);
        if (chapterId != "") {
            if (children != null && children.length > 0) {
                layer.alert("请选择最外层节点进行操作！");
                return;
            }
            layer.confirm('确认删除?', { title: "确认" }, function () {
                var delUrl = "api/CourseChapter/Delete";
                topevery.ajax({ url: delUrl, data: JSON.stringify({ Ids: chapterId }) }, function (data) {
                    var message = "删除失败";
                    if (data.Success) {
                        message = data.Result.Message;
                        //重新加载树
                        //parentId = "";
                        //topevery.BindTree("chapterTree", "Common/CourseChapterTree?CourseId=" + courseId, beforeClick, "");
                        $('#panel-chapter').html('');
                        topevery.ajax({ type: "get", url: "CourseChapter/Index?courseId=" + courseId, dataType: "html" }, function (data) {
                            $(data).appendTo($('#panel-chapter'));

                            $('#tab_courseChapter').tab('show');
                        }, true);
                    }
                    layer.msg(message, {
                        icon: 1,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 5
                    });
                });
            })

        } else {
            layer.alert("请先选择章节树的节点再进行操作！");
        }
    });
})
var editChapterLayer;
var addChapterLayer;
//跳转添加课程章节
$("#addChapter").on('click', function () {
    if (parentId != "") {
        //只允许创建二级章节 0:一级 1：二级
        if (dataLevel > 0) {
            layer.alert("章节节点最多只允许创建二级节点");
            return;
        }
        //判断是否已经维护章节视频，若已经维护有视频，则提示不允许添加子章节
        topevery.ajax({
            url: 'api/CourseVideo/HasVideo',
            data: JSON.stringify({ ChapterId: parentId })
        }, function (data) {
            if (data.Result) {
                layer.msg("该节点已维护有章节视频，不能添加子节点!", { time: 3000 });
                return;
            } else {
                var url = "CourseChapter/AddCourseChapter?CourseId=" + courseId + "&ParentId=" + parentId;
                topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
                    addChapterLayer = layer.open({
                        type: 1,
                        title: "添加课程章节",
                        skin: 'layui-layer-rim', //加上边框
                        area: [600 + 'px', 400 + 'px'], //宽高
                        content: data
                    });
                }, true);
            }
        });
    } else {
        var url = "CourseChapter/AddCourseChapter?CourseId=" + courseId + "&ParentId=" + parentId;
        topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
            addChapterLayer = layer.open({
                type: 1,
                title: "添加课程章节",
                skin: 'layui-layer-rim', //加上边框
                area: [600 + 'px', 400 + 'px'], //宽高
                content: data
            });
        }, true);
    }
})

$('#eidtChatper').on('click', function () {
    var url = "CourseChapter/EditCourseChapter?chapterid=" + chapterId + "&courseid=" + courseId;
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
        editChapterLayer = layer.open({
            type: 1,
            title: "编辑",
            skin: 'layui-layer-rim', //加上边框
            area: [600 + 'px', 400 + 'px'], //宽高
            content: data
        });
    }, true);
})
