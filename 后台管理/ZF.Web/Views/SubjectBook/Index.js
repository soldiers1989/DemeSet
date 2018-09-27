$(function () {
    var getPostDataUrl = "api/SubjectBook/GetList";
    var addUrl = "SubjectBook/AddOrEdit";
    var deleteUrl = "api/SubjectBook/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "OrderNo",
        colNames: ['序号', '书籍名称', '排序号', '链接Url', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'BookName', index: 'BookName', width: 100, align: "center" },
            { name: 'OrderNo', index: 'OrderNo', width: 100, align: "center" },
            { name: 'Url', index: 'Url', width: 70, align: "center" },
            { name: '', index: '', width: 100, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    //绑定树结构
    topevery.BindTree("treeDemoTherr", "Common/SubjectTreeList", onClickTree);
    function onClickTree(data) {
        $("#QuerySubjectId").val("");
        $("#QueryProjectId").val("");
        if (data.type == "3") {
            $(".add_btn").fadeIn("slow");
            $("#QuerySubjectId").val(data.id);
        } else if (data.type == "2") {
            $(".add_btn").fadeOut("slow");
            $("#QueryProjectId").val(data.id);
        } else {
            $(".add_btn").fadeOut("slow");
        }
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    };

    //新增明细
    $(".add_btn").on("click", function () {
        //获得选中项
        var treeObj = $.fn.zTree.getZTreeObj("treeDemoTherr");
        var nodes = treeObj.getSelectedNodes();
        var subjectId = nodes[0].id;
        topevery.ajax({ type: "get", url: "SubjectBook/AddOrEdit?SubjectId=" +subjectId, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "新增书籍",
                skin: 'layui-layer-rim', //加上边框
                area: ['600px', '500px'], //宽高
                content: data
            });
        }, true);
    })
});
