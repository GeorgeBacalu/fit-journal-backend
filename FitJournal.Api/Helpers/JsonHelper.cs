using System.Text.Json.Nodes;

namespace FitJournal.Api.Helpers;

public static class JsonHelper
{
    private static readonly string[] SensitiveFieldFragments =
    [
        "password", "token", "secret", "authorization", "cookie", "apikey", "api-key"
    ];

    public static string RemoveSensitiveFields(string body)
    {
        try
        {
            var node = JsonNode.Parse(body);
            if (node == null) return body;

            HideNode(node);
            return node.ToJsonString();
        }
        catch
        {
            return body;
        }
    }

    private static void HideNode(JsonNode node)
    {
        if (node is JsonObject jsonObject)
        {
            foreach (var (key, value) in jsonObject.ToList())
                if (SensitiveFieldFragments.Any(fragment =>
                        key.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
                    jsonObject[key] = "HIDDEN";
                else if (value != null)
                    HideNode(value);
        }
        else if (node is JsonArray jsonArray)
            foreach (var value in jsonArray)
                if (value != null) HideNode(value);
    }
}
