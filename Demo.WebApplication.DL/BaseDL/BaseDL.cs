using Dapper;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo.WebApplication.Common.Attibutes.Attributes;

namespace Demo.WebApplication.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {

        #region Method
        /// <summary>
        /// Kết nối tới database
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConnection()
        {
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            mySqlConnection.Open();
            return mySqlConnection;
        }

        /// <summary>
        /// Lấy tất cả các bản ghi
        /// author:VietDV(27/3/2023)
        /// </summary>
        /// <returns>Danh sách toàn bộ các bản ghi</returns>
        public ServiceResult GetAllRecords() 
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_GetAll";

            var dbConnection = GetOpenConnection();

            //thực hiện câu lệnh sql
            //var trans = dbConnection.BeginTransaction();
            try
            {

                var response = dbConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();
                if (response != null)
                {
                    return new ServiceResult(true, response);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {
                return new ServiceResult(false, Resource.ServiceResult_Exception);
            }
        }

        /// <summary>
        /// Xoá bản ghi theo id 
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult DeleteRecordById(Guid recordId)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_DeleteById";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add($"v_{typeof(T).Name}Id", recordId);

            var dbConnection = GetOpenConnection();

            //thực hiện câu lệnh sql
            try
            {
                var affectedRow = dbConnection.Execute(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (affectedRow > 0)
                {
                    return new ServiceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new ServiceResult(false, Resource.ServiceResult_Exception);
            }
        }

        /// <summary>
        /// Thêm mới thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>trạng thái khi thực hiện câu lệnh sql</returns>
        public ServiceResult InsertRecord(T record)
        {
            var affectedRow = 0;

            //kết nối tới database
            using (var dbConnection = GetOpenConnection())
            {
                var tran = dbConnection.BeginTransaction();

                Guid recordId = Guid.NewGuid();
                //chuẩn bị tên stored
                String storedProcedureName = $"Proc_{typeof(T).Name}_Insert";

                //chuẩn bị tham số đầu vào
                var paprameters = new DynamicParameters();

                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var primaryKey = (PrimaryKeyAttribute?)property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).FirstOrDefault();
                    var id = (IdAttribute?)property.GetCustomAttributes(typeof(IdAttribute), false).FirstOrDefault();
                    var currentTime = (CurrentTimeAttribute?)property.GetCustomAttributes(typeof(CurrentTimeAttribute), false).FirstOrDefault();

                    if (id != null)
                    {
                        paprameters.Add($"v_{property.Name}", recordId);
                    }
                    if (currentTime != null)
                    {
                        paprameters.Add($"v_{property.Name}", DateTime.Now);
                    }
                    if (currentTime == null && primaryKey == null && id == null)
                    {
                        paprameters.Add($"v_{property.Name}", property.GetValue(record));
                    }
                }


                //thực hiện câu lệnh sql
                try
                {
                    affectedRow = dbConnection.Execute(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure, transaction: tran);

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

                if (affectedRow > 0)
                {
                    return new ServiceResult(true, recordId);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }

        }

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn cập nhật</param>
        /// <param name="record">thông tin cập nhật</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult UpdateRecord(Guid recordId, T record)
        {
            var affectedRow = 0;

            //kết nối tới database
            using (var dbConnection = GetOpenConnection())
            {
                var tran = dbConnection.BeginTransaction();

                //chuẩn bị tên stored
                String storedProcedureName = $"Proc_{typeof(T).Name}_Update";

                //chuẩn bị tham số đầu vào
                var paprameters = new DynamicParameters();

                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var primaryKey = (PrimaryKeyAttribute?)property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).FirstOrDefault();
                    var currentTime = (CurrentTimeAttribute?)property.GetCustomAttributes(typeof(CurrentTimeAttribute), false).FirstOrDefault();

                    if (primaryKey != null)
                    {
                        paprameters.Add($"v_{property.Name}", recordId);
                    }
                    if (currentTime != null)
                    {
                        paprameters.Add($"v_{property.Name}", DateTime.Now);
                    }
                    if (currentTime == null && primaryKey == null)
                    {
                        paprameters.Add($"v_{property.Name}", property.GetValue(record));
                    }
                }

                //thực hiện câu lệnh sql
                try
                {
                    affectedRow = dbConnection.Execute(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure, transaction: tran);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                }
                finally
                {
                    dbConnection.Close();
                }
                if (affectedRow > 0)
                {
                    return new ServiceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
                
        }

        /// <summary>
        /// Kiểm tra id đã xuất hiện trong DB chưa
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="id">id muốn kiểm tra</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public ServiceResult CheckDuplicateID(string id)
        {
            //chuẩn bị tên stored
            String storedProcedureName = "Proc_Employee_CheckExistEmployeeCode";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add("v_Where", id);

            var dbConnection = GetOpenConnection();
            try
            {
                //thực hiện câu lệnh sql
                var response = dbConnection.QueryFirstOrDefault(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (response != null)
                {
                    return new ServiceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new ServiceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new ServiceResult(false, Resource.ServiceResult_Exception);
            }
        }
        #endregion
    }
}
