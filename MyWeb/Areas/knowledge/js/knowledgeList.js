
var nCurrentPage = 0;
var nPageRows = 10;
var nTotal = 0;
$(function () {
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
    var data = {
        page: nCurrentPage+1,
        rows: nPageRows,
        sidx: "CreateDate",
        sord: "desc"
    };
    $.ajax({
        type: "get",
        async: false,
        url: "/Knowledge/Knowledge/GetListJson",
        dataType: "json",
        data: data,
        success: function (responseData) {
            nTotal = responseData.total;
            var html = template('showContentTemplate', responseData.data);
            document.getElementById('listContent').innerHTML = html;
            initpagination();
        },
        error: function (responseData) {
            Loading.ShowFail(loadInfoDialog, "数据加载失败，请关闭重试！");
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
    window.location.href = "/Knowledge/Knowledge/Detail?id=" + id;
}

function addknowledge() {
    window.location.href = "knowledgeEdit";
}


//加载列表数据
function queryList() {
    if ($("#KeyValue").val() == "") {
        loadList();
        return;
    }
    //var loadInfoDialog = Loading.ShowLoading("加载中...");
    var data = {
        page: nCurrentPage + 1,
        rows: nPageRows,
        sort: "CreateDate",
        order: "desc",
        KeyValue: $("#KeyValue").val()
    };
    $.ajax({
        type: "get",
        async: false,
        url: "/Knowledge/Knowledge/QueryListJson",
        dataType: "json",
        data: data,
        success: function (responseData) {
            debugger;
            nTotal = responseData.total;
            template.config("escape", false);
            var html = template('showResultContentTemplate', responseData.data);
            document.getElementById('listContent').innerHTML = html;
            initpagination();
            //Loading.CloseDialog(loadInfoDialog);
        },
        error: function (responseData) {
            //Loading.ShowFail(loadInfoDialog, "数据加载失败，请关闭重试！");
        }
    });
}


//创建索引
function CreateIndex() {
    var loadInfoDialog = Loading.ShowLoading("正在创建索引...");
    //校验
    $.getJSON("/api/knowledgeManage/CreateIdex/", function (oRtn) {
        if (!oRtn) {
            Loading.CloseDialog(loadInfoDialog);
            return;
        }
        if (!oRtn.Result) {
            Loading.ShowFail(loadInfoDialog, oRtn.ErrorMessage);
        }
        else {
            Loading.ShowSuccess(loadInfoDialog, "创建索引成功");
        }
    });
}









