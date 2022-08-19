using Newtonsoft.Json.Linq;

namespace Carpentry.Logic.Scryfall.Models;

public class SetApiResult
{
    public SetApiResult()
    {
        CardTokens = new List<JToken>();
    }

    public JToken SetToken { get; set; }
    public List<JToken> CardTokens { get; set; }
}