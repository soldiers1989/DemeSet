$(function () {
    load();
});

//题目集合
var objectPaperInfo = new Array();

var ItemIndex = 1;
var objectPaperInfoLenght = 0;
var ChapterId = "";

function load() {

    topevery.ajaxwx({
        url: "api/Economy/GetPaperInfoView?PaperId=" + $("#PaperId").val() + "&PaperRecordsId=" + $("#PaperRecordsId").val(),
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
                ItemIndex = index;
                loadItem($(".questionId_" + index).attr("dataId"));
            });
        }
    });
}
function loadItem(Id) {
    if (objectPaperInfo[ItemIndex - 1] == undefined) {
        var SubjectType = $("." + Id).attr("SubjectType");
        topevery.ajaxwx({
            url: "api/Economy/GetQuestionView?questionId=" + Id + "&type=" + SubjectType + "&paperRecordsId=" + $("#PaperRecordsId").val(),
            type: "Get",
            data: JSON.stringify({})
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                row.index = ItemIndex;
                row.IsValueAddedWebApp = topevery.IsValueAddedWebApp;
                row.lenght = objectPaperInfoLenght;
                $("#Item").html(template("Item_html", row));
                objectPaperInfo[ItemIndex - 1] = row;
            }
        });
    } else {
        $("#Item").html(template("Item_html", objectPaperInfo[ItemIndex - 1]));
    }
}
$(".al-hd02").click(function () {
    $(".alfx-box").hide();
    $(".stlb-box").show();
});
$(".al-hd01").click(function () {
    if (ItemIndex > 1) {
        ItemIndex = ItemIndex - 1;
        loadItem($(".questionId_" + ItemIndex).attr("dataId"));
    }
});
$(".al-hd03").click(function () {
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
    topevery.ajaxwx({
        url: "api/Economy/InsertIntoPaperRecords?PaperId=" + $("#PaperId").val(),
        type: "Post",
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            if (row.PaperId == "-1") {
                parent.layer.msg("该试卷暂未上线,敬请期待!", {
                    time: 4000
                });
            } else {
                //跳转到试卷页面
                window.location.href = "/jjs/MyPaperTest/PaperPractice?PaperId=" + row.PaperId + "&paperRecordsId=" + row.PaperRecordsId;
            }
          
        }
    });
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
