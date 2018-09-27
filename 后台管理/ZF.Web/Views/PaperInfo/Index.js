$(function () {
    var urlList = [
        "PaperInfo/AutomaticallyGroupVolume",//自动组卷
        "PaperInfo/ManualGroupVolume?type=0&paperId",//手工组卷
        $("#tblData"),//列表
        "api/PaperInfo/Delete",//删除试卷
        "api/PaperInfo/GetList", //查询
        "PaperInfo/EditInfo" //查询
    ];
    var grid = $("#tblData");
    grid.jgridInit({
        url: urlList[4],
        sortname: "PaperName",
        colNames: ['序号', '试卷名称', "所属科目", '试卷总分', '时长', '发布状态', "试卷类别", '试卷明细', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true, key: true },
            { name: 'PaperName', index: 'PaperName', width: 120, align: "center" },
            { name: 'SubjectName', index: '', width: 70, align: "center" },
            { name: 'QuestionScore', index: 'QuestionScore', width: 50, align: "center" },
            { name: 'TestTime', index: 'TestTime', width: 50, align: "center" },
            {
                name: 'State', index: 'State', width: 50, align: "center", formatter: function (value, options, rowData) { return value === 0 ? "未发布" : "已发布"; }
            },
            {
                name: 'Type', index: 'Type', width: 50, align: "center", formatter: function (value) {
                    return value == 0 ? "历年真题" : "模拟试卷";
                }
            },
            { name: '', search: false, width: 40, align: "center", sortable: false, formatter: editLink },
            { name: '', search: false, width: 40, align: "center", sortable: false, formatter: editState },
               { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".automatically_btn").bindAddBtn(urlList[0], 600, 500);
    $(".manual_btn").bindAddBtn(urlList[1], 1200, 800);
    $(".del_btn").bindDelBtnDx(urlList[3], grid, function (rowData) {
        var id = rowData.Id;
        var flag = true;
        topevery.ajax({
            url: 'api/PaperInfo/IfUse',
            data: JSON.stringify({ Id: id }),
            async: false
        }, function (data) {
            if (data.Result) {
                layer.alert("该试卷已被使用,不能删除!");
                flag = false;
            }
        });
        return flag;
    });
    $(".edit_btn").bindEditBtn(urlList[5], grid, 600, 400);

    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: urlList[4], page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    topevery.BindTree("treeDemo1", "Common/SubjectTreeList", onClickTree, "");
    function onClickTree(data) {
        if (data.type === "1") {
            $("#QueryProjectClassId").val(data.id);
        } else if (data.type === "2") {
            $("#QueryProjectId").val(data.id);
        } else if (data.type === "3") {
            $("#QuerySubjectId").val(data.id);
            $(".query_btn").click();
        }
    }
});

function editLink(cellValue, options, rowdata, action) {
    var IsDisabled = rowdata.State === 0 ? "编辑" : "查看";

    return "<span style='color:#00c0ef;cursor:pointer;' name='" + rowdata.State + "' id='" + rowdata.Id + "' onclick='EditiPaperInfo(this)'>" + IsDisabled + "</span> ";
}

function editState(cellValue, options, rowdata, action) {
    var title = rowdata.State === 0 ? "发布" : "<span style='color:red;'>取消发布<span>";
    return "<span style='color:#00c0ef;cursor:pointer;' name='" + rowdata.Id + "' id='" + rowdata.State + "' onclick='EditiPaperInfoState(this)'>" + title + "</span> ";
}

$(".export_btn").click(function () {
    var rowIndex = $("#tblData").jqGrid('getGridParam', 'selarrrow');
    if (rowIndex.length === 1) {
        var rowData = $("#tblData").jqGrid('getRowData', rowIndex);
        window.location.href = "PaperInfo/Export?Id=" + rowData.Id;
    } else {
        layer.alert("请选择一条且只选择一条记录!");
    }
})

function EditiPaperInfo(event) {
    var detailId = $(event).attr("id");
    topevery.ajax({ type: "get", url: "PaperInfo/ManualGroupVolume?type=1&paperId=" + detailId + "&paperState=" + $(event).attr("name"), dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "参数明细",
            skin: 'layui-layer-rim', //加上边框
            area: ['1200px', '800px'], //宽高
            content: data
        });
    }, true);
}

function EditiPaperInfoState(event) {
    var message;
    var obj = new Object();
    obj.Id = $(event).attr("name");
    switch ($(event).attr("id")) {
        case "0":
            obj.State = 1;
            message = "确定要发布吗";
            break;
        case "1":
            message = "确定要取消发布吗";
            obj.State = 0;
            break;
    }
    layer.confirm(message, { title: "发布确认" }, function (index) {
        topevery.ajax({
            url: "api/PaperInfo/EditInfoState",
            data: JSON.stringify(obj)
        }, function (data) {
            $(".query_btn").click();
            var icon = data.Success === true ? 1 : 2;
            var into = data.Result;
            layer.msg(into.Message, {
                icon: icon,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        });
    });
}