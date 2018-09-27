///初始化
$(function () {
    var addorEditUrl = 'api/User/AddOrEdit';

    ///初始化From表单以及验证信息
    if ($("#Id").val()) {
        $("#LoginName").attr("readonly", "readonly");
        topevery.ajax({
            url: "api/User/GetOne",
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
        LoginName: { validators: { notEmpty: { message: '登录名不能为空!' } } },
        UserName: { validators: { notEmpty: { message: '用户名不能为空!' } } },
        Phone: {
            validators: {
                regexp: { regexp: /^[1][3,4,5,6,7,8][0-9]{9}$/, message: '请输入有效的手机号码!' }
            }
        },
    }, null, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()' }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个管理员！", {
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
