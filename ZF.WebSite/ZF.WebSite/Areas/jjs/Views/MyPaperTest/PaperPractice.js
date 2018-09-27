$(function () {
    load();
});

//题目集合
var objectPaperInfo = new Array();
var Postdata = new Array();
var ItemIndex = 1;
var objectPaperInfoLenght = 0;
var ChapterId = "";

function load() {
    topevery.ajaxwx({
        url: "api/Economy/GetPaperInfo?PaperId=" + $("#PaperId").val(),
        type: "Get",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $("#Option").html(template("Option_html", data));
            for (var i = 0; i < data.Result.length; i++) {
                objectPaperInfoLenght += data.Result[i].QuestionCount;
            }
            loadItem(data.Result[0].Question[0].Id);
            $(".stlb_sz_news span").click(function () {
                $(".alfx-box").show();
                $(".stlb-box").hide();
                var index = parseInt($(this).attr("index"));
                addPostData($(".questionId_" + ItemIndex).attr("dataId"));
                ItemIndex = index;
                loadItem($(".questionId_" + index).attr("dataId"));
            });
        }
    });
}
function loadItem(Id, callback) {
    if (objectPaperInfo[ItemIndex - 1] == undefined) {
        var SubjectType = $("." + Id).attr("SubjectType");
        var SubjectName = $("." + Id).parent().attr("SubjectName");
        topevery.ajaxwx({
            url: "api/Economy/GetQuestion?questionId=" + Id + "&type=" + SubjectType + "&paperRecordsId=" + $("#PaperRecordsId").val(),
            type: "Get",
            async: false,
            data: JSON.stringify({})
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                row.index = ItemIndex;
                row.lenght = objectPaperInfoLenght;
                row.SubjectName = SubjectName;
                $("#Item").html(template("Item_html", row));
                $(".answer_input").click(function () {
                    $("." + $("#QuestionId").val()).addClass("stlb_sz_ren");
                });
                objectPaperInfo[ItemIndex - 1] = row;
                if (callback) {
                    callback();
                }
            }
        });
    } else {
        $("#Item").html(template("Item_html", objectPaperInfo[ItemIndex - 1]));
        $(".answer_input").click(function () {
            $("." + $("#QuestionId").val()).addClass("stlb_sz_ren");
        });
        if (callback) {
            callback();
        }
    }

}

function addPostData(questionId) {
    var index = -1;
    var row = new Object();
    for (var n = 0; n < Postdata.length; n++) {
        if (Postdata[n].questionId === questionId) {
            index = n;
        }
    }
    var answer = '';
    if ($("#input_type").val() === "checkbox") {
        $(".alfx-list04").find("input[type='checkbox']:checked").each(function () {
            answer += $(this).val() + ',';
        });
        answer = answer.substring(0, answer.length - 1);
    } else {
        answer = $(".alfx-list04").find("input[type='radio']:checked").val() == undefined ? "" : $(".alfx-list04").find("input[type='radio']:checked").val();
    }
    row.Score = $("." + questionId).parent().parent().attr("questionsscore");
    row.questionId = questionId;
    row.answer = answer.trim(",");
    row.type = $("#SubjectType").val();
    row.PaperId = $("#PaperId").val();
    row.PaperRecordsId = $("#PaperRecordsId").val();
    if (index > -1) {
        Postdata[index] = row;
    } else {
        Postdata.push(row);
    }
    for (var i = 0; i < objectPaperInfo.length; i++) {
        try {
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
        } catch (e) {
        }
    }
}
$(".al-hd02").click(function () {
    $(".alfx-box").hide();
    $(".stlb-box").show();
});
$(".al-hd01").click(function () {
    addPostData($(".questionId_" + ItemIndex).attr("dataId"));
    if (ItemIndex > 1) {
        ItemIndex = ItemIndex - 1;
        loadItem($(".questionId_" + ItemIndex).attr("dataId"));
    }
});
$(".al-hd03").click(function () {
    addPostData($(".questionId_" + ItemIndex).attr("dataId"));
    if (ItemIndex < objectPaperInfoLenght) {
        ItemIndex = ItemIndex + 1;
        loadItem($(".questionId_" + ItemIndex).attr("dataId"));
    }
});
$(".close").click(function () {
    $(".alfx-box").show();
    $(".stlb-box").hide();
});

$(".al-hd04").click(function () {
    addPostData($(".questionId_" + ItemIndex).attr("dataId"));
    layer.confirm("确认提交练习记录?", function (index) {
        topevery.ajaxwx({
            url: "api/Economy/AnswerQuestion",
            type: "Post",
            data: JSON.stringify(Postdata)
        }, function (data) {
            if (data.Success) {
                if (data.Result) {
                    window.location.href = "/jjs/MyPaperTest/EndPaper?PaperId=" + $("#PaperId").val() + "&paperRecordsId=" + $("#PaperRecordsId").val();
                } else {
                    layer.msg("请勿重复提交数据");
                }
            }
        });
    });
});



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
            //$(event).find("input").attr("checked", true);
            $(event).addClass("alfxbg-red");
        } else {
            $(event).find("input").prop("checked", false);
            //$(event).find("input").attr("checked", false);
            $(event).removeClass("alfxbg-red");
        }
    }
}

$(":checkbox").click(function (e) {
    e.stopPropagation();
});
