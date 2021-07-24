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

        //public void RefreshFromToken()
        //{
        //    //JObject parsedProps = tokenProps.ToObject<JObject>();
        //    JObject parsedProps = _cardToken.ToObject<JObject>();

        //    try
        //    {
        //        //var collNum = TryParseToken<string>(parsedProps, "collector_number", null);
        //        //if(collNum == "12")
        //        //{

        //        //}

        //        //var cardLayout = parsedProps.Value<string>("layout");



        //        //if (cardLayout == "transform" || cardLayout == "modal_dfc")
        //        //{
        //        //    var normalFace = parsedProps.SelectToken("card_faces")[0];
        //        //    //Variants["normal"] = normalFace.SelectToken("image_uris.normal").ToObject<string>();
        //        //    ImageUrl = normalFace.SelectToken("image_uris.normal").ToObject<string>();
        //        //}
        //        ////else if (cardLayout == "modal_dfc")
        //        ////{
        //        ////    int breakpoint = 1;
        //        ////}
        //        //else
        //        //{
        //        //    //Variants["normal"] = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
        //        //    ImageUrl = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
        //        //}

        //        //Price = (decimal?)parsedProps.SelectToken("prices.usd");
        //        //PriceFoil = parsedProps.SelectToken("prices.usd_foil").ToObject<decimal?>();
        //        //PriceTix = parsedProps.SelectToken("prices.tix").ToObject<decimal?>();


        //        //Prices["normal"] = price;
        //        //Prices["normal_foil"] = priceFoil;


        //        ////need the string list of legalities
        //        //var legalitiesObj = parsedProps.SelectToken("legalities");

        //        //var legalitiesDictionary = legalitiesObj.ToObject<Dictionary<string, string>>();

        //        //var legalFormatList = legalitiesDictionary.Where(x => x.Value == "legal").Select(x => x.Key).ToList();

        //        //Legalities = legalFormatList;

        //        #region try/catch straight assignments that I should probably refactor

        //        //string collectorNumberStr = TryParseToken<string>("collector_number");

        //        //if (collectorNumberStr != null)
        //        //{

        //        //    if (collectorNumberStr.EndsWith('★'))
        //        //    {

        //        //        var trimmedNumber = collectorNumberStr.Trim('★');
        //        //        CollectionNumber = int.Parse(trimmedNumber);
        //        //        IsPremium = true;

        //        //    }
        //        //    else if (collectorNumberStr.EndsWith('a'))
        //        //    {
        //        //        //main-face of a double-faced card.  Trim the A and add to Cards
        //        //        var trimmedNumber = collectorNumberStr.Trim('a');
        //        //        CollectionNumber = int.Parse(trimmedNumber);
        //        //    }
        //        //    else if (collectorNumberStr.EndsWith('b'))
        //        //    {
        //        //        //main-face of a double-faced card.  Trim the A and add to Cards
        //        //        var trimmedNumber = collectorNumberStr.Trim('b');
        //        //        CollectionNumber = int.Parse(trimmedNumber);

        //        //        //Technically it's the back of a card, not premium.  But I'm not tracking those in the DB anyways
        //        //        IsPremium = true;
        //        //    }
        //        //    else if (collectorNumberStr.EndsWith('e'))
        //        //    {
        //        //        DoNotAdd = true;
        //        //        //throw new NotImplementedException("I don't know what this means yet");

        //        //        //This means many things.
        //        //        //In Strixhaven Mystical Archives, this means 'etched'


        //        //        //I don't know what this represents yet
        //        //        ////main-face of a double-faced card.  Trim the A and add to Cards
        //        //        //var trimmedNumber = collectorNumberStr.Trim('e');
        //        //        //CollectionNumber = int.Parse(trimmedNumber);

        //        //        ////Technically it's the back of a card, not premium.  But I'm not tracking those in the DB anyways
        //        //        //IsPremium = true; 
        //        //    }
        //        //    else
        //        //    {
        //        //        CollectionNumber = int.Parse(collectorNumberStr);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    CollectionNumber = 0; //TODO - Should this default to NULL instead?
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    #endregion
        //}

        //public string Serialize()
        //{
        //    return JsonConvert.SerializeObject(this, Formatting.None);
        //}

        //public MagicCardDto ToMagicCard()
        //{
        //    MagicCardDto result = new MagicCardDto()
        //    {
        //        Cmc = Cmc,
        //        ColorIdentity = ColorIdentity,
        //        Colors = Colors,
        //        Legalities = Legalities,
        //        ManaCost = ManaCost,
        //        MultiverseId = MultiverseId,
        //        Name = Name,
        //        //Prices = Prices,
        //        Rarity = Rarity,
        //        Set = Set,
        //        Text = Text,
        //        Type = Type,
        //        //Variants = Variants,
        //        CollectionNumber = CollectionNumber,
        //        ImageUrl = ImageUrl,
        //        Price = Price,
        //        PriceFoil = PriceFoil,
        //        PriceTix = PriceTix,
        //    };
        //    return result;
        //}

        public int? Cmc => _cardToken.SelectToken("cmc").ToObject<int?>();

        public List<string> ColorIdentity => _cardToken.SelectToken("color_identity").ToObject<List<string>>();

        public List<string> Colors => _cardToken.SelectToken("colors").ToObject<List<string>>();

        public string ManaCost => _cardToken.SelectToken("mana_cost").ToObject<string>();

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

        public string ImageUrl
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