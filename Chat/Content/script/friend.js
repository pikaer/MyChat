
//Main
$(function () {
    //初始化聊天列表
    InitTable();
    NewFriend();
})

//聊天好友列表
function InitTable() {
    $("#table").bootstrapTable({
        url: '/Friend/GetFriendList',
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
                    return '<div onmousemove="SetOriginId(' + row.Id + ')"  style="padding:8px;"><img src="' + row.HeadshotPathDesc + '" style="border-radius: 300px"></div>';
                }
            },
            {
                field: 'DescNameStr', align: 'left', valign: 'middle', width: '80%',
                 formatter: function (value, row, index) {
                     return '<div onmousemove="SetOriginId(' + row.Id + ')" style="padding:14px;width:100%;height:50px">' + row.DescNameStr + '</div>';
                }
            },
        ],
        onClickCell: function (field, value, row, $element) {  //单击事件
            $("#newFriend").hide();
            $("#FriendDetail").show();
            LoadUserDetail(row.Id)
        }
    })
}

//加载好友详情
function LoadUserDetail(Id) {
    var queryData = {
        OriginId: Id
    }
    $.post("/Friend/UserDetail", { data: $.toJSON(queryData) },
         function (data) {
             if (data.Data) {
                 var headpath = "<img src=" + data.Data.HeadshotPathDesc + " style='border-radius:0px' >";
                 $("#HeadshotPath").html(headpath);
                 $("#NickName").html(data.Data.NickName);
                 $("#Signature").html(data.Data.Signature);
                 $("#DescName").html("备注：" + data.Data.DescName);
                 $("#AddRess").html("地区：");
                 $("#Acount").html("账号：" + data.Data.Id);
                 $("#hiddenOriginId").val(data.Data.Id);
                 $("#Resource").html("来源：" + data.Data.AddTypeDesc);
                 if (data.Data.HomeProvinceName != null) {
                     $("#AddRess").append(data.Data.HomeProvinceName);
                     if (data.Data.HomeCityName != null) {
                         $("#AddRess").append(data.Data.HomeCityName);
                     }
                 }
                 if (data.Data.Gender != 0) {
                     if ($("#IconGender").hasClass('maniconfont')){
                         $("#IconGender").removeClass('maniconfont');
                     }
                     if ($("#IconGender").hasClass('womaniconfont')) {
                         $("#IconGender").removeClass('womaniconfont');
                     }
                     $("#IconGender").html("&#xe62c;");
                     if (data.Data.Gender == 1) {
                         $("#IconGender").addClass('maniconfont');
                     }
                     else {
                         $("#IconGender").addClass('womaniconfont');
                     }
                 }
                 else {
                     $("#IconGender").html("");
                 }
             }
            
         })
}

//加载新好友请求列表
function NewFriend() {
    //初始化
    $("#FriendDetail").hide();
    $("#newFriend").show();
    $("#newFriend").html("");
    $.post("/Friend/GetNewFriendList",
                function (data) {
                    if (data.Success) {
                        var list = data.Data;
                        for (var i = 0; i < list.length; i++) {
                            var datalist = list[i];
                            var html = " <div class='friend'  onmousemove='SetReqId(" + datalist.Id + ")' id='divnewFriend' data-toggle='context' data-target='#context-delete'><div style='float:left;'><img src='" + datalist.HeadshotPathDesc + "' style='border-radius:300px'></div>";
                            html += "<div style ='float:left;'><div><span class='nickName'>" + datalist.NickName + "</span><span class='origion'>&nbsp;&nbsp;来自：" + datalist.AddTypeDesc + "</span></div>"
                            html += "<div class='describtion'>" + datalist.GenderDesc + " " + datalist.Age + "岁</div>";
                            html += "<div class='describtion'>附加消息：" + datalist.ReqContent + "</div></div>"
                            if (datalist.IsPassive) {
                                if (datalist.AddStat == 0) {
                                    html += "<div><div class='btngroup'>";
                                    html += "<div class='btn_validate' onclick='AddValidate(3," + datalist.Id + ")'>忽略</div>";
                                    html += "<div class='btn_validate' onclick='AddValidate(2," + datalist.Id + ")'>拒绝</div>";
                                    html += "<div class='btn_validate' onclick='AddValidate(1," + datalist.Id + ")'>同意</div></div></div>";
                                }
                                else {
                                    html += "<div><div class='hasreq'>" + datalist.AddStatDesc + "</div></div></div>";
                                }
                            }
                            else {
                                html += "<div><div class='hasreq'>" + datalist.AddStatDesc + "</div></div></div>";
                            }
                            $("#newFriend").append(html);
                        }
                    }
                })
}

//好友验证
function AddValidate(stat, Id) {
    var queryData = {
        AddStat: stat,
        Id: Id
    }
    $.post("/Friend/AddValidate", { data: $.toJSON(queryData) },
         function (data) {
             if (data.Data) {
                 InitTable();
                 NewFriend();
             }
         })
}

//发消息,向数据库插入一条空数据，使其在聊天列表中置顶
function SendMsh() {
    var queryData = {
        Type: 0,
        ChatContent:"",
        OriginId: $("#hiddenOriginId").val()
    }
    $.post("/Chat/AddPersonalChatHistory", { data: $.toJSON(queryData) },
         function (data) {
             if (data.Success) {
                 window.location.href = "/Chat/Index";
             }
         })
}

//获取当前用户Id
function SetOriginId(id) {
    $("#hiddenOriginId").val(id);
}

//获取当前用户Id
function SetReqId(id) {
    $("#hiddenReqId").val(id);
}

//删除好友
function DeleteFriend()
{
    var queryData = {
        OriginId: $("#hiddenOriginId").val()
    }
    $.post("/Friend/DeleteFriend", { data: $.toJSON(queryData)},
         function (data) {
             if (data.Success) {
                 RefrashTable();
             }
         })
}

//删除添加好友请求
function DeleteAddRequest() {
    var queryData = {
        Id: $("#hiddenReqId").val()
    }
    $.post("/Friend/DeleteAddRequest", { data: $.toJSON(queryData) },
         function (data) {
             if (data.Success) {
                 NewFriend();
             }
         })
}

//刷新列表
function RefrashTable()
{
    $("#table").bootstrapTable('refresh');
}