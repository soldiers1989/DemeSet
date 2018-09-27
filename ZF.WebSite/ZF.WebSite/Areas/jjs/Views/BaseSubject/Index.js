$(function () {
    getProjectList();
})

function getProjectList() {
    topevery.ajaxwx({
        url: "api/CourseInfo/GetProject",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $('#projectAll').html(template('ProjectAll_html', data));
        }
    });
}