using Demo.WebApplication.Common.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static Demo.WebApplication.Common.Attibutes.Attributes;

namespace Demo.WebApplication.Common.Entities
{
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }

        /// <summary>
        ///Mã nhân viên 
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [CodeFormat]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [NotEmpty]
        public string FullName { get; set; }

        /// <summary>
        /// Giới tính (0:Nữ; 1:Nam; 2:Khác)
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số chứng minh nhân dân
        /// </summary>
        [OnlyNumber]
        public String? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp chứng minh nhân dân
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp chứng minh nhân dân
        /// </summary>
        public String? IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public String? Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [OnlyNumber]
        public String? PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        [OnlyNumber]
        public String? LandLine { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [IsEmail]
        public String? Email { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public String? BankNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public String? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public String? BankBranch { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        [NotEmpty]
        public String? DepartmentId { get; set; }

        /// <summary>
        /// Mã vị trí
        /// </summary>
        public String? MISACode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public String? DepartmentName { get; set; }

        /// <summary>
        /// Id chức vụ
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public String? PositionName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [CurrentTime]
        public DateTime? CreatedDate { get; set;}

        /// <summary>
        /// Người tạo
        /// </summary>
        public String ? CreatedBy { get; set;}

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        [CurrentTime]
        public DateTime ? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public String ? ModifiedBy { get; set; }

        /// <summary>
        /// Tên giới tính
        /// </summary>
        public String? GenderName { get; set; }

    }
}
