using DingDing_Projector.Maxtrix_Control;
using ProjectorModel;
using SMSUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Script.Serialization;

namespace DingDing_Projector
{
    /// <summary>
    /// ProjectorHandler 的摘要说明
    /// </summary>
    public class ProjectorHandler : IHttpHandler
    {
        ProjectorBll.BLLCommon common = new ProjectorBll.BLLCommon();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"];
            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "SetProjector": SetProjector(context); break;
                    case "GetProjector": GetProjector(context); break;
                    case "getUserInfo": getUserInfo(context); break;
                    case "GBProjector": GBProjector(context); break;
                    case "Year": Year(context); break;
                    case "Month": Month(context); break;
                    case "Projector": Projector(context); break;
                    case "InUserLog": InUserLog(context); break;
                    case "CountProjector": CountProjector(context); break;
                    case "GetList": GetList(context); break;

                }
            }
        }


        /// <summary>
        /// 断开投屏
        /// </summary>
        /// <param name="context"></param>
        public void GBProjector(HttpContext context)
        {
            string UserName = context.Request["UserName"];
            string OccupyUserName = context.Request["OccupyUserName"];
            string UserRoleID = context.Request["UserRoleID"];
            if (UserRoleID == "1")
            {
                DataTable dt = new ProjectorBll.ProjectorBll().GBProjector();
                context.Response.Write("{\"result\":\"" + dt.Rows[0][0].ToString() + "\"}");
            }
            else
            {
                if (OccupyUserName == UserName)
                {
                    DataTable dt = new ProjectorBll.ProjectorBll().GBProjector();
                    context.Response.Write("{\"result\":\"" + dt.Rows[0][0].ToString() + "\"}");
                }
                else
                {
                    string str = "NQH";
                    context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                }
            }


        }


        /// <summary>
        /// 投屏
        /// </summary>
        /// <param name="context"></param>
        public void SetProjector(HttpContext context)
        {
            try
            {
                ProjectorBll.ProjectorBll projector = new ProjectorBll.ProjectorBll();
                DataTable dt = new DataTable();
                string UserName = context.Request["UserName"];
                string OccupyUserName = context.Request["OccupyUserName"];
                string UserRoleID = context.Request["UserRoleID"];
                int OccupyID = Convert.ToInt32(context.Request["OccupyID"]);
                int id = Convert.ToInt32(context.Request["id"]);
                if (UserRoleID == "1")
                {
                    string str = KQInterface(id);
                    if (str == "OK")
                    {
                        if (OccupyID == 0) //0表示没有投屏，可以直接添加投屏信息
                        {
                            dt = projector.InProjectors(id.ToString(), UserName); //直接投屏
                        }
                        else
                        {
                            dt = projector.InProjector(id.ToString(), UserName);  //先修改上一个投屏状态，在添加投屏
                        }
                        str = dt.Rows[0][0].ToString();

                        context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                    }
                    else if (str=="远程主机强迫关闭了一个现有的连接。")
                    {
                        context.Response.Write("{\"result\":\"WAIT\"}");
                    }
                    else
                    {
                        LogHelper.Info(str);
                        str = "NO";
                        context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                    }
                }
                else
                {
                    DataTable dtable = new ProjectorBll.ProjectorBll().GetProjector();
                    if (OccupyID == 0 && dtable.Rows[0][0].ToString() == "0")//0表示没有投屏，可以直接添加投屏信息
                    {
                        string zt = KQInterface(id);
                        if (zt == "OK")
                        {
                            #region
                            dt = projector.InProjectors(id.ToString(), UserName); //直接投屏
                            if (dt!=null && dt.Rows.Count>0)
                            {
                                if (dt.Rows[0][0].ToString()=="OK")
                                {
                                    context.Response.Write("{\"result\":\"OK\"}");
                                }
                                else
                                {
                                    context.Response.Write("{\"result\":\"Error\"}");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"result\":\"Error\"}");
                            }
                            #endregion
                        }
                        else if (zt == "远程主机强迫关闭了一个现有的连接。")
                        {
                            context.Response.Write("{\"result\":\"WAIT\"}");
                        }
                        else
                        {
                            LogHelper.Info("调用接口失败" + zt);
                            context.Response.Write("{\"result\":\"NO\"}");
                        }
                    }
                    else ///投屏正在使用中，非管理员，判断状态
                    {
                        if (OccupyUserName == UserName)//正在投屏=当前登录人
                        {
                            string zt = KQInterface(id);
                            #region
                            if (zt == "OK")
                            {
                                dt = projector.InProjector(id.ToString(), UserName);  //先修改上一个投屏状态，在添加投屏
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0][0].ToString() == "OK")
                                    {
                                        context.Response.Write("{\"result\":\"OK\"}");
                                    }
                                    else
                                    {
                                        context.Response.Write("{\"result\":\"Error\"}");
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"result\":\"Error\"}");
                                }
                            }
                            else if (zt == "远程主机强迫关闭了一个现有的连接。")
                            {
                                context.Response.Write("{\"result\":\"WAIT\"}");
                            }
                            else 
                            {
                                LogHelper.Info("调用接口失败" + zt);
                                context.Response.Write("{\"result\":\"NO\"}");
                            }
                            #endregion
                        }
                        else
                        {
                            context.Response.Write("{\"result\":\"NOQXU\",\"name\":\"" + OccupyUserName + "\"}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"系统异常，请与管理联系。错误代码：" + ex.Message + "\"}");
            }
        }

        public void SetProjector2(HttpContext context)
        {
            try
            {
                ProjectorBll.ProjectorBll projector = new ProjectorBll.ProjectorBll();
                DataTable dt = new DataTable();
                string UserName = context.Request["UserName"];
                string OccupyUserName = context.Request["OccupyUserName"];
                string UserRoleID = context.Request["UserRoleID"];
                int OccupyID = Convert.ToInt32(context.Request["OccupyID"]);
                int id = Convert.ToInt32(context.Request["id"]);
                if (UserRoleID == "1")
                {
                    string str = "";
                    if (OccupyID == 0) //0表示没有投屏，可以直接添加投屏信息
                    {
                        dt = projector.InProjectors(id.ToString(), UserName); //直接投屏
                    }
                    else
                    {
                        dt = projector.InProjector(id.ToString(), UserName);  //先修改上一个投屏状态，在添加投屏
                    }
                    str = dt.Rows[0][0].ToString();

                    context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                }
                else
                {
                    DataTable dtable = new ProjectorBll.ProjectorBll().GetProjector();
                    if (OccupyID == 0 && dtable.Rows[0][0].ToString() == "0")//0表示没有投屏，可以直接添加投屏信息
                    {
                        #region
                        dt = projector.InProjectors(id.ToString(), UserName); //直接投屏
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() == "OK")
                            {
                                context.Response.Write("{\"result\":\"OK\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"result\":\"Error\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"result\":\"Error\"}");
                        }
                        #endregion
                    }
                    else ///投屏正在使用中，非管理员，判断状态
                    {
                        if (OccupyUserName == UserName)//正在投屏=当前登录人
                        {
                            dt = projector.InProjector(id.ToString(), UserName);  //先修改上一个投屏状态，在添加投屏
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0][0].ToString() == "OK")
                                {
                                    context.Response.Write("{\"result\":\"OK\"}");
                                }
                                else
                                {
                                    context.Response.Write("{\"result\":\"Error\"}");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"result\":\"Error\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"result\":\"NOQXU\",\"name\":\"" + OccupyUserName + "\"}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"系统异常，请与管理联系。错误代码：" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 加载查询是否有正在投屏的电脑，返回电脑ID，无则返回0
        /// </summary>
        /// <param name="context"></param>
        public void GetProjector(HttpContext context)
        {
            try
            {
                DataTable dt = new ProjectorBll.ProjectorBll().GetProjector();
                string oid=context.Request["oid"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string str = dt.Rows[0][0].ToString();
                    string UserName = dt.Rows[0][1].ToString();
                    if (!string.IsNullOrWhiteSpace(UserName))
                    {
                        if (!string.IsNullOrWhiteSpace(oid))
                        {
                            if (oid == str)
                            {
                                context.Response.Write("{\"result\":\"当前设备正在投屏！\",\"msg\":\"error\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"result\":\"" + str.ToString() + "\",\"result2\":\"" + UserName.ToString() + "\",\"msg\":\"ok\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"result\":\"" + str.ToString() + "\",\"result2\":\"" + UserName.ToString() + "\",\"msg\":\"ok\"}");
                        }
                        
                    }
                    else
                    {
                        context.Response.Write("{\"result\":\"\",\"result2\":\"\",\"msg\":\"null\"}");
                    }
                }
                else
                {
                    context.Response.Write("{\"result\":\"\",\"result2\":\"\",\"msg\":\"null\"}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"\",\"result2\":\"\",\"msg\":\"" + ex.Message + "\"}");
            }
        }


        /// <summary>
        /// 分组查询历史记录年
        /// </summary>
        /// <param name="context"></param>
        public void Year(HttpContext context)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                JsonModel jsonModel = new ProjectorBll.ProjectorBll().Year();
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
            catch (Exception ex)
            {

                JsonModel jsonModel = new JsonModel()
                {
                    Msg = ex.Message,
                };
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }

            //  context.Response.End();
        }

        /// <summary>
        ///  分组查询历史记录月
        /// </summary>
        /// <param name="context"></param>
        public void Month(HttpContext context)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                string Year = context.Request["Year"];
                JsonModel jsonModel = new ProjectorBll.ProjectorBll().Month(Year);
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
            catch (Exception ex)
            {
                JsonModel jsonModel = new JsonModel()
                {
                    Msg = ex.Message,
                };
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }

            // context.Response.End();
        }

        /// <summary>
        /// 根据年月查询历史记录
        /// </summary>
        /// <param name="context"></param>
        public void Projector(HttpContext context)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            try
            {
                JsonModel jsonModel = new ProjectorBll.ProjectorBll().Projector();
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                JsonModel jsonModel = new JsonModel()
               {
                   Msg = ex.Message,
                   errNum = -1,
                   Data = null,
                   Status = "error"
               };
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }

        }


        /// <summary>
        /// 获取当年当月有投屏记录的日期
        /// </summary>
        /// <param name="context"></param>
        public void GetList(HttpContext context)
        {

            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                JsonModel jsonModel = new ProjectorBll.ProjectorBll().Date();
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                JsonModel jsonModel = new JsonModel()
              {
                  Msg = ex.Message,
                  errNum = -1,
                  Data = null,
                  Status = "error"
              };
                context.Response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
        }


        /// <summary>
        /// 调用接口
        /// </summary>
        /// <param name="input_type"></param>
        /// <returns></returns>
        public string KQInterface(int input_type)
        {
            string str = "";
            Maxtrix_Change(input_type, new Action<string>((error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    str = "OK";
                }
                else
                {

                    str = error;
                }
            }));
            return str;
        }





        /// <summary>
        /// 钉钉登陆判断账号所属
        /// </summary>
        /// <param name="context"></param>
        public void getUserInfo(HttpContext context)
        {
            try
            {
                string Name = context.Request["Name"].ToString();
                string Phone = context.Request["Phone"].ToString();
                DataTable dt = new ProjectorBll.ProjectorBll().getUserInfo(Name, Phone);
                string str = "";
                if (dt == null)
                {
                    str = "NO";
                    context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        str = "OK";
                        context.Response.Write("{\"result\":\"" + str.ToString() + "\",\"result2\":\"" + dt.Rows[0]["RoleID"].ToString() + "\"}");
                    }
                    else
                    {
                        str = "NO";
                        context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"\"}");
            }

        }



        /// <summary>
        /// 新用户登陆时候自动注册账号
        /// </summary>
        /// <param name="context"></param>
        public void InUserLog(HttpContext context)
        {
            try
            {
                string Name = context.Request["Name"].ToString();
                string Phone = context.Request["Phone"].ToString();
                DataTable dt = new ProjectorBll.ProjectorBll().InUserLog(Name, Phone);
                string str = dt.Rows[0][0].ToString();
                context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"\"}");
            }

        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="context"></param>
        public void CountProjector(HttpContext context)
        {
            try
            {
                DataTable dt = new ProjectorBll.ProjectorBll().CountProjector();
                string str = dt.Rows[0][0].ToString();
                context.Response.Write("{\"result\":\"" + str.ToString() + "\"}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                context.Response.Write("{\"result\":\"\"}");
            }

        }



        /// <summary>
        /// 代理（矩阵服务）
        /// </summary>
        static Maxtrix_ControlSoap Client = new Maxtrix_ControlSoapClient();

        /// <summary>
        /// 矩阵调用
        /// </summary>
        /// <param name="intMaxtrixType"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public static void Maxtrix_Change(int intMaxtrixType, Action<string> callBack)
        {

            //new Thread(() =>
            // {
            try
            {
                //格式转换
                MaxtrixType maxtrixType = (MaxtrixType)(intMaxtrixType - 1);
                ReturnDataBase data = Client.Maxtrix_Manage(maxtrixType);

                if (data.ServerError != null)
                {
                    callBack(data.ServerError);
                }
                else
                {
                    callBack(null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                callBack(ex.Message);
            }
            //}) { IsBackground = true }.Start();//用完之后会自动释放
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}