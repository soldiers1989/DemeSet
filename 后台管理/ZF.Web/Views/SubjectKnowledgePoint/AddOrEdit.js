
///初始化
$(function() {
    var addorEditUrl = 'api/SubjectKnowledgePoint/AddOrEdit';
    topevery.BindTree("treeDemo", "Common/SubjectKnowledgePointLists?subjectId=" + $("#SubjectId").val(), onClick, onClick, false, BindSelect,0);

    function BindSelect() {
        topevery.BindSelect("SubjectId", "Common/SubjectList", "--请选择--", Initialize);
    }

    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/SubjectKnowledgePoint/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function(data) {
                if (data.Success) {
                    var row = data.Result;
                    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    var node = treeObj.getNodeByParam("id", row.ParentId, null);
                    topevery.setParmByLookForm(row);
                    if (row.ParentName === "") {
                        $("#ParentName").val(row.SubjectName);
                    }
                    treeObj.selectNode(node);
                }
            });
        } else {
            var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
            var node = "";
            node = treeObj.getNodeByParam("id", $("#ParentId").val(), null);
            treeObj.selectNode(node);
        }
    }

    $('#sumbitForm').bootstrapValidator({
        message: '输入的值无效',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            ParentName: { validators: { notEmpty: { message: '上级知识点不能为空!' } } },
            KnowledgePointName: { validators: { notEmpty: { message: '知识点名称不能为空!' } } },
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var $form = $(e.target);
        var bv = $form.data('bootstrapValidator');
        if (bv.isValid()) {
            topevery.ajax({
                url: addorEditUrl,
                data: topevery.extend({}, topevery.serializeObject($("#sumbitForm")))
            }, function (data) {
                var message = "新增失败";
                var icon = 1;
                if (data.Success) {
                    message = data.Result.Message;
                    icon = data.Result.Success === true ? 1 : 2;
                    if (data.Result.Success) {
                        if (!$("#Id").val()) {
                            layer.msg("添加成功,可以继续添加下一个知识点！", {
                                icon: 1,
                                title: false, //不显示标题
                                offset: 'auto',
                                time: 3000, //10秒后自动关闭
                                anim: 5
                            });
                            $($("#tblData")).trigger("reloadGrid");
                            $("#KnowledgePointName").val("");
                            $("#sumbit").attr("disabled", "disabled");
                        } else {
                            $(".layui-layer-close").click();
                            $(".query_btn").click();
                            layer.msg("修改成功!", {
                                icon: 1,
                                title: false, //不显示标题
                                offset: 'auto',
                                time: 3000, //10秒后自动关闭
                                anim: 5
                            });
                        }
                    } else {
                        layer.msg(message, {
                            icon: icon,
                            title: false, //不显示标题
                            offset: 'auto',
                            time: 3000, //10秒后自动关闭
                            anim: 5
                        });
                    }
                }
            }
            );
        }

    });
});
function showMenu() {
    $("#menuContent").slideDown("fast");
    $("body").bind("mousedown", onBodyDown);
}
function hideMenu() {
    $("#menuContent").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown);
}
function onBodyDown(event) {
    if (!(event.target.id == "menuBtn" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        hideMenu();
    }
}
//树形菜单点击回调时间
function onClick(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
    nodes = zTree.getSelectedNodes(),
    v = "";
    nodes.sort(function compare(a, b) { return a.id - b.id; });
    for (var i = 0, l = nodes.length; i < l; i++) {
        v += nodes[i].name + ",";
    }
    if (v.length > 0) v = v.substring(0, v.length - 1);
    var cityObj = $("#ParentName");
    cityObj.attr("value", v);
    $("#ParentId").val(nodes[0].id);
}