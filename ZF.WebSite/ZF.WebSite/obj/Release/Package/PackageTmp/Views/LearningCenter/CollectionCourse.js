$(function () {
    
    GetCollectedCourse();



    function GetCollectedCourse() {
        topevery.ajax({
            //url: 'api/MyStudy/GetCollectedCourse',
            url: 'api/MyCollection/GetList',
            data: JSON.stringify({Page:1,Rows:3})
        }, function (data) {
            if (data.Success) {
                console.log(data);
                $('#collectedCourse').html(template("collectedCourse_html", data.Result));
            }
        });
    }
})


template.helper('formatterDate', function (datetime) {
    if (datetime) {
        return datetime.split(' ')[0];
    }
})
