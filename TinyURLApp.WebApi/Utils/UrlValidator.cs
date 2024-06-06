using System.Text.RegularExpressions;

namespace TinyURLApp.WebApi.Utils;

public static class UrlValidator
{
    private const int MaxUrlLength = 1000;

    public static bool IsValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return false;

        // Ensure that the URL is within a reasonable length to prevent buffer overflow attacks.
        if (url.Length > MaxUrlLength) return false;

        // Multiple validations
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult)) return false;

        // Ensure the URL scheme is either HTTP or HTTPS
        if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps) return false;

        return true;
    }
}
