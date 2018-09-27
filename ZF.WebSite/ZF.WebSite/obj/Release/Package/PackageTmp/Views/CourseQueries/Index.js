$(function () {
    var sord = "Asc";
    var isFree = 4;
    var projectId = $('#ProjectId').val();
    var projectClassId = $("#ProjectClassId").val();
    var subjectId = $("#SubjectId").val();


    topevery.ajax({
        url: "api/CourseInfo/GetProjectClassId?projectClassId=" + projectClassId,
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $("#maintain_info_box").html(template("maintain_info_box_html", data.Result));
        }
    });

    //if (!projectId && !projectClassId && !subjectId) {
    //    topevery.ajax({
    //        url: "api/Project/GetProjectClassAll",
    //        data: JSON.stringify({})
    //    }, function (data) {
    //        if (data.Success) {
    //            $("#ProjectClassAll").html(template("ProjectClassAll_html", data));
    //            $(".level-2").hide();
    //        }
    //    });
    //}
    //else {
        //绑定所有科目
        topevery.ajax({
            url: "api/Project/GetSubjectAll",
            data: JSON.stringify({})
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                var html = "";
                for (var i = 0; i < row.length; i++) {
                    html += "<li class='level-2-slash'><a class='A' dd=" + row[i].Id + ">" + row[i].SubjectName + "</a></li>";
                }
                if (!html) {
                    $(".level-2").hide();
                } else {
                    $(".level-2").show();
                }
                $("#SubjectAll").html(html);
                $(".A").click(function () {
                    $(".A").attr("style", "color: #959595");
                    $(this).attr("style", "color: #10ae58");
                    $("#SubjectId").val($(this).attr("dd"));
                    loadingCourse();
                });
            }
        });
        //绑定项目
        topevery.ajax({
            url: "api/Project/GetProjectAll",
            data: JSON.stringify({ ProjectClassId: projectClassId })
        }, function (data) {
            if (data.Success) {
                $("#ProjectAll").html(template("ProjectAll_html", data));
                $(".myType").attr("style", "color: #333");
                if ($("#ProjectId").val()) {
                    //绑定科目
                    topevery.ajax({
                        url: "api/Project/GetSubjectAll",
                        data: JSON.stringify({ ProjectId: $("#ProjectId").val() })
                    }, function (data) {
                        if (data.Success) {
                            var row = data.Result;
                            var html = "";
                            for (var i = 0; i < row.length; i++) {
                                html += "<li class='level-2-slash'><a class='A " + row[i].Id + "' dd=" + row[i].Id + ">" + row[i].SubjectName + "</a></li>";
                            }
                            if (!html) {
                                $(".level-2").hide();
                            } else {
                                $(".level-2").show();
                            }
                            $("#SubjectAll").html(html);
                            $(".A").click(function () {
                                $(".A").attr("style", "color: #959595");
                                $(this).attr("style", "color: #10ae58");
                                $("#SubjectId").val($(this).attr("dd"));
                                loadingCourse();
                            });
                            $("." + $("#ProjectId").val() + "").attr("style", "color: #10ae58");
                            if ($("#SubjectId").val()) {
                                $("." + $("#SubjectId").val() + "").attr("style", "color: #10ae58");
                            }
                            loadingCourse();
                        }
                    });
                }
                $(".myType").click(function () {
                    $("#ProjectAll li").removeClass("selected");
                    $(this).parent().addClass("selected");

                    $(".myType").attr("style", "color: #333");
                    $(this).attr("style", "color: #10ae58");
                    var id = $(this).attr('value');
                    //绑定项目
                    topevery.ajax({
                        url: "api/Project/GetSubjectAll",
                        data: JSON.stringify({ ProjectId: id })
                    }, function (data) {
                        if (data.Success) {
                            var row = data.Result;
                            var html = "";
                            for (var i = 0; i < row.length; i++) {
                                html += "<li class='level-2-slash'><a class='A ' dd=" + row[i].Id + ">" + row[i].SubjectName + "</a></li>";
                            }
                            if (!html) {
                                $(".level-2").hide();
                            } else {
                                $(".level-2").show();
                            }
                            $("#SubjectAll").html(html);
                            $(".A").click(function () {
                                $(".A").attr("style", "color: #959595");
                                $(this).attr("style", "color: #10ae58");
                                $("#SubjectId").val($(this).attr("dd"));
                                loadingCourse();
                            });
                        }
                    });
                    //加载该项目下所有课程
                    projectId = id;
                    $('#SubjectId').val('');
                    loadingCourse();
                    return false;
                });
            }
        });
    //}
    //绑定课程科目点击事件
    $(".m-cSelUI a").click(function () {
        $(".m-cSelUI a").removeClass("selected");
        $(this).addClass("selected");
        $("#CourseType").val($(this).attr("data"));
        loadingCourse();
    });


    //绑定免费收费点击事件
    $(".uc-course-list_dropdown a").click(function () {
        var data = $(this).attr("data");
        var sords = $(this).attr("sord");
        if (sords === "Asc") {
            $(this).find("i").addClass("down-ico");
            $(this).attr("sord","Desc");
        } else {
            $(this).find("i").removeClass("down-ico");
            $(this).attr("sord","Asc");
        }
        sord = sords;
        if (data === "1") {
            $(".uc-course-list_dropdown a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 1;
        } else if (data === "2") {
            $(".uc-course-list_dropdown a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 2;
        } else if (data === "3") {
            $(".uc-course-list_dropdown a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 3;
        } else if (data === "4") {
            $(".uc-course-list_dropdown a").removeClass("selected");
            $(this).addClass("selected");
            isFree = 4;
        }
        loadingCourse();
    });


    //课程查询方法
    function loadingCourse1() {
        topevery.ajax({
            url: "api/CourseInfo/GetCourseInfoAll",
            data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), SubjectId: $("#SubjectId").val(), CourseType: $("#CourseType").val(), IsFree: isFree, ProjectId: projectId, projectClassId: projectClassId, Sord: sord })
        }, function (data) {
            if (data.Success) {
                $("#PopularCourses").html(template("PopularCourses_html", data.Result));
            }
        });
    }



    //课程查询方法
    function loadingCourse() {
        topevery.ajax({
            url: "api/CourseInfo/GetCourseInfoAll",
            data: JSON.stringify({ Page: $("#PageIndex").val(), Rows: $("#Rows").val(), SubjectId: $("#SubjectId").val(), CourseType: $("#CourseType").val(), IsFree: isFree, ProjectId: projectId, projectClassId: projectClassId, Sord: sord })
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
                            loadingCourse1();
                        }
                    });
                } else {
                    $('.M-box1').hide();
                }
            }
        });
    }
    //初始化科目加载
    loadingCourse();


    //绑定付费课程
    topevery.ajax({
        url: "api/CourseInfo/GetPaidListings",
        data: JSON.stringify({ Charge: 1 })
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            var html = '';
            for (var i = 0; i < row.length; i++) {
                var courseTypehtml = row[i].CourseType === 0 ? "/CourseInfo/CourseInfo?courseId=" + row[i].Id : "/CourseInfo/CoursePackInfo?courseId=" + row[i].Id;
                html += ' <div class="u-rankItm f-cb">' +
                    '<div class="f-fl num num' + (i + 1) + '">0' + (i + 1) + '</div>' +
                    '<a class="f-fl f-fc3 f-thide name" data-index="' + (i + 1) + '" data-name="' + row[i].CourseName + '" data-gatitle="付费排行" href="' + courseTypehtml + '" target="_blank" title="' + row[i].CourseName + '">' + row[i].CourseName + '</a>' +
                    '</div>';
            }
            $("#pay").html(html);
        }
    });

    //绑定免费课程
    topevery.ajax({
        url: "api/CourseInfo/GetPaidListings",
        data: JSON.stringify({ Charge: 0 }),
    }, function (data) {
        if (data.Success) {
            var row = data.Result;
            var html = '';
            for (var i = 0; i < row.length; i++) {
                var courseTypehtml = row[i].CourseType === 0 ? "/CourseInfo/CourseInfo?courseId=" + row[i].Id : "/CourseInfo/CoursePackInfo?courseId=" + row[i].Id;
                html += ' <div class="u-rankItm f-cb">' +
                    '<div class="f-fl num num' + (i + 1) + '">0' + (i + 1) + '</div>' +
                  '<a class="f-fl f-fc3 f-thide name" data-index="' + (i + 1) + '" data-name="' + row[i].CourseName + '" data-gatitle="付费排行" href="' + courseTypehtml + '" target="_blank" title="' + row[i].CourseName + '">' + row[i].CourseName + '</a>'
                   +
                  '</div>';
            }
            $("#free").html(html);
        }
    });

    $(".u-topTit a").click(function () {
        $(".u-topTit a").removeClass("selected");
        $(this).addClass("selected");
        $("#" + $(this).attr("data")).show();
        $("#" + $(this).attr("hidedata")).hide();
    });

    if (topevery.getQueryString("type")) {
        if (topevery.getQueryString("type") == "free") {
            $($(".uc-course-list_dropdown a")[0]).click();
        }
    }

    var indexs = 0;
    //绑定滚动图片
    topevery.ajax({
        url: "api/BaseDanye/GetSlideSettingCourseList",
        data: JSON.stringify({}),
        async: false
    }, function (data) {
        if (data.Success) {
            indexs = data.Result.length;
            $("#logo").html(template("pic_html", data));
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
            $("#logo img").css("display", "none");
            $("#logo img").eq(n).css("display", "block");
            $("#a div").css("height", "10px");
            n++;
        } else {
            n = 0;
        }
    }, 3000);
});