///初始化
$(function () {
    var addorEditUrl = 'api/Menu/AddOrEdit';
    topevery.BindSelect("ModuleId", "Common/MenuModuleList", "--请选择--", Initialize);
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/Menu/GetOne",
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
        ModuleId: { validators: { notEmpty: { message: '所属模块不能为空!' } } },
        MenuName: { validators: { notEmpty: { message: '菜单名称不能为空!' } } },
        Url: { validators: { notEmpty: { message: '路径不能为空!' } } },
        Sort: { validators: { notEmpty: { message: '排序号不能为空!' }, } },
        Class: { validators: { notEmpty: { message: '样式不能为空!' }, } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个菜单！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ ModuleId: $("#ModuleId").val() }, "sumbitForm");
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
