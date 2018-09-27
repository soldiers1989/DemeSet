
var PracticeNo = topevery.getQueryString("PracticeNo");
var postData = new Array();

$(document).ready(function () {
    //初始化
    topevery.ajax({
        url: "api/Economy/GetChapterQuestions?ChapterId=" + $("#ChapterId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            //$("#ChapterQuestionsId").val(data.Result.ChapterQuestionsId);
            //$("#ChapterTiele").html(data.Result.ChapterTiele + PracticeNo);
            //$("#DateTime").html(data.Result.DateTime);
            $("#CourseName").html(data.Result.CourseName);
            $("#ChapterName").html(data.Result.ChapterName);
            $("#Count").html("共" + data.Result.Count + "题");
            topevery.SetCookie("time" + $("#ChapterQuestionsId").val(), new Date().getTime());
            var endTime = getCookie("time" + $("#ChapterQuestionsId").val());
            var timer = setInterval(time, 1000);
            function time() {
                var curTime = new Date().getTime();
                var djs = curTime - endTime;
                if (djs >= 1000) {
                    var hours = parseInt((djs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    var minutes = parseInt((djs % (1000 * 60 * 60)) / (1000 * 60));
                    var seconds = parseInt((djs % (1000 * 60)) / 1000);
                    //$(".tn-time").html("所用时间：<strong id=\"minute_show\"><s></s>" + minutes + "</strong>:<strong id=\"second_show\"><s></s>" + seconds + "</strong>");
                    $(".tn-time").html(minutes + "：" + seconds);
                }
            }
        }
    });
    //初始化
    topevery.ajax({
        url: "api/Economy/GetChapterPracticeList?ChapterId=" + $("#ChapterId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            var html = "";
            var html3 = "";
            var html4 = "";
            var index = 1;
            var description = "";
            for (var i = 0; i < row.length; i++) {
                html += '<h4 class="ud-questionTypes">' + row[i].SubjectName + '</h4>';
                html4 = "";
                for (var j = 0; j < row[i].ChapterPracticeOutput.length; j++) {
                    var div = '<div class="box" style="width: 100%;">';
                    var div1 = '</div>';
                    postData.push(row[i].ChapterPracticeOutput[j]);
                    var html2 = "";
                    html4 += '<a  class="' + row[i].ChapterPracticeOutput[j].Id + '" data="' + row[i].ChapterPracticeOutput[j].Id + '">' + index + '</a>';
                    index++;
                    for (var h = 0; h < row[i].ChapterPracticeOutput[j].option.length; h++) {
                        html2 += '<div class="col-xs-12 inputType">' + '<input name="answer_' + row[i].ChapterPracticeOutput[j].Id + '" class="answer_input" type="' + row[i].ChapterPracticeOutput[j].input_type + '" value="' + row[i].ChapterPracticeOutput[j].option[h].id + '" id="' + row[i].ChapterPracticeOutput[j].Id + '_' + row[i].ChapterPracticeOutput[j].option[h].id + '" />' +
                            '<label for="' + row[i].ChapterPracticeOutput[j].Id + '_' + row[i].ChapterPracticeOutput[j].option[h].id + '">' + row[i].ChapterPracticeOutput[j].option[h].content + '</label></div>';
                    }
                    if (row[i].ChapterPracticeOutput[j].Description) {
                        if (j > 0) {
                            if (row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j - 1].Description) {
                                description = "";
                            } else {
                                description = row[i].ChapterPracticeOutput[j].Description;
                            }
                        } else {
                            description = row[i].ChapterPracticeOutput[j].Description;
                        }
                        if (j === 0 && row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j + 1].Description) {
                            div1 = "";
                        } else if (j > 0 && j < row[i].ChapterPracticeOutput.length - 1) {
                            //大题分析等于前一项也等于后一项  
                            if (row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j - 1].Description && row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j + 1].Description) {
                                div = "";
                                div1 = "";
                            }//大题分析等于前一项不等等于后一项  
                            else if (row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j - 1].Description && row[i].ChapterPracticeOutput[j].Description !== row[i].ChapterPracticeOutput[j + 1].Description) {
                                div = "";
                            }//大题分析等于后一项   不等于前一项
                            else if (row[i].ChapterPracticeOutput[j].Description !== row[i].ChapterPracticeOutput[j - 1].Description && row[i].ChapterPracticeOutput[j].Description === row[i].ChapterPracticeOutput[j + 1].Description) {
                                div1 = "";
                            }
                        } else if (j > 0 && j === row[i].ChapterPracticeOutput.length - 1) {
                            div = "";
                        }
                    }


                    var questioncontent = row[i].ChapterPracticeOutput[j].QuestionContent;
                    if (questioncontent.indexOf("<p>") == -1) {
                        questioncontent = (index - 1) + "、" + questioncontent;
                    } else {
                        questioncontent = questioncontent.replace(/<p>/, "<p>" + (index - 1) + "、");
                    }

                    html += div +
                        '<p class="editor">' + description + '</p>' +
                        '<div style="float: left;font-size: 18px;"  Id="l_' + row[i].ChapterPracticeOutput[j].Id + '"><span></span>' + questioncontent + '</div>' +
                        '<div class="col-xs-12" DataType="' + row[i].ChapterPracticeOutput[j].Type + '"  Id="' + row[i].ChapterPracticeOutput[j].Id + '">' + html2 +
                        '</div>' + div1;
                }
                html3 += ' <p class="tn-ordinal-num" id=""><span>' + row[i].SubjectName + '</span></p>' +
                    '<p class="topic">' + html4 +
                    '</p>';
            }
            $('#tn-ordinal').html(html3);
            $('#ChapterPractice').html(html);
            /*$("#ChapterPractice .inputType").click(function () {
                $("." + $(this).parent().attr("Id")).addClass("done-high");
            });*/
            $(document).on("change", "#ChapterPractice .answer_input", function () {
                if ($(this).parent().parent().find(".answer_input:checked").length > 0) {
                    $("." + $(this).parent().parent().attr("Id")).addClass("done-high");
                } else {
                    $("." + $(this).parent().parent().attr("Id")).removeClass("done-high");
                }
            })
            $("#tn-ordinal a").bind("click", function () {
                var id = $(this).attr("data");
                $('html, body').animate({ scrollTop: ($('#l_' + id).offset().top - 110) }, 100);
            });
        }
    });

    $("#Toview").bind("click", function () {
        topevery.ajaxToThis({ type: "get", url: "/Answer/ChapterPracticeList", dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "练习记录",
                skin: 'layui-layer-rim', //加上边框
                area: [600 + 'px', 400 + 'px'], //宽高
                content: data
            });
        }, true);
    });
});

function savePaper() {
    var data = new Array();
    for (var i = 0; i < postData.length; i++) {
        var row = new Object();
        row.QuestionId = postData[i].Id;
        var answer = '';
        if (postData[i].SubjectType === 2) {
            $("#" + postData[i].Id).find("input[type='checkbox']:checked").each(function () {
                answer += $(this).val() + ',';
            });
            answer = answer.substring(0, answer.length - 1);
        } else {
            answer = $("#" + postData[i].Id).find("input[type='radio']:checked").val() == undefined ? "" : $("#" + postData[i].Id).find("input[type='radio']:checked").val();
        }
        row.StuAnswer = answer;
        row.Type = postData[i].Type;
        data.push(row);
    }
    topevery.ajax({
        url: "api/Economy/InsertChapterQuestionsDetail",
        type: "Post",
        data: JSON.stringify({ ChapterQuestionsId: $("#ChapterQuestionsId").val(), CourseChapterQuestionsDetail: data })
    }, function (data) {
        if (data.Result) {
            location.href = "/Answer/ChapterPracticeView?ChapterQuestionsId=" + $("#ChapterQuestionsId").val() + "&ChapterId=" + $("#ChapterId").val();
        } else {
            layer.msg("请勿重复提交数据");
        }
    });
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
