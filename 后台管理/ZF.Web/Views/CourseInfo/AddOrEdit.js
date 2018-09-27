var CourseContent = UE.getEditor('CourseContent');
///初始化
$(function () {
    var addorEditUrl = 'api/CourseInfo/AddOrEdit';
    //topevery.BindSelect("TeacherId", "Common/TeacherList", "--请选择--", Initialize);

    //topevery.BindSelect("CourseAttribute", "Common/DataDictionary?dataTypeId=beb0364f-8958-49f7-916f-1f5019354053", '--请选择--', SelectTeacherList);

    topevery.BindSelect("DiscountCardId", "Common/GetDiscountCardList", '--请选择--', SelectLinkCourseList);


    var selectCourseTag = [];
    var selectedCourseTag = [];
    var keyword = [];

    function SelectLinkCourseList() {
        topevery.ajax({
            url: "Common/SelectLinkCourseList?id=" + $("#Id").val(),
            data: JSON.stringify({})
        }, function (data) {
            $("#LinkCourse").select2({
                data: data,
                multiple: true,
                placeholder: {
                    id: '-1',
                    text: '请选择'
                },
                dropdownParent: $(".box"),
                allowClear: true
            })
            $('#LinkCourse').val('-1').trigger('change');
            SelectTeacherList();
        });
    }

    function SelectTeacherList() {
        topevery.ajax({
            url: "Common/SelectTeacherList",
            data: JSON.stringify({})
        }, function (data) {
            $("#TeacherId").select2({
                data: data,
                multiple: true,
                placeholder: {
                    id: '-1',
                    text: '请选择'
                },
                dropdownParent: $(".box"),
                allowClear: true
            })
            $('#TeacherId').val('-1').trigger('change');
            Initialize();
        });
    }
    function Initialize() {
        bindTree("projectTreeDemo", "Common/ProjectTree", beforeClick, '', true, '', '', onCheck);
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/CourseInfo/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    $("#EmailNotes").prop("checked", row.EmailNotes == 1 ? true : false);
                    $("#IsValueAdded").prop("checked", row.IsValueAdded == 1 ? true : false);
                    if (row.FavourablePrice === 0) {
                        row.SetPrice = 0;
                        $(".edit-price").hide();
                    } else {
                        row.SetPrice = 1;
                    }
                    row.ValidityEndDate = topevery.dataTimeView(row.ValidityEndDate);
                    topevery.setParmByLookForm(row);
                    if (row.TeacherId) {
                        $('#TeacherId').val(row.TeacherId.split(',')).trigger("change");
                    }
                    if (row.LinkCourse) {
                        $('#LinkCourse').val(row.LinkCourse.split(',')).trigger("change");
                    }
                    $('#Discount').text(row.FavourablePrice == 0 ? 100 : (row.FavourablePrice / row.Price * 100).toFixed(1))
                    $('#Discount').val(row.FavourablePrice == 0 ? 100 : (row.FavourablePrice / row.Price * 100).toFixed(1))
                    CourseContent.ready(function () {
                        CourseContent.setContent(row.CourseContent);
                    });
                    if (row.CourseTag) {
                        selectedCourseTag = selectedCourseTag.concat(row.CourseTag.split(','));
                    }
                    if (row.Type === 1) {
                        $(".glkc").show();
                    }
                    //if (row.KeyWord) {
                    //    keyword = keyword.concat(row.KeyWord.split(','));
                    //    keyword.forEach(function (item, index) {
                    //        $(" <span class=\"tag\"><span class=\"text\">" + item + "</span><span class=\"delete\" title=\"删除\">x</span></span>").insertBefore($('.keyword-input'));
                    //    })
                    //    bindDeleteKeyWord();
                    //}
                    if (row.ClassId) {
                        var seletedArr = row.ClassId.split(',');
                        console.log('类目：', seletedArr);
                        bindTree("projectTreeDemo", "Common/ProjectTree", beforeClick, '', true, '', '', onCheck, seletedArr);
                    } else {
                        bindTree("projectTreeDemo", "Common/ProjectTree", beforeClick, '', true, '', '', onCheck);
                    }
                    bindTag();
                }
            });
        } else {
            bindTag();
        }

        $('form input>i').attr('style', "display:none;");
    }

    function bindTag() {
        topevery.ajax({
            url: 'api/BaseTag/GetSelectList',
            data: JSON.stringify({ ModelCode: 'Course' }),
        }, function (data) {
            if (data.Success) {
                $('.tag-wrap').html('');
                var content = "";
                var arr = data.Result;
                for (var i = 0; i < arr.length; i++) {
                    var index = $.inArray(arr[i].Id, selectedCourseTag);
                    if (index < 0) {
                        content += "<span class=\"tag used\">"
                                  + "<span class=\"text\" id=\"" + arr[i].Id + "\">" + arr[i].TagName + "</span>"
                                  + "<span class=\"delete\" title=\"删除\">x</span>"
                                  + "</span>";
                        selectCourseTag.push(arr[i].Id);
                    } else {
                        $("<span class=\"tag\"><span class=\"text\" id=\"" + arr[i].Id + "\">" + arr[i].TagName + "</span><span class=\"delete\" title=\"删除\">x</span></span>").insertBefore($(".text-layer>input"));
                    }
                }
                $('.tag-wrap').html(content);
                bindSelectTag();
                bindDeleteTag();
            }
        })
    }

    $("input[name='Type']").each(function () {
        $(this).click(function () {
            var id = $(this).attr('id');
            if (id === "kc") {
                $('#LinkCourse').val('-1').trigger('change');
                $(".glkc").hide();
            } else if (id === "tk") {
                $(".glkc").show();
            }
        });
    })

    $("input[name='SetPrice']").each(function () {
        $(this).click(function () {
            var id = $(this).attr('id');
            if (id == "J_free") {
                $('.edit-price').attr("style", "display:none;");
                $("#Price").val(0);
                $("#FavourablePrice").val(0);
                $("#Discount").val(0);
            } else if (id == "J_charge") {
                $('.edit-price').attr("style", "display:block;");
            }
        });
    })


    $('#FavourablePrice,#Price').keyup(function () {
        var finalprice = parseFloat($(this).val());
        var originalprice = parseFloat($("#Price").val());
        $("#Discount").val(100);
        if ($.trim(originalprice).length == 0) {
            $('#SetPrice >.error').attr("style", "margin-top:0px;display:block;");
            $('#SetPrice >.error').text("请输入有效数字");
            return;
        }
        if (finalprice > originalprice) {
            $('#SetPrice >.error').attr("style", "margin-top:0px;display:block;");
            $('#SetPrice >.error').text("实际卖价不能大于原价");
            return;
        } else if (Number.isNaN(finalprice)) {
            $("#Discount").val(100);
            $('#SetPrice >.error').attr("style", "margin-top:0px;display:block;");
            $('#SetPrice >.error').text("请输入有效数字");
            return;
        } else {
            $('#SetPrice >.error').attr("style", "margin-top:0px;display:none;");
        }
        var discount = (finalprice / originalprice * 100).toFixed(1);
        $('#Discount').val(discount)
        $('#Discount').text(discount);
    });

    function bindDeleteTag() {
        //删除标签
        $('.text-layer>.tag>.delete').on("click", function () {
            var parent = $(this).parent();
            var grandparent = $(parent).parent();
            var id = $(this).prev().attr('id');
            removeItem(selectedCourseTag, id);
            selectCourseTag.push(id);

            $(".tag-wrap,.tag-layer").attr("style", "display:block;");
            console.log($(parent).addClass("used"));
            $(".tag-wrap").append($(parent).addClass("used"));
            $('#CourseTag').val(selectedCourseTag.unique().join(',').trimRight(','));
            $(grandparent).find($(parent)).remove();
            bindSelectTag();
        })

    }



    function bindSelectTag() {
        //从已有标签选择
        $('.tag-wrap>.used>.text').on("click", function () {
            if (selectedCourseTag.unique().length > 5) {
                layer.msg("超过指定数量");
            } else {
                var parent = $(this).parent();
                var id = $(this).attr('id');
                var grandparent = $(parent).parent();
                $(grandparent).find($(parent)).remove();
                //$('.text-layer').append($(parent).removeClass('used'));
                $($(parent).removeClass('used')).insertBefore($(".text-layer>input"));
                selectedCourseTag.push(id);
                removeItem(selectCourseTag, id);
                $('#CourseTag').val(selectedCourseTag.unique().join(',').trimRight(','));
                var content = $.trim($(grandparent).html());
                if (content == "") {
                    $($(grandparent).parent()).attr("style", "display:none;");
                }
                //重新绑定事件
                bindDeleteTag();
            }

        })
    }

    $(".text-layer>input").keyup(function (event) {
        var value = $.trim($(this).val());
        if (value != "") {
            if (event.keyCode == 13) {
                topevery.ajax({
                    url: "api/BaseTag/AddOrEdit",
                    data: JSON.stringify({ ModelCode: 'Course', TagName: value })
                }, function (data) {
                    if (data.Result.Success) {
                        selectedCourseTag.push(data.Result.ModelId);
                        $("#CourseTag").val(selectedCourseTag.unique().join(',').trimRight(','));
                        $(" <span class=\"tag\"><span class=\"text\">" + value + "</span><span class=\"delete\" title=\"删除\">x</span></span>").insertBefore($('.text-layer>input'));
                        $('.text-layer>input').val('');
                        bindDeleteTag();
                    } else {
                        $(this).val('');
                        layer.alert("该标签已存在，请直接在下方选择！");

                    }
                })
                return false;
            }
        }
    })

    //禁用Enter键表单自动提交  
    document.onkeydown = function (event) {
        var target, code, tag;
        if (!event) {
            event = window.event; //针对ie浏览器  
            target = event.srcElement;
            code = event.keyCode;
            if (code == 13) {
                tag = target.tagName;
                if (tag == "TEXTAREA") { return true; }
                else { return false; }
            }
        }
        else {
            target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
            code = event.keyCode;
            if (code == 13) {
                tag = target.tagName;
                if (tag == "INPUT") { return false; }
                else { return true; }
            }
        }
    };

    ////关键字删除绑定
    //function bindDeleteKeyWord() {
    //    $(".keyword-wrap>.tag>.delete").each(function () {
    //        $(this).on("click", function () {
    //            var parent = $(this).parent();
    //            var value = $(this).prev().text();
    //            console.log('pop', value);
    //            //keyword.pop(value);
    //            removeItem(keyword, value);
    //            $('#KeyWord').val(keyword.join(',').trimRight(','));
    //            var grandparent = $(parent).parent();
    //            $(grandparent).find($(parent)).remove();
    //        })
    //    })
    //}



    //$(".keyword-input").keyup(function (event) {
    //    if ($(".keyword-wrap>.tag").length > 10) {
    //        $('.seo-error').attr("style", "color:#FF3333; display: block; font-weight: bold;line-height: 1em;margin-top: 10px;");
    //        event.preventDefault();
    //        return;
    //    }
    //    var value = $.trim($(this).val());
    //    if (value != "") {
    //        if (value.length > 7) {
    //            $(this).val(value.substr(0, 7));
    //        }
    //        if (event.keyCode == 32 || event.keyCode == 9) {
    //            $(" <span class=\"tag\"><span class=\"text\">" + value + "</span><span class=\"delete\" title=\"删除\">x</span></span>").insertBefore($('.keyword-input'));
    //            $('.keyword-input').val('');
    //            keyword.push(value);
    //            $('#KeyWord').val(keyword.join(',').trimRight(','));
    //            bindDeleteKeyWord();
    //        }
    //    }
    //})

    $(".title-input").keyup(function () {
        var len = $('.title-input').val().length;
        if (50 - len >= 0) {
            $(".title_used").text(len);
        } else {
            $(".title-input").val($(".title-input").val().substring(0, 50));
        }
    });
    $(".describe-input").keyup(function () {
        var len = $('.describe-input').val().length;
        if (100 - len >= 0) {
            $(".describe-used").text(len);
        } else {
            $(".describe-input").val($(".describe-input").val().substring(0, 100));
        }
    })

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        CourseName: { validators: { notEmpty: { message: '课程名称不能为空!' } } },
        //Price:{validators:{numeric:{message:"请输入有效数字"}}},
        //FavourablePrice:{validators:{numeric:{message:"请输入有效数字"}}},
        ValidityEndDate: { validators: { notEmpty: { message: '请输入有效截止日期!' } } },
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if ($("input[name='IdFilehiddenFile']").val() == "") {
            topevery.msg("课程封面不能为空!", 2);
            return false;
        } else {
            if ($("input[name='IdFilehiddenFile']").val().split(',').length > 1) {
                topevery.msg("课程封面只能上传一张图片!", 2);
                return false;
            }
        }
        if ($.trim(CourseContent.getContent()) == "") {
            topevery.msg("请输入课程简介", 2);
            $('.btn-primary').removeAttr('disabled');
            return false;
        }
        if ($("input[name='Type']:checked").val() === "1" && !$('#LinkCourse').val()) {
            topevery.msg("请选择关联课程", 2);
            $('.btn-primary').removeAttr('disabled');
            return false;
        }

        if ($("input[name='CourseVediohiddenFile']").val() === "") {
            //topevery.msg("课程简介视频不能为空!", 2);
            //return false;
        } else {
            if ($("input[name='CourseVediohiddenFile']").val().split(',').length > 1) {
                topevery.msg("课程简介视频只能上传一份!", 2);
                return false;
            }
        }

        var price = $('#Price').val();
        var favourableprice = $('#FavourablePrice').val();
        var val = $('input[name="SetPrice"]:checked').val();
        if (val == "1") {//收费
            if (!/^[1-9]\d*\,\d*|[1-9]\d*$/.test(price) || !/^[1-9]\d*\,\d*|[1-9]\d*$/.test(favourableprice)) {
                topevery.msg("请输入有效数字", 2);
                return false;
            }
            if (parseFloat(favourableprice) > parseFloat(price)) {
                topevery.msg("课程实际价格应小于原价", 2);
                $(this).focus();
                $('.btn-primary').removeAttr('disabled');
                return false;
            }
        }
        return true;
    }, { IdFilehiddenFile: '$("input[name=\'IdFilehiddenFile\']").val()', CourseVediohiddenFile: '$("input[name=\'CourseVediohiddenFile\']").val()' }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个课程！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({}, "sumbitForm");
            $('#TeacherId').val('-1').trigger('change');
            $('#LinkCourse').val('-1').trigger('change');
            $('#SubjectId').val($('#subjectId_copy').val());
            $($(".uploadImg")[0]).html("");
            $($(".uploadImg")[1]).html("");
            $("input[name='IdFilehiddenFile']").val("");
            $("input[name='CourseVediohiddenFile']").val("");
            CourseContent.setContent('');
            bindTree("projectTreeDemo", "Common/ProjectTree", beforeClick, '', true, '', '', onCheck);
        } else {
            //$(".layui-layer-close").click();
            $(".query_btn").click();
            layer.msg("修改成功!", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        }
    }, "sumbitForm");


    $('#EmailNotes').change(function () {
        console.log($(this));
        if ($(this).is(":checked")) {
            $(this).val("1");
        } else {
            $(this).val('0');
        }
    })

    $('#Price,#FavourablePrice').on("keydown keyup", function () {
        if (isFloat(this.value)) {
            if (this.value.split('.')[1].length > 2) {
                $(this).val(this.value.split('.')[0] + "." + this.value.split('.')[1].substr(0, 2));
            }
        }

    });

    $('#return').click(function () {
        topevery.ajax({ url: "CourseInfo/Index", type: 'get', dataType: 'html' }, function (data) {
            $('.content-wrapper').html(data);
        });
    });


    function isFloat(oNum)//判断是否为浮点数的函数
    {
        if (!oNum)
            return false;
        var strP = /^\d+\.\d+$/;
        if (!strP.test(oNum))
            return false;
        try {
            if (parseFloat(oNum) != oNum)
                return false;
        } catch (ex) {
            return false;
        }
        return true;
    }


    Array.prototype.unique = function () {
        var res = [];
        var json = {};
        for (var i = 0; i < this.length; i++) {
            if (!json[this[i]]) {
                res.push(this[i]);
                json[this[i]] = 1;
            }
        }
        return res;
    }



});
function removeItem(arr, item) {
    for (var i = 0; i < arr.length;) {
        if (item == arr[i]) {
            arr.splice(i, 1);
        } else {
            i++;
        }
    }
    return arr;
}

var classArr = [];
function bindTree(btnid, url, callback, onClick, expandNode, callback1, l, checkCallback, selectedArr) {
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
        data: JSON.stringify({})
    }, function (data) {
        var setting = {
            showLine: true,
            checkable: true,
            sonSign: true,
            isSimpleData: true,
            enable: true,
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
                onCheck: checkCallback
            },
            check: {
                enable: true,
                chkboxType: { "Y": "s", "N": "s" }
            },
            view: {
                selectedMulti: false,
                txtSelectedEnable: true,
                showLine: true,
                addDiyDom: function () {
                    try {
                        addDiyDomWithCheck();
                    } catch (e) {
                    }
                }
            }

        };

        $.fn.zTree.init($("#" + btnid), setting, data);
        var treeObj = $.fn.zTree.getZTreeObj("" + btnid + "");
        var nodeList = treeObj.getNodesByParam("type", "1", null);
        if (nodeList) {
            if (selectedArr) {
                for (var i = 0; i < nodeList.length; i++) {
                    treeObj.expandNode(nodeList[i], false, false, true);
                    for (var j = 0; j < selectedArr.length; j++) {
                        if (selectedArr[j] == nodeList[i].id) {
                            //nodeList[i].checked = true;
                            treeObj.checkNode(nodeList[i], true, true);
                        }
                    }
                }
            }
        }
        if (expandNode) {
            var nodes = treeObj.getNodesByParam("type", "1", null);
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].icon = "../Img/2.jpg";
            }
            treeObj.expandAll(true);
        } else {
            topevery.showztreemenuNum(treeObj, true, "", l);
        }


        if (callback1 != undefined && callback1 !== "") {
            callback1();
        }
        setTimeout(function () { $("#" + btnid).show() }, 200);
    });
}


function beforeClick(data) {

}

function onCheck(event, treeId, treeNode) {
    if (treeNode.checked) {
        if (treeNode.type == "1") {
            classArr.push(treeNode.id);
        }
    } else {
        if (treeNode.type == "1") {
            removeItem(classArr, treeNode.id);
        }
    }
    $('#ClassId').val(classArr.join(',').trimRight(','));
}


function addDiyDomWith(treeId, treeNode) {
    //var spaceWidth = 5;
    //var switchObj = $("#" + treeNode.tId + "_switch"),
    //icoObj = $("#" + treeNode.tId + "_ico");
    //switchObj.remove();
    //icoObj.parent().before(switchObj);
    //var spantxt = $("#" + treeNode.tId + "_span").html();
    //$("#" + treeNode.tId + "_span").css({ "fontSize": 11 });
    //$("#" + treeNode.tId + "_span").attr("data-toggle", "tooltip");
    //$("#" + treeNode.tId + "_span").attr("data-placement", "top");
    //$("#" + treeNode.tId + "_span").attr("title", spantxt);
    //if (spantxt.length > 20) {
    //    spantxt = spantxt.substring(0, 20) + "...";
    //    $("#" + treeNode.tId + "_span").html(spantxt);
    //}
}