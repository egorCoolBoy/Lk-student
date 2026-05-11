namespace ProfileDocksService.Applicantion.Dtos;

public class CreatePassportDto
{
    public string Series { get; set; }
    public string Number { get; set; }
    public string IssuedBy { get; set; }
    public string? IssuedDate { get; set; }
}

public class GetPassportDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Series { get; set; }
    public string Number { get; set; }
    public string IssuedBy { get; set; }
    public DateOnly IssuedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpdatePassportDto
{
    public string? Series { get; set; }
    public string? Number { get; set; }
    public string? IssuedBy { get; set; }
    public string? IssuedDate { get; set; }
}