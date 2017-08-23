$(function () {
    window.onload = function () {
        Loading(true);
    }

    $(".ui-filter-text,.ui-filter-btn").click(function () {
        if ($(this).next('.ui-filter-list').is(":hidden")) {
            $(this).css('border-bottom-color', '#fff');
            $(".ui-filter-list").slideDown(10);
            $(this).addClass("active")
        } else {
            $(this).css('border-bottom-color', '#ccc');
            $(".ui-filter-list").slideUp(10);
            $(this).removeClass("active")
        }
    });
    $(".profile-nav li").click(function () {
        $(".profile-nav li").removeClass("active");
        $(".profile-nav li").removeClass("hover");
        $(this).addClass("active")
    }).hover(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("hover")
        }
    }, function () {
        $(this).removeClass("hover")
    })
})
Loading = function (bool, text) {
    var ajaxbg = top.$("#loading_background,#loading_manage");
    if (bool) {
        ajaxbg.show();
    } else {
        if (top.$("#loading_manage").attr('istableloading') == undefined) {
            ajaxbg.hide();
            top.$(".ajax-loader").remove();
        }
    }
    if (!!text) {
        top.$("#loading_manage").html(text);
    } else {
        top.$("#loading_manage").html("正在努力为您加载…");
    }
    top.$("#loading_manage").css("left", (top.$('body').width() - top.$("#loading_manage").width()) / 2 - 54);
    top.$("#loading_manage").css("top", (top.$('body').height() - top.$("#loading_manage").height()) / 2);
}
dialogTop = function (content, type) {
    $(".tip_container").remove();
    var bid = parseInt(Math.random() * 100000);
    $("body").prepend('<div id="tip_container' + bid + '" class="container tip_container"><div id="tip' + bid + '" class="mtip"><i class="micon"></i><span id="tsc' + bid + '"></span><i id="mclose' + bid + '" class="mclose"></i></div></div>');
    var $this = $(this);
    var $tip_container = $("#tip_container" + bid);
    var $tip = $("#tip" + bid);
    var $tipSpan = $("#tsc" + bid);
    //先清楚定时器
    clearTimeout(window.timer);
    //主体元素绑定事件
    $tip.attr("class", type).addClass("mtip");
    $tipSpan.html(content);
    $tip_container.slideDown(300);
    //提示层隐藏定时器
    window.timer = setTimeout(function () {
        $tip_container.slideUp(300);
        $(".tip_container").remove();
    }, 4000);
    $("#tip_container" + bid).css("left", ($(window).width() - $("#tip_container" + bid).width()) / 2);
}
dialogOpen = function (options) {
    Loading(true);
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        isDisplayDialogClose: true,//是否显示弹框底部的"确认并关闭窗口"选项
        btn: ['确认', '关闭'],
        callBack: null
    };
    var options = $.extend(defaults, options);
    var _url = options.url;
    var _width = top.$.windowWidth() > parseInt(options.width.replace('px', '')) ? options.width : top.$.windowWidth() + 'px';
    var _height = top.$.windowHeight() > parseInt(options.height.replace('px', '')) ? options.height : top.$.windowHeight() + 'px';
    top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: top.contentPath + _url,
        isDisplayDialogClose: options.isDisplayDialogClose,//是否显示弹框底部的"确认并关闭窗口"选项
        btn: options.btn,
        yes: function () {
            options.callBack(options.id)
        }, cancel: function () {
            return true;
        }
    });
}
dialogContent = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        content: '',
        btn: ['确认', '关闭'],
        callBack: null
    };
    var options = $.extend(defaults, options);
    top.layer.open({
        id: options.id,
        type: 1,
        title: options.title,
        fix: false,
        area: [options.width, options.height],
        content: options.content,
        btn: options.btn,
        yes: function () {
            options.callBack(options.id)
        }
    });
}
dialogAlert = function (content, type) {
    if (type == -1) {
        type = 2;
    }
    top.layer.alert(content, {
        icon: type,
        title: "提示"
    });
}
dialogConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: 7,
        title: "提示",
        btn: ['确认', '取消'],
    }, function (index) {
        callBack(true);
    }, function () {
        callBack(false)
    });
}
dialogMsg = function (content, type) {
    if (type == -1) {
        type = 2;
    }
    top.layer.msg(content, { icon: type, time: 4000, shift: 5 });
}
dialogClose = function () {
    try {
        var index = top.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
        var IsClose = $IsdialogClose.is(":checked");
        if ($IsdialogClose.length == 0) {
            IsClose = true;
        }
        if (IsClose) {
            top.layer.close(index);
        } else {
            location.reload();
        }
    } catch (e) {
        alert(e)
    }
}
reload = function () {
    location.reload();
    return false;
}
formatDate = function (v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
    }
    var o = {
        "M+": d.getMonth() + 1,  //month
        "d+": d.getDate(),       //day
        "h+": d.getHours(),      //hour
        "m+": d.getMinutes(),    //minute
        "s+": d.getSeconds(),    //second
        "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
        "S": d.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};
toDecimal = function (num) {
    if (num == null) {
        num = "0";
    }
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '' +
                num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}
Date.prototype.DateAdd = function (strInterval, Number) {
    //y年 q季度 m月 d日 w周 h小时 n分钟 s秒 ms毫秒
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}
request = function (keyValue) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == keyValue) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
changeUrlParam = function (url, key, value) {
    var newUrl = "";
    var reg = new RegExp("(^|)" + key + "=([^&]*)(|$)");
    var tmp = key + "=" + value;
    if (url.match(reg) != null) {
        newUrl = url.replace(eval(reg), tmp);
    } else {
        if (url.match("[\?]")) {
            newUrl = url + "&" + tmp;
        }
        else {
            newUrl = url + "?" + tmp;
        }
    }
    return newUrl;
}

$.isbrowsername = function () {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    }; //判断是否Opera浏览器
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    } //判断是否Firefox浏览器
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }//判断是否Chrome浏览器//360浏览器
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    } //判断是否Safari浏览器
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    }; //判断是否IE浏览器
}
$.download = function (url, data, method) {
    if (url && data) {
        data = typeof data == 'string' ? data : jQuery.param(data);
        var inputs = '';
        $.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    };
};
$.standTabchange = function (object, forid) {
    $(".standtabactived").removeClass("standtabactived");
    $(object).addClass("standtabactived");
    $('.standtab-pane').css('display', 'none');
    $('#' + forid).css('display', 'block');
}
$.isNullOrEmpty = function (obj) {
    if ((typeof (obj) == "string" && obj == "") || obj == null || obj == undefined) {
        return true;
    }
    else {
        return false;
    }
}
$.arrayClone = function (data) {
    return $.map(data, function (obj) {
        return $.extend(true, {}, obj);
    });
}
$.windowWidth = function () {
    return $(window).width();
}
$.windowHeight = function () {
    return $(window).height();
}
IsNumber = function (obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}
IsMoney = function (obj) {
    $("#" + obj).bind("contextmenu", function () {
        return false;
    });
    $("#" + obj).css('ime-mode', 'disabled');
    $("#" + obj).bind("keydown", function (e) {
        var key = window.event ? e.keyCode : e.which;
        if (isFullStop(key)) {
            return $(this).val().indexOf('.') < 0;
        }
        return (isSpecialKey(key)) || ((isNumber(key) && !e.shiftKey));
    });
    function isNumber(key) {
        return key >= 48 && key <= 57
    }
    function isSpecialKey(key) {
        return key == 8 || key == 46 || (key >= 37 && key <= 40) || key == 35 || key == 36 || key == 9 || key == 13
    }
    function isFullStop(key) {
        return key == 190 || key == 110;
    }
}

$.httpPostJson = function (option) {
    //     发起 ajax POST请求并返回结果
    ///	</summary>
    ///	<param name="option" type="Object">
    ///		参数对象
    ///	</param>
    ///	<returns type="Json" />
    option.type = "POST";
    option.dataType = "json";
    option.async = false;
    return _ajax(option);
};
$.httpGetJson = function (option) {
    //     发起 ajax Get请求并返回结果
    ///	</summary>
    ///	<param name="option" type="Object">
    ///		参数对象
    ///	</param>
    ///	<returns type="Json" />
    option.type = "GET";
    option.dataType = "json";
    option.async = false;
    return _ajax(option);
};
function _ajax(option) {
    if (option.headers) {
        option.headers["x-charset"] = "utf-8";
    }
    else {
        option.headers = { "x-charset": "utf-8" };
    }
    var _data = null;
    option.success = function (data) {
        _data = data;
    }
    $.ajax(option);
    return _data;
}