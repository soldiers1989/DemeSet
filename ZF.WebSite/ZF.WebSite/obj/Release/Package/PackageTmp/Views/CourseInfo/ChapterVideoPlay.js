var courseid = $('#CourseId').val();
var chapterid = $('#chapterid').val();
var videoid = $('#VideoId').val();
var FavourablePrice = $('#FavourablePrice').val();
$(function () {
    //提示购买观看,显示课程价格信息
    topevery.ajax({
        url: 'api/CourseChapter/GetCourseChapter',
        data: JSON.stringify({ Id: $('#VideoId').val() })
    }, function (data) {
        if (data.Result) {
            var obj = data.Result;
            $('#FavourablePrice').val(obj.FavourablePrice);
            $('#VideoName').html(obj.VideoName);
        }
    });
    Initial();
    //var videoPlayer = videojs('example_video_1', {}, function () { });
    function Initial() {
        //加载章节信息
        var chapterid = $('#chapterid').val();
        //判断是否已购买
        topevery.ajax({
            url: 'api/Sheet/IfAreadyPay',
            data: JSON.stringify({ CourseId: $('#CourseId').val() })
        }, function (data) {
            if (data.Result) {
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
            } else {
                topevery.ajax({
                    url: 'api/CourseVideo/GetOne',
                    data: JSON.stringify({ Id: $('#VideoId').val() })
                }, function (data) {
                    var model = data.Result;
                    //判断是否为试听章节
                    if (data.Success && model.IsTaste == 1) {
                        //手动设置视频播放器的视频源
                        videoPlayer(model.VideoUrl, model.TasteLongTime * 60);
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
                                $("#message").html("该课时在购买并参加该课程后才能观看");
                            }
                        });
                    }
                })
            }
        });

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
                    $(".section").bind("click", function () {
                        var chapterId = $(this).attr("chapterId");
                        var courseId = $('#CourseId').val();
                        var videoId = $(this).attr("VideoId");
                        var chapterName = $(this).attr("ChapterName");
                        if (videoId && courseId && chapterId)
                            location.href = "/CourseInfo/ChapterVideoPlay?chapterId=" + chapterId + "&courseId=" + courseId + "&videoId=" + videoId;
                    });
                }
            }
        });
    }


    $('#joincourseBtn').click(function () {
        var courseid = $('#CourseId').val();
        //加入购物车,跳转到订单页面
        topevery.ajax({
            url: 'api/Cart/Add',
            data: JSON.stringify({ CourseType: 0, CourseId: courseid })
        }, function (data) {
            if (data.Success) {
                if (data.Result.Success) {
                    window.open('/Cart/Index', '_self');
                }
            }
        });
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
})

///添加观看视频记录
function addLearnRecord() {
    topevery.ajax({
        url: 'api/CourseLearnProgress/Add',
        data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
    })
}

//更新视频观看状态-1：已看完
function updateLearnState() {
    topevery.ajax({
        url: 'api/CourseLearnProgress/UdpateState',
        data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val(), State: 1 })
    })
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
                autoplay: false,
                format: "m3u8",
                vid: VideoUrl,
                playauth: row.PlayAuth,
            }, function (player) {
            });
            if (TasteLongTime > 0) {
                player.on('timeupdate', function (e) {
                    if (player.getCurrentTime() > TasteLongTime && $("#FavourablePrice").val() !== "0") {
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
                                $("#message").html("试听已经结束,请购买课程后在继续观看视频!");
                            }
                        });
                    }
                });
            }
            player.on('ended', function (e) {
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
            });
            player.on('play', function (e) {
                //是否已看过该视频
                topevery.ajax({
                    url: 'api/CourseLearnProgress/IfExist',
                    data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
                }, function (data) {
                    if (!data.Result) {
                        addLearnRecord();
                    }
                })
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