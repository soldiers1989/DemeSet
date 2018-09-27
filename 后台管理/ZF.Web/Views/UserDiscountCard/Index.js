$(function () {
    //topevery.ajax({
    //    url: "Common/Select2CourseList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $("#QCourseId").select2({
    //        data: data,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $(".box"),
    //        allowClear: true
    //    })
    //    $('#QCourseId').val('-1').trigger('change');
    //});

    //topevery.ajax({
    //    url: "Common/UserList",
    //    data: JSON.stringify({})
    //}, function (data) {
    //    $("#QUserId").select2({
    //        data: data,
    //        placeholder: {
    //            id: '-1',
    //            text: '请选择'
    //        },
    //        dropdownParent: $(".uniqueBox"),
    //        allowClear: true
    //    })
    //    $('#QUserId').val('-1').trigger('change');
    //});


    var getPostDataUrl = "api/UserDiscountCard/GetList";
    var grid = $("#tblData1");
    grid.jgridInit({
        url: getPostDataUrl,
        colNames: ['编码', '手机号码', '领用时间', '使用时间', ''],
        colModel: [
            { name: 'Id', index: 'Id', width: 80, align: "center", hidden: true },
            { name: 'TelphoneNum', width: 80, align: "center" },
            { name: 'AddTime', width: 80, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: 'UseTime', width: 80, align: "center", formatter: topevery.dataTimeFormatTT },
            { name: '', width: 200, align: "center" },
        ],
        pager: '#pager1',
        postData: { TelphoneNum: $('#QtelphoneNum').val(), CardId: $('#QCardId').val() },
        height:"400"
    });
    $("#btn_inner_query").on("click", function () {
        $("#tblData1").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: { TelphoneNum: $('#QtelphoneNum').val(), CardId: $('#QCardId').val() }
        }).trigger("reloadGrid");
    });


});

