using Application.Common.DTOs;
using Application.Features.Candidates.Commands.CreateCandidate;
using Application.Features.Interfaces;
using Domain.Entities;
using Moq;

namespace Application.Tests.Features.Candidates.Commands
{
    public class CreateCandidateCommandHandlerTests
    {
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
        private readonly CreateCandidateCommandHandler _handler;

        public CreateCandidateCommandHandlerTests()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _handler = new CreateCandidateCommandHandler(_candidateRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCandidateIsNull()
        {
            // Arrange
            var command = new CreateCandidateCommand(null); 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _candidateRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCallAddAsync_AndReturnCandidateId()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var candidateDto = new CandidateDto
            {
                IdCandidate = 0, // no se usa en creación
                Name = "Laura",
                Surname = "González",
                Birthdate = new DateTime(1992, 7, 20),
                Email = "laura.gonzalez@example.com",
                InsertDate = now,
                ModifyDate = null,
                Experiences = new List<CandidateExperienceDto>
                {
                    new CandidateExperienceDto
                    {
                        Company = "Tech Co",
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

            Candidate? createdCandidate = null;

            _candidateRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()))
                .Callback<Candidate, CancellationToken>((c, _) =>
                {
                    c.IdCandidate = 10;
                    createdCandidate = c;
                })
                .Returns(Task.CompletedTask);

            // Update the instantiation of CreateCandidateCommand to use the correct syntax for initializing the required property "Candidate".
            var command = new CreateCandidateCommand(candidateDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(10, result);
            Assert.NotNull(createdCandidate);
            Assert.Equal("Laura", createdCandidate!.Name);
            Assert.Equal("González", createdCandidate.Surname);
            Assert.Equal("laura.gonzalez@example.com", createdCandidate.Email);
            Assert.Single(createdCandidate.Experiences);

            var experience = createdCandidate.Experiences.First();
            Assert.Equal("Tech Co", experience.Company);
            Assert.Equal("QA Engineer", experience.Job);
            Assert.Equal(4200.50m, experience.Salary);
            Assert.Equal("Testing y automatización", experience.Description);

            _candidateRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
