$(function () {
    getRecentStudy();
})

function getRecentStudy() {
    topevery.ajaxwx({
        url: 'api/MyCollection/GetVideoWatch',
        data: JSON.stringify({ query: '', Page: $('#PageIndex').val(), Rows: $('#Rows').val() })
    }, function (data1) {
        if (data1.Success) {
            $("#recentStudy").html(template("recentStudy_html", data1.Result));
        }
    });
}