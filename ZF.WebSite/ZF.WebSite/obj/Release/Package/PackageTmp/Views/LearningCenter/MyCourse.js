$(function () {

    GetPurchaseList();
    GetRecommendCourse();

    ///购买课程
    function GetPurchaseList() {
        topevery.ajax({
            url: 'api/MyStudy/GetMyCourse',
        }, function (data) {
            if (data.Success && data.Result.length > 0) {
                $('#myCourse').html(template("MyCourse_hmtl", data));
            }
        });
    }

    //推荐课程
    function GetRecommendCourse() {
        topevery.ajax({
            url: "api/MyStudy/GetRecommendCourse",
            data: JSON.stringify({ IsFree: 1,  IsRecommend: 1, SubjectId: topevery.GetCookie("subjectId") })
        }, function (data) {
            if (data.Success) {
                console.log(data);
                $("#recommendCourse").html(template("Recommend_hmtl", data));
            }
        });
    }
})