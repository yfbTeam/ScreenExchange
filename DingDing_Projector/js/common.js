//错误处理
function OnError(XMLHttpRequest, textStatus, errorThrown) {

}

function MesTips(MesContent) {
    $('body').append('<div class="screen_success"><div class="wenzi"></div></div>');
    $('.screen_success .wenzi').html(MesContent);

    setTimeout(function () {
        $('.screen_success').remove();
    }, 2000);
}

/**日期转换成时间字符串**/
/** date日期  **/
/** format要转换的字符串格式，默认为 "yyyy-MM-dd"格式，若没有需要的格式可自己添加 **/
function DateTimeConvert(date, format, iseval) {
    iseval = iseval == false ? false : true;
    if (iseval) {
        date = date.replace(/(^\s*)|(\s*$)/g, "");
        if (date == "") {
            return "";
        }
        if (date.indexOf("Date") != -1) {
            date = eval(date.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
        } else {
            date = new Date(Date.parse(date));
        }
    }
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString();
    var twoMonth = month.length == 1 ? "0" + month : month; //月份为1位数时，前面加0
    var day = (date.getDate()).toString();
    var twoDay = day.length == 1 ? "0" + day : day; //天数为1位数时，前面加0
    var hour = (date.getHours()).toString();
    var twoHour = hour.length == 1 ? "0" + hour : hour; //小时数为1位数时，前面加0
    var minute = (date.getMinutes()).toString();
    var twoMinute = minute.length == 1 ? "0" + minute : minute; //分钟数为1位数时，前面加0
    var second = (date.getSeconds()).toString();
    var twoSecond = second.length == 1 ? "0" + second : second; //秒数为1位数时，前面加0
    var dateTime;
    if (format == "yyyy-MM-dd HH:mm:ss") {
        dateTime = year + "-" + twoMonth + "-" + twoDay + " " + twoHour + ":" + twoMinute + ":" + twoSecond;
    } else if (format == "yyyy-MM-dd HH:mm") {
        dateTime = year + "-" + twoMonth + "-" + twoDay + " " + twoHour + ":" + twoMinute;
    } else if (format == "年月日") {
        dateTime = year + "年" + month + "月" + day + "日";
    } else if (format == "yyyy-MM") {
        dateTime = year + "-" + twoMonth
    } else if (format == "MM-dd") {
        dateTime = twoMonth + "-" + twoDay
    }
    else if (format == "dd") {
        dateTime = twoDay;
    } else if (format == "HH:mm:ss") {
        dateTime = twoHour + ":" + twoMinute + ":" + twoSecond;
    }
    else {
        dateTime = year + "-" + twoMonth + "-" + twoDay
    }
    return dateTime;
}


function uniqueArry(arr) {
    var result = [], hash = {};
    for (var i = 0, elem; (elem = arr[i]) != null; i++) {
        if (!hash[elem]) {
            result.push(elem);
            hash[elem] = true;
        }
    }
    return result;
}