//新增分享内容
function onBtnSave() {
    if ($("#txtTitle").val() == "") {
        DialogTip.ShowWarning("请填写标题");
        return;
    }
    if ($("#txtSummary").val() == "") {
        DialogTip.ShowWarning("请填写摘要");
        return;
    }
    if ($("#txtDataType").val() == "") {
        DialogTip.ShowWarning("请选择分类");
        return;
    }
    if (!UE.getEditor('editor').hasContents()) {
        DialogTip.ShowWarning("分享无内容");
        return;
    }
    var loadInfoDialog = Loading.ShowLoading("提交中...");
    var knowledgeInfo = {
        Title: $("#txtTitle").val(),
        Summary: $("#txtSummary").val(),
        ContentHtml: UE.getEditor('editor').getContent(),
        ContentNoHtml: UE.getEditor('editor').getContentTxt(),
        DataType: $("#txtDataType").val()
    };
    $.ajax({
        type: "post",
        async: false,
        url: "/api/knowledgeManage/add/",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({ "knowledgeInfo": knowledgeInfo }),
        success: function (responseData) {
            if (responseData.Result) {
                Loading.ShowSuccess(loadInfoDialog, "保存成功！");
                window.location.href = "knowledgeList";
            }
            else {
                Loading.ShowFail(loadInfoDialog, "保存失败！");
            }
        },
        error: function (responseData) {
            Loading.ShowFail(loadInfoDialog, "数据保存失败，请关闭重试！");
        }
    });
}








