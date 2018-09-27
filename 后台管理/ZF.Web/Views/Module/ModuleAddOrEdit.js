///初始化
$(function () {
    var addorEditUrl = 'api/Module/AddOrEdit';

    ///初始化From表单以及验证信息
    if ($("#Id").val()) {
        topevery.ajax({
            url: "api/Module/GetOne",
            data: JSON.stringify({ id: $("#Id").val() })
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                topevery.setParmByLookForm(row);
            }
        });
    }
    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        ModuleName: { validators: { notEmpty: { message: '模块名称不能为空!' } } },
        Sort: { validators: { notEmpty: { message: '排序号不能为空!' }, } },
        Class: { validators: { notEmpty: { message: '样式不能为空!' }, } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个模块！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({}, "sumbitForm");
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
