///初始化
$(function () {

    //加载修改数据
    Initialize();

    var addorEditUrl = 'api/DataType/AddOrEdit';
    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        DataTypeCode: {
            validators: {
                notEmpty: {
                    message: '分类代码不能为空!'
                },
                //regexp: {//正则验证  
                //    regexp: /^[0-9_\.]+$/,
                //    message: '分类代码格式不正确'
                //}
            }
        },
        DataTypeName: { validators: { notEmpty: { message: '字典分类名称不能为空!' } } }
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个字典分类！", {
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

function Initialize() {
    ///初始化From表单以及验证信息
    if ($("#Id").val()) {
        topevery.ajax({
            url: "api/DataType/GetOne",
            data: JSON.stringify({ id: $("#Id").val() })
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                topevery.setParmByLookForm(row);
            }
        });
    }
}
