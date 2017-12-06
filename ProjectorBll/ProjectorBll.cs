using ProjectorModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectorBll
{
    public partial class ProjectorBll
    {
        BLLCommon common = new BLLCommon();
        public DataTable getUserInfo(string Name, string Phone)
        {
            return new ProjectorDLL.ProjectorDll().getUserInfo(Name, Phone);

        }

        public DataTable GBProjector()
        {
            return new ProjectorDLL.ProjectorDll().GBProjector();

        }

        public DataTable InProjector(string ComputerID, string UserName)
        {
            return new ProjectorDLL.ProjectorDll().InProjector(ComputerID, UserName);

        }

        public DataTable InProjectors(string ComputerID, string UserName)
        {
            return new ProjectorDLL.ProjectorDll().InProjectors(ComputerID, UserName);

        }

        public DataTable GetProjector()
        {
            return new ProjectorDLL.ProjectorDll().GetProjector();
        }

        public JsonModel Year()
        {
            DataTable modList = new ProjectorDLL.ProjectorDll().Year();
            return GetDataTableToJsonModel(modList);
        }

        public JsonModel Month(string Year)
        {
            DataTable modList = new ProjectorDLL.ProjectorDll().Month(Year);
            return GetDataTableToJsonModel(modList);
        }

        public JsonModel Projector()
        {
            DataTable modList = new ProjectorDLL.ProjectorDll().Projector();
            return GetDataTableToJsonModel(modList);
        }

        public DataTable InUserLog(string name, string phone)
        {
            return new ProjectorDLL.ProjectorDll().InUserLog(name, phone);
        }

        public DataTable CountProjector()
        {
            return new ProjectorDLL.ProjectorDll().CountProjector();
        }

        /// <summary>
        /// 获取当年当月有投屏记录的日期
        /// </summary>
        /// <returns></returns>
        public JsonModel Date()
        {
            DataTable modList = new ProjectorDLL.ProjectorDll().Date();
            return GetDataTableToJsonModel(modList);
        }

        public JsonModel GetDataTableToJsonModel(DataTable modList)
        {
            JsonModel jsonModel = null;
            PagedDataModel<Dictionary<string, object>> pagedDataModel = null;
            int RowCount = 0;
            if (modList == null)
            {
                jsonModel = new JsonModel()
                {
                    Status = "null",
                    Msg = "无数据",
                    errNum = 999
                };
                return jsonModel;
            }
            RowCount = modList.Rows.Count;
            if (RowCount <= 0)
            {
                jsonModel = new JsonModel()
                {
                    Status = "null",
                    Msg = "无数据",
                    errNum = 999
                };
                return jsonModel;
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            list = common.DataTableToList(modList);
            int PageCount = (int)Math.Ceiling(RowCount * 1.0 / 10000);
            //将数据封装到PagedDataModel分页数据实体中
            pagedDataModel = new PagedDataModel<Dictionary<string, object>>()
            {
                PageCount = PageCount,
                PagedData = list,
                PageIndex = 1,
                PageSize = 10000,
                RowCount = RowCount
            };
            //将分页数据实体封装到JSON标准实体中
            jsonModel = new JsonModel()
            {
                errNum = 0,
                Msg = "success",
                Data = pagedDataModel,
                Status = "ok"
            };
            return jsonModel;
        }

    }
}
