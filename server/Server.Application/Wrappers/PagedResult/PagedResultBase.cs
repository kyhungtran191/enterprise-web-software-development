namespace Server.Application.Wrappers.PagedResult;

public abstract class PagedResultBase
{
    protected int _pageCount = default;
    public int CurrentPage { get; set; }

    public int PageCount
    {
        get
        {
            _pageCount = (int)Math.Ceiling((double)RowCount / PageSize);
            return _pageCount;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _pageCount = value;
        }
    }

    public int PageSize { get; set; }
    public int RowCount { get; set; }
    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    public string? AdditionalData { get; set; }
}