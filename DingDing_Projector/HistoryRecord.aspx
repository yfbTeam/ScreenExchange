<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryRecord.aspx.cs" Inherits="DingDing_Projector.HistoryRecord" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <title>历史记录</title>
    <link rel="stylesheet" href="css/reset.css" />
    <link rel="stylesheet" href="css/style.css" />
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="js/jquery.tmpl.js"></script>
    <script src="js/common.js"></script>
    <%-- <script id="liYear" type="text/x-jquery-tmpl">
        <li name="${year}" id="${year}">
            <h1>${year}年<i class="icon_right"></i></h1>
            <ul id="MonthList${year}">
            </ul>
        </li>
    </script>--%>
    <script id="liMonth" type="text/x-jquery-tmpl">
        <li>
            <h1>${DateTimeConvert(date,'年月日')}<i class="icon_right"></i></h1>
            <ul id="StrList${DateTimeConvert(date)}">
            </ul>
        </li>
    </script>
    <script id="listr" type="text/x-jquery-tmpl">
        <li>
            <div class="user_time">
                <b></b>${DateTimeConvert2(BeginDate,'Hms')} ${UserName}使用了<em style="color: #45cc84">
                     {{if ComputerID==9}}外接
                     {{else}}${ComputerID}号
                     {{/if}}

                                                                          </em>电脑<em style="color: #45cc84">进行</em>了投屏
                <%--  {{if EndDate !=''}}
                 <br />
                ${DateTimeConvert2(EndDate,'Hms')} ${UserName}使用的<em style="color: #d64545">${ComputerID}号</em>电脑<em style="color: #d64545">断开</em>了投屏
                    {{/if}}--%>
            </div>
            <span class="squa"></span>
        </li>
    </script>
</head>
<body>
    <header id="header">
        <a href="screen_manage.aspx" class="back"></a>
        <span class="title">历史记录</span>
        <%--<a class="search" href="search.html"></a>--%>
    </header>
    <div id="main">
        <div class="record_screen">
            <label id="title"></label>
        </div>
        <div class="record_wrap">
            <ul class="item" id="ulstr">
            </ul>
        </div>
    </div>
    <script>
        $(function () {
            Projector();
        })

        function cli() {
            $('.record_wrap').find('li:has(ul)').children('h1').click(function () {
                var $next = $(this).next('ul');
                if ($next.is(':hidden')) {
                    $(this).parent('li').siblings('li').children('h1').removeClass('selected');
                    $(this).addClass('selected');
                    $next.slideDown();
                    if ($(this).parent('li').siblings('li').children('ul').is(':visible')) {
                        $(this).parent("li").siblings("li").find("h1").removeClass('selected');
                        $(this).parent("li").siblings("li").find("ul").slideUp();
                    }
                } else {
                    $(this).removeClass('selected');
                    $next.stop().slideUp();
                    $(this).next("ul").children("li").find("ul").slideUp();
                    $(this).next("ul").children("li").find("h1").removeClass('selected');
                }
            })
            $('.record_wrap>ul>li:eq(0)>h1').addClass('selected');
            $('.record_wrap>ul>li:eq(0)>ul').slideDown();
        }

        function Projector() {
            $.ajax({
                type: "post",
                url: "ProjectorHandler.ashx",
                dataType: "json",
                data: { "action": "Projector" },
                success: function (json) {
                    if (json.result.Status == "ok") {
                        var itemDatas = json.result.Data
                        if (itemDatas != null && itemDatas.PagedData.length > 0) {
                            var items = json.result.Data.PagedData;
                            var dates = [];
                            for (var i = 0; i < items.length; i++) {
                                if (items[i].date != "") {
                                    dates.push(items[i].date);
                                }
                            }
                            $("#title").html("近一个月,共有" + dates.length + "次投屏记录");
                            if (dates.length > 0) {
                                var udate = uniqueArry(dates);
                                var dataItem = [];
                                for (var i = 0; i < udate.length; i++) {
                                    var obj = new Object();
                                    obj.date = udate[i];
                                    dataItem.push(obj);
                                }
                                $("#liMonth").tmpl(dataItem).appendTo("#ulstr");
                                for (var j = 0; j < udate.length; j++) {
                                    var liitem = [];
                                    var datestr = udate[j];
                                    for (var z = 0; z < items.length; z++) {
                                        if (items[z].date == udate[j]) {
                                            liitem.push(items[z]);
                                        }
                                    }
                                    if (liitem.length > 0) {
                                        $("#listr").tmpl(liitem).appendTo("#StrList" + datestr);
                                    }
                                }
                                cli();
                            }
                        } else {
                            $("#ulstr").html('<div style="background:#fff url(images/norecord.jpg) no-repeat center;background-size:200px auto;height:200px;"></div>');
                            $("#title").html("近一个月,共有0次投屏记录");
                        }

                    } else if (json.result.Status == "null") {
                        $("#ulstr").html('<div style="background:#fff url(images/norecord.jpg) no-repeat center;background-size:200px auto;height:200px;"></div>');
                        $("#title").html("近一个月,共有0次投屏记录");
                    } else {
                        MesTips("系统异常请联系管理员！错误信息：" + json.result.Msg);
                    }

                },
                error: OnError
            })
        }
        function DateTimeConvert2(date2, format, iseval) {
            date2 = date2.replace(/-/g, "/");
            var date = new Date(date2);
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
            } else if (format == "HH:mm:ss" || format == "Hms") {
                dateTime = twoHour + ":" + twoMinute + ":" + twoSecond;
            }
            else {
                dateTime = year + "-" + twoMonth + "-" + twoDay
            }
            return dateTime;
        }
    </script>
</body>
</html>
