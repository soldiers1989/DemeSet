var IsFree = false;
var Rows = 5;
$(function () {
    Intialize();

    function Intialize() {
        topevery.ajax({
            url: 'api/CoursePack/GetFaceToFace',
            data: JSON.stringify({ Id: $('#courseid').val() })
        }, function (data) {
            if (data) {
                if (data.Success) {
                    var courseinfo = data.Result;
                    $('#ClassName').text(courseinfo.ClassName);
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
                    
                    $('#courseimg')[0].src = courseinfo.CourseIamge;
                    $('#teachername').text(courseinfo.TeachersName);
                    $('#CourseIntroduce').html(courseinfo.CourseIntroduce);
                    $('#Curriculum').html(courseinfo.Curriculum);
                    $('#WhatTeach').html(courseinfo.WhatTeach);
                    $('#TeachingObject').html(courseinfo.TeachingObject);
                    $('#TeachingGoal').html(courseinfo.TeachingGoal);
                    $('#Characteristic').html(courseinfo.Characteristic);


                    
                    $('#Address').html(courseinfo.Address);
                    $('#Number').html(courseinfo.Number);
                    $('#ClassTime').html(topevery.dataTimeView(courseinfo.ClassTimeStart)+"至"+ topevery.dataTimeView(courseinfo.ClassTimeEnd));

                   
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
                data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 2 })
            }, function (data) {
                if (data.Result) {
                    $('#btnLearnNow').unbind();
                    $('#btnLearnNow>span').text('已购买');
                } else {
                    $("#AddToCart").show();
                }
            });
            //添加到我的足迹
            topevery.ajax({
                url: 'api/MyCollection/AddFootprint',
                data: JSON.stringify({
                    CourseId: $('#courseid').val(), CourseType: 2
                })
            }, function (data) {
            });
        }
    }


    //加载推荐课程
    topevery.ajax({
        url: "api/CourseInfo/GetRandomListFaceToFace?courseId=" + $('#courseid').val(),
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
    //加载主讲教师

    topevery.ajax({
        url: "api/CourseInfo/GetOnTeachers?courseId=" + $('#courseid').val(),
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $('#j-course-lectors').html();
            if (data.Result.length > 0) {
                $("#j-course-lectors").html(template("j-course-lectors_html", data));
            } else {
            }
        }
    });

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
                data: JSON.stringify({ CourseType: 2, CourseId: courseId })
            }, function (data) {
                if (data.Success) {
                    if (data.Result.Success) {
                        if (IsFree) {
                            layer.msg("参加成功!");
                            setTimeout("location.reload()", 2000);
                        } else {
                            topevery.openClick("/Cart/Index");
                        }
                    } else {
                        layer.msg(data.Result.Message);
                    }
                }
            });
        } else {
            topevery.wxPcLogin();
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
                    data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 2 })
                }, function (data) {
                    if (data.Result) {
                        topevery.ajax({
                            url: 'api/CourseAppraise/Add',
                            data: JSON.stringify({ AppraiseCotent: $('#appraisetxt').val(), AppraiseLevel: finalnum, CourseId: $('#courseid').val(), CourseType: 2 })
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
            topevery.wxPcLogin();
            //layer.msg('请先登录再进行操作!', { time: 2000 });
        }
    });

    if (topevery.GetCookie("userToken")) {
        //该课程是否已购买
        topevery.ajax({
            url: 'api/Sheet/IfAreadyPay',
            data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 2 })
        }, function (data) {
            if (data.Result) {
            } else {
                $("#AddToCart").show();
            }
        });
    } else {
        $("#AddToCart").show();
        //layer.msg("请先登录再进行操作", {time:2000});
    }

    //评价加载更多
    $("#auto-id-1449466945302").click(function () {
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

    $("#AddToCart").click(function () {
        var courseId = $("#courseid").val();
        topevery.ajax({
            url: 'api/Cart/Add',
            data: JSON.stringify({ CourseType: 2, CourseId: courseId })
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


