$(function () {

    var courseId = $('#courseid').val();
    topevery.ajax({ type: "get", url: "CourseChapter/Index?courseId=" + courseId, dataType: "html" }, function (data) {
        $(data).appendTo($('#panel-chapter'));
        $('#tab_courseChapter').tab('show');
    }, true);

    //点击课程资源标签,加载课程资源列表
    $('#tab_courseResource').on('click', function () {
        $('#panel-resource').html('');
        topevery.ajax({ type: "get", url: "CourseResource/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            $(data).appendTo($('#panel-resource'));
            $('#tab_courseResource').tab('show');
        }, true);
    })

    //点击课程视频标签，加载课程视频
    $('#tab_courseVideo').on('click', function () {
        $('#panel-video').html('');
        topevery.ajax({ type: "get", url: "CourseVideo/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            //加载章节树
            $(data).appendTo($('#panel-video'));
            $('#tab_courseVideo').tab('show');
        }, true);
    })

    //课程试题
    $('#tab_coursePaper').on('click', function () {
        $('#panel-paper').html('');
        topevery.ajax({
            type: 'get',
            url: 'CoursePaper/Index?courseId=' + courseId,
            dataType: 'html'
        }, function (data) {
            $(data).appendTo($('#panel-paper'));
            $('#tab_paper').tab('show');
        })
    })
})
$('#return_btn').on('click', function () {
    topevery.ajax({ url: "CourseInfo/CourseContent", type: 'get', dataType: 'html' }, function (data) {
        $('.content-wrapper').html(data);
    });
});