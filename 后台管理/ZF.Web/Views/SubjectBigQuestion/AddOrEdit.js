var Option1 = UE.getEditor('Option1', { enterTag: 'br' });
var Option2 = UE.getEditor('Option2', { enterTag: 'br' });
var Option3 = UE.getEditor('Option3', { enterTag: 'br' });
var Option4 = UE.getEditor('Option4', { enterTag: 'br' });
var Option5 = UE.getEditor('Option5', { enterTag: 'br' });
var Option6 = UE.getEditor('Option6', { enterTag: 'br' });
var Option7 = UE.getEditor('Option7', { enterTag: 'br' });
var Option8 = UE.getEditor('Option8', { enterTag: 'br' });
var QuestionContent = UE.getEditor('QuestionContent', { enterTag: 'br' });
var QuestionTextAnalysis = UE.getEditor('QuestionTextAnalysis', { enterTag: 'br' });
//初始化
$(function () {
    var addorEditUrl = 'api/SubjectBigQuestion/AddOrEdit';
    topevery.BindSelect("SubjectType", "Common/QuestionType", "", DifficultLevelList);

    function DifficultLevelList() {
        topevery.BindSelect("DifficultLevel", "Common/DifficultLevelList", "", QuestionState);
    }
    function QuestionState() {
        topevery.BindSelect("State", "Common/QuestionState", "", Initialize);
    }
    function Initialize() {
        ///初始化From表单以及验证信息
        if ($("#Id").val()) {
            topevery.ajax({
                url: "api/SubjectBigQuestion/GetOne",
                data: JSON.stringify({ id: $("#Id").val() })
            }, function (data) {
                if (data.Success) {
                    var row = data.Result;
                    topevery.BindSelect("SubjectClassId", "Common/SubjectClassList?SubjectId=" + row.SubjectId, "", function () {
                        topevery.setParmByLookForm(row);
                        subjectTypeNumberChange(row.SubjectType, row.Number);
                        var rightAnswerData = row.RightAnswer.split(',');
                        for (var i = 0; i < rightAnswerData.length ; i++) {
                            $("input:checkbox[value='" + rightAnswerData[i] + "']").attr('checked', 'true');
                        }
                        Option1.ready(function () {
                            Option1.setContent(row.Option1);
                        });
                        Option2.ready(function () {
                            Option2.setContent(row.Option2);
                        });
                        Option3.ready(function () {
                            Option3.setContent(row.Option3);
                        });
                        Option4.ready(function () {
                            Option4.setContent(row.Option4);
                        });
                        Option5.ready(function () {
                            Option5.setContent(row.Option5);
                        });
                        Option6.ready(function () {
                            Option6.setContent(row.Option6);
                        });
                        Option7.ready(function () {
                            Option7.setContent(row.Option7);
                        });
                        Option8.ready(function () {
                            Option8.setContent(row.Option8);
                        });
                        QuestionContent.ready(function () {
                            QuestionContent.setContent(row.QuestionContent);
                        });
                        QuestionTextAnalysis.ready(function () {
                            QuestionTextAnalysis.setContent(row.QuestionTextAnalysis);
                        });
                        //if (row.SubjectType===7) {
                        //    $("#SubjectType").attr("disabled", "disabled");
                        //}
                        //if (row.VideoId) {
                        //    $('#VideoId').val(row.VideoId);
                        //}
                        //if (row.VideoIdName) {
                        //    $('#VideoIdName').val(row.VideoIdName);
                        //}
                    });
                }
            });
        } else {
            $(".cs56,.cs78").hide();
            topevery.BindSelect("SubjectClassId", "Common/SubjectClassList?SubjectId=" + $("#SubjectId").val(), "");
            subjectTypeNumberChange(1, 4);
        }
    }

    $("#Number,#SubjectType").change(function () {
        subjectTypeNumberChange($("#SubjectType").val(), $("#Number").val());
    });


    $("#SubjectClassId").change(function () {
        if ($(this).val()) {
            topevery.ajax({
                url: "api/SubjectClass/GetOneId",
                data: JSON.stringify({ id: $(this).val() })
            }, function (data) {
                if (data.Success) {
                    $("#SubjectType").val(data.Result);
                    subjectTypeNumberChange($("#SubjectType").val(), $("#Number").val());
                }
            });
        } else {
            $("#SubjectType").val("");
            subjectTypeNumberChange($("#SubjectType").val(), $("#Number").val());
        }

    });

    //通过试题类型 试题数量改变页面展示的选项数量 以及答案内容   cs12  选项12 cs3选项3 其他同理  cs0试题数量  cs9正确答案
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once InconsistentNaming
    function subjectTypeNumberChange(SubjectType, Number) {
        var html = "";
        var subjectType = parseInt(SubjectType);
        var number = parseInt(Number);
        if (number === 2) {
            $(".cs34,.cs56,.cs78").hide();
            $(".cs12").show();
        } else if (number === 3) {
            $(".cs12,.cs3").show();
            $(".cs4,.cs56,.cs78").hide();
        } else if (number === 4) {
            $(".cs12,.cs34").show();
            $(".cs56,.cs78").hide();
        } else if (number === 5) {
            $(".cs12,.cs34,.cs5").show();
            $(".cs6,.cs78").hide();
        } else if (number === 6) {
            $(".cs12,.cs34,.cs56").show();
            $(".cs78").hide();
        } else if (number === 7) {
            $(".cs12,.cs34,.cs56,.cs7").show();
            $(".cs8").hide();
        } else if (number === 8) {
            $(".cs12,.cs34,.cs56,.cs78").show();
        }
        var abc = ["A", "B", "C", "D", "E", "F", "G", "H"];
        if (subjectType === 1) {
            $(".cs0,.cs9").show();
            for (var i = 1; i < number + 1; i++) {
                html += " <input type=\"checkbox\" Id=\"RightAnswer" + i + "\" value=\"" + i + "\" name=\"RightAnswer\"><label for=\"RightAnswer" + i + "\">" + abc[i - 1] + "</label>";
            }
            $("#RightAnswerHtml").html(html);
            $(':checkbox[type="checkbox"]').each(function () {
                $(this).bind("click", function () {
                    if ($(this).attr('checked')) {
                        $(':checkbox[type="checkbox"]').removeAttr('checked');
                        $(this).attr('checked', 'checked');
                    }
                });
            });
        } else if (subjectType === 2) {
            $(".cs0,.cs9").show();
            for (var i = 1; i < number + 1; i++) {
                html += " <input type=\"checkbox\" Id=\"RightAnswer" + i + "\" value=\"" + i + "\" name=\"RightAnswer\"><label for=\"RightAnswer" + i + "\">" + abc[i - 1] + "</label>";
            }
            $("#RightAnswerHtml").html(html);
        } else if (subjectType === 3) {
            $(".cs0,.cs12,.cs34,.cs56,.cs78").hide();
            $(".cs9").show();
            html = " <input type=\"checkbox\" Id=\"RightAnswer1\" value=\"1\" name=\"RightAnswer\"><label for=\"RightAnswer1\">正确</label> " +
                "<input type=\"checkbox\" Id=\"RightAnswer2\" value=\"2\" name=\"RightAnswer\"><label for=\"RightAnswer2\">错误</label>";
            $("#RightAnswerHtml").html(html);
            $(':checkbox[type="checkbox"]').each(function () {
                $(this).bind("click", function () {
                    if ($(this).attr('checked')) {
                        $(':checkbox[type="checkbox"]').removeAttr('checked');
                        $(this).attr('checked', 'checked');
                    }
                });
            });
        } else if (subjectType === 7) {
            $(".cs0,.cs12,.cs34,.cs56,.cs78,.cs9").hide();
        }
    }


    $("#chooseVideo").click(function () {
        var addUrl = "Video/Choose";
        topevery.ajax({ type: "get", url: addUrl, dataType: "html" }, function (data) {
            layer.open({
                type: 1,
                title: "选择视频",
                skin: 'layui-layer-rim', //加上边框
                area: [950 + 'px', 650 + 'px'], //宽高
                content: data
            });
        }, true);
    });

    /**
     * 绑定验证规则
     */
    $('#sumbitForm').bootstrapValidatorAndSumbit(addorEditUrl, {
        QuestionTitle: { validators: { notEmpty: { message: '试题标题不能为空!' } } },
        SubjectType: { validators: { notEmpty: { message: '试题类型不能为空!' } } },
        State: { validators: { notEmpty: { message: '试题状态不能为空!' } } },
        SubjectClassId: { validators: { notEmpty: { message: '试题分类不能为空!' } } },
        DifficultLevel: { validators: { notEmpty: { message: '难度等级不能为空!' } } }
    }, function () {
        $(".btn-primary").removeAttr("disabled");
        if (!QuestionContent.getContent()) {
            topevery.msg("试题内容不能为空!", 2);
            return false;
        }
        var subjectType = $("#SubjectType").val();
        var number = $("#Number").val();
        if (subjectType === "1" || subjectType === "2") {
            switch (number) {
                case "2":
                    if (!validation12())
                        return false;
                    break;
                case "3":
                    if (!validation3())
                        return false;
                    break;
                case "4":
                    if (!validation4())
                        return false;
                    break;
                case "5":
                    if (!validation5())
                        return false;
                    break;
                case "6":
                    if (!validation6())
                        return false;
                    break;
                case "7":
                    if (!validation7())
                        return false;
                    break;
                case "8":
                    if (!validation8())
                        return false;
                    break;
                default:
            }
            if ($("input:checkbox:checked").map(function () { return $(this).val(); }).get().join(",").length === 0) {
                topevery.msg("请设置正确答案!", 2);
                return false;
            }
        } else if (subjectType === "3") {
            if ($("input:checkbox:checked").map(function () { return $(this).val(); }).get().join(",").length === 0) {
                topevery.msg("请设置正确答案!", 2);
                return false;
            }
        }
        return true;
    },
    {
        RightAnswer: '$("input:checkbox:checked").map(function () { return $(this).val(); }).get().join(",")',
        Option1: 'Option1.getContent()',
        Option2: 'Option2.getContent()',
        Option3: 'Option3.getContent()',
        Option4: 'Option4.getContent()',
        Option5: 'Option5.getContent()',
        Option6: 'Option6.getContent()',
        Option7: 'Option7.getContent()',
        Option8: 'Option8.getContent()',
        QuestionContent: 'QuestionContent.getContent()',
        QuestionTextAnalysis: 'QuestionTextAnalysis.getContent()',
        QuestionAudioAnalysis: '$("input[name=\'FileQuestionAudioAnalysishiddenFile\']").val()',
        QuestionVedioAnalysis: '$("input[name=\'FileQuestionVedioAnalysishiddenFile\']").val()'
    }, function () {
        if (!$("#Id").val()) {
            layer.msg("添加成功,可以继续添加下一个小题！", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
            $(".query_btn").click();
            topevery.setParmByLookForm({
                SubjectClassId: $("#SubjectClassId").val(),
                DifficultLevel: $("#DifficultLevel").val(),
                SubjectType: $("#SubjectType").val(),
                State: $("#State").val(),
                Number: $("#Number").val(),
                SubjectId: $("#SubjectId").val(),
                KnowledgePointId: $("#KnowledgePointId").val()
            }, "sumbitForm");
            $(':checkbox[type="checkbox"]').each(function () {
                $(':checkbox[type="checkbox"]').removeAttr('checked');
            });
            Option1.ready(function () {
                Option1.setContent("");
            });
            Option2.ready(function () {
                Option2.setContent("");
            });
            Option3.ready(function () {
                Option3.setContent("");
            });
            Option4.ready(function () {
                Option4.setContent("");
            });
            Option5.ready(function () {
                Option5.setContent("");
            });
            Option6.ready(function () {
                Option6.setContent("");
            });
            Option7.ready(function () {
                Option7.setContent("");
            });
            Option8.ready(function () {
                Option8.setContent("");
            });
            QuestionContent.ready(function () {
                QuestionContent.setContent("");
            });
            QuestionTextAnalysis.ready(function () {
                QuestionTextAnalysis.setContent("");
            });
            $($(".uploadImg")[0]).html("");
            $($(".uploadImg")[1]).html("");
            $("input[name='FileQuestionVedioAnalysishiddenFile']").val("");
            $("input[name='FileQuestionAudioAnalysishiddenFile']").val("");
        } else {
            $(".layui-layer-close").click();
            //$(".query_btn").click();
            reload();
            layer.msg("修改成功!", {
                icon: 1,
                title: false, //不显示标题
                offset: 'auto',
                time: 3000, //10秒后自动关闭
                anim: 5
            });
        }
    });

    function reload() {
        var page = $('#tblData').getGridParam('page'); // current page
        $("#tblData").jqGrid('setGridParam', {
            url: "api/SubjectBigQuestion/GetList", page: page, postData: topevery.form2Json("selectFrom")
        }).trigger("reloadGrid");
    }

    function validation12() {
        if (!Option1.getContent()) {
            topevery.msg("A选项不能为空!", 2);
            return false;
        }
        if (!Option2.getContent()) {
            topevery.msg("B选项不能为空!", 2);
            return false;
        }
        return true;
    };
    function validation3() {
        if (validation12()) {
            if (!Option3.getContent()) {
                topevery.msg("C选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
    function validation4() {
        if (validation3()) {
            if (!Option4.getContent()) {
                topevery.msg("D选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
    function validation5() {
        if (validation4()) {
            if (!Option5.getContent()) {
                topevery.msg("E选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
    function validation6() {
        if (validation5()) {
            if (!Option6.getContent()) {
                topevery.msg("F选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
    function validation7() {
        if (validation6()) {
            if (!Option7.getContent()) {
                topevery.msg("G选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
    function validation8() {
        if (validation7()) {
            if (!Option8.getContent()) {
                topevery.msg("H选项不能为空!", 2);
                return false;
            }
        } else {
            return false;
        }
        return true;
    };
});
