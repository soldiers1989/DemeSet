$(function () {
    dataList();
})

function alertAddAddressDiag() {
    layer.open({
        type: 2,
        title: '地址新增编辑',
        shadeClose: true,
        maxmin: false, //开启最大化最小化按钮
        area: ['600px', '500px'],
        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
        content: 'HandOutPathAddOrEnditi',
        end: function () {
            dataList();
        }
    });
}

function dataList() {
    topevery.ajaxwx({
        url: "api/DeliveryAddress/GetList",
        data: JSON.stringify({})
    }, function (data) {
        if (data.Success) {
            $("#bindinfo").html(template("path_html", data.Result))
        }
    });
}

//删除地址
function alertDelAddressDiag(event) {
    var obj = new Object();
    obj.Id = $(event).attr("name");
    layer.confirm("确定删除吗?", { title: "删除确认" }, function (index) {
        topevery.ajaxwx({
            url: "api/DeliveryAddress/DelDeliveryAddress",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                layer.msg(info.Message, { time: 1500, icon: 1 });
                dataList();
            } else {
                layer.msg(info.Message, { time: 1500, icon: 2 })
            }

        });
    });
}

//修改默认地址
function makeAddressAllDefaultByoverseas(event) {
    var obj = new Object();
    obj.Id = $(event).attr("name");
    topevery.ajaxwx({
        url: "api/DeliveryAddress/EnditiDefaultAddress",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            layer.msg(info.Message, { time: 1500, icon: 1 });
            dataList();
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 })
        }

    });
}

//编辑
function alertUpdateAddressDiagByoverseas(event) {
    layer.open({
        type: 2,
        title: '地址新增编辑',
        shadeClose: true,
        maxmin: false, //开启最大化最小化按钮
        area: ['600px', '500px'],
        shade: [0.7, '#BEBEBE'], //0.7透明度的白色
        content: 'HandOutPathAddOrEnditi?id=' + $(event).attr("name"),
        end: function () {
            dataList();
        }
    });
}