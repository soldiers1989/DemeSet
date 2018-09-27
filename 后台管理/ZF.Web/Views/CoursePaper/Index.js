$(function() {
    var getPostDataUrl = "api/CoursePaper/GetList";
    var grid = $("#paperData");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: false,
        sortname:"OrderNo",
        colNames: ['编码', '试卷组名称','试卷组类别', '排序号', '操作'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, hidden: true, align: "center" },
            { name: 'PaperInfoName', width: 50, align: "center" },
            {
                name: 'Type', width: 50, align: "center", formatter: function (value) {
                    return value == 0 ? "历年真题" : "模拟试卷";
                }
            },
            { name: 'OrderNo', width: 50, align: "center" },
            {
                name: '',
                width: 20,
                align: "center",
                formatter: function(cellValue, options, rowdata, action) {
                    return "<a class=\"glyphicon glyphicon-remove edit_btn\" onclick=\"deletePaper(this)\"  data-id=\"" + rowdata.Id + "\" style=\"cursor:pointer;color:red;\"></a>";
                }
            }
        ],
        postData: topevery.form2Json("paperSelectFrom"),
        pager: '#paperPager',
        height: '350'
    });


});

var addCoursePaper;

$("#addPaper").on('click', function () {
    var url = "CoursePaper/AddOrEdit" + "?id=" + $("#QueryCourseId").val();
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
        addCoursePaper = layer.open({
            type: 1,
            title: "添加课程试卷组",
            skin: 'layui-layer-rim', //加上边框
            area: [800 + 'px', 500 + 'px'], //宽高
            content: data
        });
    }, true);
});

function deletePaper(e) {
    var id = $(e).data("id");
    layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
        topevery.ajax({
            url: "api/CoursePaper/Delete",
            data: JSON.stringify({ "Ids": id })
        }, function (dataObj) {
            var getPostDataUrl = "api/CoursePaper/GetList";
            var grid = $("#paperData");
            grid.jqGrid('setGridParam', {
                url: getPostDataUrl, page: 1, postData: topevery.form2Json("paperSelectFrom")
            }).trigger("reloadGrid");
            //$(".layui-layer-close").click();
            layer.close(index);
        });
    })
}

//function editPaper() {
//    var url = "CoursePaper/AddOrEdit" + "?id=" + $("#QueryCourseId").val();
//    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
//        addCoursePaper = layer.open({
//            type: 1,
//            title: "编辑课程试卷",
//            skin: 'layui-layer-rim', //加上边框
//            area: [600 + 'px', 400 + 'px'], //宽高
//            content: data
//        });
//    }, true);
//}