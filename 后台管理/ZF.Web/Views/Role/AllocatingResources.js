$('#return_btn').on('click', function () {
    topevery.ajax({ url: "Role/Index", type: 'get', dataType: 'html' }, function (data) {
        $('.content-wrapper').html(data);
    });
});
topevery.BindTree("treeDemoTwo", "Common/MenuModuleTree", "", "", "", SetTreeDemoTwoCheckIsTrue, "", true, "");
$("#sumbit").click(function () {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo"),
         nodes = treeObj.getCheckedNodes(true),
         v = "";
    for (var i = 0; i < nodes.length; i++) {
        v += nodes[i].id + "," + nodes[i].level + ";";
    }
    topevery.ajax({
        url: "api/RoleMenu/AddOrEdit",
        data: JSON.stringify({
            "Ids": v, Id: $("#RoleId").val()
        })
    }, function (data) {
        if (data.Success) {
            if (data.Result.Success) {
                layer.msg("保存成功!", {
                    icon: 1,
                    title: false, //不显示标题
                    offset: 'auto',
                    time: 3000, //10秒后自动关闭
                    anim: 5
                });
            }
        }

    });
});
///绑定选中节点
function SetTreeDemoTwoCheckIsTrue() {
    topevery.ajax({
        url: "api/RoleMenu/GetList",
        data: JSON.stringify({
            RoleId: $("#RoleId").val()
        })
    }, function (data) {
        if (data.Success) {
            var treeObj = $.fn.zTree.getZTreeObj("treeDemoTwo");
            var chenodes = treeObj.getNodes();
            for (var j = 0; j < chenodes.length; j++) {
                for (var i = 0; i < data.Result.length; i++) {
                    if (data.Result[i].MenuId == chenodes[j].id) {
                        treeObj.checkNode(chenodes[j], true, true);
                    }
                }
            }
        }
    });
}



//有复选框的情况下
function addDiyDomWithCheck(treeId, treeNode) {
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