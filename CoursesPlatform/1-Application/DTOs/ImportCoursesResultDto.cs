namespace _1_Application.DTOs;

public class ImportCoursesResultDto
{
    public int TotalRows { get; set; }
    public int Imported { get; set; }
    public int Skipped { get; set; }
}