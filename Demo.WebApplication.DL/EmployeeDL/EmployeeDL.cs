using Dapper;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using MySqlConnector;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
//using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Demo.WebApplication.DL.EmployeeDL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        #region Method

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>mã nhân viên lớn nhất trong db</returns>
        public ServiceResult GetNewEmployeeCode()
        {
            //chuẩn bị tên stored
            String storedProcedureName = "Proc_Employee_NewEmployeeCode";

            //kết nối tới database
            var dbConnection = GetOpenConnection();

            try
            {
                //thực hiện câu lệnh sql
                var res = dbConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                //đóng kết nối tới db
                dbConnection.Close();

                if (res != null)
                {
                    return new ServiceResult(true, res);
                }
                else 
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new ServiceResult(false, Resource.ServiceResult_Exception);
            }

        }

        /// <summary>
        /// Phân trang nhân viên
        /// author:VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">Mã phòng ban</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">vị trí bắt đầu</param>
        /// <returns>mảng các bản ghi đã lọc</returns>
        public Dictionary<string, object> GetPaging(string? keyword, string? MISACode, int pageSize = 10, int offSet = 0) 
        {
            if(MISACode == null)
            {
                MISACode = Resource.DefaultMISACode;
            }

            //chuẩn bị tên stored
            String storedProcedureName = "Proc_Employee_Filter";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_Where", keyword);
            paprameters.Add("v_Offset", offSet);
            paprameters.Add("v_Limit", pageSize);
            paprameters.Add("v_MISACode", MISACode);

            //kết nối tới database
            var dbConnection = GetOpenConnection();

            {
                //thực hiện câu lệnh sql
                var res = dbConnection.QueryMultiple(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                {
                    var customer = res.Read<Employee>().ToList();
                    var amountData = res.Read<int>().First();

                    //đóng kết nối tới db
                    dbConnection.Close();
                    
                    return new Dictionary<string, object>{
                    { "PageData", customer},
                    { "Total", amountData },
                    };
                }
            };

        }

        /// <summary>
        /// Xoá nhiều nhân viên
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách các id nhân viên muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult MultipleDelete(string IDs)
        {
            var affectedRow = 0;

            //kết nối tới database
            using (var dbConnection  = GetOpenConnection())
            {
                var tran = dbConnection.BeginTransaction();

                //chuẩn bị tên stored
                String storedProcedureName = "Proc_Employee_MultipleDelete";

                //chuẩn bị tham số đầu vào
                var paprameters = new DynamicParameters();
                paprameters.Add("v_IDs", IDs);

                try
                {
                    //thực hiện câu lệnh sql
                    affectedRow = dbConnection.QueryFirstOrDefault<int>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure, transaction:tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
                finally 
                { 
                    dbConnection.Close();
                }

                if (affectedRow > 0)
                {
                    return new ServiceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }

            }

        }

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        //public List<Employee> ExcelExport(ExportDataParams param)
        //{
        //    try
        //    {

        //        var FileData = GetPaging(param.keyword, param.total, 0);
        //        List<Employee> AllData = (List<Employee>)FileData["PageData"];
        //        return AllData;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        /// <summary>
        /// Lấy thông tin bản ghi theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn lấy thông tin</param>
        /// <returns>thông tin bản ghi</returns>
        public ServiceResult GetEmployeeById(Guid recordId)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_Employee_GetById";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add($"v_EmployeeId", recordId);

            //khởi tạo kết nối tới DB

            var dbConnection = GetOpenConnection();


            //thực hiện câu lệnh sql
            try
            {
                Employee record = dbConnection.QueryFirstOrDefault<Employee>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (record != null)
                {
                    return new ServiceResult(true, record);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new ServiceResult(false, Resource.ServiceResult_Exception);
            }
        }
        #endregion
    }
}
