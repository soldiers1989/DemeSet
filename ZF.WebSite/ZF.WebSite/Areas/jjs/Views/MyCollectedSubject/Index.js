$(function () {
    GetCollectedList();




    ///上一题
    $('.al-hd01').click(function () {
        if (Index > 0) {
            Index -= 1;
            loadItem(Index);
        } else {
            layer.msg("已经是第一页！");
        }
    });
    //下一题
    $('.al-hd03').click(function () {
        if (Index < total-1) {
            Index += 1;
            loadItem(Index);
        } else {
            layer.msg("已经是最后一页！");
        }
    });

    $('.al-hd04').click(function () {
        window.location.href = "/jjs/MyCollectedSubject/Repractice";
    })

})
var Index = 0;
var resultArr = [];
var total = 0;

function GetCollectedList() {
    topevery.ajaxwx({
        url: 'api/CourseSubject/GetCollectedSubject',
        data: JSON.stringify({ QuestionContent: $('.search input').val() }),
        async: false
    }, function (data) {
        
        if (data.Success) {
            resultArr = data.Result;
            if (resultArr.length > 0) {
                total = data.Result.length;
                $('#total').text(total);
                loadItem(0);
            } else {
                $('.ctj_h3').attr("style", 'display:none;');
                $('.zanwu').attr("style",'display:block;');
            }
        }
    });
}


function loadItem(index) {
    var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var contentHtml = "";
    var obj = resultArr[parseInt(index)];
    if (obj.Description) {
        contentHtml += "<div class=\"alfx-list01\">"
                        + "<div class=\"alfx-tit\"><b>案例分析题</b></div></div>";
        contentHtml += "<div class=\"alfx-list03\">"
                        + "<p>" +obj.Description + "</p>"
                            + "<div class=\"clear\"></div>"
                        + "</div>"
                        + "<div class=\"line_3px\></div>";
    } else {
        contentHtml += "<div class=\"alfx-list01\">"
                            + "<div class=\"alfx-tit\"><b>" + formatterSubjectType(obj.SubjectType) + "</b></div></div>";
    }
    var optionHtml = "";
    var optionCount = parseInt(obj.Number);
    for (var j = 0; j < optionCount; j++) {
        optionHtml += "<li><i>" + azstr.substr(j, 1) + "</i>" + eval("obj.Option" + (j + 1)).replace('<p>', '').replace('</p>', '')
                  + "</li>";
    }
    contentHtml += "<div class=\"ctj_list\" style=\"margin-top:0em;\">"
                        + "<div class=\"ctj_h4\">" + obj.QuestionContent + "</div>"
                        + "<div class=\"ctj_xx\" style=\"display:block;\">"
                            + "<ul style=\"padding: 2em 0;\">"
                            + optionHtml
                            + "</ul>"
                        + "</div>"
                        + "<div class=\"ctj_tb\">"
                            + "<ul>"
                                + "<li><img src=\"/Areas/jjs/resources/images/cuoti01@2x.png\" alt=\"\">" + formatterDate(obj.AddTime) + "</li>"
                                + "<li>"
                                    + "<a style=\"cursor:pointer;\" onclick=\"show(this);\" class=\"btn2\"><img src=\"/Areas/jjs/resources/images/zhank@2x.png\"  alt=\"\">展开</a>"
                                    + "<a style=\"cursor:pointer;\" onclick=\"hide(this);\" class=\"btn1\"><img src=\"/Areas/jjs/resources/images/cuoti02@2x.png\"  alt=\"\">收起</a>"
                                + "</li>"
                                + "<li><a class=\"conrect\" data=\"" + obj.Id + "\" type=\"1\" ><img src=\"/Areas/jjs/resources/images/jrdt05.png\" alt=\"\">纠错</a></li>"
                                + "<li><a class=\"remove\" data=\"" + obj.Id + "\" type=\"1\" ><img src=\"/Areas/jjs/resources/images/bianji01@2x.png\" alt=\"\">删除此题</a></li>"
                                + "<div class=\"clear\"></div>"
                            + "</ul>"
                        + "</div>"
                        + "<div class=\"ctj-list\" style=\"display:block;\">"
                            + "<div class=\"ctj-dtcw\">"
                                ////+ "<span class=\"dtcw01\">答题错误!</span>"
                                ////+"<span class=\"dtcw02\">你的答案：<b class=\"dtred\">A</b></span>"
                                + "<span class=\"dtcw03\">正确答案：" + formatterRightAnswer(obj.RightAnswer) + "</span>"
                            + "</div>"
                            + "<p>【评析】" + (obj.QuestionTextAnalysis==""?"暂无评析":obj.QuestionTextAnalysis) + "</p>"
                            +(topevery.IsValueAddedWebApp?"": "<div style=\"border-top:0px;\" class=\"learnVideo\"><div><label>" + (obj.VideoName == null ? "暂无视频" : obj.VideoName) + "</label>" + (obj.VideoId == null || obj.VideoId == "" ? "" : "<a  href=\"/jjs/CourseInfo/Index?code="+obj.VideoId+"\">开始学习</a>") + "</div></div>")
                        + "</div>"
                    + "</div>";
    $('#collectedlist').html(contentHtml);
    $(".remove").click(removeItem);
    $(".conrect").click(conrectItem);
}


function formatterRightAnswer(answerStr) {
    var result = "";
    var azstr = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    if (answerStr != null || answerStr != undefined || answerStr != '') {
        var arr = answerStr.split(',');
        for (var item in arr) {
            result += azstr[arr[parseInt(item)] - 1] + ",";
        }
        result = result.substr(0, result.length - 1);
        return result;
    }
    return result;

}

function conrectItem() {
    window.location.href = "/jjs/MyErrorSubject/Checker";
}

function removeItem() {
    var id = $(this).attr('data');
    //var type = $(this).attr('type');
    layer.confirm("确认删除?", function (index) {
        topevery.ajaxwx({
            url: 'api/MyCollectionItem/CancelCollectSubject',
            data: JSON.stringify({ Id: id })
        }, function (data) {
            GetCollectedList();
            layer.close(index);
        });

    })
}

function formatterDate(datetime) {
    if (datetime) {
        return datetime.split(' ')[0].replace(/\//g, '-');
    }
}


function formatterLabel(content) {
    return content.replace(/<p>/ig, '').replace(/<\/p>/ig, '').replace(/<\/?span[^>]*>/ig, '');
}

function show(e) {
    //var target = $(e).parent().parent().parent().prev();
    //$(target).attr("style", "display:block;");
    var next = $(e).parent().parent().parent().next();
    $(next).attr("style", "display:block;");
    $(e).attr("style", 'display:none;');
    $(e).next().attr("style", "display:block;");
}

function hide(e) {
    //var target = $(e).parent().parent().parent().prev();
    //$(target).attr("style", "display:none;");
    var next = $(e).parent().parent().parent().next();
    $(next).attr("style", "display:none;");
    $(e).attr("style", 'display:none;');
    $(e).prev().attr("style", "display:block;");
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
        default: "";
    }
}


