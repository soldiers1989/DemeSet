
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
    topevery.ajaxwx({
        url: 'api/UserDiscountCard/GetMyCard',
        data: JSON.stringify({ IfUse: IfUse, IfExpair: ifExpair, Type: 0 })
    }, function (data) {
        if (data.Success) {
            $.each(data.Result, function (i, item) {
                if (item.EndDate) {
                    item.EndDate = topevery.dataTimeView(item.EndDate);
                }
            })
            $(".body").html(template("card_html", data.Result))
        }
    });
}