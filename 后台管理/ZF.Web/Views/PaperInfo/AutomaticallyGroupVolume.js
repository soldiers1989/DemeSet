$(function () {
    Ladda.bind('#btnsave', { callback: saveInfo });

    //绑定试卷结构参数
    topevery.BindSelect("PaperParamId", "Common/GetListToSelect", "");
});

function saveInfo(instance) {
 

    if (inputIsnull()) {
        $(".ladda-label").html("正在组卷");
        var data = new Object();
        data.Id = "";
        data.PaperName = $("#PaperName").val();
        data.PaperInfoCount = $("#PapersNo").val();
        data.PaperParamId = $("#PaperParamId").val();
        data.TestTime = $("#TestTime").val();
        data.Type = $("#QuizpaperType").val();
        topevery.ajax({
            url: "api/PaperInfo/PaperInfoAddList",
            data: JSON.stringify(data)
        }, function (data) {
            layer.alert(data.Result.Message);
            $(".ladda-label").html("开始组卷");
            instance.stop();
            $(".query_btn").click();
        });

    } else {
        $(".ladda-label").html("开始组卷");
        instance.stop();
        layer.alert("试卷名称,试卷数量,试卷参数,考试时长不允许为空");
    }
}

//判断非空
function inputIsnull() {
    var input = [
        $("#PaperName").val(),
        $("#PapersNo").val(),
        $("#PaperParamId").val(),
        $("#TestTime").val()
    ];
    var inputisTrue = true;
    for (var i = 0; i < input.length; i++) {
        if (input[i] === "") {
            inputisTrue = false;
            break;
        }
    }
    return inputisTrue;
}