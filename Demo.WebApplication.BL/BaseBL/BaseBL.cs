using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.RegularExpressions;
using static Demo.WebApplication.Common.Attibutes.Attributes;

namespace Demo.WebApplication.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>

    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả các bản ghi
        /// author:VietDV(27/3/2023)
        /// </summary>
        /// <returns>Danh sách toàn bộ các bản ghi</returns>
        public serviceResult GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Xoá bản ghi theo id 
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult DeleteRecordById(Guid recordId)
        {
            return _baseDL.DeleteRecordById(recordId);
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn lấy thông tin</param>
        /// <returns>thông tin bản ghi</returns>
        public serviceResult GetRecordById(Guid recordId)
        {
            return _baseDL.GetRecordById(recordId);
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

            var validateFailures = ValidateRecord(record);


            if (validateFailures.Count > 0)
            {
                return new serviceResult(false, validateFailures);
            }

            var validateFailuresCustom = ValidateRecordCustom(record, false);

            if(validateFailuresCustom.Count > 0)
            {
                return new serviceResult(false, validateFailuresCustom);
            }

            return _baseDL.UpdateRecord(recordId, record);
            
            
        }

        /// <summary>
        /// Thêm mới thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>trạng thái khi thực hiện câu lệnh sql</returns>
        public serviceResult InsertRecord(T record)
        {
            var validatefailures = ValidateRecord(record);

            if (validatefailures.Count > 0)
            {
                return new serviceResult(false, validatefailures);
            }

            var validateFailuresCustom = ValidateRecordCustom(record, true);

            if (validateFailuresCustom.Count > 0)
            {
                return new serviceResult(false, validateFailuresCustom);
            }

            return _baseDL.InsertRecord(record);

        }

        /// <summary>
        /// Validate chung
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">form body thông tin cần validate</param>
        /// <returns>Danh sách lỗi</returns>
        public List<errorResult> ValidateRecord(T record)
        {
            var validateFailures = new List<errorResult>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                var requiredAttribute = (NotEmptyAttribute?)property.GetCustomAttributes(typeof(NotEmptyAttribute), false).FirstOrDefault();

                //Validate các trường bắt buộc
                if (requiredAttribute != null && propertyValue != null && String.IsNullOrEmpty(propertyValue.ToString()))
                {
                    validateFailures.Add( new errorResult {
                            ErrorField = propertyName,
                            ErrorCode = ErrorCode.InvalidData,
                            DevMsg = Resource.Error_InvalidData,
                            UserMsg = Resource.Error_InvalidData,
                        });
                }


            }
            return validateFailures;
            #endregion
        }

        /// <summary>
        /// Cho phép các class khác override để custom validate riêng
        /// </summary>
        /// <param name="record">form body thông tin cần validate</param>
        /// <param name="isInsert">cờ xác định xem có phải API thêm mới không</param>
        /// <returns></returns>
        public virtual List<errorResult> ValidateRecordCustom(T record, bool isInsert)
        {
            return new List<errorResult>();
        }
    }
}
