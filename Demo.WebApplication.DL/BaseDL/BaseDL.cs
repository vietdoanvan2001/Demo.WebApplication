﻿using Dapper;
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
        public serviceResult GetAllRecords()
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_GetAll";

            var dbConnection = GetOpenConnection();

            //thực hiện câu lệnh sql
            var trans = dbConnection.BeginTransaction();
            try
            {

                var response = dbConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                trans.Commit();
                

                if(response != null)
                {
                    return new serviceResult(true, response);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                return new serviceResult(false, Resource.ServiceResult_Exception);
            }
            finally
            {
                dbConnection.Close();
            }

        }

        /// <summary>
        /// Xoá bản ghi theo id 
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult DeleteRecordById(Guid recordId)
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
                    return new serviceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn lấy thông tin</param>
        /// <returns>thông tin bản ghi</returns>
        public serviceResult GetRecordById(Guid recordId)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_GetById";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();
            paprameters.Add($"v_{typeof(T).Name}Id", recordId);

            //khởi tạo kết nối tới DB

            var dbConnection = GetOpenConnection();


            //thực hiện câu lệnh sql
            try
            {
                T record = dbConnection.QueryFirstOrDefault<T>(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                dbConnection.Close();

                if (record != null)
                {
                    return new serviceResult(true, record);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }
        }

        /// <summary>
        /// Thêm mới thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>trạng thái khi thực hiện câu lệnh sql</returns>
        public serviceResult InsertRecord(T record)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_Insert";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var primaryKey = (PrimaryKeyAttribute?)property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).FirstOrDefault();
                var currentTime = (CurrentTimeAttribute?)property.GetCustomAttributes(typeof(CurrentTimeAttribute), false).FirstOrDefault();

                if (currentTime != null)
                {
                    paprameters.Add($"v_{property.Name}", DateTime.Now);
                }
                if (currentTime == null && primaryKey == null)
                {
                    paprameters.Add($"v_{property.Name}", property.GetValue(record));
                }
            }

            //Khởi tạo kết nối với DB
            var dbConnection = GetOpenConnection();

            //thực hiện câu lệnh sql
            try
            {
                var affectedRow = dbConnection.Execute(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                if (affectedRow > 0)
                {
                    return new serviceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }

        }

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn cập nhật</param>
        /// <param name="record">thông tin cập nhật</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult UpdateRecord(Guid recordId, T record)
        {
            //chuẩn bị tên stored
            String storedProcedureName = $"Proc_{typeof(T).Name}_Update";

            //chuẩn bị tham số đầu vào
            var paprameters = new DynamicParameters();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var primaryKey = (PrimaryKeyAttribute?)property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).FirstOrDefault();
                var currentTime = (CurrentTimeAttribute?)property.GetCustomAttributes(typeof(CurrentTimeAttribute), false).FirstOrDefault();

                if(primaryKey != null)
                {
                    paprameters.Add($"v_{property.Name}", recordId);
                }
                if(currentTime != null)
                {
                    paprameters.Add($"v_{property.Name}", DateTime.Now);
                }
                if(currentTime == null && primaryKey == null)
                {
                    paprameters.Add($"v_{property.Name}",property.GetValue(record));
                }
            }   


            var dbConnection = GetOpenConnection();

            //thực hiện câu lệnh sql
            try
            {
                var affectedRow = dbConnection.Execute(storedProcedureName, paprameters, commandType: System.Data.CommandType.StoredProcedure);

                if (affectedRow > 0)
                {
                    return new serviceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }
        }

        /// <summary>
        /// Kiểm tra id đã xuất hiện trong DB chưa
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="id">id muốn kiểm tra</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult CheckDuplicateID(string id)
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
                    return new serviceResult(true, Resource.ServiceResult_Success);
                }
                else
                {
                    return new serviceResult(false, Resource.ServiceResult_Fail);
                }
            }
            catch (Exception)
            {

                return new serviceResult(false, Resource.ServiceResult_Exception);
            }
        }
        #endregion
    }
}