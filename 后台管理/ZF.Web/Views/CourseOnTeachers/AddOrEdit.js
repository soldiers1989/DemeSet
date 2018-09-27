var Synopsis = UE.getEditor('Synopsis');
var TeachStyle = UE.getEditor('TeachStyle');
///初始化
$(function () {
    var addorEditUrl = 'api/CourseOnTeachers/AddOrEdit';

    //topevery.BindSelect("ProjectId", "Common/ProjectIdSelect", "全部");
    topevery.ajax({
        url: "Common/ProjectIdSelect2",
        data: JSON.stringify({
        }),
    }, function (data) {
        $("#ProjectId").select2({
            data: data,
            multiple: true,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
            dropdownParent: $(".box"),
            allowClear: true
        })
        $('#ProjectId').val('-1').trigger('change');
    });

    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseOnTeachers/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function(data) {
                if (data.Success) {
                    var row = data.Result;
                   
                    Synopsis.ready(function () {
                        Synopsis.setContent(row.Synopsis);
                    });
                    TeachStyle.ready(function () {
                        TeachStyle.setContent(row.TeachStyle);
                    });
                    topevery.setParmByLookForm(row);
                    $("input[name='IsFamous'][value='" + row.IsFamous + "']").attr("checked", true);
                    if (row.ProjectId) {
                        $('#ProjectId').val(row.ProjectId.split(',')).trigger("change");
                    }
                }
            });
        }
    }
  

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        TeachersName: { validators: { notEmpty: { message: '教师名称不能为空!' } } }
    }, null, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()', }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个教师！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({}, "sumbitForm");
            $($(".uploadImg")[0]).html("");
            $("input[name='IdFilehiddenFile']").val("");
            $('#ProjectId').val('-1').trigger('change');
            Synopsis.setContent("");
            TeachStyle.setContent("");
        } else {
            $(".layui-layer-close").click();
            $(".query_btn").click();
            layer.msg("修改成功!", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        }
    });

    //$('#return_btn').click(function () {
    //    topevery.ajax({ url: "CourseOnTeachers/Index", type: 'get', dataType: 'html' }, function (data) {
    //        $('.content-wrapper').html(data);
    //    });
    //});
});
