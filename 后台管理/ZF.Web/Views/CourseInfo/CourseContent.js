$(function () {

    var CourseInfo = {
        formatterState: function (state) {
            return state == 1 ? "已上架" : "未上架"
        },
        formatterPrice: function (price) {
            return '' + price + '￥';
        },
    }
    var header = $(".content-wrapper").outerHeight(true)
                    - $(".nav-header").outerHeight(true) - 3
                    - $(".content-header").outerHeight(true)
                    - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                    - 31;
    $(".KnowledgePointTree").height(header);
    var getPostDataUrl = "api/CourseInfo/GetList";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: false,
        colNames: ['编码', '课程名称', '课程所属科目', '科目ID', '项目ID', '有效截止日期', '上下架状态', '主讲教师', '', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "left", hidden: true },
            { name: 'CourseName', width: 70, align: "left" },
            { name: 'SubjectName', width: 30, align: "center" },
            { name: 'SubjectId', width: 0, align: 'center', hidden: true },
            { name: 'ProjectId', width: 0, align: 'center', hidden: true },
            //{ name: 'CourseContent',  width: 100, align: "center" },
            {
                name: 'ValidityEndDate', width: 20, align: "center", formatter: topevery.dataTimeView
            },
            { name: 'State', width: 20, align: "center", formatter: CourseInfo.formatterState },
            { name: 'TeachersName', width: 30, align: "center" },
            {
                name: '', width: 45, align: "center", formatter: function (cellValue, options, rowdata, action) {
                    //return (" <div style='float:left;'> <a  onclick=\"editContent(this)\" title=\"课程内容维护\" data-state=\"" + rowdata.State + "\"  data-id=\"" + rowdata.Id + "\" class='glyphicon glyphicon-pencil' style=\"cursor:pointer\">课程内容维护</a></div>")
                    //    + "<div style='float:left;'><a onclick=\"editChapterExercise(this)\" title=\"章节练习维护\" data-projectId=\"" + rowdata.ProjectId + "\" data-courseName=\"" + rowdata.CourseName + "\" data-subject=\"" + rowdata.SubjectId + "\"  data-id=\"" + rowdata.Id + "\" class='glyphicon glyphicon-pencil' style=\"cursor:pointer\">章节练习维护</a></div>\n\
                    //" + "<div style='float:left;' id=\"layer" + rowdata.Id + "\" class=\"layer-photos-demo\"><label style='font-size: 10px;color: #3c8dbc;cursor: pointer;' onclick=\"createTwoDimensionalCode(this,0)\" name=\"" + rowdata.Id + "\" >按编号生成二维码</label>&nbsp;&nbsp;<label name=\"" + rowdata.Id + "\" onclick=\"createTwoDimensionalCode(this,1)\" style='font-size: 10px;color: #3c8dbc;cursor: pointer;'>按视频生成二维码</label></div>";
                    return "<div class=\"btn-group\">"
                            + "<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\"> "
                            + "操作 <span class=\"caret\"></span>"
                            + "</button>"
                            + "<ul class=\"dropdown-menu\" role=\"menu\">"
                                + "<li><a  onclick=\"editContent(this)\"  data-state=\"" + rowdata.State + "\"  data-id=\"" + rowdata.Id + "\"  style=\"cursor:pointer\">课程内容维护</a></li>"
                                + "<li><a onclick=\"editChapterExercise(this)\"  data-projectId=\"" + rowdata.ProjectId + "\" data-courseName=\"" + rowdata.CourseName + "\" data-subject=\"" + rowdata.SubjectId + "\"  data-id=\"" + rowdata.Id + "\"  style=\"cursor:pointer\">章节练习维护</a></li>"
                         + "<li><a onclick=\"CourseSecurityCode(this)\" style=\"cursor:pointer\" name=\"" + rowdata.Id + "\">课程防伪码维护</a></li>"
                                + "<li><a onclick=\"createTwoDimensionalCode(this,0)\" style=\"cursor:pointer\" name=\"" + rowdata.Id + "\">按编号生成二维码</a></li>"
                                + "<li><a onclick=\"createTwoDimensionalCode(this,1)\" style=\"cursor:pointer\" name=\"" + rowdata.Id + "\">按视频生成二维码</a></li>"
                            + "</ul>"
                            + "</div>";
                }
            },
               { name: '', width: 50, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom"),
        pager: '#contentpager',
        loadComplete: function () {
            $("tbody>tr>td").attr("title", "");
        }
    });

    //<img onclick=\"createTwoDimensionalCode(this)\" name=\"" + rowdata.CourseName + "\" title=\"查看二维码\" id=\"" + rowdata.Id + "\" style='cursor:pointer;' layer-src=\"../Images/searchbox_button.png\" src=\"../Images/searchbox_button.png\" alt=\"" + rowdata.CourseName + "\" /> 

    //查询
    $(".query_btn").on("click", function () {
        var postData = { CourseName: $('#QueryCourseName').val(), TeacherId: $('#QueryTeacherId').val(), ProjectId: $('#QueryProjectId').val(), SubjectId: $('#QuerySubjectId').val(), ProjectClassId: $('#QueryProjectClassId').val(), Type: $("#QueryType").val() };
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: postData
        }).trigger("reloadGrid");
    });


    //绑定教师列表
    //topevery.BindSelect("QueryTeacherId", "Common/TeacherList", "--请选择--");
    topevery.ajax({
        url: "Common/SelectTeacherList",
        data: JSON.stringify({}),
        async: false
    }, function (data) {
        $("#QueryTeacherId").select2({
            data: data,
            placeholder: { id: '-1', text: '请选择' },
            dropdownParent: $(".col-xs-10"),
            allowClear: true
        })
        $('#QueryTeacherId').val('-1').trigger('change');
    });

    //点击树结构自动查询结果
    topevery.BindTree("subjectTree", "Common/CourseInfoPointTree", beforeClick, "");
    function beforeClick(data) {
        if (data.level == 0) {
            $('#QueryProjectClassId').val(data.id);
            $('#QueryProjectId').val('');
            $("#QuerySubjectId").val('');
        }
        if (data.level == 1) {
            $('#QueryProjectClassId').val('');
            $("#QuerySubjectId").val("");
            $("#QueryProjectId").val(data.id);
        } else if (data.level === 2) {
            $('#QueryProjectClassId').val('');
            $("#QuerySubjectId").val(data.id);
            $("#QueryProjectId").val("");
            addUrl = "CourseInfo/AddOrEdit?SubjectId=" + $("#QuerySubjectId").val();
        }
        $(".query_btn").click();
    }




    //点击课程资源标签,加载课程资源列
    $('#tab_courseResource').on('click', function () {
        if (courseId == "") {
            return;
        }
        $('#panel-resource').html('');
        topevery.ajax({ type: "get", url: "CourseResource/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            $(data).appendTo($('#panel-resource'));
            $('#tab_courseResource').tab('show');
        }, true);
    })

    //点击课程视频标签，加载课程视频
    $('#tab_courseVideo').on('click', function () {
        $('#panel-video').html('');
        topevery.ajax({ type: "get", url: "CourseVideo/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            //加载章节树
            $(data).appendTo($('#panel-video'));
            $('#tab_courseVideo').tab('show');
        }, true);
    })

    //课程试题
    $('#tab_coursePaper').on('click', function () {
        $('#panel-paper').html('');
        topevery.ajax({
            type: 'get',
            url: 'CoursePaper/Index?courseId=' + courseId,
            dataType: 'html'
        }, function (data) {
            $(data).appendTo($('#panel-paper'));
            $('#tab_paper').tab('show');
        })
    })


    //查看所有二维码
    //$("#twoDimensionalCode").on("click", function () {
    //    var parment = new Object();
    //    parment.CourseName = $('#QueryCourseName').val();
    //    parment.TeacherId = $('#QueryTeacherId').val();
    //    parment.ProjectId = $('#QueryProjectId').val();
    //    parment.SubjectId = $('#QuerySubjectId').val();
    //    parment.ProjectClassId = $('#QueryProjectClassId').val();
    //    $.StandardPost('CourseInfo/CourseDimensionalCodeInfo', parment);
    //});
});


var courseId = '';
//单击数据表格单行
function rowClick(rowId) {
    $('#panel-chapter').html('');
    var rowData = $("#tblData").jqGrid('getRowData', rowId);

    courseId = rowData.Id;//课程编码
    topevery.ajax({ type: "get", url: "CourseChapter/Index?courseId=" + courseId, dataType: "html" }, function (data) {
        $(data).appendTo($('#panel-chapter'));
        $('#tab_courseChapter').tab('show');
    }, true);
}

var courseDetailUrl = "CourseInfo/CourseInfoDetail"

function contentLink(cellValue, options, rowdata, action) {
    return "<span style='color:#00c0ef;cursor:pointer;' data-id='" + rowdata.Id + "' data-subjectname='" + rowdata.SubjectName + "' data-courseName='" + rowdata.CourseName + "' data-teachersname='" + rowdata.TeachersName + "' onclick='checkCourseDetail(this)'>查看</span> ";
}

function checkCourseDetail(event) {
    var id = $(event).data("id");
    var subjectName = $(event).data("subjectname");
    var courseName = $(event).data("coursename");
    var teachersName = $(event).data("teachersname");
    window.location.href = courseDetailUrl;
    //$.ajax({ type: "get", url: courseDetailUrl + "?Id=" + id + "&SubjectName=" + subjectName + "&CourseName=" + courseName + "&TeachersName=" + teachersName }, function () {
    //    window.location.href = courseDetailUrl;
    //});
}


///点击操作按钮,跳转页面
function editContent(e) {
    var id = $(e).data("id");
    var url = "CourseInfo/EditContent?courseid=" + id;
    //topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
    //    layer.open({
    //        type: 1,
    //        title: "编辑课程内容",
    //        skin: 'layui-layer-rim', //加上边框
    //        area: [(1000) + 'px', (650) + 'px'], //宽高
    //        content: data
    //    });
    //}, true);
    topevery.ajax({ type: 'get', url: url, dataType: 'html' }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}

function editChapterExercise(e) {
    //console.log(e);
    var id = $(e).data('id');
    var courseName = $(e).data("coursename");
    var subjectId = $(e).data('subject');
    var projectId = $(e).data('projectid');
    topevery.ajax({ type: 'get', url: 'CourseSubject/Index?courseId=' + id + "&courseName=" + courseName + "&subjectId=" + subjectId + "&projectId=" + projectId, dataType: 'html' }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}


function CourseSecurityCode(e) {
    var id = $(e).attr('name');
    topevery.ajax({ type: 'get', url: 'CourseSecurityCode/Index?courseId=' + id, dataType: 'html' }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}

//生成二维码
function createTwoDimensionalCode(event, type) {

    if (type === 0) {
        layer.prompt({ title: '输入生成二维码数量，并确认', formType: 0 }, function (pass, index) {
            layer.close(index);
            var parment = new Object();
            parment.Id = $(event).attr("name");
            parment.CourseWareCount = pass;
            parment.EmailNotes = type;
            $.StandardPost('CourseInfo/CourseDimensionalCodeInfo', parment);
        });
    } else {
        var parment = new Object();
        parment.Id = $(event).attr("name");
        parment.EmailNotes = type;
        console.log(parment);
        $.StandardPost('CourseInfo/CourseDimensionalCodeInfo', parment);
    }
    return;



    //topevery.ajax({
    //    url: "api/CourseInfo/GetTwoDimensionalCode",
    //    data: JSON.stringify(obj),
    //    async: false
    //}, function (data) {
    //    var info = data.Result;
    //    if (info.Success) {
    //        $(event).attr("layer-src", info.Message);
    //        layer.photos({
    //            photos: '#layer' + $(event).attr("id"),
    //            anim: 5 //0-6的选择，指定弹出图片动画类型，默认随机（请注意，3.0之前的版本用shift参数）
    //        });
    //    } else {
    //        layer.msg(info.Message, { time: 2000, icon: 2 });
    //    }
    //});
}

$.extend({
    StandardPost: function (url, args) {
        var form = $("<form method='post' target='_blank'></form>"),
            input;
        $(document.body).append(form);
        //document.body.appendChild(form);
        form.attr({ "action": url });
        $.each(args, function (key, value) {
            input = $("<input type='hidden'>");
            input.attr({ "name": key });
            input.val(value);
            form.append(input);
        });
        form.submit();
    }
});