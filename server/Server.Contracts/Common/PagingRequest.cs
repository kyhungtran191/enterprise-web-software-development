using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Common;

public class PagingRequest
{    
    [FromQuery]
    public string? Keyword { get; set; }

    [FromQuery]
    public int PageIndex { get; set; } = 1;
    
    [FromQuery]
    public int PageSize { get; set; } = 10;
}