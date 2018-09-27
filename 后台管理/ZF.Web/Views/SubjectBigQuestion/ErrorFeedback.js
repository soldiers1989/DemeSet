$(function () {
    var getPostDataUrl = "api/SubjectBigQuestion/GetErrFeedBackList"
    var grid = $("#tblData");
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "BeginDate",
        sortorder: "desc",
        colNames: ['编码', '反馈错题内容', '状态', ''],
        colModel: [
            { name: 'Id', index: 'Id', width: 80, align: "center", hidden: true },
            { name: 'Content', width: 300, align: "center" },
            { name: 'Audit', width: 40, align: "center",formatter:function(value){
                return value==1?"已确认":"待确认";
            }
            },
            {
                name: '', width: 40, align: "center", formatter: function (cellValue, options, rowdata, action) {
                    return rowdata.Audit==1? "<a style=\"cursor:pointer;color:red;\" title=\"已审核\" >已审核</a> " : "<a style=\"cursor:pointer;\"  title=\"审核\" data='" + rowdata.Id + "' onclick='Audit(this)'>审核</a> ";
                }
            },
        ],
        postData: {Audit:$("input[name='Audit']:checked").val(), BeginDate: $('#QBeginDate').val(), EndDate: $('#QEndDate').val() }
    });


    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });


})

function Audit(e) {
    var id=$(e).attr("data");
    var url = "api/SubjectBigQuestion/Audit?Id="+id;
    topevery.ajax({
        url: url,
    }, function (data) {
        if (data.Result.Success) {
            $(".query_btn").click();
            layer.msg("审核成功");
        }
    });
}