using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.DL.OverTimeDetailDL
{
    public interface IOverTimeDetailDL : IBaseDL<OverTimeDetail>
    {
        /// <summary>
        /// xoá tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public int DeleteRecordByOverTimeId(Guid overTimeId);

        /// <summary>
        /// lấy tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public IEnumerable<OverTimeDetail> GetAllRecordById(Guid overTimeId);
    }
}
