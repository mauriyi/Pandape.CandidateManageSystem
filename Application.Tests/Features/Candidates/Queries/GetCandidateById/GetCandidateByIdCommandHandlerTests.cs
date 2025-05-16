using Application.Common.DTOs;
using Application.Features.Candidates.Queries.GetCandidateById;
using Application.Features.Interfaces;
using Domain.Entities;
using Moq;

namespace Application.Tests.Features.Candidates.Queries.GetCandidateById
{
    public class GetCandidateByIdCommandHandlerTests
    {
        private readonly Mock<ICandidateRepository> _mockRepo;
        private readonly GetCandidateByIdCommandHandler _handler;

        public GetCandidateByIdCommandHandlerTests()
        {
            _mockRepo = new Mock<ICandidateRepository>();
            _handler = new GetCandidateByIdCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCandidateDto_WhenCandidateExists()
        {
            // Arrange
            var candidateId = 1;
            var candidate = new Candidate
            {
                IdCandidate = candidateId,
                Name = "Ana",
                Surname = "Gomez",
                Birthdate = new DateTime(1985, 5, 20),
                Email = "ana.gomez@example.com",
                InsertDate = DateTime.UtcNow.AddYears(-3),
                ModifyDate = null,
                Experiences = new List<CandidateExperience>
            {
                new CandidateExperience
                {
                    Company = "Pandapé",
                    Job = "Manager",
                    Description = "Gestión de proyectos",
                    Salary = 8000m,
                    BeginDate = new DateTime(2019, 1, 1),
                    EndDate = null,
                    InsertDate = DateTime.UtcNow.AddYears(-2),
                    ModifyDate = null
                }
            }
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(candidateId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(candidate);

            var command = new GetCandidateByIdCommand(candidateId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(candidateId, result.IdCandidate);
            Assert.Equal("Ana", result.Name);
            Assert.Single(result.Experiences);
            Assert.Equal("Pandapé", result.Experiences[0].Company);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenCandidateDoesNotExist()
        {
            // Arrange
            var candidateId = 999;
            _mockRepo.Setup(repo => repo.GetByIdAsync(candidateId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Candidate)null);

            var command = new GetCandidateByIdCommand(candidateId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
