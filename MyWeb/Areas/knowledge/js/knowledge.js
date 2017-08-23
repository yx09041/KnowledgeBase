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
    //校验
    $.getJSON("/api/knowledgeManage/GetModel/" + id, function (oRtn) {
        if (!oRtn) {
            return;
        }
        var html = template('showContentTemplate', oRtn);
        document.getElementById('content').innerHTML = html;
    });
}








