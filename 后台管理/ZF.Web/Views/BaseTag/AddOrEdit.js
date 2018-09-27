///初始化
$(function () {
    var addorEditUrl = 'api/BaseTag/AddOrEdit';
    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/BaseTag/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                console.log(data);
                if (data.Success) {
                    var row = data.Result;
                    //topevery.setParmByLookForm(row, 'sumbitForm');
                    $("#ModelCode").val(row.ModelCode);
                    $("#TagName").val(row.TagName);
                    $("#Remark").val(row.Remark);
                }
            });
        }
    }

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        ModelCode: { validators: { notEmpty: { message: '模块编码不能为空!' } } },
        TagName: { validators: { notEmpty: { message: '标签名称不能为空!' } } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个标签！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            //topevery.setParmByLookForm({ModelCode:'',TagName:'',Remark:''}, "sumbitForm");
            $("#ModelCode").val('');
            $("#TagName").val('');
            $("#Remark").val('');
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
