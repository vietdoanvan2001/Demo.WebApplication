using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.OverTimeDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.DL.OverTimeDetailDL
{
    public class OverTimeDetailDL : BaseDL<OverTimeDetail>, IOverTimeDetailDL
    {
        #region Method
        /// <summary>
        /// xoá tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public int DeleteRecordByOverTimeId(Guid overTimeId)
        {
            int affetecRows = 0;
            using (var dbConnection = GetOpenConnection())
            {

                var tran = dbConnection.BeginTransaction();

                var storedProcedureName = "Proc_OverTimeDetail_DeleteAllById";
                var paprameters = new DynamicParameters();
                paprameters.Add("v_OverTimeId", overTimeId);

                try
                {
                    affetecRows = dbConnection.Execute(
                                storedProcedureName,
                                paprameters,
                                commandType: System.Data.CommandType.StoredProcedure,
                                transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
                finally
                {
                    dbConnection.Close();
                }

                return affetecRows;
            }
        }

        /// <summary>
        /// lấy tất cả request detail theo id của cha
        /// </summary>
        /// <param name="overTimeId"></param>
        /// <returns></returns>
        public IEnumerable<OverTimeDetail> GetAllRecordById( Guid overTimeId)
        {
            using (var dbConnection = GetOpenConnection())
            {
                var storedProcedureName = "Proc_OverTimeDetail_GetByOverTimeId";
                var paprameters = new DynamicParameters();
                paprameters.Add("v_OverTimeId", overTimeId);

                var employees = dbConnection.Query<OverTimeDetail>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);
                return employees;
            }

        }
        #endregion
    }
}
