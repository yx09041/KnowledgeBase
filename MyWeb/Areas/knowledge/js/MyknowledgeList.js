
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
    Loading(true);
    var data = {
        page: nCurrentPage+1,
        rows: nPageRows,
        sidx: "UpdateDate",
        sord: "desc"
    };
    $.ajax({
        type: "get",
        async: false,
        url: "/Knowledge/Knowledge/GetMyListJson",
        dataType: "json",
        data: data,
        success: function (responseData) {
            Loading(false);
            nTotal = responseData.total;
            var html = template('showContentTemplate', responseData.data);
            document.getElementById('listContent').innerHTML = html;
            initpagination();
        },
        error: function (responseData) {
            Loading(false);
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

//查看
function openDetaile(id) {
    window.open("/Knowledge/Knowledge/Detail?id=" + id);
}

//编辑
function Editknowledge(id) {
    window.open("/Knowledge/Knowledge/Edit?id=" + id);
}








