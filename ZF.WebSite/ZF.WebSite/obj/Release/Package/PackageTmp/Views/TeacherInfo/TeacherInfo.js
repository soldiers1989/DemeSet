$(function () {
    Initilize();
    function Initilize() {
        $('#courselist').html("");
        var afterUrl =  window.location.search.substring(1);
        var teacherid = afterUrl.substring(afterUrl.indexOf('=') + 1);
        topevery.ajax({
            url: 'api/CourseInfo/GetList',
            data: JSON.stringify({ TeacherId :teacherid})
        }, function (data) {
            if (data.Success && data.Result.length > 0) {
                var arr = data.Result;
                var contentHtml = "";
                for (var i = 0; i < arr.length; i++) {
                    contentHtml += "<li class=\"uc-course-list_itm f-ib\">"
                                        + "<div class=\"uc-coursecard uc-ykt-coursecard f-fl\">"
                                            + "<a class=\"j-href\" href='/CourseInfo/CourseInfo?courseId=" + arr[i].Id + "'>"
                                                 + "<div class=\"uc-ykt-coursecard-wrap f-cb f-pr\">"
                                                        + "<div class=\"uc-ykt-coursecard-wrap_box\">"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_picbox f-pr\">"
                                                                + "<img class=\"imgPic j-img\" src=\"" + arr[i].CourseIamge + "\" alt=\"" + arr[i].TeachersName + "\" />"
                                                            + "</div>"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_tit\">"
                                                                 + "<h3 class=\"\">" + arr[i].CourseName + "</h3>"
                                                            + "</div>"
                                                            + "<div class=\"uc-ykt-coursecard-wrap_orgName f-fs0 f-thide\">"
                                                                + arr[i].TeachersName
                                                            + "</div>"
                                                            + " <div class=\"uc-ykt-coursecard-wrap_price f-pa\">"
                                                                + "<span class=\"u-discount\">¥ " + arr[i].FavourablePrice + "</span>"
                                                                + "<span class=\"u-normal z-discount\">¥" + arr[i].Price + "</span>"
                                                            + "</div>"
                                                        + "</div>"
                                                + "</div>"
                                            + "</a>"
                                        + "</div>"
                                    + "</li>";
                }
                $('#courselist').html(contentHtml);


                //设置高度
                //console.log(document.getElementById("courselist").clientHeight, document.getElementById("courselist").offsetTop);
                document.getElementById('courselist').style.height = document.getElementById("courselist").clientHeight + document.getElementById("courselist").offsetTop + "px";
            } else {
                $('#j-cpcourse-empty').attr("style", "display:block");
            }
        });


        topevery.ajax({
            url: 'api/CourseOnTeacher/GetInfo',
            data: JSON.stringify({Id:teacherid})
        }, function (data) {
            if (data.Success) {
                var teacherinfo = data.Result;
                console.log(teacherinfo);
                $('#teacherImage').attr('src', teacherinfo.TeacherPhoto);
                $('#infocontent').html(teacherinfo.Synopsis);
            } else {
                $('#j-cpteacher-empty').arrt('style','display:block;')
            }
        })
        
    }

    $('.ux-tabs_hd li').on('click', function (e) {

        var id = $(this).data("id");
        $(".ux-tabs_hd li").each(function () {
            $(this).removeClass("z-crt");
        });
        $(this).addClass("z-crt");
        if (id == "metag") {
            $("#aboutcourse").attr("style", "display:none;");
            $('#aboutme').attr("style", "display:block;");

        } else {
            $("#aboutcourse").attr("style", "display:block;");
            $('#aboutme').attr("style", "display:none;");
        }
        
    })
})