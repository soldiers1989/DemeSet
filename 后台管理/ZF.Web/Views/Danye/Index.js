var Content = UE.getEditor('Content');

$(function () {
    var addorEditUrl = 'api/Danye/AddOrEdit';
    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Code").val()) {
            topevery.ajax({
                url: "api/Danye/GetOne",
                data: JSON.stringify({ Code: $("#Code").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                    Content.ready(function () {
                        Content.setContent(row.Content);
                    });
                }
            });
        } 
    }

    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
    },"","",function() {
        layer.msg("更新成功!", {
            icon: 1,
            title: false, //不显示标题
            offset: 'auto',
            time: 3000, //10秒后自动关闭
            anim: 5
        });
    });
})