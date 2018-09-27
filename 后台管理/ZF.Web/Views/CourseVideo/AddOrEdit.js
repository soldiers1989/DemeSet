///初始化
$(function () {
    var chapterId = $('#ChapterId').val();
    //topevery.ajax({
    //    url: "Common/VideoList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $("#VideoUrl").select2({
    //        data: data,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $(".box"),
    //        allowClear: true
    //    })
    //    $('#VideoUrl').val('-1').trigger('change');
    //    Initialize();
    //});
    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseVideo/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.setParmByLookForm(row);

                    if (row.IsTaste == 0) {
                        $('#tasteTimeArea').attr('style', 'display:none;')
                    } else if (row.IsTaste == 1) {
                        $('#tasteTimeArea').attr('style', 'display:block;')
                    }
                    //$('#VideoUrl').val(row.VideoUrl).trigger('change');
                }
            });
        }
    }



    var addUrl = "api/CourseVideo/AddOrEdit";

    $('#sumbitForm').bootstrapValidatorAndSumbit(addUrl, {
        VideoName: { validators: { notEmpty: { message: '请输入视频名称' } } },
        //VideoUrl: { validators: { notEmpty: { message: '请课程视频名称' } } },
        OrderNo: { validators: { notEmpty: { message: '请输入排序号' } } },
        Code: { validators: { notEmpty: { message: '请输入视频编号' } } },
    }, function () {
        if ($('#IsTaste').val() === "1") {
            if (!$('#TasteLongTime').val()) {
                topevery.msg("可试听时长不能为空!", 2);
                return false;
            }
        }
        //判断编号是否唯一
        if ($("#Code").val()) {
            var istrue = true;
            topevery.ajax({
                url: "api/CourseVideo/VideoIsOnlyOne",
                data: JSON.stringify({ Code: $("#Code").val(), Id: $("#Id").val() }),
                async: false
            }, function (data) {
                var info = data.Result;
                istrue = info.Success;
                if (!info.Success) {
                    topevery.msg("视频编号已存在", 2);
                }
            });
            if (!istrue) {
                return false;
            }
        }
        return true;
    }, {}, function () {
        $(".layui-layer-close")[0].click();
        //parent.CourseVideo();

    }, "sumbitForm");
    $('#IsTaste').change(function () {
        if (this.value == "0") {
            $('#tasteTimeArea').attr("style", 'display:none;');
            $('#TasteLongTime').val(0);
        } else if (this.value == "1") {
            $('#tasteTimeArea').attr("style", 'display:block;');
        }
    });
    $("#chooseVideo").click(function () {
        var addUrl = "CourseVideo/Choose";
        topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "选择视频",
                skin: 'layui-layer-rim', //加上边框
                area: [600 + 'px', 500 + 'px'], //宽高
                content: data
            });
        }, true);
    });
});


