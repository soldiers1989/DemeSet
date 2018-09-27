$(function () {
    var getPostDataUrl = "api/PaperGroup/GetList";
    var addUrl = "PaperGroup/AddOrEdit";
    var deleteUrl = "api/PaperGroup/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '试卷组名称', '所属科目', '试卷属性', '试卷状态', '试卷状态', '创建时间', '试卷明细', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true, key: true },
            { name: 'PaperGroupName', index: 'PaperGroupName', width: 130, align: "center" },
            { name: 'SubjectName', index: '', width: 60, align: "center" },
            { name: 'TypeName', index: '', width: 60, align: "center" },
            { name: 'StateName', index: '', width: 60, align: "center" },
             { name: 'State', index: '', width: 60, align: "center", hidden: true },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: '', search: false, width: 40, sortable: false, align: "center", formatter: editLink },
             { name: '', search: false, width: 40, align: "center", sortable: false, formatter: editState },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    // $(".add_btn").bindAddBtn(addUrl, 600, 400);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    topevery.BindTree("treeDemo", "Common/SubjectTreeList", onClickTree);
    function onClickTree(data) {
        if (data.type == "3") {
            $("#infoAdd").fadeIn("slow");
        } else {
            $("#infoAdd").fadeOut("slow");
        }
        $("#QuerySubjectId").val(data.id);
        $(".query_btn").click();
    };

    //添加结构
    $("#infoAdd").on("click", function () {
        //获得选中项
        var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
        var nodes = treeObj.getSelectedNodes();
        var id = "#" + nodes[0].id;
        topevery.ajax({ type: "get", url: "PaperGroup/AddOrEdit?id=" + encodeURIComponent(id), dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "新增试卷组",
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '500px'], //宽高
                content: data
            });
        }, true);
    })
});
function editLink(cellValue, options, rowdata, action) {
    return "<span style='color:#00c0ef;cursor:pointer;' id='" + rowdata.Id + "' onclick='addStructureDetail(this)'>试卷明细</span> ";
}

function editState(cellValue, options, rowdata, action) {
    debugger;
    var title = rowdata.State === 0 ? "发布" : "取消发布";
    return "<span style='color:#00c0ef;cursor:pointer;' name='" + rowdata.Id + "' id='" + rowdata.State + "' onclick='EditiPaperInfoState(this)'>" + title + "</span> ";
}

//url + "?Id=" + id
function addStructureDetail(event) {
    var detailId = $(event).attr("id");
    topevery.ajax({ type: "get", url: "PaperGroup/PersonnelManagement?PaperGroupId=" + detailId, dataType: "html" }, function (data) {
        $('.content-wrapper').html(data);
    }, true);
}


function EditiPaperInfoState(event) {
    debugger;
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
            url: "api/PaperGroup/EditInfoState",
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