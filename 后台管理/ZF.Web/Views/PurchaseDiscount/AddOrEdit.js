///初始化
$(function () {
    var addorEditUrl = 'api/PurchaseDiscount/AddOrEdit';
    //topevery.ajax({
    //    url: "Common/Select2CourseList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $('#TargetCourse').select2({
    //        data: data,
    //        multiple: true,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $("#sumbitForm"),
    //        allowClear: true
    //    })
    //    $('#TargetCourse').val('-1').trigger('change');
    //});

    Initialize();
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/PurchaseDiscount/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    row.BeginDate = topevery.dataTimeView(row.BeginDate);
                    row.EndDate = topevery.dataTimeView(row.EndDate);
                    topevery.setParmByLookForm(row);
                    //$('#TargetCourse').val(row.TargetCourse.split(',')).trigger("change");
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
        //TargetCourse: { validators: { notEmpty: { message: '课程不能为空!' } } },
        TopNum: { validators: { notEmpty: { message: "购买金额不能为空" } }, numeric: { message: "请输入有效数字" } },
        MinusNum: { validators: { notEmpty: { message: "购买金额不能为空" } }, numeric: { message: "请输入有效数字" } },
        BeginDate: { Validators: { notEmpty: { message: "有效期起始时间不能为空" } } },
        EndDate: { Validators: { notEmpty: { message: "有效期截止时间不能为空" } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        var price = $('#TopNum').val();
        var favourableprice = $('#MinusNum').val();
        if (parseFloat(favourableprice) > parseFloat(price)) {
            topevery.msg("减免金额应小于购买金额", 2);
            $(this).focus();
            $('.btn-primary').removeAttr('disabled');
            return false;
        }
        if (JSON.parse($.trim($dp.$('BeginDate').value) == "" || $.trim($dp.$('EndDate').value == ""))) {
            topevery.msg("请选择有效起止日期",2);
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


$('#TopNum,#MinusNum').on("keydown keyup", function () {
    if (isFloat(this.value)) {
        if (this.value.split('.')[1].length > 2) {
            $(this).val(this.value.split('.')[0] + "." + this.value.split('.')[1].substr(0, 2));
        }
    }
});


function isFloat(oNum)
{
    if (!oNum)
        return false;
    var strP = /^\d+(\.\d+)+$/;
    if (!strP.test(oNum)) {
        return false;
    }
    return true;
}

function today() {
    var today = new Date();
    var h = today.getFullYear();
    var m = today.getMonth() + 1;
    var d = today.getDate();
    m = m < 10 ? "0" + m : m;   //  这里判断月份是否<10,如果是在月份前面加'0'
    d = d < 10 ? "0" + d : d;        //  这里判断日期是否<10,如果是在日期前面加'0'
    return h + "-" + m + "-" + d;
}
