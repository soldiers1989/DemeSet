$(function () {
    var urlList = [
    "api/PaperStructureDetail/GetList" //查询
    ];
    var grid = $("#Datalist");
    grid.jgridInit({
        url: urlList[0],
        pager: "#pagerlist",
        multiselect: false,
        sortname: "OrderNo",
        height:"390px",
        colNames: ['序号', '题型', '难度等级', '试题数量', '题型总分', '操作'], //列头,, '题型类型' '题型名称' , '排序号'
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            //{ name: 'QuesitonTypeName', index: 'QuesitonTypeName', width: 100, align: "center", editable: true },
           // { name: 'QuestionTypeVal', index: 'QuestionType', width: 100, align: "center", editable: true },
            { name: 'QuestionClassName', index: 'QuestionClass', width: 100, align: "center", editable: true },
            { name: 'DifficultLevelName', index: 'DifficultLevel', width: 100, align: "center", editable: true },
            { name: 'QuestionCount', index: 'QuestionCount', width: 100, align: "center", editable: true },
            { name: 'QuestionTypeScoreSum', index: 'QuestionTypeScoreSum', width: 100, align: "center", editable: true },
            //{ name: 'OrderNo', index: 'OrderNo', width: 100, align: "center", editable: true },

            { name: 'edit', index: 'edit', width: 130, height: 300, align: "right" },
        ],
        gridComplete: function () { // 第二步：数据加载完成后 添加操作按钮  
            RowBindEnditButton();
        },
        postData: topevery.form2Json("selectFromtoDetail")
    });
    $("#btnselect").on("click", function () {
        $("#Datalist").jqGrid('setGridParam', {
            url: "api/PaperStructureDetail/GetList", page: 1, postData: topevery.form2Json("selectFromtoDetail")
        }).trigger("reloadGrid");
    });

    $("#btnAdd").on("click", function () {
        addRow();
    })
});

function editParam(rowId, event) { //第三步：定义编辑操作
    
    var data = $("#Datalist").jqGrid('getRowData', rowId);
    var oldList = [data.QuestionTypeVal, data.QuestionClassName, data.DifficultLevelName];
    if ($("#" + rowId + "_QuestionCount").attr("type") === "number") {
        return;
    }
    if (event) {
        $(event).parent().find("input[name='save']").show();
        $(event).parent().find("input[name='cancel']").show();
    }

    $("#Datalist").editRow(rowId);//开启可编辑模式
    //改变文本框类型
    $("#" + rowId + "_QuestionCount").attr("type", "number");
    $("#" + rowId + "_QuestionTypeScoreSum").attr("type", "number");

    //做正整数判断
    $("#" + rowId + "_QuestionCount").keyup(function () {
        if (!isPositiveInteger($(this).val())) {
            $(this).val("");
        }
    });

    $("#" + rowId + "_QuestionTypeScoreSum").keyup(function () {
        if (!isPositiveInteger($(this).val())) {
            $(this).val("");
        }
    });

    //题型类型
    //var selectid = "" + rowId + "_QuestionTypeVal";
    //GetSelectHtml("Common/QuestionTypeList", "-请选择-", $("#" + rowId + "_QuestionTypeVal").parent(), selectid, "QuestionTypeVal", oldList[0]);
    //题型分类
    var selectid = "" + rowId + "_QuestionClassName";
    GetSelectQuestionClass("api/SubjectClass/GetListByStructureId?ProjectId=" + $("#StuctureId").val(), "-请选择-", $("#" + rowId + "_QuestionClassName").parent(), selectid, "QuestionClassName", oldList[1]);
    //难度等级
    var selectid = "" + rowId + "_DifficultLevelName";
    GetSelectHtml("Common/DifficultLevelList", "-请选择-", $("#" + rowId + "_DifficultLevelName").parent(), selectid, "DifficultLevelName", oldList[2]);
}

function isPositiveInteger(s) {//是否为正整数
    var reg = /(^[1-9]+\d*$)|(^0$)/;
    return reg.test(s);
}

//第四步：定义保存操作，通过键值对把编辑的数据传到后台,如下
function saveParam(rowId,event) {
    var data = $("#Datalist").jqGrid('getRowData', rowId);
    var parment = GetParament(rowId);
    if (parment.QuestionCount == "" || parment.QuestionTypeScoreSum == "" || parment.OrderNo == "" || parment.QuestionClass == undefined || parment.QuestionClass == "") {
        var message = "题型分类,试题数量、题型总分、排序号不允许为空"
        layer.msg(message, {
            icon: 2,
            title: false, //不显示标题
            offset: 'auto',
            time: 5000,
            anim: 5
        });
        return;
    }
    topevery.ajax({
        url: "api/PaperStructureDetail/AddOrEdit",
        data:  JSON.stringify(parment)
    }, function (data) {
        var info = data.Result;
        if (info.Success) {
            $("#Datalist").jqGrid('setGridParam', {
                url: "api/PaperStructureDetail/GetList", page: 1, postData: topevery.form2Json("selectFromtoDetail")
            }).trigger("reloadGrid");
        } else {
            layer.msg(info.Message, { time: 2000, icon: 2 });
        }
    });
}

//第五步：定义取消操作
function cancelParam(rowId,event) {

    $(event).parent().find("input[name='save']").hide();
    $(event).parent().find("input[name='cancel']").hide();
    $('#Datalist').restoreRow(rowId); //用修改前的数据填充当前行
}

//删除
function delParam(rowId) {
    var data = $("#Datalist").jqGrid('getRowData', rowId);
    if (data.Id == "") {
        $("#Datalist").jqGrid("delRowData", rowId);
    } else {
        var row = new Object();
        row.Ids = data.Id;
        topevery.ajax({
            url: "api/PaperStructureDetail/Delete",
            data: JSON.stringify(row)
        }, function (data) {
            var info = data.Result;
            if (info.Success) {
                $("#Datalist").jqGrid('setGridParam', {
                    url: "api/PaperStructureDetail/GetList", page: 1, postData: topevery.form2Json("selectFromtoDetail")
                }).trigger("reloadGrid");
            } else {
                layer.msg(info.Message, { time: 1000, icon: 2 });
            }
        });
    }
}

//grid添加新的一行
var newrowid;
function addRow() {
    //$("#operate").val("");
    var selectedId = $("#Datalist").jqGrid("getGridParam", "selrow");
    var ids = jQuery("#Datalist").jqGrid('getDataIDs');
    //获得当前最大行号（数据编号）
    var rowid = Math.max.apply(Math, ids);
    if (rowid == "-Infinity") {
        rowid = 0;
    }
    //获得新添加行的行号（数据编号）
    newrowid = rowid + 1;
    var dataRow = {
        Id: "",
        UserName: "",
        TelphoneNum: '',
        edit: ''
    }
    //debugger
    //editParam(newrowid);
    $("#Datalist").jqGrid("addRowData", newrowid, dataRow, "first");
    editParam(newrowid,'');
}

//给行绑定编辑按钮
function RowBindEnditButton() {
    var ids = jQuery("#Datalist").jqGrid('getDataIDs'); //获取表格的所有列
    for (var i = 0; i < ids.length; i++) {
        var id = ids[i];
        debugger
        var cellId = jQuery("#Datalist").jqGrid('getCell', id, "Id");
        var isShow = cellId == "" ? "" : "display:none;"
        var editBtn = "<div> "
                        + "<input class='btn btn-info' type='button' value='编辑' name='edit'  onclick='editParam("
                        + id
                        + ",this)'>&nbsp;&nbsp;"
                        + "<input class='btn btn-primary' value='保存' style='" + isShow + "' name='save' type='button' onclick='saveParam("
                        + id
                        + ",this)'>&nbsp;&nbsp;"
                        + "<input class='btn btn-primary' value='取消' style='" + isShow + "' name='cancel' type='button'  onclick='cancelParam("
                        + id
                        + ",this)'>&nbsp;&nbsp;"
                        + "<input class='btn btn-danger' type='button' value='删除' name='del' onclick='delParam("
                        + id + ")'/> </div>"

        $("#Datalist").jqGrid('setRowData', ids[i], { edit: editBtn }); //给每一列添加操作按钮
    }
}

function BindQuestionType() {
    var QuestionHtml = "";
    QuestionHtml = GetSelectHtml("Common/QuestionTypeList", "-请选择-");
    return QuestionHtml;
}

function BindDifficultLevel() {
    var LevelHtml = "";
    QuestionHtml = GetSelectHtml("Common/DifficultLevelList", "-请选择-");
    return QuestionHtml;
}

function GetSelectHtml(url, defaultText, enent, selectid, selectname, rowId) {
    topevery.ajax({
        url: url,
        data: JSON.stringify({})
    }, function (data) {
        var html = "<select style='width:80px;height:34px;' name='" + selectname + "' id='" + selectid + "'>";
        for (var i = 0; i < data.length; i++) {
            if (data[i].Value == rowId) {
                html += "<option selected ='selected' value=" + data[i].Key + ">" + data[i].Value + "</option>";
            } else {
                html += "<option value=" + data[i].Key + ">" + data[i].Value + "</option>";
            } 
        }
        html += "</select>";
        $(enent).html(html);
    });

}

function GetSelectQuestionClass(url, defaultText, enent, selectid, selectname, rowId) {
    var data = $("#Datalist").jqGrid('getRowData', rowId);
    var oldVal = $(enent).val();
    topevery.ajax({
        type: "GET",
        url: url,
        data: JSON.stringify({})
    }, function (data) {
        var html = "<select style='width:80px;height:34px;' name='" + selectname + "' id='" + selectid + "'>";
        $.each(data.Result, function (i, item) {
            if (rowId == item.ClassName) {
                html += "<option selected ='selected' value=" + item.Id + ">" + item.ClassName + "</option>";
            } else {
                html += "<option value=" + item.Id + ">" + item.ClassName + "</option>";
            }
            
        })
        html += "</select>";
        $(enent).html(html);
    });
    $(enent).val(oldVal);
}

function GetParament(rowindex) {    
    var data = $("#Datalist").jqGrid('getRowData', rowindex);
    var row = new Object();
    row.StuctureId = $("#StuctureId").val();
    row.Id = data.Id;
    row.QuesitonTypeName = "1"; //$("#" + rowindex + "_QuesitonTypeName").val();
    //row.QuestionType = $("#" + rowindex + "_QuestionTypeVal").val();
    row.QuestionClass = $("#" + rowindex + "_QuestionClassName").val();
    row.DifficultLevel = $("#" + rowindex + "_DifficultLevelName").val();
    row.QuestionCount = $("#" + rowindex + "_QuestionCount").val();
    row.QuestionTypeScoreSum = $("#" + rowindex + "_QuestionTypeScoreSum").val();
    row.OrderNo = "1";//$("#" + rowindex + "_OrderNo").val();
    console.log(row);
    return row;
}

function BindDataList() {
   
}