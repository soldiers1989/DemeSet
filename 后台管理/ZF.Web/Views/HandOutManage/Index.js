var selectType = "";
var getPostDataUrl = "api/ExpressCompany/GetList";
var addUrl = "HandOutManage/AddOrEdit";
var deleteUrl = "api/ExpressCompany/Delete";
var grid = $("#tblData");
$(function () {
    $(".j-order-status").find("a").each(function () {
        $(this).on("click", function () {
            RemoveClass(this);
        });
    });
    $(".add_btn").bindAddBtn(addUrl, 600, 500);
    $(".edit_btn").bindEditBtn(addUrl, grid, 600, 500);
    $(".del_btn").bindDelBtn(deleteUrl, grid);
    $(".query_btn").on("click", function () {
        $("#tblData").jqGrid('setGridParam', {
            url: getPostDataUrl, page: 1, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    });

    getHandoutInfo(0);
});

//需邮寄信息
function getHandoutInfo(infoType) {
    var obj = new Object();
    obj.SelectType = infoType;
    topevery.ajax({
        url: "api/ExpressCompany/GetDeliveryAddRessList",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (data.Success) {
            $("#expressHtml").html(template("express_jshtml", info))
        }
    });
}

//快递公司信息
function getExpressCompanyInfo() {
    grid.jgridInit({
        url: getPostDataUrl,
        sortname: "AddTime",
        sortorder: "desc",
        colNames: ['序号', '快递公司名称', '快递公司网址', '是否默认', '设置', '', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'Name', index: 'Name', width: 300, align: "center" },
            { name: 'Companyurl', index: 'Companyurl', width: 300, align: "center" },
            { name: 'IsDefault', index: 'IsDefault', width: 100, align: "center", formatter: function (value) { return value === 0 ? "否" : "是" } },
             { name: '', search: false, width: 200, align: "center", sortable: false, formatter: editDefault },
            { name: '', index: '', width: 410, align: "center" },
            { name: '', index: '', width: 240, align: "center" },
        ],
        postData: topevery.form2Json("selectFrom")
    });

}

function editDefault(cellValue, options, rowdata, action) {
    if (rowdata.IsDefault === 0) {
        return "<span style='color:#00c0ef;cursor:pointer;' name='" + rowdata.IsDefault + "' id='" + rowdata.Id + "' onclick='EditiExpressDefault(this)'>设置为默认快递公司</span> ";
    } else {
        return "";
    }
}

//设置默认快递公司
function EditiExpressDefault(event) {
    var obj = new Object();
    obj.Id = $(event).attr("id");
    topevery.ajax({
        url: "api/ExpressCompany/UpdateDefault",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            layer.msg(info.Message, { time: 1500, icon: 1 });
            $(".query_btn").click();
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}


function RemoveClass(event) {
    $(".j-order-status").find("a").each(function () {
        $(this).removeClass("current");
    });
    $(event).addClass("current");
    selectType = $(event).attr("name");
    //加载需邮寄信息
    getHandoutInfo(selectType);
}

//寄出讲义
function sendOutHandouts(event) {
    var obj = new Object();
    obj.ExpressCompanyId = $(event).parent().find("select").val();
    obj.ExpressNumber = $(event).parent().find("input[type='text']").val();
    obj.OrderNo = $(event).parent().parent().find("div[class='info']").attr("name");
    if (!obj.ExpressNumber || !obj.ExpressCompanyId) {
        layer.msg("订单号和快递公司不能为空", { time: 1500, icon: 2 });
    } else {
        topevery.ajax({
            url: "api/OrderSheet/UpdateSheetExpressCompany",
            data: JSON.stringify(obj)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                getHandoutInfo(selectType);
                layer.msg(info.Message, { time: 1500, icon: 1 });
            } else {
                layer.msg(info.Message, { time: 1500, icon: 2 });
            }
        });
    }
}

//撤销邮寄讲义
function revoke(event) {
    var obj = new Object();
    obj.OrderNo = $(event).parent().parent().find("div[class='info']").attr("name");
    topevery.ajax({
        url: "api/OrderSheet/UpdateSheetExpressCompany",
        data: JSON.stringify(obj)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            getHandoutInfo(selectType);
            layer.msg(info.Message, { time: 1500, icon: 1 });
        } else {
            layer.msg(info.Message, { time: 1500, icon: 2 });
        }
    });
}

$(".report_btn").click(function () {
    window.location.href = "HandOutManage/Export?type=0";
});
$(".report_btn1").click(function () {
    window.location.href = "HandOutManage/Export?type=1";
});