using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Tests.Entities
{
    public class CandidateTests
    {
        [Fact]
        public void Candidate_WithValidData_IsValid()
        {
            // Arrange
            var candidate = new Candidate
            {
                Name = "Usuario",
                Surname = "Ejemplo",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "test@example.com",
                InsertDate = DateTime.UtcNow,
                ModifyDate = null
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(candidate, new ValidationContext(candidate), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Candidate_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var candidate = new Candidate
            {
                Name = "Mauricio",
                Surname = "Gómez",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "no-es-un-email",
                InsertDate = DateTime.UtcNow
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(candidate, new ValidationContext(candidate), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage!.Contains("email", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Candidate_Experiences_IsInitialized()
        {
            // Arrange
            var candidate = new Candidate
            {
                Name = "Usuario",
                Surname = "Ejemplo",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "test@example.com",
                InsertDate = DateTime.UtcNow,
                ModifyDate = null
            };

            // Assert
            Assert.NotNull(candidate.Experiences);
            Assert.Empty(candidate.Experiences);
        }
    }
}
