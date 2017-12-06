
using SMSUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectorDLL
{
    public partial class ProjectorDll
    {
        /// <summary>
        /// 钉钉登陆判断账号所属
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public DataTable getUserInfo(string Name, string Phone)
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" select * from UserInfo where Name='" + Name + "' and Phone='" + Phone + "' ", CommandType.Text, null);
            return dt;
        }

        /// <summary>
        /// 断开投屏
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public DataTable GBProjector()
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_GBProjector", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 断开投屏并添加投屏信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public DataTable InProjector(string ComputerID, string UserName)
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_InProjector '" + ComputerID + "','" + UserName + "'", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 添加投屏信息
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable InProjectors(string ComputerID, string UserName)
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_InProjectors '" + ComputerID + "','" + UserName + "'", CommandType.StoredProcedure, null);
            return dt;
        }


        /// <summary>
        /// 加载查询是否有正在投屏的电脑，返回电脑ID，无则返回0
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable GetProjector()
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_GetProjector", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 分组查询历史记录年
        /// </summary>
        /// <returns></returns>
        public DataTable Year()
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_Year", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 分组查询历史记录月
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public DataTable Month(string Year)
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_Month '" + Year + "'", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 根据年月查询历史记录
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataTable Projector()
        {
            DataTable dt = SQLHelp.ExecuteDataTable(" exec Proc_Projector ", CommandType.StoredProcedure, null);
            return dt;
        }


        /// <summary>
        /// 新用户登陆时候自动注册账号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public DataTable InUserLog(string name, string phone)
        {
            DataTable dt = SQLHelp.ExecuteDataTable("exec PROC_InUserLog '" + name + "','" + phone + "'", CommandType.StoredProcedure, null);
            return dt;
        }


        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public DataTable CountProjector()
        {
            DataTable dt = SQLHelp.ExecuteDataTable("exec PROC_CountProjector", CommandType.StoredProcedure, null);
            return dt;
        }

        /// <summary>
        /// 获取当年当月有投屏记录的日期
        /// </summary>
        /// <returns></returns>
        public DataTable Date()
        {
            DataTable dt = SQLHelp.ExecuteDataTable("exec Proc_Date", CommandType.StoredProcedure, null);
            return dt;
        }

    }
}
