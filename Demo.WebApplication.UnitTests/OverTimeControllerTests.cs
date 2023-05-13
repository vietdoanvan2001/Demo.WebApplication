using Demo.WebApplication.API.Controllers;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.OverTimeBL;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.OverTimeDL;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.UnitTests
{
    internal class OverTimeControllerTests
    {
        #region GetAllRecord test

        /// <summary>
        /// Lấy thông tin thành công
        /// </summary>
        [Test]
        public void GetAllRecord_ReturnsSuccess()
        {
            //Arrange
            var id1 = new Guid();
            var id2 = new Guid();
            var id3 = new Guid();

            var expectedResult = new ServiceResult(true, new List<OverTime>()
                    {
                        new OverTime
                        {
                            OverTimeId = id1,
                            FullName = "Đoàn Văn Việt",
                            DepartmentName = "Phòng kĩ thuật",
                            PositionName = "DEV",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        },
                        new OverTime
                        {
                            OverTimeId = id2,
                            FullName = "Đoàn Văn Việt 1",
                            DepartmentName = "Phòng kĩ thuật 1",
                            PositionName = "DEV 1",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        },
                        new OverTime
                        {
                            OverTimeId = id3,
                            FullName = "Đoàn Văn Việt 2",
                            DepartmentName = "Phòng kĩ thuật 2",
                            PositionName = "DEV 2",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        }
                    });

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.GetAllRecords().Returns(new ServiceResult(true, new List<OverTime>()
                    {
                        new OverTime
                        {
                            OverTimeId = id1,
                            FullName = "Đoàn Văn Việt",
                            DepartmentName = "Phòng kĩ thuật",
                            PositionName = "DEV",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        },
                        new OverTime
                        {
                            OverTimeId = id2,
                            FullName = "Đoàn Văn Việt 1",
                            DepartmentName = "Phòng kĩ thuật 1",
                            PositionName = "DEV 1",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        },
                        new OverTime
                        {
                            OverTimeId = id3,
                            FullName = "Đoàn Văn Việt 2",
                            DepartmentName = "Phòng kĩ thuật 2",
                            PositionName = "DEV 2",
                            ApplyDate = DateTime.Now,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                        }
                    }));

            var overTimeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.GetAllRecords();

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }

        /// <summary>
        /// Lấy thông tin thất bại, sql không trả về kết quả
        /// </summary>
        [Test]
        public void GetAllRecord_ReturnsNotFound()
        {
            //Arrange

            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.GetAllRecords().Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var employeeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = employeeBL.GetAllRecords();

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }


        #endregion

        #region GetRecordById test

        /// <summary>
        /// Lấy thông tin thành công
        /// </summary>
        [Test]
        public void GetOverTimeById_ExistsEmployee_ReturnsSuccess()
        {
            //Arrange
            var id = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");
            var expectedResult = new ObjectResult(new ServiceResult(true, new OverTime
            {
                OverTimeId = id,
                FullName = "Đoàn Văn Việt 2",
                DepartmentName = "Phòng kĩ thuật 2",
                PositionName = "DEV 2",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            })
            );
            expectedResult.StatusCode = 200;

            var fakeOverTimeBL = Substitute.For<IOverTimeBL>();
            fakeOverTimeBL.GetOverTimeById(
                Arg.Any<Guid>()
                ).Returns(new ServiceResult(true, new OverTime
                {
                    OverTimeId = id,
                    FullName = "Đoàn Văn Việt 2",
                    DepartmentName = "Phòng kĩ thuật 2",
                    PositionName = "DEV 2",
                    ApplyDate = DateTime.Now,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                }));

            var overTimesController = new OverTimesController(fakeOverTimeBL);

            //Act
            var actualResult = (ObjectResult)overTimesController.GetOverTimeById(id);

            //Assert

            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        /// <summary>
        /// Lấy thông tin thất bại, sql không trả về kết quả
        /// </summary>
        [Test]
        public void GetOverTimeById_NotExistsEmployee_ReturnsNotFount()
        {
            //Arrange
            var overTimeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");

            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeEmployeeDL = Substitute.For<IOverTimeDL>();
            fakeEmployeeDL.GetOverTimeById(
                overTimeId
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var overTimeBL = new OverTimeBL(fakeEmployeeDL);

            //Act
            var actualResult = overTimeBL.GetOverTimeById(overTimeId);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }
        #endregion

        #region DeleteRecordById test

        /// <summary>
        /// Xoá thành công
        /// </summary>
        [Test]
        public void DeleteOverTimeById_ExistsOverTime_ReturnsSuccess()
        {
            //Arrange
            var overTimeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");

            var expectedResult = new ServiceResult(true, Resource.ServiceResult_Success);

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.DeleteRecordById(
                overTimeId
                ).Returns(new ServiceResult(true, Resource.ServiceResult_Success));

            var overTimeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.DeleteRecordById(overTimeId);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Xoá thất bại, sql trả về null
        /// </summary>
        [Test]
        public void DeleteEmployeeId_ExistsEmployee_ReturnsFail()
        {
            //Arrange
            var overTimeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");


            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.DeleteRecordById(
                overTimeId
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var overTimeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.DeleteRecordById(overTimeId);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        #endregion

        #region MultipleDelete test
        /// <summary>
        /// Xoá nhiều thành công
        /// </summary>
        [Test]
        public void MultipleDeleteOverTime_ReturnsSuccess()
        {
            //Arrange
            var IDs = "('46a8617e-ccaf-11ed-9dd9-7c50798155a5','37e9723c-ccaf-11ed-9dd9-7c50798155a5')";


            var expectedResult = new ServiceResult(true, Resource.ServiceResult_Success);

            var fakeOverTimeDL = Substitute.For<IOverTimeDL>();
            fakeOverTimeDL.MultipleDelete(
                IDs
                ).Returns(new ServiceResult(true, Resource.ServiceResult_Success));

            var overTimeBL = new OverTimeBL(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.MultipleDelete(IDs);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Xoá nhiều thất bại
        /// </summary>
        [Test]
        public void MultipleDeleteOverTime_ReturnsFail()
        {
            //Arrange
            var IDs = "('46a8617e-ccaf-11ed-9dd9-7c50798155a5','37e9723c-ccaf-11ed-9dd9-7c50798155a5')";


            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IOverTimeDL>();
            fakeOverTimeDL.MultipleDelete(
                IDs
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var overTimeBL = new OverTimeBL(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.MultipleDelete(IDs);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        #endregion

        #region ChangeStatus test
        /// <summary>
        /// Xoá nhiều thành công
        /// </summary>
        [Test]
        public void ChangeStatus_ReturnsSuccess()
        {
            //Arrange
            var IDs = "('46a8617e-ccaf-11ed-9dd9-7c50798155a5','37e9723c-ccaf-11ed-9dd9-7c50798155a5')";
            var status = 1;


            var expectedResult = new ServiceResult(true, Resource.ServiceResult_Success);

            var fakeOverTimeDL = Substitute.For<IOverTimeDL>();
            fakeOverTimeDL.ChangeStatus(
                IDs,status
                ).Returns(new ServiceResult(true, Resource.ServiceResult_Success));

            var overTimeBL = new OverTimeBL(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.ChangeStatus(IDs,status);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Xoá nhiều thất bại
        /// </summary>
        [Test]
        public void ChangeStatus_ReturnsFail()
        {
            //Arrange
            var IDs = "('46a8617e-ccaf-11ed-9dd9-7c50798155a5','37e9723c-ccaf-11ed-9dd9-7c50798155a5')";
            var status = 1;


            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IOverTimeDL>();
            fakeOverTimeDL.ChangeStatus(
                IDs,status
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var overTimeBL = new OverTimeBL(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.ChangeStatus(IDs, status);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        #endregion

        #region GetPaging test
        /// <summary>
        /// Xoá nhiều thành công
        /// </summary>
        [Test]
        public void GetPaging_ReturnsSuccess()
        {
            //Arrange
            var keyword = "việt";
            var MISACode = "1-1-1";
            var status = 1;
            var pageSize = 10;
            var offSet = 0;

            var overTimes = new List<OverTime>();
            overTimes.Add(new OverTime
            {
                OverTimeId = new Guid(),
                FullName = "Đoàn Văn Việt",
                DepartmentName = "Phòng kĩ thuật",
                PositionName = "DEV",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            });
            overTimes.Add(new OverTime
            {
                OverTimeId = new Guid(),
                FullName = "Đoàn Văn Việt 1",
                DepartmentName = "Phòng kĩ thuật 1",
                PositionName = "DEV 1",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            });
            overTimes.Add(new OverTime
            {
                OverTimeId = new Guid(),
                FullName = "Đoàn Văn Việt 2",
                DepartmentName = "Phòng kĩ thuật 2",
                PositionName = "DEV 2",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            });

            var result = new Dictionary<string, object>{
                    { "PageData", overTimes},
                    { "Total", 100 },
                    };
            var begin = 0;
            var end = 100;

            var expectedResult = new OverTimeFilterResult(overTimes, 100, begin, end);

            var fakeOverTimeDL = Substitute.For<IOverTimeDL>();
            fakeOverTimeDL.GetPaging(
                keyword,MISACode, status,pageSize,offSet
                ).Returns(result);

            var overTimeBL = new OverTimeBL(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.GetPaging(keyword, MISACode, status, pageSize, offSet);

            //Assert

            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }


        #endregion

        #region InsertRecord test

        /// <summary>
        /// Thêm mới thất bại
        /// </summary>
        [Test]
        public void InsertOverTime_ValidRecord_ReturnsFalse()
        {
            //Arrange
            var overTime = new OverTime
            {
                OverTimeId = new Guid(),
                FullName = "Đoàn Văn Việt 2",
                DepartmentName = "Phòng kĩ thuật 2",
                PositionName = "DEV 2",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };

            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.InsertRecord(
                overTime
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var overTimeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = overTimeBL.InsertRecord(overTime);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }
        #endregion

        #region UpdateRecord test

        /// <summary>
        /// Sửa thông tin thất bại
        /// </summary>
        [Test]
        public void UpdateEmployee_ValidRecord_ReturnsFail()
        {
            //Arrange
            var id = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");
            var overTime = new OverTime
            {
                FullName = "Đoàn Văn Việt 2",
                DepartmentName = "Phòng kĩ thuật 2",
                PositionName = "DEV 2",
                ApplyDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };

            var expectedResult = new ServiceResult(false, Resource.ServiceResult_Fail);

            var fakeOverTimeDL = Substitute.For<IBaseDL<OverTime>>();
            fakeOverTimeDL.UpdateRecord(
                id,
                overTime
                ).Returns(new ServiceResult(false, Resource.ServiceResult_Fail));

            var employeeBL = new BaseBL<OverTime>(fakeOverTimeDL);

            //Act
            var actualResult = employeeBL.UpdateRecord(id, overTime);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }
        #endregion
    }
}
