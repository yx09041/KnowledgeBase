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








