namespace Contracts;

public class ManagerTookApplicant
{
    public Guid ApplicantId { get; set; }
    public Guid ManagerId { get; set; }
    public string ApplicantName { get; set; }
    public string ManagerName { get; set; }
    public string ProgramName { get; set; }
}