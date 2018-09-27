$(function () {
    ///初始化From表单以及验证信息
    Initial();

    function Initial() {
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseResource/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    //topevery.setParmByLookForm(row);
                    var objs = $("#resourceSubmitForm input,select,textarea");
                    for (var i = 0; i < objs.length; i++) {
                        var o = objs[i];
                        try {
                            var name = o.name;
                            var tagName = o.tagName.toLocaleLowerCase();
                            var type = o.type; o.tagName.toLocaleLowerCase();
                            var dysetMes = "";
                            var jqObject = name.substring(0, 1) + name.substring(1, name.length);
                            row[jqObject] = row[jqObject] == null ? "" : row[jqObject];
                            if (tagName === "input" || tagName === "select") {
                                if (type === "radio") {
                                    dysetMes = "$('input[name='" + name + "'][value='" + row[jqObject] + "']').attr('checked', true)";
                                } else {
                                    dysetMes = "$('#" + name + "').val('" + row[jqObject] + "');";
                                }
                            }
                            if (tagName === "textarea") {
                                dysetMes = "$('#" + name + "').text('" + row[jqObject] + "');";
                            }
                            try {
                                eval(dysetMes);
                            } catch (e) {

                            }
                        } catch (e) {

                        }
                    }
                }
            });
        }
    }
    


    /**
     * 绑定验证规则
     */
    var addResourceUrl = 'api/CourseResource/AddOrEdit';
    $('#resourceSubmitForm').bootstrapValidatorAndSumbit(addResourceUrl, {
        ResourceName: { validators: { notEmpty: { message: '课程资源名称不能为空' } } },
    },function () {
        $('.btn-primary').removeAttr('disabled');
        if ($("input[name='IdFilehiddenFile']").val() === "") {
            topevery.msg("课程资源不能为空!", 2);
            return false;
        }
        return true;
    }, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()' }, function () {
        //$('.layui-layer-close').click();
        //layer.close(addResourceLayer);
        //layer.close(editResourceLayer);
        if (!$('#Id').val()) {
            layer.msg("添加成功,可以继续添加下一个资源！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $('#tab_courseResource').click();
            $('#ResourceName').val('');
            $($(".uploadImg")[0]).html("");
            $("input[name=\'IdFilehiddenFile\']").val("");
        } else {
            layer.close(editResourceLayer);
            layer.msg("修改成功!", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        }
    }, "resourceSubmitForm");
});
