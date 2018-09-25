
$(function () {
    InitLeftArea();
    GenderComboboxV1();
    BloodTypeCombobox();
    ProvinceCombobox();
    YearCombobox();
    MonthCombobox();
    IsShowTitle = false;//省市下拉列表框赋值标题是否显示开关
    BasicInfoView(true);
})

//初始化左边切换按钮
function InitLeftArea() {
    // 左部页签切换
    $('#LeftArea div').click(function () {
        var index = $('#LeftArea div').index(this);
        for (var i = 0; i < 4; i++) {
            var id = "RightArea" + i;
            if (i == index) {
                $('#' + id).show();
            }
            else {
                $('#' + id).hide();
            }
        }
        $(this).addClass('configClick').siblings().addClass('config').removeClass('configClick');
    })
    //编辑
    $('#btnEdit').click(function () {
        InitClick();
        $('#BasicInfoView').hide();
        $('#BasicInfoEdit').show();
        BasicInfoView(false)
    })
}

//取消编辑
function CanceltEdit() {
    $('#BasicInfoView').show();
    $('#BasicInfoEdit').hide();
}

//上传头像
function UpLoadHead() {
    $("#avatar-modal").modal({ backdrop: 'static', keyboard: false });
}

//用户详情
function BasicInfoView(IsView) {
    $.post("/UserInfo/GetBasicInfo",
           function (data) {
               if (data.Success) {
                   var dto = data.Data;
                   if (IsView)//预览页面
                   {
                       $('#divNickName').html(dto.NickName);
                       $('#divBirthDay').html(dto.BirthDayDesc);
                       $('#divPhone').html(dto.Mobile);
                       $('#divEmail').html(dto.Email);
                       $('#divHomeTown').html(dto.HomeTownDesc);
                       $('#divHome').html(dto.HomeDesc);
                       $('#divSignature').html(dto.Signature);
                       $('#divGender').html(dto.GenderDesc);
                       $('#divBlood').html(dto.BloodTypeDesc);
                       $('#divQQ').html(dto.QQ);
                       $('#divUserId').html("账号：" + dto.Id);
                       $('#divHeadPath').html("");
                       var path = "<img src='" + dto.HeadshotPathDesc + "' style='border-radius:3px' onclick='UpLoadHead()'>"
                       $('#divHeadPath').html(path);
                   }
                   else { //编辑页面
                       //Birth
                       $('#YearSelect').val(dto.Year);
                       $('#MonthSelect').val(dto.Month);
                       DateTimeChange();
                       $("#editNickName").val(dto.NickName);
                       $('#Gender').val(dto.Gender);
                       $('#BloodType').val(dto.BloodType);
                       $('#editQQ').val(dto.QQ);
                       $('#editEmail').val(dto.Email);
                       $('#editSignature').val(dto.Signature);
                       $('#Home').val(dto.HomeDesc);
                       $('#Hometown').val(dto.HomeTownDesc);
                       //HomeTown
                       $('#HometownProvince').val(dto.HomeTownProvinceId);
                       $('#HomeProvince').val(dto.HomeProvinceId);
                       SetCombobxV2("/Common/CityCombobox", "HometownCity", dto.HomeTownCityId, { ProvinceId: dto.HomeTownProvinceId })
                       SetCombobxV2("/Common/CityCombobox", "HomeCity", dto.HomeCityId, { ProvinceId: dto.HomeProvinceId })
                       SetCombobxV2("/Common/AreaCombobox", "HometownArea", dto.HomeTownAreaId, { CityId: dto.HomeTownCityId })
                       SetCombobxV2("/Common/AreaCombobox", "HomeArea", dto.HomeAreaId, { CityId: dto.HomeCityId })
                       $('#DaySelect').val(dto.Day);
                      }
               }
           })
}

//提交编辑
function SubmitEdit() {
    var queryData = {
        NickName: $("#editNickName").val(),
        Gender: $('#Gender').find("option:selected").val(),
        BirthDay: $("#hiddenOriginId").val(),
        BloodType: $('#BloodType').find("option:selected").val(),
        QQ: $("#editQQ").val(),
        Email: $("#editEmail").val(),
        Signature: $("#editSignature").val(),
        BirthDay: $("#editBirthDay").val(),
        //HomeTown
        HomeTownProvinceId: $('#HometownProvince').find("option:selected").val(),
        HomeTownCityId: $('#HometownCity').find("option:selected").val(),
        HomeTownAreaId: $('#HometownArea').find("option:selected").val(),
        //Home
        HomeProvinceId: $('#HomeProvince').find("option:selected").val(),
        HomeCityId: $('#HomeCity').find("option:selected").val(),
        HomeAreaId: $('#HomeArea').find("option:selected").val()
    }
    $.post("/UserInfo/BasicInfoEdit", { data: $.toJSON(queryData) },
            function (data) {
                if (data.Success) {
                    //重置左上角头像
                    RefreshHeadPath();
                    BasicInfoView(true);
                    //取消编辑
                    CanceltEdit();
                }
            })
};

//重置左上角头像
function RefreshHeadPath() {
    $.post("/Home/GetUserDefaultInfo",
        function (data) {
            if (data.Success) {
                var str = "<img src='" + data.Data.DefaultPath_50 + "' style='border-radius: 2px'>";
                $("#DefaultPath_50").html(str);
            }
        })
}

//填充省市区下拉列表，解决异步请求导致的填充失败问题
function SetCombobxV2(url, Id, dateValue, queryData) {
    $.post(url, { data: $.toJSON(queryData) },
    function (datas) {
        if (datas.Success) {
            var select = $('#' + Id);
            select.html("");
            for (var i = 0; i < datas.Data.length; i++) {
                select.append("<option value='" + datas.Data[i].SelectKey + "' class='optionSelect'>" + datas.Data[i].SelectValue + "</option>");
            }
            select.val(dateValue);
        }
    })
}