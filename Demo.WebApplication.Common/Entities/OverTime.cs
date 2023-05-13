using Demo.WebApplication.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo.WebApplication.Common.Attibutes.Attributes;

namespace Demo.WebApplication.Common.Entities
{
    public class OverTime
    {
        /// <summary>
        /// ID bảng đăng ký làm thêm
        /// </summary>
        [Id]
        public Guid OverTimeId { get; set; }

        /// <summary>
        /// ID nhân viên đăng ký làm thêm
        /// </summary>
        [NotEmpty]
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên đăng ký làm thêm
        /// </summary>
        [NotEmpty]
        public String? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên đăng ký làm thêm
        /// </summary>
        [NotEmpty]
        public String? FullName { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        public String? DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public String? MISACode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public String? DepartmentName { get; set; }

        /// <summary>
        /// Id chức vụ công việc
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public String? PositionName { get; set; }

        /// <summary>
        /// Ngày nộp đơn
        /// </summary>
        [NotEmpty]
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// Làm thêm từ
        /// </summary>
        [NotEmpty]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Nghỉ giữa ca từ
        /// </summary>
        public DateTime? BreakTimeFrom { get; set; }

        /// <summary>
        /// Nghỉ giữa ca đến
        /// </summary>
        public DateTime? BreakTimeTo { get; set;}

        /// <summary>
        /// Làm thêm đến
        /// </summary>
        [NotEmpty]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// giá trị Thời điểm làm thêm (0:Trước ca, 1: Sau ca, 2: Nghỉ giữa ca, 3:Ngày nghỉ)
        /// </summary> 
        [NotEmpty]
        public OverTimeInWorkingShift? OverTimeInWorkingShiftName { get; set; }

        /// <summary>
        /// Tên thời điểm làm thêm
        /// </summary>
        public String? OverTimeInWorkingShift { get; set; }

        /// <summary>
        /// Giá trị Ca áp dụng (0:Ca 1, 1: Ca 2, 2: Ca đêm, 3:Ca hành chính)
        /// </summary>
        [NotEmpty]
        public WorkingShift? WorkingShiftName { get; set; }

        /// <summary>
        /// Tên ca áp dụng
        /// </summary>
        public String? WorkingShift { get; set; }

        /// <summary>
        /// Lý do làm thêm
        /// </summary>
        [NotEmpty]
        public String? Reason { get; set; }

        /// <summary>
        /// Tên người duyệt
        /// </summary>
        public String? ApprovalName { get; set; }

        /// <summary>
        /// Id người duyệt
        /// </summary>
        [NotEmpty]
        public Guid? ApprovalId { get; set; }

        /// <summary>
        /// Danh sách Id người liên quan
        /// </summary>
        public String? RelationShipIDs { get; set; }

        /// <summary>
        /// Danh sách tên người liên quan
        /// </summary>
        public String? RelationShipNames { get; set; }

        /// <summary>
        /// giá trị trạng thái (0: tất cả 1: chờ duyệt, 2: Đã duyệt, 3: từ chối)
        /// </summary>
        [NotEmpty]
        public Status? Status { get; set; }

        /// <summary>
        /// Tên trạng thái
        /// </summary>
        public String? StatusName { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public String? Description { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [CurrentTime]
        public DateTime? CreatedDate { get; set;}

        /// <summary>
        /// Người tạo
        /// </summary>
        public String? CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        [CurrentTime]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public String? ModifiedBy { get; set;}

        [PrimaryKey]
        public List<OverTimeDetail> OvertimeEmployee { get; set; }
    }
}
