///初始化
$(function () {
    var addorEditUrl = 'api/SysSet/AddOrEdit';
    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/SysSet/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                    $("#Name,#ArguName").attr("readonly", "readonly");
                }
            });
        }
    }

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        Name: { validators: { notEmpty: { message: '名称不能为空!' } } },
        ArguName: { validators: { notEmpty: { message: '参数名称不能为空!' } } },
        ArguValue: { validators: { notEmpty: { message: '参数值不能为空!' } } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个配置！", {
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
