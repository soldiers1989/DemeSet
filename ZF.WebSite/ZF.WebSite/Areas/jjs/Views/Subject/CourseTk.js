loadProject();
CourseInfoAll(topevery.GetCookie("SubjectId"));
//绑定项目
function loadProject() {
    //绑定项目
    topevery.ajaxwx({
        url: "api/CourseInfo/GetCourseSubject",
        data: JSON.stringify({ SubjectId: topevery.GetCookie("SubjectId") })
    }, function (data) {
        if (data.Success) {
            $("#subject").html(template("subject_html", data.Result));
            $(".qtkc").click(function (event) {
                $(".jjsc-tclb").toggle();
            });
            $(".jjsc-tclb a").click(function () {
                $(".jjsc-tclb").toggle();
                var subjectId = $(this).attr("subjectId");
                $("#SubjectName").html($(this).attr("SubjectName"));
                CourseInfoAll(subjectId);
            });
        }
    });
}
//课程查询方法
function CourseInfoAll(subjectId) {
    topevery.ajaxwx({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), SubjectId: subjectId, Type: 1 })
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses").html(template("PopularCourses_html", data.Result));
        }
    });
}