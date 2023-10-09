namespace CloudDrive.Dto;

public class CreateLinkDto
{
    public string ParentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
}
