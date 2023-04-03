using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Demo.WebApplication.DL.EmployeeDL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Lấy thông tin phân trang nhân viên
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="keyword">từ khoá tìm kiếm(tìm kiếm theo tên nhân viên hoặc mã nhân viên)</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="offSet">thứ tự bản ghi bắt đầu của trang</param>
        /// <returns>danh sách các nhân viên thoả mãn</returns>
        /// <returns>tổng số nhân viên trong db</returns>
        public Dictionary<string, object> GetPaging(
            String? keyword,
            int pageSize = 10,
            int offSet = 0
            );

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>mã nhân viên lớn nhất trong db</returns>
        public serviceResult GetNewEmployeeCode();

        /// <summary>
        /// Xoá nhiều nhân viên
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách các id nhân viên muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult MultipleDelete(String IDs);

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        public List<Employee> ExcelExport(exportDataParams param);

    }
}
