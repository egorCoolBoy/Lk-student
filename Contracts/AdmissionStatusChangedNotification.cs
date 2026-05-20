namespace Contracts;

public class AdmissionStatusChangedNotification
{
    public string ApplicantEmail { get; set; }
    public string ApplicantName { get; set; }
    public string ProgramName { get; set; }
    public string Status { get; set; }
}