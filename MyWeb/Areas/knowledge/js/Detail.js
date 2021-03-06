var keyValue = $.request("id");
template.helper("dateformatter", dateformatter);
$(function () {
    LoadInfo();
    document.getElementById('MainContent').innerHTML = $("#hidShowContent").val();
    uParse('#MainContent', {
        rootPath: '/Content/ueditor/'
    })
});


//加载单条数据
function LoadInfo() {
    var id = $("#knowledgeGUID").val();
    $.SetForm({
        url: "/Knowledge/Knowledge/GetFormJson",
        param: { keyValue: id },
        success: function (data) {
            if (!data) {
                return;
            }
            var html = template('showContentTemplate', data);
            document.getElementById('content').innerHTML = html;
        }
    });
}

//收藏
function StoreKnowledge() {
    $.ConfirmAjax({
        url: "/Knowledge/Knowledge/StoreKnowledge",
        param: { knowledgeGUID: keyValue },
        msg: "注：您确定要收藏吗？",
        success: function (data) {
           
        }
    });
}








