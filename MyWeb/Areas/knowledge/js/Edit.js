var keyValue = $.request("id");
var ue;
$(function () {
    //实例化编辑器
    //建议使用工厂方法getEditor创建和引用编辑器实例，如果在某个闭包下引用该编辑器，直接调用UE.getEditor('editor')就能拿到相关的实例
    ue = UE.getEditor('editor');
    initControl();
});

function initControl() {
    $.SetForm({
        url: "/Knowledge/Knowledge/GetFormJson",
        param: { keyValue: keyValue },
        success: function (data) {
            if (!data) {
                return;
            }
            $("#txtTitle").val(data.Title);
            $("#txtSummary").val(data.Summary);
            $("#txtDataType").val(data.DataType);
            ue.ready(function () {
                ue.setContent(data.ContentHtml);
            });
        }
    });
}


//保存分享内容
function onBtnSave() {
    if ($("#txtTitle").val() == "") {
        dialogAlert("请填写标题",2);
        return;
    }
    if ($("#txtSummary").val() == "") {
        dialogAlert("请填写摘要", 2);
        return;
    }
    if ($("#txtDataType").val() == "") {
        dialogAlert("请选择分类", 2);
        return;
    }
    if (!UE.getEditor('editor').hasContents()) {
        dialogAlert("分享无内容", 2);
        return;
    }
    var knowledgeInfo = {
        Title: $("#txtTitle").val(),
        Summary: $("#txtSummary").val(),
        ContentHtml: UE.getEditor('editor').getContent(),
        ContentNoHtml: UE.getEditor('editor').getContentTxt(),
        DataType: $("#txtDataType").val()
    };
    knowledgeInfo.keyValue = keyValue;
    $.SaveForm({
        url: "/Knowledge/Knowledge/SubmitForm",
        param: knowledgeInfo,
        success: function (data) {
            window.location.href = "/Knowledge/Knowledge/MyIndex";
        }
    });
}








