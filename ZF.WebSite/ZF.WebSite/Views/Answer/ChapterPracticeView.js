$(function () {
    topevery.ajax({
        url: "api/Economy/GetChapterQuestionsResult?ChapterQuestionsId=" + $("#ChapterQuestionsId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            $('#subjectlist').html(template("PracticeView_html", row));
            $('.zkjx').click(function () {
                var jx = $(this).parent().parent().parent().siblings().next().next();
                $(jx).attr("style", "display:block;");
                $(this).attr('style', 'display:none;');
                $(this).siblings(":first").attr('style', 'display:block;');
            });
            $(".sqjx").click(function () {
                var jx = $(this).parent().parent().parent().siblings().next().next();
                $(jx).attr("style", "display:none;");
                $(this).attr('style', 'display:none;');
                $(this).siblings(":first").attr('style', 'display:block;');
            });
            $(".shoucan").click(function () {
                var data = $(this).attr("data");
                var id;
                if (data === "1") {
                    id =$(this).attr('data-id');
                    $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
                    $(this).html("<i></i>收藏");
                    $(this).attr("data", "0");
                    topevery.ajax({
                        url: 'api/MyCollectionItem/CancelCollectSubject',
                        data: JSON.stringify({ Id: id })
                    });
                } else if (data === "0") {
                    id = $(this).data('id');
                    var that = $(this);
                    $(this).html("<i></i>取消收藏");
                    $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
                    $(this).attr("data", "1");
                    topevery.ajax({
                        url: 'api/MyCollectionItem/AddCollectSubject',
                        data: JSON.stringify({ QuestionId: id })
                    }, function (row) {
                        that.attr('data-id', row.Result.ModelId);
                    });
                }
            });
        }
    });

    ////初始化
    //topevery.ajax({
    //    url: "api/Economy/GetChapterPracticeViewList?ChapterQuestionsId=" + $("#ChapterQuestionsId").val(),
    //    type: "Post",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    if (data.Success) {
    //        var row = data.Result;
    //        var html3 = "";
    //        var html4 = "";
    //        var index = 1;
    //        html4 = "";
    //        for (var i = 0; i < row.length; i++) {
    //            html4 += '<a href="#' + row[i].Id + '" class="' + row[i].Id + '" data="' + row[i].Id + '">' + index + '</a>';
    //            index++;
    //        }
    //        html3 += ' <p class="tn-ordinal-num" id="" style="background: none;"></p>' +
    //            '<p id="" style="display: block;">' + html4 +
    //            '</p>';
    //        $('#tn-ordinal').html(html3);
    //    }
    //});
});
function savePaper() {
    var chapterId = $("#ChapterId").val();
    topevery.ajax({
        url: "api/Economy/InsertChapterQuestions?ChapterId=" + chapterId,
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            location.href = "/Answer/ChapterPractice?ChapterId=" + chapterId + "&chapterQuestionsId=" + data.Result.Id + "&PracticeNo=" + data.Result.PracticeNo;
        }
    });
}


function jumpToVideo(event) {
    console.log($(event));
    var videoId = $(event).attr("data");
    console.log(videoId);
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
