var children;
var chapterId = "";
///初始化
$(function () {
    var courseId = $('#CourseIdParam').val();
    var videoId = "";
    topevery.BindTree("videoChapterTree", "Common/CourseChapterTree?CourseId=" + courseId, beforeClick, "");

    function beforeClick(data) {
        $('#addVideo,#UpdateVideoTasteLongTime').removeAttr("disabled");
        parentId = chapterId = data.id;
        $('#ChapterIdParam').val("" + chapterId);
        children = data.children;
        CourseVideo();
        //显示视频列表数据
    };

});


function CourseVideo() {
    topevery.ajax({ url: "api/CourseVideo/GetList", data: JSON.stringify({ ChapterId: chapterId }) }, function (data) {
        var videoHtml = "";
        $('#videoList').html("");
        var arr = data.Result;
        if (arr != null && arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                var sthtml = "";
                if (arr[i].IsTaste === 1) {
                    if (arr[i].TasteLongTime > 0) {
                        sthtml = "可试听:" + formatterTasteLongTime(arr[i].TasteLongTime);
                    } else {
                        sthtml = "可试听全部时长)";
                    }
                } else if (arr[i].IsTaste === 0) {
                    sthtml = "不能试听)";
                }
                videoHtml += "<li>" + arr[i].VideoName + "(时长:" + arr[i].VideoLongTime + "," + sthtml + " &nbsp;&nbsp;&nbsp;<a class='glyphicon glyphicon-remove' data-id='" + arr[i].Id + "' style=\"color: red; cursor: pointer;\"></a><a class='glyphicon glyphicon-pencil edit_video' data-id='" + arr[i].Id + "' style=\"color: green; cursor: pointer;\"></a></li>";
            }
            $(videoHtml).appendTo($('#videoList'));
            //绑定删除图标事件
            $(" .glyphicon-remove").each(function () {
                $(this).on('click', function () {
                    var id = $(this).data('id');
                    layer.confirm('确认删除?', { title: "确认" }, function () {
                        var delUrl = "api/CourseVideo/Delete";
                        topevery.ajax({ url: delUrl, data: JSON.stringify({ Ids: id }) }, function (data) {
                            var message = "删除失败";
                            if (data.Success) {
                                message = data.Result.Message;
                                topevery.ajax({ url: "api/CourseVideo/GetList", data: JSON.stringify({ ChapterId: chapterId }) }, function (data) {
                                    var videoHtml = "";
                                    $('#videoList').html("");
                                    var arr = data.Result;
                                    if (arr != null && arr.length > 0) {
                                        for (var i = 0; i < arr.length; i++) {

                                            videoHtml += "<li>" + arr[i].VideoName + "(时长:" + arr[i].VideoLongTime + "," + (arr[i].TasteLongTime > 0 ? "可试听:" + formatterTasteLongTime(arr[i].TasteLongTime) + ")" : "不能试听)") + "  &nbsp;&nbsp;&nbsp;<a class='glyphicon glyphicon-remove' data-id='" + arr[i].Id + "'  style=\"color: red; cursor: pointer;\"></a><a class='glyphicon glyphicon-pencil edit_video' data-id='" + arr[i].Id + "' style=\"color: green; cursor: pointer;\"></a></li>";
                                        }
                                        $(videoHtml).appendTo($('#videoList'));
                                    }
                                })
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



            $(".edit_video").each(function () {
                $(this).on('click', function () {
                    var id = $(this).data('id');
                    var url = "CourseVideo/AddOrEdit?ChapterId=" + $('#ChapterIdParam').val() + "&id=" + id;

                    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
                        editCourseVideoLayer = layer.open({
                            type: 1,
                            title: "修改视频信息",
                            skin: 'layui-layer-rim', //加上边框
                            area: [700 + 'px', 600 + 'px'], //宽高
                            content: data
                        });
                    }, true);
                })
            });
        }
    })

}

var editCourseVideoLayer;
var addCourseVideoLayer;
//跳转添加章节视频
$("#addVideo").on('click', function () {
    if (children != null && children.length > 0) {
        layer.alert("请选择章节最外层节点进行操作！");
        return;
    } else {
        //一个章节只允许上传一个视频
        //topevery.ajax({
        //    url: "api/CourseVideo/HasVideo",
        //    data: JSON.stringify({ ChapterId: $('#ChapterIdParam').val() })
        //}, function (data) {
        //    if (data.Result) {
        //        layer.msg("该章节已经维护有视频信息！");
        //    } else {
        var url = "CourseVideo/AddOrEdit?ChapterId=" + $('#ChapterIdParam').val();
        addCourseVideoLayer = topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "添加章节视频",
                skin: 'layui-layer-rim', //加上边框
                area: [900 + 'px', 600 + 'px'], //宽高
                content: data
            });
        }, true);
    }
    //})
    //}
})


//跳转添加章节视频
$("#UpdateVideoTasteLongTime").on('click', function () {
    if (children != null && children.length > 0) {
        layer.alert("请选择章节最外层节点进行操作！");
        return;
    } else {
        var url = "CourseVideo/TasteLongTimeView?ChapterId=" + $('#ChapterIdParam').val();
        addCourseVideoLayer = topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "设置试听时长(秒)",
                skin: 'layui-layer-rim', //加上边框
                area: [320 + 'px', 180 + 'px'], //宽高
                content: data
            });
        }, true);
    }
});


function formatterTasteLongTime(longtime) {
    var value = parseInt(longtime);
    if (value > 0 && value < 1 * 60 * 60) {
        var minute = Math.floor(value / 60);
        var second = value % 60;
        if (minute > 0) {
            if (second > 0) {
                return minute + "分" + second + "秒";
            } else {
                return minute + "分钟"
            }
        } else {
            if (second > 0) {
                return second + "秒";
            }
            return 0;
        }
    }
    return 0;
}