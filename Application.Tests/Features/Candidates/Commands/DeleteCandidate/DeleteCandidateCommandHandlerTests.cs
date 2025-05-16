using Application.Features.Candidates.Commands.DeleteCandidate;
using Application.Features.Interfaces;
using Domain.Entities;
using Moq;

namespace Application.Tests.Features.Candidates.Commands.DeleteCandidate
{
    public class DeleteCandidateCommandHandlerTests
    {
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
        private readonly DeleteCandidateCommandHandler _handler;

        public DeleteCandidateCommandHandlerTests()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _handler = new DeleteCandidateCommandHandler(_candidateRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCandidateDoesNotExist()
        {
            // Arrange
            _candidateRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Candidate?)null);

            var command = new DeleteCandidateCommand(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _candidateRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCandidateAndReturnTrue_WhenCandidateExists()
        {
            // Arrange
            var candidate = new Candidate
            {
                IdCandidate = 1,
                Name = "Juan",
                Surname = "Pérez",
                Email = "juan.perez@example.com",
                Birthdate = new DateTime(1990, 5, 15),
                InsertDate = DateTime.UtcNow,
                Experiences = new List<CandidateExperience>
                {                    
                    new CandidateExperience
                    {
                        IdCandidateExperience = 1,
                        IdCandidate = 1,
                        Company = "Pandapé",
                        Job = "Developer",
                        Description = "Desarrollo de software",
                        Salary = 3000000,
                        BeginDate = new DateTime(2015, 1, 1),
                        EndDate = new DateTime(2019, 12, 31),
                        InsertDate = DateTime.UtcNow
                    }
                }
            };

            _candidateRepositoryMock
                .Setup(r => r.GetByIdAsync(candidate.IdCandidate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _candidateRepositoryMock
                .Setup(r => r.DeleteAsync(candidate, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteCandidateCommand (candidate.IdCandidate);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Empty(candidate.Experiences); // Verifica que las experiencias se limpiaron

            _candidateRepositoryMock.Verify(r => r.GetByIdAsync(candidate.IdCandidate, It.IsAny<CancellationToken>()), Times.Once);
            _candidateRepositoryMock.Verify(r => r.DeleteAsync(candidate, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
