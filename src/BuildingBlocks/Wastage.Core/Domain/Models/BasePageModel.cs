namespace Wastage.Core.Domain.Models;

public abstract class BasePageModel
{
    public int Size { get; set; }
    public int Index { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
