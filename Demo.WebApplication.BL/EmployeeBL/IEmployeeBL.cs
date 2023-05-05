using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        /// <summary>
        /// Phân trang nhân viên
        /// author:VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">Mã phòng ban</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">vị trí bắt đầu</param>
        /// <returns>mảng các bản ghi đã lọc</returns>
        public PagingResult GetPaging(
            String? keyword,
            String? MISACode,
            int pageSize = 50,
            int offSet = 0
            );

        /// <summary>
        /// Lấy mã nhân viên kế tiếp
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>Mã nhân viên kết tiếp</returns>
        public ServiceResult GetNewEmployeeCode();

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách id các bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult MultipleDelete(String IDs);

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        //public MemoryStream ExcelExport(ExportDataParams param);

        /// <summary>
        /// Lấy thông tin nhân viên theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id nhân viên muốn lấy thông tin</param>
        /// <returns>thông tin nhân viên</returns>
        public ServiceResult GetEmployeeById(Guid recordId);
    }
}
