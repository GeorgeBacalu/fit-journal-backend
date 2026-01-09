using System.Text.Json.Nodes;

namespace FitnessTracker.Api.Helpers;

public static class JsonHelper
{
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
                if (key.Contains("password", StringComparison.OrdinalIgnoreCase))
                    jsonObject[key] = "HIDDEN";
                else if (value != null)
                    HideNode(value);
        }
        else if (node is JsonArray jsonArray)
            foreach (var value in jsonArray)
                if (value != null) HideNode(value);
    }
}
