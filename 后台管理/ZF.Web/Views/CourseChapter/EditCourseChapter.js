$(function () {
    var courseId = $('#CourseId').val()
    Initialize();
    function Initialize() {
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseChapter/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    $("#ChapterEditSumbitForm input[name='CapterName']").val(row.CapterName);
                    $("#ChapterEditSumbitForm input[name='CapterCode']").val(row.CapterCode);
                    $("#ChapterEditSumbitForm input[name='OrderNo']").val(row.OrderNo);
                }
            });
        }
    }



    var editChapterUrl = 'api/CourseChapter/AddOrEdit';
    $('#ChapterEditSumbitForm').bootstrapValidatorAndSumbit(editChapterUrl, {
        CapterName: { validators: { notEmpty: { message: '章节名称不能为空' } } },
        CapterCode: { validators: { notEmpty: { message: '章节编码不能为空!' } } },
        OrderNo: { validators: { notEmpty: { message: '排序号不能为空!' } } },
    }, null, null, function () {
        layer.close(editChapterLayer);
        $('#panel-chapter').html('');
        topevery.ajax({ type: "get", url: "CourseChapter/Index?courseId=" + courseId, dataType: "html" }, function (data) {
            $(data).appendTo($('#panel-chapter'));
            $('#tab_courseChapter').tab('show');
        }, true);
    }, "ChapterEditSumbitForm");
})