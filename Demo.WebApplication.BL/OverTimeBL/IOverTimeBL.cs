using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.OverTimeBL
{
    public interface IOverTimeBL : IBaseBL<OverTime>
    {
        /// <summary>
        /// Lấy thông tin làm thêm theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id làm thêm muốn lấy thông tin</param>
        /// <returns>thông tin làm thêm</returns>
        public ServiceResult GetOverTimeById(Guid recordId);

        /// <summary>
        /// Phân trang bảng làm thêm
        /// author: VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">mã phòng ban</param>
        /// <param name="status">trạng thái</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">bản ghi bắt đầu</param>
        /// <returns>mảng các bản ghi được lọc</returns>
        public OverTimeFilterResult GetPaging(
            String? keyword,
            String? MISACode,
            int? status,
            int pageSize = 10,
            int offSet = 0
            );

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult MultipleDelete(String IDs);

        /// <summary>
        /// Chuyển trạng thái các bản ghi đã chọn
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ServiceResult ChangeStatus(String IDs, int status);

        /// <summary>
        /// Xuất khẩu toàn bộ dữ liệu làm thêm
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="param">truy vấn lọc</param>
        /// <returns>Danh sách bản ghi thoả mãn</returns>
        public MemoryStream ExcelExport(OverTimesExportDataParams param);

        /// <summary>
        /// Xuất khẩu các dữ liệu làm thêm được chọn
        /// author: VietDV(30/4/2023)
        /// </summary>
        /// <param name="param">Danh sách các bản ghi</param>
        /// <returns>File excel</returns>
        public MemoryStream ExcelExportSelected(List<OverTime> listOverTimes);
    }
}
