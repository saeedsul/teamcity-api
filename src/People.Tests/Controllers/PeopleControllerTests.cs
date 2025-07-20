using Microsoft.AspNetCore.Mvc;
using Moq;
using People.Api.Controllers;
using People.Api.DTOs;
using People.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Tests.Controllers
{
    public class PeopleControllerTests
    {
        [Fact]
        public async Task List_ReturnsAllPeople()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(s => s.GetAllPeopleAsync())
                .ReturnsAsync(new List<PersonDto>
                {
                    new PersonDto { Id = 1, Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) },
                    new PersonDto { Id = 2, Name = "Jane Doe", DateOfBirth = new DateOnly(1992, 2, 2) }
                });
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.List();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var people = Assert.IsAssignableFrom<IEnumerable<PersonDto>>(okResult.Value);
            Assert.Equal(2, people.Count());
        }

        [Fact]
        public async Task GetById_ReturnsPerson_WhenExists()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(s => s.GetPersonByIdAsync(1))
                .ReturnsAsync(new PersonDto { Id = 1, Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) });
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.GetById(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var person = Assert.IsType<PersonDto>(okResult.Value);
            Assert.Equal(1, person.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(s => s.GetPersonByIdAsync(1))
                .ReturnsAsync((PersonDto?)null);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedPerson_WhenValid()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            var newPerson = new PersonDto { Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) };
            var addedPerson = new PersonDto { Id = 1, Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) };
            mockService.Setup(s => s.AddPersonAsync(newPerson))
                .ReturnsAsync(addedPerson);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Add(newPerson);
            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var person = Assert.IsType<PersonDto>(createdResult.Value);
            Assert.Equal(1, person.Id);
        }

        [Fact]
        public async Task Add_ReturnsBadRequest_WhenInvalid()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            var invalidPerson = new PersonDto { Name = "", DateOfBirth = default };
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Add(invalidPerson);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task Update_ReturnsNoContent_WhenPersonExists()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            var updatedPerson = new UpdatePersonDto {  Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) };
            mockService.Setup(s => s.UpdatePersonAsync(1, updatedPerson))
                .ReturnsAsync(true);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Update(1, updatedPerson);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            var updatedPerson = new UpdatePersonDto { Name = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) };
            mockService.Setup(s => s.UpdatePersonAsync(1, updatedPerson))
                .ReturnsAsync(false);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Update(1, updatedPerson);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenPersonExists()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(s => s.DeletePersonAsync(1))
                .ReturnsAsync(true);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Delete(1);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(s => s.DeletePersonAsync(1))
                .ReturnsAsync(false);
            var controller = new PeopleController(mockService.Object);
            // Act
            var result = await controller.Delete(1);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
