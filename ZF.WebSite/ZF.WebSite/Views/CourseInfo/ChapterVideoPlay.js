var courseid = $('#CourseId').val();
var chapterid = $('#chapterid').val();
var videoid = $('#VideoId').val();
var FavourablePrice = $('#FavourablePrice').val();
$(function () {
    $("." + courseid).addClass("cur");//提示购买观看,显示课程价格信息

    function scrollToLocation() {
        var mainContainer = $('#tb-chapterlist-box'),
        scrollToContainer = $('#' + $('#VideoId').val());//滚动到<div id="thisMainPanel">中类名为son-panel的最后一个div处
        //scrollToContainer = mainContainer.find('.son-panel:eq(5)');//滚动到<div id="thisMainPanel">中类名为son-panel的第六个处
        //非动画效果
        //mainContainer.scrollTop(
        //  scrollToContainer.offset().top - mainContainer.offset().top + mainContainer.scrollTop()
        //);
        //动画效果
        mainContainer.animate({
            scrollTop: scrollToContainer.offset().top - mainContainer.offset().top + mainContainer.scrollTop()
        }, 0);//2秒滑动到指定位置
    }



    topevery.ajax({
        url: 'api/CourseChapter/GetCourseChapter',
        data: JSON.stringify({ Id: $('#VideoId').val() })
    }, function (data) {
        if (data.Result) {
            var obj = data.Result;
            $('#FavourablePrice').val(obj.FavourablePrice);
            $('#VideoName').html(obj.VideoName);
            document.getElementsByTagName('title')[0].innerText = obj.VideoName;
        }
    });
    Initial();
    //var videoPlayer = videojs('example_video_1', {}, function () { });
    function Initial() {
        //加载章节信息
        var chapterid = $('#chapterid').val();

        if (topevery.GetCookie("userToken")) {
            //判断是否已购买
            topevery.ajax({
                url: 'api/Sheet/IfAreadyPay',
                data: JSON.stringify({ CourseId: $('#CourseId').val() })
            }, function (data) {
                if (data.Result) {
                    if ($('#VideoId').val()) {
                        //判断是否已购买，已购买则播放视频
                        topevery.ajax({
                            url: 'api/CourseVideo/GetOne',
                            data: JSON.stringify({ Id: $('#VideoId').val() })
                        }, function (data) {
                            if (data.Success) {
                                var model = data.Result;
                                //手动设置视频播放器的视频源
                                //videoPlayer.src(model.VideoUrl);
                                videoPlayer(model.VideoUrl, 0);
                            }
                        });
                    }
                } else {
                    if ($('#VideoId').val()) {
                        loadVideo();
                    }
                }
            });
        } else {
            if ($('#VideoId').val()) {
                loadVideo();
            }
        }

        topevery.ajax({
            url: "api/CourseInfo/GetCourseVideo",
            data: JSON.stringify({ Id: $('#CourseId').val() })
        }, function (data) {
            if (data) {
                if (data.Success) {
                    var courseinfo = data.Result;
                    $("#CourseName").html(courseinfo.CourseName);
                    $("#AppraiseNum").html("(" + courseinfo.AppraiseNum + "份评价)");
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
                    $('#courseappraiselevel').html(appraise);
                    $('#courseimg')[0].src = courseinfo.CourseIamge;
                    $("#chapterList").html(template("chapterList_html", data.Result));
                    scrollToLocation();
                    $("." + $('#VideoId').val()).addClass("current");
                    $(".section").bind("click", function () {
                        var chapterId = $(this).attr("chapterId");
                        var courseId = $('#CourseId').val();
                        var videoId = $(this).attr("VideoId");
                        var videoUrl = $(this).attr("VideoUrl");
                        var chapterName = $(this).attr("ChapterName");
                        if (videoId && courseId && chapterId && videoUrl)
                            location.href = "/CourseInfo/ChapterVideoPlay?chapterId=" + chapterId + "&courseId=" + courseId + "&videoId=" + videoId;
                    });
                }
            }
        });

        isVideoCollected();
    }

    function loadVideo() {
        topevery.ajax({
            url: 'api/CourseVideo/GetOne',
            data: JSON.stringify({ Id: $('#VideoId').val() })
        }, function (data) {
            var model = data.Result;
            //if(data.)

            //判断是否为试听章节
            if (data.Success && model.IsTaste == 1) {
                if (!topevery.GetCookie("userToken")) {
                    //播放一栏显示试听时间
                    $('.msg-note').removeClass('hidetips');
                    if (model.TasteLongTime2 > 0) {
                        //手动设置视频播放器的视频源
                        videoPlayer(model.VideoUrl, model.TasteLongTime2);
                        $('.msg-note').html("试看<span class=\"tastetime\">" + formatterTasteLongTime(model.TasteLongTime2) + "</span>！去<a onclick=\"JumpToLogin();\" style=\"color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;\">登录</a>");
                    } else {
                        $(".msg-note").html('无法观看该视频,请先<a onclick="JumpToLogin();" style="color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;">登录</a>');
                    }

                } else {
                    //手动设置视频播放器的视频源
                    videoPlayer(model.VideoUrl, model.TasteLongTime);
                    //播放一栏显示试听时间
                    $('.msg-note').removeClass('hidetips');
                    if (model.TasteLongTime > 0) {
                        $('.msg-note').html("当前课程未购买，试看<span class=\"tastetime\">" + formatterTasteLongTime(model.TasteLongTime) + "</span>！去<a onclick=\"JumpToCart();\" style=\"color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;\">购买</a>");
                    } else {
                        $(".msg-note").html('当前课程未购买，当前视频可全部试看,购买即可观看全部视频,去<a onclick="JumpToCart();" style="color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;">购买</a>');
                        //$(".msg-note").html('当前课程未购买，当前视频可全部试看,购买即可观看全部视频<a onclick="JumpToCart();" style="color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;">去购买</a>');
                    }
                }
            } else {
                //提示购买观看,显示课程价格信息
                topevery.ajax({
                    url: 'api/CourseInfo/GetOne',
                    data: JSON.stringify({ Id: $('#CourseId').val() })
                }, function (data) {
                    if (data.Result) {
                        var obj = data.Result;
                        $('.presentPrice').html('¥' + obj.FavourablePrice);
                        $('.originPrice').html('¥' + obj.Price);
                        $('.u-coursePlanPayMsg').attr('style', 'display:block;z-index:99;');
                        if (!topevery.GetCookie("userToken")) {
                            $("#message").html('无法观看该视频,请先<a onclick="JumpToLogin();" class="joinBtn j-joinBtn">登录</a>');
                        } else {
                            $("#message").html('无法观看该视频,去<a onclick="JumpToCart();" class="joinBtn j-joinBtn">购买</a>');
                        }
                    }
                });
            }

        })
    }



    $('#joincourseBtn').click(function () {
        JumpToCart();
    });



    $("#j-hideRightBtn").bind("click", function () {
        var data = $(this).attr("data");
        if (data === "1") {
            $("#j-coursebox").attr("style", "margin-right: 0px;");
            $("#course-toolbar-box").hide();
            $(this).addClass('u-hiderightbtn-hide');
            $(this).attr("title", "显示课程目录");
            $(this).attr("data", "2");
        } else if (data === "2") {
            $("#j-coursebox").attr("style", "margin-right: 300px;");
            $("#course-toolbar-box").show();
            $(this).removeClass('u-hiderightbtn-hide');
            $(this).attr("title", "隐藏课程目录");
            $(this).attr("data", "1");
        }
    });

    /* $(document).on("click", ".chapter", function () {
         alert("df")
         $(this).next(".chapterList").stop().slideToggle(300);
     })*/
})

function JumpToLogin() {
    topevery.wxPcLogin();
}

function JumpToCart() {
    var courseid = $('#CourseId').val();
    var courseType = 0;
    if (topevery.GetCookie("userToken")) {
        //加入购物车,跳转到订单页面
        topevery.ajax({
            url: 'api/Cart/Add',
            data: JSON.stringify({ CourseType: 0, CourseId: courseid })
        }, function (data) {
            console.log(data);
            if (data.Success) {
                if (data.Result.Success) {
                    topevery.openClick("/Cart/Index");
                } else {
                    parent.layer.msg(data.Result.Message);
                }
            } else {
                parent.layer.msg(data.Result.Message);
            }
        });
    } else {
        //没登录的情况下写入cookie
        var cookieCart = topevery.GetCookie("cartInfoList");
        var cartInfoList = courseid + "@" + courseType;
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
        topevery.SetCookie("cartInfoList", cartInfoList);
        parent.setNum();
        parent.layer.msg("加入成功!");
    }
}


///收藏课程视频
function collectCourseVideo() {
    if (topevery.GetCookie("userToken")) {
        var courseId = $('#CourseId').val();
        var VideoId = $('#VideoId').val();
        topevery.ajax({
            url: 'api/MyCollection/CollectCourseVideo',
            data: { CourseId: courseId, VideoId: VideoId }
        }, function (data) {
            if (data.Success) {
                layer.msg("收藏成功", 2);
            }
        })
    } else {
        layer.msg("暂未登录,请登录后再进行该操作!");
    }
}

///添加观看视频记录
function addLearnRecord() {
    if (topevery.GetCookie("userToken")) {
        topevery.ajax({
            url: 'api/CourseLearnProgress/Add',
            data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
        })
    }
}

//更新视频观看状态-1：已看完
function updateLearnState() {
    if (topevery.GetCookie("userToken")) {
        topevery.ajax({
            url: 'api/CourseLearnProgress/UdpateState',
            data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val(), State: 1 })
        })
    }
}

function videoPlayer(VideoUrl, TasteLongTime) {
    topevery.ajax({
        url: 'api/CourseVideo/GetVideo',
        data: JSON.stringify({ Id: VideoUrl })
    }, function (data) {
        if (data.Result) {
            var row = data.Result;
            var player = new Aliplayer({
                id: 'J_prismPlayer',
                width: '98%',
                height: '98%',
                cover: row.CoverURL,
                autoplay: true,
                format: "m3u8",
                vid: VideoUrl,
                playauth: row.PlayAuth,
            }, function (player) {
            });
            player.on('timeupdate', function (e) {
                if (TasteLongTime > 0) {
                    if (player.getCurrentTime() > TasteLongTime) {
                        player.pause();
                        topevery.ajax({
                            url: 'api/CourseInfo/GetOne',
                            data: JSON.stringify({ Id: $('#CourseId').val() })
                        }, function (data) {
                            if (data.Result) {
                                var obj = data.Result;
                                $('.presentPrice').html('¥' + obj.FavourablePrice);
                                $('.originPrice').html('¥' + obj.Price);
                                $('.u-coursePlanPayMsg').attr('style', 'display:block;z-index:99;');
                                $("#message").html("试听已经结束,请购买课程后在继续观看视频! 去<a onclick=\"JumpToCart();\" style=\"color: #fff;background: red;padding: 3px 10px;border-radius: 3px;display: inline-block;margin-left: 15px;\">购买</a>");
                            }
                        });
                    }
                }
            });
            player.on('ready', function (e) {
                player.play();
            });
            player.on('ended', function (e) {
                if (topevery.GetCookie("userToken")) {
                    topevery.ajax({
                        url: 'api/CourseLearnProgress/IfExist',
                        data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
                    }, function (data) {
                        if (!data.Result) {
                            addLearnRecord();
                        } else {
                            updateLearnState();
                        }
                    });
                }
                var rowIndex = parseInt($("." + $('#VideoId').val()).attr("RowsIndex"));
                $(".rowIndex_" + getIndex(rowIndex + 1)).click();
            });

            player.on('play', function (e) {
                if (topevery.GetCookie("userToken")) {
                    //是否已看过该视频
                    topevery.ajax({
                        url: 'api/CourseLearnProgress/IfExist',
                        data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
                    }, function (data) {
                        if (!data.Result) {
                            addLearnRecord();
                        }
                    })
                }
            });
            $(function () {
                $(document).keydown(function (e) {
                    if (e.keyCode == 32) {
                        if (player.getStatus() == "playing") {
                            player.pause();
                        } else {
                            player.play();

                        }
                    } else if (e.keyCode == 37) {
                        player.seek(player.getCurrentTime() - 5);
                    } else if (e.keyCode == 39) {
                        player.seek(player.getCurrentTime() + 5);
                    }
                });
            });
        }
    });
}


function getIndex(rowIndex) {
    var length = $(".rowIndex_" + rowIndex).length;
    if (length === 0) {
        getIndex(rowIndex + 1);
    } else {
        return rowIndex;
    }
}

//去掉登陆用户信息-2018-6-9
//var object = new Object();
//object.userToken = topevery.GetCookie("userToken");
//if (object.userToken) {
//    topevery.ajax({
//        type: "GET",
//        url: "api/Home/GetUserInfoByTicket"
//    }, function (data) {
//        if (data.Success) {
//            object.NickNamw = data.Result.NickNamw;
//            object.CartCount = data.Result.CartCount;
//            object.LearnCount = data.Result.LearnCount;
//            object.HeadImage = data.Result.HeadImage;
//            $(".personCenter").html(template("question_userinfo", object));
//        }
//    });
//} else {
//    $(".personCenter").html(template("question_userinfo", object));
//}
////跳转登录页
//function userLogin() {
//    layer.open({
//        type: 2,
//        title: '用户登录',
//        shadeClose: true,
//        maxmin: false, //开启最大化最小化按钮
//        area: ['650px', '430px'],
//        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
//        content: '/Login/UserLogin?RefUrl=' + location.href,
//        end: function () {
//            //location.reload();
//        }
//    });
//}


////退出
//function outUser() {
//    topevery.DelCookie("userToken"); // 删除 cookie
//    location.href = "/Home/Index";
//}

function collectVideo(event) {
    if (topevery.GetCookie("userToken")) {
        var value = $(event).attr("data");
        //收藏
        if (value == "0") {
            topevery.ajax({
                url: 'api/MyCollection/CollectCourseVideo',
                data: JSON.stringify({ CourseId: $('#CourseId').val(), VideoId: $('#VideoId').val() })
            }, function (data) {
                if (data.Result.Success) {
                    $(event).attr("data", 1);
                    $(event).attr("title", "取消收藏");
                    $(event).addClass("cur");
                    layer.msg('收藏成功');
                }
            })
        } else if (value == "1") { //取消收藏
            topevery.ajax({
                url: 'api/MyCollection/CancelCollectVideo',
                data: JSON.stringify({ CourseId: $('#CourseId').val(), VideoId: $('#VideoId').val() })
            }, function (data) {
                if (data.Result.Success) {
                    $(event).attr("data", 0);
                    $(event).attr("title", "收藏该视频");
                    $(event).removeClass("cur");
                    layer.msg('取消收藏');
                }
            });
        }
    } else {
        layer.msg("暂未登录,请登录后再进行该操作!");
    }
}

function isVideoCollected() {
    if (topevery.GetCookie("userToken")) {
        //判断是否收藏
        if ($('#VideoId').val()) {
            topevery.ajax({
                url: 'api/MyCollection/IsVideoCollected',
                data: JSON.stringify({ CourseId: $('#CourseId').val(), VideoId: $('#VideoId').val() })
            }, function (data) {
                if (data.Result.Success) {
                    $('.collection').addClass('cur');
                } else {
                    $('.collection').removeClass('cur');
                }
            });
        }
    }
}

//template.helper('setCur', function (data) {
//    if (data == $('#VideoId').val()) {
//        return "cur";
//    }
//})
