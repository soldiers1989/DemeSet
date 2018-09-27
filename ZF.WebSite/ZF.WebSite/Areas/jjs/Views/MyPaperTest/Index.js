$(function () {

    GetTestRecord();

})

var pageIndex = 1;
var rows = 5;
var total = 0;
var obj = new Object();

function GetTestRecord() {
    topevery.ajaxwx({
        url: 'api/CoursePaperRecord/GetList',
        data: JSON.stringify({ query: "", page: pageIndex, rows: rows })
    }, function (data1) {
        if (data1.Success) {
            obj = data1.Result;
            total = data1.Result.Records;
            $("#papertest").html(template("papertest_html", data1.Result));
        }
    });
}

$(window).scroll(function () {

    var scrollTop = $(this).scrollTop();    //滚动条距离顶部的高度  
    var scrollHeight = $(document).height();   //当前页面的总高度  
    var clientHeight = $(this).height();    //当前可视的页面高度  
    //console.log("top:" + scrollTop + ",doc:" + scrollHeight + ",client:" + clientHeight);
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
            url: 'api/CoursePaperRecord/GetList',
            data: JSON.stringify({ Page: pageIndex, Rows: rows, CourseName: '' })
        }, function (data) {
            if (data.Success) {
                var arr = data.Result.Rows;
                for (var i = 0; i < arr.length; i++) {
                    obj.Rows.push(arr[i]);
                }
                $('#papertest').html(template("papertest_html", obj));
            }
        })
    } else {
        $('.loadmore').text('无更多数据');
    }
}

function jxcp(PaperId) {
    topevery.ajaxwx({
        url: "api/Economy/InsertIntoPaperRecords?PaperId=" + PaperId,
        type: "Post",
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            if (row.PaperId == "-1") {
                parent.layer.msg("该试卷暂未上线,敬请期待!", {
                    time: 4000
                });
            } else {
                //跳转到试卷页面
                window.location.href = "/jjs/MyPaperTest/EnterPaper?PaperId=" + row.PaperId + "&paperRecordsId=" + row.PaperRecordsId;
            }
        }
    });
}
