///初始化
$(function () {
    var parentId = "";
    var chapterId = "";
    Initialize();
    //加载课程资源列表
    function Initialize() {
        $('#resList').tab('show');
        $("#resourseList").html("");
        topevery.ajax({
            url: "api/CourseResource/GetList",
            data: JSON.stringify({ CourseId: $('#QueryCourseId').val() }),
        }, function (data) {
            var length = data.Result.length;
            var contentHtml = "";
            if (length > 0) {
                var arr = data.Result;
                for (var i = 0; i < length; i++) {
                    contentHtml+="<tr><th>"+arr[i].ResourceName+"</th>"
                        +"<th>"+arr[i].ResourceSize+"</th>"
                        +"<th>"+arr[i].ResourceUrl+"</th>"
                        + "<th><a  class=\"delete_btn\"   data-id=\"" + arr[i].Id + "\" style=\"cursor:pointer\">删除</a></th>"+
                    "</tr>";

                }
            }
            $(contentHtml).appendTo($("#resourseList"));

            $('.delete_btn').each(function () {
                $(this).on('click', function () {

                    var courseId = $(this).data("id");
                    console.log(courseId);
                    layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
                        topevery.ajax({
                            url: "api/CourseResource/Delete",
                            data: JSON.stringify({ "Ids": courseId })
                        }, function (dataObj) {
                            console.log(dataObj);
                            var message = "删除失败";
                            if (dataObj.Success) {
                                if (dataObj.Result.Success) {
                                    Initialize();
                                }
                                message = dataObj.Result.Message;
                            } else {
                                message = dataObj.Error;
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
                });
            });
        });
    }

    var courseId = $("#QueryCourseId").val();
    $("#addResource").on('click', function () {
        var url = "CourseInfo/AddCourseResource" + "?CourseId=" + $("#QueryCourseId").val();
        topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "添加课程资源",
                skin: 'layui-layer-rim', //加上边框
                area: [600 + 'px', 400 + 'px'], //宽高
                content: data
            });
        }, true);
    });


    function jumpToAddPage(url,length,height,title) {
        topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title:title,
                skin: 'layui-layer-rim', //加上边框
                area: [length + 'px', height + 'px'], //宽高
                content: data
            });
        }, true);
    }

    //跳转添加课程章节
    $("#addChapter").on('click', function () {
        if (parentId != "") {
            var url = "CourseChapter/AddCourseChapter?CourseId=" + courseId+"&ParentId=" + parentId;
            jumpToAddPage(url,600,400, "添加课程章节");
        } else {
            layer.confirm("未选择章节父节点,是否创建一级章节?",{title:"确认"}, function () {
                var url = "CourseChapter/AddCourseChapter?CourseId=" + courseId + "&ParentId=" + parentId;
                jumpToAddPage(url,600,400,"添加课程章节");
            });
        }
    })


    //跳转到添加课程章节视频页面
    $('#addVideo').on('click', function () {
        if (chapterId != "") {
            var url = "CourseVideo/AddCourseVideo?ChapterId=" + chapterId;
            jumpToAddPage(url, 900, 500, "添加课程章节视频");
        } else {
            layer.alert("请先选择章节树的某一节点再进行操作");
        }
    });

    topevery.BindTree("chapterTree", "Common/CourseChapterTree?CourseId="+courseId, beforeClick, "");
    function beforeClick(data) {
        //获取章节父节点Id
        chapterId = parentId = data.id;

        $('#videoList').html('');//清空
        //点击课程章节节点，加载视频列表
        topevery.ajax({ url: "api/CourseVideo/GetList", data: JSON.stringify({ChapterId:chapterId}) }, function (data) {
            
            var videoListHtml = "<ul>";
            var arr = data.Result.Rows;
            console.log(arr);
            if (arr.length > 0) {
                for (var i = 0; i < arr.length;i++) {
                    videoListHtml += "<li>" + arr[i].VideoName + "(时长:" + arr[i].VideoLongTime + "分钟)";
                }
            }
            videoListHtml += "</ul>";
            $(videoListHtml).appendTo($('#videoList'));
        });
    }


    
});
