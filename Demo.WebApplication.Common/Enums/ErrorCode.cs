namespace Demo.WebApplication.Common.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Ngoại lệ
        /// </summary>
        Exception = 1,

        /// <summary>
        /// dữ liệu bắt buộc nhập
        /// </summary>
        InvalidData = 2,

        /// <summary>
        /// trùng mã
        /// </summary>
        DuplicateCode = 3,

        /// <summary>
        /// sai định dạng mã
        /// </summary>
        WrongFormatCode = 4,

        /// <summary>
        /// sai định dạng Email
        /// </summary>
        WrongFormatEmail = 5,

        /// <summary>
        /// Sai định dạng chuỗi chỉ chứa số
        /// </summary>
        WrongFormatOnlyNumber = 6,

        /// <summary>
        /// SQL trả về dữ liệu rỗng
        /// </summary>
        SqlReturnNull = 7,

        /// <summary>
        /// SQL bắt exception
        /// </summary>
        SqlCatchException = 8,

        /// <summary>
        /// trùng đơn
        /// </summary>
        DuplicateOverTime = 9,
    }
}
