$(function () {
    Initialize();

    var addorEditUrl = 'api/ExamInfo/AddOrEdit';

    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        Description: {validators: { notEmpty: { message: '考试地址不能为空!'},}},
        SignUp: { validators: { notEmpty: { message: '考试报名不能为空!' } } },
        Content: { validators: { notEmpty: { message: '考试内容不能为空!' } } },
        ScoreManage: { validators: { notEmpty: { message: '成绩管理不能为空!' } } },
    }, "", "", function () {
            $(".layui-layer-close").click();
            $(".query_btn").click();
            layer.msg("保存成功!", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
    });
})

function Initialize() {
    if ($("#Id").val()) {
        topevery.ajax({
            url: "api/ExamInfo/GetOne",
            data: JSON.stringify({ id: $("#Id").val() })
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                topevery.setParmByLookForm(row);
            }
        });
    }
}