$(function () {
    GetHeadPath();
})

//添加好友
function AddFriend(Id) {
    //初始化
    $('#OriginId').val(Id);
    $("#divReqContent").show();
    $("#btn_next").show();
    $("#divDescName").hide();
    $("#btn_submit").hide();
    $('#ReqContent').val("");
    $('#DescName').val("");
    $("#addModel").modal({ backdrop: 'static', keyboard: false });

}

//下一步
function Next_Submit() {
    $("#divReqContent").hide();
    $("#btn_next").hide();
    $("#divDescName").show();
    $("#btn_submit").show();
}

//提交
function Submit() {
    var queryData = {
        ReqOriginId: $('#OriginId').val(),
        ReqContent: $('#ReqContent').val(),
        DescName: $('#DescName').val(),
        AddType: $('#AddType').val(),
    }
    $.post("/Add/AddFriend", { data: $.toJSON(queryData) },
        function (datas) {
            if (datas.Success) {
                $("#addModel").modal('hide');
                AddTableRefresh();
            }
        })
}

function GetHeadPath()
{
    $.post("/Home/GetUserDefaultInfo",
        function (data) {
            if (data.Success) {
                var str = "<img src='" + data.Data.DefaultPath_50+"' style='border-radius: 2px'>"
                $('#DefaultPath_50').html(str);
                $('#hiddenDefaultPath_50').val(data.Data.DefaultPath_50);
            }
        })
}

