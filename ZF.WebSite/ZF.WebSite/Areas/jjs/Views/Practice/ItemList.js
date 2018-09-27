$(function () {
    load();
});

var objectPaperInfo = new Array();
var ItemIndex = 1;
var No = 0;
var objectPaperInfoLenght = 0;
var ChapterId = "";
function load() {
    topevery.ajaxwx({
        url: "api/Economy/GetChapterQuestionsResult?ChapterQuestionsId=" + $("#ChapterQuestionsId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            $("#Option").html(template("Option_html", row));
            $(".stlb_sz_news span").click(function () {
                $(".alfx-box").show();
                $(".stlb-box").hide();
                var index = parseInt($(this).attr("index"));
                ItemIndex = index;
                loadItem(objectPaperInfo[index - 1]);
                No = index - 1;
            });
            ChapterId = row.ChapterId;
            objectPaperInfo = row.ChapterQuestionsDetailOutput;
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
        data.IsValueAddedWebApp = topevery.IsValueAddedWebApp;
        console.log(data);
        $("#Item").html(template("Item_html", data));
    }
}
$(".al-hd02").click(function () {
    $(".alfx-box").hide();
    $(".stlb-box").show();
});
$(".al-hd01").click(function () {
    if (No > 0) {
        ItemIndex = ItemIndex - 1;
        loadItem(objectPaperInfo[No - 1]);
        No = No - 1;
    }
});
$(".al-hd03").click(function () {
    if (No < objectPaperInfoLenght - 1) {
        ItemIndex = ItemIndex + 1;
        loadItem(objectPaperInfo[No + 1]);
        No = No + 1;
    }
});
$(".close").click(function () {
    $(".alfx-box").show();
    $(".stlb-box").hide();
});

$(".al-hd04").click(function () {
    topevery.ajaxwx({
        url: "api/Economy/InsertChapterQuestions?ChapterId=" + ChapterId,
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            location.href = "/jjs/Practice/ChapterPractice?ChapterId=" + ChapterId + "&chapterQuestionsId=" + data.Result.Id;
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
