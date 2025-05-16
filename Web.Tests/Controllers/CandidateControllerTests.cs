using Application.Common.DTOs;
using Application.Features.Candidates.Commands.CreateCandidate;
using Application.Features.Candidates.Commands.DeleteCandidate;
using Application.Features.Candidates.Queries.GetAllCandidates;
using Application.Features.Candidates.Queries.GetCandidateById;
using CandidateManagement.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Web.ViewModels.Candidate;

namespace Web.Tests.Controllers
{
    public class CandidateControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CandidateController _controller;

        public CandidateControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CandidateController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithCandidates()
        {
            // Arrange
            var candidates = new List<CandidateDto>
            {
                new CandidateDto
                {
                    IdCandidate = 1,
                    Name = "Test",
                    Surname = "User",
                    Email = "test@example.com",
                    Birthdate = new DateTime(1990, 1, 1),
                    InsertDate = DateTime.UtcNow,
                    Experiences = new List<CandidateExperienceDto>()
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCandidatesCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(candidates);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public void Create_Get_ReturnsViewWithEmptyModel()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<CandidateViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsSameView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var now = DateTime.UtcNow;
            var vm = new CandidateViewModel
            {
                Name = "Laura",
                Surname = "González",
                Birthdate = new DateTime(1992, 7, 20),
                Email = "laura.gonzalez@example.com",
                InsertDate = now,
                ModifyDate = null,
                Experiences = new List<CandidateExperienceViewModel>
                {
                    new CandidateExperienceViewModel
                    {
                        Company = "Pandapé",
                        Job = "QA Engineer",
                        Description = "Testing y automatización",
                        Salary = 4200.50m,
                        BeginDate = new DateTime(2020, 1, 1),
                        EndDate = new DateTime(2022, 1, 1),
                        InsertDate = now,
                        ModifyDate = null
                    }
                }
            };

            // Act
            var result = await _controller.Create(vm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var vm = new CandidateViewModel
            {
                Name = "Test",
                Surname = "User",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "test@example.com",
                InsertDate = DateTime.UtcNow,
                Experiences = new List<CandidateExperienceViewModel>()
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCandidateCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            // Act
            var result = await _controller.Create(vm);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            // Arrange
            var dto = new CandidateDto
            {
                IdCandidate = 1,
                Name = "Test",
                Surname = "User",
                Birthdate = DateTime.Now,
                Email = "test@example.com",
                InsertDate = DateTime.Now,
                Experiences = new List<CandidateExperienceDto>()
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCandidateByIdCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dto);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CandidateViewModel>(viewResult.Model);
            Assert.Equal(dto.Name, model.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFoundIfNull()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCandidateByIdCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((CandidateDto?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Post_ReturnsRedirectToIndexOnSuccess()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCandidateCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Delete_Post_Failure_RedirectsWithErrorMessage()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCandidateCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Delete(99);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.True(_controller.TempData.ContainsKey("ErrorMessage")); 
        }
    }
}
