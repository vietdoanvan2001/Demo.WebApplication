using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Enums
{
    /// <summary>
    /// Thời điểm làm thêm
    /// </summary>
    public enum OverTimeInWorkingShift
    {
        /// <summary>
        /// Trước ca
        /// </summary>
        BeforeShift = 0,

        /// <summary>
        /// Sau ca
        /// </summary>
        AfterShift = 1,

        /// <summary>
        /// Nghỉ giữa ca 
        /// </summary>
        MiddleShift = 2,

        /// <summary>
        /// Ngày nghỉ
        /// </summary>
        DayOff = 3,
    }
}
