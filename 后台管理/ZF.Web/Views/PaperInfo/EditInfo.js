///初始化
$(function () {
    var addorEditUrl = 'api/PaperInfo/EditInfo';
    ///初始化From表单以及验证信息
    if ($("#Id").val()) {
        topevery.ajax({
            url: "api/PaperInfo/GetInfo",
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
        PaperName: { validators: { notEmpty: { message: '试卷名称不能为空!' } } },
        TestTime: { validators: { notEmpty: { message: '考试时长不能为空!' } } },
    });
});
