/*
 * dialog
 * http://sufangyu.github.io
 * 方雨_Yu
 * 1.0.0(2016-06-04)
 */
;(function(win,$){

    /*
     * Private methods 
     */    
    var wrap, overlay, content, title, close, cancelBtn, okBtn, delBtn, settings, timer;


    var  defaultsTip = {
        type : 'alert',     // alert、confirm、info
        titleText : '信息提示',
        showTitle : true,
        contentHtml : '',
        dialogClass : '',
        autoClose : 0,
        overlayClose : false,
        drag : false,

        buttonText : {
            ok : '确定',
            cancel : '取消',
            delete : '删除'
        },
        buttonClass : {
            ok : '',
            cancel : '',
            delete : ''
        },

        infoText : '',      // working in info type
        infoIcon : '',      // working in info type

        onClickOk : function(){},
        onClickCancel : function(){},
        onClickClose : function(){},

        onBeforeShow : function(){},
        onShow : function(){},
        onBeforeClosed : function(){},
        onClosed : function(){}
    };

    var _renderDOM = function(){
        if( $('.dialog-wrap').length > 0){
            return;
        }

        clearTimeout(timer);
        settings.onBeforeShow();
        
        $('body').append( dialogWrapper = $('<div class="dialog-wrap '+ settings.dialogClass +'"></div>') );
        dialogWrapper.append(
            overlay = $('<div class="dialog-overlay"></div>'),
            content = $('<div class="dialog-content"></div>')
        );

        switch (settings.type){
            case 'alert' :
                if(settings.showTitle){
                    content.append(
                        title = $('<div class="dialog-content-hd"><h4 class="dialog-content-title">'+ settings.titleText +'</h4></div>')
                    );
                }
                content.append(
                    contentBd = $('<div class="dialog-content-bd">'+ settings.contentHtml +'</div>')
                );
                content.append(
                    contentFt = $('<div class="dialog-content-ft"></div>')                   
                );
                contentFt.append(
                    okBtn = $('<a class="dialog-btn dialog-btn-ok '+ settings.buttonClass.ok +'" href="javascript:;">'+ settings.buttonText.ok +'</a>')
                );
                break;

            case 'confirm' :
                if(settings.showTitle){
                    content.append(
                        title = $('<div class="dialog-content-hd"><h4 class="dialog-content-title">'+ settings.titleText +'</h4></div>')
                    );
                }
                content.append(
                    contentBd = $('<div class="dialog-content-bd">'+ settings.contentHtml +'</div>')
                );
                content.append(
                    contentFt = $('<div class="dialog-content-ft"></div>')
                );
                contentFt.append(
                    cancelBtn = $('<a class="dialog-btn dialog-btn-cancel '+ settings.buttonClass.cancel +'" href="javascript:;">'+ settings.buttonText.cancel +'</a>'),
                    okBtn = $('<a class="dialog-btn dialog-btn-ok '+ settings.buttonClass.ok +'" href="javascript:;">'+ settings.buttonText.ok +'</a>')
                );
                break;

            case 'info' :
                var infoContent = settings.contentHtml || '<img class="info-icon" src="'+ settings.infoIcon +'" alt="'+ settings.infoText +'" /><p class="info-text">'+ settings.infoText +'</p>';
                content.append(
                    contentBd = $('<div class="dialog-content-bd">'+ infoContent +'</div>')
                );
                dialogWrapper.addClass('dialog-wrap-info');
                content.addClass('dialog-content-info');
                break;

            default :
                break;
        }

        setTimeout(function(){
            dialogWrapper.addClass('dialog-wrap-show');
            settings.onShow();
        }, 10);

    };

    var _bindEvent = function() {

        $(okBtn).on('click', function(e){
            settings.onClickOk();
            $.dialog.close();
            return false;
        });

        $(cancelBtn).on('click', function(e){
            settings.onClickCancel();
            $.dialog.close();
            return false;
        });

        // overlay clisk hide
        if( settings.overlayClose ){
            overlay.on('click', function(e){
                $.dialog.close();
            });
        }

        // auto close, set autoClose and type isn't info
        if( settings.autoClose > 0 ){
            _autoClose();
        }

    };

    var _autoClose = function(){
        clearTimeout(timer);
        timer = window.setTimeout(function(){
            $.dialog.close();
        }, settings.autoClose);
    };



    /*
     * Public methods 
     */

    $.dialog = function(options) {
        settings = $.extend({},defaultsTip, options);        
        $.dialog.init();
        return this;
    };   

    $.dialog.init = function(){
        _renderDOM();
        _bindEvent();
    };


    $.dialog.close = function(){
        settings.onBeforeClosed();

        dialogWrapper.removeClass('dialog-wrap-show');
        setTimeout(function(){
            dialogWrapper.remove();
            settings.onClosed();
        }, 200);
    };

    $.dialog.update = function(params) {
        if(params.infoText) {
            content.find('.info-text').html(params.infoText);
        }
        if(params.infoIcon) {            
            content.find('.info-icon').attr('src', params.infoIcon);
        }
        if(params.autoClose>0){
            window.setTimeout(function(){
                $.dialog.close();
            }, params.autoClose);
        }
    };


    // 插件
    $.fn.dialog = function(options){
        return this;
    };


   

})(window, window.Zepto || window.jQuery);




/*
*消息提示公共封装
*/
var Loading = {
    /*
    * 正在加载
    *
    */
    ShowLoading: function (message) {
        var infoDialog = $.dialog({
            type: 'info',
            infoText: message,
            infoIcon: '/Content/jquery.TipDialog/images/icon/loading.gif'
        });
        return infoDialog;
    },

    /*
    * 加载成功
    *
    */
    ShowSuccess: function (infoDialog, message) {
        infoDialog.dialog.update({
            autoClose: 1500,
            infoText: message,
            infoIcon: '/Content/jquery.TipDialog/images/icon/success.png'
        });
    },
    ShowFail: function (infoDialog, message) {
        infoDialog.dialog.update({
            autoClose: 1500,
            infoText: message,
            infoIcon: '/Content/jquery.TipDialog/images/icon/fail.png'
        });
    },
    CloseDialog:function(infoDialog){
       infoDialog.dialog.close();
    }
}


/*
*警告
*/
var DialogTip = {
    //提示
    ShowWarning: function (message) {
        $.dialog({
            titleText : '消息提示',
            showTitle : true,
            //autoClose: 2000,
            contentHtml: message
        });
    },
    ShowError: function (message) {
        $.dialog({
            //autoClose: 1500,
            type: 'info',
            infoText: message,
            infoIcon: '/Content/jquery.TipDialog/images/icon/fail.png'
        });
    }
}