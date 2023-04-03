using Demo.WebApplication.Common.Enums;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class errorResult
    {

        /// <summary>
        /// Tên trường dữ liệu lỗi
        /// </summary>
        public String ErrorField { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public ErrorCode ErrorCode { get; set; } 
        
        /// <summary>
        /// Thông báo cho dev
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Thông báo cho người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// ID phục vụ cho tìm lỗi nhanh (Sử dụng trong dự án)
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// Mô tả chi tiết lỗi
        /// </summary>
        public object MoreInfor { get; set; }

    }
}
