using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpentry.Logic.Models.Scryfall
{
    public class ScryfallMagicCard
    {
        //What if...the ScryfallMagicCard just set the JObject on construction, then Getters parsed properties on-demand
        //The scryfall repo could store the TRUE representation of a card.  Scryfall wouldn't have to be re-queried if I updated fields 
        public ScryfallMagicCard()
        {
            //Prices = new Dictionary<string, decimal?>();
            Legalities = new List<string>();
            //Variants = new Dictionary<string, string>();
        }

        private static T TryParseToken<T>(JObject cardObject,string tokenPath, T defaultValue)
        {
            try
            {

                //if(cardObject.TryGetValue(tokenPath, out var parseResult))
                //{
                //    var mapped = parseResult.ToObject<T>();
                //    int breakpoint = 1;
                //    return mapped;
                //}

                var actual = cardObject.SelectToken(tokenPath).ToObject<T>();
                return actual;
            }
            catch
            {
                return defaultValue;
            }
        }

        public void RefreshFromToken(JToken tokenProps)
        {
            JObject parsedProps = tokenProps.ToObject<JObject>();

            try
            {
                //var collNum = TryParseToken<string>(parsedProps, "collector_number", null);
                //if(collNum == "12")
                //{
                   
                //}


                var cardLayout = parsedProps.Value<string>("layout");



            if (cardLayout == "transform" || cardLayout == "modal_dfc")
            {
                var normalFace = parsedProps.SelectToken("card_faces")[0];
                //Variants["normal"] = normalFace.SelectToken("image_uris.normal").ToObject<string>();
                ImageUrl = normalFace.SelectToken("image_uris.normal").ToObject<string>();
            }
            //else if (cardLayout == "modal_dfc")
            //{
            //    int breakpoint = 1;
            //}
            else
            {
                //Variants["normal"] = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
                ImageUrl = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
            }

            Price = (decimal?)parsedProps.SelectToken("prices.usd");
            PriceFoil = parsedProps.SelectToken("prices.usd_foil").ToObject<decimal?>();
            PriceTix = parsedProps.SelectToken("prices.tix").ToObject<decimal?>();


            //Prices["normal"] = price;
            //Prices["normal_foil"] = priceFoil;


            //need the string list of legalities
            var legalitiesObj = parsedProps.SelectToken("legalities");

            var legalitiesDictionary = legalitiesObj.ToObject<Dictionary<string, string>>();

            var legalFormatList = legalitiesDictionary.Where(x => x.Value == "legal").Select(x => x.Key).ToList();

            Legalities = legalFormatList;

            #region try/catch straight assignments that I should probably refactor

            Cmc = TryParseToken(parsedProps, "cmc", 100);

            ManaCost = TryParseToken(parsedProps, "mana_cost", "");

            MultiverseId = TryParseToken<int?>(parsedProps, "multiverse_ids[0]", null);
            
            Name = TryParseToken<string>(parsedProps, "name", null);

            Rarity = TryParseToken<string>(parsedProps, "rarity", null);

            Set = TryParseToken<string>(parsedProps, "set", null);

            //TODO - Figure out how to handle oracle text for adventure cards
            Text = TryParseToken<string>(parsedProps, "oracle_text", null);

            Type = TryParseToken<string>(parsedProps, "type_line", null);

            ColorIdentity = TryParseToken<List<string>>(parsedProps, "color_identity", null);

            Colors = TryParseToken<List<string>>(parsedProps, "colors", null);

            string collectorNumberStr = TryParseToken<string>(parsedProps, "collector_number", null);

            if(collectorNumberStr != null)
            {
                




                if (collectorNumberStr.EndsWith('★'))
                {

                    var trimmedNumber = collectorNumberStr.Trim('★');
                    CollectionNumber = int.Parse(trimmedNumber);
                    IsPremium = true;

                }
                else if(collectorNumberStr.EndsWith('a'))
                {
                    //main-face of a double-faced card.  Trim the A and add to Cards
                    var trimmedNumber = collectorNumberStr.Trim('a');
                    CollectionNumber = int.Parse(trimmedNumber);
                }
                else if (collectorNumberStr.EndsWith('b'))
                {
                    //main-face of a double-faced card.  Trim the A and add to Cards
                    var trimmedNumber = collectorNumberStr.Trim('b');
                    CollectionNumber = int.Parse(trimmedNumber);

                    //Technically it's the back of a card, not premium.  But I'm not tracking those in the DB anyways
                    IsPremium = true; 
                }
                else if (collectorNumberStr.EndsWith('e'))
                {
                        DoNotAdd = true;
                        //throw new NotImplementedException("I don't know what this means yet");

                        //This means many things.
                        //In Strixhaven Mystical Archives, this means 'etched'


                        //I don't know what this represents yet
                        ////main-face of a double-faced card.  Trim the A and add to Cards
                        //var trimmedNumber = collectorNumberStr.Trim('e');
                        //CollectionNumber = int.Parse(trimmedNumber);

                        ////Technically it's the back of a card, not premium.  But I'm not tracking those in the DB anyways
                        //IsPremium = true; 
                    }
                else
                {
                    CollectionNumber = int.Parse(collectorNumberStr);
                }
            }
            else
            {
                CollectionNumber = 0; //TODO - Should this default to NULL instead?
            }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        //public void ApplyVariant(JToken variantProps)
        //{

        //    //if (cardLayout == "token")
        //    //{
        //    //    //want to ignore tokens
        //    //    return;
        //    //}

        //    string cardLayout;
        //    string borderColor;

        //    bool isPromo;
        //    string language;

        //    try
        //    {

        //        cardLayout = variantProps.Value<string>("layout");
        //        borderColor = (variantProps.SelectToken("border_color") ?? "").ToString();//.ToObject<List<string>>();
        //        isPromo = variantProps.Value<bool>("promo");
        //        language = variantProps.SelectToken("lang").ToString();

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //    var rawFrameEffect = variantProps.SelectToken("frame_effects");

        //    string variantKey;
        //    //{[lang, {ja}]}

        //    string name = variantProps.Value<string>("name");

        //    if (borderColor != null && borderColor == "borderless")
        //    {
        //        variantKey = "borderless";
        //    }
        //    else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>().Contains("showcase"))
        //    {
        //        variantKey = "showcase";
        //    }
        //    else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>().Contains("extendedart"))
        //    {
        //        variantKey = "extendedart";
        //    }
        //    else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>()[0] == "inverted")
        //    {
        //        variantKey = "inverted";
        //    }
        //    else if (isPromo)
        //    {
        //        variantKey = "promo";
        //    }
        //    else if (language == "ja")
        //    {
        //        variantKey = "ja";
        //    }
        //    else
        //    {
        //        //throw new Exception("Unknown card variant encountered");
        //        return;
        //    }

        //    var price = (decimal?)variantProps.SelectToken("prices.usd");
        //    var priceFoil = variantProps.SelectToken("prices.usd_foil").ToObject<decimal?>();

        //    Prices[variantKey] = price;
        //    Prices[$"{variantKey}_foil"] = priceFoil;

        //    if (cardLayout == "transform")
        //    {
        //        var normalFace = variantProps.SelectToken("card_faces")[0];
        //        var imageUrl = normalFace.SelectToken("image_uris.normal").ToObject<string>();
        //        Variants[variantKey] = imageUrl;
        //    }
        //    else
        //    {
        //        var imageUrl = variantProps.SelectToken("image_uris.normal").ToObject<string>();
        //        Variants[variantKey] = imageUrl;
        //    }

        //}

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public MagicCardDto ToMagicCard()
        {
            MagicCardDto result = new MagicCardDto()
            {
                Cmc = Cmc,
                ColorIdentity = ColorIdentity,
                Colors = Colors,
                Legalities = Legalities,
                ManaCost = ManaCost,
                MultiverseId = MultiverseId,
                Name = Name,
                //Prices = Prices,
                Rarity = Rarity,
                Set = Set,
                Text = Text,
                Type = Type,
                //Variants = Variants,
                CollectionNumber = CollectionNumber,
                ImageUrl = ImageUrl,
                Price = Price,
                PriceFoil = PriceFoil,
                PriceTix = PriceTix,
            };
            return result;
        }

        #region Public fields

        //TODO - remove this field, hack to allow STX mystical Archive etched foils

        public bool DoNotAdd { get; set; }


        [JsonProperty("cmc")]
        public int? Cmc { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> ColorIdentity { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseId")]
        public int? MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prices")]
        public Dictionary<string, decimal?> Prices { get; set; }

        [JsonProperty("variants")]
        public Dictionary<string, string> Variants { get; set; }

        [JsonProperty("lealities")]
        public List<string> Legalities { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public int CollectionNumber { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceFoil { get; set; }

        public decimal? PriceTix { get; set; }

        public bool IsPremium { get; set; }

        public string ImageUrl { get; set; }

        //public bool IsPremium

        #endregion

    }

}