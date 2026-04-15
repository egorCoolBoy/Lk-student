namespace DirectoryService.Application.DTO;

public class ProgramsResponseDto
{
    public List<ProgramDto> Programs { get; set; } = new();
    public Pagination Pagination { get; set; } = null!;
}