var IsFree = false;
var Rows = 5;
$(function () {
    Intialize();

    function Intialize() {
        topevery.ajax({
            url: 'api/CoursePack/GetCoursePackDetail',
            data: JSON.stringify({ Id: $('#courseid').val() })
        }, function (data) {
            if (data) {
                if (data.Success) {
                    var courseinfo = data.Result;
                    console.log(courseinfo);
                    //seo相关
                    document.getElementsByTagName('meta')[1]['content'] = courseinfo.KeyWord;
                    document.getElementsByTagName('meta')[2]['content'] = courseinfo.Description;
                    document.getElementsByTagName('title')[0].innerText = courseinfo.Title;

                    $('#coursename').text(courseinfo.CourseName);
                    $('#appraiseLevel').val(courseinfo.EvaluationScore);
                    if (courseinfo.FavourablePrice <= 0) {
                        $('#favorableprice').html("&nbsp;&nbsp;&nbsp;免费&nbsp;&nbsp;&nbsp;");
                        $('#price').html('');
                        IsFree = true;
                    } else {
                        $('#price').html("¥&nbsp;" + courseinfo.Price);
                        $('#favorableprice').html("¥&nbsp;" + courseinfo.FavourablePrice);
                        IsFree = false;
                    }
                    if (courseinfo.ValidityPeriod == 0) {
                        $('#ValidityPeriod').attr('style', 'display:none;');
                        $('#validityTips').text("有效期：永久有效");
                    } else {
                        $("#ValidityPeriod").html(courseinfo.ValidityPeriod + "天");
                    }
                    $('#courseimg')[0].src = courseinfo.CourseIamge;
                    $('#teacherphoto')[0].src = courseinfo.TeacherPhoto;
                    $('#teachername').text(courseinfo.TeachersName);
                    $('#btn_course_vedio')[0].dataset.src = courseinfo.CourseVedio;
                    $('#synopsis').html(courseinfo.Synopsis);
                    $('#coursecontent').html(courseinfo.CourseContent);
                    $('#teacherinfo')[0].dataset.id = courseinfo.TeacherId;



                    if (courseinfo.CourseLongTime) {
                        $('#CourseLongTime').text(courseinfo.CourseLongTime);
                    } else {
                        $('#CourseLongTime').text(0);
                    }
                    if (courseinfo.CourseWareCount) {
                        $('#CourseWareCount').text(courseinfo.CourseWareCount);
                    } else {
                        $('#CourseWareCount').text(0);
                    }
                    if (courseinfo.CollectionNum) {
                        $('#CollectionNum').text(courseinfo.CollectionNum);
                    } else {
                        $('#CollectionNum').text(0);
                    }
                    if (courseinfo.PurchaseNum) {
                        $('#PurchaseNum').text(courseinfo.PurchaseNum);
                    } else {
                        $('#PurchaseNum').text(0);
                    }
                    if (courseinfo.AppraiseNum) {
                        $('#AppraiseNum').text(courseinfo.AppraiseNum);
                    } else {
                        $('#AppraiseNum').text(0);
                    }



                    if (courseinfo.CourseVedio) {
                        $("#btn_course_vedio").show();
                    }

                    ///子课程列表
                    $('#coursepacklist').html();
                    var contentHtml = "";
                    var arr = courseinfo.SubCourseList;
                    for (var i = 0; i < arr.length; i++) {
                        contentHtml += "<a class=\"u-recommendCourse f-cb f-db f-pr first\" target=\"_blank\"  href=\"/CourseInfo/CourseInfo?courseid=" + arr[i].Id + "\">"
                                                + "<div class=\"cImg f-fl f-pr\">"
                                                        + "<img src=\"" + arr[i].CourseIamge + "\" class=\"j-info img\">"
                                                        + "<span class=\"publicStatus f-pa j-publicStatus exclusive\"></span>"
                                                + "</div>"
                                                + "<div class=\"cInfo f-cb f-fl\">"
                                                    + "<div class=\"tit f-cb b-30\">"
                                                            + "<h4 class=\"j-info courseTit f-thide f-fl\" title=\"\">" + arr[i].CourseName + "</h4>"
                                                    + "</div>"
                                                    + "<div class=\"j-from from b-20\">" + (arr[i].TeachersName == null || arr[i].TeachersName == undefined ? "" : arr[i].TeachersName) + "</div>"
                                                    + "<ul class=\"j-info enroll f-cb\">"
                                                            + "<li class=\"j-enroll li-1  f-ib f-fl hot\" title=\"选课人数\">" + arr[i].Count + "</li>"
                                                    + "</ul>"
                                                + "</div>"
                                                //+ "<span class=\"j-price price f-ib f-pa\">"
                                                //    + "<span class=\"d\">¥ " + arr[i].Price + "</span>"
                                                //    + "<span class=\"pay\">¥ " + arr[i].FavourablePrice + "</span>"
                                                //+ "</span>"
                                        + "</a>";
                    }
                    $('#coursepacklist').html(contentHtml);
                    var level = parseInt(courseinfo.EvaluationScore, 10);
                    var on1 = "";
                    var on2 = "";
                    var on3 = "";
                    var on4 = "";
                    var on5 = "";
                    for (var i = 1; i <= level; i++) {
                        on1 = i >= 1 ? "on" : "";
                        on2 = i >= 2 ? "on" : "";
                        on3 = i >= 3 ? "on" : "";
                        on4 = i >= 4 ? "on" : "";
                        on5 = i >= 5 ? "on" : "";
                    }
                    var appraise = "<div class=\"star " + on1 + "\"></div>"
                                    + "<div class=\"star " + on2 + "\"></div>"
                                    + "<div class=\"star " + on3 + "\"></div>"
                                    + "<div class=\"star " + on4 + "\"></div>"
                                    + "<div class=\"star " + on5 + "\"></div>";
                    var tip = formatterTip(level);
                    appraise += "<span class=\"j-tip\">" + tip + "</span>";
                    $('#courseappraiselevel').html(appraise);


                }
            }
        });
        if (topevery.GetCookie("userToken")) {
            topevery.ajax({
                url: 'api/MyCollection/IsCollected',
                data: JSON.stringify({ CourseId: $('#courseid').val() })
            }, function (data) {
                if (data.Result) {
                    $('#btn_collect').addClass('collected');
                }
            })
        };


        GetAppraiseList();
        loadLearnPerson();

        if (topevery.GetCookie("userToken")) {
            topevery.ajax({
                url: 'api/Sheet/IfAreadyPay',
                data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 1 })
            }, function (data) {
                if (data.Result) {
                    $('#btnLearnNow').unbind();
                    $('#btnLearnNow>span').text('已购买');
                    //已购买，显示学习进度条
                    LoadLearnProgress();
                } else {
                    $("#AddToCart").show();
                    $('#processwrap').attr("style", 'display:none;');
                }
            });
        } else {
            $('#processwrap').attr("style", 'display:none;');
        }
    }

    function LoadLearnProgress() {
        topevery.ajax({
            url: "api/MyStudy/LoadPackProgress?courseid=" + $('#courseid').val(),
        }, function (data) {
            $('#btn_start').text('开始学习');
            if (data.Success) {
                var model = data.Result;
                if (model.HasLearnCount > 0) {
                    $('#btn_start').text('继续学习');
                } 
                $('#learnCount').text(model.HasLearnCount);
                $('.probar').attr("style", 'width:' + (model.HasLearnCount / model.CourseWareCount * 100) + "%");
            }
        })
    }

    //点击加入收藏
    $('.toStore').click(function () {
        var courseId = $('#courseid').val();
        var flag = false;
        //判断是否已经加入收藏
        topevery.ajax({
            url: "api/MyCollection/IsCollected",
            data: JSON.stringify({ CourseId: courseId })
        }, function (data) {
            flag = data.Result;
            if (!flag) {
                topevery.ajax({
                    url: 'api/MyCollection/AddCollection',
                    data: JSON.stringify({ CourseId: courseId })
                }, function (data) {
                    if (data.Result.Success) {
                        $('#btn_collect').addClass('collected');
                        $('#btn_collect').attr('title', '取消收藏');
                        layer.msg("收藏成功!");
                    }
                })
            } else {
                topevery.ajax({ url: 'api/MyCollection/CancelCollection', data: JSON.stringify({ CourseId: courseId }) }, function (data) {
                    if (data.Result.Success) {
                        $('#btn_collect').removeClass('collected');
                        $('#btn_collect').attr('title', '加入收藏');
                        layer.msg('取消收藏!');
                    }
                });
            }
        });
    })


    $('#btnLearnNow').click(function () {
        if (topevery.GetCookie("userToken")) {
            var courseId = $('#courseid').val();
            //加入购物车,跳转到订单页面
            topevery.ajax({
                url: 'api/Cart/Add',
                data: JSON.stringify({ CourseType: 1, CourseId: courseId })
            }, function (data) {
                if (data.Success) {
                    if (data.Result.Success) {
                        if (IsFree) {
                            layer.msg("参加成功!");
                            setTimeout("location.reload()", 2000);
                        } else {
                            window.open('/Cart/Index', '_self');
                        }
                    } else {
                        layer.msg(data.Result.Message);
                    }
                }
            });
        } else {
            userLogin();
        }
    })

    function loadLearnPerson() {
        topevery.ajax({
            url: 'api/CourseInfo/GetLearnList',
            data: JSON.stringify({ Page: 1, Rows: 12, CourseId: $('#courseid').val() })
        }, function (data) {
            $('.pics').html('');
            $('.j-num').text(data.Result.Records);
            $('#num').text(data.Result.Records);
            var arr = data.Result.Rows;
            if (arr.length > 0) {
                $('.pics').html(template("learnUserList_html", data.Result));
            } else {
                $('.userDefault').attr("style", "display:block;");
            }
        });
    }


    var num = finalnum = tempnum = 0;
    var lis = $(".j-solo > .u-rating > .star");
    //num:传入点亮星星的个数
    //finalnum:最终点亮星星的个数
    //tempnum:一个中间值
    function fnShow(num) {
        $(lis).removeClass('on');
        finalnum = num || tempnum;//如果传入的num为0，则finalnum取tempnum的值
        for (var i = 0; i < lis.length; i++) {
            i < finalnum ? $(lis[i]).addClass('on') : $(lis[i]).addClass('');//点亮星星就是加class为light的样式
        }
    }
    for (var i = 1; i <= lis.length; i++) {
        lis[i - 1].index = i;
        lis[i - 1].onmouseover = function () { //鼠标经过点亮星星。
            $('#tip').text(formatterTip(this.index));
            fnShow(this.index);//传入的值为正，就是finalnum
        }
        lis[i - 1].onmouseout = function () { //鼠标离开时星星变暗
            fnShow(0);//传入值为0，finalnum为tempnum,初始为0
            $('#tip').text('');
        }
        lis[i - 1].onclick = function () { //鼠标点击,同时会调用onmouseout,改变tempnum值点亮星星
            tempnum = this.index;
            $('#tip').text(formatterTip(this.index));
        }
    }

    function formatterTip(num) {
        switch (num) {
            case 1:
                return '较差';
            case 2:
                return '一般';
            case 3:
                return '良好';
            case 4:
                return '推荐';
            case 5:
                return '极佳';
            default: return '';
        }
    }

    $('#appraisetxt').focus(function () {
        $('.s-fc6').attr('style', 'display:block;');
        $('.u-cmtsb').attr('style', 'display:block;')
    })

    $('#appraisetxt').on('keydown', limitinputlenght).on('keyup', limitinputlenght);

    ///限制输入长度
    function limitinputlenght() {
        var length = $('#appraisetxt').val().length;
        if (100 - length >= 0) {
            $('.s-fc1').text(100 - length);
        }
        if (length > 100) {
            $('#appraisetxt').val($('#appraisetxt').val().substring(0, 100));
        }
    }

    GetAppraiseList();
    $('#appraisesubmit').click(function () {
        if (topevery.GetCookie("userToken")) {
            var content = $('#appraisetxt').val();
            if ($.trim(content) != "") {
                topevery.ajax({
                    url: 'api/Sheet/IfAreadyPay',
                    data: JSON.stringify({ CourseId: $('#courseid').val(),CourseType:1 })
                }, function (data) {
                    if (data.Result) {
                        topevery.ajax({
                            url: 'api/CourseAppraise/Add',
                            data: JSON.stringify({ AppraiseCotent: $('#appraisetxt').val(), AppraiseLevel: finalnum, CourseId: $('#courseid').val(), CourseType: 0 })
                        }, function (data) {
                            //成功发表评价，更新评价列表，清空评价内容
                            if (data.Success) {
                                $('#appraisetxt').val('');
                                GetAppraiseList();
                                layer.msg(data.Result.Message);
                            }
                        });

                    } else {
                        $('#appraisetxt').val('');
                        layer.msg("您尚未购买该课程，暂不能评价!");
                    }
                });
            } else {
                layer.msg("请输入评论内容!");
            }
        } else {
            userLogin();
            //layer.msg('请先登录再进行操作!', { time: 2000 });
        }
    });

    if (topevery.GetCookie("userToken")) {
        //该课程是否已购买
        topevery.ajax({
            url: 'api/Sheet/IfAreadyPay',
            data: JSON.stringify({ CourseId: $('#courseid').val(),CourseType:1 })
        }, function (data) {
            if (data.Result) {
            } else {
                $("#AddToCart").show();
            }
        });
        //添加到我的足迹
        topevery.ajax({
            url: 'api/MyCollection/AddFootprint',
            data: JSON.stringify({
                CourseId: $('#courseid').val(), CourseType: 1
            })
        }, function (data) {
        });
    } else {
        $("#AddToCart").show();
        //layer.msg("请先登录再进行操作", {time:2000});
    }

    function hasAppraise() {

    }

    $("#auto-id-1449466945302").click(function() {
        Rows += 5;
        GetAppraiseList();
    });

    function GetAppraiseList() {
        topevery.ajax({
            url: 'api/CourseChapter/GetList',
            data: JSON.stringify({ CourseId: $('#courseid').val(), Page: 1, Rows: Rows })
        }, function (data) {
            if (data.Success) {
                if (data.Result && data.Result.Rows.length > 0) {
                    //加载评价列表
                    $('#appraiselist').html('');
                    var arr = data.Result.Rows;
                    console.log(arr);
                    var contentHtml = "";
                    for (var i = 0; i < arr.length; i++) {

                        var level = parseInt(arr[i].AppraiseLevel, 10);
                        var on1 = "";
                        var on2 = "";
                        var on3 = "";
                        var on4 = "";
                        var on5 = "";
                        for (var j = 1; j <= level; j++) {
                            on1 = j >= 1 ? "on" : "";
                            on2 = j >= 2 ? "on" : "";
                            on3 = j >= 3 ? "on" : "";
                            on4 = j >= 4 ? "on" : "";
                            on5 = j >= 5 ? "on" : "";
                        }
                        var appraise = "<div class=\"star " + on1 + "\"></div>"
                                        + "<div class=\"star " + on2 + "\"></div>"
                                        + "<div class=\"star " + on3 + "\"></div>"
                                        + "<div class=\"star " + on4 + "\"></div>"
                                        + "<div class=\"star " + on5 + "\"></div>";
                        var tip = formatterTip(level);
                        appraise += "<span class=\"j-tip\">" + tip + "</span>";
                        console.log(arr[i].AppraiseCotent);
                        contentHtml += "<div class=\"u-cmt\">"
                                            + "<div class=\"wrp f-cb f-pr\">"
                                                + "<img class=\"~/Libs/CourseInfo/imgs/j-info f-cb\" alt=\"\"  src=\"" + (arr[i].HeadImage ? arr[i].HeadImage : '/Libs/CourseInfo/imgs/small.jpg') + "\" data-cid=\"\">"
                                                + "<div class=\"info\">"
                                                      + "<p class=\"presoninfo j-personinfo name\"  data-cid=\"\"> <span class=\"name s-fc3 f-fs0 f-thide j-info\" >" + arr[i].UserName + "</span> </p>"
                                                      + " <span class=\"time s-fc2 f-fs0 j-info\">" + dateFormatter(arr[i].AppraiseTime) + "</span>"
                                                      + "<div class=\"rate j-rate\">"
                                                            + "<div class=\"u-rating\">"
                                                                + appraise
                                                            + "</div>"
                                                      + "</div>"
                                                + "</div>"
                                                + "<a class=\"f-pa reply s-fc3 j-replyBtn\"  style=\"display: none;\">回复</a>"
                                            + "</div>"
                                            + "<div class=\"cnt s-fc5 f-fs0 j-info\">" + (arr[i].AppraiseCotent == null || arr[i].AppraiseCotent == undefined ? "" : arr[i].AppraiseCotent) + "</div>"
                                            + "<div class=\"j-reply\"></div>"
                                        + "</div>";
                    }
                    $('#appraiselist').html(contentHtml);
                } else {
                    //提示没有评价
                    $('.j-al-null').attr('style', 'display:none;');
                }
            }
        });
    }

    //取消评价
    $('#cancelsubmit').click(function () {
        $('.s-fc6').attr('style', 'display:none;');
        $('.u-cmtsb').attr('style', 'display:none;')
    });

    function dateFormatter(datetime) {
        datetime = new Date(datetime);
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1;
        var day = datetime.getDate();
        return year + '年' + month + '月' + day + "日";
    }

    //点击播放视频
    $('.cliBtn').click(function () {
        $('#video-parent').attr('style', 'height:100%;background:#000;display:block;');
        $('.course_cover').attr('style', 'display:none');
        var video = document.getElementById("coursevideo");
        var src = $('#btn_course_vedio')[0].dataset.src;
        console.log('src', src);
        if (src == '') {
            $('#coursevideo').attr('style', "display:none;");
            $('.notips').attr('style', "display:block");
        } else {
            var srcobj = $("<source src=\"" + src + "\" type=\"video/mp4\">")
            $('#coursevideo').append(srcobj);
            video.play();
        }
    });
    $("#AddToCart").click(function () {
        var courseId = $("#courseid").val();
        topevery.ajax({
            url: 'api/Cart/Add',
            data: JSON.stringify({ CourseType: 1, CourseId: courseId })
        }, function (data) {
            if (data.Success) {
                if (data.Result.Success) {
                    layer.msg(data.Result.Message);
                    $(".CartCount").html(parseInt($(".CartCount").html()) + 1);
                } else {
                    layer.msg(data.Result.Message);
                }
            }
        });
    });
})


