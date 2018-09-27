$(function () {
    GetMyStudyCard();

    ///购买课程
    function GetMyStudyCard() {
        topevery.ajaxwx({
            url: 'api/UserDiscountCard/GetMyCard',
            data: JSON.stringify({})
        }, function (data) {
            if (data.Success) {
                $('.body').html(template("studycard_html", data));
            }
        });
    }
})


template.helper("formatterState", function (value,enddate) {
    if (enddate && new Date(enddate) < new Date()) {
        return "已过期";
    } else {
        switch (value) {
            case 1:
                return "已使用";
            case 0:
                return "未使用";
            default:
                return "未使用";
        }
    }
});


template.helper('formatterDate', function (datetime) {
    if (datetime) {
        return datetime.split(' ')[0];
    }
})