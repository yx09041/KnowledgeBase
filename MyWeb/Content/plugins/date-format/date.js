//easyui 日期格式化
function dateformatter(value,format) {
    if (!value) {
        return "";
    }
    var date = parseISO8601(value);
    return date.format(format);
}

/**Parses string formatted as YYYY-MM-DD to a Date object.
  * If the supplied string does not match the format, an
  * invalid Date (value NaN) is returned.
  * @param {string} dateStringInRange format YYYY-MM-DD, with year in
  * range of 0000-9999, inclusive.
  * @return {Date} Date object representing the string.
  */
function parseISO8601(dateStringInRange) {
    var isoExp = /^[\s\S]*(\d{4})-(\d\d)-(\d\d)[\s\S]*$/,
        date = new Date(NaN), month,
        parts = isoExp.exec(dateStringInRange);

    if (parts) {
        month = +parts[2];
        date.setFullYear(parts[1], month - 1, parts[3]);
        if (month != date.getMonth() + 1) {
            date.setTime(NaN);
        }
    }
    return date;
}


/**  
* 时间对象的格式化  
*/
Date.prototype.format = function (format) {
    /*  
    * format="yyyy-MM-dd hh:mm:ss";  
    */
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
            ? o[k]
            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
