
//Main函数
$(function () {
    GetHeadPath();

    //初始化聊天列表
    InitTable();

    //回车提交事件
    document.getElementById("Content").onkeydown = function (e) {
        e = e || window.event;
        if (e.ctrlKey && e.keyCode == 13) {
            $("#Submit").click();
        }
    };

    //申明服务器代理
    var chat = $.connection.chatHub;

    //发布消息
    $.connection.hub.start().done(function () {
        //提交聊天内容
        $("#Submit").click(function () {
            var content = $("#Content").val();
            if (content == "") {
                return;
            }
            var path = $('#hiddenDefaultPath_50').val();
            //将此消息推到自己窗口
            var html = "<div class='ChatRight'> <div class='ChatContentRight'>" + content + " </div> <div style='padding-right:5px;float:right;'><img src='" + path + "' class='img-rounded'> </div>  </div>";
            $("#ChatHistory").append(html);
            $("#Content").val("");
            ScrollToTop();

            var queryData = {
                Type: 0,
                ChatContent: content,
                OriginId: $("#hiddenOriginId").val()
            };
            var data = JSON.stringify(queryData);
            chat.server.sendMessageToUser(data);
            //刷新未读条数及内容
            RefrashUnReadMsg();
        });
    });

    //订阅消息
    chat.client.pushMessgeToUser = function (userId) {
        //如果对方打开了和该用户的聊天窗口
        if ($("#hiddenOriginId").val() == userId) {
            //刷新聊天内容
            GetChatContent();
        }
        //刷新未读条数及内容
        RefrashUnReadMsg();
    };

    //右击菜单
    $('#table').contextmenu({
        target: '#context-menu',
        onItem: function (context, e) {
            alert($(e.target).text());
        }
    });
});

//聊天好友列表
function InitTable() {
    $("#table").bootstrapTable({
        url: '/Chat/GetChatFriendList',
        showHeader: false,
        cache: false,
        striped: false,  //表格显示条纹，默认为false
        clickToSelect: true,
        paginationLoop: false, //是否开启无限循环
        pagination: false,
        sidePagination: 'server',
        columns: [
            {
                field: 'imgUrl',
                title: '头像',
                align: 'left',
                valign: 'middle',
                width: '20%',
                formatter: function (value, row, index) {
                    return '<img src="' + row.HeadshotPathDesc + '" style="border-radius:3px;float:left">';
                }
            },
            {
                field: 'DescName', align: 'left', valign: 'middle', width: '50%',
                formatter: function (value, row, index) {
                    var Id = "RecentChatContent" + row.OriginId;
                    var html = row.DescName + '<br/>' + '<span id="' + Id + '"  class="RecentChat" ></span>';
                    return html;
                }
            },
            {
                field: 'RecentChatTimeDesc', align: 'left', valign: 'middle', width: '20%',
                formatter: function (value, row, index) {
                    var Id = "RecentChatTime" + row.OriginId;
                    return '<span id="' + Id + '" class="RecentChat"></span>';
                }
            },
            {
                field: 'Unread', align: 'left', valign: 'middle', width: '10%',
                formatter: function (value, row, index) {
                    var Id = "UnreadCount" + row.OriginId;
                    return '<span id="' + Id + '" ></span>';
                }
            }
        ],
        onLoadSuccess: function (data) {  //加载成功事件
            if (data.total != 0) {
                $("#hiddenOriginId").val(data.rows[0].OriginId);
                InitChatContent(data.rows[0].DescName);
                RefrashUnReadMsg();  //刷新未读条数及最新聊天内容
            }
        },
        onClickCell: function (field, value, row, $element) {  //单击事件
            $("#hiddenOriginId").val(row.OriginId);
            InitChatContent(row.DescName);
        }
    })
};

//初始化聊天内容
function InitChatContent(descName) {
    $("#contentTitle").html(descName);
    GetChatContentHistory();
    UpdateReadTime();//将第一行标记为已读
};

//刷新对方发来的最新聊天内容
function GetChatContent() {
    var queryData = {
        OriginId: $("#hiddenOriginId").val(),
        Time: $("#hiddenLastTime").val()
    };
    $.post("/Chat/GetChatContent", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                $("#hiddenLastTime").val(data.Data.LastTimeStr);
                datalist = data.Data.ChatHistory;
                if (datalist.length > 0) {
                    UpdateReadTime();//将对方发来的这条消息标为已读
                }
                for (var i = 0; i < datalist.length; i++) {
                    var content = datalist[i].ChatContent;
                    var html = '<div style="float:left;clear:both;padding-top:5px;padding-left:5px"><div style="padding-left:5px;float:left"> <img src="' + datalist[i].DefaultPath_50+'" class="img-rounded"> </div><div style="padding-top:10px;float:right;background-color:#e6edd8;min-height:40px;max-width:450px;margin-top:5px;margin-left:10px;padding-top:10px;padding-bottom:10px;padding-left:10px;padding-right:10px;border-radius:8px"> ' + content + ' </div> </div>';
                    if (content == "" || content == null) {
                        continue;
                    }
                    else {
                        $("#ChatHistory").append(html);
                    }
                    content = "";
                    ScrollToTop();
                }
            }
        })
};

//获取聊天历史记录
function GetChatContentHistory() {
    $("#ChatHistory").html("");
    var queryData = {
        OriginId: $("#hiddenOriginId").val()
    };
    $.post("/Chat/GetChatContentHistory", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Success) {
                $("#hiddenLastTime").val(data.Data.LastTimeStr);
                datalist = data.Data.ChatHistory;
                for (var i = 0; i < datalist.length; i++) {
                    if (datalist[i].ChatContent == "" || datalist[i].ChatContent == null) {
                        continue;
                    }
                    else {
                        if (datalist[i].OnRight) {
                            var contentUser = datalist[i].ChatContent;
                            var htmlUser = '<div class="ChatRight"><div class="ChatContentRight">' + contentUser + ' </div><div class="ChatImgRight"><img src="' + datalist[i].DefaultPath_50 + '" class="img-rounded"></div></div>';
                            $("#ChatHistory").append(htmlUser);
                            var contentUser = "";
                        }
                        else {
                            var contentOrigin = datalist[i].ChatContent;
                            var htmlOrigin = '<div class="ChatLeft"><div class="ChatImgLeft"><img src="' + datalist[i].DefaultPath_50 + '" class="img-rounded"></div><div class="ChatContentLeft">' + contentOrigin + ' </div></div>'
                            $("#ChatHistory").append(htmlOrigin);
                            var contentOrigin = "";
                        }
                    }
                }
                ScrollToTop(); //滚动至最底部
                GetChatContent(); //启动刷新聊天记录
            }
        })
}

//刷新未读消息内容及条数
function RefrashUnReadMsg() {
    $.post("/Chat/RefrashUnReadMsg",
        function (data) {
            if (data.Success) {
                datalist = data.Data;
                for (var i = 0; i < datalist.length; i++) {
                    //刷新聊天内容
                    var contentId = "#RecentChatContent" + datalist[i].OriginId;
                    $(contentId).html(datalist[i].RecentChatContentDesc);
                    //刷新聊天时间
                    var chatTimeId = "#RecentChatTime" + datalist[i].OriginId;
                    $(chatTimeId).html(datalist[i].RecentChatTimeDesc);
                    //过滤当前打开的窗口
                    if (datalist[i].OriginId != $("#hiddenOriginId").val()) {
                        if (datalist[i].UnReadCountStr != "") {
                            //未读条数
                            var Id = "#UnreadCount" + datalist[i].OriginId;
                            if (!$(Id).hasClass('unread')) {
                                $(Id).addClass('unread');
                            }
                            $(Id).html(datalist[i].UnReadCountStr);
                            //TableStick(datalist[i].OriginId);
                        }
                    }
                }
            }
        })
}

//将未读标记为已读
function UpdateReadTime() {
    var originId = $("#hiddenOriginId").val();
    var queryData = {
        OriginId: originId
    }
    var Id = "#UnreadCount" + originId;
    $.post("/Chat/UpdateReadTime", { data: $.toJSON(queryData) },
        function (data) {
            if (data.Data) {
                $(Id).html("");
                $(Id).removeClass('unread');
            }
        })
}

//滚动至最底部
function ScrollToTop() {
    $("#ChatHistory").scrollTop($("#ChatHistory")[0].scrollHeight);
}

//新消息对应列表置顶
function TableStick(originId) {
    //获取列表节点和数据
    var tabledatas = $("#table").bootstrapTable('getData');
    $(tabledatas).each(function (index, item) {
        if (item.OriginId == originId) {
            var $tr = $(index).parents("tr");
            $tr.fadeOut().fadeIn();
            $("#table").prepend($tr);
        }
    });
}

