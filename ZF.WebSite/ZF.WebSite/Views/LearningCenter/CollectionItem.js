
GetItemList();
//GetPageList();


$('#btn_search_item').click(function () {
    GetItemList();
});

function GetItemList() {
    var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    topevery.ajax({
        url: 'api/CourseSubject/GetCollectedSubjectList',
        data: JSON.stringify({ Page: $('#PageIndex').val(), Rows: $('#Rows').val(), QuestionContent: $('.search input').val() }),
        async: false
    }, function (data) {
        if (data.Success) {
            var contentHtml = "";
            var arr = data.Result.Rows;
            totalCount = data.Result.Records;
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    //案例分析题
                    if (arr[i].SubjectType == 7) {//遍历子题列表
                        var caseContentHmtl = "";
                        var subContent = "";
                        if (arr[i].SubjectSmallquestions && arr[i].SubjectSmallquestions.length > 0) {
                            var sublist = arr[i].SubjectSmallquestions;
                            for (var k = 0; k < sublist.length; k++) {
                                //子题选项
                                var subOptionHtml = "";
                                var subOptionCount = parseInt(sublist[k].Number);

                                for (var j = 0; j < subOptionCount; j++) {
                                    subOptionHtml += "<li>" + formatterLabel(azstr.substr(j, 1) + "、" + eval("sublist[k].Option" + (j + 1))) + "</li>";
                                }
                                //子题内容
                                subContent += "<div class=\"topicItem\">"
                                                    + "<div class=\"topic\">"
                                                        + "<span class=\"type\">" + formatterSubjectType(sublist[k].SubjectType) + "</span>"
                                                        + "<div>" + formatterLabel(sublist[k].QuestionContent) + "</div>"
                                                    + "</div>"
                                                    + "<div class=\"toggleWrap\">"
                                                        + "<ul class=\"item\">"
                                                           + subOptionHtml
                                                        + "</ul>"
                                                        + "<div class=\"answer\">"
                                                            + "<div class=\"state\">"
                                                                + "<label>正确答案：<font class=\"red\">" + formatterRightAnswer(sublist[k].RightAnswer) + "</font></label>"
                                                            + "</div>"
                                                            + "<div class=\"details\">"
                                                                + (sublist[k].QuestionTextAnalysis ? sublist[k].QuestionTextAnalysis : "")
                                                            + "</div>"
                                                            + "<div class=\"learnVideo\"><div><label>" + (sublist[k].VideoName == null ? "暂无视频" : sublist[k].VideoName) + "</label>" + (sublist[k].VideoId == null || arr[i].VideoId == "" ? "" : "<a data=\"" + sublist[k].VideoId + "\" onclick='jumpToVideo(this);'\">开始学习</a>") + "</div></div>"
                                                        + "</div>"
                                                    + "</div>"
                                                        + "<div class=\"operation\">"
                                                                + "<a href=\"javascript:;\" class=\"time\"></i>" + formatterDate(sublist[k].AddTime) + "</a>"
                                                                + "<a href=\"javascript:;\" data=\"" + sublist[k].Id + "\" class=\"remove\">删除</a>"
                                                                + "<a subjectType=\"1\" data=\"0\" class=\"toggle\">展开</a>"
                                                        + "</div>"
                                            + "</div>";
                            }
                            caseContentHmtl += "<div class=\"layui-form-item box\">"
                                   + "<div class=\"topic\">"
                                       + "<span class=\"type\">案例分析题</span>"
                                       + "<div>" + formatterLabel(arr[i].QuestionContent) + "</div>"
                                   + "</div>"
                                   + "<div class=\"toggleWrap anli\">"
                                   //子题
                                   + subContent
                                   + "</div>"
                                   + "<div class=\"operation\">"
                                       + "<a href=\"javascript:;\" class=\"time\">" + formatterDate(arr[i].AddTime) + "</a>"
                                       //+ "<a href=\"javascript:;\" class=\"remove\">删除</a>"
                                       + "<a subjectType=\"7\" data=\"0\" href=\"javascript:;\" class=\"toggle\">展开</a>"
                                   + "</div>"
                               + "</div>";
                        }
                        contentHtml += caseContentHmtl;
                    } else {

                        var optionCount = parseInt(arr[i].Number);
                        var optionHtml = "";
                        for (var j = 0; j < optionCount; j++) {
                            optionHtml += "<li>" + formatterLabel(azstr.substr(j, 1) + "、" + eval("arr[i].Option" + (j + 1)))
                                           + "</li>";
                        }
                        contentHtml += "<div class=\"layui-form-item box\">"
                                            + "<div class=\"topic\">"
                                                + "<span class=\"type\">" + formatterSubjectType(arr[i].SubjectType) + "</span>"
                                            + "<div>" + formatterLabel(arr[i].QuestionContent) + "</div>"
                                            + "</div>"
                                            + "<div class=\"toggleWrap\">"
                                                + "<ul class=\"item\">"
                                                    + optionHtml
                                                + "</ul>"
                                                + "<div class=\"answer\">"
                                                    + "<div class=\"state\">"
                                                        + "<label>正确答案：<font class=\"red\">" + formatterRightAnswer(arr[i].RightAnswer) + "</font></label>"
                                                    + "</div>"
                                                    + "<div class=\"details\">"
                                                    + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                    + "</div>"
                                                    + "<div class='learnVideo'><div><label>" + (arr[i].VideoName == null ? "暂无视频" : arr[i].VideoName) + "</label>" + (arr[i].VideoId == null || arr[i].VideoId == "" ? "" : "<a data=\"" + arr[i].VideoId + "\" onclick='jumpToVideo(this);'>开始学习</a>") + "</div></div>"
                                                + "</div>"
                                            + "</div>"
                                            + "<div class=\"operation\">"
                                                + "<a href=\"javascript:;\" class=\"time\"></i>" + formatterDate(arr[i].AddTime) + "</a>"
                                                + "<a href=\"javascript:;\" data=\"" + arr[i].Id + "\" class=\"remove\">删除</a>"
                                                + "<a subjectType=\"1\" data=\"0\" class=\"toggle\">展开</a>"
                                            + "</div>"
                                    + "</div>";
                    }
                }
                //$(contentHtml).insertAfter($('#colectedSubject'));
                $("#collectedSubject").html(contentHtml);
                $('.page').attr('style', 'display:block;');
                $('.toggle').click(toggleContent);
                $('.cur').click(toggleContent);
                $(".remove").click(removeItem);
                $('.M-box1').show();
                $('.M-box1').pagination({
                    pageCount: Math.ceil(data.Result.Records / parseInt($("#Rows").val())),
                    jump: true,
                    coping: true,
                    homePage: '首页',
                    endPage: '末页',
                    prevContent: '上页',
                    nextContent: '下页',
                    callback: function (api) {
                        $("#PageIndex").val(api.getCurrent());
                        GetItemList1();
                        $(".btn_top").click();
                    }
                });
            } else {
                $("#collectedSubject").html(contentHtml);
                $('#empty_area').attr('style', 'display:block;');
                $('.page').attr('style', 'display:none;');
                $(".M-box1").hide();
            }
        }
    });
}

function GetItemList1() {
    var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    topevery.ajax({
        url: 'api/CourseSubject/GetCollectedSubjectList',
        data: JSON.stringify({ Page: $('#PageIndex').val(), Rows: $('#Rows').val(), QuestionContent: $('.search input').val() }),
        async: false
    }, function (data) {
        if (data.Success) {
            var contentHtml = "";
            var arr = data.Result.Rows;
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    //案例分析题
                    if (arr[i].SubjectType == 7) {//遍历子题列表
                        var caseContentHmtl = "";
                        var subContent = "";
                        if (arr[i].SubjectSmallquestions && arr[i].SubjectSmallquestions.length > 0) {
                            var sublist = arr[i].SubjectSmallquestions;
                            for (var k = 0; k < sublist.length; k++) {
                                //子题选项
                                var subOptionHtml = "";
                                var subOptionCount = parseInt(sublist[k].Number);

                                for (var j = 0; j < subOptionCount; j++) {
                                    subOptionHtml += "<li>" + formatterLabel(azstr.substr(j, 1) + "、" + eval("sublist[k].Option" + (j + 1))) + "</li>";
                                }
                                //子题内容
                                subContent += "<div class=\"topicItem\">"
                                                    + "<div class=\"topic\">"
                                                        + "<span class=\"type\">" + formatterSubjectType(sublist[k].SubjectType) + "</span>"
                                                        + "<div>" + formatterLabel(sublist[k].QuestionContent) + "</div>"
                                                    + "</div>"
                                                    + "<div style=\"display:none;\">"
                                                        + "<ul class=\"item\">"
                                                           + subOptionHtml
                                                        + "</ul>"
                                                        + "<div class=\"answer\">"
                                                            + "<div class=\"state\">"
                                                                + "<label>正确答案：<font class=\"red\">" + formatterRightAnswer(sublist[k].RightAnswer) + "</font></label>"
                                                            + "</div>"
                                                            + "<div class=\"details\">"
                                                                + (sublist[k].QuestionTextAnalysis ? sublist[k].QuestionTextAnalysis : "")
                                                            + "</div>"
                                                            + "<div class=\"learnVideo\"><div><label>" + (sublist[k].VideoName == null ? "暂无视频" : sublist[k].VideoName) + "</label>" + (sublist[k].VideoId == null || arr[i].VideoId == "" ? "" : "<a data=\"" + sublist[k].VideoId + "\" onclick='jumpToVideo(this);'\">开始学习</a>") + "</div></div>"
                                                        + "</div>"
                                                    + "</div>"
                                                        + "<div class=\"operation\">"
                                                                + "<a href=\"javascript:;\" class=\"time\"></i>" + formatterDate(sublist[k].AddTime) + "</a>"
                                                                + "<a href=\"javascript:;\" data=\"" + sublist[k].Id + "\" class=\"remove\">删除</a>"
                                                                + "<a subjectType=\"1\" data=\"0\" class=\"toggle\">展开</a>"
                                                        + "</div>"
                                            + "</div>";
                            }
                            caseContentHmtl += "<div class=\"layui-form-item box\">"
                                   + "<div class=\"topic\">"
                                       + "<span class=\"type\">案例分析题</span>"
                                       + "<div>" + formatterLabel(arr[i].QuestionContent) + "</div>"
                                   + "</div>"
                                   + "<div class=\"toggleWrap anli\">"
                                   //子题
                                   + subContent
                                   + "</div>"
                                   + "<div class=\"operation\">"
                                       + "<a href=\"javascript:;\" class=\"time\">" + formatterDate(arr[i].AddTime) + "</a>"
                                       //+ "<a href=\"javascript:;\" class=\"remove\">删除</a>"
                                       + "<a subjectType=\"7\" data=\"0\" href=\"javascript:;\" class=\"toggle\">展开</a>"
                                   + "</div>"
                               + "</div>";
                        }
                        contentHtml += caseContentHmtl;
                    } else {

                        var optionCount = parseInt(arr[i].Number);
                        var optionHtml = "";
                        for (var j = 0; j < optionCount; j++) {
                            optionHtml += "<li>" + formatterLabel(azstr.substr(j, 1) + "、" + eval("arr[i].Option" + (j + 1)))
                                           + "</li>";
                        }
                        contentHtml += "<div class=\"layui-form-item box\">"
                                            + "<div class=\"topic\">"
                                                + "<span class=\"type\">" + formatterSubjectType(arr[i].SubjectType) + "</span>"
                                            + "<div>" + formatterLabel(arr[i].QuestionContent) + "</div>"
                                            + "</div>"
                                            + "<div class=\"toggleWrap\">"
                                                + "<ul class=\"item\">"
                                                    + optionHtml
                                                + "</ul>"
                                                + "<div class=\"answer\">"
                                                    + "<div class=\"state\">"
                                                        + "<label>正确答案：<font class=\"red\">" + formatterRightAnswer(arr[i].RightAnswer) + "</font></label>"
                                                    + "</div>"
                                                    + "<div class=\"details\">"
                                                    + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                    + "</div>"
                                                    + "<div class='learnVideo'><div><label>" + (arr[i].VideoName == null ? "暂无视频" : arr[i].VideoName) + "</label>" + (arr[i].VideoId == null || arr[i].VideoId == "" ? "" : "<a data=\"" + arr[i].VideoId + "\" onclick='jumpToVideo(this);'>开始学习</a>") + "</div></div>"
                                                + "</div>"
                                            + "</div>"
                                            + "<div class=\"operation\">"
                                                + "<a href=\"javascript:;\" class=\"time\"></i>" + formatterDate(arr[i].AddTime) + "</a>"
                                                + "<a href=\"javascript:;\" data=\"" + arr[i].Id + "\" class=\"remove\">删除</a>"
                                                + "<a subjectType=\"1\" data=\"0\" class=\"toggle\">展开</a>"
                                            + "</div>"
                                    + "</div>";
                    }
                }
                //$(contentHtml).insertAfter($('#colectedSubject'));
                $("#collectedSubject").html(contentHtml);
                //$('.page').attr('style', 'display:block;');
                $('.toggle').click(toggleContent);
                $('.cur').click(toggleContent);
                $(".remove").click(removeItem);
            } else {
                //$("#collectedSubject").html(contentHtml);
                $('#empty_area').attr('style', 'display:block;');
                //$('.page').attr('style', 'display:none;');
                //$(".M-box1").hide();
            }
        }
    });
}




function jumpToVideo(event) {
    console.log($(event));
    var videoId = $(event).attr("data");
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


var totalCount = 0;

function GetPageList() {
    var rows = parseInt($('#Rows').val());//每页显示数据
    var totalPage = totalCount % rows > 0 ? parseInt(totalCount / rows) + 1 : parseInt(totalCount / rows);//总页数
    var pageContent = "";
    var pageIndex = parseInt($('#PageIndex').val());
    for (i = 0; i < totalPage; i++) {
        if (pageIndex == (i + 1)) {
            pageContent += "<a href=\"javascript:;\" class=\"cur\">" + (i + 1) + "</a>";
        } else {
            pageContent += "<a href=\"javascript:;\" >" + (i + 1) + "</a>";
        }
    }

    //$(pageContent).insertAfter($('.page .prev')[0]);
    $('#pageContent').html(pageContent);
    bindPage();
}


function bindPage() {
    $('.page>a,.page>span>a').each(function () {
        var rows = parseInt($('#Rows').val());
        var totalPage = totalCount % rows > 0 ? parseInt(totalCount / rows) + 1 : parseInt(totalCount / rows);//总页数
        $(this).click(function () {
            var pageIndex = parseInt($("#PageIndex").val());
            var targertIndex = $(this).text();
            if (targertIndex == "<<") {
                if (pageIndex > 1) {
                    pageIndex -= 1;
                }
            } else if (targertIndex == ">>") {
                if (pageIndex < totalPage) {
                    pageIndex += 1;
                }
            } else if (targertIndex == "跳转") {
                var value = parseInt($(".page input").val());
                if (value > 0 && value <= totalPage - 1) {
                    pageIndex = parseInt(value);
                }
            } else {
                var val = parseInt($(this).text());
                if (val != pageIndex) {
                    pageIndex = val;
                }
            }
            $("#PageIndex").val(pageIndex);
            setCurretnPage(pageIndex);
            GetItemList();
        })
    });
}

function setCurretnPage(pageIndex) {
    $('#pageContent>a').each(function () {
        var val = parseInt($(this).text());
        if (pageIndex == val) {
            $('#pageContent>a').each(function () {
                $(this).removeClass('cur');
            })
            $(this).addClass('cur');
            return false;
        }
    })
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

function removeItem() {
    var id = $(this).attr('data');
    layer.confirm("确认删除?", function (index) {
        topevery.ajax({
            url: 'api/MyCollectionItem/CancelCollectSubject',
            data: JSON.stringify({ Id: id })
        }, function (data) {
            GetItemList();
            layer.close(index);
        });

    })
}

function formatterRightAnswer(answerStr) {
    var result = "";
    var azstr = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    if (answerStr) {
        var arr = answerStr.split(',');
        for (var item in arr) {
            result += azstr[arr[parseInt(item)] - 1] + ",";
        }
        result = result.substr(0, result.length - 1);
        return result;
    }
    return result;

}

function toggleContent() {
    var data = $(this).attr('data');
    var obj = $($(this).parent().prev()).find('.answer');
    if (data == "0") {
        $(obj).attr("style", "display:block;");
        $(this).addClass("cur");
        $(this).html("收起");
        $(this).attr("data", "1");
    } else if (data == "1") {
        $(obj).attr("style", "display:none;");
        $(this).html("展开");
        $(this).attr("data", "0");
        $(this).removeClass("cur");
    }
}

function formatterDate(datetime) {
    if (datetime) {
        return datetime.split(' ')[0].replace(/\//g, '-');
    }
}


function formatterLabel(content) {
    if (content)
        return content.replace(/<p>/ig, '').replace(/<\/p>/ig, '').replace(/<\/?span[^>]*>/ig, '');
    return "";
}