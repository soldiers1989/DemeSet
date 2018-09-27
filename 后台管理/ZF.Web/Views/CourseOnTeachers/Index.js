$(function () {
    var getPostDataUrl = "api/CourseOnTeachers/GetList";
    var addUrl = "CourseOnTeachers/AddOrEdit";
    var deleteUrl = "api/CourseOnTeachers/Delete";
    var grid = $("#tblData");
    topevery.ajax({
        url: "Common/SelectTeacherList",
        data: JSON.stringify({}),
        async: false
    }, function (data) {
        $("#QueryTeachersName").select2({
            data: data,
            placeholder: { id: '-1', text: '请选择' },
            dropdownParent: $(".drop_list"),
            allowClear: true
        })
        $('#QueryTeachersName').val('-1').trigger('change'); 
    });

    topevery.ajax({
        url: "Common/ProjectIdSelect2",
        data: JSON.stringify({}),
        async:false
    }, function (data) {
        console.log(data);
        $("#QueryProjectId").select2({
            data: data,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
            dropdownParent: $(".drop_list"),
            allowClear: true
        })
        $('#QueryProjectId').val('-1').trigger('change');
    });

    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '教师名称', '擅长科目','是否在名师频道','所属项目','项目Id', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'TeachersName', index: 'TeachersName', width: 60, align: "center" },
            { name: 'TheLabel', index: 'TheLabel', width: 100, align: "center" },
            {
                name: 'IsFamous', index: 'IsFamous', width: 40, align: "center", formatter: function (data) {
                    return data == 0 ? "否" : "是";
                }
            },
            { name: 'ProjectName', index: 'ProjectName', width: 80, align: "center" },
            { name: 'ProjectId', index: 'ProjectId', width: 30, align: "center",hidden:true },
            { name: '', width: 200 }
            //{ name: 'Synopsis', index: 'Synopsis', width: 200, align: "center" },
            //{ name: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
        ],
        postData: JSON.stringify({ TeachersName: '',ProjectId:''})
    });
    $(".add_btn").bindAddBtn(addUrl, 800, 600);
    //$('.add_btn').click(function () {
    //    topevery.ajax({ type: 'get', url: 'CourseOnTeachers/AddOrEdit', dataType: 'html' }, function (data) {
    //        $('.content-wrapper').html(data);
    //    });
    //});
    $(".edit_btn").bindEditBtn(addUrl, grid, 800, 600);
    $(".del_btn").bindDelBtnDx(deleteUrl, grid, beforeDelete);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: { TeachersName: $('#QueryTeachersName').select2("val") == null || $('#QueryTeachersName').select2("val") == "" ? "" : $('#QueryTeachersName').select2("data")[0].text, ProjectId: $("#QueryProjectId").val() }
            }).trigger("reloadGrid");
    });

    function beforeDelete(obj) {
        var flag = false;
        topevery.ajax({
            url: 'api/CourseInfo/ExistTeacherInCourse',
            data: JSON.stringify({ TeacherId: obj.Id }),
            async: false
        }, function (data) {
            if (data.Result) {
                layer.msg("该讲师已维护到课程信息中,不能删除!", { time: 2000 });
                flag = false;
            } else {
                flag = true;
            }
        });
        return flag;
    }


   
    
});