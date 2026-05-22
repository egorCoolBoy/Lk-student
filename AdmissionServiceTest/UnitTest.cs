using AdmisionsService.Application;
using AdmisionsService.Application.Dtos;
namespace AdmissionServiceTest;

public class CheckEducationLevelTests
{
    [Fact]
    public void ReturnsTrue_WhenLevelMatches()
    {
        var educations = new List<EducationDocxDto>
        {
            new()
            {
                EducationTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            }
        };

        var program = new ProgramDto
        {
            EducationLevel = new EducationLevelDto
            {
                Id = 10
            }
        };

        var documentTypes = new List<EducationDocumentTypesDto>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EducationLevel = new EducationLevelDto { Id = 10 },
                NextEducationLevels = new List<EducationLevelDto>()
            }
        };
        var result = AdmissionService.CheckEducationLevel(educations, program, documentTypes);
        
        Assert.True(result);
    }
    [Fact]
    public void ReturnsFalse_WhenNoMatch()
    {
        
        var educations = new List<EducationDocxDto>
        {
            new()
            {
                EducationTypeId = Guid.NewGuid()
            }
        };

        var program = new ProgramDto
        {
            EducationLevel = new EducationLevelDto { Id = 2 }
        };

        var documentTypes = new List<EducationDocumentTypesDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                EducationLevel = new EducationLevelDto { Id = 1 },
                NextEducationLevels = new List<EducationLevelDto>
                {
                    new() { Id = 2 }
                }
            }
        };

        
        var result = AdmissionService.CheckEducationLevel(educations, program, documentTypes);

        
        Assert.False(result);
    }
    [Fact]
    public void ReturnsFalse_WhenEducationsEmpty()
    {
        
        var educations = new List<EducationDocxDto>();

        var program = new ProgramDto
        {
            EducationLevel = new EducationLevelDto { Id = 10 }
        };

        var documentTypes = new List<EducationDocumentTypesDto>();

        
        var result = AdmissionService.CheckEducationLevel(educations, program, documentTypes);

        
        Assert.False(result);
    }
    [Fact]
    public void ReturnsTrue_WhenNextLevelsMatch()
    {
        var educations = new List<EducationDocxDto>
        {
            new()
            {
                EducationTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            }
        };

        var program = new ProgramDto
        {
            EducationLevel = new EducationLevelDto { Id = 20 }
        };

        var documentTypes = new List<EducationDocumentTypesDto>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EducationLevel = new EducationLevelDto { Id = 10 },
                NextEducationLevels = new List<EducationLevelDto>
                {
                    new() { Id = 20 }
                }
            }
        };

        var result = AdmissionService.CheckEducationLevel(educations, program, documentTypes);

        Assert.True(result);
    }
}