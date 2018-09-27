$(function () {
    var CourseInfo = {
        formatterState: function (state) {
            return state == 1 ? "已上架" : "未上架"
        },
        formatterPrice: function (price) {
            return '' + price + '￥';
        },
    }
    //获取不包括套餐课程中已有的课程列表
    var getPostDataUrl = "api/CourseInfo/GetListExceptInPackCourse";
    var grid = $("#subCourseData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '科目编码', '科目名称', '课程名称', '有效期(天)', '主讲教师', '课程时长(分钟)', '课件数'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'SubjectId', width: 100, align: "center", hidden: true },
            { name: 'SubjectName', width: 80, align: "center" },
            { name: 'CourseName', width: 150, align: "center" },
            //{ name: 'Price', width: 40, align: "center", formatter: CourseInfo.formatterPrice },
            //{ name: 'FavourablePrice', width: 40, align: "center", formatter: CourseInfo.formatterPrice },
            {
                name: 'ValidityPeriod', width: 50, align: "center", formatter: function (value) {
                    if (value === 0) {
                        return "永久有效";
                    }
                    return value;
                }
            },
            //{ name: 'State', width: 50, align: "center", formatter: CourseInfo.formatterState },
            { name: 'TeachersName', width: 50, align: "center" },
            { name: 'CourseLongTime', width: 50, align: "center" },
            { name: 'CourseWareCount', width: 50, align: "center" },
        ],
        postData: topevery.form2Json("subCourseSelectFrom"),
        pager: '#subCoursePager',
        height: '490'
    });


    $(".query_btn1").on("click", function () {
        var postData = { PackCourseId: $('#packCourseId').val(), ProjectClassId: $('#QProjectClassId').val(), ProjectId: $('#QProjectId').val(), SubjectId: $('#QSubjectId').val(), CourseName: $('#QCourseName').val(), TeacherId: $('#QTeacherId').val() };
        $("#subCourseData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: postData
        }).trigger("reloadGrid");
    });

    $('.add_btnSubCourse').on('click', function () {
        var ids = $('#subCourseData').jqGrid('getGridParam', 'selarrrow');
        if (ids == undefined || ids.length == 0) {
            layer.alert("请选择课程");
            return;
        }
        var arr = [];
        for (var i = 0; i < ids.length; i++) {
            var rowId = ids[i];
            var rowData = $('#subCourseData').jqGrid('getRowData', rowId);
            arr.push(rowData.Id);
        }
        var packCourseId = $('#packCourseId').val();
        layer.confirm('确认添加?', { title: "确认" }, function () {
            topevery.ajax({
                url: 'api/CourseSuitDetail/AddSubCourse?PackCourseId=' + packCourseId + "&SubCourseIds=" + arr.join(',')
            }, function (data) {
                if (data.Result.Success) {

                    layer.msg(data.Result.Message, {
                        icon: 1,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 3000, //10秒后自动关闭
                        anim: 5
                    });
                    $(".layui-layer-close").click();//关闭窗口


                    //添加成功,加载数据
                    $('#packCourseData').jqGrid('setGridParam', {
                        url: 'api/CoursePackcourseInfo/GetList', page: 1, postData: topevery.form2Json("selectFrom")
                    }).trigger("reloadGrid");
                }
            });
        })
    })
    //topevery.BindSelect("QTeacherId", "Common/TeacherList", "--全部--");
    topevery.ajax({
        url: "Common/SelectTeacherList",
        data: JSON.stringify({
        })
    }, function (data) {
        $("#QTeacherId").select2({
            data: data,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
            dropdownParent: $(".subcourse"),
            allowClear: true
        })
        $('#QTeacherId').val('-1').trigger('change');
    });

    topevery.BindSelect("QProjectClassId", "Common/ProjectClassificationList", "--请选择--");
    topevery.BindSelect("QProjectId", "Common/ProjectList", "--请选择--");
    topevery.BindSelect("QSubjectId", "Common/SubjectList", "--全部--");

    $('#QProjectClassId').change(function () {
        var projectClassId = $(this).children('option:selected').val();
        topevery.BindSelect("QProjectId", "Common/ProjectList?ProjectClassId=" + projectClassId, "--请选择--");
        topevery.BindSelect("QSubjectId", "Common/SubjectList?projectId=" + $('#QProjectId').children("option:selected").val(), "--全部--");
    });
    $('#QProjectId').change(function () {
        var projectId = $(this).children("option:selected").val();
        topevery.BindSelect("QSubjectId", "Common/SubjectList?projectId="+projectId, "--全部--");
    });
    

});
