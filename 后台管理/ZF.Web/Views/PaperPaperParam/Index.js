$(function () {
    var getPostDataUrl = "api/PaperPaperParam/GetList";
    var addUrl = "PaperPaperParam/AddOrEdit";
    var deleteUrl = "api/PaperPaperParam/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '参数名称', '所属试卷结构', '创建时间', '发布状态', '参数明细', '操作', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'ParamName', index: 'ParamName', width: 100, align: "center" },
            { name: 'StuctureId', index: 'StuctureId', width: 100, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 70, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: 'State', index: 'State', width: 70, align: "center", formatter: fabu },
            { name: '', search: false, width: 30, align: "center", sortable: false, formatter: editLink },
            { name: '', search: false, width: 30, align: "center", sortable: false, formatter: releaseLink },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    }); 

    function releaseLink(cellValue, options, rowdata, action) {
        var msg = rowdata.State == 0 ? "取消发布" : "发布";
        var state = rowdata.State == 0 ? 1 : 0;
        return "<span style='color:#00c0ef;cursor:pointer;' id='" + rowdata.Id + "' name='" + state + "' onclick='editState(this)'>" + msg + "</span> ";
    }

    function fabu(tm) {
        if (tm === 0) {
            return "已发布";
        } else if (tm === 1) {
            return "未发布";
        }
        return "";
    }

    //$(".add_btn").bindAddBtn(addUrl, 600, 400);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 400);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    topevery.BindSelect("QueryStuctureId", "Common/StructureList", "--全部--");
    //绑定树结构
    topevery.BindTree("treeDemoTherr", "Common/SubjectTreeList", onClickTree);
    function onClickTree(data) {
        if (data.type == "3") {
            $(".add_btn").fadeIn("slow");
        } else {
            $(".add_btn").fadeOut("slow");
        }
        var obj = new Object();
        obj.id = data.id
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: obj
        }).trigger("reloadGrid");
        //重新绑定结构下拉列表
        bindQueryStuctureId(data.id);
    };

    //新增明细
    $(".add_btn").on("click", function () {
        //获得选中项
        var treeObj = $.fn.zTree.getZTreeObj("treeDemoTherr");
        var nodes = treeObj.getSelectedNodes();
        var id = "#" + nodes[0].id;
        topevery.ajax({ type: "get", url: "PaperPaperParam/AddOrEdit?id=" + encodeURIComponent(id), dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "新增结构",
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '400px'], //宽高
                content: data
            });
        }, true);
    })
});

//绑定结构
function bindQueryStuctureId(stuId) {
    var obj = new Object();
    obj.SubjectId = stuId;
    topevery.ajax({
        url: "api/Structure/GetList",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            $("#QueryStuctureId").empty();
            var sub = " <option value=''>--全部--</option> ";
            $.each(info.Rows, function (i, item) {
                sub += "<option value='" + item.Id + "'>" + item.StuctureName + "</option>"
            })
            $("#QueryStuctureId").append(sub);
        }
    });
}
function editLink(cellValue, options, rowdata, action) {
    return "<span style='color:#00c0ef;cursor:pointer;' id='" + rowdata.Id + "' onclick='addPaperPaperParam(this)'>配置</span> ";
}

function addPaperPaperParam(event) {
    var detailId = $(event).attr("id");
    topevery.ajax({ type: "get", url: "ParamDetail/Index?pid=" + detailId, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "参数明细",
            skin: 'layui-layer-rim', //加上边框
            area: ['1000px', '550px'], //宽高
            content: data
        });
    }, true);
}

//修改发布状态
function editState(event) {
    var obj = new Object();
    obj.Id = $(event).attr("id");
    obj.State = $(event).attr("name");
    topevery.ajax({
        url: "api/PaperPaperParam/UpdateState",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            layer.msg(info.Message, { time: 2000, icon: 1 })
            $(".query_btn").click();
        } else {
            layer.msg(info.Message, { time: 2000, icon: 2 })
        }
    });
}