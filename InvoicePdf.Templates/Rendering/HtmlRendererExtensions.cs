using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace InvoicePdf.Templates.Rendering;

public static class HtmlRendererExtensions
{
    public static Task<string> RenderComponent<T>(this HtmlRenderer renderer) where T : IComponent
        => RenderComponent<T>(renderer, ParameterView.Empty);

    public static Task<string> RenderComponent<T>(this HtmlRenderer renderer, Dictionary<string, object?> dictionary) where T : IComponent
        => RenderComponent<T>(renderer, ParameterView.FromDictionary(dictionary));

    private static Task<string> RenderComponent<T>(this HtmlRenderer renderer, ParameterView parameters) where T : IComponent
    {
        return renderer.Dispatcher.InvokeAsync(async () =>
        {
            var result = await renderer.RenderComponentAsync<T>(parameters);
            return result.ToHtmlString();
        });
    }
}
