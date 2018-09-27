///初始化
$(function () {
    var addorEditUrl = 'api/SubjectClass/AddOrEdit';
    topevery.BindSelect("ProjectId", "Common/ProjectIdSelect", "--请选择--", QuestionType);
    function QuestionType() {
        topevery.BindSelect("BigType", "Common/QuestionType", "--请选择--", Initialize);
    }
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/SubjectClass/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                }
            });
            $("#BigType").attr("disabled", "disabled");
        }
    }


    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        ClassName: { validators: { notEmpty: { message: '题型名称不能为空!' } } },
        ProjectId: { validators: { notEmpty: { message: '题型所属项目不能为空!' } } },
        BigType: { validators: { notEmpty: { message: '试题表现形式(大题)不能为空!' } } },
        OrderNo: { validators: { notEmpty: { message: '排序号不能为空!' } } },
    }, "", "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个试题题型！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({
                ProjectId: $("#ProjectId").val(),
                BigType: $("#BigType").val(),
            }, "sumbitForm");
            $("#Remark").val("");
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
