using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.DL.BaseDL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Kết nối tới database
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConnection();

        /// <summary>
        /// Lấy tất cả các bản ghi
        /// author:VietDV(27/3/2023)
        /// </summary>
        /// <returns>Danh sách toàn bộ các bản ghi</returns>
        public serviceResult GetAllRecords();

        /// <summary>
        /// Lấy thông tin bản ghi theo id
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn lấy thông tin</param>
        /// <returns>thông tin bản ghi</returns>
        public serviceResult GetRecordById(Guid recordId);

        /// <summary>
        /// Thêm mới thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>trạng thái khi thực hiện câu lệnh sql</returns>
        public serviceResult InsertRecord(T record);

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn cập nhật</param>
        /// <param name="record">thông tin cập nhật</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult UpdateRecord(Guid recordId, T record);

        /// <summary>
        /// Xoá bản ghi theo id 
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="recordId">id bản ghi muốn xoá</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult DeleteRecordById(Guid recordId);

        /// <summary>
        /// Kiểm tra id đã xuất hiện trong DB chưa
        /// author: VietDV(27/3/2023)
        /// </summary>
        /// <param name="id">id muốn kiểm tra</param>
        /// <returns>trạng thái thực hiện câu lệnh sql</returns>
        public serviceResult CheckDuplicateID(string id);


    }
}
