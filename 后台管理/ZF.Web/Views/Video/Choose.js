$(function () {
    var getPostDataUrl = "api/CourseVideo/GetLists";
    var grid = $("#tblData2");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "VideoName",
        sortorder: "asc",
        multiselect: false,
        pager: "pager2",
        height: "340",
        colNames: ['序号', '视频名称', '视频编号', '二维码标题', ''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'VideoName', index: 'VideoName', width: 200, align: "center" },
            { name: 'Code', index: 'Code', width: 150, align: "center" },
             { name: 'QcodeTitle', index: 'QcodeTitle', width: 100, align: "center" },
              { name: '', index: '', width: 50, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
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
            parent.$("#VideoId").val(rowData.Id);
            parent.$("#VideoName").val(rowData.VideoName);
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

