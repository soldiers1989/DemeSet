///初始化
$(function () {
    var addorEditUrl = 'api/User/ModifyPassWord';

    /**
     * 绑定验证规则
     */
    $(".btn-primary").click(function () {
        topevery.ajax({
            url: addorEditUrl,
            data:JSON.stringify({ OldPassWord: $("#OldPassWord").val(), PassWord: $("#PassWord").val() })
        }, function(data) {
            var message = "修改失败";
            var icon = 1;
            if (data.Success) {
                if (data.Result.Success) {
                    $(".layui-layer-close").click();
                }
                message = data.Result.Message;
                icon = data.Result.Success === true ? 1 : 2;
            }
            layer.msg(message, {
                icon: icon,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        });
    });
});
