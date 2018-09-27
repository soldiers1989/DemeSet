$(function () {
    var getPostDataUrl = "api/CourseSecurityCode/GetList";
    var deleteUrl = "api/CourseSecurityCode/Delete";
    var importUrl = "CourseSecurityCode/ImpRept?courseId=" + $("#CourseId").val();
    var addUrl = "CourseSecurityCode/AddOrEdit?courseId=" + $("#CourseId").val();
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "Code",
        colNames: ['序号', '防伪码', '是否使用', '是否增值服务', '使用用户昵称', '使用时间', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Code', index: 'Code', width: 100, align: "center" },
            { name: 'IsUse', index: 'IsUse', width: 100, align: "center", formatter: topevery.IsEconomicBase },
            { name: 'IsValueAdded', index: 'IsValueAdded', width: 100, align: "center", formatter: topevery.IsEconomicBase },
            { name: 'NickNamw', index: 'NickNamw', width: 100, align: "center" },
            { name: 'GetDateTime', index: 'GetDateTime', width: 100, align: "center" },
            { name: '', index: '', width: 200, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });

    $(".delete_btn").on("click", function () {
        layer.confirm("确认要清空吗，清空后不能恢复", { title: "清空确认" }, function (index) {
            $.topevery.ajax({
                url: deleteUrl,
                data: JSON.stringify({
                    "Ids": $("#CourseId").val()
                })
            }, function (data) {
                var message = "清空失败";
                var icon = 1;
                if (data.Success) {
                    icon = data.Result.Success === true ? 1 : 2;
                    if (data.Result.Success) {
                        $("#tblData").trigger("reloadGrid");
                    }
                    message = data.Result.Message;
                }
                layer.msg(message, {
                    icon: icon,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            });
        });
    });
    $(".import_btn").on("click", function () {
        layer.open({
            type: 2,
            title: "导入",
            closeBtn: 1, //不显示关闭按钮
            area: [600 + 'px', 420 + 'px'],
            anim: 2,
            content: [importUrl, 'no'] //iframe的url，no代表不显示滚动条
        });
    });
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });
    $(".add_btn").on("click", function () {
        topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "新增",
                skin: 'layui-layer-rim', //加上边框
                area: [400 + 'px', 300 + 'px'], //宽高
                content: data
            });
        }, true);
    });
    $('#return_btn').on('click', function () {
        topevery.ajax({ url: "CourseInfo/CourseContent", type: 'get', dataType: 'html' }, function (data) {
            $('.content-wrapper').html(data);
        });
    });
});