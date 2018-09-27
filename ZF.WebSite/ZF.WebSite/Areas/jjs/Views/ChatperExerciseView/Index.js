$(function () {
    //章节练习主信息
    GetChapterQuestions();
    //试题列表
    GetChapterQuestionList();

    ///上一题
    $('.al-hd01').click(function () {
        if (pageIndex > 1) {
            pageIndex -= 1;
            GetChapterQuestionList();
        } else {
            layer.msg("已经是第一页！");
        }
    });
    //下一题
    $('.al-hd03').click(function () {
        var total = parseInt($('#total').text());
        if (pageIndex < total) {
            pageIndex += 1;
            GetChapterQuestionList();
        } else {
            layer.msg("已经是最后一页！");
        }
    });

})

function GetChapterQuestions() {
    topevery.ajax({
        url: "api/Economy/GetChapterQuestions?ChapterId=" + $("#ChapterId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            //$("#CourseName").html(data.Result.CourseName);
            //$("#ChapterName").html(data.Result.ChapterName);
            //$("#Count").html("共" + data.Result.Count + "题");
            //topevery.SetCookie("time" + $("#ChapterQuestionsId").val(), new Date().getTime());
            //var endTime = getCookie("time" + $("#ChapterQuestionsId").val());
            //var timer = setInterval(time, 1000);
            //function time() {
            //    var curTime = new Date().getTime();
            //    var djs = curTime - endTime;
            //    if (djs >= 1000) {
            //        var hours = parseInt((djs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            //        var minutes = parseInt((djs % (1000 * 60 * 60)) / (1000 * 60));
            //        var seconds = parseInt((djs % (1000 * 60)) / 1000);
            //        //$(".tn-time").html("所用时间：<strong id=\"minute_show\"><s></s>" + minutes + "</strong>:<strong id=\"second_show\"><s></s>" + seconds + "</strong>");
            //        //$(".tn-time").html(minutes + "：" + seconds);
            //    }
            //}
        }
    })
}

var pageIndex = 1;
function GetChapterQuestionList() {
    var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    topevery.ajaxwx({
        url: "api/Economy/GetChapterQuestionViewListByPage",
        data: JSON.stringify({ Page: pageIndex, Rows: 1, ChapterId: $('#ChapterId').val() })
    }, function (data) {
        if (data.Success) {
            var arr = data.Result.Rows;
            $('#total').text(data.Result.Records);
            $('.red').text(pageIndex);
            var contentHtml = "";
            for (var i = 0; i < arr.length; i++) {

                $('#subjectType').text(formatterSubjectType(arr[i].SubjectType));

                if (arr[i].SubjectType == 7 && arr[i].SubjectSmallquestions && arr[i].SubjectSmallquestions.length > 0) {
                    var caseContentHtml = "";
                    var subContent = "";
                    var sublist = arr[i].SubjectSmallquestions;
                    for (var k = 0; k < sublist.length; k++) {
                        var subOptionHtml = "";
                        var subOptionCount = parseInt(sublist[k].Number);
                        for (var j = 0; j < subOptionCount; j++) {
                            subOptionHtml += "<li><span>" + formatterLabel(azstr.substr(j, 1) + "</span>" + eval("sublist[k].Option" + (j + 1))) + "</li>";
                        }
                        subContent += "<div class=\"line_3px\"></div>"
                                       + "<div class=\"alfx-list03\">"
                                            + (k + 1) + "、" + formatterLabel(sublist[k].QuestionContent)
                                       + "</div>"
                                       + "<div class=\"line_3px\"></div>"
                                       + "<div class=\"alfx-list04\">"
                                    + "<ul>"
                                        + subOptionHtml
                                    + "</ul>"
                                + "</div>";
                    }
                    caseContentHtml += "<div class=\"alfx-list02\">"
                                    + "<p>" + arr[i].QuestionContent + "</p>"
                                    + "<div class=\"list02-tubiao\">"
                                        + "<span class=\"l-tb01\">标记</span>"
                                        + "<span class=\"l-tb02\">收藏</span>"
                                        + "<span class=\"l-tb03\">纠错</span>"
                                    + "</div>"
                                    + "<div class=\"clear\"></div>"
                                + "</div>"
                                + subContent;
                    contentHtml += caseContentHtml;
                    continue;
                }
                var number = parseInt(arr[i].Number);
                var optionHtml = "";

                for (var j = 0; j < number; j++) {
                    optionHtml += "<li isSelected=\"0\" data=\"" + (j + 1) + "\"></span>" + formatterLabel(azstr.substr(j, 1) + "</span>" + eval("arr[i].Option" + (j + 1))) + "</li>";
                }
                contentHtml += "<div class=\"alfx-list02\">"
                                    + "<p>" + arr[i].QuestionContent + "</p>"
                                    + "<div class=\"list02-tubiao\">"
                                        + "<span class=\"l-tb01\">标记</span>"
                                        + "<span class=\"l-tb02\">收藏</span>"
                                        + "<span class=\"l-tb03\">纠错</span>"
                                    + "</div>"
                                    + "<div class=\"clear\"></div>"
                                + "</div>"
                                + "<div class=\"line_3px\"></div>"
                                + "<div class=\"alfx-list04\">"
                                    + "<ul>"
                                        + optionHtml
                                    + "</ul>"
                                + "</div>";
            }
            $('.alfx-box').html(contentHtml);
            BindSelectOption();
        }
    });
}

function formatterLabel(content) {
    return content.replace(/<p>/ig, '').replace(/<\/p>/ig, '').replace(/<\/?span[^>]*>/ig, '');
}

//题型类型转成题型名
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
        default: "";
    }
}

function BindSelectOption() {
    $("ul li").click(function () {
        var isSelected = parseInt($(this).attr("isSelected"));
        if (isSelected == 0) {
            $(this).addClass("alfxbg-red");
            $(this).attr("isSelected", "1")
        } else {
            $(this).removeClass("alfxbg-red");
            $(this).attr("isSelected", "0")
        }
    })
}

