$(function () {
    var getPostDataUrl = "api/CourseVideo/VideoGetList";
    var addUrl = "Video/AddOrEdit";
    var deleteUrl = "api/CourseVideo/VideoDelete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Name",
        sortorder: "asc",
        colNames: ['序号', '原文件名称', '视频别名', '视频类型', '视频时长(秒)', '操作', ''],//列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 200, align: "center" },
               { name: 'VideoAlias', index: 'VideoAlias', width: 200, align: "center" },
            { name: 'Type', index: 'Type', width: 100, align: "center" },
             { name: 'Duration', index: 'Duration', width: 100, align: "center" },
            {
                name: '', index: '', width: 70, align: "center", formatter: function (a, b, c) {
                    return "<a onclick=\"VideoPlay('" + c.Id + "')\">预览视频</a>";
                }
            },
              { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });
    $(".add_btn").bindAddBtn(addUrl, 1000, 700, "上传视频");
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
});

function VideoPlay(id) {
    var addUrl = "Video/Play?Id=" + id;
    topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
        layer.open({
            type: 1,
            title: "视频预览",
            skin: 'layui-layer-rim', //加上边框
            area: [1200 + 'px', 650 + 'px'], //宽高
            content: data
        });
    }, true);
}