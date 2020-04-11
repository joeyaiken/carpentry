using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Carpentry.Data.Models
{
    public class MagicCardDto
    {
        public MagicCardDto()
        {

        }

        public MagicCardDto(string stringProps)
        {
            JObject parsedProps = JObject.Parse(stringProps);

            try
            {
                try
                {
                    //Cmc
                    Cmc = parsedProps.SelectToken("cmc").ToObject<int>();
                }
                catch
                {
                    Cmc = 100;
                }



                try
                {
                    //ImageUrl
                    ImageUrl = parsedProps.SelectToken("image_uris.normal").ToObject<string>();
                }
                catch
                {
                    ImageUrl = "";
                }
                
                try
                {
                    //ImageArtCropUrl
                    ImageArtCropUrl = parsedProps.SelectToken("image_uris.art_crop").ToObject<string>();
                }
                catch
                {
                    ImageArtCropUrl = "";
                }
                
                try
                {
                    //ManaCost
                    ManaCost = parsedProps.SelectToken("mana_cost").ToObject<string>();
                }
                catch
                {
                    ManaCost = "";
                }
                
                try
                {
                    //MultiverseId
                    MultiverseId = (int)parsedProps.SelectToken("multiverse_ids[0]");
                }
                catch
                {
                    MultiverseId = 0;
                }
                
                try
                {
                    //Name
                    Name = parsedProps.Value<string>("name");
                }
                catch
                {
                    Name = null;
                }
                
                try
                {
                    //Price 
                    Price = (decimal?)parsedProps.SelectToken("prices.usd");
                }
                catch//(Exception ex)
                {
                    Price = 0;
                }
                try
                {
                    //PriceFoil 
                    PriceFoil = parsedProps.SelectToken("prices.usd_foil").ToObject<decimal?>();
                }
                catch// (Exception ex)
                {
                    PriceFoil = 0;
                }


                try
                {
                    //Rarity
                    Rarity = parsedProps.SelectToken("rarity").ToObject<string>();
                }
                catch
                {
                    Rarity = null;
                }
                
                try
                {
                    //Set 
                    Set = parsedProps.SelectToken("set").ToObject<string>();
                }
                catch
                {
                    Set = null;
                }
                
                try
                {
                    //Text 
                    Text = parsedProps.SelectToken("oracle_text").ToObject<string>();
                }
                catch
                {
                    Text = null;
                }
                
                try
                {
                    //Type 
                    Type = parsedProps.SelectToken("type_line").ToObject<string>();
                }
                catch
                {
                    Type = null;
                }

                try
                {
                    //ColorIdentity 
                    ColorIdentity = parsedProps.SelectToken("color_identity").ToObject<List<string>>();
                }
                catch
                {
                    ColorIdentity = null;
                }

            }
            catch (Exception e)
            {
                throw;
                //idk, log something?
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        [JsonProperty("cmc")]
        public int? Cmc { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> ColorIdentity { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("imageArtCropUrl")]
        public string ImageArtCropUrl { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("priceFoil")]
        public decimal? PriceFoil { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("lealities")]
        public List<string> Legalities { get; set; }
    }
}
