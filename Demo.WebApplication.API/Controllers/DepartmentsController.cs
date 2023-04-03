using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApplication.BL.DepartmentBL;
using Demo.WebApplication.Common.Entities;

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/v1/[controller]")] //attribute: http://localhost:43154/api/[controller]
    [ApiController]
    public class DepartmentsController : BasesController<Department>
    {
        #region Field

        private IDepartmentBL _departmentBL;

        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;
        }
        #endregion

        #region Method
        
        #endregion

    }
}
