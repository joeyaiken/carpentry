using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpentry.Logic.Models.Scryfall
{
    //This class is currently a hot mess

    //This represents a card object retrieved from Scryfall
    //The original approach was to parse all relevant fields when the record was being created
    //Instead, this should hold the parsed token representing the card object (NOT the raw string, that's for the DB)
    public class ScryfallMagicCard
    {
        private readonly JToken _cardToken;

        public ScryfallMagicCard(JToken token)
        {
            _cardToken = token;
        }

        /// <summary>
        /// Attempts to read a property of the card token.
        /// If that property is null, it assumes a multi-faced card, and looks for the same property on the first card face
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        private T GetFaceValue<T>(string token)
        {
            var property = _cardToken[token];
            if (property != null) return property.ToObject<T>();
            var cardFaces = _cardToken.SelectToken("card_faces") as JArray;
            return cardFaces[0].SelectToken(token).ToObject<T>();
        }

        public int? Cmc => _cardToken.SelectToken("cmc").ToObject<int?>();

        public List<string> ColorIdentity => _cardToken.SelectToken("color_identity").ToObject<List<string>>();

        public List<string> Colors => GetFaceValue<string[]>("colors").ToList();

        public string ManaCost => GetFaceValue<string>("mana_cost");
        
        public int? MultiverseId
        {
            get
            {
                var ids = _cardToken.SelectToken("multiverse_ids").ToObject<int[]>();
                if (ids == null || ids.Length == 0) return null;
                return ids[0];
            }
        }

        public string Name => _cardToken.SelectToken("name").ToObject<string>();

        public List<string> Legalities
        {
            get
            {
                var legalitiesObj = _cardToken.SelectToken("legalities");
                var legalitiesDictionary = legalitiesObj.ToObject<Dictionary<string, string>>();
                return legalitiesDictionary.Where(x => x.Value == "legal").Select(x => x.Key).ToList();
            }
        }

        public string Rarity => _cardToken.SelectToken("rarity").ToObject<string>();

        public string Set => _cardToken.SelectToken("set").ToObject<string>();

        public string Text 
        { 
            get 
            {
                var cardFaceTokens =_cardToken["card_faces"];

                if(cardFaceTokens == null)
                {
                    return _cardToken["oracle_text"].Value<string>();
                }
                else if(cardFaceTokens is JArray)
                {
                    return string.Join(" ", cardFaceTokens.Select(t => t["oracle_text"].Value<string>()));
                }
                else
                {
                    throw new Exception("Unsure of how to parse oracle text");
                }
            } 
        }

        public string Type => _cardToken.SelectToken("type_line").ToObject<string>();

        public int CollectionNumber
        {
            get
            {
                if (int.TryParse(CollectionNumberStr, out var parsedInt))
                {
                    return parsedInt;
                }
                else
                {
                    var substring = CollectionNumberStr[0..^1];
                    return int.Parse(substring);
                }
            }
        }

        public char? CollectionNumberSuffix
        {
            get
            {
                if (int.TryParse(CollectionNumberStr, out var parsedInt))
                {
                    return null;
                }
                else
                {
                    return CollectionNumberStr.Last();
                }
            }
        }

        public string CollectionNumberStr => _cardToken.SelectToken("collector_number").ToObject<string>();

        public decimal? Price => _cardToken.SelectToken("prices.usd").ToObject<decimal?>();

        public decimal? PriceFoil => _cardToken.SelectToken("prices.usd_foil").ToObject<decimal?>();

        public decimal? PriceTix => _cardToken.SelectToken("prices.tix").ToObject<decimal?>();

        public string ImageUrl// => GetFaceValue<string>("image_uris.normal");
        {
            get
            {
                var cardLayout = _cardToken.Value<string>("layout");

                if (cardLayout == "transform" || cardLayout == "modal_dfc")
                {
                    var normalFace = _cardToken.SelectToken("card_faces")[0];
                    return normalFace.SelectToken("image_uris.normal").ToObject<string>();
                }
                else
                {
                    return _cardToken.SelectToken("image_uris.normal").ToObject<string>();
                }
            }
        }

    }

}