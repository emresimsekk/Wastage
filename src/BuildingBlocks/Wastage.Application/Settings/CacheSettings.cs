namespace Wastage.Application.Settings;

public class CacheSettings
{
    public int SlidingExpiration { get; set; }
    public string Connection { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
}
