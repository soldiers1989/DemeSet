$(function () {
    topevery.ajaxwx({
        url: "api/BaseDanye/GetDataType?code=zxgl",
    }, function (data) {
        if (data.Success) {
            $("#GetDataType").html(template("GetDataType_html", data));
            $('ul>li>a').first().addClass("hover");
            $('ul>li').first().find('a').click();
        }
    });
})

var BigClassId = "";
function GetClassList(event) {
    BigClassId = event;
    $('ul>li>a').each(function () {
        $(this).removeClass("hover");
    })
    $("." + event).addClass('hover');
    topevery.ajax({
        url: "api/BaseDanye/GetAfficheHelp",
        data: JSON.stringify({ Type: 0, BigClassId: event, page: $('#PageIndex').val(), rows: $('#Rows').val() })
    }, function (data1) {
        if (data1.Success) {
            for (var i = 0; i < data1.Result.Rows.length; i++) {
                data1.Result.Rows[i].Content = topevery.delHtmlTag(data1.Result.Rows[i].Content);
                data1.Result.Rows[i].AddTime = topevery.dataTimeView(data1.Result.Rows[i].AddTime);
            }
            $("#Affiche").html(template("Affiche_html", data1.Result));
        }
    });
}
$(window).scroll(function () {
    //已经滚动到上面的页面高度
    var scrollTop = $(this).scrollTop();
    //页面高度
    var scrollHeight = $(document).height();
    //浏览器窗口高度
    var windowHeight = $(this).height();
    //此处是滚动条到底部时候触发的事件，在这里写要加载的数据，或者是拉动滚动条的操作
    if (scrollTop + windowHeight == scrollHeight) {
        $('#Rows').val(parseInt($('#Rows').val()) + 5);
        topevery.ajax({
            url: "api/BaseDanye/GetAfficheHelp",
            data: JSON.stringify({ Type: 0, BigClassId: BigClassId, page: $('#PageIndex').val(), rows: $('#Rows').val() })
        }, function (data1) {
            if (data1.Success) {
                for (var i = 0; i < data1.Result.Rows.length; i++) {
                    data1.Result.Rows[i].Content = topevery.delHtmlTag(data1.Result.Rows[i].Content);
                    data1.Result.Rows[i].AddTime = topevery.dataTimeView(data1.Result.Rows[i].AddTime);
                }
                $("#Affiche").html(template("Affiche_html", data1.Result));
            }
        });
    }
});
