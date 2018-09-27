var id = "";
var data = {
    Initialize: function () {
        //绑定资讯详情
        topevery.ajax({
            url: "api/BaseDanye/GetAfficheHelpView",
            data: JSON.stringify({ Id: id })
        }, function (data) {
            if (data.Success) {
                document.getElementsByTagName('title')[0].innerText = data.Result.Title;
                $("#newsDetails").html(template("newsDetails_html", data.Result));
            }
        });
    },
    GetList: function () {
        //绑定资讯
        topevery.ajax({
            url: "api/BaseDanye/GetAfficheHelp",
            data: JSON.stringify({ Type: 0, Id: topevery.getQueryString("Id"), Page: 1, Rows: 4 })
        }, function (data) {
            if (data.Success) {
                $("#down").html(template("down_html", data.Result));
            }
        });
    }
    , //绑定畅销好课
    bindingCourse: function () {
        topevery.ajax({
            url: "api/CourseInfo/GetCourseInfoAll",
            data: JSON.stringify({ IsFree: 1, Page: 1, Rows: 4, SubjectId: topevery.GetCookie("SubjectId"), Type: 0, IsValueAdded: 0 })
        }, function (data) {
            if (data.Success) {
                $("#PopularCourses").html(template("PopularCourses_html", data.Result));
            }
        });
    }
};
$(function () {
    if (!id) {
        id = topevery.getQueryString("Id");
    }
    data.Initialize();
    data.GetList();
    data.bindingCourse();
})

function getInformation(event) {
    id = $(event).attr("id");
    data.Initialize();
}