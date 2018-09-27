///初始化
$(function () {
    var addorEditUrl = 'api/Subject/AddOrEdit';
    topevery.BindSelect("ProjectId", "Common/ProjectList", "--请选择--", Initialize);
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/Subject/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                }
            });
        }
    }

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        SubjectName: { validators: { notEmpty: { message: '科目名称不能为空!' } } },
        ProjectId: { validators: { notEmpty: { message: '所属项目不能为空!' } } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个科目！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ ProjectId: $("#ProjectId").val() }, "sumbitForm");
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
});
