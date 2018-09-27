var BigQuestionIdList = [];
var AddList = [];
var oldList = [];
$(function () {
    if ($("#paperState").val() == "1") {
        $("#btnManualGroupVolume").hide();
        $("#btndel").hide();
    }
    //绑定试卷结构参数
    topevery.BindSelect("PaperParamId", "Common/GetListToSelect", "", Initialize);

    function Initialize() {
        bindtree();
    }
    //删除试题事件
    $("#btndel").on("click", function () {
        var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
        var nodes = treeObj.getCheckedNodes(true);
        if (nodes.length <= 0) {
            layer.alert("请选择要删除的试题!");
            return false;
        } else {
            var idList = "";
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].type == "99") {
                    //删除数组元素
                    BigQuestionIdList.splice($.inArray(nodes[i].KIdList, BigQuestionIdList), 1);

                    AddList.splice($.inArray(nodes[i].KIdList, AddList), 1);
                    //移除节点
                    treeObj.removeNode(nodes[i]);
                } else if (nodes[i].type == "4") {
                    if (idList == "") {
                        idList = "" + nodes[i].id + "";
                    } else {
                        idList += "," + nodes[i].id + "";
                    }
                   
                }
            }
            console.log(idList);
            var data = new Object();
            data.Ids = idList;
            topevery.ajax({
                url: "api/PaperDetatail/Delete",
                data: JSON.stringify(data)
            }, function (data) {
                var info = data.Result;
                layer.msg(info.Message, {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 5000,
                    anim: 5
                });
                if (data.Success) {
                    BigQuestionIdList = [];
                    AddList = [];
                    oldList = [];
                    //刷新树
                    bindtree();
                }
            });
        }
    });

    $("#PaperParamId").on("change", function () {
        $.fn.zTree.init($("#treeDemoTwo"), null, null);
        bindtree();
    });
    //组卷
    $("#btnManualGroupVolume").on("click", function () {
        //试题主表
        var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
        var nodes;
        //试题数量
        var SumQuestionCount = 0;
        //试题分数
        var SumQScore = 0;
        if (treeObj != null) {
            nodes = treeObj.getNodesByParam("type", "2", null);
            if (nodes.length > 0) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].QuestionCount != undefined && nodes[i].QuestionCount != "" && nodes[i].Code != "NO") {
                        SumQuestionCount = SumQuestionCount + parseInt(nodes[i].QuestionCount);
                        SumQScore = SumQScore + parseInt(nodes[i].QScoreSum);
                    }
                } 
            }
            nodes = treeObj.getNodesByParam("type", "3", null);
            if (nodes.length > 0) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].QuestionCount != undefined && nodes[i].QuestionCount != "" && nodes[i].Code != "NO") {
                        SumQuestionCount = SumQuestionCount + parseInt(nodes[i].QuestionCount);
                        SumQScore = SumQScore + parseInt(nodes[i].QScoreSum);
                    }
                }
            }
        }
        //试题明细
            if (inputIsnull()) {
                if (SumQuestionCount != BigQuestionIdList.length) {
                    layer.confirm("所选试题数不等于参数设置试题数,确认要组卷吗?", { title: "组卷确认" }, function (index) {
                        var parment = new Object();
                        parment.PaperName = $("#PaperName").val();//试卷名称
                        parment.PaperParamId = $("#PaperParamId").val();//参数ID
                        parment.TestTime = $("#TestTime").val();//考试时长
                        parment.QuestionCount = SumQuestionCount;//试题数量
                        parment.QuestionScoreSum = SumQScore;//试题分数
                        parment.QuestionId = BigQuestionIdList.join(",");
                        parment.PaperId = $("#paperId").val();
                        parment.Type = $("#QuizpaperType").val();
                        topevery.ajax({
                            url: "api/PaperInfo/ManualGroupVolumeAddInfo",
                            data: JSON.stringify(parment)
                        }, function (data) {
                            BigQuestionIdList = [];
                            AddList = [];
                            oldList = [];

                            var icon = data.Result.Success === true ? 1 : 2;
                            $(".layui-layer-close").click();
                            $(".query_btn").click();
                            $(".return").click();
                            layer.msg(data.Result.Message, {
                                icon: icon,
                                title: false, //不显示标题
                                offset: 'auto',
                                time: 3000, //10秒后自动关闭
                                anim: 5
                            });
                        });
                    });
                } else {
                    var parment = new Object();
                    parment.PaperName = $("#PaperName").val();//试卷名称
                    parment.PaperParamId = $("#PaperParamId").val();//参数ID
                    parment.TestTime = $("#TestTime").val();//考试时长
                    parment.QuestionCount = SumQuestionCount;//试题数量
                    parment.QuestionScoreSum = SumQScore;//试题分数
                    parment.QuestionId = BigQuestionIdList.join(",");
                    parment.PaperId = $("#paperId").val();
                    parment.Type = $("#QuizpaperType").val();
                    topevery.ajax({
                        url: "api/PaperInfo/ManualGroupVolumeAddInfo",
                        data: JSON.stringify(parment)
                    }, function (data) {
                        BigQuestionIdList = [];
                        AddList = [];
                        oldList = [];

                        var icon = data.Result.Success === true ? 1 : 2;
                        $(".layui-layer-close").click();
                        $(".query_btn").click();
                        $(".return").click();
                        layer.msg(data.Result.Message, {
                            icon: icon,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                    });
                }
            } else {
                layer.alert("试卷参数，试卷名称，考试时长为必填项!");
            }
    });
});
function BindDetailTree(btnid, url, callback, onClick, expandNode, callback1, l, data) {;
    if (onClick == undefined || onClick === "") {
        onClick = function () {
        };
    }
    if (l == undefined || l === "") {
        l = 1;
    }
    if (expandNode == undefined || expandNode === "") {
        expandNode = false;
    }
    topevery.ajax({
        url: url,
        data: JSON.stringify(data)
    }, function (data) {
        var setting = {
            showLine: true,
            checkable: true,
            sonSign: true,
            isSimpleData: true,
            expandFlag: true,
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "pId",
                rootPId: 0
            },
            callback: {
                beforeClick: function (event, treeId, treeNode) {
                    if (callback != undefined) {
                        callback(treeId);
                    }
                },
                onClick: onClick,
                beforeCheck: function (treeId, treeNode) {      //选择复选框前相关回调
                    //if (treeNode.isParent) {
                    //    if (treeNode.type !== "4") { //判断是否是需要显示复选框的节点
                    //        treeNode.nocheck = true;   //设置复选框不显示
                    //    }
                    //    return false;
                    //}
                }

            },
            view: {
                selectedMulti: false,
                txtSelectedEnable: true,
                showLine: true,
                addDiyDom: addDiyDomWith
            },
            check: {
                enable: true,     //这里设置是否显示复选框
                chkboxType: { "Y": "s", "N": "s" }     //设置复选框是否与 父/子 级相关联
            }
        };
        $.fn.zTree.init($("#" + btnid), setting, data.Result);
        var treeObj = $.fn.zTree.getZTreeObj("" + btnid + "");
        if (expandNode) {
            var nodes = treeObj.getNodesByParam("type", "4", null);
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].icon = "../Img/2.jpg";
                BigQuestionIdList.push(nodes[i].BigQuestionId);
            }
            treeObj.expandAll(true);
        } else {
            topevery.showztreemenuNum(treeObj, true, "", l);
        }
        if (callback1 != undefined && callback1 !== "") {
            callback1();
        }
    });
}
////第四步：定义保存操作，通过键值对把编辑的数据传到后台,如下   layer.alert("请选择一条且只选择一条记录!");
function onClickStructureTree(data) {
    var PaperId = $("#paperId").val();//试卷id
    switch (data.type){
        case "4":
            $.fn.zTree.init($("#treeDemoTwo"), null, null);
            break;
        case "3":
            //先保存已选择的项
            GetTreeDemoTwoCheckIsTrue();
            //绑定试题树
            var idisList = data.KIdList.split(',');
            debugger
            if (idisList.length > 1) {
                idisList = data.pId;
            } else {
                idisList = data.id;
            }
            topevery.BindTree("treeDemoTwo", "Common/SubjectKnowledgePointAndpaperInfoLists?KnowledgePointId=" + idisList + "&DifficultLevel=" + data.DLevel + "&PaperId=" + PaperId + "&DetailId=" + data.DetailId, "", "", "", SetTreeDemoTwoCheckIsTrue, "", true, zTreeOnCheck);
            break;
        case "2":
            //先保存已选择的项
            GetTreeDemoTwoCheckIsTrue();
            //绑定试题树
            topevery.BindTree("treeDemoTwo", "Common/SubjectKnowledgePointAndpaperInfoLists?KnowledgePointId=" + data.KIdList + "&DifficultLevel=" + data.DLevel + "&PaperId=" + PaperId + "&DetailId=" + data.DetailId, "", "", "", SetTreeDemoTwoCheckIsTrue, "", true, zTreeOnCheck);

            break;
        //case "99":
        //    $("#btndel").show();
        //    break;
        default:
            $.fn.zTree.init($("#treeDemoTwo"), null, null);
            break;
    }



}
function delNode(data) {
    
}
function zTreeOnCheck(event, treeId, treeNode) {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo");
    var nodes = treeObj.getCheckedNodes(true);
    for (var i = 0; i < nodes.length; i++) {
        if (!BigQuestionIdList.contain(nodes[i].id) && nodes[i].type == "6") {
            BigQuestionIdList.push(nodes[i].id);
            AddList.push({
                aid: nodes[i].Code,
                aname: nodes[i].name,
                apid: "",
                KIdList: nodes[i].id
            });
        }
    }
    if (!treeNode.checked) {
        if (treeNode.type == "6") {
            //删除数组元素
            BigQuestionIdList.splice($.inArray(treeNode.id, BigQuestionIdList), 1);
            for (var i = 0; i < AddList.length; i++) {
                if (treeNode.name == AddList[i].aname) {
                    AddList.splice($.inArray(AddList[i], AddList), 1);
                }
            }
        }
        else {
            var chenodes = treeObj.getNodesByParam("type", "6", null);
            for (var i = 0; i < chenodes.length; i++) {
                if (!chenodes[i].checked) {
                    BigQuestionIdList.splice($.inArray(chenodes[i].id, BigQuestionIdList), 1);
                    for (var j = 0; j < AddList.length; j++) {
                        if (chenodes[i].name == AddList[j].aname) {
                            AddList.splice($.inArray(AddList[j], AddList), 1);
                        }
                    }
                 
                }
            }
        }
    }
    //将勾选的节点添加到左边节点
    TreeAddModel(AddList);
};
Array.prototype.removeByValue = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            this.splice(i, 1);
            break;
        }
    }
}

//获得已选中的节点
function GetTreeDemoTwoCheckIsTrue() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo");
    if (treeObj != null) {
        var nodes = treeObj.getCheckedNodes(true);
        if (nodes.length > 0) {
            for (var i = 0; i < nodes.length; i++) {
                if (!BigQuestionIdList.contain(nodes[i].id) && nodes[i].type == "6") {
                    BigQuestionIdList.push(nodes[i].id);
                    AddList.push({
                        aid: nodes[i].Code,
                        aname: nodes[i].name,
                        apid: "",
                        KIdList: nodes[i].id
                    });
                }
            }

        }
    }
}

function SetTreeDemoTwoCheckIsTrue() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    var chenodes = treeObj.getNodesByParam("type", "99", null);
    for (var i = 0; i < chenodes.length; i++) {
        var treeObjTwo = $.fn.zTree.getZTreeObj("treeDemoTwo");
        var chenodesTwo = treeObjTwo.getNodesByParam("type", "6", null);
        for (var j = 0; j < chenodesTwo.length; j++) {
            if (chenodes[i].KIdList == chenodesTwo[j].id) {
                treeObjTwo.checkNode(chenodesTwo[j], true, true);
            }
        }
    }
}

function TreeAddModel(AddList) {
    //1、获取zTree对象  
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    var chenodes = treeObj.getNodesByParam("type", "99", null);
    if (chenodes.length > 0) {
        for (var i = 0; i < chenodes.length; i++) {
            treeObj.removeNode(chenodes[i]);
        }
    }
    //遍历已勾选的节点 PDetailId
    var listss = [];
    for (var i = 0; i < AddList.length; i++) {
        var nodes = treeObj.getNodesByParam("OneCode", AddList[i].aid, null);
        if (nodes.length > 0) {
            //先删后写
            for (var j = 0; j < nodes.length; j++) {
                if (!listss.contain(AddList[i].aid + AddList[i].aname)) {
                    listss.push(AddList[i].aid + AddList[i].aname);
                    var newNode = {
                        id: AddList[i].aid,
                        name: AddList[i].aname,
                        pId: AddList[i].apid,
                        type: "99",
                        icon: "../Img/2.jpg",
                        KIdList: AddList[i].KIdList,
                    };
                    treeObj.addNodes(nodes[0], newNode);
                }
            }
        }

      
    }
    AddList = [];
}

//文本框是否为空
function inputIsnull() {
    var inputText = [
        $("#PaperParamId").val(),
        $("#PaperName").val(),
        $("#TestTime").val()

    ];
    var isTrue = true;
    for (var i = 0; i < inputText.length; i++) {
        if (inputText[i] == "") {
            isTrue = false;
            break;
        }
    }
    return isTrue;
}


function ChildrenList(childrens) {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    if (childrens.length > 0) {
        for (var i = 0; i < childrens.length; i++) {
            if (childrens[i].type == "99") {
                treeObj.removeNode(childrens[t]);
            }
            ChildrenList(ChildrenList[i].children);
        }
    }
}

Array.prototype.contain = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            return true;
        }
    }
    return false;
};

function bindtree() {
    var groupType = $("#groupType").val();
    var paperId = $("#paperId").val();
    if (groupType == 1) {
        topevery.ajax({
            url: "api/PaperInfo/GetInfo",
            data: JSON.stringify({ id: paperId })
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                $("#PaperName").val(row.PaperName);
                $("#TestTime").val(row.TestTime);
                $("#PaperParamId").val(row.PaperParamId);
                $("#PaperParamId").attr("disabled", "disabled");
                $("#QuizpaperType").val(row.Type);
                $("#QuizpaperType").attr("disabled", "disabled");
            }
        });
    }
    //刷新树
    setTimeout(function () {
        var datas = new Object();
        datas.PaperParamId = $("#PaperParamId").val();
        datas.GroupType = $("#groupType").val();//操作类型
        datas.PaperId = $("#paperId").val();//试卷id
        BindDetailTree("treeDemo", "api/PaperParamDetail/PaperTreeList", onClickStructureTree, "", true, "", "", datas);
        SetTreeDemoTwoCheckIsTrue();
    }, 500);

 
}

//节点字符太长的清空下截图
function addDiyDomWith(treeId, treeNode) {
    var spaceWidth = 5;
    var switchObj = $("#" + treeNode.tId + "_switch"),
    icoObj = $("#" + treeNode.tId + "_ico");
    switchObj.remove();
    icoObj.parent().before(switchObj);
    var spantxt = $("#" + treeNode.tId + "_span").html();
    $("#" + treeNode.tId + "_span").css({ "fontSize": 11 });
    $("#" + treeNode.tId + "_span").attr("data-toggle", "tooltip");
    $("#" + treeNode.tId + "_span").attr("data-placement", "top");
    $("#" + treeNode.tId + "_span").attr("title", spantxt);
    if (spantxt.length > 20) {
        spantxt = spantxt.substring(0, 20) + "...";
        $("#" + treeNode.tId + "_span").html(spantxt);
    }
}

//有复选框的情况下
function addDiyDomWithCheck(treeId, treeNode) {
    var spaceWidth = 5;
    var switchObj = $("#" + treeNode.tId + "_switch"),
    checkObj = $("#" + treeNode.tId + "_check"),
    icoObj = $("#" + treeNode.tId + "_ico");
    switchObj.remove();
    checkObj.remove();
    icoObj.parent().before(switchObj);
    icoObj.parent().before(checkObj);

    var spantxt = $("#" + treeNode.tId + "_span").html();
    $("#" + treeNode.tId + "_span").css({ "fontSize": 11 });
    $("#" + treeNode.tId + "_span").attr("data-toggle", "tooltip");
    $("#" + treeNode.tId + "_span").attr("data-placement", "top");
    $("#" + treeNode.tId + "_span").attr("title", spantxt);
    if (spantxt.length > 20) {
        spantxt = spantxt.substring(0, 20) + "...";
        $("#" + treeNode.tId + "_span").html(spantxt);
    }

}