namespace Demo.WebApplication.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// Id phòng ban
        /// </summary>
        public String DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public String MISACode { get; set; }
        
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public String DepartmentName { get; set; }

        /// <summary>
        /// Id phòng ban cha
        /// </summary>
        public String ParentId { get; set; }

        /// <summary>
        /// Thuộc tính xác định sổ dữ liệu xuống
        /// </summary>
        public int Expanded { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public String ModifiedBy { get; set; }
    }
}
