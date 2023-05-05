using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.Hosting.Server;
using MySqlConnector;
using Demo.WebApplication.DL.EmployeeDL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common;
using Demo.WebApplication.BL.EmployeeBL;

//test exception
//throw new NotImplementedException();

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/v1/[controller]")] //attribute: http://localhost:43154/api/[controller]
    [ApiController]
    public class EmployeesController : BasesController<Employee>
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Constructor
        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Phân trang nhân viên
        /// author:VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">Mã phòng ban</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">vị trí bắt đầu</param>
        /// <returns>mảng các bản ghi đã lọc</returns>
        [HttpGet("Filter")]
        public IActionResult GetPaging(
            [FromQuery] String? keyword,
            [FromQuery] String? MISACode,
            [FromQuery] int pageSize = 10,
            [FromQuery] int offSet = 0
            )
        {
            try
            {
                var pagingData = _employeeBL.GetPaging(keyword, MISACode, pageSize, offSet);
                
                if (pagingData != null)
                {
                    return StatusCode(200, pagingData);
                }
                else
                {
                    return StatusCode(204, new ErrorResult
                    {
                        ErrorCode = ErrorCode.SqlReturnNull,
                        DevMsg = Resource.ServiceResult_Fail,
                        UserMsg = Resource.UserMsg_Exception,
                        TradeId = HttpContext.TraceIdentifier,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Exception,
                    TradeId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API lấy mã nhân viên tiếp theo
        /// author: VietDV(11/3/2023)
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                var serviceResult = _employeeBL.GetNewEmployeeCode();
                if(serviceResult.IsSuccess == true)
                {
                    return StatusCode(200, serviceResult.Data);
                }
                else
                {
                    if (serviceResult.Data == Resource.ServiceResult_Fail)
                    {
                        return StatusCode(204, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(500, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlCatchException,
                            DevMsg = Resource.ServiceResult_Exception,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Exception,
                    TradeId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API xoá nhiều nhân viên theo danh sách id
        /// author: VietDV(11/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách ID</param>
        /// <returns>1 nếu thành công</returns>
        [HttpDelete("MultipleDelete")]
        public IActionResult MultipleDelete([FromQuery] MultipleDeleteParams IDs)
        {
            try
            {
                var serviceResult = _employeeBL.MultipleDelete(IDs.listID);

                if(serviceResult.IsSuccess == true)
                {
                    return StatusCode(200, QueryResult.Success);
                }
                else
                {
                    if (serviceResult.Data == Resource.ServiceResult_Fail)
                    {
                        return StatusCode(204, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(500, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlCatchException,
                            DevMsg = Resource.ServiceResult_Exception,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Exception,
                    TradeId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API xuất khẩu file excel
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        //[HttpGet("Export")]
        //public IActionResult ExcelExport([FromQuery] ExportDataParams param)
        //{
        //    var stream = _employeeBL.ExcelExport(param);
        //    string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

        //    return File(stream, "application/vnd.ms-excel", excelName);
        //}

        /// <summary>
        /// API Lấy thông tin chi tiết 1 nhân viên
        /// Created by: VietDV(09/03/2001)
        /// </summary>
        /// <param name="id">ID nhân viên muốn lấy</param>
        /// <returns>Đối tượng nhân viên</returns>
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _employeeBL.GetEmployeeById(id);

                if (serviceResult.IsSuccess == true)
                {
                    return StatusCode(200, serviceResult.Data);
                }
                else
                {
                    if (serviceResult.Data == Resource.ServiceResult_Fail)
                    {
                        return StatusCode(204, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(500, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlCatchException,
                            DevMsg = Resource.ServiceResult_Exception,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Exception,
                    TradeId = HttpContext.TraceIdentifier,
                });
            }

        }
        #endregion
    }

    
}
