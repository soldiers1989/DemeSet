$(function () {
    $(".curSubject").show();
    $("#searchcourseinfo").val(decodeURI(topevery.getQueryStringHtlm("query")));
    $("#subjectName1").html(topevery.GetCookie("subjectName") + "&gt;");
    var isFree = "";
    var sord = "Asc";
    //绑定免费收费点击事件
    $(".right a").click(function () {
        var data = $(this).attr("data");
        var sords = $(this).attr("sord");
        if (sords === "Asc") {
            $(this).find("i").addClass("down-ico");
            $(this).attr("sord", "Desc");
        } else {
            $(this).find("i").removeClass("down-ico");
            $(this).attr("sord", "Asc");
        }
        sord = sords;
        if (data === "1") {
            $(".right a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 1;
        } else if (data === "2") {
            $(".right a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 2;
        } else if (data === "3") {
            $(".right a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 3;
        } else if (data === "4") {
            $(".right a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 4;
        }
        loadingCourse();
    });

    //课程查询方法
    function loadingCourse() {
        topevery.ajax({
            url: "api/CourseInfo/GetCourseInfoAll",
            data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), IsFree: isFree, Sord: sord, SubjectId: topevery.GetCookie("subjectId") })
        }, function (data) {
            if (data.Success) {
                $("#PopularCourses").html(template("PopularCourses_html", data.Result));
            }
        });
    }
    //初始化科目加载
    loadingCourse();
});