using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.OverTimeBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.OverTimeDetailDL;
using Demo.WebApplication.DL.OverTimeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.OverTimeDetailBL
{
    public class OverTimeDetailBL : BaseBL<OverTimeDetail>, IOverTimeDetailBL
    {
        #region Field

        public IOverTimeDetailDL _overtimeDetailDL;

        #endregion

        #region Constructor

        public OverTimeDetailBL(IBaseDL<OverTimeDetail> baseDL, IOverTimeDetailDL overtimeDetailDL) : base(baseDL)
        {
            _overtimeDetailDL = overtimeDetailDL;
        }

        #endregion


        #region Method
        /// <summary>
        /// xoá tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public void DeleteRecordByOverTimeId(Guid overTimeId)
        {
            _overtimeDetailDL.DeleteRecordByOverTimeId(overTimeId);
        }

        /// <summary>
        /// lấy tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public OverTime GetAllRecordById(OverTime record, Guid overTimeId)
        {
            var employees = _overtimeDetailDL.GetAllRecordById(overTimeId);
            record.OvertimeEmployee = (List<OverTimeDetail>)employees;
            
            return record;
        }
        #endregion
    }
}

