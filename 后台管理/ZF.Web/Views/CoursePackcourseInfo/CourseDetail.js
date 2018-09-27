$(function () {
    var CourseInfo = {
        formatterState: function (state) {
            return state == 1 ? "已上架" : "未上架"
        },
        formatterPrice: function (price) {
            return '' + price + '￥';
        },
    }
    var dataType = false;
    

    packsuitlist();
  
   
   
});

function contentLink(cellValue, options, rowdata, action) {
    return "<span style=\"color:#00c0ef;cursor:pointer;\" data-id='" + rowdata.Id + "' onclick=\"deleteCourse('" + rowdata.Id + "')\">删除</span> "
            + "<span style=\"color:#00c0ef;cursor:pointer;\" data-id='" + rowdata.Id + "' onclick=\"editCourse('" + rowdata.Id + "')\">编辑</span> ";
}

function deleteCourse(id) {
    layer.confirm('确定删除?', { title: "删除子课程" }, function () {
        var url = "api/CourseSuitDetail/Delete";
        var getPostDataUrl = "api/CourseSuitDetail/GetList";
        topevery.ajax({
            url: url,
            data: JSON.stringify({ Ids: id}),
        }, function (data) {
            layer.alert("删除成功!");
            //$(".layui-layer-close").click();//关闭窗口
            $('#packCourseDetailData').jqGrid('setGridParam', {
                url: getPostDataUrl, page: 1, postData: topevery.form2Json("suitCourseSelectFrom")
            }).trigger("reloadGrid");

            $('#packCourseData').jqGrid('setGridParam', {
                url: 'api/CoursePackcourseInfo/GetList', page: 1, postData: topevery.form2Json("selectFrom")
            }).trigger("reloadGrid");
        });
    });
}


function packsuitlist() {
    var getPostDataUrl = "api/CourseSuitDetail/GetList";
    var grid = $("#packCourseDetailData");
    grid.jgridInit({
        url: getPostDataUrl,
        multiselect: false,
        colNames: ['编码', '科目编码', '科目名称', '课程名称', '有效期(天)', '课程时长(分钟)', '课件数', '排序', ''], //列头
        colModel: [
            { name: 'Id', index: 'Id', width: 50, align: "center", hidden: true },
            { name: 'SubjectId', width: 100, align: "center", hidden: true },
            { name: 'SubjectName', width: 80, align: "center" },
            { name: 'CourseName', width: 150, align: "center" },
            //{ name: 'CourseContent',  width: 100, align: "center" },
            //{ name: 'Price',  width: 40, align: "center",formatter:CourseInfo.formatterPrice },
            //{ name: 'FavourablePrice', width: 40, align: "center", formatter: CourseInfo.formatterPrice },
            {
                name: 'ValidityPeriod', width: 50, align: "center", formatter: function (value) {
                    if (value === 0) {
                        return "永久有效";
                    }
                    return value;
                }
            },
            //{ name: 'State', width: 50, align: "center",formatter:CourseInfo.formatterState },
            { name: 'CourseLongTime', width: 58, align: "center" },
            { name: 'CourseWareCount', width: 50, align: "center" },
            { name: 'OrderNo', width: 20, align: "center" },
            {
                name: '',
                width: 40,
                align: "center",
                formatter: contentLink
            }
        ],
        postData: topevery.form2Json("suitCourseSelectFrom"),
        pager: '#packCourseDetailpager',
        height: '520'
    });
}

function reload() {
    var grid = $("#packCourseDetailData");
    grid.jqGrid('setGridParam', {
        url: "api/CourseSuitDetail/GetList", page: 1, postData: topevery.form2Json("suitCourseSelectFrom")
    }).trigger("reloadGrid");
}

var editLayer;
function editCourse(id) {
    var url = "CoursePackcourseInfo/EditSubCourse?id=" + id;
    topevery.ajax({ type: 'get', url: url, dataType: 'html' }, function (data) {
        editLayer= layer.open({
            type: 1,
            title: "编辑",
            skin: 'layui-layer-rim', //加上边框
            area: [400 + 'px', 300 + 'px'], //宽高
            content: data,
            end: function () {
                reload();
            }
        });
    }, true);
}


