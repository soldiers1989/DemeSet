var IsFree = false;
$(function () {
    $($(".navIn li")[0]).removeClass("cur");
    $($(".navIn li")[1]).addClass("cur");
    Intialize();
    function Intialize() {
        topevery.ajax({
            url: 'api/CoursePack/GetCoursePackDetail',
            data: JSON.stringify({ Id: $('#courseid').val() })
        }, function (data) {
            if (data) {
                if (data.Success) {
                    var courseinfo = data.Result;
                    debugger;
                    courseinfo.ValidityEndDate = topevery.dataTimeView(courseinfo.ValidityEndDate);
                    $(".tc_tit_r").html(template("tc_tit_r_html", courseinfo));
                    $("#CourseIamge").attr("src", courseinfo.CourseIamge);
                    $(".txt").html(courseinfo.CourseContent);
                    $("#courseList").html(template("courseList_html", courseinfo));

                    $("#courseList a").click(function () {
                        $("#courseList li").removeClass("cur");
                        $(this).find("li").addClass("cur");
                        $("#mainIframe").attr("src", "CourseInfoPack?courseId=" + $(this).attr("courseId") + "&courseType=0");
                    });
                    $("#courseList a")[0].click();
                    $("#AddToCart").hide();
                    $("#btnLearnNow").bind("click", function () {
                        var that = this;
                        var courseId = $('#courseid').val();
                        var courseType = 1;
                        if (topevery.GetCookie("userToken")) {
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
                                            topevery.openClick("/Cart/Index");
                                        }
                                    } else {
                                        layer.msg(data.Result.Message);
                                    }
                                }
                            });
                        } else {
                            //没登录的情况下写入cookie
                            var cookieCart = topevery.GetCookie("cartInfoList");
                            var cartInfoList = courseId + "@" + courseType;
                            if (cookieCart) {
                                var cartSplit = cookieCart.split(",");
                                if ($.inArray(cartInfoList, cartSplit) > -1) {
                                    parent.layer.msg("该课程已加入购物车!");
                                    return;
                                } else {
                                    topevery.DelCookie("cartInfoList"); // 删除 cookie
                                    cartInfoList = cookieCart + "," + cartInfoList;
                                }
                            }
                            topevery.shoping({ that: that });
                            topevery.SetCookie("cartInfoList", cartInfoList);
                            parent.setNum();
                        }
                    });
                    $("#AddToCart").click(function () {
                        var that = this;
                        var courseId = $("#courseid").val();
                        var courseType = 1;
                        if (topevery.GetCookie("userToken")) {
                            topevery.ajax({
                                url: 'api/Cart/Add',
                                data: JSON.stringify({ CourseType: 1, CourseId: courseId })
                            }, function (data) {
                                if (data.Success) {
                                    if (data.Result.Success) {
                                        topevery.shoping({ that: that });
                                        layer.msg(data.Result.Message);
                                        $(".CartCount").html(parseInt($(".CartCount").html()) + 1);
                                    } else {
                                        layer.msg(data.Result.Message);
                                    }
                                }
                            });
                        } else {
                            //没登录的情况下写入cookie
                            var cookieCart = topevery.GetCookie("cartInfoList");
                            var cartInfoList = courseId + "@" + courseType;
                            if (cookieCart) {
                                var cartSplit = cookieCart.split(",");
                                if ($.inArray(cartInfoList, cartSplit) > -1) {
                                    parent.layer.msg("该课程已加入购物车!");
                                    return;
                                } else {
                                    topevery.DelCookie("cartInfoList"); // 删除 cookie
                                    cartInfoList = cookieCart + "," + cartInfoList;
                                }
                            }
                            topevery.shoping({ that: that });
                            topevery.SetCookie("cartInfoList", cartInfoList);
                            parent.setNum();
                            parent.layer.msg("加入成功!");
                        }
                    });
                    topevery.ajax({
                        url: 'api/CourseInfo/GetRecommendCourseByTagExceptSelf',
                        data: JSON.stringify({ CourseId: $('#courseid').val(), Page: 1, Rows: 4 })
                    }, function (data) {
                        if (data.Success) {
                            $("#CourseByTagExceptSelf").html(template("CourseByTagExceptSelf_html", data.Result));
                        }
                    });
                    if (topevery.GetCookie("userToken")) {
                        //该课程是否已购买
                        topevery.ajax({
                            url: 'api/Sheet/IfAreadyPay',
                            data: JSON.stringify({ CourseId: $('#courseid').val(), CourseType: 1 })
                        }, function (data) {
                            if (data.Result) {
                                $("#btnLearnNow").unbind();
                                $("#btnLearnNow").html("<span>已购买</span>");
                                $('#btnLearnNow').show();
                            } else {
                                console.log(courseinfo.State);
                                if (parseInt(courseinfo.State) == 0) {
                                    $('#Unshelve').show();
                                } else {
                                    $("#btnLearnNow").show();
                                    $("#AddToCart").show();
                                }
                            }
                        });
                        //添加到我的足迹
                        topevery.ajax({
                            url: 'api/MyCollection/AddFootprint',
                            data: JSON.stringify({
                                CourseId: $('#courseid').val(),
                                CourseType: 1
                            })
                        }, function (data) {
                        });
                    } else {
                        console.log(courseinfo.State);
                        if (parseInt(courseinfo.State) == 0) {
                            $('#Unshelve').show();
                        } else {
                            $("#btnLearnNow").show();
                            $("#AddToCart").show();
                        }
                        //layer.msg("请先登录再进行操作", {time:2000});
                    }
                }
            }
        });

        //加载推荐课程
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


    $(document).ready(function () {
        var $wrapper = $('.tab-wrapper'),
            $allTabs = $wrapper.find('.tab-content > div'),
            $tabMenu = $wrapper.find('.tab-menu li'),
            $line001 = $('<div class="line001"></div>').appendTo($tabMenu);
        $allTabs.not(':first-of-type').hide();
        $tabMenu.filter(':first-of-type').find(':first').width('100%')
        $tabMenu.each(function (i) {
            $(this).attr('data-tab', 'tab' + i);
        });
        $allTabs.each(function (i) {
            $(this).attr('data-tab', 'tab' + i);
        });
        $tabMenu.on('click', function () {
            var dataTab = $(this).data('tab'),
                $getWrapper = $(this).closest($wrapper);
            $getWrapper.find($tabMenu).removeClass('active');
            $(this).addClass('active');
            $getWrapper.find('.line001').width(0);
            $(this).find($line001).animate({ 'width': '100%' }, 'fast');
            $getWrapper.find($allTabs).hide();
            $getWrapper.find($allTabs).filter('[data-tab=' + dataTab + ']').show();
        });

    });
});



function setIframeHeight(iframe) {
    if (!iframe) {
        iframe = document.getElementById("mainIframe");
    }
    if (iframe) {
        var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
        if (iframeWin.document.body) {
            setTimeout(function () { iframe.height = $(window.frames["mainIframe"].document.getElementsByClassName("g-flow")[0]).height() }, 200);
            console.log(iframe.height);
        }
    }
};