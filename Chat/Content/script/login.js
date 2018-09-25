
var Islogin = true;

$(function () {

    //回车登录
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            document.getElementById('Submit').click = LoginorRegister();
        }
    };
})

//文本切换
function txtChange() {
    $("#validateMobile").html('');
    $("#validatePwd").html('');
    $("#validateTxt").html('');
    if (Islogin) {
        $("#txtTitle").html('注册 Chat');
        $("#Submit").html('注 册');
        $("#spanHasAcount").html('已有账号 ');
        $("#btnHasAcount").html('直接登录');
        Islogin = false;
    }
    else {
        $("#txtTitle").html('登录 Chat');
        $("#Submit").html('登 录');
        $("#spanHasAcount").html('没有账号 ');
        $("#btnHasAcount").html('立即注册');
        Islogin = true;
    }
}

//提交数据
function LoginorRegister() {
    if (Validate()) {
        var status = document.getElementById('RememberMe');
        var queryData = {
            Mobile: $("#Mobile").val(),
            PassWord: $("#Pwd").val(),
            RememberMe:1
        }
        if (Islogin) {
            Login(queryData);
        }
        else {
            Register(queryData);
        }
    }
}

//登录
function Login(queryData) {
    $.post("/Login/Login", { data: $.toJSON(queryData) },
           function (data) {
               var info = eval('(' + data + ')');
               if (info.Success) {
                   window.location.href = "/Chat/Index";
               }
               else {
                   $("#validateTxt").html('手机号或者密码错误！');
               }
           })
}

//注册
function Register(queryData) {
    $.post("/Login/Register", { data: $.toJSON(queryData) },
           function (data) {
               if (data.Success) {
                   window.location.href = "/Chat/Index";
               }
               else {
                   $("#validateTxt").html('该手机号已注册！');
               }
           })
}

//表单校验
function Validate() {
    $("#validateMobile").html('');
    $("#validatePwd").html('');
    $("#validateTxt").html('');
    if ($("#Mobile").val() == "" || $("#Mobile").val() == null) {
        $("#validateMobile").html('请输入手机号！');
        return false;
    }
    if (!IsPoneAvailable($("#Mobile").val())) {
        $("#validateMobile").html('请输入有效的手机号！');
        return false;
    }
    if ($("#Pwd").val() == "" || $("#Pwd").val() == null) {
        $("#validatePwd").html('请输入密码！');
        return false;
    }
    return true;
}

//手机号正则匹配
function IsPoneAvailable(str) {
    var myreg = /^[1][3,4,5,7,8][0-9]{9}$/;
    if (!myreg.test(str)) {
        return false;
    } else {
        return true;
    }
}