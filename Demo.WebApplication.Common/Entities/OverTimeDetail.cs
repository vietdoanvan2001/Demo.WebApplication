using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo.WebApplication.Common.Attibutes.Attributes;

namespace Demo.WebApplication.Common.Entities
{
    public class OverTimeDetail
    {
        /// <summary>
        /// Id bảng nhân viên làm thêm
        /// </summary>
        [PrimaryKey]
        public Guid OverTimeDetailID { get; set; }

        /// <summary>
        /// Id bảng thông tin làm thêm
        /// </summary>
        public Guid OverTimeId { get; set; }

        /// <summary>
        /// Id nhân viên làm thêm
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// mã nhân viên làm thêm
        /// </summary>
        public String EmployeeCode { get; set;}

        /// <summary>
        /// Tên nhân viên làm thêm
        /// </summary>
        public String FullName { get; set;}

        /// <summary>
        /// Id phòng ban
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public String MISACode { get; set;}

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public String DepartmentName { get; set;}

        /// <summary>
        /// Id vị trí
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public String PositionName { get; set;}
    }
}
