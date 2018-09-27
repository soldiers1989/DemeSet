var KnowledgePointIdList = [];
var KnowledgePointNameList = [];
$(function () {
    Initialize();
    $("#sumbit").on("click", function () {
        if (inputIsnull()) {
            var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
            var nodes = treeObj.getSelectedNodes();
            var data = new Object();
            data.Id = nodes[0].type == "1" ? "" : nodes[0].DetailId;
            data.PaperParamId = $("#Pid").val();
            data.PaperStuctureDetailId = nodes[0].PDetailId;
            data.KnowledgePointId = KnowledgePointIdList.join(",");
            data.QuestionCount = $("#QuestionCount").val();
            data.DifficultLevel = $("#DifficultLevel").val();
            data.QuestionScoreSum = $("#QuestionScoreSum").val();
            topevery.ajax({
                url: "api/PaperParamDetail/AddOrEdit",
                data: JSON.stringify(data)
            }, function (data) {
                var info = data.Result;
                if (info.Success) {
                    if (data.Result.Success) {
                        htmlReset();
                        layer.msg(data.Result.Message, {
                            icon: 1,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 5000,
                            anim: 5
                        });
                        Initialize();
                    }
                } else {
                    layer.msg(data.Result.Message, {
                        icon: 2,
                        title: false, //不显示标题
                        offset: 'auto',
                        time: 5000,
                        anim: 5
                    });
                }
            });

        } else {
            layer.alert("知识点编码,试题数量,试题分数不允许为空");
        }
    });

    $("#btndel").on("click", function () {
        var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
        var nodes = treeObj.getSelectedNodes();
        if (nodes[0].type == "2") {
            var data = new Object();
            data.Ids = nodes[0].DetailId;

            topevery.ajax({
                url: "api/PaperParamDetail/Delete",
                data: JSON.stringify(data)
            }, function (data) {
                htmlReset();
                layer.msg(data.Result.Message, {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 5000,
                    anim: 5
                });
                if (data.Result.Success) {
                    Initialize();
                }
            });
        }
    })

    $("#btnReset").on("click", function () {
        htmlReset();
    });
    //绑定难度等级
    topevery.BindSelect("DifficultLevel", "Common/DifficultLevelList", "");

    //删除部门
    $("#m_del").on("click", function () {
        removeTreeNode();
    });
});

function removeTreeNode(e) {

    var zTree = $.fn.zTree.getZTreeObj("treeDemo");
    var nodes = zTree.getSelectedNodes();;
    hideRMenu();
};


function Initialize() {
    ///初始化From表单以及验证信息
    if ($("#Pid").val()) {
        topevery.ajax({
            url: "api/PaperPaperParam/GetListById",
            data: JSON.stringify({ id: $("#Pid").val() })
        }, function (data) {
            if (data.Success) {
                var row = data.Result;
                //结构ID
                $("#PStructureId").val(row.Id);
                //科目ID
                $("#PSubjectId").val(row.StuctureId);
                //绑定树
                var datas = new Object();
                datas.StuctureId = $("#PStructureId").val();
                datas.PaperParamId = $("#Pid").val();
                //绑定结构明细
                BindDetailTree("treeDemo", "Common/ParamDetailTreeList", onClickStructureTree, "", "", "", "", datas);
                //绑定知识点
                // topevery.BindTree("treeDemoTwo", "Common/SubjectKnowledgePointListsByPaperParamId?subjectId=" + $("#PSubjectId").val() + "&PaperParamId=" + $("#Pid").val(), "", "", "", "", "", true, zTreeOnCheck);
                $("#treeDemoTwo").hide();
            }
        });
    }
}

function addDiyDomWithCheck() { }


////第四步：定义保存操作，通过键值对把编辑的数据传到后台,如下   layer.alert("请选择一条且只选择一条记录!");
function onClickStructureTree(data) {
    if (data.type === "2") {
        $("#btndel").show();
    } else {
        $("#btndel").hide();
    }

    switch (data.type) {
        case "1":
            $("#btndel").hide();
            $("#sumbit").show();
            $("#btnReset").show();
            break;
        case "2":
            $("#btndel").show();
            $("#sumbit").show();
            $("#btnReset").show();
            break;
        case "3":
            $("#btndel").hide();
            $("#sumbit").hide()
            $("#btnReset").hide();
            break;
        default:
            $("#btndel").hide();
            $("#sumbit").hide()
            $("#btnReset").hide();
            break;
    }
    htmlReset();
    if (data.type === "1" || data.type === "2") {
        $("#treeDemoTwo").show();
        //获取知识点
        if (data.KIdList != "") {
            var klist = data.KIdList.split(",");
            for (var i = 0; i < klist.length; i++) {
                var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo");
                if (treeObj != null) {
                    var nodes = treeObj.getNodesByParam("id", klist[i], null);
                    if (nodes.length > 0) {
                        if (!KnowledgePointIdList.contain(nodes[0].id)) {
                            KnowledgePointIdList.push(nodes[0].id);
                            KnowledgePointNameList.push(nodes[0].name);
                        }
                    }
                }
            }
            if (KnowledgePointNameList.length > 0) {
                $("#PaperStuctureDetailId").val(KnowledgePointNameList.join(","));
            }
        }
        $("#DifficultLevel").val(data.DLevel);
        $("#QuestionCount").val(data.QuestionCount);
        $("#QuestionScoreSum").val(data.QScoreSum);
    }
    //绑定知识点
    var detailid;
    if (data.type === "1") {
        topevery.BindTree("treeDemoTwo", "Common/SubjectKnowledgePointListsByPaperParamId?subjectId=" + $("#PSubjectId").val() + "&PaperParamId=" + $("#Pid").val() + "&structureDetailId=" + data.PDetailId + "&detailId=", "", "", "", "", "", true, zTreeOnCheck);
    } else if (data.type === "2") {
        topevery.BindTree("treeDemoTwo", "Common/SubjectKnowledgePointListsByPaperParamId?subjectId=" + $("#PSubjectId").val() + "&PaperParamId=" + $("#Pid").val() + "&structureDetailId=" + data.PDetailId + "&detailId=" + data.DetailId, "", "", "", "", "", true, zTreeOnCheck);
    }
    

}

function zTreeOnCheck(event, treeId, treeNode) {
    htmlReset();
    var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo");
    var nodes = treeObj.getCheckedNodes(true);
    for (var i = 0; i < nodes.length; i++) {
        if (!KnowledgePointIdList.contain(nodes[i].id) && nodes[i].type != "3") {
            KnowledgePointIdList.push(nodes[i].id);
            KnowledgePointNameList.push(nodes[i].name);
        }
    }
    if (KnowledgePointNameList.length > 0) {
        $("#PaperStuctureDetailId").val(KnowledgePointNameList.join(","));
    }
};

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
                onClick: onClick

            }
        };
        $.fn.zTree.init($("#" + btnid), setting, data);
        var treeObj = $.fn.zTree.getZTreeObj("" + btnid + "");
        if (expandNode) {
            treeObj.expandAll(true);
        } else {
            topevery.showztreemenuNum(treeObj, true, "", l);
        }
        var nodeList = treeObj.getNodesByParam("type", "1", null);
        if (nodeList) {
            for (var i = 0; i < nodeList.length; i++) {
                treeObj.expandNode(nodeList[i], false, false, true);
            }
        }

        if (callback1 != undefined && callback1 !== "") {
            callback1();
        }
    });
}

Array.prototype.contain = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            return true;
        }
    }
    return false;
};

function htmlReset() {
    KnowledgePointIdList = [];
    KnowledgePointNameList = [];
    $("#PaperStuctureDetailId").val("");
}

function inputIsnull() {
    var inputList = [
    $("#PaperStuctureDetailId").val(),
    $("#QuestionCount").val(),
    $("#QuestionScoreSum").val()
    ];
    var istrue = true;
    for (var i = 0; i < inputList.length; i++) {
        if (inputList[i] == "") {
            istrue = false;
            break;
        }
    }
    return istrue;
}

