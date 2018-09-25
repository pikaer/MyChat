

$(function () {

    MyBottleList();

    //我的瓶子按钮
    $("#btnmyBottle").click(function () {
        BackHome = false;
        $("#modalReplyList").hide();
        AllReplyBottleList();
        $("#modalMyBottleList").show();
        $("#ReplyBottle").modal({ backdrop: 'static', keyboard: false });
    })
    //扔一个按钮
    $("#btnthrowBottle").click(function () {
        $("#throwBottleContent").val("");
        $("#throwBottleModel").modal({ backdrop: 'static', keyboard: false });
    })

    //捞捞看
    $("#btnreceivedBottle").click(function () {
        $.post("/Discovery/GetNewBottle",
            function (data) {
                if (data.Success) {
                    MyBottleList();
                }
            })
    })

    // 右部页签切换
    $('#actRightTab div').click(function () {
        var index = $('#actRightTab div').index(this);
        $(this).addClass('btnMenuClick').siblings().addClass('btnMenu').removeClass('btnMenuClick');
    })

    //申明服务器代理
    var bottle = $.connection.bottleHub;
    //发布消息
    $.connection.hub.start().done(function () {
        $("#SubmitReplyBottle").click(function () {
            if ($.trim($("#ReplyBottleContent").val()) == "") {
                return;
            }
            var queryData = {
                Id: $("#hiddenBottleId").val(),
                Content: $("#ReplyBottleContent").val()
            }
            var data = JSON.stringify(queryData);
            bottle.server.replyBottle(data);
            InitBottleList();
            $("#ReplyBottleContent").val("");
        });
    });

    //订阅消息
    bottle.client.pushReplyToUser = function (bottleId) {
        //do something
    };
})



//扔一个瓶子提交按钮
function btnSubmit() {
    $("#ChatHistory").html("");
    var queryData = {
        BottleContent: $("#throwBottleContent").val()
    }
    $.post("/Discovery/ThrowOneBottle", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                $("#throwBottleModel").modal('hide');
                MyBottleList();
            }
        })
}

//加载某个瓶子聊天记录
function ReplyBottleList(bottleId) {
    BackHome = true;
    BottleList(bottleId);
}

function BottleList(bottleId) {
    $("#modalMyBottleList").hide();
    $("#hiddenBottleId").val(bottleId);
    InitBottleList();
    $("#ReplyBottle").modal({ backdrop: 'static', keyboard: false });
    $("#modalReplyList").show();
}
//回复某个瓶子
function ReplyBottle() {
    if ($.trim($("#ReplyBottleContent").val()) == "") {
        return;
    }
    var queryData = {
        Id: $("#hiddenBottleId").val(),
        Content: $("#ReplyBottleContent").val()
    }
    $.post("/Discovery/ReplyBottle", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                InitBottleList();
                $("#ReplyBottleContent").val("");
            }
        })
}


//初始化加载我捡到但未回复的瓶子列表(一级列表）
function MyBottleList() {
    $.post("/Discovery/GetAllBottleList",
        function (data) {
            if (data.Success) {
                datalist = data.Data;
                $("#MybottleList").html("");
                for (var i = 0; i < datalist.length; i++) {
                    var bottle = datalist[i];
                    var htmlOrigin = "<div class='divBottle'  data-toggle='context' data-target='#context-deleteBottle' onmousemove='SetBottleId(" + bottle.BottleId + ")'  onclick='ReplyBottleList(" + bottle.BottleId + ")' ><div class='bottleContent'>" + bottle.BottleContent + "</div><div class='bottleNickName'>" + bottle.NickName + "</div>";
                    //男
                    if (bottle.Gender == 1) {
                        htmlOrigin += "<div class='bottleGenderMan'>&#xe62c;</div><div class='bottleCity'>" + bottle.CityName + "</div> <div class='cityiconfont'>&#xe671;</div></div>";
                    }
                    //女
                    else if (bottle.Gender == 2) {
                        htmlOrigin += "<div class='bottleGenderWoman'>&#xe62c;</div><div class='bottleCity'>" + bottle.CityName + "</div> <div class='cityiconfont'>&#xe671;</div></div>";
                    }
                    //性别未知
                    else {
                        htmlOrigin += "<div class='bottleCity'>" + bottle.CityName + "</div> <div class='cityiconfont'>&#xe671;</div></div>";
                    }
                    $("#MybottleList").append(htmlOrigin);
                }
            }
        })

}

//我回复过的或我扔出去被回复的瓶子列表(二级列表）
function AllReplyBottleList() {
    $.post("/Discovery/AllReplyBottleList",
        function (data) {
            if (data.Success) {
                $("#divMyBottleList").html("");
                datalist = data.Data;
                for (var i = 0; i < datalist.length; i++) {
                    var bottle = datalist[i];
                    var htmlOrigin = "<div class='myBottleList' data-toggle='context' data-target='#context-deleteBottleReply' onmousemove='SetBottleReplyId(" + bottle.BottleId + ")'  onclick='BottleList(" + bottle.BottleId + ")'><div class='headshot'><img src='" + bottle.HeadshotPathDesc + "' class='img-headshot'></div>";
                    htmlOrigin += "<div class='modeluserInfo'><div class='bottleContent'>" + bottle.RecentContent + "</div>";
                    htmlOrigin += "<div class='bottleNickName'>" + bottle.NickName + "</div></div>";
                    htmlOrigin += "<div class='time'><div>" + bottle.LastTimeDesc + "</div><div class='unreadBottleList'>20</div></div></div>";
                    $("#divMyBottleList").append(htmlOrigin);
                }
            }
        })
}


//初始化某个瓶子聊天对话列表数据(三级列表）
function InitBottleList() {
    $("#ReplyBottleArea").html("");
    $("#ReplyBottleContent").val("");
    var queryData = {
        Id: $("#hiddenBottleId").val(),
    }
    $.post("/Discovery/ReplyBottleList", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                datalist = data.Data;
                for (var i = 0; i < datalist.length; i++) {
                    var bottle = datalist[i];
                    var htmlOrigin = "<div class='replyBottleList' data-toggle='context' data-target='#context-deleteBottleChat' onmousemove='SetBottleChatId(" + bottle.BottleId + "," + bottle.BottleChatId + ")' ><div class='headshot'><img src='" + bottle.HeadshotPathDesc + "' class='img-headshot'></div>";
                    htmlOrigin += "<div class='modeluserInfo'><div class='bottleContent'>" + bottle.RecentContent + "</div>";
                    htmlOrigin += "<div class='replyTime'>" + bottle.LastTimeDesc + "</div></div>";
                    htmlOrigin += "<div class='cityArea'><div class='bottleCity'>" + bottle.CityName + "</div><div class='cityiconfont'>&#xe671;</div></div></div>";
                    $("#ReplyBottleArea").append(htmlOrigin);
                }
            }
        })
}

//是否返回主页
var BackHome = true;
//返回上一级
function ReturnUp() {
    if (BackHome) {
        MyBottleList();
        $("#ReplyBottle").modal('hide');
    }
    else {
        $("#modalReplyList").hide();
        AllReplyBottleList();
        $("#modalMyBottleList").show();
    }
}


//删除或者举报瓶子(一级列表）
function btnUpdateBottle(state) {
    var queryData = {
        Id: $("#hiddenUpdateBottleId").val(),
        Content: state
    }
    $.post("/Discovery/UpdateBottle", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                MyBottleList();
            }
        })
}

//删除我的瓶子（二级列表）
function btnDeleteMyBottleReply() {
    var queryData = {
        Id: $("#hiddenReplyBottleId").val()
    }
    $.post("/Discovery/DeleteBottleReply", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                AllReplyBottleList();
            }
        })
}

//删除某个瓶子对话（三级列表）
function btnDeleteMyBottleChat() {
    var queryData = {
        Id: $("#hiddenChatId").val(),        //BottleChatId,瓶子对话Id
        BId: $("#hiddenChatBottleId").val()  //BottleId,瓶子Id
    }
    $.post("/Discovery/DeleteBottleChat", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                InitBottleList();
            }
        })
}


//一级列表当前bottleId
function SetBottleId(bottleId) {
    $("#hiddenUpdateBottleId").val(bottleId)
}

//二级列表当前bottleId
function SetBottleReplyId(bottleId) {
    $("#hiddenReplyBottleId").val(bottleId)
}

//三级列表当前bottleId
function SetBottleChatId(bottleId, chatId) {
    $("#hiddenChatBottleId").val(bottleId);
    $("#hiddenChatId").val(chatId);
}