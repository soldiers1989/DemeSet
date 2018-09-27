
function LoadCard() {
    var cardCode = $.trim($("#txtNumber").val());
    if (!cardCode) {
        $(".error_msg").html("学习卡号不能为空")
    } else {
        topevery.ajaxwx({
            url: "api/UserDiscountCard/IfExistAndExpair?CardId=" + cardCode,
        }, function (data) {
            if (data.Result && data.Result.Success) {
                topevery.ajaxwx({
                    url: "api/UserDiscountCard/AddCard",
                    data: JSON.stringify({ CardId: $.trim($('#txtNumber').val()) })
                }, function (data) {
                    if (data.Success && data.Result) {
                        //显示添加的学习卡
                        $("#txtNumber").val("");
                        $(".error_msg").html(data.Result.Message);
                    } else {
                        $(".error_msg").html(data.Result.Message);
                    }
                })
            } else {
                $(".error_msg").html(data.Result.Message);
                $("#txtNumber").focus();
            }
        });
    }
}