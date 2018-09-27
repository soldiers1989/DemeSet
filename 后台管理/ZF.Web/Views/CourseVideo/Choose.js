$(function () {
    $("#QueryName").val(topevery.GetCookie("QueryName"));
    var getPostDataUrl = "api/CourseVideo/VideoGetList";
    var grid = $("#tblData2");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Name",
        sortorder: "asc",
        height: 300,
        pager: "#pager2",
        colNames: ['序号', '原文件名称', '视频别名', ''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
              { name: 'Name', index: 'Name', width: 200, align: "center" },
            { name: 'VideoAlias', index: 'VideoAlias', width: 200, align: "center" },
              { name: '', index: '', width: 20, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom1")
    });
    $(".query_btn").on("click", function () {
        $("#tblData2").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom1")
        }).trigger("reloadGrid");
    });
    $(".addVideo").click(function () {
        var rowIndex = $("#tblData2").jqGrid('getGridParam', 'selrow');
        if (rowIndex != null) {
            var rowData = $("#tblData2").jqGrid('getRowData', rowIndex);

            parent.$("#VideoUrl").val(rowData.Id);
            parent.$("#VideoName1").val(rowData.VideoAlias);
            topevery.SetCookie("QueryName", $("#QueryName").val());
            try {
                $(".layui-layer-close1")[2].click();
            } catch (e) {
                $(".layui-layer-close1")[1].click();
            }
        } else {
            layer.alert("请选择一条且只选择一条记录!");
        }
    });
});

