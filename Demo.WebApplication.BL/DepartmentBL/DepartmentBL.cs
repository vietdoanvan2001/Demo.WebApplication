using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.DL.DepartmentDL;
using Demo.WebApplication.DL.EmployeeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.DepartmentBL
{
    public class DepartmentBL : BaseBL<Department> ,IDepartmentBL
    {
        #region Field

        public IDepartmentDL _departmentDL;

        #endregion

        #region Constructor

        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy thông tin phòng ban theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="departmentId">id phòng ban muốn lấy thông tin</param>
        /// <returns>thông tin nhân viên</returns>
        public ServiceResult GetDepartmentById(String departmentId)
        {
            return _departmentDL.GetDepartmentById(departmentId);
        } 
        #endregion
    }
}
