///初始化
$(function () {
    /**
     * 绑定验证规则
     */
    var courseId = $('#CourseIdParam').val();
    var addChapterUrl = 'api/CourseChapter/AddOrEdit';
    $('#ChapterSumbitForm').bootstrapValidatorAndSumbit(addChapterUrl, {
        CapterName: { validators: { notEmpty: { message: '章节名称不能为空' } } },
        CapterCode: { validators: { notEmpty: { message: '章节编码不能为空!' } } },
        OrderNo: { validators: { notEmpty: { message: '排序号不能为空!' } } },
    }, null, null, function () {
        //$(".layui-layer-close").click();
        layer.close(addChapterLayer);
        $('#panel-chapter').html('');
        topevery.ajax({ type: "get", url: "CourseChapter/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            $(data).appendTo($('#panel-chapter'));
            $('#tab_courseChapter').tab('show');
        }, true);
    }, "ChapterSumbitForm");

    
});
