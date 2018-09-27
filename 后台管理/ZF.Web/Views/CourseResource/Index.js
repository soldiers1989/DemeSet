$(function () {
    var getPostDataUrl = "api/CourseResource/GetList";
    var grid = $("#resourceData");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect:false,
        colNames: ['编码', '资源名称', '大小',  '创建时间','操作'], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, hidden: true, align: "center" },
            { name: 'ResourceName', width: 50, align: "center" },
            { name: 'ResourceSize', width: 20, align: "center" },
            //{
            //    name: 'ResourceUrl', width: 100, align: "center"
            //},
            { name: 'AddTime', width: 60, align: "center", formatter: topevery.dataTimeFormatTT },
            {
                name: '', width: 40, align: "center", formatter: function (cellValue, options, rowdata, action) {
                    return "<a class=\"glyphicon glyphicon-remove edit_btn\" onclick=\"deleteResource(this)\" title=\"删除\" data-id=\"" + rowdata.Id + "\" style=\"cursor:pointer;color:red;\"></a>&nbsp;&nbsp;" + "<a class=\"glyphicon glyphicon-pencil delete_btn\" onclick=\"editResource(this)\" title=\"编辑\"  data-id=\"" + rowdata.Id + "\" style=\"cursor:pointer\"></a>";
            }}
        ],
        postData: topevery.form2Json("resourceSelectFrom"),
        pager: '#resourcePager',
        height: '350'
    });

});

var addResourceLayer;
var editResourceLayer;

$("#addResource").on('click', function () {
    var url = "CourseResource/AddOrEdit" + "?CourseId=" + $("#QueryCourseId").val()+"&id="+'';
     topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
       addResourceLayer= layer.open({
            type: 1,
            title: "添加课程资源",
            skin: 'layui-layer-rim', //加上边框
            area: [600 + 'px', 400 + 'px'], //宽高
            content: data
        });
    }, true);
});

function deleteResource(e) {
    var id = $(e).data("id");
    layer.confirm("确认要删除吗，删除后不能恢复", { title: "删除确认" }, function (index) {
        topevery.ajax({
            url: "api/CourseResource/Delete",
            data: JSON.stringify({ "Ids": id })
        }, function (dataObj) {
            var getPostDataUrl = "api/CourseResource/GetList";
            var grid = $("#resourceData");
            grid.jqGrid('setGridParam', {
                url: getPostDataUrl, page: 1, postData: topevery.form2Json("resourceSelectFrom")
            }).trigger("reloadGrid");
            //$(".layui-layer-close").click();
        });
        layer.close(index);
    })
}

function editResource(e) {
    var id = $(e).data('id');
    console.log(id);
    var url = "CourseResource/AddOrEdit" + "?CourseId=" + $("#QueryCourseId").val()+"&id="+id;
    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
        editResourceLayer = layer.open({
            type: 1,
            title: "编辑课程资源",
            skin: 'layui-layer-rim', //加上边框
            area: [600 + 'px', 400 + 'px'], //宽高
            content: data
        });
    }, true);
}