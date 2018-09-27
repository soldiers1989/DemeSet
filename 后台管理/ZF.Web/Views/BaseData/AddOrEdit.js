/////初始化
//$(function () {
//    var addorEditUrl = 'api/BaseData/AddOrEdit';
//    topevery.BindSelect("DataTypeId", "Common/DataTypeList", "--请选择--", Initialize);
//    function Initialize() {
//        ///初始化From表单以及验证信息
//        if ($("#Id").val()) {
//            topevery.ajax({
//                url: "api/BaseData/GetOne",
//                data: JSON.stringify({ id: $("#Id").val() })
//            }, function (data) {
//                var row = data.Result;
//                if (data.Success) {
//                    topevery.setParmByLookForm(row);
//                }
//            });
//        }
//    }

//    /**
//     * 绑定验证规则
//     */
//    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
//        Name: { validators: { notEmpty: { message: '数据名称不能为空!' } } },
//        Code: { validators: { notEmpty: { message: '数据类型编码!' } } },
//        DataTypeId: { validators: { notEmpty: { message: '请选择字典分类!' } } },
//    }, "", "", function () {
//        if (!$("#Id").val()) {
//            layer.msg("添加成功,可以继续添加下一个数据字典！", {
//                icon: 1,
//                title: false, //不显示标题
//                offset: 'auto',
//                time: 3000, //10秒后自动关闭
//                anim: 5
//            });
//            $(".query_btn").click();
//            topevery.setParmByLookForm({ DataTypeId: $("#DataTypeId").val() }, "sumbitForm");
//        } else {
//            $(".layui-layer-close").click();
//            $(".query_btn").click();
//            layer.msg("修改成功!", {
//                icon: 1,
//                title: false, //不显示标题
//                offset: 'auto',
//                time: 3000, //10秒后自动关闭
//                anim: 5
//            });
//        }
//    });
//});


$(function () {
    var app = new Vue({
        data: {
            addorEditUrl: "api/BaseData/AddOrEdit",
        },
        methods: {
            init: function () {
                topevery.BindSelect("DataTypeId", "Common/DataTypeList", "--请选择--");
                if ($("#Id").val()) {
                    topevery.ajax({
                        url: "api/BaseData/GetOne",
                        data: JSON.stringify({ id: $("#Id").val() })
                    }, function (data) {
                        var row = data.Result;
                        if (data.Success) {
                            topevery.setParmByLookForm(row);
                        }
                    });
                };
                /**
                * 绑定验证规则
                */
                $('#sumbitForm').bootstrapValidatorAndSumbit(this.addorEditUrl, {
                    Name: { validators: { notEmpty: { message: '数据名称不能为空!' } } },
                    Code: { validators: { notEmpty: { message: '数据类型编码!' } } },
                    DataTypeId: { validators: { notEmpty: { message: '请选择字典分类!' } } },
                }, "", "", function () {
                    if (!$("#Id").val()) {
                        layer.msg("添加成功,可以继续添加下一个数据字典！", {
                            icon: 1,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                        $(".query_btn").click();
                        topevery.setParmByLookForm({ DataTypeId: $("#DataTypeId").val() }, "sumbitForm");
                    } else {
                        $(".layui-layer-close").click();
                        $(".query_btn").click();
                        layer.msg("修改成功!", {
                            icon: 1,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                    }
                });
            }
        }
    });
    app.init();
})