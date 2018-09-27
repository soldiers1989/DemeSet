///初始化
$(function () {
    var addorEditUrl = 'api/SlideSetting/AddOrEdit';
    topevery.BindSelect("State", "Common/QuestionState", "", Initialize);
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/SlideSetting/GetOne",
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
        State: { validators: { notEmpty: { message: '状态不能为空!' } } },
        LinkAddress: { validators: { notEmpty: { message: '链接地址不能为空!' } } },
        OrderNo: { validators: { notEmpty: { message: '排序号不能为空!' } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if ($("input[name='IdFilehiddenFile']").val() === "") {
            topevery.msg("封面图片不能为空!", 2);
            return false;
        } else {
            if ($("input[name='IdFilehiddenFile']").val().split(',').length > 1) {
                topevery.msg("封面图片只能上传一张图片!", 2);
                return false;
            }
        }
        return true;
    }, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()' }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个课程幻灯片！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ Type: $("#Type").val() }, "sumbitForm");
            $($(".uploadImg")[0]).html("");
            $("input[name='IdFilehiddenFile']").val("");

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
