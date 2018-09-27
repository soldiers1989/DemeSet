$(".curSubject").show();
var isFree = "";
var sord = "Asc";
var projectSubjectMode;
var ProjectId;
var SubjectId;
$($(".navIn li")[0]).removeClass("cur");
$($(".navIn li")[2]).addClass("cur");
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
    CourseInfoAll();
});

//课程查询方法
function CourseInfoAll() {
    console.log(JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), IsFree: isFree, Sord: sord, SubjectId: SubjectId, ProjectId: ProjectId, CourseType: 0, Type: 1, IsValueAdded: 0 }));
    topevery.ajax({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), IsFree: isFree, Sord: sord, SubjectId: SubjectId, ProjectId: ProjectId, CourseType: 0, Type: 1, IsValueAdded: 0 }),
        async:false
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses").html(template("PopularCourses_html", data.Result));

            if (data.Result.Records > 0) {
                $('.M-box1').show();
                $('.M-box1').pagination({
                    pageCount: Math.ceil(data.Result.Records / parseInt($("#Rows").val())),
                    jump: true,
                    coping: true,
                    homePage: '首页',
                    endPage: '末页',
                    prevContent: '上页',
                    nextContent: '下页',
                    callback: function (api) {
                        $("#PageIndex").val(api.getCurrent());
                        CourseInfoAll1();
                    }
                });
            } else {
                $('.M-box1').hide();
            }
        }
    });
}

//课程查询方法
function CourseInfoAll1() {
    console.log('heheh',JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), IsFree: isFree, Sord: sord, SubjectId: SubjectId, ProjectId: ProjectId, CourseType: 0, Type: 1, IsValueAdded: 0 }));
    topevery.ajax({
        url: "api/CourseInfo/GetCourseInfoAll",
        data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), IsFree: isFree, Sord: sord, SubjectId: SubjectId, ProjectId: ProjectId, CourseType: 0, Type: 1, IsValueAdded: 0 })
    }, function (data) {
        if (data.Success) {
            $("#PopularCourses").html(template("PopularCourses_html", data.Result));
        }
    });
}
//初始化科目加载
//CourseInfoAll();
//绑定项目
topevery.ajax({
    url: "api/CourseInfo/GetProject",
    data: JSON.stringify({})
}, function (data) {
    if (data.Success) {
        $("#ProjectClassAll").html(template("ProjectClassAll_html", data));
        ProjectId = $("#ProjectClassAll .kclist_hover").attr("ProjectId");
        projectSubjectMode = data.Result;
        //CourseInfoAll();
        SubjectLoad(ProjectId);
        $("#ProjectClassAll a").click(function () {
            $("#ProjectClassAll a").removeClass("kclist_hover");
            $(this).addClass("kclist_hover");
            ProjectId = $(this).attr("ProjectId");
            SubjectLoad(ProjectId);
        });
    }
});

function SubjectLoad(projectId) {
    for (var i = 0; i < projectSubjectMode.length; i++) {
        if (projectSubjectMode[i].Id == projectId) {
            $("#SubjectAll").html(template("SubjectAll_html", projectSubjectMode[i]));
            SubjectId = "";
            
            CourseInfoAll();
            $("#SubjectAll a").click(function () {
                $("#PageIndex").val("1");
                $("#SubjectAll a").removeClass("kclist_hover");
                $(this).addClass("kclist_hover");
                SubjectId = $(this).attr("SubjectId");
                CourseInfoAll();
            });
        }
    }
}