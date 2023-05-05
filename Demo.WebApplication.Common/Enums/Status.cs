using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Enums
{
    /// <summary>
    /// Trạng thái
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Tất cả
        /// </summary>
        All = 0,

        /// <summary>
        /// Chờ duyệt
        /// </summary>
        Wait = 1,

        /// <summary>
        /// Đã duyệt
        /// </summary>
        Approved = 2,

        /// <summary>
        /// Từ chối
        /// </summary>
        Denied = 3,
    }
}
