using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Attibutes
{
    public class Attributes
    {
        /// <summary>
        /// Attribute đánh dấu trường không được rỗng
        /// </summary>
        public class NotEmptyAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if(value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường không được trùng
        /// </summary>
        public class NotDuplicateAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường có định dạng mã
        /// </summary>
        public class CodeFormatAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường chỉ chứa các chữ số
        /// </summary>
        public class OnlyNumberAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường có định dạng email
        /// </summary>
        public class IsEmailAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường là khoá chính
        /// </summary>
        public class PrimaryKeyAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Attribute đánh dấu trường là thời gian hiện tại
        /// </summary>
        public class CurrentTimeAttribute : ValidationAttribute
        {
            public override bool IsValid(object? value)
            {
                if (value == null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
