var Content = UE.getEditor('Content');
///初始化
$(function () {
    var addorEditUrl = 'api/AfficheHelp/AddOrEdit';
    topevery.BindSelect("BigClassId", "Common/GetBigClassIdList?Code=bzgl", "--请选择--", Initialize);
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/AfficheHelp/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                    topevery.BindSelect("ClassId", "Common/DataDictionary?dataTypeId=" + row.BigClassId, "--请选择--", function () {
                        $("#ClassId").val(row.ClassId);
                    });
                    Content.ready(function () {
                        Content.setContent(row.Content);
                    });
                }
            });
        }
    }
    $("#BigClassId").change(function () {
        topevery.BindSelect("ClassId", "Common/DataDictionary?dataTypeId=" + $("#BigClassId").val(), "--请选择--");
    });
    $(".return").bindAddSkip("Help/Index");
    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        BigClassId: { validators: { notEmpty: { message: '所属类别不能为空!' } } },
        ClassId: { validators: { notEmpty: { message: '所属小类不能为空!' } } },
        Title: { validators: { notEmpty: { message: '标题不能为空!' } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if (!Content.getContent()) {
            topevery.msg("内容不能为空!", 2);
            return false;
        }
        return true;
    },
    {
        Content: 'Content.getContent()',
    }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个帮助！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ BigClassId: $("#BigClassId").val(), ClassId: $("#ClassId").val(), }, "sumbitForm");
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
