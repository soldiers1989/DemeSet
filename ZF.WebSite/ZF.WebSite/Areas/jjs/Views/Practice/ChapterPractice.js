$(function () {
    load();
});

var Postdata = new Array();
var objectPaperInfo = new Array();
var ItemIndex = 1;
var No = 0;
var objectPaperInfoLenght = 0;
var ChapterId = "";
function load() {
    topevery.ajaxwx({
        url: "api/Economy/GetChapterPracticeList?ChapterId=" + $("#ChapterId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            console.log(row);
            for (var i = 0; i < row.length; i++) {
                for (var j = 0; j < row[i].ChapterPracticeOutput.length; j++) {
                    row[i].ChapterPracticeOutput[j].SubjectName = row[i].SubjectName;
                    objectPaperInfo.push(row[i].ChapterPracticeOutput[j]);
                    Postdata.push({ QuestionId: row[i].ChapterPracticeOutput[j].Id, Type: row[i].ChapterPracticeOutput[j].Type, StuAnswer: "" });
                }
            }
            var ItemData = new Array();
            ItemData.ChapterQuestionsDetailOutput = objectPaperInfo;
            $("#Option").html(template("Option_html", ItemData));
            $(".stlb_sz_news span").click(function () {
                addPostData();
                $(".alfx-box").show();
                $(".stlb-box").hide();
                var index1 = parseInt($(this).attr("index"));
                ItemIndex = index1;
                loadItem(objectPaperInfo[index1 - 1]);
                No = index1 - 1;
            });
            ChapterId = row.ChapterId;
            objectPaperInfoLenght = objectPaperInfo.length;
            loadItem(objectPaperInfo[0]);
        }
    }
    );
}
function loadItem(data) {
    if (data) {
        data.index = ItemIndex;
        data.lenght = objectPaperInfoLenght;
        $("#Item").html(template("Item_html", data));
        $(".answer_input").click(function () {
            $("." + $("#QuestionId").val()).addClass("stlb_sz_ren");
        });
    }
}
$(".al-hd02").click(function () {
    $(".alfx-box").hide();
    $(".stlb-box").show();
});
$(".al-hd01").click(function () {
    addPostData();
    if (No > 0) {
        ItemIndex = ItemIndex - 1;
        loadItem(objectPaperInfo[No - 1]);
        No = No - 1;
    } else {
        layer.msg("已经是第一题");
    }
});
$(".al-hd03").click(function () {
    addPostData();
    if (No < objectPaperInfoLenght - 1) {
        ItemIndex = ItemIndex + 1;
        loadItem(objectPaperInfo[No + 1]);
        No = No + 1;
    } else {
        layer.msg("已经是最后一题");
    }
});
$(".close").click(function () {
    $(".alfx-box").show();
    $(".stlb-box").hide();
});
function addPostData() {
    var index = -1;
    var row = new Object();
    for (var n = 0; n < Postdata.length; n++) {
        if (Postdata[n].QuestionId === $("#QuestionId").val()) {
            index = n;
        }
    }
    row.QuestionId = $("#QuestionId").val();
    var answer = '';
    if ($("#input_type").val() === "checkbox") {
        $(".alfx-list04").find("input[type='checkbox']:checked").each(function () {
            answer += $(this).val() + ',';
        });
        answer = answer.substring(0, answer.length - 1);
    } else {
        answer = $(".alfx-list04").find("input[type='radio']:checked").val() == undefined ? "" : $(".alfx-list04").find("input[type='radio']:checked").val();
    }
    row.StuAnswer = answer;
    row.Type = $("#Type").val();
    if (index > -1) {
        Postdata[index] = row;
    } else {
        Postdata.push(row);
    }
    for (var i = 0; i < objectPaperInfo.length; i++) {
        var questionId = $("#QuestionId").val();
        if (objectPaperInfo[i].Id === questionId) {
            var answerList = answer.split(",");
            for (var h = 0; h < answerList.length; h++) {
                for (var j = 0; j < objectPaperInfo[i].option.length; j++) {
                    if (parseInt(answerList[h]) === objectPaperInfo[i].option[j].id) {
                        objectPaperInfo[i].option[j].ischeck = "1";
                    }
                }
            }
        }
    }
}
$(".al-hd04").click(function () {
    addPostData();
    layer.confirm("确认提交练习记录?", function (index) {
        topevery.ajaxwx({
            url: "api/Economy/InsertChapterQuestionsDetail",
            type: "Post",
            data: JSON.stringify({ ChapterQuestionsId: $("#ChapterQuestionsId").val(), CourseChapterQuestionsDetail: Postdata })
        }, function (data) {
            if (data.Result) {
                location.href = "/jjs/Practice/ItemList?ChapterQuestionsId=" + $("#ChapterQuestionsId").val() + "&ChapterId=" + $("#ChapterId").val();
            } else {
                layer.msg("请勿重复提交数据");
            }
        });
    });
})



function checkState(event) {
    var type = $(event).attr("checkType");
    if (type == "radio") {
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
    } else if (type == "checkbox") {
        if ($(event).find("input").prop("checked") == false) {
            $(event).find("input").prop("checked", true);
            $(event).addClass("alfxbg-red");
        } else {
            $(event).find("input").prop("checked", false);
            $(event).removeClass("alfxbg-red");
        }
    }
}

$(":checkbox").click(function (e) {
    e.stopPropagation();
});

/*禁止复制*/
document.oncontextmenu = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
};
/*禁止审查元素*/
jQuery(document).keydown(function (event) {
    var src = (event.srcElement || event.target);
    if (src != null && src.nodeType == 1) {
        var nodeName = src.nodeName.toLowerCase();
        if (nodeName == "input" || nodeName == "textarea") {
            return true;
        }
    }
    if (event.keyCode == 67 && event.ctrlKey == true) {
        return false;
    }
    return true;
});
jQuery(document).keyup(function (event) {
    var src = (event.srcElement || event.target);
    if (src != null && src.nodeType == 1) {
        var nodeName = src.nodeName.toLowerCase();
        if (nodeName == "input" || nodeName == "textarea") {
            return true;
        }
    }
})
/*禁止F12*/
document.onkeydown = function () {
    if (window.event && window.event.keyCode == 123) {
        event.keyCode = 0;
        event.returnValue = false;
    }
    if (window.event && window.event.keyCode == 13) {
        window.event.keyCode = 505;
    }
    if (window.event && window.event.keyCode == 8) {
        window.event.returnValue = false;
    }
    if (window.event && window.event.keyCode == 64) {
        window.event.returnValue = false;
    }

};
