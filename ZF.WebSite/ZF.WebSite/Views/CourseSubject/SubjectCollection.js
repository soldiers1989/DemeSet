$(function () {
    Initilize();
    var azstr="ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    function Initilize() {
        GetPageList();
    };

    $(".shoucan").click(function () {
        var data = $(this).attr("data");
        if (data == "1") {
            var id = $(this).data('collectionid');
            console.log(id);
            $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
            $(this).html("<i></i>收藏");
            $(this).attr("data", "0");
            topevery.ajax({
                url: 'api/MyCollectionItem/CancelCollectSubject',
                data: JSON.stringify({ Id: id })
            });
        } else if (data == "0") {
            var questionid = $(this).data('id');
            $(this).html("<i></i>取消收藏")
            $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
            $(this).attr("data", "1");
            topevery.ajax({
                url: 'api/MyCollectionItem/AddCollectSubject',
                data: JSON.stringify({ QuestionId: questionid })
            });



        }
    })
    $('.zkjx').click(function () {
        var jx = $(this).parent().parent().parent().siblings().next().next();
        $(jx).attr("style", "display:block;")
        $(this).attr('style', 'display:none;')
        $(this).siblings(":first").attr('style', 'display:block;')
    })
    $(".sqjx").click(function () {
        var jx = $(this).parent().parent().parent().siblings().next().next();
        $(jx).attr("style", "display:none;")
        $(this).attr('style', 'display:none;')
        $(this).siblings(":first").attr('style', 'display:block;')
    })
});

function GetPageList() {
    //加载我的试题
    $('#subjectlist').html('');
    topevery.ajax({
        url: 'api/CourseSubject/GetCollectedSubjectList',
        data: JSON.stringify({ Page: $('#PageIndex').val(), Rows: $('#Rows').val() })
    }, function (data) {
        if (data.Success) {
            var contentHtml = "";
            var arr = data.Result.Rows;
            for (var i = 0; i < arr.length; i++) {
                var optionHtml = "";
                var optionCount = parseInt(arr[i].Number);
                for (var j = 0; j < optionCount; j++) {
                    optionHtml += "<dd class=\"m-question-option cho-this\">"
                                       + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                       + eval("arr[i].Option" + (j + 1))
                                   + "</dd>"
                }

                contentHtml += "<li style=\"display: block;\">"
                                    + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                        + "<div class=\"subject-con\" style=\"background:inherit\">"
                                            + "<div class=\"sub-content sub-conanswer\">"
                                                + "<div class=\"sub-dotitle\">"
                                                     + "<em>" + (i + 1) + "</em>"
                                                     + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                     + arr[i].QuestionContent
                                                + "</div>"
                                                + "<dl class=\"sub-answer  sub-answer-no\">"
                                                         + optionHtml
                                                + "</dl>"
                                            + "</div>"
                                        + "</div>"
                                        + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                            + "<div class=\"reck\">"
                                                + "参考答案：<em class=\"right\">" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1) + "</em>"
                                            + "</div>"
                                            + "<ul>"
                                                + " <li class=\"nobro\">"
                                                + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                        + "<i></i>取消收藏"
                                                + "</a>"
                                                + "<li>"
                                                        + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                        + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                + "</li>"
                                            + "</ul>"
                                        + "</div>"
                                        //解析块
                                        + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                            + "<div class=\"solution\">"
                                                + "<ul>"
                                                    + "<li class=\"da-list\">"
                                                          + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                          + "<div class=\"so-timu\">"
                                                                + "<div class=\"wenzi\">"
                                                                    + "参考答案：" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1)
                                                                    + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                + "</div>"
                                                          + "</div>"
                                                    + "</li>"
                                                + "</ul>"
                                            + "</div>"
                                    + "</div>"
                              + "</li>"
            }
            $('#subjectlist').html(contentHtml);

            if (arr.length > 0) {
                $('#pageBox').show();
                $('#pageBox').pagination({
                    pageCount: Math.ceil(arr.length / parseInt($('#Rows').val())),
                    jump: true,
                    console: true,
                    homePage: '首页',
                    endPage: '末页',
                    prevContent: '上一页',
                    nexContent: '下一页',
                    callback: function (api) {
                        $("#PageIndex").val(api.getCurrent());
                        GetPageList2();
                    }
                });
            } else {
                $('#pageBox').hide();
               
            }
        }
    });
}


function GetPageList2() {
    topevery.ajax({
        url: 'api/CourseSubject/GetCollectedSubjectList',
        data: JSON.stringify({ Page: $('#PageIndex').val(),Rows:$('#Rows').val() })
    }, function (data) {
        if (data.Success) {
            var contentHtml = "";
            var arr = data.Result.Rows;
            for (var i = 0; i < arr.length; i++) {
                var optionHtml = "";
                var optionCount = parseInt(arr[i].Number);
                for (var j = 0; j < optionCount; j++) {
                    optionHtml += "<dd class=\"m-question-option cho-this\">"
                                       + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                       + eval("arr[i].Option" + (j + 1))
                                   + "</dd>"
                }

                contentHtml += "<li style=\"display: block;\">"
                                    + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                        + "<div class=\"subject-con\" style=\"background:inherit\">"
                                            + "<div class=\"sub-content sub-conanswer\">"
                                                + "<div class=\"sub-dotitle\">"
                                                     + "<em>" + (i + 1) + "</em>"
                                                     + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                     + arr[i].QuestionContent
                                                + "</div>"
                                                + "<dl class=\"sub-answer  sub-answer-no\">"
                                                         + optionHtml
                                                + "</dl>"
                                            + "</div>"
                                        + "</div>"
                                        + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                            + "<div class=\"reck\">"
                                                + "参考答案：<em class=\"right\">" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1) + "</em>"
                                            + "</div>"
                                            + "<ul>"
                                                + " <li class=\"nobro\">"
                                                + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                        + "<i></i>取消收藏"
                                                + "</a>"
                                                + "<li>"
                                                        + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                        + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                + "</li>"
                                            + "</ul>"
                                        + "</div>"
                                        //解析块
                                        + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                            + "<div class=\"solution\">"
                                                + "<ul>"
                                                    + "<li class=\"da-list\">"
                                                          + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                          + "<div class=\"so-timu\">"
                                                                + "<div class=\"wenzi\">"
                                                                    + "参考答案：" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1)
                                                                    + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                + "</div>"
                                                          + "</div>"
                                                    + "</li>"
                                                + "</ul>"
                                            + "</div>"
                                    + "</div>"
                              + "</li>"
            }
            $('#subjectlist').html(contentHtml);
        }
    });
}

function formatterSubjectType(subjectType) {
    switch (parseInt(subjectType)) {
        case 1:
            return "单选题";
        case 2:
            return "多选题";
        case 3:
            return "判断题";
        case 7:
            return "案例分析题";
        default:"";
    }
}