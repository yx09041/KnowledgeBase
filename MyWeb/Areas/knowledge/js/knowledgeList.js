
var nCurrentPage = 0;
var nPageRows = 10;
var nTotal = 0;
$(function () {
    //回车键
    document.onkeydown = function (e) {
        if (!e) e = window.event;
        if ((e.keyCode || e.which) == 13) {
            var btnSearch = document.getElementById("btnSearch");
            btnSearch.click();
        }
    }

    //加载数据
    loadList();
    initpagination();

   
})
template.config("escape", false);

template.helper("dateformatter", dateformatter);
template.helper("contentformatter", contentformatter);
function contentformatter(content) {
    if (!content) {
        return "";
    }
    else {
        if (content.length > 200) {
            return content.substring(0, 200) + "……";
        }
    }
    return content;
}

//加载列表数据
function loadList() {
    var param = {
        page: nCurrentPage+1,
        rows: nPageRows,
        sidx: "UpdateDate",
        sord: "desc"
    };
    $.GetListDataAjax({
        url: "/Knowledge/Knowledge/GetListJson",
        param: param,
        success: function (data) {
            nTotal = data.total;
            var html = template('showContentTemplate', data.data);
            document.getElementById('listContent').innerHTML = html;
            initpagination();
        }
    });
}


function initpagination() {
    var optInit = {
        items_per_page: nPageRows,
        callback: ChgPageIndex,
        current_page: nCurrentPage
    };
    $("#Pagination").pagination(nTotal, optInit);
}



function ChgPageIndex(currentPage) {
    if (nCurrentPage != currentPage) {
        nCurrentPage = currentPage;
        loadList();
    }
}


function openDetaile(id) {
    window.open("/Knowledge/Knowledge/Detail?id=" + id);
}

function addknowledge() {
    window.location.href = "/Knowledge/Knowledge/Form";
}


//加载列表数据
function queryList() {
    if ($("#KeyValue").val() == "") {
        loadList();
        return;
    }
    Loading(true);
    //var loadInfoDialog = Loading.ShowLoading("加载中...");
    var data = {
        page: nCurrentPage + 1,
        rows: nPageRows,
        sort: "UpdateDate",
        order: "desc",
        KeyValue: $("#KeyValue").val()
    };

    $.GetListDataAjax({
        url: "/Knowledge/Knowledge/QueryListJson",
        param: data,
        success: function (responseData) {
            nTotal = responseData.total;
            template.config("escape", false);
            var html = template('showResultContentTemplate', responseData.data);
            document.getElementById('listContent').innerHTML = html;
            initpagination();
        }
    });
}


//重新创建全文索引
function CreateIndex() {
    $.ConfirmAjax({
        msg:"确认创建全文索引？该功能将花费较长时间。",
        loading: "正在重新创建索引...",
        url: "/Knowledge/Knowledge/CreateIndex",
        success: function (data) {

        }
    });
}









