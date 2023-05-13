using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class ExportDataSelectedParams
    {
        /// <summary>
        /// Chuỗi Id
        /// </summary>
        public string listID { get; set; }

        public List<HeaderType> header { get; set; }
    }
}
