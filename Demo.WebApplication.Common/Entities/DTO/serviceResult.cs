using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class ServiceResult
    {
        #region Field
        public bool IsSuccess;

        public object Data;
        #endregion

        #region Constructor
        /// <summary>
        /// Kết quả trả về từ server
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="isSuccess">trạng thái thực hiện câu lệnh sql có thành công không</param>
        /// <param name="data">dữ liệu trả về kèm theo</param>
        public ServiceResult(bool isSuccess, object data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        #endregion
    }
}
