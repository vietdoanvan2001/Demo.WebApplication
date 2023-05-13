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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Demo.WebApplication.Common.Attibutes.Attributes;

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
        public OverTimeBL(IOverTimeDL overTimeDL) : base(overTimeDL)
        {
            _overTimeDL = overTimeDL;
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
        public MemoryStream ExcelExport(ExportBody body)
        {
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _overTimeDL.ExcelExport(body.param);
            if (data != null)
            {
                List<OverTime> missionallowances = new List<OverTime>();

                missionallowances = (List<OverTime>)data;

                return OverTimeExport(missionallowances,body.header);
            }
            return new MemoryStream();
        }

        /// <summary>
        /// Xuất khẩu dữ liệu đơn làm thêm được chọn
        /// author: VietDV(5/5/2023)
        /// </summary>
        /// <param name="param">danh sách id các đơn được chọn</param>
        /// <returns>Danh sách bản ghi thoả mãn</returns>
        public MemoryStream ExcelExportSelected(String IDs, List<HeaderType> header)
        {
           var response = _overTimeDL.ExcelExportSelected(IDs);
            if (response != null)
            {
                return OverTimeExport(response, header);
            }
            return new MemoryStream();
        }

        /// <summary>
        /// Hàm xuất file excel
        /// </summary>
        /// <param name="missionallowances"></param>
        /// <returns></returns>
        public MemoryStream OverTimeExport(List<OverTime> missionallowances, List<HeaderType> header)
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

                var listCaption = new string[header.Count];
                var listDataField = new string[header.Count];
                var listHeader = new int[header.Count];
                var letters = new char[listHeader.Length];

                var numberI = 0;

                for(int i = 0; i< header.Count; i++)
                {
                    if (header[i].isDisplay)
                    {
                        numberI++;
                        listHeader[i] = numberI;
                        listCaption[i] = header[i].caption;
                        listDataField[i] = header[i].dataField;
                    }
                }

                for(int i=0; i<listHeader.Length; i++)
                {
                    if (listHeader[i] != 0)
                    {
                        letters[i] = (char)(listHeader[i] + 64);
                    }
                    else
                    {
                        letters[i] = letters[i];
                    }
                }

                var numberRow = string.Join(",", listHeader);
                var number = numberRow.Replace(",0", "");
                var listNumber = number.Split(',');
                Array.Resize(ref listNumber, listNumber.Length - 1);
                var listRow = string.Join(",", letters);
                var list = listRow.Replace("\0,", "");
                var listAlphabet = list.Split(',');
                Array.Resize(ref listAlphabet, listAlphabet.Length - 1);
                var captions = listCaption.Where(item => item != null).ToList();
                var dataField = listDataField.Where(item => item != null).ToList();
                var nameExcel = new String[listAlphabet.Length];
                
                for(int i=0; i<listAlphabet.Length; i++)
                {
                    nameExcel[i] = (string)(listAlphabet[i]+'3');
                }

                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_don_cong_tac");

                    worksheet.Row(2).Height = 20;
                    worksheet.Row(3).Height = 20;

                    worksheet.Cells["A1"].Value = "Danh sách đơn đăng ký làm thêm";

                    // Hợp cột A1 -> J1 của dòng 1 trong sheet Danh_sach_don_cong_tac
                    //using (var r = worksheet.Cells["A1:Q1"])
                    using (var r = worksheet.Cells[listAlphabet[0]+"1:"+listAlphabet[listAlphabet.Length-1]+"1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    //using (var r = worksheet.Cells["A2:Q2"])
                    using (var r = worksheet.Cells[listAlphabet[0] + "2:" + listAlphabet[listAlphabet.Length - 1] + "2"])
                    {
                        r.Merge = true;
                    }
                    //using (var r = worksheet.Cells["A3:Q3"])
                    using (var r = worksheet.Cells[listAlphabet[0] + "3:" + listAlphabet[listAlphabet.Length - 1] + "3"])
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
                    //worksheet.Cells["A3"].Value = "STT";
                    //worksheet.Cells["B3"].Value = "Mã nhân viên";
                    //worksheet.Cells["C3"].Value = "Người nộp đơn";
                    //worksheet.Cells["D3"].Value = "Vị trí công việc";
                    //worksheet.Cells["E3"].Value = "Đơn vị công tác";
                    //worksheet.Cells["F3"].Value = "Ngày nộp đơn";
                    //worksheet.Cells["G3"].Value = "Làm thêm từ";
                    //worksheet.Cells["H3"].Value = "Làm thêm đến";
                    //worksheet.Cells["I3"].Value = "Nghỉ giữa ca từ";
                    //worksheet.Cells["J3"].Value = "Nghỉ giữa ca đến";
                    //worksheet.Cells["K3"].Value = "Thời điểm làm thêm";
                    //worksheet.Cells["L3"].Value = "Ca áp dụng";
                    //worksheet.Cells["M3"].Value = "Lý do làm thêm";
                    //worksheet.Cells["N3"].Value = "Người duyệt";
                    //worksheet.Cells["O3"].Value = "Người liên quan";
                    //worksheet.Cells["P3"].Value = "Ghi chú";
                    //worksheet.Cells["Q3"].Value = "Trạng thái";
                    
                    for(int i=0; i< listAlphabet.Length; i++)
                    {
                        worksheet.Cells[nameExcel[i]].Value = captions[i];
                    }


                    //worksheet.Column(1).Width = 30;
                    //worksheet.Column(2).Width = 30;
                    //worksheet.Column(3).Width = 30;
                    //worksheet.Column(4).Width = 30;
                    //worksheet.Column(5).Width = 30;
                    //worksheet.Column(6).Width = 30;
                    //worksheet.Column(7).Width = 30;
                    //worksheet.Column(8).Width = 30;
                    //worksheet.Column(9).Width = 30;
                    //worksheet.Column(10).Width = 30;
                    //worksheet.Column(11).Width = 30;
                    //worksheet.Column(12).Width = 30;
                    //worksheet.Column(13).Width = 30;
                    //worksheet.Column(14).Width = 30;
                    //worksheet.Column(15).Width = 30;
                    //worksheet.Column(16).Width = 30;
                    //worksheet.Column(17).Width = 30;
                    //for(int i=0; i < listAlphabet.Length; i++)
                    //{
                    //    worksheet.Column(i + 1).Width = 45;
                    //}

                    int row = 4;
                    int STT = 1;
                    int start = 4;
                    int end = 4;
                    foreach (var entity in missionallowances)
                    {
                        //worksheet.Cells[row, 1].Value = STT++;
                        //worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 2].Value = entity.EmployeeCode;
                        //worksheet.Cells[row, 3].Value = entity.FullName;
                        //worksheet.Cells[row, 4].Value = entity.PositionName;
                        //worksheet.Cells[row, 5].Value = entity.DepartmentName;
                        //worksheet.Cells[row, 6].Value = entity.ApplyDate?.ToString("dd/MM/yyyy hh:mm");
                        //worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 7].Value = entity.FromDate?.ToString("dd/MM/yyyy hh:mm");
                        //worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 8].Value = entity.ToDate?.ToString("dd/MM/yyyy hh:mm");
                        //worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 9].Value = entity.BreakTimeFrom?.ToString("dd/MM/yyyy hh:mm");
                        //worksheet.Cells[row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 10].Value = entity.BreakTimeTo?.ToString("dd/MM/yyyy hh:mm");
                        //worksheet.Cells[row, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[row, 11].Value = entity.OverTimeInWorkingShift;
                        //worksheet.Cells[row, 12].Value = entity.WorkingShift;
                        //worksheet.Cells[row, 13].Value = entity.Reason;
                        //worksheet.Cells[row, 14].Value = entity.ApprovalName;
                        //worksheet.Cells[row, 15].Value = entity.RelationShipNames;
                        //worksheet.Cells[row, 16].Value = entity.Description;
                        //worksheet.Cells[row, 17].Value = entity.StatusName;
                        for(var i = 0; i< dataField.Count; i++)
                        {
                            var field = dataField[i];
                            switch(field)
                            {
                                case "EmployeeCode":
                                    worksheet.Cells[row, i + 1].Value = entity.EmployeeCode;
                                    worksheet.Column(i + 1).Width = 25;
                                    break;
                                case "FullName":
                                    worksheet.Cells[row, i + 1].Value = entity.FullName;
                                    worksheet.Column(i + 1).Width = 40;
                                    break;
                                case "PositionName":
                                    worksheet.Cells[row, i + 1].Value = entity.PositionName;
                                    worksheet.Column(i + 1).Width = 40;
                                    break;
                                case "DepartmentName":
                                    worksheet.Cells[row, i + 1].Value = entity.DepartmentName;
                                    worksheet.Column(i + 1).Width = 40;
                                    break;
                                case "ApplyDate":
                                    worksheet.Cells[row, i + 1].Value = entity.ApplyDate?.ToString("dd/MM/yyyy hh:mm");
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "FromDate":
                                    worksheet.Cells[row, i + 1].Value = entity.FromDate?.ToString("dd/MM/yyyy hh:mm");
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "ToDate":
                                    worksheet.Cells[row, i + 1].Value = entity.ToDate?.ToString("dd/MM/yyyy hh:mm");
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "BreakTimeFrom":
                                    worksheet.Cells[row, i + 1].Value = entity.BreakTimeFrom?.ToString("dd/MM/yyyy hh:mm");
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "BreakTimeTo":
                                    worksheet.Cells[row, i + 1].Value = entity.BreakTimeTo?.ToString("dd/MM/yyyy hh:mm");
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "OverTimeInWorkingShift":
                                    worksheet.Cells[row, i + 1].Value = entity.OverTimeInWorkingShift;
                                    worksheet.Column(i + 1).Width = 20;
                                    break;
                                case "WorkingShift":
                                    worksheet.Cells[row, i + 1].Value = entity.WorkingShift;
                                    worksheet.Column(i + 1).Width = 20;
                                    break;
                                case "Reason":
                                    worksheet.Cells[row, i + 1].Value = entity.Reason;
                                    worksheet.Column(i + 1).Width = 20;
                                    break;
                                case "ApprovalName":
                                    worksheet.Cells[row, i + 1].Value = entity.ApprovalName;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "RelationShipNames":
                                    worksheet.Cells[row, i + 1].Value = entity.RelationShipNames;
                                    worksheet.Column(i + 1).Width = 50;
                                    break;
                                case "Description":
                                    worksheet.Cells[row, i + 1].Value = entity.Description;
                                    worksheet.Column(i + 1).Width = 30;
                                    break;
                                case "StatusName":
                                    worksheet.Cells[row, i + 1].Value = entity.StatusName;
                                    worksheet.Column(i + 1).Width = 20;
                                    break;
                            }
                        }


                        // Tạo border 1 trường dữ liệu
                        //var recordRow = worksheet.Cells["A" + start++ + ":Q" + end++];
                        var recordRow = worksheet.Cells[listAlphabet[0] + start++ + ":"+ listAlphabet[listAlphabet.Length-1] + end++];

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

        #region Override
        /// <summary>
        /// Validate các dữ liệu đầu vào theo các rules validate của riêng class Employee
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">form body dữ liệu cần validate</param>
        /// <param name="isInsert">cờ xác định xem có phải là API thêm mới không</param>
        /// <returns>Danh sách các lỗi</returns>
        public override List<ErrorResult> ValidateRecordCustom(OverTime record, bool isInsert)
        {
            var validateFailuresCustom = new List<ErrorResult>();
            var result = _overTimeDL.CheckOverTimeDuplicate(record);
            if(result.IsSuccess == false)
            {
                validateFailuresCustom.Add(new ErrorResult
                {
                    ErrorCode = ErrorCode.DuplicateOverTime,
                    DevMsg = Resource.OverTime_Duplicate,
                    UserMsg = Resource.OverTime_Duplicate
                });
            }

            return validateFailuresCustom;
        }

        #endregion

    }
}
