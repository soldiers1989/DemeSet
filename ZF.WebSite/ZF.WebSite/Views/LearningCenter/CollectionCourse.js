
GetCollectedCourse();

$('#search_collection').click(function () {
    GetCollectedCourse();
});




function GetCollectedCourse() {
    topevery.ajax({
        url: 'api/MyCollection/GetList',
        data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), CourseName: $(".search input").val() })
    }, function (data) {
        if (data.Success) {
            $('#collectedCourse').html(template("collectedCourse_html", data.Result));
            if (data.Result.Records > 0) {
                $('.M-box1').show();
                $('.M-box1').pagination({
                    pageCount: Math.ceil(data.Result.Records / parseInt($("#Rows").val())),
                    jump: true,
                    coping: true,
                    homePage: '首页',
                    endPage: '末页',
                    prevContent: '上页',
                    nextContent: '下页',
                    callback: function (api) {
                        $("#PageIndex").val(api.getCurrent());
                        GetCollectedCourse1();
                    }
                });
            } else {
                $('.M-box1').hide();
            }
        }
    });
}

function GetCollectedCourse1() {
    topevery.ajax({
        url: 'api/MyCollection/GetList',
        data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), CourseName: $(".search input").val() })
    }, function (data) {
        if (data.Success) {
            $('#collectedCourse').html(template("collectedCourse_html", data.Result));
        }
    });
}


template.helper('formatterDate', function (datetime) {
    if (datetime) {
        return datetime.split(' ')[0];
    }
})


function jumpToVideo(event) {
    var videoId = $(event).attr("VideoId");
    //通过视频ID查询课程章节以及课程
    topevery.ajax({
        url: "api/MyCollectionItem/GetVideoInfo?videoId=" + videoId,
    }, function (data) {
        if (data.Success) {
            var obj = data.Result
            var url = "/CourseInfo/ChapterVideoPlay?chapterId=" + obj.ChapterId + "&courseId=" + obj.CourseId + "&videoId=" + obj.VideoId;
            topevery.openClick(url);
        }
    })
}

function removeCollectedVideo(event) {
    var videoId = $(event).attr("VideoId");
    var courseId = $(event).attr("CourseId");
    layer.confirm("确认删除?", function (index) {
        topevery.ajax({
            url: 'api/MyCollection/CancelCollectVideo',
            data: JSON.stringify({ CourseId: courseId, VideoId: videoId })
        }, function (data) {
            if (data.Result.Success) {
                layer.close(index);
                GetCollectedCourse();
            }
        });
    })
}