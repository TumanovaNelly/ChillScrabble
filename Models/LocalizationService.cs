using Microsoft.Extensions.Localization;
using System;

public class LocalizationService
{
    private readonly IStringLocalizer<LocalizationService> _localizer;

    public LocalizationService(IStringLocalizer<LocalizationService> localizer)
    {
        _localizer = localizer;
    }

    public string Get(string key)
    {
        return _localizer[key];
    }
}