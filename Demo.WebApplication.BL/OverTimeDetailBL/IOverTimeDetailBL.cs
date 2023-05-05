using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.OverTimeDetailBL
{
    public interface IOverTimeDetailBL : IBaseBL<OverTimeDetail>
    {
        #region Method
        /// <summary>
        /// xoá tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public void DeleteRecordByOverTimeId(Guid overTimeId);

        /// <summary>
        /// lấy tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public OverTime GetAllRecordById(OverTime record, Guid overTimeId);
        #endregion
    }
}
