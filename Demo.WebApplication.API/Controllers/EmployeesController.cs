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
        /// API phân trang và tìm kiếm theo keyword ( Tên nhân viên hoặc mã nhân viên)
        /// author: VietDV(11/3/2023)
        /// </summary>
        /// <param name="keyword">Tên nhân viên hoặc mã nhân viên</param>
        /// <param name="pageSize">số bản ghi 1 trang</param>
        /// <param name="pageNumber">trang cần tìm</param>
        /// <returns>Danh sách nhân viên thoả mãn và tổng số bản ghi thoả mãn</returns>
        [HttpGet("Filter")]
        public IActionResult GetPaging(
            [FromQuery] String? keyword,
            [FromQuery] int pageSize = 10,
            [FromQuery] int offSet = 0
            )
        {
            try
            {
                var pagingData = _employeeBL.GetPaging(keyword, pageSize, offSet);
                
                if (pagingData != null)
                {
                    return StatusCode(200, pagingData);
                }
                else
                {
                    return StatusCode(204, new errorResult
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
                return StatusCode(500, new errorResult
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
                        return StatusCode(204, new errorResult
                        {
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(500, new errorResult
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
                return StatusCode(500, new errorResult
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
        public IActionResult MultipleDelete([FromQuery] multipleDeleteParams IDs)
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
                        return StatusCode(204, new errorResult
                        {
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(500, new errorResult
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
                return StatusCode(500, new errorResult
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
        [HttpGet("Export")]
        public IActionResult ExcelExport([FromQuery] exportDataParams param)
        {
            var stream = _employeeBL.ExcelExport(param);
            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.ms-excel", excelName);
        }
        #endregion
    }

    
}
