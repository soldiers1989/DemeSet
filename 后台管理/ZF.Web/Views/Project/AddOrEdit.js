///初始化
$(function () {
    var addorEditUrl = 'api/Project/AddOrEdit';
    topevery.BindSelect("ProjectClassId", "Common/ProjectClassificationList", "--请选择--", Initialize);
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/Project/GetOne",
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
        ProjectName: { validators: { notEmpty: { message: '项目名称不能为空!' } } },
        ProjectClassId: { validators: { notEmpty: { message: '所属项目分类不能为空!' } } },
        OrderNo: { validators: { notEmpty: { message: '排序号不能为空!' } } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个项目！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ ProjectClassId: $("#ProjectClassId").val() }, "sumbitForm");
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
