
//省市下拉列表框赋值标题是否显示开关
var IsShowTitle = true;
//是否正在为Home赋值
var IsSetHome = true;

function GenderCombobox() {
    //性别下拉列表框
    SetCombobx("/Common/GenderCombobox", "Gender", "");
}

function GenderComboboxV1() {
    //性别下拉列表框
    SetCombobx("/Common/GenderComboboxV1", "Gender", "");
}

function AgeCombobox() {
    //年龄范围下拉列表
    SetCombobx("/Common/AgeCombobox", "Age", "");
}

function ProvinceCombobox() {
    //省下拉列表
    SetCombobx("/Common/ProvinceCombobox", "HomeProvince", "HometownProvince");
}

function BloodTypeCombobox() {
    //血型下拉列表框
    SetCombobx("/Common/BloodTypeCombobox", "BloodType", "");
}

function YearCombobox() {
    //年份下拉列表
    SetCombobx("/Common/YearCombobox", "YearSelect", "");
}

function MonthCombobox() {
    //月份下拉列表
    SetCombobx("/Common/MonthCombobox", "MonthSelect", "");
}



function InitClick(){
    //点击窗体隐藏省市下拉列表（过滤目标区域）
    $('body').click(function (e) {
        if (e.target.id != 'Home' && e.target.id != 'Hometown' && e.target.className != 'optionSelect' && e.target.id != 'editBirthDay'
            && e.target.id != 'HomeProvinceArea' && e.target.id != 'HometownProvinceArea' && e.target.id != 'DateTimeArea'
             && e.target.id != 'HomeProvince' && e.target.id != 'HometownProvince' && e.target.id != 'YearSelect'
             && e.target.id != 'HomeCity' && e.target.id != 'HometownCity' && e.target.id != 'MonthSelect'
             && e.target.id != 'HomeArea' && e.target.id != 'HometownArea' && e.target.id != 'DaySelect') {
            if (!$("#HomeProvinceArea").is(":hidden")) {
                $("#HomeProvinceArea").hide();
            }
            if (!$("#HometownProvinceArea").is(":hidden")) {
                $("#HometownProvinceArea").hide();
            }
            if (!$("#DateTimeArea").is(":hidden")) {
                $("#DateTimeArea").hide();
            }
        }
    })
}



//下拉列表框公共方法
function SetCombobx(url, Id1, Id2) {
    $.post(url,
    function (datas) {
        if (datas.Success) {
            var select1 = $('#' + Id1);
            select1.html("");
            if (Id2 != null && Id2 != "") {
                var select2 = $('#' + Id2);
                select2.html("");
                for (var i = 0; i < datas.Data.length; i++) {
                    var options = "<option value='" + datas.Data[i].SelectKey + "' class='optionSelect'>" + datas.Data[i].SelectValue + "</option>";
                    select1.append(options);
                    select2.append(options);
                }
            }
            else {
                for (var i = 0; i < datas.Data.length; i++) {
                    select1.append("<option value='" + datas.Data[i].SelectKey + "' class='optionSelect'>" + datas.Data[i].SelectValue + "</option>");
                }
            }
        }
    })
}

//市，区下拉列表框公共方法
function SetProvincesCombobx(url, Id, queryData) {
    $.post(url, { data: $.toJSON(queryData) },
        function (datas) {
            if (datas.Success) {
                var select = $('#' + Id);
                select.html("");//先清空
                select.append('<option value="-1" class="optionSelect">不限</option>');
                for (var i = 0; i < datas.Data.length; i++) {
                    select.append("<option value='" + datas.Data[i].SelectKey + "' class='optionSelect' >" + datas.Data[i].SelectValue + "</option>");
                }
            }
            //刷新控件值
            if (IsSetHome)
            {
                SetHomeValue();
            }
            else {
                SetHometownValue();
            }
        })
}

function DateTimeClick() {
    if ($("#DateTimeArea").is(":hidden")) {
        $("#DateTimeArea").show();
    }
    else {
        $("#DateTimeArea").hide();
    }
}


//所在地单击事件
function HomeAreaCtrl() {
    $("#HometownProvinceArea").hide();
    if ($("#HomeProvinceArea").is(":hidden")) {
        $("#HomeProvinceArea").show();
    }
    else {
        $("#HomeProvinceArea").hide();
    }
}

//故乡单击事件
function HometownAreaCtrl() {
    $("#HomeProvinceArea").hide();
    if ($("#HometownProvinceArea").is(":hidden")) {
        $("#HometownProvinceArea").show();
    }
    else {
        $("#HometownProvinceArea").hide();
    }
}

//所在地-省onChange
function HomeProvinceChange() {
    IsSetHome = true;
    $("#HomeArea").html("");
    $("#HomeArea").append('<option value="-1" class="optionSelect">不限</option>')
    var queryData = {
        ProvinceId: $('#HomeProvince').find("option:selected")[0].value
    }
    SetProvincesCombobx("/Common/CityCombobox", "HomeCity", queryData);
}

//所在地-市onChange
function HomeCityChange() {
    IsSetHome = true;
    var queryData = {
        CityId: $('#HomeCity').find("option:selected")[0].value
    }
    SetProvincesCombobx("/Common/AreaCombobox", "HomeArea", queryData);
}

//故乡-省onChange
function HometownProvinceChange() {
    IsSetHome = false;
    $("#HometownArea").html("");
    $("#HometownArea").append('<option value="-1" class="optionSelect">不限</option>')
    var queryData = {
        ProvinceId: $('#HometownProvince').find("option:selected")[0].value
    }
    SetProvincesCombobx("/Common/CityCombobox", "HometownCity", queryData);
}

//故乡-市onChange
function HometownCityChange() {
    IsSetHome = false;
    var queryData = {
        CityId: $('#HometownCity').find("option:selected")[0].value
    }
    SetProvincesCombobx("/Common/AreaCombobox", "HometownArea", queryData);
}

//所在地赋值
function SetHomeValue() {
    var txt = "";
    $('#Home').val("");
    var HomeProvince = $('#HomeProvince').find("option:selected")[0].text;
    var HomeCity = $('#HomeCity').find("option:selected")[0].text;
    var HomeArea = $('#HomeArea').find("option:selected")[0].text;
    if (HomeProvince == "不限")
    {
        return;
    }
    if (IsShowTitle)
    {
        txt = "所在地:" + HomeProvince + ',' + HomeCity + ',' + HomeArea;
    }
    else {
        if (HomeCity == "不限") {
            HomeCity = "";
        }
        if (HomeArea == "不限") {
            HomeArea = "";
        }
        txt = HomeProvince + ' ' + HomeCity + ' ' + HomeArea;
    }
    $('#Home').val(txt);
}

//故乡赋值
function SetHometownValue() {
    var txt = "";
    $('#Hometown').val("");
    var HometownProvince = $('#HometownProvince').find("option:selected")[0].text;
    var HometownCity = $('#HometownCity').find("option:selected")[0].text;
    var HometownArea = $('#HometownArea').find("option:selected")[0].text;
    if (HometownProvince == "不限") {
        return;
    }
    if (IsShowTitle)
    {
        txt = "故乡:" + HometownProvince + ',' + HometownCity + ',' + HometownArea;
    }
    else {
        if (HometownCity == "不限")
        {
            HometownCity = "";
        }
        if (HometownArea == "不限") {
            HometownArea = "";
        }
        txt = HometownProvince + ' ' + HometownCity + ' ' + HometownArea;
    }
    $('#Hometown').val(txt);
}


function DateTimeChange()
{
    var queryData = {
        Id: $('#YearSelect').find("option:selected")[0].value,
        BId: $('#MonthSelect').find("option:selected")[0].value
    }
    $.post("/Common/DayCombobox", { data: $.toJSON(queryData) },
       function (datas) {
           if (datas.Success) {
               $('#DaySelect').html("");//先清空
               for (var i = 0; i < datas.Data.length; i++) {
                   $('#DaySelect').append("<option value='" + datas.Data[i].SelectKey + "' class='optionSelect' >" + datas.Data[i].SelectValue + "</option>");
               }
               SetDateTimeValue();
           }
       })
}

function SetDateTimeValue()
{
    var year = $('#YearSelect').find("option:selected")[0].value;
    var month = $('#MonthSelect').find("option:selected")[0].value;
    var day = $('#DaySelect').find("option:selected")[0].value;
    var txt = year + "-" + month + "-" + day;
    $('#editBirthDay').val(txt);
}