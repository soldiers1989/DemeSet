$(function () {
    Initilize();
    function Initilize() {
        topevery.ajax({
            url: 'api/Register/GetUser',
            data: JSON.stringify({ Id: $('#Id').val()})
        }, function (data) {
            console.log('user',data);
            if (data.Success) {
                var obj = data.Result;
                $('.ux-user-info-bottom_name').text(obj.NickNamw);
                $('.ux-user-info-top_img >img').attr("src",obj.HeadImage);
            }
        });
    }
})