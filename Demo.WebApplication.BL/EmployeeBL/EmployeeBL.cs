using Dapper;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.EmployeeDL;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Demo.WebApplication.Common.Attibutes.Attributes;
using Color = System.Drawing.Color;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public class EmployeeBL : BaseBL<Employee> ,IEmployeeBL
    {
        #region Field
        
        public IEmployeeDL _employeeDL;
        public IBaseDL<Employee> _baseDL;

        #endregion

        #region Constructor

        public EmployeeBL(IEmployeeDL employeeDL, IBaseDL<Employee> baseDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy mã nhân viên kế tiếp
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>Mã nhân viên kết tiếp</returns>
        public ServiceResult GetNewEmployeeCode()
        {
            //Lấy mã nhân viên lớn nhất
            var result = _employeeDL.GetNewEmployeeCode();

            if(result.IsSuccess == true)
            {
                //Tách thành 2 phần kí tự và số
                string maxCode = result.Data.ToString();
                string[] code = maxCode.Split("-");

                //tăng phần số thứ tự lên 1
                int nextNumber = int.Parse(code[1]) + 1;
                String newCode = code[0] + "-" + nextNumber.ToString();

                //trả về mã kế tiếp
                return new ServiceResult(true, newCode);
            }
            else
            {
                if(result.Data == Resource.ServiceResult_Fail)
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Exception);
                }
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
        public PagingResult GetPaging(string? keyword,String? MISACode, int pageSize = 50, int offSet = 0)
        {
            var result =  _employeeDL.GetPaging(keyword, MISACode, pageSize, offSet);

            var totalRecord = result["Total"];
            var resultArray = (List<Employee>)result["PageData"];

            //gán tên giới tính theo giá trị giới tính
            foreach (Employee emp in resultArray)
            {
                //Giới tính nam
                if (emp.Gender == Gender.Male)
                {
                    emp.GenderName = Resource.Male;
                }
                //giới tính nữ
                else if (emp.Gender == Gender.Female)
                {
                    emp.GenderName = Resource.Female;
                }
                //giới tính khác
                else if (emp.Gender == Gender.Other)
                {
                    emp.GenderName = Resource.Other;
                }

            }

            //index bản ghi đầu của trang luôn >=1
            var begin = (offSet + 1) > 0 ? (offSet + 1) : 1;

            //index bản ghi cuối của trang luôn <= tổng số bản ghi
            var end = (begin + pageSize) <= Convert.ToInt32(totalRecord) ? (begin + pageSize - 1) : Convert.ToInt32(totalRecord);

            return new PagingResult(resultArray, Convert.ToInt32(totalRecord), begin, end);
        }

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách id các bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult MultipleDelete(string IDs)
        {
            return _employeeDL.MultipleDelete(IDs);
        }

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        //public MemoryStream ExcelExport(ExportDataParams param)
        //{
        //    var AllData = _employeeDL.ExcelExport(param);

        //    //gán tên giới tính theo giá trị giới tính
        //    foreach (Employee emp in AllData)
        //    {
        //        //Giới tính nam
        //        if (emp.Gender == Gender.Male)
        //        {
        //            emp.GenderName = Resource.Male;
        //        }
        //        //giới tính nữ
        //        else if (emp.Gender == Gender.Female)
        //        {
        //            emp.GenderName = Resource.Female;
        //        }
        //        //giới tính khác
        //        else if (emp.Gender == Gender.Other)
        //        {
        //            emp.GenderName = Resource.Other;
        //        }
        //    }

        //    DataTable Dt = new DataTable();

            

        //    Dt.Columns.Add(Resource.Title_Index, typeof(string));
        //    Dt.Columns.Add(Resource.Title_EmployeeCode, typeof(string));
        //    Dt.Columns.Add(Resource.Title_FullName, typeof(string));
        //    Dt.Columns.Add(Resource.Title_Gender, typeof(string));
        //    Dt.Columns.Add(Resource.Title_DateOfBirth, typeof(string));
        //    Dt.Columns.Add(Resource.Title_IdentityNumber, typeof(string));
        //    Dt.Columns.Add(Resource.Title_Position, typeof(string));
        //    Dt.Columns.Add(Resource.Title_Department, typeof(string));
        //    Dt.Columns.Add(Resource.Title_BankNumber, typeof(string));
        //    Dt.Columns.Add(Resource.Title_BankName, typeof(string));
        //    Dt.Columns.Add(Resource.Title_BankBranch, typeof(string));

        //    int stt = 1;

        //    foreach (var data in AllData)
        //    {
        //        DataRow row = Dt.NewRow();
        //        row[0] = stt++;
        //        row[1] = data.EmployeeCode;
        //        row[2] = data.FullName;
        //        row[3] = data.GenderName;
        //        row[4] = data.DateOfBirth?.ToString("dd-MM-yyyy");
        //        row[5] = data.IdentityNumber;
        //        row[6] = data.PositionName;
        //        row[7] = data.DepartmentName;
        //        row[8] = data.BankNumber;
        //        row[9] = data.BankName;
        //        row[10] = data.BankBranch;
        //        Dt.Rows.Add(row);
        //    }

        //    var stream = new MemoryStream();
        //    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        //    using (var package = new ExcelPackage(stream))
        //    {
        //        var workSheet = package.Workbook.Worksheets.Add("Sheet1");

        //        //Chỉnh độ rộng cột
        //        workSheet.Column(1).Width = 5;
        //        workSheet.Column(2).Width = 15;
        //        workSheet.Column(3).Width = 30;
        //        workSheet.Column(4).Width = 10;
        //        workSheet.Column(5).Width = 20;
        //        workSheet.Column(6).Width = 20;
        //        workSheet.Column(7).Width = 30;
        //        workSheet.Column(8).Width = 30;
        //        workSheet.Column(9).Width = 20;
        //        workSheet.Column(10).Width = 30;
        //        workSheet.Column(11).Width = 30;

        //        // Định dạng tên bảng
        //        workSheet.Cells[1, 1].Value = Resource.Excel_Table_Name;
        //        workSheet.Row(1).Height = 24;
        //        workSheet.Cells[1, 1].Style.Font.Size = 18;
        //        workSheet.Cells[1, 1, 1, Dt.Columns.Count].Merge = true;
        //        workSheet.Cells[1, 1].Style.Font.Bold = true;
        //        workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        //Load dữ liệu vào bảng
        //        workSheet.Cells[3,1].LoadFromDataTable(Dt, true);

        //        // Định dạng tiêu đề bảng
        //        for (int i = 1; i <= Dt.Columns.Count; i++)
        //        {
        //            workSheet.Cells[3, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            workSheet.Cells[3, i].Style.Font.Bold = true;
        //            workSheet.Cells[3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            workSheet.Cells[3, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        //        }

        //        // Căn giữa cột stt và Ngày sinh
        //        for (int i = 1; i <= Dt.Rows.Count + 3; i++)
        //        {
        //            workSheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            workSheet.Cells[i, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }

        //        //Thêm boder cho các ô trong bảng
        //        for (int row = 3; row <= Dt.Rows.Count + 3; row++)
        //        {
        //            for (int col = 1; col <= Dt.Columns.Count; col++)
        //            {
        //                workSheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            }
        //        }

        //            package.Save();
        //    }
        //    stream.Position = 0;
        //    string excelName = $"UserList.xlsx-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
        //    return stream;
        //}

        /// <summary>
        /// Lấy thông tin nhân viên theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id nhân viên muốn lấy thông tin</param>
        /// <returns>thông tin nhân viên</returns>
        public ServiceResult GetEmployeeById(Guid recordId)
        {
            return _employeeDL.GetEmployeeById(recordId);
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
        public override List<ErrorResult> ValidateRecordCustom(Employee record, bool isInsert)
        {
            var validateFailuresCustom = new List<ErrorResult>();
            var properties = typeof(Employee).GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

                var notDuplicateAttribute = (NotDuplicateAttribute?)property.GetCustomAttributes(typeof(NotDuplicateAttribute), false).FirstOrDefault();
                var codeFormatAttribute = (CodeFormatAttribute?)property.GetCustomAttributes(typeof(CodeFormatAttribute), false).FirstOrDefault();
                var isEmailAttribute = (IsEmailAttribute?)property.GetCustomAttributes(typeof(IsEmailAttribute), false).FirstOrDefault();
                var onlyNumberAttribute = (OnlyNumberAttribute?)property.GetCustomAttributes(typeof(OnlyNumberAttribute), false).FirstOrDefault();

                //validate trường có định dạng mã (phải có dạng NV-x trong đó x là số thứ tự nhân viên)
                if (codeFormatAttribute != null)
                {
                    string code = propertyValue.ToString();
                    string[] array = code.Split("-");
                    if (array.Length != 2 || array[0] != "NV" || Regex.IsMatch(array[1], @"^\d+$") == false)
                    {
                        validateFailuresCustom.Add( new ErrorResult{ 
                            ErrorField = propertyName, 
                            ErrorCode = ErrorCode.WrongFormatCode, 
                            DevMsg = Resource.DevMsg_ValidateFailed, 
                            UserMsg = Resource.WrongCodeFormat 
                        });
                    }
                }

                //validate trường không được trùng lặp
                //nếu là API thêm mới thì check mã nhân viên không trùng, là API sửa thì không check
                if (notDuplicateAttribute != null && isInsert == true)
                {
                    var result = _baseDL.CheckDuplicateID(propertyValue.ToString());
                    if (result.IsSuccess == true)
                    {
                        validateFailuresCustom.Add( new ErrorResult
                        {
                            ErrorField = propertyName,
                            ErrorCode = ErrorCode.DuplicateCode,
                            DevMsg = Resource.DevMsg_ValidateFailed,
                            UserMsg = Resource.DuplicateCode,

                        });
                    }
                }

                //validate trường có định dạng email
                if (isEmailAttribute != null)
                {
                    if (propertyValue != null && propertyValue.ToString().Trim() != "")
                    {
                        bool isValidate = Regex.IsMatch(propertyValue.ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        if (!isValidate)
                        {
                            validateFailuresCustom.Add(new ErrorResult { 
                                ErrorField = propertyName,
                                ErrorCode = ErrorCode.WrongFormatEmail,
                                DevMsg = Resource.DevMsg_ValidateFailed,
                                UserMsg = Resource.WrongEmailFormat,
                                });
                        }
                    }
                }

                //validate trường có định dạng chỉ chứa các chữ số
                if (onlyNumberAttribute != null)
                {
                    if (propertyValue != null && propertyValue.ToString().Trim() != "")
                    {
                        string value = propertyValue.ToString();
                        bool isNumeric = Regex.IsMatch(value, @"^\d+$");
                        if (!isNumeric)
                        {
                            validateFailuresCustom.Add( new ErrorResult{ 
                                ErrorField = propertyName, 
                                ErrorCode = ErrorCode.WrongFormatOnlyNumber, 
                                DevMsg = Resource.DevMsg_ValidateFailed, 
                                UserMsg = Resource.WrongOnlyNumberFormat
                            });
                        }
                    }
                }
            }

            return validateFailuresCustom;
        }

        #endregion
    }
}
