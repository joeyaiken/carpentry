﻿using Newtonsoft.Json;
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
            Legalities = new List<string>();
            _cardToken = token;
        }

        private T TryParseToken<T>(string tokenPath, T defaultValue)
        {
            try
            {

                //if(cardObject.TryGetValue(tokenPath, out var parseResult))
                //{
                //    var mapped = parseResult.ToObject<T>();
                //    int breakpoint = 1;
                //    return mapped;
                //}

                var actual = _cardToken.SelectToken(tokenPath).ToObject<T>();
                return actual;
            }
            catch
            {
                return defaultValue;
            }
        }


        public void RefreshFromToken()
        {
            //JObject parsedProps = tokenProps.ToObject<JObject>();
            JObject parsedProps = _cardToken.ToObject<JObject>();

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

                string collectorNumberStr = TryParseToken<string>("collector_number", null);

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

        //TODO - remove this field/hack to allow STX mystical Archive etched foils
        public bool DoNotAdd { get; set; }



        public int? Cmc => TryParseToken("cmc", 100);

        public List<string> ColorIdentity => TryParseToken<List<string>>("color_identity", null);

        public List<string> Colors => TryParseToken<List<string>>("colors", null);

        public string ManaCost => TryParseToken("mana_cost", "");

        public int? MultiverseId => TryParseToken<int?>("multiverse_ids[0]", null);

        public string Name => TryParseToken<string>("name", null);

        public List<string> Legalities { get; set; }

        public string Rarity => TryParseToken<string>("rarity", null);

        public string Set => TryParseToken<string>("set", null);

        //TODO - Figure out how to handle oracle text for adventure cards
        public string Text 
        { 
            get 
            {
                //Double-faced / adventure cards will have multiple card faces with multiple oracle texts
                //Other cards will have a normal oracle_text property
                return TryParseToken<string>("oracle_text", null); 
            } 
        }

        public string Type => TryParseToken<string>("type_line", null);

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