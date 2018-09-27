$(function () {
    //topevery.ajax({
    //    url: "Common/Select2CourseList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $('#QTargetCourseId').select2({
    //        data: data,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $(".box"),
    //        allowClear: true
    //    })
    //    $('#QTargetCourseId').val('-1').trigger('change');
    //});


    var getPostDataUrl = "api/PurchaseDiscount/GetList";
    var addUrl = "PurchaseDiscount/AddOrEdit";
    var deleteUrl = "api/PurchaseDiscount/Delete";
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "BeginDate",
        sortorder: "desc",
        colNames: ['编码',  '购买(元)', '减免(元)', '有效期开始', '有效期结束',  ''],
        colModel: [
            { name: 'Id', index: 'Id', width: 80, align: "center", hidden: true },
            //{ name: 'TargetCourse', width: 80, align: "center" },
            { name: 'TopNum', width: 40, align: "center" },
            { name: 'MinusNum', width: 40, align: "center" },
            { name: 'BeginDate', width: 50, align: "center", formatter: topevery.dataTimeView },
            { name: 'EndDate', width: 50, align: "center", formatter: topevery.dataTimeView },
            //{ name: 'Record', width: 40, align: "center" },
            //{
            //    name: '', width: 60, align: "center", formatter: function (cellValue, options, rowdata, action) {
            //        return "<input onclick=\"javascript:checkDetail(this)\" data-id=\"" + rowdata.CardCode + "\" class=\"btn btn-info edit_btn\" type=\"button\" value=\"使用记录\">";
            //    }
            //},
            { name: '', width: 200, align: "center" },
        ],
        postData: { TargetCourse: $('#QTargetCourseId').val(), BeginDate: $('#QBeginDate').val(), EndDate: $('#QEndDate').val() }
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 500);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: { TargetCourse: $('#QTargetCourseId').val(), BeginDate: $('#QBeginDate').val(), EndDate: $('#QEndDate').val() }
        }).trigger("reloadGrid");
    });


});

//function checkDetail(e) {
//    var id = $(e).data('id');
//    var url = "UserDiscountCard/Index?id=" + id;
//    topevery.ajax({ type: "get", url: url, dataType: "html" }, function (data) {
//        layer.open({
//            type: 1,
//            title: '学习卡使用记录',
//            skin: 'layui-layer-rim', //加上边框
//            area: [1000 + 'px', 600 + 'px'], //宽高
//            content: data
//        });
//    }, true);
//}