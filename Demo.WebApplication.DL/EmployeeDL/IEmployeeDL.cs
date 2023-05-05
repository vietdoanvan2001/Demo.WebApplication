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
        /// Phân trang nhân viên
        /// author:VietDV(26/4/2023)
        /// </summary>
        /// <param name="keyword">Tên hoặc mã nhân viên</param>
        /// <param name="MISACode">Mã phòng ban</param>
        /// <param name="pageSize">số bản ghi trên trang</param>
        /// <param name="offSet">vị trí bắt đầu</param>
        /// <returns>mảng các bản ghi đã lọc</returns>
        public Dictionary<string, object> GetPaging(
            String? keyword,
            String? MISACode,
            int pageSize = 10,
            int offSet = 0
            );

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns>mã nhân viên lớn nhất trong db</returns>
        public ServiceResult GetNewEmployeeCode();

        /// <summary>
        /// Xoá nhiều nhân viên
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="IDs">Danh sách các id nhân viên muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult MultipleDelete(String IDs);

        /// <summary>
        /// API xuất dữ liệu ra file excel
        /// Author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        //public List<Employee> ExcelExport(ExportDataParams param);


        /// <summary>
        /// Lấy thông tin nhân viên theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="employeeId">id nhân viên muốn lấy thông tin</param>
        /// <returns>thông tin nhân viên</returns>
        public ServiceResult GetEmployeeById(Guid employeeId);

    }
}
