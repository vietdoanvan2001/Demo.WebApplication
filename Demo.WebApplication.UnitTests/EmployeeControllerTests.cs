using Demo.WebApplication.API.Controllers;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.Common;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.EmployeeDL;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.UnitTests
{
    internal class EmployeeControllerTests
    {
        #region GetAllRecord test

        /// <summary>
        /// Lấy thông tin thành công
        /// </summary>
        [Test]
        public void GetAllRecord_ReturnsSuccess()
        {
            //Arrange

            var expectedResult = new serviceResult(true, new List<Department>() 
            {
                new Department
                {
                    DepartmentId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5"),
                    DepartmentName = "Phòng kĩ thuật",
                    DepartmentCode = "VP01"
                },
                new Department
                {
                    DepartmentId = new Guid("32a8617e-ccaf-11ed-9dd9-7c50798155a5"),
                    DepartmentName = "Phòng nhân sự",
                    DepartmentCode = "VP02"
                },
                new Department
                {
                    DepartmentId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a9"),
                    DepartmentName = "Phòng điều hành",
                    DepartmentCode = "VP03"
                }
            });

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.GetAllRecords().Returns(new serviceResult(true, new List<Department>()
            {
                new Department
                {
                    DepartmentId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5"),
                    DepartmentName = "Phòng kĩ thuật",
                    DepartmentCode = "VP01"
                },
                new Department
                {
                    DepartmentId = new Guid("32a8617e-ccaf-11ed-9dd9-7c50798155a5"),
                    DepartmentName = "Phòng nhân sự",
                    DepartmentCode = "VP02"
                },
                new Department
                {
                    DepartmentId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a9"),
                    DepartmentName = "Phòng điều hành",
                    DepartmentCode = "VP03"
                }
            }));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.GetAllRecords();

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

            var expectedResult = new serviceResult(false, Resource.ServiceResult_Fail);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.GetAllRecords().Returns(new serviceResult(false, Resource.ServiceResult_Fail));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

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
        public void GetEmployeeId_ExistsEmployee_ReturnsSuccess()
        {
            //Arrange
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");
            var expectedResult = new ObjectResult(new serviceResult(true, new Employee
            {
                EmployeeId = employeeId,
                EmployeeCode = "NV-001",
                FullName = "Doan Van Viet"
            })
            );
            expectedResult.StatusCode = 200;
            var fakeEmployeeBL = Substitute.For<IEmployeeBL>();
            fakeEmployeeBL.GetRecordById(
                Arg.Any<Guid>()
                ).Returns(new serviceResult(true, new Employee
                {
                    EmployeeId = employeeId,
                    EmployeeCode = "NV-001",
                    FullName = "Doan Van Viet"
                }));

            var employeeController = new EmployeesController(fakeEmployeeBL);

            //Act
            var actualResult = (ObjectResult)employeeController.GetRecordById(employeeId);

            //Assert

            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        /// <summary>
        /// Lấy thông tin thất bại, sql không trả về kết quả
        /// </summary>
        [Test]
        public void GetEmployeeId_NotExistsEmployee_ReturnsNotFount()
        {
            //Arrange
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");

            var expectedResult = new serviceResult(false, Resource.ServiceResult_Fail);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.GetRecordById(
                employeeId
                ).Returns(new serviceResult(false, Resource.ServiceResult_Fail));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.GetRecordById(employeeId);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }
        
        #endregion

        #region InsertRecord test
        
        /// <summary>
        /// Thêm mới thành công
        /// </summary>
        [Test]
        public void InsertEmployee_ValidRecord_ReturnsSuccess()
        {
            //Arrange
            var employee = new Employee
            {
                EmployeeCode = "NV-001",
                FullName = "Doan Van Viet",
                Gender = Gender.Male,
                PositionName = "Giám đốc",
            };

            var expectedResult = new serviceResult(true, Resource.ServiceResult_Success);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.InsertRecord(
                employee
                ).Returns(new serviceResult(true, Resource.ServiceResult_Success));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.InsertRecord(employee);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Thêm mới thất bài, lỗi Mã không đúng định dạng
        /// </summary>
        [Test]
        public void InsertEmployee_InValidRecord_ReturnsFail()
        {
            //Arrange
            var employee = new Employee
            {
                EmployeeCode = "ABC",
                FullName = "Doan Van Viet",
                Gender = Gender.Male,
                PositionName = "Giám đốc",
            };

            var expectedResult = new serviceResult(false, Resource.WrongCodeFormat);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.InsertRecord(
                employee
                ).Returns(new serviceResult(false, Resource.WrongCodeFormat));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.InsertRecord(employee);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        #endregion

        #region UpdateRecord test

        /// <summary>
        /// Sửa thông tin thành công
        /// </summary>
        [Test]
        public void UpdateEmployee_ValidRecord_ReturnsSuccess()
        {
            //Arrange
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");
            var employee = new Employee
            {
                EmployeeCode = "NV-001",
                FullName = "Doan Van Viet",
                Gender = Gender.Male,
                PositionName = "Giám đốc",
            };

            var expectedResult = new serviceResult(true, Resource.ServiceResult_Success);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.UpdateRecord(
                employeeId,
                employee
                ).Returns(new serviceResult(true, Resource.ServiceResult_Success));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.UpdateRecord(employeeId,employee);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Sửa thông tin thất bại, lỗi Mã không đúng định dạng
        /// </summary>
        [Test]
        public void UpdateEmployee_InValidRecord_ReturnsFail()
        {
            //Arrange
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");
            var employee = new Employee
            {
                EmployeeCode = "ABC",
                FullName = "Doan Van Viet",
                Gender = Gender.Male,
                PositionName = "Giám đốc",
            };

            var expectedResult = new serviceResult(false, Resource.WrongCodeFormat);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.UpdateRecord(
                employeeId,
                employee
                ).Returns(new serviceResult(false, Resource.WrongCodeFormat));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.UpdateRecord(employeeId,employee);

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
        public void DeleteEmployeeId_ExistsEmployee_ReturnsSuccess()
        {
            //Arrange
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");

            var expectedResult = new serviceResult(true, Resource.ServiceResult_Success);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.DeleteRecordById(
                employeeId
                ).Returns(new serviceResult(true, Resource.ServiceResult_Success));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.DeleteRecordById(employeeId);

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
            var employeeId = new Guid("46a8617e-ccaf-11ed-9dd9-7c50798155a5");


            var expectedResult = new serviceResult(false, Resource.WrongCodeFormat);

            var fakeEmployeeDL = Substitute.For<IBaseDL<Employee>>();
            fakeEmployeeDL.DeleteRecordById(
                employeeId
                ).Returns(new serviceResult(false, Resource.WrongCodeFormat));

            var employeeBL = new BaseBL<Employee>(fakeEmployeeDL);

            //Act
            var actualResult = employeeBL.DeleteRecordById(employeeId);

            //Assert

            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        #endregion
    }
}
