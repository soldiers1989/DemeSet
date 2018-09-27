

var IsFree = false;
var teachers = [];
var currentTeacherIndex = 0;
$(function () {
    var hasPay = false;
    Intialize();
    function Intialize() {
        topevery.ajax({
            url: "api/CourseInfo/GetOne",
            data: JSON.stringify({ Id: $('#courseid').val() })
        }, function (data) {
            if (data) {
                if (data.Success) {
                    var courseinfo = data.Result;

                    //seo相关
                    //document.getElementsByTagName('meta')[1]['content'] = courseinfo.KeyWord;
                    //document.getElementsByTagName('meta')[2]['content'] = courseinfo.Description;
                    //document.getElementsByTagName('title')[0].innerText = courseinfo.Title;

                    $('#coursename').text(courseinfo.CourseName);
                    if (courseinfo.FavourablePrice <= 0) {
                        $('#favorableprice').html("&nbsp;&nbsp;&nbsp;免费&nbsp;&nbsp;&nbsp;");
                        $('#price').html('');
                        IsFree = true;
                    } else {
                        $('#price').html("原价：¥&nbsp;" + courseinfo.Price);
                        $('#favorableprice').html("¥&nbsp;" + courseinfo.FavourablePrice);
                        IsFree = false;
                    }
                    $("title").html(courseinfo.CourseName);
                    if (courseinfo.ValidityPeriod == 0) {
                        $('#ValidityPeriod').attr('style', 'display:none;');
                        $('#validityTips').text("有效期：永久有效");
                    } else {
                        $("#ValidityPeriod").html(courseinfo.ValidityPeriod + "天");
                    }
                    $('#courseimg')[0].src = courseinfo.CourseIamge;
                    //多个讲师
                    var teacher = "";
                    try {
                        if (courseinfo.TeacherList && courseinfo.TeacherList.length > 1) {
                            $('#turn_area').attr("style", 'display:block;');
                            teacher = courseinfo.TeacherList[0];
                            teachers = courseinfo.TeacherList;
                        } else if (courseinfo.TeacherList.length == 1) {
                            teacher = courseinfo.TeacherList[0];
                        }
                    } catch (e) {

                    }
                    $('#teacherphoto')[0].src = teacher.TeacherPhoto == null || teacher.TeacherPhoto == "" ? "/Libs/CourseInfo/imgs/big.jpg" : teacher.TeacherPhoto;
                    $('#teachername').text(teacher.TeachersName == null || teacher.TeachersName == undefined ? "" : teacher.TeachersName);
                    $('#synopsis').html(teacher.Synopsis == null || teacher.Synopsis == undefined ? "" : teacher.Synopsis);
                    $('#teacherinfo')[0].dataset.id = teacher.Id;



                    $('#coursecontent').html(courseinfo.CourseContent);


                    //$('#btn_course_vedio')[0].dataset.src = courseinfo.CourseVedio;
                    var player = new Aliplayer({
                        id: 'J_prismPlayer',
                        width: '98%',
                        height: '98%',
                        autoplay: false,
                        format: "m3u8",
                        vid: courseinfo.CourseVedio,
                        source: courseinfo.CourseVedio,
                    }, function (player) {
                    });
                    //点击播放视频
                    $('.cliBtn').click(function () {
                        $('#video-parent').attr('style', 'height:100%;background:#000;display:block;');
                        $('.course_cover').attr('style', 'display:none');
                        var video = document.getElementById("coursevideo");
                        var src = $('#btn_course_vedio')[0].dataset.src;
                        var srcobj = $("<source src=\"" + src + "\" type=\"video/mp4\">")
                        $('#coursevideo').append(srcobj);
                        player.play();
                    });



                    //$(".j-djMark").addClass(courseinfo.Code);

                    if (courseinfo.CourseLongTimes) {
                        $('#CourseLongTime').text(courseinfo.CourseLongTimes);
                    } else {
                        $(".m20").hide();
                        $('#CourseLongTime').text(0 + "分钟");
                    }
                    if (courseinfo.CourseWareCounts) {
                        $('#CourseWareCount').text(courseinfo.CourseWareCounts);
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
            });
            //购买的课程才显示学习进度'
            IfHasPay();
            if (hasPay) {
                //已登陆，显示个人课程学习进度   
                LearnProgress();
            } else {
                $('#processwrap').attr("style", 'display:none;');
            }
        } else {
            $('#processwrap').attr("style", 'display:none;');
        }
        GetAppraiseList();
        loadLearnPerson();
        //GetAppraise();
        //更新浏览量
        topevery.ajax({
            url: 'api/CourseInfo/UpdateViewCount',
            data: JSON.stringify({ Id: $('#courseid').val() })
        });
    }

    function IfHasPay() {
        topevery.ajax({
            url: 'api/Sheet/IfAreadyPay',
            data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 0 }),
            async: false
        }, function (data) {
            if (data.Result) {
                hasPay = true;
            } else {
                hasPay = false;
            }
        });
    }


    bindRightClick();
    bindLeftClick();

    function bindRightClick() {
        $('.jtsright').click(function () {
            currentTeacherIndex += 1;
            if (currentTeacherIndex < teachers.length) {
                if (currentTeacherIndex == teachers.length - 1) {
                    $('#turn_right').removeClass('jtsright').addClass('jtsright-no');
                    $('#turn_left').removeClass('jtsleft-no').addClass('jtsleft');
                } else {
                    $('#turn_left').removeClass('jtsleft-no').addClass('jsleft');
                }
                var teacher = teachers[currentTeacherIndex];
                $('#teacherphoto')[0].src = teacher.TeacherPhoto == null || teacher.TeacherPhoto == "" ? "/Libs/CourseInfo/imgs/big.jpg" : teacher.TeacherPhoto;
                $('#teachername').text(teacher.TeachersName == null || teacher.TeachersName == undefined ? "" : teacher.TeachersName);
                $('#synopsis').html(teacher.Synopsis == null || teacher.Synopsis == undefined ? "" : teacher.Synopsis);
                $('#teacherinfo')[0].dataset.id = teacher.Id;
            } else {
                currentTeacherIndex = teachers.length - 1;
                $('#turn_right').removeClass('jtsright').addClass('jtsright-no');
                $('#turn_left').removeClass('jtsleft-no').addClass('jtsleft');
            }
            bindLeftClick();
        });
    }


    function bindLeftClick() {
        $('.jtsleft').click(function () {
            currentTeacherIndex -= 1;
            if (currentTeacherIndex >= 0) {
                if (currentTeacherIndex == 1) {
                    $('#turn_right').removeClass('jtsright-no').addClass('jtsright');
                    $('#turn_left').removeClass('jtsleft').addClass('jtsleft-no');
                }
                var teacher = teachers[currentTeacherIndex];
                $('#teacherphoto')[0].src = teacher.TeacherPhoto == null || teacher.TeacherPhoto == "" ? "/Libs/CourseInfo/imgs/big.jpg" : teacher.TeacherPhoto;
                $('#teachername').text(teacher.TeachersName == null || teacher.TeachersName == undefined ? "" : teacher.TeachersName);
                $('#synopsis').html(teacher.Synopsis == null || teacher.Synopsis == undefined ? "" : teacher.Synopsis);
                $('#teacherinfo')[0].dataset.id = teacher.Id;
            } else {
                currentTeacherIndex = 0;
                $('#turn_right').removeClass('jtsright-no').addClass('jtsright');
                $('#turn_left').removeClass('jtsleft').addClass('jtsleft-no');
            }
            bindRightClick();
        })
    }


    function LearnProgress() {
        topevery.ajax({
            url: 'api/CourseLearnProgress/GetProgress',
            data: JSON.stringify({ CourseId: $('#courseid').val() })
        }, function (data) {
            $('#btn_start').text('开始学习');
            if (data.Result) {
                var model = data.Result;
                if (model.LearnedCount > 0) {
                    $('#btn_start').text('继续学习');
                }
                $('#learnCount').text(model.LearnedCount);
                $('.probar').attr("style", 'width:' + (model.LearnedCount / model.TotalLearnCount * 100) + "%");
                if (model.LastWatch) {
                    $('.pTaskLeft').html("上次学习到:<span style=\"padding-right: 20px;font-weight: bold;color: sienna;\">" + model.LastWatch + "</span>");
                }
            }
        })
    }

    //加载推荐课程
    topevery.ajax({
        url: "api/CourseInfo/GetRandomListExceptCurrent?courseId=" + $('#courseid').val(),
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $('#recommendcourselist').html();
            if (data.Result.length > 0) {
                $("#recommendcourselist").html(template("recommendcourselist_html", data));
            } else {
                $("#j-recommend").hide();
            }
        }
    });

    //加载相关打包课程
    topevery.ajax({
        url: "api/CourseInfo/GetPackcourseInfoList?courseId=" + $('#courseid').val(),
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $('#recommendcourselist1').html();
            if (data.Result.length > 0) {
                $("#recommendcourselist1").html(template("recommendcourselist1_html", data));
            } else {
                $("#PackcourseInfo").hide();
            }
        }
    });

    //$('#teacherinfo').click(function () {
    //    var teacherid = $('#teacherinfo').data('id');
    //    if ($.trim(teacherid) != "") {
    //        window.open('/TeacherInfo/Index?teacherid=' + teacherid, '_blank');
    //    }
    //});

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
                        parent.layer.msg("收藏成功!");
                    }
                })
            } else {
                topevery.ajax({ url: 'api/MyCollection/CancelCollection', data: JSON.stringify({ CourseId: courseId }) }, function (data) {
                    if (data.Result.Success) {
                        $('#btn_collect').removeClass('collected');
                        $('#btn_collect').attr('title', '加入收藏');
                        parent.layer.msg('取消收藏!');
                    }
                });
            }
        });
    })


    var num = finalnum = tempnum = 0;
    var lis = $(".j-solo > .u-rating > .star");

    //num:传入点亮星星的个数
    //finalnum:最终点亮星星的个数
    //tempnum:一个中间值
    function fnShow(num) {
        $(lis).removeClass('on');
        finalnum = num || tempnum; //如果传入的num为0，则finalnum取tempnum的值
        for (var i = 0; i < lis.length; i++) {
            i < finalnum ? $(lis[i]).addClass('on') : $(lis[i]).addClass(''); //点亮星星就是加class为light的样式
        }
    }

    for (var i = 1; i <= lis.length; i++) {
        lis[i - 1].index = i;
        lis[i - 1].onmouseover = function () { //鼠标经过点亮星星。
            $('#tip').text(formatterTip(this.index));
            fnShow(this.index); //传入的值为正，就是finalnum
        }
        lis[i - 1].onmouseout = function () { //鼠标离开时星星变暗
            fnShow(0); //传入值为0，finalnum为tempnum,初始为0
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
            default:
                return "暂无评价";
        }
    }

    $('#appraisetxt').focus(function () {
        $('#wordTip').attr('style', 'display:block;');
        $('.u-cmtsb').attr('style', 'display:block;')
    })

    $('#appraisecontent').on('keydown', limitinputlenght).on('keyup', limitinputlenght);

    ///限制输入长度
    function limitinputlenght() {
        var length = $('#appraisecontent').val().length;
        if (100 - length >= 0) {
            $('.s-fc1').text(100 - length);
        }
        if (length > 100) {
            $('#appraisecontent').val($('#appraisecontent').val().substring(0, 100));
        }
    }

    topevery.ajax({
        url: 'api/CourseChapter/IsPj',
    }, function (data) {
        //判断是否开启评价功能
        if (data.Result) {
            $('#courseappraise').parent().attr('style', 'display:Block;');
        } else {

        }
    });

    $('#appraisesubmit').click(function () {
        if (topevery.GetCookie("userToken")) {
            //该课程是否已购买
            topevery.ajax({
                url: 'api/Sheet/IfAreadyPay',
                data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 0 })
            }, function (data) {
                if (data.Result) {
                    var content = $('#appraisetxt').val();
                    if ($.trim(content) != "") {
                        topevery.ajax({
                            url: 'api/CourseAppraise/Add',
                            data: JSON.stringify({ AppraiseCotent: $('#appraisetxt').val(), AppraiseLevel: finalnum, CourseId: $('#courseid').val(), CourseType: 0, AppraiseIp: returnCitySN.cip })
                        }, function (data) {
                            //成功发表评价，更新评价列表，清空评价内容
                            if (data.Success) {
                                $('#appraisetxt').val('');
                                GetAppraiseList();
                                $("#auto-id-1523345154243 .star").removeClass("on");
                                parent.layer.msg(data.Result.Message);
                                $('.j-al-null').attr('style', 'display:none;');
                            } else {
                                parent.layer.msg(data.Result.Message);
                            }
                        });
                    } else {
                        parent.layer.msg("请输入评论内容!");
                    }
                } else {
                    $('#appraisetxt').val('');
                    parent.layer.msg("您尚未购买该课程，暂不能评价!");
                }
            });

        } else {
            userLogin();
            //layer.msg('请先登录再进行操作!', { time: 2000 });
        }
    });

    function GetAppraiseList1() {
        topevery.ajax({
            url: 'api/CourseChapter/GetList',
            data: JSON.stringify({ CourseId: $('#courseid').val(), Page: $("#PageIndex").val(), Rows: $("#Rows").val() })
        }, function (data) {
            $('#j-forumposts').html('');
            if (data.Success) {
                if (data.Result && data.Result.Rows.length > 0) {
                    //加载评价列表
                    var arr = data.Result.Rows;
                    var contentHtml = "";
                    for (var i = 0; i < arr.length; i++) {
                        var on1 = arr[i].AppraiseLevel >= 1 ? "on" : "";
                        var on2 = arr[i].AppraiseLevel >= 2 ? "on" : "";
                        var on3 = arr[i].AppraiseLevel >= 3 ? "on" : "";
                        var on4 = arr[i].AppraiseLevel >= 4 ? "on" : "";
                        var on5 = arr[i].AppraiseLevel >= 5 ? "on" : "";
                        var ReplyContent = "";
                        if (arr[i].ReplyContent) {
                            ReplyContent = "</div>"
                                + "<div class=\"f-cb cnt f-db f-thide\">"
                                + "<a class=\"s-fc5 post\">回复内容："
                                + arr[i].ReplyContent
                                + "</a>"
                                + "</div>";
                        }
                        contentHtml += "<li class=\"u-forumli j-forumli\">"
                            + "<div class=\"forumi\">"
                            + "<div class=\"f-cb\">"
                            + "<div class=\"head\">"
                            + "<a class=\"j-userLink\">"
                            + "<img class=\"img j-avatar\"  src=\"" + (arr[i].HeadImage ? arr[i].HeadImage : '/Libs/CourseInfo/imgs/small.jpg') + "\" alt=\"murphyfu\" width=\"30\" height=\"30\">"
                            + "<span class=\"j-userName\">"
                            + "<a class=\"u-forumname j-userLink\" href=\"/u/ykt1512888885723\" target=\"_blank\">" + arr[i].UserName + "</a>"
                            + "</span>"
                             + "<div class=\"info\">"
                            + "<div class=\"rate j-rate\">"
                            + "<div class=\"u-rating\">"
                            + "<div class=\"star " + on1 + "\"></div>"
                            + "<div class=\"star " + on2 + "\"></div>"
                            + "<div class=\"star " + on3 + "\"></div>"
                            + "<div class=\"star " + on4 + "\"></div>"
                            + "<div class=\"star " + on5 + "\"></div>"
                            + "</div>"
                            + "</div>"
                            + "</div>"
                            + "<span class=\"time s-fc5 j-postTime\" >" + dateFormatter(arr[i].AppraiseTime) + "</span>"
                           
                            + "</a>"
                            + "</div>"
                            + "</div>"
                            + "<div class=\"f-cb cnt f-db f-thide\">"
                            + "<a class=\"s-fc5 post\">"
                            + arr[i].AppraiseCotent
                            + "</a>"
                            + "</div>"
                             + ReplyContent
                            + "</div>"
                            + "</li>";
                    }
                    $('#j-forumposts').html(contentHtml);
                } else {
                    //提示没有评价
                    $('.j-al-null').attr('style', 'display:block;');
                }
            }
        });
    }

    function GetAppraiseList() {
        topevery.ajax({
            url: 'api/CourseChapter/GetList',
            data: JSON.stringify({ CourseId: $('#courseid').val(), Page: $("#PageIndex").val(), Rows: $("#Rows").val() })
        }, function (data) {
            $('#j-forumposts').html('');
            if (data.Success) {
                if (data.Result && data.Result.Rows.length > 0) {
                    //加载评价列表
                    var arr = data.Result.Rows;
                    var contentHtml = "";
                    for (var i = 0; i < arr.length; i++) {
                        var on1 = arr[i].AppraiseLevel >= 1 ? "on" : "";
                        var on2 = arr[i].AppraiseLevel >= 2 ? "on" : "";
                        var on3 = arr[i].AppraiseLevel >= 3 ? "on" : "";
                        var on4 = arr[i].AppraiseLevel >= 4 ? "on" : "";
                        var on5 = arr[i].AppraiseLevel >= 5 ? "on" : "";

                        var ReplyContent = "";
                        if (arr[i].ReplyContent) {
                            ReplyContent = "</div>"
                                + "<div class=\"f-cb cnt f-db f-thide\">"
                                + "<a class=\"s-fc5 post\">回复内容："
                                + arr[i].ReplyContent
                                + "</a>"
                                + "</div>";
                        }
                        contentHtml += "<li class=\"u-forumli j-forumli\">"
                            + "<div class=\"forumi\">"
                            + "<div class=\"f-cb\">"
                            + "<div class=\"head\">"
                            + "<a class=\"j-userLink\">"
                            + "<img class=\"img j-avatar\"  src=\"" + (arr[i].HeadImage ? arr[i].HeadImage : '/Libs/CourseInfo/imgs/small.jpg') + "\" alt=\"murphyfu\" width=\"30\" height=\"30\">"
                            + "<span class=\"j-userName\">"
                            + "<a class=\"u-forumname j-userLink\" href=\"\" >" + arr[i].UserName + "</a>"
                            + "</span>"
                            + "<div class=\"info\">"
                            + "<div class=\"rate j-rate\">"
                            + "<div class=\"u-rating\">"
                            + "<div class=\"star " + on1 + "\"></div>"
                            + "<div class=\"star " + on2 + "\"></div>"
                            + "<div class=\"star " + on3 + "\"></div>"
                            + "<div class=\"star " + on4 + "\"></div>"
                            + "<div class=\"star " + on5 + "\"></div>"
                            + "</div>"
                            + "</div>"
                            + "</div>"
                            + "<span class=\"time s-fc5 j-postTime\" >" + dateFormatter(arr[i].AppraiseTime) + "</span>"
                            
                            + "</a>"
                            + "</div>"
                            + "</div>"
                            + "<div class=\"f-cb cnt f-db f-thide\">"
                            + "<a class=\"s-fc5 post\">"
                            + arr[i].AppraiseCotent
                            + "</a>" +
                            ReplyContent
                            + "</div>"
                            + "</li>";
                    }
                    $('#j-forumposts').html(contentHtml);
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
                            GetAppraiseList1();
                        }
                    });
                } else {
                    //提示没有评价
                    $('.j-al-null').attr('style', 'display:block;');
                }
            }
        });
    }

    //取消评价
    //$('#cancelsubmit').click(function () {
    //    $('.s-fc6').attr('style', 'display:none;');
    //    $('.u-cmtsb').attr('style', 'display:none;')
    //});
    function dateFormatter(datetime) {
        datetime = new Date(datetime);
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1;
        var day = datetime.getDate();
        return year + "年" + month + '月' + day + "日";
    }



    $('#btnLearnNow').click(function () {
        if (topevery.GetCookie("userToken")) {
            topevery.ajax({
                url: 'api/Sheet/IfAreadyPay',
                data: JSON.stringify({
                    CourseId: $('#courseid').val(), CourseType: 0
                })
            }, function (data) {
                if (data.Result) {
                    CourseChapter();
                } else {
                    var courseId = $('#courseid').val();
                    //加入购物车,跳转到订单页面
                    topevery.ajax({
                        url: 'api/Cart/Add',
                        data: JSON.stringify({
                            CourseType: 0,
                            CourseId: courseId
                        })
                    }, function (data) {
                        if (data.Success) {
                            if (data.Result.Success) {
                                if (IsFree) {
                                    parent.layer.msg("参加成功!");
                                    setTimeout("location.reload()", 2000);
                                } else {

                                    window.open('/Cart/Index', '_blank');
                                }
                            } else {
                                parent.layer.msg(data.Result.Message);
                            }
                        }
                    });
                }
            });
        } else {
            userLogin();
        }
    });

    $("#AddToCart").click(function () {
        var courseId = $("#courseid").val();
        topevery.ajax({
            url: 'api/Cart/Add',
            data: JSON.stringify({
                CourseType: 0, CourseId: courseId
            })
        }, function (data) {
            if (data.Success) {
                if (data.Result.Success) {
                    parent.setNum();

                    parent.layer.msg(data.Result.Message);
                    //setTimeout("location.reload()", 2000);
                } else {
                    parent.layer.msg(data.Result.Message);
                }
            }
        });
    });

    $('#courseappraise').click(function () {
        var courseid = $(this).data('id');
        $('#appraisecontent').attr("style", "display:block;");
        $('#coursearea').attr("style", "display:none;");
        $('#CoursePaper').attr("style", "display:none;");
        $('#CourseChapter').attr("style", "display:none;");
        $('.tabarea >li> a').attr('style', '').removeClass('selected');
        $('#courseappraise').attr('style', 'color:#ffffff; background:#ff9900; border-radius:4px; padding-left:10px;padding-right:10px;');

        //加载数据
        GetAppraiseList();
        parent.setIframeHeight();
    });
    ////绑定项目->科目->课程
    //topevery.ajax({
    //    url: "api/CourseInfo/GetProjectSubject?courseId=" + $('#courseid').val(),
    //    data: JSON.stringify({})
    //}, function (data) {
    //    if (data.Success) {
    //        $("#maintain_info_box").html(template("maintain_info_box_html", data.Result));
    //    }
    //});

    if (topevery.GetCookie("userToken")) {
        //该课程是否已购买
        topevery.ajax({
            url: 'api/Sheet/IfAreadyPay',
            data: JSON.stringify({
                CourseId: $('#courseid').val(), CourseType: 0
            })
        }, function (data) {
            if (data.Result) {
                GetChapterList(true);
            } else {
                $("#AddToCart").show();
                GetChapterList(false);
            }
        });

        //添加到我的足迹
        topevery.ajax({
            url: 'api/MyCollection/AddFootprint',
            data: JSON.stringify({
                CourseId: $('#courseid').val(), CourseType: 0
            })
        }, function (data) {
        });
    } else {
        $("#AddToCart").show();
        GetChapterList(false);
        //layer.msg("请先登录再进行操作", {time:2000});
    }
    var learnStatehtml = "";
    var subObj = '';
    function GetChapterList(hasBuy) {
        var api = "api/CourseChapter/GetCourseChapterList";
        if (topevery.GetCookie("userToken")) {
            api = "api/CourseAppraise/GetCourseChapterList";
        }
        //加载课程章节
        topevery.ajax({
            url: api,
            data: JSON.stringify({
                CourseId: $('#courseid').val()
            })
        }, function (data) {
            if (data.Success) {
                $('#chapterlist').html("");
                var contentHtml = "";
                var parentChapters = data.Result;
                //判断是否存在章节
                if (parentChapters.length > 0) {
                    $("#CourseChapterId").parent().show();
                    //循环最外一层章
                    for (var i = 0; i < parentChapters.length; i++) {
                        var html = "";
                        //判断是否章节下面是否存在练习试题
                        if (parentChapters[i].Count > 0) {
                            html = "<a class=\"ChapterPractice\" ChapterId=\"" + parentChapters[i].Id + "\"  target='_blank'>在线练习</a>";
                        }
                        contentHtml += " <div class=\"chapter\" >"
                            + "<div class=\"chapterhead\">"
                            + "<span class=\"f-fl f-thide chaptertitle\">"
                            + ""
                            + "</span>"
                            + "<span class=\"f-fl f-thide chaptername\">"
                            + parentChapters[i].CapterName
                            + "</span>"
                            + html
                            + "</div>";
                        var contentHtml1 = "";
                        if (parentChapters[i].SubChapterList.length > 0) {
                            var sublist = parentChapters[i].SubChapterList;
                            //循环第二层章节  可能是子节点  也有可能是视频节点
                            for (var j = 0; j < sublist.length; j++) {
                                var html1 = "";
                                if (sublist[j].Count > 0) {
                                    html1 = "<a class=\"ChapterPractice\" ChapterId=\"" + sublist[j].VideoId + "\"  target='_blank'>在线练习</a>";
                                }
                                //若存在视频编号则展示视频内容
                                if (sublist[j].VideoId) {
                                    var freeShow1 = "";
                                    if (sublist[j].VideoLongTime === "0") {
                                        freeShow1 += "<a class=\"f-fr ksjbtn\" chapterId='" + sublist[j].Id + "' VideoId='" + sublist[j].VideoId + "'  courseId='" + $('#courseid').val() + "'  chapterName='" + sublist[j].CapterName + "'    target='_blank' >"
                                                                  + "<span class=\"f-fr\">暂无视频</span>"
                                                                  + "<div class=\"f-fr ksjicon-look\"></div>"
                                                              + "</a>";
                                    } else {
                                        if (hasBuy) {
                                            freeShow1 += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + sublist[j].Id + "'  VideoId='" + sublist[j].VideoId + "'  courseId='" + $('#courseid').val() + "'  chapterName='" + sublist[j].CapterName + "'    target='_blank' >"
                                                                  + "<span class=\"f-fr\">在线观看</span>"
                                                                  + "<div class=\"f-fr ksjicon-look\"></div>"
                                                              + "</a>";
                                        } else {
                                            if (!IsFree) {
                                                //判断是否为试听章节
                                                if (sublist[j].IsTaste == 1) {
                                                    freeShow1 += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + sublist[j].Id + "'  courseId='" + $('#courseid').val() + "'   VideoId='" + sublist[j].VideoId + "'chapterName='" + sublist[j].CapterName + "'    target='_blank' >"
                                                             + "<span class=\"f-fr\">课时预览</span>"
                                                             + "<div class=\"f-fr ksjicon-look\"></div>"
                                                         + "</a>";
                                                } else if (sublist[j].IsTaste == 0) {
                                                    freeShow1 += "<a class=\"f-fr ksjbtn j-hovershow\"    courseId='" + $('#courseid').val() + "'   chapterName=\"" + sublist[j].CapterName + "\" chapterid=\"" + sublist[j].Id + "\"  VideoId='" + sublist[j].VideoId + "'  target='_blank'>"
                                                              + "<span class=\"f-fr\">购买观看</span>"
                                                              + "<div class=\"f-fr ksjicon-look\"></div>"
                                                          + "</a>";
                                                }
                                            } else {
                                                freeShow1 += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + sublist[j].Id + "'  VideoId='" + sublist[j].VideoId + "'  courseId='" + $('#courseid').val() + "'  chapterName='" + sublist[j].CapterName + "'    target='_blank' >"
                                                                 + "<span class=\"f-fr\">课时预览</span>"
                                                                 + "<div class=\"f-fr ksjicon-look\"></div>"
                                                             + "</a>";
                                            }
                                        }
                                    }
                                    //视频学习状态
                                    getVideoState(sublist[j].State);
                                    contentHtml1 += "<div class=\"section\">"
                                                        + "<span class=\"f-fl f-thide ks\">"
                                                            + "&nbsp "
                                                        + "</span>"
                                                        + learnStatehtml
                                                        + "<span class=\"f-fl f-thide ksname\" title=" + sublist[j].CapterName + ">"
                                                            + "<a href=\"javascript:void(0)\"> " + sublist[j].CapterName + "</a>"
                                                        + "</span>"
                                                        + html1
                                                        + freeShow1
                                                        + "<span class=\"f-fr ksinfo j-hoverhide\">"
                                                            + "<span class=\"f-fr f-thide kstime\">"
                                                                + (sublist[j].VideoLongTime === "0" ? "暂无视频" : sublist[j].VideoLongTime)
                                                            + "</span>"
                                                            + "<span  class=\"f-fr ksinfoicon ksinfoicon-2\" title=\"视频\">"
                                                            + "</span>"
                                                        + "</span>"
                                                    + "</div>";
                                } else {
                                    contentHtml1 += " <div class=\"chapter\" >"
                                        + "<div class=\"chapterhead\">"
                                        + "<span class=\"f-fl f-thide chaptertitle\">"
                                        + "</span>"
                                        + "<span class=\"f-fl f-thide chaptername\">"
                                        + sublist[j].CapterName
                                        + "</span>"
                                        + html1
                                        + "</div>";
                                }
                                if (parentChapters[i].SubChapterList[j].SubChapterList) {
                                    //循环第三次视频节点
                                    for (var k = 0; k < parentChapters[i].SubChapterList[j].SubChapterList.length; k++) {
                                        var freeShow = "";
                                        var videoList = parentChapters[i].SubChapterList[j].SubChapterList;
                                        var html11 = "";
                                        if (videoList[k].Count > 0) {
                                            html11 = "<a class=\"ChapterPractice\" ChapterId=\"" + videoList[k].VideoId + "\"  target='_blank'>在线练习</a>";
                                        }
                                        //判断视频时间
                                        if (videoList[k].VideoLongTime === "0") {
                                            freeShow += "<a class=\"f-fr ksjbtn\" chapterId='" + videoList[k].Id + "' VideoId='" + videoList[k].VideoId + "'  courseId='" + $('#courseid').val() + "'   target='_blank' >"
                                                                      + "<span class=\"f-fr\">暂无视频</span>"
                                                                      + "<div class=\"f-fr ksjicon-look\"></div>"
                                                                  + "</a>";
                                        } else {
                                            //判断课程是否购买
                                            if (hasBuy) {
                                                freeShow += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + videoList[k].Id + "'  VideoId='" + videoList[k].VideoId + "'  courseId='" + $('#courseid').val() + "'    target='_blank' >"
                                                                      + "<span class=\"f-fr\">在线观看</span>"
                                                                      + "<div class=\"f-fr ksjicon-look\"></div>"
                                                                  + "</a>";
                                            } else {
                                                //判断视频是否免费
                                                if (!IsFree) {
                                                    //判断是否为试听章节
                                                    if (videoList[k].IsTaste == 1) {
                                                        freeShow += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + videoList[k].Id + "'  courseId='" + $('#courseid').val() + "'   VideoId='" + videoList[k].VideoId + "'  target='_blank' >"
                                                                 + "<span class=\"f-fr\">课时预览</span>"
                                                                 + "<div class=\"f-fr ksjicon-look\"></div>"
                                                             + "</a>";
                                                    } else if (videoList[k].IsTaste == 0) {
                                                        freeShow += "<a class=\"f-fr ksjbtn j-hovershow\"    courseId='" + $('#courseid').val() + "'    chapterid=\"" + videoList[k].Id + "\"  VideoId='" + videoList[k].VideoId + "' target='_blank'>"
                                                                  + "<span class=\"f-fr\">购买观看</span>"
                                                                  + "<div class=\"f-fr ksjicon-look\"></div>"
                                                              + "</a>";
                                                    }
                                                } else {
                                                    freeShow += "<a class=\"f-fr ksjbtn j-hovershow\" chapterId='" + videoList[k].Id + "'   courseId='" + $('#courseid').val() + "'  VideoId='" + videoList[k].VideoId + "'    target='_blank' >"
                                                                     + "<span class=\"f-fr\">课时预览</span>"
                                                                     + "<div class=\"f-fr ksjicon-look\"></div>"
                                                                 + "</a>";
                                                }
                                            }
                                        }
                                        //视频学习状态
                                        getVideoState(videoList[k].State);
                                        contentHtml1 += "<div class=\"section\">"
                                                            + "<span class=\"f-fl f-thide ks\">"
                                                                + "&nbsp "
                                                            + "</span>"
                                                            + learnStatehtml
                                                            + "<span class=\"f-fl f-thide ksname\" title=" + videoList[k].CapterName + ">"
                                                                + "<a href=\"javascript:void(0)\"> " + videoList[k].CapterName + "</a>"
                                                            + "</span>" +
                                                            html11
                                                            + freeShow
                                                            + "<span class=\"f-fr ksinfo j-hoverhide\">"
                                                                + "<span class=\"f-fr f-thide kstime\">"
                                                                    + (videoList[k].VideoLongTime === "0" ? "暂无视频" : videoList[k].VideoLongTime)
                                                                + "</span>"
                                                                + "<span  class=\"f-fr ksinfoicon ksinfoicon-2\" title=\"视频\">"
                                                                + "</span>"
                                                            + "</span>"
                                                        + "</div>";
                                    }
                                }
                            }
                        }
                        contentHtml += contentHtml1 + "</div>";
                    }
                    $("#chapterlist").html(contentHtml);
                    //绑定试题练习点击事件
                    $(".ChapterPractice").click(function () {
                        var chapterId = $(this).attr("ChapterId");
                        topevery.ajax({
                            url: "api/Economy/InsertChapterQuestions?ChapterId=" + chapterId + "&courseId=" + $('#courseid').val(),
                            type: "Post",
                            data: JSON.stringify({})
                        }, function (data) {
                            if (data.Success) {
                                if (data.Result) {
                                    window.open("/Answer/ChapterPractice?ChapterId=" + chapterId + "&chapterQuestionsId=" + data.Result.Id + "&PracticeNo=" + data.Result.PracticeNo, '_blank');
                                } else {
                                    parent.layer.msg("暂无权限在线练习,请前往购买！", {
                                        time: 4000
                                    });
                                }
                            }
                        });
                    });
                    //绑定视频点击事件
                    $(".j-hovershow").click(function () {
                        if (topevery.GetCookie("userToken")) {
                            var chapterId = $(this).attr("chapterId");
                            var videoId = $(this).attr("VideoId");
                            var courseId = $(this).attr("courseId");
                            window.open("/CourseInfo/ChapterVideoPlay?chapterId=" + chapterId + "&courseId=" + courseId + "&videoId=" + videoId);
                        } else {
                            userLogin();
                        }
                    });
                }
            }
        });
    }


    function getVideoState(state) {
        learnStatehtml = "";
        switch (state) {
            case "":
                learnStatehtml = "<span class=\"f-fl ksicon ksicon-0\" title=\"未开始\"></span>";
                break;
            case "0":
                learnStatehtml = "<span class=\"f-fl ksicon ksicon-20\" title=\"继续学习\"></span>";
                break;
            case "1":
                learnStatehtml = "<span class=\"f-fl ksicon ksicon-30\" title=\"已完成\"></span>";
                break;
            default:
                learnStatehtml = "<span class=\"f-fl ksicon ksicon-0\" title=\"未开始\"></span>"
        }
    }

    function videoLearnProgress(courseid, videoid) {
        //登录状态显示章节学习进度
        if (topevery.GetCookie("userToken")) {
            topevery.ajax({
                url: 'api/CourseLearnProgress/GetVideoLearnState',
                data: JSON.stringify({
                    CourseId: courseid, VideoId: videoid
                }),
                async: false
            }, function (data) {
                if (data.Success) {
                    getVideoState(data.Result);
                }
            });
        }
    }


    //加载课程资源
    var resourceContent = "";
    topevery.ajax({
        url: 'api/CourseResource/GetList',
        data: JSON.stringify({
            CourseId: $('#courseid').val()
        })
    }, function (data) {
        if (data.Success) {
            var arr = data.Result;
            if (arr.length > 0) {
                $('#CourseResourceId').parent().attr('style', 'display:block;');
                resourceContent += "<ul>"
                for (var i = 0; i < arr.length; i++) {
                    var url = arr[i].ResourceUrl;
                    var suffix = url.split('.')[url.split('.').length - 1];

                    resourceContent += "<a href=\"" + arr[i].ResourceUrl + "\"><li class=\"res " + suffix + "\" data-name=\"" + arr[i].ResourceName + "\" data-link=\"" + arr[i].ResourceUrl + "\"><span >" + arr[i].ResourceName + "</sapn><span>(" + arr[i].ResourceSize + ")</span></li></a>"
                }
                resourceContent += "</ul>"
                $('#courseresource').html(resourceContent);
            }
        }
    });
});

//function getPaperTest() {
//试卷测评
topevery.ajax({
    url: "api/CourseInfo/GetPaperList?courseId=" + $('#courseid').val(),
    data: JSON.stringify({})
}, function (data) {
    if (data.Success) {
        if (data.Result.length > 0) {
            $('#CoursePaperId').parent().attr('style', 'display:Block;');
            $("#CoursePaper_text").html(template("CoursePaper_html", data));
            $(".ChapterPracticeId").click(function () {
                var paperId = $(this).attr("data");
                topevery.ajax({
                    url: "api/Economy/InsertIntoPaperRecords?paperId=" + paperId + "&courseId=" + $('#courseid').val(),
                    async:false,//open新开一个窗口浏览器会拦截，改为false
                    data: JSON.stringify({})
                }, function (data) {
                    if (data.Result) {
                        topevery.myopen("/Answer/Index?paperId=" + paperId + "&paperRecordsId=" + data.Result);
                    } else {
                        parent.layer.msg("暂无权限练习试卷,请前往购买!", {
                            time: 4000
                        });
                    }
                });
            });
            $(".ChapterPracticeView").click(function () {
                var paperId = $(this).attr("data");
                topevery.ajaxToThis({
                    type: "get", url: "/CourseInfo/ChapterPracticeList?paperId=" + paperId, dataType: "html"
                }, function (data) {
                    parent.layer.open({
                        type: 1,
                        title: "试卷练习记录",
                        skin: 'layui-layer-rim', //加上边框
                        area: [800 + 'px', 400 + 'px'], //宽高
                        content: data
                    });
                }, true);
            });
        }
    }
});

function setIframeHeight(iframe) {
    if (iframe) {
        var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
        if (iframeWin.document.body) {
            setTimeout(function () { iframe.height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight; }, 400);
        }
    }
};
function CoursePaper() {
    parent.setIframeHeight();
    $('#coursearea').attr("style", "display:none;");
    $('#appraisecontent').attr("style", "display:none;");
    $('#CourseChapter').attr("style", "display:none;");
    $('#CourseResource').attr("style", "display:none;");
    $('#CoursePaper').attr("style", "display:block;");
    $('.tabarea >li> a').attr('style', '').removeClass('selected');
    $('#CoursePaperId').attr('style', 'color:#ffffff; background:#ff9900; border-radius:4px; padding-left:10px;padding-right:10px;');
}

function CourseChapter() {
    parent.setIframeHeight();
    $('#coursearea').attr("style", "display:none;");
    $('#appraisecontent').attr("style", "display:none;");
    $('#CoursePaper').attr("style", "display:none;");
    $('#CourseResource').attr("style", "display:none;");
    $('#CourseChapter').attr("style", "display:block;");
    $('.tabarea >li> a').attr('style', '').removeClass('selected');
    $('#CourseChapterId').attr('style', 'color:#ffffff; background:#ff9900; border-radius:4px; padding-left:10px;padding-right:10px;');
}

function CourseResource() {
    parent.setIframeHeight();
    $('#coursearea').attr("style", "display:none;");
    $('#appraisecontent').attr("style", "display:none;");
    $('#CoursePaper').attr("style", "display:none;");
    $('#CourseChapter').attr("style", "display:none;");
    $('#CourseResource').attr("style", "display:block;");
    $('.tabarea >li> a').attr('style', '').removeClass('selected');
    $('#CourseResourceId').attr('style', 'color:#ffffff; background:#ff9900; border-radius:4px; padding-left:10px;padding-right:10px;');
}


function CourseContent() {
    parent.setIframeHeight();
    $('#coursearea').attr("style", "display:none;");
    $('#appraisecontent').attr("style", "display:none;");
    $('#CoursePaper').attr("style", "display:none;");
    $('#CourseChapter').attr("style", "display:none;");
    $('#coursearea').attr("style", "display:block;");
    $('.tabarea >li> a').attr('style', '').removeClass('selected');
    $('#coursecontenttab').attr('style', 'color:#ffffff; background:#ff9900; border-radius:4px; padding-left:10px;padding-right:10px;');
}

function loadLearnPerson() {
    topevery.ajax({
        url: 'api/CourseInfo/GetLearnList',
        data: JSON.stringify({
            Page: 1, Rows: 12, CourseId: $('#courseid').val()
        })
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




