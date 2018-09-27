topevery.SetCookie("PromotionCode", topevery.getQueryString("PromotionCode"));
document.addEventListener("error", function (e) {
    var elem = e.target;
    if (elem.tagName.toLowerCase() == 'img') {
        elem.src = "/Libs/CourseInfo/imgs/temp_couver.jpg";
    }
}, true);

$(".headImg").hover(function () {
    $("#login1").css("display", "block");
},
function () {
    $("#login1").hover(function () {

    }, function () {
        $("#login1").css("display", "none");
    });
});


//绑定课程以及科目
topevery.ajax({
    url: "api/CourseInfo/GetProject",
    data: JSON.stringify({})
}, function (data) {
    if (data.Success) {
        $("#CourseAll").html(template("CourseAll_html", data));
    }
});
//绑定项目
topevery.ajax({
    url: "api/Project/GetProjectAll",
    data: JSON.stringify({})
}, function (data) {
    if (data.Success) {
        $("#ProjectAll").html(template("ProjectAll_html", data));
        $(".Project").hover(function () {
            $("#CourseAll").show();
            $(".ProjectCourseInfo").hide();
            $("." + $(this).attr("id") + "").show();
        }, function () {
            $("." + $(this).attr("id") + "").hover(function () {

            }, function () {
            });
        });
        $("#j-nav-indexcatedialog").hover(function () { }, function () {
            $("#CourseAll").hide();
            $(".ProjectCourseInfo").hide();
        });
    }
});
//绑定免费课程
topevery.ajax({
    url: "api/CourseInfo/GetCourseInfoAll",
    data: JSON.stringify({ IsFree: 0, Page: 1, Rows: 4, IsRecommend: 1, IsValueAdded: 0 })
}, function (data) {
    if (data.Success) {
        $("#FreeClasses").html(template("FreeClasses_html", data.Result));
    }
});

topevery.ajax({
    url: "api/BaseDanye/GetArguValue?arguName=webdescript",
}, function (data) {
    if (data.Success) {
        if (data.Result.Success) {
            $("#webdescript").html(data.Result.Message);
        }
    }
});

//绑定畅销好课
topevery.ajax({
    url: "api/CourseInfo/GetCourseInfoAll",
    data: JSON.stringify({ IsFree: 1, Page: 1, Rows: 10, IsRecommend: 1, IsValueAdded: 0 })
}, function (data) {
    if (data.Success) {
        $("#PopularCourses").html(template("PopularCourses_html", data.Result));
    }
});
var object = new Object();
object.userToken = topevery.GetCookie("userToken");
object.CartCount = 0;
if (object.userToken && object.userToken !== "") {
    topevery.ajax({
        type: "GET",
        url: "api/Home/GetUserInfoByTicket"
    }, function (data) {
        if (data.Success) {
            object.NickNamw = data.Result.NickNamw;
            object.CartCount = data.Result.CartCount;
            object.LearnCount = data.Result.LearnCount;
            object.HeadImage = data.Result.HeadImage;
            $("#LoginIndex").html(template("question_common1", object));
            $(".j-nav-myimg").hover(function () {
                $(".u-navusermenu").addClass("x-show");
                $(".u-navusermenu").removeClass("x-hide");
            }, function () {
                $(".u-navusermenu").hover(function () {
                    $(".u-navusermenu").addClass("x-show").removeClass("x-hide");
                }, function () {
                    $(".u-navusermenu").addClass("x-hide").removeClass("x-show");
                });
                $(".u-navusermenu").addClass("x-hide").removeClass("x-show");
            });
        }
    });
} else {
    $("#LoginIndex").html(template("question_common1", object));
}


//退出
function outUser() {
    topevery.DelCookie("userToken"); // 删除 cookie
    //刷新当前页面.
    window.location.reload();
}

//搜索
$('#btn_search').click(function () {
    var input = encodeURI($('#searchcourseinfo').val());
    location.href = "/CourseQueries/IndexQuery?query=" + input;
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
        indexs = data.Result.length;
        $("#a div").click(function () {
            var index = $(this).index();
            $("#pic img").css("display", "none");
            $("#pic img").eq(index).css("display", "block");
            $("#pic img").eq(index).fadeIn(3000);
            $("#a div").eq(index).fadeIn(3000, function () {
                $(this).css("height", "20px");
            });
        });
    }
});

//<!-- 滚动的广告 -->
var n = 0;
var time = setInterval(function () {
    if (n < indexs) {
        $("#pic img").css("display", "none");
        $("#pic img").eq(n).css("display", "block");
        $("#a div").css("height", "10px");
        n++;
    } else {
        n = 0;
    }
}, 3000);

//跳转登录页
function userLogin() {
    layer.open({
        type: 2,
        title: '用户登录',
        shadeClose: true,
        maxmin: false, //开启最大化最小化按钮
        area: ['650px', '430px'],
        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
        content: '/Login/UserLogin?RefUrl=' + location.href,
        end: function () {
        }
    });
}

$("#xxzx").click(function () {
    if (topevery.GetCookie("userToken")) {
        location.href = "/CourseInfo/MyStudy";
    } else {
        userLogin();
    }
})