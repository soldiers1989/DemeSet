topevery.SetCookie("PromotionCode", topevery.getQueryString("PromotionCode"));

$(".curSubject").show();

var Rows1 = 4;
CourseInfoAll();
function CourseInfoAll() {
    //绑定畅销好课
    topevery.ajax({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ IsFree: 1, Page: 1, Rows: Rows1, IsRecommend: 1, SubjectId: topevery.GetCookie("subjectId") })
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses").html(template("PopularCourses_html", data.Result));
        }
    });
}
$(".PopularCoursesMore").click(function () {
    location.href = "/CourseQueries/IndexQuery";
});

var Rows = 3;
AfficheHelp();
function AfficheHelp() {
    //绑定资讯
    topevery.ajax({
        url: "api/BaseDanye/GetAfficheHelp",
        data: JSON.stringify({ IsIndex: 0, Type: 0, Page: 1, Rows: Rows })
    }, function (data) {
        if (data.Success) {
            for (var i = 0; i < data.Result.length; i++) {
                data.Result[i].AddTime = topevery.dataTimeView(data.Result[i].AddTime);
            }
            $("#Affiche").html(template("Affiche_html", data));
        }
    });
}
$(".AfficheMore").click(function () {
    Rows += 3;
    AfficheHelp();
});


var indexs = 0;
//绑定滚动图片
topevery.ajax({
    url: "api/BaseDanye/GetSlideSettingList",
    data: JSON.stringify({}),
    async: false
}, function (data) {
    if (data.Success) {
        $("#pic").html(template("pic_html", data));
        layui.use(['jquery', 'form', 'carousel'], function () {
            var $ = layui.jquery, form = layui.form, carousel = layui.carousel;
            //建造实例
            carousel.render({
                elem: '#test1',
                width: '100%', //设置容器宽度
                height:'300px',//设置轮播容器高度
                arrow: 'always', //始终显示箭头
                anim: 'fade' //切换动画方式
            });
        });
    }
});

