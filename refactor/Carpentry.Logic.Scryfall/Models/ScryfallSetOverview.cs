using Newtonsoft.Json.Linq;
using System;

namespace Carpentry.Logic.Scryfall.Models;

//This is used by the 'GetAvailableSets' ScryfallService method
//Represents a scryfall set returned by the API

public class ScryfallSetOverview
{
    // public static ScryfallSetOverview FromJToken(JToken scryfallToken)
    // {
    //     
    // }
    //
    //
    
    private readonly JToken _token;
    public ScryfallSetOverview(JToken scryfallToken)
    {
        _token = scryfallToken;
    }

    //"object": "set",
    //"id": "372dafe8-b5d1-4b81-998f-3ae96b59498a",
    //"code": "2xm",
    public string Code => _token.Value<string>("code");
    //"mtgo_code": "2xm",
    //"arena_code": "2xm",
    //"tcgplayer_id": 2655,
    //"name": "Double Masters",
    public string Name => _token.Value<string>("name");
    //"uri": "https://api.scryfall.com/sets/372dafe8-b5d1-4b81-998f-3ae96b59498a",
    //"scryfall_uri": "https://scryfall.com/sets/2xm",
    //"search_uri": "https://api.scryfall.com/cards/search?order=set&q=e%3A2xm&unique=prints",
    //"released_at": "2020-08-07",
    //public string ReleasedAtString => _token.Value<string>("released_at");
    public DateTime ReleasedAt => DateTime.Parse(_token.Value<string>("released_at"));

    //"set_type": "masters",
    public string SetType => _token.Value<string>("set_type");
    //"card_count": 10,
    public int CardCount => _token.Value<int>("card_count");
    //"digital": false,
    public bool Digital => _token.Value<bool>("digital");
    //"nonfoil_only": false,
    public bool NonfoilOnly => _token.Value<bool>("nonfoil_only");
    //"foil_only": false,
    public bool FoilOnly => _token.Value<bool>("foil_only");
    //"icon_svg_uri": "https://img.scryfall.com/sets/2xm.svg?1593403200"

    private static T TryParseToken<T>(JObject setObject, string tokenPath, T defaultValue)
    {
        try
        {
            var actual = setObject.SelectToken(tokenPath).ToObject<T>();
            return actual;
        }
        catch
        {
            return defaultValue;
        }
    }
}

