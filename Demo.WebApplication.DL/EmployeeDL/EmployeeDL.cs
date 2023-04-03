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
        public serviceResult GetNewEmployeeCode()
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
                    return new serviceResult(true, res);
                }
                else 
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }

        }

        /// <summary>
        /// Lấy thông tin phân trang nhân viên
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="keyword">từ khoá tìm kiếm(tìm kiếm theo tên nhân viên hoặc mã nhân viên)</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="offSet">thứ tự bản ghi bắt đầu của trang</param>
        /// <returns>danh sách các nhân viên thoả mãn</returns>
        /// <returns>tổng số nhân viên trong db</returns>
        public Dictionary<string, object> GetPaging(string? keyword, int pageSize = 10, int offSet = 0) 
        {
            //chuẩn bị tên stored
            String storedProcedureName = "Proc_Employee_Filter";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_Where", keyword);
            paprameters.Add("v_Offset", offSet);
            paprameters.Add("v_Limit", pageSize);

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
        public serviceResult MultipleDelete(string IDs)
        {
            //chuẩn bị tên stored
            String storedProcedureName = "Proc_Employee_MultipleDelete";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_IDs", IDs);

            //kết nối tới database
            var dbConnection = GetOpenConnection();

            try
            {
                //thực hiện câu lệnh sql
                var affectedRow = dbConnection.QueryFirstOrDefault<int>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                //đóng kết nối tới database
                dbConnection.Close();

                if (affectedRow > 0)
                {
                    return new serviceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }

        }

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        public List<Employee> ExcelExport(exportDataParams param)
        {
            try
            {

                var FileData = GetPaging(param.keyword, param.total, 0);
                List<Employee> AllData = (List<Employee>)FileData["PageData"];
                return AllData;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion
    }
}
