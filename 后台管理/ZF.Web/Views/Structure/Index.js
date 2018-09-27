$(function () {
    var getPostDataUrl = "api/Structure/GetList";
    var addUrl = "Structure/AddOrEdit";
    var deleteUrl = "api/Structure/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['序号', '试卷结构名称', '所属科目', '创建人', '创建时间', '查看', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true,key:true },
            { name: 'StuctureName', index: 'StuctureName', width: 100, align: "center" },
            { name: 'SubjectId', index: 'SubjectId', width: 80, align: "center" },
            { name: 'AddUserId', index: 'AddUserId', width: 80, align: "center" },
            { name: 'AddTime', index: 'AddTime', width: 100, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: '', search: false, width: 40, sortable: false, align: "center", formatter: editLink },
              { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
   // $(".add_btn").bindAddBtn(addUrl, 600, 400);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 400);
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
        topevery.ajax({ type: "get", url: "Structure/AddOrEdit?id=" +encodeURIComponent(id), dataType: "html" }, function (data) {
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
function editLink(cellValue, options, rowdata, action) {
    return "<span style='color:#00c0ef;cursor:pointer;' id='" + rowdata.Id + "' onclick='addStructureDetail(this)'>结构明细</span> ";
}
//url + "?Id=" + id
function addStructureDetail(event) {
    var detailId = $(event).attr("id");
    topevery.ajax({ type: "get", url: "StructureDetail/Index?pid=" + detailId, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "试卷结构明细",
            skin: 'layui-layer-rim', //加上边框
            area: ['1300px', '600px'], //宽高
            content: data
        });
    }, true);
}
