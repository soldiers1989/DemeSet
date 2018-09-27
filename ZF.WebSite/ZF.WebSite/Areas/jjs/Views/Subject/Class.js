loadProject();
//绑定项目
function loadProject() {
    //绑定项目
    topevery.ajaxwx({
        url: "api/CourseInfo/GetProject",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            data.subjectId = topevery.GetCookie("SubjectId");
            $("#ProjectAll").html(template("ProjectAll_html", data));
            $(".kcfl_mc a").click(function () {
                topevery.SetCookie("SubjectId", $(this).attr("subjectId"));
                location.href = "/jjs/HomePage/Index";
                //if ($(this).attr("subjectId")) {
                //    topevery.ajaxwx({
                //        url: "api/MyCollection/UpdateSubject?subjectId=" + $(this).attr("subjectId")
                //    }, function (data) {
                //        if (data.Success) {
                //            location.href = "/jjs/HomePage/Index";
                //        }
                //    });
                //}
            });
        }
    });
}