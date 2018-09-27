///初始化
$(function () {
    var addorEditUrl = 'api/PaperGroup/AddOrEdit';
    if ($("#sId").val()) {
        topevery.BindSelect("SubjectId", "Common/SubjectList", "--请选择--", SetVal);
    } else {
        topevery.BindSelect("SubjectId", "Common/SubjectList", "--请选择--", Initialize);
        function Initialize() {
            ///初始化From表单以及验证信息
            if ($("#Id").val()) {
                topevery.ajax({
                    url: "api/PaperGroup/GetOne",
                    data: JSON.stringify({ id: $("#Id").val() })
                }, function (data) {
                    if (data.Success) {
                        var row = data.Result;
                        topevery.setParmByLookForm(row);
                    }
                });
            }
        }
    }
    function SetVal() {
        //$("#SubjectId").attr("disabled", "disabled");
        $("#SubjectId").val($("#sId").val());
    }
    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        PaperGroupName: { validators: { notEmpty: { message: '试卷组名称不能为空!' } } },
        SubjectId: { validators: { notEmpty: { message: '所属科目不能为空!' } } },
    });
});
