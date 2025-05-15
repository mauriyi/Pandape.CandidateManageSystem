using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Tests.Entities
{
    public class CandidateExperienceTests
    {
        [Fact]
        public void CandidateExperience_WithValidData_IsValid()
        {
            // Arrange
            var experience = new CandidateExperience
            {
                IdCandidate = 1,
                Company = "TechCorp",
                Job = "Software Developer",
                Description = "Desarrolló múltiples soluciones en .NET.",
                Salary = 4500.50m,
                BeginDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2022, 1, 1),
                InsertDate = DateTime.UtcNow,
                ModifyDate = null,
                Candidate = new Candidate
                {
                    Name = "Usuario",
                    Surname = "Ejemplo",
                    Birthdate = new DateTime(1990, 1, 1),
                    Email = "test@example.com",
                    InsertDate = DateTime.UtcNow
                }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(experience, new ValidationContext(experience), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void CandidateExperience_WithTooLongDescription_IsInvalid()
        {
            // Arrange
            var experience = new CandidateExperience
            {
                IdCandidate = 1,
                Company = "Empresa",
                Job = "Desarrollador",
                Description = new string('x', 4001), // 1 char más del máximo permitido
                Salary = 1000.00m,
                BeginDate = DateTime.UtcNow.AddYears(-1),
                EndDate = DateTime.UtcNow,
                InsertDate = DateTime.UtcNow,
                Candidate = new Candidate
                {
                    Name = "Usuario",
                    Surname = "Ejemplo",
                    Birthdate = new DateTime(1990, 1, 1),
                    Email = "test@example.com",
                    InsertDate = DateTime.UtcNow
                }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(experience, new ValidationContext(experience), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains(nameof(CandidateExperience.Description)));
        }

        [Fact]
        public void CandidateExperience_EndDate_CanBeNull()
        {
            // Arrange
            var experience = new CandidateExperience
            {
                IdCandidate = 1,
                Company = "TechCorp",
                Job = "Software Developer",
                Description = "Trabajo actual.",
                Salary = 5000,
                BeginDate = new DateTime(2023, 1, 1),
                EndDate = null,
                InsertDate = DateTime.UtcNow,
                Candidate = new Candidate
                {
                    Name = "Usuario",
                    Surname = "Ejemplo",
                    Birthdate = new DateTime(1990, 1, 1),
                    Email = "test@example.com",
                    InsertDate = DateTime.UtcNow
                }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(experience, new ValidationContext(experience), validationResults, true);

            // Assert
            Assert.True(isValid);
        }
    }
}
