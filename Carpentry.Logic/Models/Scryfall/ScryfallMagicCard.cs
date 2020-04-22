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
        public ScryfallMagicCard()
        {
            Prices = new Dictionary<string, decimal?>();
            Legalities = new List<string>();
            Variants = new Dictionary<string, string>();
        }

        public void RefreshFromToken(JToken tokenProps)
        {
            JObject parsedProps = tokenProps.ToObject<JObject>();

            var cardLayout = parsedProps.Value<string>("layout");

            if (cardLayout == "transform")
            {
                var normalFace = parsedProps.SelectToken("card_faces")[0];
                Variants["normal"] = normalFace.SelectToken("image_uris.normal").ToObject<string>();
            }
            else
            {
                Variants["normal"] = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
            }

            var price = (decimal?)parsedProps.SelectToken("prices.usd");
            var priceFoil = parsedProps.SelectToken("prices.usd_foil").ToObject<decimal?>();

            Prices["normal"] = price;
            Prices["normal_foil"] = priceFoil;

            //need the string list of legalities
            var legalitiesObj = parsedProps.SelectToken("legalities");

            var legalitiesDictionary = legalitiesObj.ToObject<Dictionary<string, string>>();

            var legalFormatList = legalitiesDictionary.Where(x => x.Value == "legal").Select(x => x.Key).ToList();

            Legalities = legalFormatList;

            #region try/catch straight assignments that I should probably refactor

            try
            {
                Cmc = parsedProps.SelectToken("cmc").ToObject<int>();
            }
            catch
            {
                Cmc = 100;
            }

            try
            {
                ManaCost = parsedProps.SelectToken("mana_cost").ToObject<string>();
            }
            catch
            {
                ManaCost = "";
            }

            try
            {
                MultiverseId = (int)parsedProps.SelectToken("multiverse_ids[0]");
            }
            catch
            {
                MultiverseId = 0;
            }

            try
            {
                Name = parsedProps.Value<string>("name");
            }
            catch
            {
                Name = null;
            }

            try
            {
                Rarity = parsedProps.SelectToken("rarity").ToObject<string>();
            }
            catch
            {
                Rarity = null;
            }

            try
            {
                Set = parsedProps.SelectToken("set").ToObject<string>();
            }
            catch
            {
                Set = null;
            }

            try
            {
                //TODO - Figure out how to handle oracle text for adventure cards
                Text = parsedProps.SelectToken("oracle_text").ToObject<string>();
            }
            catch
            {
                Text = null;
            }

            try
            {
                Type = parsedProps.SelectToken("type_line").ToObject<string>();
            }
            catch
            {
                Type = null;
            }

            try
            {
                ColorIdentity = parsedProps.SelectToken("color_identity").ToObject<List<string>>();
            }
            catch
            {
                ColorIdentity = null;
            }

            try
            {
                Colors = parsedProps.SelectToken("colors").ToObject<List<string>>();
            }
            catch
            {
                Colors = null;
            }

            #endregion
        }

        public void ApplyVariant(JToken variantProps)
        {

            //if (cardLayout == "token")
            //{
            //    //want to ignore tokens
            //    return;
            //}

            string cardLayout;
            string borderColor;

            bool isPromo;
            string language;

            try
            {

                cardLayout = variantProps.Value<string>("layout");
                borderColor = (variantProps.SelectToken("border_color") ?? "").ToString();//.ToObject<List<string>>();
                isPromo = variantProps.Value<bool>("promo");
                language = variantProps.SelectToken("lang").ToString();

            }
            catch (Exception e)
            {
                throw;
            }

            var rawFrameEffect = variantProps.SelectToken("frame_effects");

            string variantKey;
            //{[lang, {ja}]}


            if (borderColor != null && borderColor == "borderless")
            {
                variantKey = "borderless";
            }
            else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>()[0] == "showcase")
            {
                variantKey = "showcase";
            }
            else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>()[0] == "extendedart")
            {
                variantKey = "extendedart";
            }
            else if (rawFrameEffect != null && rawFrameEffect.ToObject<List<string>>()[0] == "inverted")
            {
                variantKey = "inverted";
            }
            else if (isPromo)
            {
                variantKey = "promo";
            }
            else if (language == "ja")
            {
                variantKey = "ja";
            }
            else
            {
                //throw new Exception("Unknown card variant encountered");
                return;
            }

            var price = (decimal?)variantProps.SelectToken("prices.usd");
            var priceFoil = variantProps.SelectToken("prices.usd_foil").ToObject<decimal?>();

            Prices[variantKey] = price;
            Prices[$"{variantKey}_foil"] = priceFoil;

            if (cardLayout == "transform")
            {
                var normalFace = variantProps.SelectToken("card_faces")[0];
                var imageUrl = normalFace.SelectToken("image_uris.normal").ToObject<string>();
                Variants[variantKey] = imageUrl;
            }
            else
            {
                var imageUrl = variantProps.SelectToken("image_uris.normal").ToObject<string>();
                Variants[variantKey] = imageUrl;
            }

        }

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
                Prices = Prices,
                Rarity = Rarity,
                Set = Set,
                Text = Text,
                Type = Type,
                Variants = Variants,
            };
            return result;
        }

        #region Public fields

        [JsonProperty("cmc")]
        public int? Cmc { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> ColorIdentity { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

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

        #endregion

    }

}