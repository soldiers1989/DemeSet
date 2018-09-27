$(function () {
    ///购买课程
    function GetMyCourse() {
        topevery.ajaxwx({
            url: 'api/MyStudy/GetMyCourse',
        }, function (data) {
            if (data.Success) {
                $('#myquestion').html(template("MyCourse_html", data));
            }
        });
    }

    GetMyCourse();
})

template.helper('dataTimeView', function (datatime) {
    if (datatime !== null) {
        var data = datatime.split(" ")[0];
        return data;
    } else {
        return "";
    }
})