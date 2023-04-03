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
        /// lấy thông tin nhân viên theo phân trang
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="keyword">từ khoá tìm kiếm (tìm kiếm theo tên hoặc mã)</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="offSet">index bản ghi bắt đầu</param>
        /// <returns>các bản ghi thoả mã điều kiện</returns>
        public pagingResult GetPaging(
            String? keyword,
            int pageSize = 10,
            int offSet = 0
            );

        /// <summary>
        /// Lấy mã nhân viên kế tiếp
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>Mã nhân viên kết tiếp</returns>
        public serviceResult GetNewEmployeeCode();

        /// <summary>
        /// Xoá nhiều bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách id các bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult MultipleDelete(String IDs);

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        public MemoryStream ExcelExport(exportDataParams param);
    }
}
