$(function () {

    GetPurchaseList();
    function GetPurchaseList() {
        topevery.ajax({
            url: 'api/MyStudy/MyQuestion',
        }, function (data) {
            if (data.Success) {
                $('#myCourse1').html(template("MyCourse1_hmtl", data));
            }
        });
    }
})