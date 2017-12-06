<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Screen_Manage.aspx.cs" Inherits="DingDing_Projector.Screen_Manage" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <title>投屏管理</title>
    <link rel="stylesheet" href="css/reset.css" />
    <link rel="stylesheet" href="css/style.css" />
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="js/fastclick.js"></script>
    <script>
        $(function () {
            FastClick.attach(document.body);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hid_Name" runat="server" />
        <asp:HiddenField ID="hid_Phone" runat="server" />
        <asp:HiddenField ID="hid_type" runat="server" />
        <header id="header">
            <span class="title">
                当前正在投屏的是: 
                <label id="lab_Name" style="color: #fff;">无投屏信息</label></span>
            <a class="record" href="HistoryRecord.aspx"></a>
        </header>
        <div class="marked_screen">
            <ul class="clearfix">
                <li>
                    <i class="selected"></i>
                    已选
                </li>
                <li>
                    <i class="screened"></i>
                    投屏
                </li>
                <li>
                    <i class="option"></i>
                    可选
                </li>
            </ul>
        </div>
        <div id="main1">
            <div class="screen_control">
                <div class="screen_wrap">
                    <div class="screen_left"></div>
                    <div class="screen_center">
                        <h1 class="screen_name">711大屏幕</h1>
                        <div class="screen_layout">
                            <em class="screen_layout_center">屏幕中央</em>
                            <div class="screen_equipwrap pr">
                                <ul class="screen_equip clearfix">
                                    <li>
                                        <div class="computer" id="8">8号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="7">7号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="1">1号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="6">6号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="2">2号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="5">5号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="3">3号</div>
                                    </li>
                                    <li>
                                        <div class="computer" id="4">4号</div>
                                    </li>
                                </ul>
                                <ol>
                                    <li>
                                        <div class="Externaldevice" id="9">外接</div>
                                    </li>
                                </ol>
                            </div>
                            <div class="screen_bottom"></div>
                        </div>
                    </div>
                    <div class="screen_right"></div>
                </div>
            </div>
       </div>
        <footer class="footer">
            <div class="fl notice">
                请选择要投屏的电脑
            </div>
            <a href="javascript:;" class="btn btnblue" id="SureScreen">确认投屏</a>
            <a href="javascript:;" class="btn btnred" id="DKProjector">断开连接</a>
        </footer>
        <div class="loadding_wrap none">
            <div class="loadding">
                <div class="spinner">
                    <div class="bar1"></div>
                    <div class="bar2"></div>
                    <div class="bar3"></div>
                    <div class="bar4"></div>
                    <div class="bar5"></div>
                    <div class="bar6"></div>
                    <div class="bar7"></div>
	                <div class="bar8"></div>
	                <div class="bar9"></div>
	                <div class="bar10"></div>
	                <div class="bar11"></div>
	                <div class="bar12"></div>
	            </div>
            </div>
        </div>
        <script>
            var reg1 = /AppleWebKit.*Mobile/i, reg2 = /MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/;
            var id = "";
            $(function () {
                //选择电脑提示
                var name = $("#hid_Name").val();
                var Phone = $("#hid_Phone").val();
                sessionStorage.setItem("name", name);
                sessionStorage.setItem("phone", Phone);

                $('#time').html(currentTime);
                if (reg1.test(navigator.userAgent) || reg2.test(navigator.userAgent)) {
                    $('.screen_equipwrap li>div').on('touchstart', function () {
                        $('.screen_equipwrap li').removeClass('selected');
                        $(this).parent().addClass('selected');
                        id = $(this).attr('id');
                        if (id == 9) $('.notice').text('当前选中为外接电脑');
                        else $('.notice').text('当前选中为' + id + '号电脑');
                    })
                } else {
                    $('.screen_equipwrap li>div').on('click', function () {
                        $('.screen_equipwrap li').removeClass('selected');
                        $(this).parent().addClass('selected');
                        if (id == 9) $('.notice').text('当前选中为外接电脑');
                        else $('.notice').text('当前选中为' + id + '号电脑');
                    })
                }
                
                document.querySelector("#SureScreen").addEventListener("click", function () {
                    SetProjectors();
                }, false);
                //
                document.querySelector("#DKProjector").addEventListener("click", function () {
                    GBProjector();
                }, false);
                

                // $("#lab_Name").html(sessionStorage.getItem("name"));

                getUserInfo();
                //  dkORtp();
                setInterval(function () { $('#time').html(currentTime) }, 1000);

            })

            

            function currentTime() {
                var d = new Date(), str = '';
                str += d.getFullYear() + '年';
                str += d.getMonth() + 1 + '月';
                str += d.getDate() + '日';
                str += d.getHours() + '时';
                str += d.getMinutes() + '分';
                str += d.getSeconds() + '秒';
                return str;
            }
            //
            function MesTips(MesContent) {
                $('body').append('<div class="screen_success"><div class="wenzi"></div></div>');
                $('.screen_success .wenzi').html(MesContent);

                setTimeout(function () {
                    $('.screen_success').remove();
                }, 2000);
            }

            ///按钮显示隐藏
            function dkORtp() {
                if (sessionStorage.getItem("RoleID") == "1") {
                    $("#DKProjector").css('display', 'block');
                    $("#SureScreen").css('display', 'block');
                } else {

                    if (sessionStorage.getItem("OccupyID") == "0") {
                        $("#DKProjector").hide();
                        $("#SureScreen").css('display', 'block');
                    } else {
                        if (sessionStorage.getItem("OccupyUserName") == sessionStorage.getItem("name")) {
                            $("#DKProjector").css('display', 'block');
                            $("#SureScreen").css('display', 'block');
                        } else {
                            $("#DKProjector").hide();
                            $("#SureScreen").css('display', 'block');
                        }
                    }
                }

            }

            //关闭正在投屏的电脑
            function GBProjector() {
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "action": "GetProjector" },
                    success: function (jsons) {
                        if (jsons.msg == "ok") {
                            $.ajax({
                                type: "post",
                                url: "ProjectorHandler.ashx",
                                dataType: "json",
                                data: { "OccupyUserName": jsons.result2, "UserName": sessionStorage.getItem("name"), "UserRoleID": sessionStorage.getItem("RoleID"), "action": "GBProjector" },
                                success: function (json) {
                                    if (json.result == "OK") {
                                        //sessionStorage.setItem("OccupyID", 0)
                                        //sessionStorage.setItem("OccupyUserName", "");
                                        sessionStorage.removeItem("OccupyID");
                                        sessionStorage.removeItem("OccupyUserName");
                                        id = "";
                                        MesTips('投屏设备已断开！');
                                        GetProjector();
                                    } else if (json.result == "NQH") {
                                        MesTips('您无对应权限断开投屏！');
                                        GetProjector();
                                    } else {
                                        MesTips('系统异常，请与管理员联系。');
                                    }
                                },
                                error: OnError
                            });
                        } else if (jsons.msg == "null") {
                            MesTips('系统没有投屏信息！');
                        } else {
                            MesTips('系统异常，请与管理员联系。错误信息：' + jsons.msg);
                        }
                    },
                    error: OnError
                });
            }

            ///获取账号信息
            function getUserInfo() {

                var Name = sessionStorage.getItem("name");
                var Phone = sessionStorage.getItem("phone");
                if (Name == "" || Phone == "") {
                    MesTips("无用户信息！");
                }
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "Name": Name, "Phone": Phone, "action": "getUserInfo" },
                    success: function (json) {

                        if (json.result == "OK") {
                            sessionStorage.setItem("RoleID", json.result2); //存储登陆人的ROLEID
                            GetProjector();
                        } else {
                            InUserLog(Name, Phone);
                        }
                    },
                    error: OnError
                });
            }

            //新用户自动注册账号
            function InUserLog(Name, Phone) {
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "Name": Name, "Phone": Phone,  "action": "InUserLog" },
                    success: function (json) {
                        if (json.result == "OK") {

                            sessionStorage.setItem("RoleID", 2); //存储登陆人的ROLEID
                            GetProjector();

                        } else {
                            MesTips("新用户您好，您的账号自动注册失败，请联系管理员！");
                        }
                    },
                    error: OnError
                });
            }

            //加载查询是否有正在投屏的电脑，返回电脑ID，无则返回0
            function GetProjector() {
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "action": "GetProjector" },
                    success: function (json) {
                        if (json.msg == "null") {
                            sessionStorage.setItem("OccupyID", json.result);
                            sessionStorage.setItem("OccupyUserName", "");
                            $("#lab_Name").html("无投屏信息");
                        } else if (json.msg == "ok") {
                            sessionStorage.setItem("OccupyID", json.result);
                            sessionStorage.setItem("OccupyUserName", json.result2);
                            $("#lab_Name").html(json.result2);

                        } else {
                            MesTips("系统异常，请与管理员联系。错误信息：" + json.msg);
                        }
                        dkORtp();
                        if (json.result=="") {
                            $(".notice").html("请选择要投屏的电脑");
                            $('.screen_equipwrap li').removeClass('screened').removeClass('selected');
                        } else {
                            $('.screen_equipwrap li').each(function () {
                                if ($(this).children('div').attr('id') == json.result) {
                                    $(this).addClass('screened');
                                    if (json.result == 9)
                                        $(".notice").html("正在投屏的是外接电脑");
                                    else
                                        $(".notice").html("正在投屏的是" + json.result + "号电脑");
                                } else {
                                    $(this).removeClass('screened');
                                    $(this).removeClass('selected');
                                }
                            })
                        }
                        
                    },
                    error: OnError
                });

            }

            function SetProjectors() {

                if (id == "") {
                    MesTips('请选择要投屏的电脑！');
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "action": "GetProjector","oid":id },
                    success: function (jsons) {
                        if (jsons.msg == "ok") {
                            setProjectorsMsg(jsons.result2, jsons.result);
                        } else if (jsons.msg == "error") {
                            MesTips(jsons.result);
                            GetProjector();
                        }
                        else if (jsons.msg == "null") {
                            setProjectorsMsg("", 0);
                        } else {
                            MesTips("系统异常，请与管理员联系。错误信息：" + jsons.msg);
                        }

                    },
                    error: OnError
                });
            }

            function setProjectorsMsg(ouname,oid)
            {
                $(".loadding_wrap").show();
                $.ajax({
                    type: "post",
                    url: "ProjectorHandler.ashx",
                    dataType: "json",
                    data: { "id": id, "OccupyUserName": ouname, "OccupyID": oid, "UserName": sessionStorage.getItem("name"), "UserRoleID": sessionStorage.getItem("RoleID"), "action": "SetProjector" },
                    success: function (json) {
                        $(".loadding_wrap").hide();
                        var items = json.result;
                        if (items == "OK") {
                            MesTips("投屏成功。");
                            sessionStorage.setItem("OccupyID", id)  //存储投屏中的ID
                            sessionStorage.setItem("OccupyUserName", sessionStorage.getItem("name")); //存储投屏中的UserName

                        } else if (items == "NOQX") {
                            MesTips("管理员正在投屏，您无权限切换投屏。");
                        } else if (items == "NO") {
                            MesTips("投屏服务器电源未开启！");
                        } else if (items == "Error"){
                            MesTips("切换投屏失败，请联系管理员！");
                        } else if (items=="NOQXU") {
                            MesTips("当前用户" + json.name + "正在投屏。");
                        } else if (items == "WAIT") {
                            MesTips("5秒后设备将开启，请稍候。");
                        }
                        else
                        {
                            MesTips(items);
                        }
                        id = "";
                        GetProjector();

                    },
                    error: function (textStatus) {
                        $(".loadding_wrap").hide();
                        MesTips('投屏失败，请联系管理员！');
                    }

                });
            }



            //错误处理
            function OnError(XMLHttpRequest, textStatus, errorThrown) {

            }

        </script>
    </form>
</body>
</html>
