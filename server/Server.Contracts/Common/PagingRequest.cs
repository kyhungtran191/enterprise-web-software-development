using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Common;

public class PagingRequest
{    
    [FromQuery(Name="keyword")]
    public string? Keyword { get; set; }

    [FromQuery(Name = "pageindex")]
    public int PageIndex { get; set; } = 1;
    
    [FromQuery(Name = "pagesize")]
    public int PageSize { get; set; } = 10;
}