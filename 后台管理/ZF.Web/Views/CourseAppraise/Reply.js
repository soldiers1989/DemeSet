$(function () {

    var url = "api/CourseAppraise/Reply";
    $('#sumbitForm').bootstrapValidatorAndSumbit(url, {
        ReplyContent: { validators: { notEmpty: { message: '回复内容不能为空!' } } },
    },'','', function () {
        $(".layui-layer-close").click();
        $('.query_btn').click();
    });
})