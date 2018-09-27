$(function () {
    GetCollectedCourse();
})
var pageIndex = 1;
var obj = new Object();
var total = 0;
var rows = 4;
function GetCollectedCourse() {
    topevery.ajaxwx({
        //url: 'api/MyStudy/GetCollectedCourse',
        url: 'api/MyCollection/GetList',
        data: JSON.stringify({ Page: pageIndex, Rows: rows, CourseName: '' })
    }, function (data) {
        if (data.Success) {
            obj = data.Result;
            total = data.Result.Records;
            $('#collectedCourse').html(template("collectedCourse_html", obj));
        }
    });
}

$(window).scroll(function () {

    var scrollTop = $(this).scrollTop();    //滚动条距离顶部的高度  
    var scrollHeight = $(document).height();   //当前页面的总高度  
    var clientHeight = $(this).height();    //当前可视的页面高度  
     console.log("top:"+scrollTop+",doc:"+scrollHeight+",client:"+clientHeight);  
     if (scrollTop + clientHeight == scrollHeight) {   //距离顶部+当前高度 >=文档总高度 即代表滑动到底部   
         //滚动条到达底部  
         $('.loadmore').attr('style', 'display:block;')
         //$('.loadmore').attr('style','display:block;');
         //} else if (scrollTop <= 0) {
         //    //滚动条到达顶部  
         //    alert(4)
         //    //滚动条距离顶部的高度小于等于0 TODO  

         //}
     } else {
         $('.loadmore').attr('style', 'display:none;')
     }
});

function showmore() {
    if (pageIndex * rows < total) {
        pageIndex += 1;
        topevery.ajaxwx({
            url: 'api/MyCollection/GetList',
            data: JSON.stringify({ Page: pageIndex, Rows: rows, CourseName: '' })
        }, function (data) {
            if (data.Success) {
                var arr = data.Result.Rows;
                for (var i = 0; i < arr.length; i++) {
                    obj.Rows.push(arr[i]);
                }
                $('#collectedCourse').html(template("collectedCourse_html", obj));
            }
        })
    } else {
        $('.loadmore').text('无更多数据');
    }
}

template.helper('formatterDate', function (datetime) {
    if (datetime) {
        return datetime.split(' ')[0];
    }
})


//function jumpToVideo(event) {
//    var videoId = $(event).attr("VideoId");
//    //通过视频ID查询课程章节以及课程
//    topevery.ajaxwx({
//        url: "api/MyCollectionItem/GetVideoInfo?videoId=" + videoId,
//    }, function (data) {
//        if (data.Success) {
//            var obj = data.Result
//            var url = "/jjs/CourseInfo/VideoPlay?chapterId=" + obj.ChapterId + "&courseId=" + obj.CourseId + "&videoId=" + obj.VideoId;
//            location.href = url;
//        }
//    })
//}

///取消收藏
function remove(event) {
    var videoid = $(event).attr("VideoId");
    var courseId = $(event).attr("CourseId");
    topevery.ajaxwx({
        url: 'api/MyCollection/CancelCollectVideo',
        data: JSON.stringify({ CourseId: courseId, VideoId: videoid })
    }, function (data) {
        if (data.Result.Success) {
            GetCollectedCourse();
        }
    });
}


