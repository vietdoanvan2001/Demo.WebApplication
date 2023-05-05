using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.BL.BaseBL;

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method
        /// <summary>
        /// API sửa thông tin chi tiêt 1 bản ghi
        /// author: VietDV(11/3/2023)
        /// </summary>
        /// <param name="recordId">Id bản ghi cần sửa</param>
        /// <param name="record">Thông tin cập nhật của bản ghi</param>
        /// <returns>1 nếu thành công</returns>
        [HttpPut("{recordId}")]
        public IActionResult UpdateRecord([FromRoute] Guid recordId, [FromBody] T record)
        {
            try
            {

                var serviceResult = _baseBL.UpdateRecord(recordId, record);


                if (serviceResult.IsSuccess == true)
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
                    else if (serviceResult.Data == Resource.ServiceResult_Exception)
                    {
                        return StatusCode(500, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlCatchException,
                            DevMsg = Resource.ServiceResult_Exception,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(400, serviceResult.Data);
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

        /// <summary>
        /// API Thêm mới thông tin bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T record)
        {
            try
            {

                var serviceResult = _baseBL.InsertRecord(record);

                if (serviceResult.IsSuccess == true)
                {
                    return StatusCode(201, QueryResult.Success);
                }
                else
                {
                    if(serviceResult.Data == Resource.ServiceResult_Fail)
                    {
                        return StatusCode(204, new ErrorResult 
                        { 
                            ErrorCode = ErrorCode.SqlReturnNull,
                            DevMsg = Resource.ServiceResult_Fail,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else if(serviceResult.Data == Resource.ServiceResult_Exception)
                    {
                        return StatusCode(500, new ErrorResult
                        {
                            ErrorCode = ErrorCode.SqlCatchException,
                            DevMsg = Resource.ServiceResult_Exception,
                            UserMsg = Resource.UserMsg_Exception,
                            TradeId = HttpContext.TraceIdentifier,
                        });
                    }
                    else
                    {
                        return StatusCode(400, serviceResult.Data);
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

        /// <summary>
        /// API xoá thông tin bản ghi
        /// author: VietDV(11/3/2023)
        /// </summary>
        /// <param name="id">Id bản ghi cần xoá</param>
        /// <returns>1 nếu thành công</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteRecordById([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _baseBL.DeleteRecordById(id);


                if (serviceResult.IsSuccess == true)
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
        /// API lấy toàn bộ danh sách bản ghi
        /// author: VietDV(09/03/2023)
        /// </summary>
        /// <returns>Danh sách tất cả các bản ghi</returns>
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {

                var serviceResult = _baseBL.GetAllRecords();

                if(serviceResult.IsSuccess == true)
                {
                    return StatusCode(200,serviceResult.Data);
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
