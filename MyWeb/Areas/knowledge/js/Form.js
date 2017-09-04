//新增分享内容
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
    knowledgeInfo.keyValue = "";
    $.SaveForm({
        url: "/Knowledge/Knowledge/SubmitForm",
        param: knowledgeInfo,
        success: function (data) {
            window.location.href = "/Knowledge/Knowledge/Index";
        }
    });
}








