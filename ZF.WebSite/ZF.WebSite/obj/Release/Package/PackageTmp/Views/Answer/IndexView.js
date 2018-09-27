//题目集合
var objectPaperInfo = new Array();
//提交对象
var postdata = new Array();
//题型序号
var index = 0;
//题目序号
var serialNumber = 0;
//试题数量
var questionsNumber = 0;
//试题分数
var questionsScore = 0;
//试题类型
var questionType = 0;
//试题数量
var questioncount = 0;
$(document).ready(function () {
    //初始化 考试时间  考生名称  试卷名称
    topevery.ajax({
        url: "api/Economy/GetExaminationPaper?PaperId=" + $("#PaperId").val() + "&PaperRecordsId=" + $("#PaperRecordsId").val(),
        type: "Post",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            //topevery.SetCookie("time", new Date().getTime() + 1000 * 60 * row.TestTime);
            $("#UserName").html(row.UserName);
            $("#ImgUrl").attr("src", row.ImgUrl);
            $("#PaperName").html("试卷名称：" + row.PaperName + "&nbsp试卷练习编号：" + row.PracticeNo + "&nbsp总分：(" + row.QuestionScore + ")&nbsp得分(<a style='color:red'>" + row.Score + "</a>)");
            questioncount = row.Count;
        }
    });
    //初始化试卷结构试题结构
    topevery.ajax({
        url: "api/Economy/GetPaperInfoView?PaperId=" + $("#PaperId").val() + "&PaperRecordsId=" + $("#PaperRecordsId").val(),
        type: "Get",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            var row = data;
            $('#myquestion1').html(template("question_common1", row));
            $($("#myquestion1 a")[0]).parents('.mainLeft').find('.myitem').removeClass('current');
            $($("#myquestion1 a")[0]).addClass('current');
            var qid = $($("#myquestion1 a")[0]).attr('qid');
            $('#question_id').val(qid);
            $("#SubjectType").val($($("#myquestion1 a")[0]).attr('SubjectType'));
            questionsNumber = $($("#myquestion1 a")[0]).parent().attr('questionsNumber');
            questionsScore = parseFloat($($("#myquestion1 a")[0]).parent().attr('questionsScore'));
            questionType = $($("#myquestion1 a")[0]).parent().attr('QuestionType');
            GetQuestion();
        }
    });
    //绑定每个试题的点击事件
    $(document).on('click', '.mainLeft .title a', function () {
        $(this).parents('.mainLeft').find('.myitem').removeClass('current');
        $(this).addClass('current');
        var qid = $(this).attr('qid');
        $('#question_id').val(qid);
        $("#SubjectType").val($(this).attr('SubjectType'));
        serialNumber = parseInt($(this).html() - 1);
        index = parseInt($(this).parent().attr("data") - 1);

        questionsNumber = $(this).parent().attr("questionsNumber");
        questionsScore = parseFloat($(this).parent().attr("questionsScore"));
        questionType = $(this).parent().attr("QuestionType");

        GetQuestion();
        if ($(".myitem.current").hasClass('addkey')) {
            $(".key").addClass('current');
            $(".key").val("取消");
        } else {
            $(".key").removeClass('current');
            $(".key").val("标记");
        }

    });
    //绑定重新考试按钮
    $('#handin_btn').click(function () {
        $(".hs").css("height", $(window).height());
        topevery.ajax({
            url: "api/Economy/InsertIntoPaperRecords?PaperId=" + $("#PaperId").val(),
            type: "Post",
        }, function (data) {
            if (data.Success) {
                window.location.href = "/Answer/Index?PaperId=" + $("#PaperId").val() + "&paperRecordsId=" + data.Result;
            }
        });
    });
    //绑定上一题按钮事件
    $(document).on('click', '.up', function () {
        if ($('.mainLeft .title .current').prev('a').length > 0) {
            $('.mainLeft .title .current').prev('a').click();
        } else {
            if ($('.mainLeft .title .current').parents('.title').find('a').length > 0) {
                var tmp_obj = $('.mainLeft .title .current').parents('.title').prev();
                tmp_obj.addClass(' show');
                tmp_obj.find('div').show();
                $('.mainLeft .title .current').parents('.title').prev().find('a:last').click();
            }
        }
    })
    //绑定下一题按钮事件
    $(document).on('click', '.next', function () {
        var obj_cur = $('.mainLeft .title .current');
        if (obj_cur.next().length > 0) {
            obj_cur.next().click();
        } else {
            if (obj_cur.parents('.title').next().find('a').length > 0) {
                var tmp_obj = obj_cur.parents('.title').next();
                tmp_obj.addClass(' show');
                tmp_obj.find('div').show();
                obj_cur.parents('.title').next().find('a:first').click();
            }
        }
        return false;
    })

    $('#cancel_btn').click(function () {
        $('#handin_msg').hide();
    });
    $(document).on('click', "input.answer_input", function () {
        var answer = '';
        var htmlname = $(this).attr('name');
        var qid = htmlname.replace('answer_', '');
        var qid = qid.replace('[]', '');
        if ($('input[name="' + htmlname + '"]:checked').length > 1) {
            $('input[name="' + htmlname + '"]:checked').each(function () {
                answer += $(this).val() + ',';
            });
            answer = answer.substring(0, answer.length - 1);
        } else {
            var answer = $(this).val();
        }
        for (var j = 0; j < objectPaperInfo[index][serialNumber].option.length; j++) {
            objectPaperInfo[index][serialNumber].option[j].ischeck = "0";
        }
        for (var i = 0; i < answer.split(",").length; i++) {
            objectPaperInfo[index][serialNumber].option[parseInt(answer.split(",")[i]) - 1].ischeck = "1";
        }
        $('#li_qid_' + qid).addClass('ed');
        if ($('#myauto').val() == 1 && $('#type').val() != 2) {
            setTimeout(function () {
                $('.next').click();
                return false;
            }, 200)
        }
    });

    $(window).resize(function (event) {
        chooseHeight();
    });
    chooseHeight();

    function chooseHeight() {
        $(".subject").css("height", $(window).height() - $(".header").outerHeight() - $(".btn").outerHeight() - $(".part").outerHeight() - 10);
    }
    //加载试题
    function GetQuestion() {
        if (objectPaperInfo[index] == undefined || objectPaperInfo[index][serialNumber] == undefined) {
            topevery.ajax({
                url: "api/Economy/GetQuestionView?questionId=" + $('#question_id').val() + "&type=" + $("#SubjectType").val() + "&paperRecordsId=" + $("#PaperRecordsId").val(),
                type: "Get",
                data: JSON.stringify({})
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    getPartTitle(row.SubjectType, row.Description);
                    $('#question_id').val(row.Id);
                    $('#type').val(row.SubjectType);
                    var addcode = $('#li_qid_' + $('#question_id').val()).html();
                    /*试题内容、综合题描述、答案（图片路径）*/
                    // json.data.content=addcode+'. '+json.data.content.replace('__URL__','/Asset/');
                    row.itemcode = addcode; //题号(题号与题干分离)
                    row.QuestionContent = row.QuestionContent.replace('__URL__', '/Asset/');
                    row.Description = row.Description.replace('__URL__', '/Asset/');
                    $('#myquestion').html(template("question_common", row));
                    if (row.Description != '') {
                        $('.subItem').show();
                    } else {
                        $('.subItem').hide();
                        $(".main").removeClass("two");
                    }
                    if (objectPaperInfo[index] === undefined) {
                        objectPaperInfo[index] = new Array();
                    }
                    objectPaperInfo[index][serialNumber] = row;
                    /***dageyang 2016/10/22 {**/
                    $(".mainLeft").height(0);
                    if ($(".mainRight").height() < $(window).height()) {
                        $(".mainLeft").height($(document).height() - 150);
                    } else {
                        $(".mainLeft").height($(".mainRight").height());
                    }
                    bindBtn();
                    /***} dageyang 2016/10/22 **/
                } else {
                    $('#myquestion').html(template("status_" + data.Error, json));
                }
            }
            );
        } else {
            var row = objectPaperInfo[index][serialNumber];
            getPartTitle(row.SubjectType, row.Description);
            $('#question_id').val(row.Id);
            $('#type').val(row.SubjectType);
            var addcode = $('#li_qid_' + $('#question_id').val()).html();
            /*试题内容、综合题描述、答案（图片路径）*/
            // json.data.content=addcode+'. '+json.data.content.replace('__URL__','/Asset/');
            row.itemcode = addcode; //题号(题号与题干分离)
            row.QuestionContent = row.QuestionContent.replace('__URL__', '/Asset/');
            row.Description = row.Description.replace('__URL__', '/Asset/');
            $('#myquestion').html(template("question_common", row));
            if (row.Description != '') {
                $('.subItem').show();
            } else {
                $('.subItem').hide();
                $(".main").removeClass("two");
            }
            /***dageyang 2016/10/22 {**/
            $(".mainLeft").height(0);
            if ($(".mainRight").height() < $(window).height()) {
                $(".mainLeft").height($(document).height() - 150);
            } else {
                $(".mainLeft").height($(".mainRight").height());
            }
            bindBtn();
        }


    };

    $('.mainLeft .title:first a:first').click();
    $('.ch_auto').click(function () {
        var tmp = $(this).attr('type');
        $('#myauto').val(tmp);
    });
    $(document).on("keydown", function (event) {
        var event = event || window.event;
        if (event.keyCode == 116) {
            if ($('#myauto').val() != 2 && $("#myauto") != 1) {
                event.preventDefault();
                $('.next').click();
                return false;
            } else {
                return false;
            }
        } else if (event.keyCode == 115) {
            if (event && event.preventDefault) {
                event.preventDefault();
            } else {
                window.event.returnValue = false;
            }
            $('.up').click();
            return false;
        }
    });

});
$(function () {
    $(document).on("click", ".titPart", function () {
        if ($(this).siblings("div").css("display") == "block") {
            $(this).parent().addClass('noshow');
            $(this).siblings("div").stop().slideUp(300);
        } else {
            $(this).parent().removeClass('noshow');
            $(this).siblings("div").stop().slideDown(300);
        }
    });
    $(document).on("click", ".full", function () {
        if ($(".main").hasClass('screen')) {
            $(".main").removeClass('screen');
            $(this).removeClass('fei');
        } else {
            $(".main").addClass("screen");
            $(this).addClass('fei');
        }
    });
    $(document).on("click", ".row", function () {
        if ($(".main").hasClass('two')) {
            $(".main").removeClass('two');
            $(this).removeClass('heng');
        } else {
            $(".main").addClass("two");
            $(this).addClass('heng');
        }
    });
    $(document).on("click", ".big", function () {
        var a = $("#myquestion .subItem p").css("font-size");
        var len = a.length - 2;
        a = a.substring(0, len);
        a++;
        var a = $("#myquestion .subItem p").css("font-size", a + "px");
    });
    $(document).on("click", ".smill", function () {
        var a = $("#myquestion .subItem p").css("font-size");
        var len = a.length - 2;
        a = a.substring(0, len);
        a--;
        var a = $("#myquestion .subItem p").css("font-size", a + "px");
    });
    //点击设置
    $(document).on("click", ".set", function (event) {
        $(this).siblings(".dropContents").slideToggle(300);
        event.stopPropagation();
    });
    //点击设置选项--设置框消失
    $(document).on("click", ".dropContents a", function (event) {
        $(this).addClass("current").siblings().removeClass("current");
        $(this).parent().parent(".dropContents").stop().slideUp(300);
        event.stopPropagation();
    });
    //点击空白--设置框消失
    $(document).on("click", function () {
        $(".dropContents").stop().slideUp(300);
    });
    $(document).on("click", ".key", function () {
        if ($(this).hasClass('current')) {
            $(this).removeClass('current');
            $(this).val("标记");
            $(".myitem.current").removeClass('addkey');
        } else {
            $(this).addClass('current');
            $(this).val("取消");
            $(".myitem.current").addClass('addkey');
        }
    })
    $(document).on("click", ".Calculator", function () {
        $("#Calculator").show();
    });
    $(document).on("click", "#cancel", function () {
        $("#Calculator").hide();
    });
});

function setColor(color) {
    if (color == 1) {
        color = "#ffff00";
    } else {
        color = "#edfade";
    }
    if (navigator.userAgent.indexOf("MSIE") > -1) {
        var tr = document.selection.createRange();
        if (tr.parentElement().className == "editor" || tr.parentElement().parentNode.nodeName == "P") {
            tr.execCommand("BackColor", false, color);
            return;
        }
    } else {
        var tr = window.getSelection().getRangeAt(0);
        if (tr.commonAncestorContainer.parentNode.className == "editor" || tr.commonAncestorContainer.parentNode.nodeName == "FONT") {
            var span = document.createElement("font");
            span.style.cssText = "background-color:" + color;
            tr.surroundContents(span);
            return;
        }
    }
}

function bindBtn() {
    $('.zkjx').click(function () {
        var jx = $(this).parent().parent().parent().siblings().next().next();
        $(jx).attr("style", "display:block;");
        $(this).attr('style', 'display:none;');
        $(this).siblings(":first").attr('style', 'display:block;');
    });
    $(".sqjx").click(function () {
        var jx = $(this).parent().parent().parent().siblings().next().next();
        $(jx).attr("style", "display:none;");
        $(this).attr('style', 'display:none;');
        $(this).siblings(":first").attr('style', 'display:block;');
    });
    $(".shoucan").click(function () {
        var data = $(this).attr("data");
        var id;
        if (data === "1") {
            id = $(this).attr('data-id');
            $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
            $(this).html("<i></i>收藏");
            $(this).attr("data", "0");
            topevery.ajax({
                url: 'api/MyCollectionItem/CancelCollectSubject',
                data: JSON.stringify({ Id: id })
            });
        } else if (data === "0") {
            id = $(this).attr('data-id');
            var that = $(this);
            $(this).html("<i></i>取消收藏");
            $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
            $(this).attr("data", "1");
            topevery.ajax({
                url: 'api/MyCollectionItem/AddCollectSubject',
                data: JSON.stringify({ QuestionId: id })
            }, function (row) {
                that.attr('data-id', row.Result.ModelId);
            });
        }
    });
}

function getPartTitle(type, desc) {
    var mytitle = '';
    if (type == 1 & desc == '') {
        mytitle = '单项选择题（共' + questionsNumber + '题，每题' + questionsScore + '分。每题的备选项中，只有1个最符合题意）';
    } else if (type == 2 & desc == '') {
        mytitle = '多项选择题（共' + questionsNumber + '题，每题' + questionsScore + '分。每题的备选项中，有2个或2个以上符合题意，至少有1个错项。错选，本题不得分；少选，所选的每个选项得' + questionsScore / 4 + '分）';
    } else if (type == 3 & desc == '') {
        mytitle = '判断题（共' + questionsNumber + '题，每题' + questionsScore + '分。每题的备选项中，只有1个正确答案）';
    } else {
        mytitle = '案例分析题（共' + questionsNumber + '题，每题' + questionsScore + '分。由单选和多选组成。错选，本题不得分；少选，所选的每个选项得' + questionsScore / 4 + '分）';
    }
    $('#mytitle').html(mytitle);
}


//function getCookie(cname) {
//    var name = cname + "=";
//    var ca = document.cookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i].trim();
//        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
//    }
//    return "";
//}

//var endTime = getCookie("time");

//var timer = setInterval(time, 1000);

//function time() {
//    var curTime = new Date().getTime();
//    var djs = endTime - curTime;
//    if (djs >= 1000) {
//        var hours = parseInt((djs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
//        var minutes = parseInt((djs % (1000 * 60 * 60)) / (1000 * 60));
//        var seconds = parseInt((djs % (1000 * 60)) / 1000);
//        $(".timer").html("剩余时间：" + hours + ":" + minutes + ":" + seconds);
//    } else {
//        clearInterval(timer);
//        $(".timer").html("剩余时间：0:0:0");
//    }
//}