$(function () {
    if ($("#handid").val()) {
        getOne($("#handid").val());
    };

});

function getOne(id) {
    var obj = new Object();
    obj.Id = id;
    topevery.ajax({
        url: "api/DeliveryAddress/GetOne",
        data: JSON.stringify(obj)
    }, function (data) {
        if (data.Success) {
            var info = data.Result;
            $("#consigneeName").val(info.Contact);
            $("#province").val(info.Province);
            $("#province").change();
            $("#city").val(info.City);
            $("#city").change();
            $("#town").val(info.Town);
            $("#consigneeAddress").val(info.DetailedAddress);
            $("#consigneeMobile").val(info.ContactPhone);
            $("#consigneeEmail").val(info.Zip);
        } else {
            $(".btns").hide();
        }
    });
}

//添加修改信息
function addAddress() {
    if (!$("#consigneeName").val()) {
        showError(1);
        return;
    } else if (!$("#province").val() || !$("#city").val() || !$("#town").val()) {
        showError(2);
        return;
    } else if (!$("#consigneeAddress").val()) {
        showError(3);
        return;
    } else if (!$("#consigneeMobile").val()) {
        showError(4);
        return;
    } 
    $("#consigneeAddressNote").hide();

    var obj = new Object();
    if ($("#handid").val()) {
        obj.Id = $("#handid").val();
    }
    obj.Contact = $("#consigneeName").val();
    obj.ContactPhone = $("#consigneeMobile").val();
    obj.Zip = $("#consigneeEmail").val();
    obj.Province = $("#province").val();
    obj.City = $("#city").val();
    obj.Town = $("#town").val();
    obj.DetailedAddress = $("#consigneeAddress").val();
    topevery.ajax({
        url: "api/DeliveryAddress/AddOrEdit",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        layer.msg(info.Message, { time: 1500, icon: 1 });
    });
}


function showError(type) {
    switch (type) {
        case 1:
            $("#consigneeAddressNote").html("收货人不能为空");
            break;
        case 2:
            $("#consigneeAddressNote").html("所在地区不完整");
            break;
        case 3:
            $("#consigneeAddressNote").html("详细地址不能为空");
            break;
        case 4:
            $("#consigneeAddressNote").html("手机号码不能为空");
            break;
        case 5:
            $("#consigneeAddressNote").html("邮编不能为空");
            break;
    }
    $("#consigneeAddressNote").show();
}

function phoneIsvalidata(event) {
    var pattern_chin = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(15[0-3]{1})|(15[5-9]{1})|(18[0-9]{1}))+\d{8})$/;
    var phone = $(event).val();
    var matchResult = pattern_chin.test(phone);
    if (!matchResult) {
        $("#consigneeAddressNote").html("手机号格式不正确");
        $(event).val("");
        $("#consigneeAddressNote").show();
        return false;
    } else {
        $("#consigneeAddressNote").html("");
        $("#consigneeAddressNote").hide();
    }
}