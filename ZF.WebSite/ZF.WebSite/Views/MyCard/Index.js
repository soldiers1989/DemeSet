

GetCard(false);

///根据状态获取学习卡 1：未使用 2:已使用 3:已过期
function GetCard(state) {
    var contentHtml = "";
    var IfUse = 0//是否使用
    var ifExpair = 0;//是否过期
    var url = "";
    if (state == "1") {
        IfUse = 0;
    } else if (state == "2") {
        IfUse = 1;
    } else if (state == "3") {
        IfUse = 0;
        ifExpair = 1;
    }
    topevery.ajax({
        url: 'api/UserDiscountCard/GetMyCard',
        data: JSON.stringify({ IfUse: IfUse, IfExpair: ifExpair, Type: 0 })
    }, function (data) {
        if (data.Success) {
            $.each(data.Result, function (i, item) {
                if (item.EndDate) {
                    item.EndDate = topevery.dataTimeView(item.EndDate);
                }
            })

            $("#bindCardInfo").html(template("card_html", data.Result))
        }
    });
}

$(document).on("click", ".add", function () {

})

function AddCard() {
    topevery.ajax({
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
}

//获取 URL参数
function GetUrlParam(paramName) {
    var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
    return sValue ? sValue[1] : sValue;
}

function LoadCard() {
    layer.open({
        type: 1,
        title: "领取学习卡",
        content: $("#add").html(),
        btn: ['确定', '取消'],
        btnAlign: "c",
        area: ['500px', '200px'],
        yes: function () {
            var cardCode = $.trim($("#txtNumber").val());
            if (!cardCode) {
                $(".error_msg").html("学习卡号不能为空")
            } else {
                topevery.ajax({
                    url: "api/UserDiscountCard/IfExistAndExpair?CardId=" + cardCode,
                }, function (data) {
                    if (data.Result && data.Result.Success) {
                        AddCard();
                    } else {
                        $(".error_msg").html(data.Result.Message);
                        $("#txtNumber").focus();
                    }
                });
            }

        },
        end: function () {
            GetCard(false);
        }
    });
    $("#txtNumber").focus()
}