$(function () {
  
    GetUserInfo();

    var fileBtn = $("input[type=file]");
    fileBtn.on("change", function () {
        document.getElementById("formid").submit();
        setTimeout(function () { $("#uimage").attr("src", $("#hiimage").val()) }, 500);
    });

    //保存用户信息
    $("#userSave").on("click", function () {
        var obj = new Object();
        obj.Id = $("#hiuid").val();
        obj.HeadImage = $("#uimage").attr("src");
        obj.NickNamw = $("#uname").val();
        //更改用户信息
        topevery.ajax({
            url: "api/Register/UpdateOne",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (data.Success) {
                $("#hiimage").val("");
                $("#hiUname").val("")
                layer.msg(info.Message, { time: 1000, icon: 1 });
                GetUserInfo();
                parent.location.reload();
            } else {
                layer.msg(info.Message, { time: 1000, icon: 2 });
            }
        });
    })
 
})


function GetUserInfo() {
    //获取用户信息
    topevery.ajax({
        url: "api/Register/GetOne"
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            //TelphoneNum
            if ($("#hiimage").val() != "") {
                $("#uimage").attr("src", $("#hiimage").val());
                $("#uname").val($("#hiUname").val())
            } else {
                $("#uimage").attr("src", info.HeadImage);
                $("#hiimage").val(info.HeadImage);
                $("#uname").val(info.NickNamw);
            }
            $("#uphone").html(info.TelphoneNum);
            $("#hiuid").val(info.Id);
        }
    });
}
