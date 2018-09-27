///初始化
$(function () {
    var addorEditUrl = '../api/OrderSheet/SetOrderPrice';
    ///初始化From表单以及验证信息
    var id = getUrlParam("OrderNo");
    var price = getUrlParam("price");
    $("#OrderNo").val(id);
    $("#OrderAmount").val(price);
    /**
     * 绑定验证规则 
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        OrderAmount: {
            validators: {
                notEmpty: {
                    message: '金额不能为空!'
                },
                regexp: {
                    regexp: /^\d+(\.\d{1,2})?$/,
                    message: '金额必须大于0'
                }
            }
        },
    });
});


function getUrlParam(paramName) {//获取 URL参数
    var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
    return sValue ? sValue[1] : sValue;
}
