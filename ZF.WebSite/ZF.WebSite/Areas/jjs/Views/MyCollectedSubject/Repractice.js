$(function () {
    GetCollectList();

    ///上一题
    $('.al-hd01').click(function () {
        addPostData();
        if (Index > 0) {
            Index -= 1;
            $("#currentQuestionId").val(resultArr[Index].Id);
            loadItem(Index);

        } else {
            layer.msg("已经是第一页！");
        }
    });
    //下一题
    $('.al-hd03').click(function () {
        addPostData();
        if (Index < total-1) {
            $("#currentQuestionId").val(resultArr[Index].Id);
            Index += 1;
            loadItem(Index);
            console.log(resultArr[Index-1]);
        } else {
            layer.msg("已经是最后一页！");
        }
    });
})


function addPostData() {
    var curentIndex = -1;
    var answer = "";
    var row = new Object();
    var subjectType = $('#subjectType').val()
    for (var i = 0; i < postData.length; i++) {
        if (subjectType == 7) {
            if (postData[i].SmallQuestionId === $('#currentQuestionId').val()) {
                curentIndex = i;
            }
        } else {
            if (postData[i].BigQuestionId === $('#currentQuestionId').val()) {
                curentIndex = i;
            }
        }

    }
    if (subjectType == 1) {
        answer = $(".ctj_xx").find("input[type='radio']:checked").val() == undefined ? "" : $(".ctj_xx").find("input[type='radio']:checked").val();
    } else {
        $(".ctj_xx").find("input[type='checkbox']:checked").each(function () {
            answer += $(this).val() + ',';
        });
        answer = answer.substring(0, answer.length - 1);
    }
    console.log(answer, subjectType);
    if (subjectType == 7) {
        row.BigQuestionId = "";
        row.SmallQuestionId = $('#currentQuestionId').val();
    } else {
        row.BigQuestionId = $('#currentQuestionId').val();
        row.SmallQuestionId = "";
    }
    row.StuAnswer = answer;

    for (var i = 0; i < resultArr.length; i++) {
        var id = $('#currentQuestionId').val();
        if (resultArr[i].Id == id) {
            resultArr[i].StuAnswer = answer;
        }
    }

    if (curentIndex > -1) {
        row.RightAnswer = postData[curentIndex].RightAnswer;
        postData[curentIndex] = row;
    } else {
        postData.push(row);
    }

}

var Index = 0;
var resultArr = [];
var total = 0;
var postData = [];

function GetCollectList() {
    topevery.ajaxwx({
        url: 'api/CourseSubject/GetCollectedSubject',
        data: JSON.stringify({ QuestionContent: $('.search input').val() }),
        async: false
    }, function (data) {
        if (data.Success) {
            var contentHtml = "";
            resultArr = data.Result;
            if (resultArr.length > 0) {
                for (var i = 0; i < resultArr.length; i++) {
                    if (resultArr[i].subjectType == 7) {
                        postData.push({ BigQuestionId: "", SmallQuestionId: resultArr[i].Id, StuAnswer: "", RightAnswer: resultArr[i].RightAnswer, IsCorrect: 0 })
                    } else {
                        postData.push({ BigQuestionId: resultArr[i].Id, SmallQuestionId: "", StuAnswer: "", RightAnswer: resultArr[i].RightAnswer, IsCorrect: 0 });
                    }
                    resultArr[i].StuAnswer = "";
                }
                $('.zanwu').attr("style", "display:none;");
                $('.ctj_h3').attr("style", "display:block;");
                $('.alfx-head').attr("style", "display:block;");
                var itemData = new Array();
                itemData.PracticeSubjectList = resultArr;
                $("#Option").html(template("Option_html", itemData));
                $('.stlb_sz_news span').click(function () {
                    addPostData();
                    $(".alfx-box").show();
                    $(".stlb-box").hide();
                    var index1 = parseInt($(this).attr("index"));
                    Index = index1 - 1;
                    loadItem(Index);

                })
                total = resultArr.length;
                $('#total').text(total);
                loadItem(0);
            } else {
                $('.zanwu').attr("style", "display:block;");
                $('.ctj_h3').attr("style", "display:none;");
                $('.alfx-head').attr("style", "display:none;");
            }
        }
    });
}


function loadItem(index) {
    $('.red').text(index + 1);
    var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var contentHtml = "";
    var obj = resultArr[parseInt(index)];
    $('#subjectType').val(obj.SubjectType);
    $('#currentQuestionId').val(obj.Id);
    if (obj.Description) {
        contentHtml += "<div class=\"alfx-list01\">"
                        + "<div class=\"alfx-tit\"><b>案例分析题</b></div></div>";
        contentHtml += "<div class=\"alfx-list03\">"
                        + "<p>" + obj.Description + "</p>"
                            + "<div class=\"clear\"></div>"
                        + "</div>"
                        + "<div class=\"line_3px\></div>";
    } else {
        contentHtml += "<div class=\"alfx-list01\">"
                            + "<div class=\"alfx-tit\"><b>" + formatterSubjectType(obj.SubjectType) + "</b></div></div>";
    }
    var optionHtml = "";
    var optionCount = parseInt(obj.Number);
    var StuAnswerList = obj.StuAnswer.split(",");
    for (var j = 0; j < optionCount; j++) {
        for (var k = 0; k < StuAnswerList.length; k++) {
            var isCheck = false;
            if (parseInt(j + 1) == parseInt(StuAnswerList[k])) {
                isCheck = true;
            }
        }
        optionHtml += "<li " + (isCheck ? "class='alfxbg-red'" : "") + " onclick='checkState(this)' type='" + obj.SubjectType + "' >" + formatterSubjectOption(obj.SubjectType, obj.Id, (j + 1), isCheck) + "<i>" + azstr.substr(j, 1) + "、</i>" + eval("obj.Option" + (j + 1)).replace('<p>', '').replace('</p>', '')
                  + "</li>";
    }
    contentHtml += "<div class=\"ctj_list\" style=\"margin-top:0em;\">"
                        + "<div class=\"ctj_h4\">" + obj.QuestionContent + "</div>"
                        + "<div class=\"ctj_xx\" style=\"display:block;\">"
                            + "<ul style=\"padding: 2em 0;\">"
                            + optionHtml
                            + "</ul>"
                        + "</div>"
                    + "</div>";
    $('#Item').html(contentHtml);
    $(".answer_input").click(function () {
        $("." + $("#currentQuestionId").val()).addClass("stlb_sz_ren");
    });
}




function conrectItem() {
    window.location.href = "/jjs/MyErrorSubject/Checker";
}


function formatterDate(datetime) {
    if (datetime) {
        return datetime.split(' ')[0].replace(/\//g, '-');
    }
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

function formatterSubjectOption(subjectType, questionId, value, isCheck) {
    switch (parseInt(subjectType)) {
        case 1:
            return "<input class='answer_input' type='radio' name='" + questionId + "' value='" + value + "'  " + (isCheck ? "checked='checked'" : "") + "/>"
        case 2:
            return "<input class='answer_input' type='checkbox' name='" + questionId + "' value='" + value + "' " + (isCheck ? "checked='checked'" : "") + "/>";
        case 7:
            return "<input class='answer_input' type='checkbox' name='" + questionId + "'  value='" + value + "' " + (isCheck ? "checked='checked'" : "") + " />";
        default: "";
    }
}

function formatterLabel(content) {
    if (content) {
        return content.replace(/<p>/ig, '').replace(/<\/p>/ig, '').replace(/<\/?span[^>]*>/ig, '');
    }
}

$(".al-hd02").click(function () {
    $(".alfx-box").hide();
    $(".stlb-box").show();
});

$(".close").click(function () {
    $(".alfx-box").show();
    $(".stlb-box").hide();
});

function checkState(event) {
    var type = $(event).attr("type");
    if (type == "1") {
        if ($(event).find("input").attr("checked") == undefined) {
            var danname = $(event).find("input").attr("name");
            $.each($("input[name='" + danname + "']"), function (i, val) {
                $(val).attr("checked", false);
                $(val).parent().removeClass("alfxbg-red");
            });
            $(event).find("input").attr("checked", true);
            $(event).addClass("alfxbg-red");
            $(event).find("input").click();
        }
    } else if (type == "2") {
        if ($(event).find("input").prop("checked") == false) {
            $(event).find("input").prop("checked", true);
            $(event).addClass("alfxbg-red");
        } else {
            $(event).find("input").prop("checked", false);
            $(event).removeClass("alfxbg-red");
        }
    }
    addPostData();
}

$(":checkbox").click(function (e) {
    e.stopPropagation();
});

function savePractice() {
    layer.confirm("确认提交练习记录?", function (index) {
        var arr = new Array();
        var correctNum = 0;//正确数
        var total = 0;//做题总数
        for (var i = 0; i < postData.length; i++) {
            if (postData[i].StuAnswer != "") {
                if (postData[i].StuAnswer == postData[i].RightAnswer) {
                    postData[i].IsCorrect = 1;
                    correctNum += 1;
                } else {
                    postData[i].IsCorrect = 0;
                }
                total += 1;
                arr.push(postData[i]);
            }
        }
        topevery.ajaxwx({
            url: "api/MyErrorSubject/SavePracticeSubject",
            data: JSON.stringify({ Type: 0, SubjectPracticeList: arr })
        }, function (data) {
            if (data.Result) {
                location.href = "/jjs/MyErrorSubject/PracticeResult?practiceNo=" + data.Result.Message + "&total=" + total + "&correctNum=" + correctNum;//练习结果页面
            }
        })
    })
}