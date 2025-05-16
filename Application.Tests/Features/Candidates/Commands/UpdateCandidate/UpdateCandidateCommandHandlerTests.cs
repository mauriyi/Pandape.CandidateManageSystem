using Application.Common.DTOs;
using Application.Features.Candidates.Commands.UpdateCandidate;
using Application.Features.Interfaces;
using Domain.Entities;
using Moq;

namespace Application.Test.Features.Candidates.Commands
{
    public class UpdateCandidateCommandHandlerTests
    {
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
        private readonly UpdateCandidateCommandHandler _handler;

        public UpdateCandidateCommandHandlerTests()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _handler = new UpdateCandidateCommandHandler(_candidateRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCandidateDoesNotExist()
        {
            // Arrange
            _candidateRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Candidate?)null);

            var command = new UpdateCandidateCommand (new CandidateDto
            {
                IdCandidate = 1,
                Name = "Test",
                Surname = "User",
                Birthdate = DateTime.UtcNow.AddYears(-30),
                Email = "test@example.com",
                InsertDate = DateTime.UtcNow,
                Experiences = new List<CandidateExperienceDto>()
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _candidateRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCandidateAndReturnDto_WhenCandidateExists()
        {
            // Arrange
            var existingCandidate = new Candidate
            {
                IdCandidate = 1,
                Name = "OldName",
                Surname = "OldSurname",
                Birthdate = DateTime.UtcNow.AddYears(-40),
                Email = "old@example.com",
                InsertDate = DateTime.UtcNow.AddYears(-1),
                Experiences = new List<CandidateExperience>
                {
                    new CandidateExperience
                    {
                        Company = "OldCompany",
                        Job = "OldJob",
                        Description = "OldDescription",
                        Salary = 1000,
                        BeginDate = DateTime.UtcNow.AddYears(-2),
                        EndDate = DateTime.UtcNow.AddYears(-1),
                        InsertDate = DateTime.UtcNow.AddYears(-2)
                    }
                }
            };

            _candidateRepositoryMock
                .Setup(r => r.GetByIdAsync(existingCandidate.IdCandidate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCandidate);

            _candidateRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var updatedDto = new CandidateDto
            {
                IdCandidate = 1,
                Name = "NewName",
                Surname = "NewSurname",
                Birthdate = DateTime.UtcNow.AddYears(-30),
                Email = "new@example.com",
                InsertDate = existingCandidate.InsertDate,
                Experiences = new List<CandidateExperienceDto>
                {
                    new CandidateExperienceDto
                    {
                        Company = "NewCompany",
                        Job = "NewJob",
                        Description = "NewDescription",
                        Salary = 2000,
                        BeginDate = DateTime.UtcNow.AddYears(-1),
                        EndDate = null,
                        InsertDate = DateTime.UtcNow.AddYears(-1)
                    }
                }
            };

            var command = new UpdateCandidateCommand(updatedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedDto.Name, result.Name);
            Assert.Equal(updatedDto.Surname, result.Surname);
            Assert.Equal(updatedDto.Email, result.Email);
            Assert.Equal(updatedDto.Experiences.Count, result.Experiences.Count);
            Assert.Equal(updatedDto.Experiences[0].Company, result.Experiences[0].Company);

            _candidateRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
