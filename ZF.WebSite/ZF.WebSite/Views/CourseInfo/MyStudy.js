document.addEventListener("error", function (e) {
    var elem = e.target;
    if (elem.tagName.toLowerCase() == 'img') {
        elem.src = "/Libs/CourseInfo/imgs/temp_couver.jpg";
    }
}, true);
$(function () {
    //个人信息
    GetPersonalInfo();
    GetPurchaseList();
    $('.ux-tabs_hd>li').click(function () {
        var value = $(this).attr("data");
        var dataid = $(this).attr("dataid");
        $('.ux-tabs_hd li').each(function () {
            $(this).removeClass('z-crt');
        });
        $(".lihide").hide();
        $(this).addClass("z-crt");
        if (value == "1") {
            GetCollectionList();
            $("#" + dataid).attr('style', 'display:block;height:1200px');
        } else if (value == "2") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetItemList();
        } else if (value == "0") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetPurchaseList();
        } else if (value == "4") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetExerciseRecords();
        } else if (value == "5") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetPaperRecords();
        } else if (value == "3") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetErrList();
        } else if (value == "6") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetFootprint();
        } else if (value == "7") {
            $("#" + dataid).attr('style', 'display:block;height:1200px');
            GetVideoWatch();
        }
    })

    ///收藏课程
    function GetCollectionList() {
        $('#mycollectionlist').html('');
        topevery.ajax({
            url: 'api/MyStudy/GetCollectedCourseList',
            data: JSON.stringify({ UserId: '' }),
            async: false
        }, function (data) {
            //console.log(data);
            if (data.Success && data.Result.length > 0) {
                var arr = data.Result;
                var contentHtml = "";
                for (var i = 0; i < arr.length; i++) {
                    contentHtml += "<li class=\"uc-course-list_itm f-ib\">"
                                        + "<div class=\"uc-coursecard uc-ykt-coursecard f-fl\">"
                                            + "<a class=\"j-href\" href='CourseInfo/CourseInfo?courseId=" + arr[i].Id + "&courseType=" + arr[i].CourseType + "'>"
                                                 + "<div class=\"uc-ykt-coursecard-wrap f-cb f-pr\">"
                                                        + "<div class=\"uc-ykt-coursecard-wrap_box\">"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_picbox f-pr\">"
                                                                + "<img class=\"imgPic j-img\" src=\"" + arr[i].CourseIamge + "\" alt=\"" + arr[i].TeachersName + "\" />"
                                                            + "</div>"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_tit\">"
                                                                 + "<h3 class=\"\">" + arr[i].CourseName + "</h3>"
                                                            + "</div>"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_orgName f-fs0 f-thide\">"
                                                                + (arr[i].TeachersName == null || arr[i].TeachersName == "" ? "" : arr[i].TeachersName)
                                                            + "</div>"
                                                            //+ " <div class=\"uc-ykt-coursecard-wrap_price f-pa\">"
                                                            //    + "<span class=\"u-discount\">¥ " + arr[i].FavourablePrice + "</span>"
                                                            //    + "<span class=\"u-normal z-discount\">¥" + arr[i].Price + "</span>"
                                                            //+ "</div>"
                                                        + "</div>"
                                                + "</div>"
                                            + "</a>"
                                        + "</div>"
                                    + "</li>";
                }
                $('#mycollectionlist').html(contentHtml);

                //设置高度
                document.getElementById('mycollectionlist').style.height = document.getElementById("mycollectionlist").cssHeight; +document.getElementById("mycollectionlist").offsetTop + "px";
            } else {
                $('#j-cpcourse-empty_collect').attr("style", "display:block");
            }
        });
    }
    ///购买课程
    function GetPurchaseList() {
        $('#mypurchaselist').html('');
        topevery.ajax({
            url: 'api/MyStudy/GetPurchaseList',
            data: JSON.stringify({ UserId: '' })
        }, function (data) {
            if (data.Success && data.Result.length > 0) {
                console.log(data.Result);
                var arr = data.Result;
                var contentHtml = "";
                for (var i = 0; i < arr.length; i++) {
                    var showProgress = arr[i].HasLearnCount == 0 ? "<a class=\"uc-ykt-course-card_btn\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "&courseType=" + arr[i].CourseType + "\" target=\"_blank\">开始学习</a>" : arr[i].HasLearnCount == arr[i].CourseWareCount ? "<div class=\"uc-ykt-course-card_done\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "&courseType=" + arr[i].CourseType + "\" target=\"_blank\">已完成</div>" : arr[i].HasLearnCount > 0 ? "<div class=\"uc-ykt-course-card_progress\"><div class=\"uc-ykt-course-card_progress_all\"><div class=\"uc-ykt-course-card_progress_current\" style=\"width: " + (parseFloat(arr[i].HasLearnCount) / parseFloat(arr[i].CourseWareCount) * 100) + "%;\"></div></div><div class=\"uc-ykt-course-card_progress_txt\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "&courseType=" + arr[i].CourseType + "\" target=\"_blank\">已学习" + arr[i].HasLearnCount + "/" + arr[i].CourseWareCount + "课时</div></div>" : "";
                    contentHtml += "<li class=\"uc-course-list_itm f-ib\">"
                                        + "<div class=\"uc-coursecard uc-ykt-coursecard f-fl\">"
                                            + "<a class=\"j-href\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "&courseType=" + arr[i].CourseType + "\" target=\"_blank\">"
                                                 + "<div class=\"uc-ykt-coursecard-wrap f-cb f-pr\">"
                                                        + "<div class=\"uc-ykt-coursecard-wrap_box\">"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_picbox f-pr\">"
                                                                + "<img class=\"imgPic j-img\" src=\"" + arr[i].CourseIamge + "\" alt=\"" + arr[i].TeachersName + "\" />"
                                                            + "</div>"
                                                            + "<div class=\"uc-ykt-course-card_title\">"
                                                                 + arr[i].CourseName
                                                            + "</div>"
                                                            //+ "<div class=\"uc-ykt-coursecard-wrap_orgName f-fs0 f-thide\">"
                                                            //    + (arr[i].TeachersName == null || arr[i].TeachersName == "" ? "" : arr[i].TeachersName)
                                                            //+ "</div>"
                                                            + showProgress
                                                            //+ "<a class=\"uc-ykt-course-card_btn\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "\" target=\"_blank\">开始学习</a>"
                                                            //+"<div class=\"uc-ykt-course-card_done\" href=\"CourseInfo/CourseInfo?courseId=" + arr[i].Id + "\" target=\"_blank\">已完成</div>"
                                                            //+ " <div class=\"uc-ykt-coursecard-wrap_price f-pa\">"
                                                            //    + "<span class=\"u-discount\">¥ " + arr[i].FavourablePrice + "</span>"
                                                            //    + "<span class=\"u-normal z-discount\">¥" + arr[i].Price + "</span>"
                                                            //+ "</div>"
                                                        + "</div>"
                                                + "</div>"
                                            + "</a>"
                                        + "</div>"
                                    + "</li>";
                }
                $('#mypurchaselist').html(contentHtml);


                //设置高度
                document.getElementById('mypurchaselist').style.height = document.getElementById("mypurchaselist").cssHeight + document.getElementById("mypurchaselist").offsetTop + "px";
            } else {
                $('#j-cpcourse-empty_purchase').attr("style", "display:block");
            }
        });
    }
    //收藏试题
    function GetItemList() {
        var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //加载我的试题
        $('#subjectlist').html('');
        topevery.ajax({
            url: 'api/CourseSubject/GetCollectedSubjectList',
            data: JSON.stringify({ Page: $('#PageIndex').val(), Rows: $('#Rows').val() })
        }, function (data) {
            if (data.Success) {
                var contentHtml = "";
                var arr = data.Result.Rows;
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var optionHtml = "";
                        var optionCount = parseInt(arr[i].Number);
                        for (var j = 0; j < optionCount; j++) {
                            optionHtml += "<dd class=\"m-question-option cho-this\">"
                                               + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                               + eval("arr[i].Option" + (j + 1))
                                           + "</dd>"
                        }

                        contentHtml += "<li style=\"display: block;\">"
                                            + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                                + "<div class=\"subject-con\" style=\"background:inherit\">"
                                                    + "<div class=\"sub-content sub-conanswer\">"
                                                        + "<div class=\"sub-dotitle\">"
                                                             + "<em>" + (i + 1) + "</em>"
                                                             + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                             + arr[i].QuestionContent
                                                        + "</div>"
                                                        + "<dl class=\"sub-answer  sub-answer-no\">"
                                                                 + optionHtml
                                                        + "</dl>"
                                                    + "</div>"
                                                + "</div>"
                                                + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                                    + "<div class=\"reck\">"
                                                        + "参考答案：<em class=\"right\">" + formatterRightAnswer(arr[i].RightAnswer) + "</em>"
                                                    + "</div>"
                                                    + "<ul>"
                                                        + " <li class=\"nobro\">"
                                                        + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                                + "<i></i>取消收藏"
                                                        + "</a>"
                                                        + "<li>"
                                                                + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                                + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                        + "</li>"
                                                    + "</ul>"
                                                + "</div>"
                                                //解析块
                                                + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                                    + "<div class=\"solution\">"
                                                        + "<ul>"
                                                            + "<li class=\"da-list\">"
                                                                  + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                                  + "<div class=\"so-timu\">"
                                                                        + "<div class=\"wenzi\">"
                                                                            + "参考答案：" + formatterRightAnswer(arr[i].RightAnswer)
                                                                            + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                        + "</div>"
                                                                  + "</div>"
                                                            + "</li>"
                                                        + "</ul>"
                                                    + "</div>"
                                            + "</div>"
                                      + "</li>"
                    }

                    $('#subjectlist').html(contentHtml);
                    //console.log(document.getElementById("subjectlist").offsetHeight, document.getElementById("subjectlist").offsetTop);
                    //document.getElementById('subjectlist').style.height = document.getElementById("subjectlist").offsetHeight + document.getElementById("subjectlist").offsetTop-200 + "px";

                    $(".shoucan").click(function () {
                        var data = $(this).attr("data");
                        if (data == "1") {
                            var id = $(this).data('collectionid');
                            console.log(this);
                            console.log(id);
                            $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
                            $(this).html("<i></i>收藏");
                            $(this).attr("data", "0");
                            topevery.ajax({
                                url: 'api/MyCollectionItem/CancelCollectSubject',
                                data: JSON.stringify({ Id: id })
                            });
                        } else if (data == "0") {
                            var questionid = $(this).data('id');
                            console.log(questionid);
                            $(this).html("<i></i>取消收藏")
                            $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
                            $(this).attr("data", "1");
                            topevery.ajax({
                                url: 'api/MyCollectionItem/AddCollectSubject',
                                data: JSON.stringify({ QuestionId: questionid })
                            });



                        }
                    })
                    $('.zkjx').click(function () {
                        var jx = $(this).parent().parent().parent().siblings().next().next();
                        $(jx).attr("style", "display:block;")
                        $(this).attr('style', 'display:none;')
                        $(this).siblings(":first").attr('style', 'display:block;')
                    })
                    $(".sqjx").click(function () {
                        var jx = $(this).parent().parent().parent().siblings().next().next();
                        $(jx).attr("style", "display:none;")
                        $(this).attr('style', 'display:none;')
                        $(this).siblings(":first").attr('style', 'display:block;')
                    })


                    $('#pageBox').show();
                    $('#pageBox').pagination({
                        pageCount: Math.ceil(data.Result.Records / parseInt($('#Rows').val())),
                        jump: true,
                        console: true,
                        homePage: '首页',
                        endPage: '末页',
                        prevContent: '上页',
                        nexContent: '下页',
                        callback: function (api) {
                            $("#PageIndex").val(api.getCurrent());
                            GetItemList2();
                        }
                    });
                } else {
                    $('#j-cpcourse-empty_collectitem').attr("style", 'display:block;');
                    $('#pageBox').hide();

                }
            }
        });
    }
    //加载收藏试题 非分页
    function GetItemList2() {
        var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        topevery.ajax({
            url: 'api/CourseSubject/GetCollectedSubjectList',
            data: JSON.stringify({ Page: $('#PageIndex').val(), Rows: $('#Rows').val() })
        }, function (data) {
            if (data.Success) {
                var contentHtml = "";
                var arr = data.Result.Rows;
                for (var i = 0; i < arr.length; i++) {
                    var optionHtml = "";
                    var optionCount = parseInt(arr[i].Number);
                    for (var j = 0; j < optionCount; j++) {
                        optionHtml += "<dd class=\"m-question-option cho-this\">"
                                           + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                           + eval("arr[i].Option" + (j + 1))
                                       + "</dd>"
                    }

                    contentHtml += "<li style=\"display: block;\">"
                                        + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                            + "<div class=\"subject-con\" style=\"background:inherit\">"
                                                + "<div class=\"sub-content sub-conanswer\">"
                                                    + "<div class=\"sub-dotitle\">"
                                                         + "<em>" + (i + 1) + "</em>"
                                                         + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                         + arr[i].QuestionContent
                                                    + "</div>"
                                                    + "<dl class=\"sub-answer  sub-answer-no\">"
                                                             + optionHtml
                                                    + "</dl>"
                                                + "</div>"
                                            + "</div>"
                                            + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                                + "<div class=\"reck\">"
                                                    + "参考答案：<em class=\"right\">" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1) + "</em>"
                                                + "</div>"
                                                + "<ul>"
                                                    + " <li class=\"nobro\">"
                                                    + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                            + "<i></i>取消收藏"
                                                    + "</a>"
                                                    + "<li>"
                                                            + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                            + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                    + "</li>"
                                                + "</ul>"
                                            + "</div>"
                                            //解析块
                                            + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                                + "<div class=\"solution\">"
                                                    + "<ul>"
                                                        + "<li class=\"da-list\">"
                                                              + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                              + "<div class=\"so-timu\">"
                                                                    + "<div class=\"wenzi\">"
                                                                        + "参考答案：" + azstr.substr(parseInt(arr[i].RightAnswer) - 1, 1)
                                                                        + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                    + "</div>"
                                                              + "</div>"
                                                        + "</li>"
                                                    + "</ul>"
                                                + "</div>"
                                        + "</div>"
                                  + "</li>"
                }
                //console.log(document.getElementById("subjectlist").offsetHeight, document.getElementById("subjectlist").offsetTop);
                //document.getElementById('subjectlist').style.height = document.getElementById("subjectlist").offsetHeight + document.getElementById("subjectlist").offsetTop + "px";
                $('#subjectlist').html(contentHtml);

                $(".shoucan").click(function () {
                    var data = $(this).attr("data");
                    if (data == "1") {
                        var id = $(this).data('collectionid');
                        console.log(this);
                        console.log(id);
                        $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
                        $(this).html("<i></i>收藏");
                        $(this).attr("data", "0");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/CancelCollectSubject',
                            data: JSON.stringify({ Id: id })
                        });
                    } else if (data == "0") {
                        var questionid = $(this).data('id');
                        console.log(questionid);
                        $(this).html("<i></i>取消收藏")
                        $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
                        $(this).attr("data", "1");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/AddCollectSubject',
                            data: JSON.stringify({ QuestionId: questionid })
                        });



                    }
                })
                $('.zkjx').click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:block;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })
                $(".sqjx").click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:none;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })
            }
        });
    }
    //加载我的错题
    function GetErrList() {
        var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        topevery.ajax({
            url: 'api/MyErrorSubject/GetList',
            data: JSON.stringify({ Page: $('#PageIndex_err').val(), Rows: $('#Rows').val() })
        }, function (data) {
            if (data.Success) {
                var contentHtml = "";
                var arr = data.Result.Rows;
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var optionHtml = "";
                        var optionCount = parseInt(arr[i].Number);
                        for (var j = 0; j < optionCount; j++) {
                            optionHtml += "<dd class=\"m-question-option cho-this\">"
                                               + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                               + eval("arr[i].Option" + (j + 1))
                                           + "</dd>"
                        }

                        contentHtml += "<li style=\"display: block;\">"
                                            + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                                + "<div class=\"subject-con\" style=\"background:inherit\">"
                                                    + "<div class=\"sub-content sub-conanswer\">"
                                                        + "<div class=\"sub-dotitle\">"
                                                             + "<em>" + (i + 1) + "</em>"
                                                             + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                             + arr[i].QuestionContent
                                                        + "</div>"
                                                        + "<dl class=\"sub-answer  sub-answer-no\">"
                                                                 + optionHtml
                                                        + "</dl>"
                                                    + "</div>"
                                                + "</div>"
                                                + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                                    + "<div class=\"reck\">"
                                                        + "参考答案：<em class=\"right\">" + formatterRightAnswer(arr[i].RightAnswer) + "</em>"
                                                    + "</div>"
                                                    + "<div class=\"reck\">"
                                                        + "我的答案：<em class=\"right\">" + formatterRightAnswer(arr[i].StuAnswer) + "</em>"
                                                    + "</div>"
                                                    + "<ul>"
                                                        //+ " <li class=\"nobro\">"
                                                        //    + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                        //            + "<i></i>取消收藏"
                                                        //    + "</a>"
                                                        //+"</li>"
                                                        + "<li>"
                                                                + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                                + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                        + "</li>"
                                                    + "</ul>"
                                                + "</div>"
                                                //解析块
                                                + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                                    + "<div class=\"solution\">"
                                                        + "<ul>"
                                                            + "<li class=\"da-list\">"
                                                                  + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                                  + "<div class=\"so-timu\">"
                                                                        + "<div class=\"wenzi\">"
                                                                            + "参考答案：" + formatterRightAnswer(arr[i].RightAnswer)
                                                                            + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                        + "</div>"
                                                                  + "</div>"
                                                            + "</li>"
                                                        + "</ul>"
                                                    + "</div>"
                                            + "</div>"
                                      + "</li>"
                    }
                    $('#errlist').html(contentHtml);


                    $('#pageBox_err').show();
                    $('#pageBox_err').pagination({
                        pageCount: Math.ceil(data.Result.Records / parseInt($('#Rows').val())),
                        jump: true,
                        console: true,
                        homePage: '首页',
                        endPage: '末页',
                        prevContent: '上页',
                        nexContent: '下页',
                        callback: function (api) {
                            $("#PageIndex_err").val(api.getCurrent());
                            GetErrList2();
                        }
                    });
                } else {
                    $('#j-cpcourse-empty_error').attr('style', 'display:block;');
                    $('#pageBox_err').hide();
                }

                $(".shoucan").click(function () {
                    var data = $(this).attr("data");
                    if (data == "1") {
                        var id = $(this).data('collectionid');
                        console.log(this);
                        console.log(id);
                        $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
                        $(this).html("<i></i>收藏");
                        $(this).attr("data", "0");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/CancelCollectSubject',
                            data: JSON.stringify({ Id: id })
                        });
                    } else if (data == "0") {
                        var questionid = $(this).data('id');
                        console.log(questionid);
                        $(this).html("<i></i>取消收藏")
                        $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
                        $(this).attr("data", "1");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/AddCollectSubject',
                            data: JSON.stringify({ QuestionId: questionid })
                        });



                    }
                })
                $('.zkjx').click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:block;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })
                $(".sqjx").click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:none;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })

                //console.log(document.getElementById("errlist").offsetHeight, document.getElementById("errlist").offsetTop);
                //document.getElementById('errlist').style.height = document.getElementById("errlist").offsetHeight + document.getElementById("errlist").offsetTop + "px";
            }
        });
    }
    //加载我的练习记录
    function GetExerciseRecords() {
        topevery.ajax({
            url: 'api/ChapterExerciseRecord/GetExerciseRecords'
        }, function (data) {
            if (data.Success) {
                var content = "";
                if (data.Result.length > 0) {
                    var list = data.Result;
                    for (var i = 0; i < list.length; i++) {
                        content += "<tr ><td>" + list[i].CourseName + "</td><td>" + list[i].CapterName + "</td><td>" + list[i].AddTime + "</td><td>" + list[i].PracticeNo + "</td><td><span  data-id=\"" + list[i].Id + "\"  style=\"cursor:pointer;color:blue;\" onclick=\"checkChapterExercise(this)\">查看<span></td></tr>";
                    }
                } else {
                    content = "<tr style=\"align-content:center;\">暂无记录</tr>";
                }
                console.log(document.getElementById("j-cp-box-exercise").cssHeight, document.getElementById("j-cp-box-exercise").offsetTop);
                $('#ex_body').html(content);
                document.getElementById('j-cp-box-exercise').style.height = document.getElementById("j-cp-box-exercise").clientHeight + document.getElementById("j-cp-box-exercise").offsetTop + "px";
            }
        })
    }
    //加载我的测评记录
    function GetPaperRecords(){
        topevery.ajax({
            url: 'api/CoursePaperRecord/GetList'
        }, function (data) {
            console.log(data);
            if (data.Success) {
                if (data.Result.length > 0) {
                    var content = "";
                    var list = data.Result;
                    for (var i = 0; i < list.length; i++) {
                        content += "<tr><td>" + list[i].CourseName + "</td><td>" + list[i].PaperName + "</td><td>" + list[i].AddTime + "</td><td>" + list[i].PracticeNo + "</td><td>"+(list[i].Score+"/"+list[i].ScoreSum)+"</td><td><sapn style=\"cursor:pointer;color:blue;\" data-paperrecordid=\""+list[i].Id+"\"  data-paperid=\"" + list[i].PaperId + "\" onclick=\"checkPaperRecord(this);\">查看</span></td></tr>";
                    }
                } else {
                    content = "<tr style=\"align-content:center;\">暂无记录</tr>";
                }
                $('#paper_body').html(content);
                document.getElementById('j-cp-box-paper').style.height = document.getElementById("j-cp-box-paper").clientHeight  + document.getElementById("j-cp-box-paper").offsetTop + "px";
            }
        })
    }
    //加载我的错题  非分页
    function GetErrList2() {
        var azstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        topevery.ajax({
            url: 'api/MyErrorSubject/GetList',
            data: JSON.stringify({ Page: $('#PageIndex_err').val(), Rows: $('#Rows').val() })
        }, function (data) {
            if (data.Success) {
                var contentHtml = "";
                var arr = data.Result.Rows;
                console.log(arr);
                for (var i = 0; i < arr.length; i++) {
                    var optionHtml = "";
                    var optionCount = parseInt(arr[i].Number);
                    for (var j = 0; j < optionCount; j++) {
                        optionHtml += "<dd class=\"m-question-option cho-this\">"
                                           + "<i></i><span>" + azstr.substr(j, 1) + ".</span>"
                                           + eval("arr[i].Option" + (j + 1))
                                       + "</dd>"
                    }

                    contentHtml += "<li style=\"display: block;\">"
                                        + "<div class=\"subject-con bor clearfix m-question disabled\"  style=\"\">"
                                            + "<div class=\"subject-con\" style=\"background:inherit\">"
                                                + "<div class=\"sub-content sub-conanswer\">"
                                                    + "<div class=\"sub-dotitle\">"
                                                         + "<em>" + (i + 1) + "</em>"
                                                         + "<i>[" + formatterSubjectType(arr[i].SubjectType) + "]</i>"
                                                         + arr[i].QuestionContent
                                                    + "</div>"
                                                    + "<dl class=\"sub-answer  sub-answer-no\">"
                                                             + optionHtml
                                                    + "</dl>"
                                                + "</div>"
                                            + "</div>"
                                            + "<div class=\"m__answerLine refer-answer clearfix\"  style=\"display:block;height:10px;\">"
                                                + "<div class=\"reck\">"
                                                    + "参考答案：<em class=\"right\">" + formatterRightAnswer(arr[i].RightAnswer) + "</em>"
                                                + "</div>"
                                                 + "<div class=\"reck\">"
                                                    + "我的答案：<em class=\"right\">" + formatterRightAnswer(arr[i].StuAnswer) + "</em>"
                                                + "</div>"
                                                + "<ul>"
                                                    //+ " <li class=\"nobro\">"
                                                    //    + "<a data-collectionid=\"" + arr[i].CollectionId + "\"  data-id=\"" + arr[i].Id + "\" class=\"btn__scbt scbt this shoucan\" data=\"1\">"
                                                    //            + "<i></i>取消收藏"
                                                    //    + "</a>"
                                                    //+"</li>"
                                                    + "<li>"
                                                            + "<a  class=\"btn__zkjx zkjx\"><i></i>展开解析</a>"
                                                            + "<a  class=\"btn__sqjx sqjx\" style=\"display:none;\"><i></i>收起解析</a>"
                                                    + "</li>"
                                                + "</ul>"
                                            + "</div>"
                                            //解析块
                                            + "<div class=\"m__analyse detail-intro\"  style=\"display:none;\">"
                                                + "<div class=\"solution\">"
                                                    + "<ul>"
                                                        + "<li class=\"da-list\">"
                                                              + " <span class=\"s-jx\"><i></i>参考解析：</span>"
                                                              + "<div class=\"so-timu\">"
                                                                    + "<div class=\"wenzi\">"
                                                                        + "参考答案：" + formatterRightAnswer(arr[i].RightAnswer)
                                                                        + (arr[i].QuestionTextAnalysis ? arr[i].QuestionTextAnalysis : "")
                                                                    + "</div>"
                                                              + "</div>"
                                                        + "</li>"
                                                    + "</ul>"
                                                + "</div>"
                                        + "</div>"
                                  + "</li>"
                }
                $('#errlist').html(contentHtml);
                //console.log(document.getElementById("errlist").cssHeight, document.getElementById("errlist").clientHeight);
                //document.getElementById('errlist').style.height = document.getElementById("errlist").clientHeight + document.getElementById("errlist").offsetTop + "px";

                $(".shoucan").click(function () {
                    var data = $(this).attr("data");
                    if (data == "1") {
                        var id = $(this).data('collectionid');
                        console.log(this);
                        console.log(id);
                        $(this).removeClass("btn__scbt scbt this").addClass("btn__scbt scbt that");
                        $(this).html("<i></i>收藏");
                        $(this).attr("data", "0");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/CancelCollectSubject',
                            data: JSON.stringify({ Id: id })
                        });
                    } else if (data == "0") {
                        var questionid = $(this).data('id');
                        console.log(questionid);
                        $(this).html("<i></i>取消收藏")
                        $(this).removeClass("btn__scbt scbt that").addClass("btn__scbt scbt this");
                        $(this).attr("data", "1");
                        topevery.ajax({
                            url: 'api/MyCollectionItem/AddCollectSubject',
                            data: JSON.stringify({ QuestionId: questionid })
                        });



                    }
                })
                $('.zkjx').click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:block;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })
                $(".sqjx").click(function () {
                    var jx = $(this).parent().parent().parent().siblings().next().next();
                    $(jx).attr("style", "display:none;")
                    $(this).attr('style', 'display:none;')
                    $(this).siblings(":first").attr('style', 'display:block;')
                })
            }
        });
    }
    //个人信息
    function GetPersonalInfo() {
        topevery.ajax({
            url: 'api/Register/GetOne'
        }, function (data) {
            if (data.Success) {
                var time = new Date();
                var html = "";
                if (time.getHours() > 5 && time.getHours() < 12) {
                    html = ",上午好!";
                } else if (time.getHours() > 1 && time.getHours() < 18) {
                    html = ",下午好!";
                } else if (time.getHours() > 18 && time.getHours() < 23) {
                    html = ",晚上好!";
                } else {
                    html = ",深夜好!";
                }
                $('#registeruser').text(data.Result.NickNamw + html);
                $('#userimage').attr('src', data.Result.HeadImage);
            }
        });
    }
    //加载我的足迹
    function GetFootprint() {
        topevery.ajax({
            url: 'api/MyCollection/GetFootprint',
            data: JSON.stringify({ Type: 0 })
        }, function (data) {
            if (data.Success) {
                $("#Footprint").html(template("Footprint_html", data));
            }
        });
    };
    //获取我的视频观看记录
    function GetVideoWatch() {
        topevery.ajax({
            url: 'api/MyCollection/GetVideoWatch',
            data: JSON.stringify({ Type: 0 })
        }, function (data) {
            if (data.Success) {
                $("#VideoWatch").html(template("VideoWatch_html", data));
            }
        });
    }
})
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

function checkPaperRecord(e) {
    var paperId = $(e).data('paperid');
    var paperRecordsId = $(e).data("paperrecordid");
    topevery.myopen("/Answer/Index?paperId=" + paperId + "&paperRecordsId=" + paperRecordsId);
}

function checkChapterExercise(e) {
    var id = $(e).data("id");
    topevery.openClick("/Answer/ChapterPracticeView?&ChapterQuestionsId=" + id);
}
