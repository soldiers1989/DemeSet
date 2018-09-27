var videoid = $('#VideoId').val();
$(function () {
    Initial();
    //var videoPlayer = videojs('example_video_1', {}, function () { });
    function Initial() {
        //加载章节信息
                topevery.ajax({
                    url: 'api/CourseVideo/GetOne',
                    data: JSON.stringify({ Id: $('#VideoId').val() })
                }, function (data) {
                    if (data.Success) {
                        var model = data.Result;
                        //手动设置视频播放器的视频源
                        videoPlayer(model.VideoUrl, 0);
                    }
                });
    }
})
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
            //if (TasteLongTime > 0) {
            //    player.on('timeupdate', function (e) {
            //        if (player.getCurrentTime() > TasteLongTime && $("#FavourablePrice").val() !== "0") {
            //            player.pause();
            //            topevery.ajax({
            //                url: 'api/CourseInfo/GetOne',
            //                data: JSON.stringify({ Id: $('#CourseId').val() })
            //            }, function (data) {
            //                if (data.Result) {
            //                    var obj = data.Result;
            //                    $('.presentPrice').html('¥' + obj.FavourablePrice);
            //                    $('.originPrice').html('¥' + obj.Price);
            //                    $('.u-coursePlanPayMsg').attr('style', 'display:block;z-index:99;');
            //                    $("#message").html("试听已经结束,请购买课程后在继续观看视频!");
            //                }
            //            });
            //        }
            //    });
            //}
            player.on('ended', function (e) {
                //topevery.ajax({
                //    url: 'api/CourseLearnProgress/IfExist',
                //    data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
                //}, function (data) {
                //    if (!data.Result) {
                //        addLearnRecord();
                //    } else {
                //        updateLearnState();
                //    }
                //});
            });
            player.on('play', function (e) {
                //是否已看过该视频
                //topevery.ajax({
                //    url: 'api/CourseLearnProgress/IfExist',
                //    data: JSON.stringify({ CourseId: $('#CourseId').val(), ChapterId: $('#chapterid').val(), VideoId: $('#VideoId').val() })
                //}, function (data) {
                //    if (!data.Result) {
                //        addLearnRecord();
                //    }
                //})
            });
            $(function () {
                player.play();
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