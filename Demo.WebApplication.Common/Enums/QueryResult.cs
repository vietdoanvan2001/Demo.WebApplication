using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Enums
{
    /// <summary>
    /// Kết quả khi thực hiện câu lệnh sql
    /// </summary>
    public enum QueryResult
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 1 ,

        /// <summary>
        /// Thất bại
        /// </summary>
        Fail = 0,
    }
}
