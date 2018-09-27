var index;
$(function () {
    var header = $(".content-wrapper").outerHeight(true)
                      - $(".nav-header").outerHeight(true) - 3
                      - $(".content-header").outerHeight(true)
                      - $(this).parent().parent().prev().find(".ui-jqgrid-hbox").outerHeight(true)
                      - 31;
    $(".KnowledgePointTree").height(header);
    var getPostDataUrl = "api/SubjectKnowledgePoint/GetList";
    var addUrl = "SubjectKnowledgePoint/AddOrEdit?ParentId=" + $("#QueryParentId").val() + "&ParentName=" + $("#QueryParentName").val();
    var editUrl = "SubjectKnowledgePoint/AddOrEdit";
    var deleteUrl = "api/SubjectKnowledgePoint/Delete";
    var subjectId = "";
    var dataType = false;
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "KnowledgePointCode",
        sortorder: "asc",
        colNames: ['序号', '科目编号', '知识点名称', '知识点代码', '所属科目', '上级知识点', '电子书页码', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'SubjectId', index: 'SubjectId', width: 50, align: "center", hidden: true },
            { name: 'KnowledgePointName', index: 'KnowledgePointName', width: 80, align: "left" },
            { name: 'KnowledgePointCode', index: 'KnowledgePointCode', width: 60, align: "center" },
            { name: 'SubjectName', index: 'SubjectName', width: 80, align: "center" },
            { name: 'ParentName', index: 'ParentName', width: 60, align: "center" },
            { name: 'DigitalBookPage', index: 'DigitalBookPage', width: 60, align: "center" },
               { name: '', index: '', width: 150, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    //新增按钮绑定
    $(".add_btn").on("click", function () {
        if (dataType) {
            index = topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: "新增",
                    skin: 'layui-layer-rim', //加上边框
                    area: [600 + 'px', 500 + 'px'], //宽高
                    content: data
                });
            }, true);
        } else {
            layer.alert("请先选择树形菜单上的科目或知识点节点在进行知识点新增!");
        }

    });
    //修改按钮绑定
    $(".edit_btn").on("click", function () {
        var rowIndex = $(grid).jqGrid('getGridParam', 'selarrrow');
        if (rowIndex.length === 1) {
            var rowData = $(grid).jqGrid('getRowData', rowIndex);
            /*layer弹出一个html页面或者html片段*/
            topevery.ajax({ type: "get", url: editUrl + "?Id=" + rowData.Id + "&SubjectId=" + rowData.SubjectId, dataType: "html" }, function (data) {
                layer.open({
                    type: 1,
                    title: "修改",
                    skin: 'layui-layer-rim', //加上边框
                    area: [600 + 'px', 500 + 'px'], //宽高
                    content: data
                });
            }, true);

        } else {
            layer.alert("请选择一条且只选择一条记录!");
        }
    });
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    topevery.BindTree("treeDemo1", "Common/SubjectKnowledgePointTree", onClickTree, "");
    function onClickTree(data) {
        $("#QueryProjectClassId").val("");
        $("#QueryProjectId").val("");
        //判断树节点是否是 科目  知识点  刷新右侧列表
        if (data.type === "3" || data.type === "4" || data.type === "5") {
            subjectId = data.subjectId;
            dataType = true;
            $("#QueryParentId").val(data.id);
            $("#QueryParentName").val(data.name);
            addUrl = "SubjectKnowledgePoint/AddOrEdit?ParentId=" + $("#QueryParentId").val() + "&ParentName=" + $("#QueryParentName").val() + "&SubjectId=" + subjectId;
            $(".query_btn").click();
        } else {
            subjectId = "";
            dataType = false;
            $("#QueryParentId").val("");
            $("#QueryParentName").val("");
            if (data.type === "1") {
                $("#QueryProjectClassId").val(data.id);
            } else if (data.type === "2") {
                $("#QueryProjectId").val(data.id);
            }
            $(".query_btn").click();
        }
    }
});

