$(function () {
    getExerciseList();
})

function getExerciseList() {
    topevery.ajaxwx({
        url: 'api/ChapterExerciseRecord/GetExerciseRecords',
        data: JSON.stringify({ query: '', page: $('#PageIndex').val(), rows: $('#Rows').val() })
    }, function (data1) {
        if (data1.Success) {
            $("#PracticeRecords").html(template("PracticeRecords_html", data1.Result));
        }
    });
}