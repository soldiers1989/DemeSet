///初始化
$(function () {
    var addorEditUrl = 'api/DiscountCard/AddOrEdit';
    topevery.ajax({
        url: "Common/Select2CourseList",
        data: JSON.stringify({})
    }, function (data) {
        $('#TargetCourse').select2({
            data: data,
            multiple: true,
            placeholder: {
                id: '-1',
                text: '请选择'
            },
            dropdownParent: $("#sumbitForm"),
            allowClear: true
        })
        $('#TargetCourse').val('-1').trigger('change');
    });

    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/DiscountCard/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    row.BeginDate = topevery.dataTimeView(row.BeginDate);
                    row.EndDate = topevery.dataTimeView(row.EndDate);
                    topevery.setParmByLookForm(row);
                    $('#TargetCourse').val(row.TargetCourse.split(',')).trigger("change");
                }
            });
        } else {
            $("BeginDate").val(today());
        }
    }

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        CardName: { validators: { notEmpty: { message: '名称不能为空!' } } },
        CardCode: { validators: { notEmpty: { message: '卡号不能为空!' } } },
        Amount: { validators: { notEmpty: { message: '卡号不能为空!' }, numeric: {message:"请输入有效数字"} }, },
        BeginDate: { Validators: { date: { message: "有效期起始时间不能为空" } } },
        EndDate: { Validators: { date: { message: "有效期截止时间不能为空" } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if ($('#CourseId').val() == "-1") {
            topevery.msg("请选择课程!", 2);
            $(".btn-primary").removeAttr("disabled");
            return false;
        }
        if ($.trim($('#Amount').val()) == "" || $.trim($('#Amount').val()) == "0") {
            topevery.msg("请输入折扣金额", 2);
            $(".btn-primary").removeAttr("disabled");
            return false;
        }
        var re = /^[0-9]+.?[0-9]*$/; //判断字符串是否为数字 //判断正整数 /^[1-9]+[0-9]*]*$/ 
        var nubmer = $('#Amount').val();
        if (!re.test(nubmer)) {
            topevery.msg("请输入有效金额", 2);
            $('#Amount').val("");
            return false;
        }
        return true;
    }, "", function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({}, "sumbitForm");
            $('#CourseId').val("-1").trigger("change");
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
    }, "sumbitForm");
});

$('#CardCode').on('blur', function () {
    topevery.ajax({
        url: 'api/DiscountCard/IfUnique?CardCode=' + $('#CardCode').val(),
        async: false
    }, function (data) {
        if (data.Success && data.Result) {
            $(".btn-primary").removeAttr("disabled");
        } else {
            topevery.msg("该卡号已经存在，请重新输入!",2);
            $(".btn-primary").attr("disabled","disabled");
        }
    })
})

function today() {
    var today = new Date();
    var h = today.getFullYear();
    var m = today.getMonth() + 1;
    var d = today.getDate();
    m = m < 10 ? "0" + m : m;   //  这里判断月份是否<10,如果是在月份前面加'0'
    d = d < 10 ? "0" + d : d;        //  这里判断日期是否<10,如果是在日期前面加'0'
    return h + "-" + m + "-" + d;
}
