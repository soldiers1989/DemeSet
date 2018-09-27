///初始化
$(function () {
    var addorEditUrl = 'api/PaperPaperParam/AddOrEdit';
    if ($("#sId").val()) {
        debugger
        var obj = new Object();
        obj.SubjectId = $("#sId").val();
        topevery.ajax({
            url: "api/Structure/GetList",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                $("#QueryNewStuctureId").empty();
                var sub = " <option value=''>--请选择--</option> ";
                $.each(info.Rows, function (i, item) {
                    sub += "<option value='" + item.Id + "'>" + item.StuctureName + "</option>"
                })
                $("#QueryNewStuctureId").append(sub);
            }
        });
    } else {
        //绑定试卷结构
        topevery.BindSelect("QueryNewStuctureId", "Common/StructureList", "--全部--", Initialize);
        //Initialize();
        function Initialize() {
            ///初始化From表单以及验证信息
            if ($("#Id").val()) {
                topevery.ajax({
                    url: "api/PaperPaperParam/GetOne",
                    data: JSON.stringify({ id: $("#Id").val() })
                }, function (data) {
                    if (data.Success) {
                        var row = data.Result;
                        $("#ProjectClassName").val(row.ParamName);
                        $("#QueryNewStuctureId").val(row.StuctureId);
                    }
                });
            }
        }
    }
    $('#sumbitFormTwo').bootstrapValidator({
        message: '输入的值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            ProjectClassName: { validators: { notEmpty: { message: '参数名称名称不能为空!' } } },
            AddUserId: { validators: { notEmpty: { message: '试卷结构不能为空!' } } },
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var $form = $(e.target);
        var bv = $form.data('bootstrapValidator');
        if (bv.isValid()) {
            var parament = new Object();
            parament.Id = $("#Id").val();
            parament.StuctureId = $("#QueryNewStuctureId").val();
            parament.ParamName = $("#ProjectClassName").val();
            topevery.ajax({
                url: addorEditUrl,
                data: JSON.stringify(parament)
            }, function (data) {
                var message = "新增失败";
                var icon = 1;
                if (data.Success) {
                    message = data.Result.Message;
                    icon = data.Result.Success === true ? 1 : 2;
                    if (data.Result.Success) {
                        if (!$("#Id").val()) {
                            layer.msg("新增成功！", {
                                icon: 1,
                                title: false, //不显示标题
                                offset: 'auto',
                                time: 3000, //10秒后自动关闭
                                anim: 5
                            });
                            $($("#tblData")).trigger("reloadGrid");
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
                    } else {
                        layer.msg(message, {
                            icon: icon,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                    }
                }
            }
            );
        }

    });
});
