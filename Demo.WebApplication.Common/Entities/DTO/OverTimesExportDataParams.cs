using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class OverTimesExportDataParams
    {
        /// <summary>
        /// Từ khoá tìm kiếm
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string MISACode { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int total { get; set; }
    }
}
