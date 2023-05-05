using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.DepartmentBL;
using Demo.WebApplication.BL.OverTimeDetailBL;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.DepartmentDL;
using Demo.WebApplication.DL.OverTimeDL;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.OverTimeBL
{
    public class OverTimeBL : BaseBL<OverTime>, IOverTimeBL
    {
        #region Field

        public IOverTimeDL _overTimeDL;
        public IOverTimeDetailBL _overTimeDetailBL;


        #endregion

        #region Constructor

        public OverTimeBL(IOverTimeDL overTimeDL, IOverTimeDetailBL overTimeDetailBL) : base(overTimeDL)
        {
            _overTimeDL = overTimeDL;
            _overTimeDetailBL = overTimeDetailBL;
        }

        #endregion

        #region Method
        /// <summary>
        /// hàm thêm thông tin vào bảng detail
        /// </summary>
        /// <param name="record"></param>
        /// <param name="recordId"></param>
        public override void InsertDetailData(OverTime record, Guid id)
        {
            var employees = record.OvertimeEmployee;
            _overTimeDetailBL.DeleteRecordByOverTimeId(id);
            foreach (OverTimeDetail employee in employees)
            {
                employee.OverTimeId = id;
                _overTimeDetailBL.InsertRecord(employee);
            }
        }

        /// <summary>
        /// Lấy thông tin làm thêm theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id làm thêm muốn lấy thông tin</param>
        /// <returns>thông tin làm thêm</returns>
        public ServiceResult GetOverTimeById(Guid recordId)
        {
            var record = _overTimeDL.GetOverTimeById(recordId);
            if(record.IsSuccess == true)
            {
                var overtime = _overTimeDetailBL.GetAllRecordById((OverTime)record.Data, (Guid)recordId);
                return new ServiceResult(true, overtime);
            }
            else
            {
                return record;
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
        public OverTimeFilterResult GetPaging(string? keyword, String? MISACode, int? status, int pageSize = 10, int offSet = 0)
        {
            var result = _overTimeDL.GetPaging(keyword, MISACode, status, pageSize, offSet);

            var totalRecord = result["Total"];
            var resultArray = (List<OverTime>)result["PageData"];

            
            foreach (OverTime record in resultArray)
            {
                //gán tên trạng thái theo trạng thái
                if(record.Status == Status.All)
                {
                    record.StatusName = Resource.Status_All;
                }
                else if(record.Status == Status.Wait)
                {
                    record.StatusName = Resource.Status_Wait;
                }
                else if (record.Status == Status.Approved)
                {
                    record.StatusName = Resource.Status_Approved;
                }
                else {
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

            //index bản ghi đầu của trang luôn >=1
            var begin = (offSet + 1) > 0 ? (offSet + 1) : 1;

            //index bản ghi cuối của trang luôn <= tổng số bản ghi
            var end = (begin + pageSize) <= Convert.ToInt32(totalRecord) ? (begin + pageSize - 1) : Convert.ToInt32(totalRecord);

            return new OverTimeFilterResult(resultArray, Convert.ToInt32(totalRecord), begin, end);
        }

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult MultipleDelete(string IDs)
        {
            return _overTimeDL.MultipleDelete(IDs);
        }

        /// <summary>
        /// Chuyển trạng thái các bản ghi đã chọn
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult ChangeStatus(string IDs, int status)
        {
            return _overTimeDL.ChangeStatus(IDs, status);
        }

        /// <summary>
        /// Xuất khẩu toàn bộ dữ liệu làm thêm
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="param">truy vấn lọc</param>
        /// <returns>Danh sách bản ghi thoả mãn</returns>
        public MemoryStream ExcelExport(OverTimesExportDataParams param)
        {
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _overTimeDL.ExcelExport(param);
            if (data != null)
            {
                List<OverTime> missionallowances = new List<OverTime>();

                missionallowances = (List<OverTime>)data;

                return OverTimeExport(missionallowances);
            }
            return new MemoryStream();
        }

        /// <summary>
        /// Xuất khẩu các dữ liệu làm thêm được chọn
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="param">Danh sách các bản ghi</param>
        /// <returns>File excel</returns>
        public MemoryStream ExcelExportSelected(List<OverTime> listOverTimes)
        {
           return OverTimeExport(listOverTimes);
        }

        public MemoryStream OverTimeExport(List<OverTime> missionallowances)
        {
            {
                //gán tên thời điểm làm thêm, tên ca áp dụng, tên trạng thái
                    foreach (OverTime item in missionallowances)
                {
                    //Gán tên thời điểm làm thêm
                    if(item.OverTimeInWorkingShiftName == OverTimeInWorkingShift.BeforeShift)
                    {
                        item.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_BeforeShift;
                    }
                    else if(item.OverTimeInWorkingShiftName == OverTimeInWorkingShift.AfterShift)
                    {
                        item.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_AfterShift;
                    }
                    else if(item.OverTimeInWorkingShiftName == OverTimeInWorkingShift.MiddleShift)
                    {
                        item.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_MiddleShift;
                    }
                    else
                    {
                        item.OverTimeInWorkingShift = Resource.OverTimeInWorkingShift_DayOff;
                    }

                    //Gán tên ca áp dụng
                    if(item.WorkingShiftName == WorkingShift.FirstCase)
                    {
                        item.WorkingShift = Resource.WorkingShift_FirstCase;
                    }
                    else if(item.WorkingShiftName == WorkingShift.SecondCase)
                    {
                        item.WorkingShift = Resource.WorkingShift_SecondCase;
                    }
                    else if(item.WorkingShiftName == WorkingShift.NightCase)
                    {
                        item.WorkingShift = Resource.WorkingShift_NightCase;
                    }
                    else
                    {
                        item.WorkingShift = Resource.WorkingShift_InWork;
                    }
                    //Gán tên trạng thái
                    if(item.Status == Status.All)
                    {
                        item.StatusName = Resource.Status_All;
                    }
                    else if(item.Status == Status.Approved)
                    {
                        item.StatusName = Resource.Status_Approved;
                    }
                    else if(item.Status == Status.Wait) 
                    {
                        item.StatusName = Resource.Status_Wait;
                    }
                    else
                    {
                        item.StatusName = Resource.Status_Denied;
                    }
                }

                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_don_cong_tac");

                    worksheet.Row(2).Height = 20;
                    worksheet.Row(3).Height = 20;

                    worksheet.Cells["A1"].Value = "Danh sách đơn đăng ký làm thêm";

                    // Hợp cột A1 -> J1 của dòng 1 trong sheet Danh_sach_don_cong_tac
                    using (var r = worksheet.Cells["A1:Q1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    using (var r = worksheet.Cells["A2:Q2"])
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells["A3:Q3"])
                    {
                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 12;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Định dạng border
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    worksheet.Cells["A3"].Value = "STT";
                    worksheet.Cells["B3"].Value = "Mã nhân viên";
                    worksheet.Cells["C3"].Value = "Người nộp đơn";
                    worksheet.Cells["D3"].Value = "Vị trí công việc";
                    worksheet.Cells["E3"].Value = "Đơn vị công tác";
                    worksheet.Cells["F3"].Value = "Ngày nộp đơn";
                    worksheet.Cells["G3"].Value = "Làm thêm từ";
                    worksheet.Cells["H3"].Value = "Làm thêm đến";
                    worksheet.Cells["I3"].Value = "Nghỉ giữa ca từ";
                    worksheet.Cells["J3"].Value = "Nghỉ giữa ca đến";
                    worksheet.Cells["K3"].Value = "Thời điểm làm thêm";
                    worksheet.Cells["L3"].Value = "Ca áp dụng";
                    worksheet.Cells["M3"].Value = "Lý do làm thêm";
                    worksheet.Cells["N3"].Value = "Người duyệt";
                    worksheet.Cells["O3"].Value = "Người liên quan";
                    worksheet.Cells["P3"].Value = "Ghi chú";
                    worksheet.Cells["Q3"].Value = "Trạng thái";

                    worksheet.Column(1).Width = 6;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 30;
                    worksheet.Column(4).Width = 20;
                    worksheet.Column(5).Width = 50;
                    worksheet.Column(6).Width = 25;
                    worksheet.Column(7).Width = 25;
                    worksheet.Column(8).Width = 25;
                    worksheet.Column(9).Width = 25;
                    worksheet.Column(10).Width = 25;
                    worksheet.Column(11).Width = 25;
                    worksheet.Column(12).Width = 25;
                    worksheet.Column(13).Width = 30;
                    worksheet.Column(14).Width = 25;
                    worksheet.Column(15).Width = 60;
                    worksheet.Column(16).Width = 25;
                    worksheet.Column(17).Width = 25;

                    int row = 4;
                    int STT = 1;
                    int start = 4;
                    int end = 4;
                    foreach (var entity in missionallowances)
                    {
                        worksheet.Cells[row, 1].Value = STT++;
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 2].Value = entity.EmployeeCode;
                        worksheet.Cells[row, 3].Value = entity.FullName;
                        worksheet.Cells[row, 4].Value = entity.PositionName;
                        worksheet.Cells[row, 5].Value = entity.DepartmentName;
                        worksheet.Cells[row, 6].Value = entity.ApplyDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 7].Value = entity.FromDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 8].Value = entity.ToDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 9].Value = entity.BreakTimeFrom?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 10].Value = entity.BreakTimeTo?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 11].Value = entity.OverTimeInWorkingShift;
                        worksheet.Cells[row, 12].Value = entity.WorkingShift;
                        worksheet.Cells[row, 13].Value = entity.Reason;
                        worksheet.Cells[row, 14].Value = entity.ApprovalName;
                        worksheet.Cells[row, 15].Value = entity.RelationShipNames;
                        worksheet.Cells[row, 16].Value = entity.Description;
                        worksheet.Cells[row, 17].Value = entity.StatusName;

                        // Tạo border 1 trường dữ liệu
                        var recordRow = worksheet.Cells["A" + start++ + ":Q" + end++];

                        recordRow.Style.Font.Size = 12;
                        recordRow.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                    }

                    //worksheet.Cells.AutoFitColumns();
                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                }
                stream.Position = 0;

                return stream;
            }
        }
        #endregion
    }
}
