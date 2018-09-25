
//Main
$(function () {
    InitTable();
    GenderCombobox();
    AgeCombobox();
    ProvinceCombobox();
    InitClick();
});

//用户列表
function InitTable() {
    $("#table").bootstrapTable({
        url: '/Add/GetUserList',
        showHeader: false,
        cache: false,
        striped: false,  //表格显示条纹，默认为false
        clickToSelect: true,
        paginationLoop: false, //是否开启无限循环
        pagination: false,
        sidePagination: 'server',
        queryParams: function (params) {
            return {
                PageIndex: $('#pageIndex').val()
            }
        },
        columns: [
            {
                field: 'imgUrl',
                title: '头像',
                align: 'left',
                valign: 'middle',
                width: '8%',
                formatter: function (value, row, index) {
                    if (row.Column1 == null) {
                        return "";
                    }
                    return '<img src="' + row.Column1.HeadshotPathDesc + '" style="border-radius:300px;float:left" data-toggle="context" data-target="#context-menu" >';
                }
            },
            {
                field: 'NickName', align: 'left', valign: 'middle', width: '17%',
                formatter: function (value, row, index) {
                    if (row.Column1 == null) {
                        return "";
                    }
                    var html = '<div><div style="font-size:14px">' + row.Column1.NickName + '</div>';
                    html += '<div style="font-size:12px;margin-bottom: 2px; ">' + '女 21 北京' + '</div>';
                    html += '<div class="addFriend" onclick="AddFriend(' + row.Column1.Id + ')" >' + '+  好友' + '</div></div>';
                    return html;
                }
            },
            {
                field: 'imgUrl',
                title: '头像',
                align: 'left',
                valign: 'middle',
                width: '8%',
                formatter: function (value, row, index) {
                    if (row.Column2 == null) {
                        return "";
                    }
                    return '<img src="' + row.Column2.HeadshotPathDesc + '" style="border-radius:300px;float:left">';
                }
            },
            {
                field: 'NickName', align: 'left', valign: 'middle', width: '17%',
                formatter: function (value, row, index) {
                    if (row.Column2 == null) {
                        return "";
                    }
                    var html = '<div><div style="font-size:14px">' + row.Column2.NickName + '</div>';
                    html += '<div style="font-size:12px;margin-bottom: 2px; ">' + '北京' + '</div>';
                    html += '<div class="addFriend" onclick="AddFriend(' + row.Column2.Id + ')">' + '+好友' + '</div></div>';
                    return html;
                }
            },
            {
                field: 'imgUrl',
                title: '头像',
                align: 'left',
                valign: 'middle',
                width: '8%',
                formatter: function (value, row, index) {
                    if (row.Column3 == null) {
                        return "";
                    }
                    return '<img src="' + row.Column3.HeadshotPathDesc + '" style="border-radius:300px;float:left">';
                }
            },
            {
                field: 'NickName', align: 'left', valign: 'middle', width: '17%',
                formatter: function (value, row, index) {
                    if (row.Column3 == null) {
                        return "";
                    }
                    var html = '<div><div style="font-size:14px">' + row.Column3.NickName + '</div>';
                    html += '<div style="font-size:12px;margin-bottom: 2px; ">' + '北京' + '</div>';
                    html += '<div class="addFriend" onclick="AddFriend(' + row.Column3.Id + ')">' + '+好友' + '</div></div>';
                    return html;
                }
            },
            {
                field: 'imgUrl',
                title: '头像',
                align: 'left',
                valign: 'middle',
                width: '8%',
                formatter: function (value, row, index) {
                    if (row.Column4 == null) {
                        return "";
                    }
                    return '<img src="' + row.Column4.HeadshotPathDesc + '" style="border-radius:300px;float:left">';
                }
            },
            {
                field: 'NickName', align: 'left', valign: 'middle', width: '17%',
                formatter: function (value, row, index) {
                    if (row.Column4 == null) {
                        return "";
                    }
                    var html = '<div><div style="font-size:14px">' + row.Column4.NickName + '</div>';
                    html += '<div style="font-size:12px;margin-bottom: 2px; ">' + '北京' + '</div>';
                    html += '<div class="addFriend" onclick="AddFriend(' + row.Column4.Id + ')">' + '+  好友' + '</div></div>';
                    return html;
                }
            },

        ],
        onLoadSuccess: function (data) {
        },
        onClickCell: function (field, value, row, $element) {
        }
    })
}

//上一页
function UpPage() {
    var pageIndex = parseInt($('#pageIndex').val());
    if (pageIndex == 1) {
        return;
    }
    else {
        $('#pageIndex').val(pageIndex - 1);
        AddTableRefresh();
    }
}

//下一页
function DownPage() {
    var pageIndex = parseInt($('#pageIndex').val());
    $('#pageIndex').val(pageIndex + 1);
    AddTableRefresh();
}

//刷新列表
function AddTableRefresh() {
    $("#table").bootstrapTable('refresh');
}

//搜索
function SearchClick() {
    //查找都是第一页
    $('#pageIndex').val(1);
    AddTableRefresh();
}