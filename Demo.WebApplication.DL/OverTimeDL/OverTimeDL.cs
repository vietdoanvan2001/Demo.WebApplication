using Dapper;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.DepartmentDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.DL.OverTimeDL
{
    public class OverTimeDL : BaseDL<OverTime>, IOverTimeDL
    {
        /// <summary>
        /// Lấy thông tin làm thêm theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id làm thêm muốn lấy thông tin</param>
        /// <returns>thông tin làm thêm</returns>
        public ServiceResult GetOverTimeById(Guid recordId)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_Overtime_GetById";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add($"v_OverTimeId", recordId);

            //khởi tạo kết nối tới DB

            var dbConnection = GetOpenConnection();


            //thực hiện câu lệnh sql
            try
            {
                OverTime record = dbConnection.QueryFirstOrDefault<OverTime>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (record != null)
                {
                    {
                        //gán tên trạng thái theo trạng thái
                        if (record.Status == Status.All)
                        {
                            record.StatusName = Resource.Status_All;
                        }
                        else if (record.Status == Status.Wait)
                        {
                            record.StatusName = Resource.Status_Wait;
                        }
                        else if (record.Status == Status.Approved)
                        {
                            record.StatusName = Resource.Status_Approved;
                        }
                        else
                        {
                            record.StatusName = Resource.Status_Denied;
                        }

                        //gán tên thời điểm làm thêm theo thời điểm làm thêm
                        if (record.OverTimeInWorkingShiftName == OverTimeInWorkingShift.BeforeShift)
                        {
                            record.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_BeforeShift;
                        }
                        else if (record.OverTimeInWorkingShiftName == OverTimeInWorkingShift.AfterShift)
                        {
                            record.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_AfterShift;
                        }
                        else if (record.OverTimeInWorkingShiftName == OverTimeInWorkingShift.MiddleShift)
                        {
                            record.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_MiddleShift;
                        }
                        else
                        {
                            record.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_DayOff;
                        }

                        //gán tên Ca làm thêm theo Ca làm thêm
                        if (record.WorkingShiftName == WorkingShift.FirstCase)
                        {
                            record.WorkingShift = Resource.WorkingShift_FirstCase;
                        }
                        else if (record.WorkingShiftName == WorkingShift.SecondCase)
                        {
                            record.WorkingShift = Resource.WorkingShift_SecondCase;
                        }
                        else if (record.WorkingShiftName == WorkingShift.NightCase)
                        {
                            record.WorkingShift = Resource.WorkingShift_NightCase;
                        }
                        else
                        {
                            record.WorkingShift = Resource.WorkingShift_InWork;
                        }
                    }
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

        /// <summary>
        /// Phân trang bảng làm thêm
        /// author: VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">mã phòng ban</param>
        /// <param name="status">trạng thái</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">bản ghi bắt đầu</param>
        /// <returns>mảng các bản ghi được lọc</returns>
        public Dictionary<string, object> GetPaging(string? keyword, String? MISACode, int? status, int pageSize = 10, int offSet = 0)
        {
            if (MISACode == null)
            {
                MISACode = Resource.DefaultMISACode;
            }

            //chuẩn bị tên stored
            String storedProcedureName = "Proc_OverTime_Filter";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_Where", keyword);
            paprameters.Add("v_Offset", offSet);
            paprameters.Add("v_Limit", pageSize);
            paprameters.Add("v_Status", status);
            paprameters.Add("v_Departmentid", MISACode);

            //kết nối tới database
            var dbConnection = GetOpenConnection();

            {
                //thực hiện câu lệnh sql
                var res = dbConnection.QueryMultiple(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                {
                    var overTimes = res.Read<OverTime>().ToList();
                    var amountData = res.Read<int>().First();

                    //đóng kết nối tới db
                    dbConnection.Close();

                    return new Dictionary<string, object>{
                    { "PageData", overTimes},
                    { "Total", amountData },
                    };
                }
            };

        }

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult MultipleDelete(string IDs)
        {
            var affectedRow = 0;

            //kết nối tới database
            using (var dbConnection = GetOpenConnection())
            {
                var tran = dbConnection.BeginTransaction();

                //chuẩn bị tên stored
                String storedProcedureName = "Proc_OverTime_MultipleDelete";

                //chuẩn bị tham số đầu vào
                var paprameters = new DynamicParameters();
                paprameters.Add("v_IDs", IDs);

                try
                {
                    //thực hiện câu lệnh sql
                    affectedRow = dbConnection.QueryFirstOrDefault<int>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure, transaction: tran);
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
        /// Chuyển trạng thái các bản ghi đã chọn
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult ChangeStatus(string IDs, int status)
        {
            var affectedRow = 0;

            //kết nối tới database
            using (var dbConnection = GetOpenConnection())
            {
                var tran = dbConnection.BeginTransaction();

                //chuẩn bị tên stored
                String storedProcedureName = "Proc_OverTime_ChangeStatus";

                //chuẩn bị tham số đầu vào
                var paprameters = new DynamicParameters();
                paprameters.Add("v_IDs", IDs);
                paprameters.Add("v_status", status);

                try
                {
                    //thực hiện câu lệnh sql
                    affectedRow = dbConnection.QueryFirstOrDefault<int>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure, transaction: tran);
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
        /// Xuất khẩu toàn bộ dữ liệu làm thêm
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="param">truy vấn lọc</param>
        /// <returns>Danh sách bản ghi thoả mãn</returns>
        public List<OverTime> ExcelExport(OverTimesExportDataParams param)
        {
            try
            {

                var FileData = GetPaging(param.keyword, param.MISACode, param.status, param.total, 0);
                List<OverTime> AllData = (List<OverTime>)FileData["PageData"];
                return AllData;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Xuất khẩu dữ liệu đơn làm thêm được chọn
        /// author: VietDV(5/5/2023)
        /// </summary>
        /// <param name="param">danh sách id các đơn được chọn</param>
        /// <returns>Danh sách bản ghi thoả mãn</returns>
        public List<OverTime> ExcelExportSelected(String IDs)
        {

            //chuẩn bị tên stored
            String storedProcedureName = "Proc_OverTime_SelectedByListIDs";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_IDs", IDs);

            //kết nối tới database
            var dbConnection = GetOpenConnection();
            try
            {
                //thực hiện câu lệnh sql
                var listOverTimes = dbConnection.Query<OverTime>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);
                
                return (List<OverTime>)listOverTimes;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public ServiceResult CheckOverTimeDuplicate(OverTime record)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_OverTime_CheckDuplicate";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add($"v_EmployeeId", record.EmployeeId);
            paprameters.Add($"v_FromDate", record.FromDate);
            paprameters.Add($"v_ToDate", record.ToDate);

            //khởi tạo kết nối tới DB

            var dbConnection = GetOpenConnection();


            //thực hiện câu lệnh sql
            try
            {
                OverTime affectedRow = dbConnection.QueryFirstOrDefault<OverTime>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (affectedRow == null)
                {
                    return new ServiceResult(true, Resource.ServiceResult_Success);
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
    }
}
