namespace Wastage.Core.Domain.Models.Responses;

public class GetListResponse<T> : BasePageModel
{
    private IList<T> _items;
    public IList<T> Items
    {
        get => _items ??= new List<T>();
        set => _items = value;
    }
}
