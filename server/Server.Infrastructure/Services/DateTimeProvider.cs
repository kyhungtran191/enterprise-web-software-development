using Server.Application.Common.Interfaces.Services;
using System;
using System.Globalization;

namespace Server.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

}