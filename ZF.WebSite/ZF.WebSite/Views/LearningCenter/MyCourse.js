
GetPurchaseList();
GetRecommendCourse();

///购买课程
function GetPurchaseList() {
    topevery.ajax({
        url: 'api/MyStudy/GetMyCourse',
    }, function (data) {
        if (data.Success) {
            $('#myCourse').html(template("MyCourse_hmtl", data));
        }
    });
}

//推荐课程
function GetRecommendCourse() {
    topevery.ajax({
        url: "api/MyStudy/GetRecommendCourse",
        data: JSON.stringify({ IsFree: 1, SubjectId: topevery.GetCookie("SubjectId"), Page: 1, Rows: 6,Type:0 })
    }, function (data) {
        if (data.Success) {
            $("#recommendCourse").html(template("Recommend_hmtl", data));
        }
    });
}
