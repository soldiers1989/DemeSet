var CourseContent = UE.getEditor('CourseContent');
var CourseIntroduce = UE.getEditor('CourseIntroduce');
var Curriculum = UE.getEditor('Curriculum');
var WhatTeach = UE.getEditor('WhatTeach');
var selectCourseTag = [];
var selectedCourseTag = [];
///初始化
$(function () {
    var addorEditUrl = 'api/CourseFaceToFace/AddOrEdit';

    topevery.BindSelect("CourseAttribute", "Common/DataDictionary?dataTypeId=beb0364f-8958-49f7-916f-1f5019354053", '--请选择--', Initialize);

    function Initialize() {
        topevery.ajax({
            url: "Common/SelectTeacherList",
            data: JSON.stringify({
            }),
            async: false
        }, function (data) {
            $("#TeacherId").select2({
                data: data,
                multiple: true,
                placeholder: {
                    id: '-1',
                    text: '请选择'
                },
                dropdownParent: $(".box"),
                allowClear: true
            })
            $('#TeacherId').val('-1').trigger('change');
        });
        topevery.ajax({
            url: "Common/SelectProjectList",
            data: JSON.stringify({
            }),
            async: false
        }, function (data) {
            $("#ClassId").select2({
                data: data,
                multiple: true,
                placeholder: {
                    id: '-1',
                    text: '请选择'
                },
                dropdownParent: $(".box"),
                allowClear: true
            })
            $('#ClassId').val('-1').trigger('change');
        });
        topevery.ajax({
            url: "Common/SelectSubjectList",
            data: JSON.stringify({
            }),
            async: false
        }, function (data) {
            $("#SubjectId").select2({
                data: data,
                multiple: true,
                placeholder: {
                    id: '-1',
                    text: '请选择'
                },
                dropdownParent: $(".box"),
                allowClear: true
            })
            $('#SubjectId').val('-1').trigger('change');
        });
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseFaceToFace/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);
                    try {
                        CourseContent.ready(function () {
                            CourseContent.setContent(row.CourseContent);
                        });
                    } catch (e) {

                    }
                    try {
                        CourseIntroduce.ready(function () {
                            CourseIntroduce.setContent(row.CourseIntroduce);
                        });
                    } catch (e) {

                    }
                    try {
                        Curriculum.ready(function () {
                            Curriculum.setContent(row.Curriculum);
                        });
                    } catch (e) {

                    }
                    try {
                        WhatTeach.ready(function () {
                            WhatTeach.setContent(row.WhatTeach);
                        });
                    } catch (e) {

                    }
                    if (row.TeacherId != null || row.TeacherId != undefined) {
                        $('#TeacherId').val(row.TeacherId.split(',')).trigger("change");
                    }
                    if (row.ClassId != null || row.ClassId != undefined) {
                        $('#ClassId').val(row.ClassId.split(',')).trigger("change");
                    }
                    if (row.SubjectId != null || row.SubjectId != undefined) {
                        $('#SubjectId').val(row.SubjectId.split(',')).trigger("change");
                    }
                    $("#ClassTimeStart").val(topevery.dataTimeView(row.ClassTimeStart));
                    $("#ClassTimeEnd").val(topevery.dataTimeView(row.ClassTimeEnd));
                    if (row.CourseTag) {
                        selectedCourseTag = selectedCourseTag.concat(row.CourseTag.split(','));
                    }
                    bindTag();
                }
            });
        } else {
            bindTag();
        }
    }

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        ClassName: { validators: { notEmpty: { message: '课程名称不能为空!' } } },
        Price: { validators: { notEmpty: { message: '原价不能为空!' } } },
        Address: { validators: { notEmpty: { message: '上课地点不能为空!' } } },
        Number: { validators: { notEmpty: { message: '班级人数不能为空!' } } },
    }, function () {
        $('.btn-primary').removeAttr('disabled');
        var price = $('#Price').val();
        var favourableprice = $('#FavourablePrice').val();
        if (parseFloat(favourableprice) > parseFloat(price)) {
            topevery.msg("课程优惠价应小于原价", 2);
            $(this).focus();
            return false;
        }
        if (!$("#ClassTimeStart").val()) {
            topevery.msg("上课时间不能为空!", 2);
            $(this).focus();
            return false;
        }
        if (!$("#ClassTimeEnd").val()) {
            topevery.msg("上课时间不能为空!", 2);
            $(this).focus();
            return false;
        }
        if (!favourableprice) {
            topevery.msg("最终价格不能为空!", 2);
            $(this).focus();
            return false;
        }
        if (!CourseIntroduce.getContent()) {
            topevery.msg("课程介绍不能为空!", 2);
            $(this).focus();
            return false;
        }
        if (!$('#ClassId').val()) {
            topevery.msg("服务分类不能为空!", 2);
            $(this).focus();
            return false;
        }
        if (!$('#SubjectId').val()) {
            topevery.msg("科目分类不能为空!", 2);
            $(this).focus();
            return false;
        }
        return true;
    }, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()' }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个课程！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({ IsTop: $("#IsTop").val(), IsRecommend: $("#IsRecommend").val(), Discount: $("#Discount").val() }, "sumbitForm");
            $('#TeacherId').val('-1').trigger('change');
            $('#SubjectId').val($('#subjectId_copy').val());
            $($(".uploadImg")[0]).html("");
            $("input[name='IdFilehiddenFile']").val("");
            CourseContent.setContent('');
            CourseIntroduce.setContent('');
            Curriculum.setContent('');
            WhatTeach.setContent('');
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


    $('#Price,#FavourablePrice').on("keydown keyup", function () {
        if (isFloat(this.value)) {
            if (this.value.split('.')[1].length > 2) {
                console.log(this.value.split('.')[0], this.value.split('.')[1].substr(0, 2))
                $(this).val(this.value.split('.')[0] + "." + this.value.split('.')[1].substr(0, 2));
            }
        }
    });

    $('#return,#return1').click(function () {
        topevery.ajax({ url: "FaceToFace/Index", type: 'get', dataType: 'html' }, function (data) {
            $('.content-wrapper').html(data);
        });
    });


    function isFloat(oNum)//判断是否为浮点数的函数
    {
        if (!oNum)
            return false;
        var strP = /^\d+(\.\d+)?$/;
        if (!strP.test(oNum))
            return false;
        try {
            if (parseFloat(oNum) != oNum)
                return false;
        } catch (ex) {
            return false;
        }
        return true;
    }

    $("#Price").bind("change", function () {
        $("#FavourablePrice").val($("#Price").val());
    });
});

function bindTag() {
    topevery.ajax({
        url: 'api/BaseTag/GetSelectList',
        data: JSON.stringify({ ModelCode: 'msk' }),
    }, function (data) {
        if (data.Success) {
            $('.tag-wrap').html('');
            $("#tagViw").html("");
            var content = "";
            var arr = data.Result;
            for (var i = 0; i < arr.length; i++) {
                var index = $.inArray(arr[i].Id, selectedCourseTag);
                if (index < 0) {
                    content += "<span class=\"tag used\">"
                        + "<span class=\"text\" onclick=\"bindSelectTag(this)\" id=\"" + arr[i].Id + "\">" + arr[i].TagName + "</span>"
                        + "<span class=\"delete\"  onclick=\"DeleteTag(this)\" title=\"删除\">x</span>"
                        + "</span>";
                    selectCourseTag.push(arr[i].Id);
                } else {
                    $("#tagViw").append($("<span class=\"tag\"><span  class=\"text\" id=\"" + arr[i].Id + "\">" + arr[i].TagName + "</span><span  onclick=\"bindDeleteTag(this)\" class=\"delete\"  title=\"删除\">x</span></span>"));
                }
            }
            $('.tag-wrap').html(content);
        }
    });
};


function DeleteTag(thiss) {
    //数据库删除标签
    var id = $(thiss).prev().attr('id');
    topevery.ajax({
        url: 'api/BaseTag/Delete',
        data: JSON.stringify({ Ids: id }),
    }, function (data) {
        if (data.Success) {
            bindTag();
        }
    });
}


function bindDeleteTag(thiss) {
    //删除标签
    var parent = $(thiss).parent();
    var grandparent = $(parent).parent();
    var id = $(thiss).prev().attr('id');
    removeItem(selectedCourseTag, id);
    selectCourseTag.push(id);
    $(".tag-wrap,.tag-layer").attr("style", "display:block;");
    $(".tag-wrap").append($(parent).addClass("used"));
    $('#CourseTag').val(selectedCourseTag.unique().join(',').trimRight(','));
    $(grandparent).find($(parent)).remove();
    bindSelectTag();
}


function bindSelectTag(thiss) {
    //从已有标签选择
    var parent = $(thiss).parent();
    var id = $(thiss).attr('id');
    var grandparent = $(parent).parent();
    $(grandparent).find($(parent)).remove();
    $("#tagViw").append($($(parent).removeClass('used')));
    selectedCourseTag.push(id);
    removeItem(selectCourseTag, id);
    $('#CourseTag').val(selectedCourseTag.unique().join(',').trimRight(','));
    var content = $.trim($(grandparent).html());
    if (content == "") {
        $($(grandparent).parent()).attr("style", "display:none;");
    }
}
function removeItem(arr, item) {
    for (var i = 0; i < arr.length;) {
        if (item == arr[i]) {
            arr.splice(i, 1);
        } else {
            i++;
        }
    }
    return arr;
}

Array.prototype.unique = function () {
    var res = [];
    var json = {};
    for (var i = 0; i < this.length; i++) {
        if (!json[this[i]]) {
            res.push(this[i]);
            json[this[i]] = 1;
        }
    }
    return res;
};

function tagAdd() {
    topevery.ajax({
        url: 'api/BaseTag/AddOrEdit',
        data: JSON.stringify({ ModelCode: 'msk', TagName: $("#TagName").val() }),
    }, function (data) {
        if (data.Success) {
            if (data.Result.Success) {
                $("#TagName").val("");
                selectedCourseTag.push(data.Result.ModelId);
                bindTag();
                layer.msg(data.Result.Message, {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            } else {
                layer.msg(data.Result.Message, {
                    icon: 2,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            }
        }
    });
}
