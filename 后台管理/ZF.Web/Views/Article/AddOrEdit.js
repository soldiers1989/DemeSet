var Content = UE.getEditor('ArticleContent');
///初始化
$(function () {
    var addorEditUrl = 'api/Article/AddOrEdit';
    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/Article/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                    Content.ready(function () {
                        Content.setContent(row.Content);
                    });
                }
            });
        }
    }




    $(".return").bindAddSkip("Article/Index");
    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        //BigClassId: { validators: { notEmpty: { message: '所属类别不能为空!' } } },
        //ClassId: { validators: { notEmpty: { message: '所属小类不能为空!' } } },
        //Title: { validators: { notEmpty: { message: '标题不能为空!' } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if (!Content.getContent()) {
            topevery.msg("内容不能为空!", 2);
            return false;
        }
        //if ($("input[name='IdFilehiddenFile']").val() === "") {
        //    topevery.msg("封面图片不能为空!", 2);
        //    return false;
        //} else {
        //    if ($("input[name='IdFilehiddenFile']").val().split(',').length > 1) {
        //        topevery.msg("封面图片只能上传一张图片!", 2);
        //        return false;
        //    }
        //}
        return true;
    },
    {}, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({}, "sumbitForm");
            Content.ready(function () {
                Content.setContent("");
            });
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
