using Application.Features.Candidates.Queries.GetAllCandidates;
using Application.Features.Interfaces;
using Domain.Entities;
using Moq;

namespace Application.Tests.Features.Candidates.Queries.GetAllCandidates
{
    public class GetAllCandidatesCommandHandlerTests
    {
        private readonly Mock<ICandidateRepository> _mockRepo;
        private readonly GetAllCandidatesCommandHandler _handler;

        public GetAllCandidatesCommandHandlerTests()
        {
            _mockRepo = new Mock<ICandidateRepository>();
            _handler = new GetAllCandidatesCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfCandidateDto()
        {
            // Arrange
            var candidates = new List<Candidate>
        {
            new Candidate
            {
                IdCandidate = 1,
                Name = "Juan",
                Surname = "Perez",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "juan.perez@example.com",
                InsertDate = DateTime.UtcNow.AddYears(-1),
                ModifyDate = null,
                Experiences = new List<CandidateExperience>
                {
                    new CandidateExperience
                    {
                        Company = "Pandapé",
                        Job = "Developer",
                        Description = "Desarrollo de software",
                        Salary = 50000000,
                        BeginDate = new DateTime(2018, 1, 1),
                        EndDate = null,
                        InsertDate = DateTime.UtcNow.AddYears(-2),
                        ModifyDate = null
                    }
                }
            }
        };

            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(candidates);

            var command = new GetAllCandidatesCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var list = result.ToList();
            Assert.Single(list);
            Assert.Equal(candidates[0].IdCandidate, list[0].IdCandidate);
            Assert.Equal(candidates[0].Name, list[0].Name);
            Assert.Single(list[0].Experiences);
            Assert.Equal("Pandapé", list[0].Experiences[0].Company);
        }
    }
}