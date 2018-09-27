///初始化
$(function () {
    var addorEditUrl = 'api/ExpressCompany/AddOrEdit';
    ///初始化From表单以及验证信息
    debugger
    if ($("#Id").val()) {
        topevery.ajax({
            url: "api/ExpressCompany/GetInfo",
            data: JSON.stringify({ Id: $("#Id").val() })
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
        Name: { validators: { notEmpty: { message: '名称不能为空!' } } },
        Companyurl: { validators: { notEmpty: { message: '网址不能为空!' } } },
    });
});
