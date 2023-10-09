namespace CloudDrive.Utilities;

public class ErrorResponse
{
    public List<string> Errors { get; set; }

    public ErrorResponse(string error)
    {
        Errors = new() { error };
    }

    public ErrorResponse(IDictionary<string, string[]> errors)
    {
        Errors = new();

        foreach (KeyValuePair<string, string[]> pair in errors)
        {
            Errors.AddRange(pair.Value);
        }
    }
}
