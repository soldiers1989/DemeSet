$(function () {
    var urlList = [
        "api/RegisterUser/GetList" //查询
    ];
    var grid = $("#tblData");
    grid.jgridInit({
        url: urlList[0],
        colNames: ['序号', '昵称', '手机号', '注册时间', '注册终端(方式)', '头像', '状态', '用户详情', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'NickNamw', index: 'NickNamw', width: 100, align: "left" },
            { name: 'TelphoneNum', index: 'TelphoneNum', width: 70, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center" },
            { name: 'RegiesterType', index: 'RegiesterType', width: 60, align: "center" },
            { name: 'HeadImage', index: 'HeadImage', width: 60, align: "center", formatter: imageFormat },
            { name: 'State', index: 'State', width: 50, align: "center", edittype: 'select', formatter: userSate },
            { name: '', search: false, width: 30, align: "center", sortable: false, formatter: editLink },
            { name: '', index: '', width: 150, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: urlList[0], page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    $("#report").on("click", function () {
        GraphicReport();
    });

    //选择年份查询事件
    $("#reportselect").on("click", function () {
        GraphicReport();
    });
    //禁用
    $("#userDisabled").on("click", function () {
        userEnditorSate("1");
    });
    //启用
    $("#userEnable").on("click", function () {
        userEnditorSate("0");
    });
});

function editLink(cellValue, options, rowdata, action) {
    return "<span style='color:#00c0ef;cursor:pointer;' id='" + rowdata.Id + "' onclick='getUserInfo(this)'>查看</span> ";
}

function getUserInfo(event) {
    var detailId = $(event).attr("id");
    topevery.ajax({ type: "get", url: "RegisterUser/UserDetails?pid=" + detailId, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "用户详情",
            skin: 'layui-layer-rim', //加上边框
            area: ['450px', '350px'], //宽高
            content: data
        });
    }, true);
}
//自定义图片的格式，可以根据rowdata自定义
function imageFormat(cellvalue, options, rowObject) {
    return '<img src="' + cellvalue + '"  style="width:50px;height:40px;" />';
}
function userSate(tm) {
    if (tm === 0) {
        return "启用";
    } else if (tm === 1) {
        return "禁用";
    }
    return "";
}

//图形报表
function GraphicReport() {
    var xdata = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
    var apiurl = "api/RegisterUser/GetRegisterUserCountByType?addtime=" + $("#addtime").val();
    var lendata = ['app', 'web', 'WeChat']
    //绑定图形报表
    topevery.BindReport(xdata, apiurl, "注册数", document.getElementById('main'), lendata);
}

function userEnditorSate(sate) {
    var rowIds = $("#tblData").jqGrid('getGridParam', 'selarrrow');
    var ids = [];
    if (rowIds.length) {
        for (var i = 0; i < rowIds.length; i++) {
            var ID = $("#tblData").jqGrid('getRowData', rowIds[i]).Id;
            ids[i] = ID + "#" + sate;
        }
        var obj = new Object();
        obj.Ids = ids.join(',');
        topevery.ajax({
            url: "api/RegisterUser/UpdateUserSate",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                $("#tblData").jqGrid('setGridParam', {
                    url: "api/RegisterUser/GetList", page: 1, postData: topevery.form2Json("selectFrom")
                }).trigger("reloadGrid");
                layer.msg(info.Message, { time: 2000, icon: 1 });
            } else {
                layer.msg(info.Message, { time: 2000, icon: 2 });
            }
        });
    } else {
        var msg = sate == "0" ? "请选择需启用的用户" : "请选择需禁用的用户";
        layer.msg(msg, { time: 1000, icon: 2 });
    }
}