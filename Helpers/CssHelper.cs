using Microsoft.AspNetCore.Mvc.Rendering;


namespace ChillScrabble.Helpers;

public static class CssHelper
{
    public static string RenderControllerSpecificCss(ViewContext viewContext)
    {
        var controllerName = viewContext.RouteData.Values["controller"]?.ToString();

        if (!string.IsNullOrEmpty(controllerName))
            return $"<link rel=\"stylesheet\" href=\"/css/{controllerName}.css\"/>";
        
        return string.Empty;
    }
}