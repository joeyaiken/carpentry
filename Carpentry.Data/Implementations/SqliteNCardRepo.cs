using Carpentry.Data.DataContext;
using Carpentry.Data.Models;
//using Carpentry.Data.NewData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;

namespace Carpentry.Data.Implementations
{
    public class _SqliteNCardRepo
    {
        
        //readonly Sqlite4CardRepoContext _legacyContext;
        readonly SqliteNRepoContext _normContext;
        readonly ScryfallDataContext _scryContext;

        public _SqliteNCardRepo(//Sqlite4CardRepoContext legacyContext, 
            SqliteNRepoContext normContext, ScryfallDataContext scryContext)
        {
            //_legacyContext = legacyContext;
            _normContext = normContext;
            _scryContext = scryContext;
        }
      

        #region Core

    

        private void EnsureFKTableRecordsExist()
        {
            var totalRarities = _normContext.Rarities.Count();

            if(totalRarities == 0)
            {
                _normContext.Rarities.Add(new CardRarity
                {
                    Name = "uncommon"  
                });

                _normContext.Rarities.Add(new CardRarity
                {
                    Name = "common"
                });

                _normContext.Rarities.Add(new CardRarity
                {
                    Name = "rare"
                });

                _normContext.Rarities.Add(new CardRarity
                {
                    Name = "mythic"
                });

                _normContext.SaveChanges();
            }

            var totalManaTypes = _normContext.ManaTypes.Count();

            if(totalManaTypes == 0)
            {
                _normContext.ManaTypes.Add(new ManaType
                {
                   Abbreviation = 'W',
                   Name = "White"
                });

                _normContext.ManaTypes.Add(new ManaType
                {
                    Abbreviation = 'U',
                    Name = "Blue"
                });

                _normContext.ManaTypes.Add(new ManaType
                {
                    Abbreviation = 'B',
                    Name = "Black"
                });

                _normContext.ManaTypes.Add(new ManaType
                {
                    Abbreviation = 'R',
                    Name = "Red"
                });

                _normContext.ManaTypes.Add(new ManaType
                {
                    Abbreviation = 'G',
                    Name = "Green"
                });


                _normContext.SaveChanges();
            }

        }



        //migrates Card records for scryfall data
        //Does not migrate InventoryCards or deck data....need to figure that out
        public async Task MigrateLegacyCard(int mid, CardSet matchingSet, List<CardRarity> rarityOptions, List<ManaType> manaTypeOptions)
        {
            var thisScryfallItem = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == mid);

            MagicCardDto parsedLegacyItem = JsonConvert.DeserializeObject<MagicCardDto>(thisScryfallItem.StringData);

            var matchingRarity = rarityOptions.FirstOrDefault(x => x.Name == parsedLegacyItem.Rarity);

            //var matchingSet = setOptions.FirstOrDefault(x => x.Code == parsedLegacyItem.Set);

            NewData.Card newCard = new NewData.Card()
            {
                Cmc = parsedLegacyItem.Cmc,
                ImageUrl = parsedLegacyItem.ImageUrl,
                ImageArtCropUrl = parsedLegacyItem.ImageArtCropUrl,
                ManaCost = parsedLegacyItem.ManaCost,
                MultiverseId = parsedLegacyItem.MultiverseId,
                Name = parsedLegacyItem.Name,
                Price = parsedLegacyItem.Price,
                PriceFoil = parsedLegacyItem.PriceFoil,

                //Rarity = parsedScryfallItem.SelectToken("rarity").ToObject<string>(),
                Rarity = matchingRarity,

                //Set = parsedScryfallItem.SelectToken("set").ToObject<string>(),
                Set = matchingSet,

                Text = parsedLegacyItem.Text,
                Type = parsedLegacyItem.Type,

                //ColorIdentity = parsedScryfallItem.SelectToken("color_identity").ToObject<List<string>>(),
            };

            parsedLegacyItem.ColorIdentity.ForEach(cid =>
            {
                _normContext.ColorIdentities.Add(new CardColorIdentity
                {
                    Card = newCard,
                    ManaType = manaTypeOptions.FirstOrDefault(x => x.Abbreviation.ToString() == cid)
                });
            });

            await Task.Delay(0);

            //also need to add inventory cards for each  legacy card

            //var newInventoryCards = _legacyContext.Cards.Where(x => x.MultiverseId == mid).Select(x => new NewData.InventoryCard
            //{
            //    Card = newCard,
            //    MultiverseId = x.MultiverseId,
            //    IsFoil = x.IsFoil,
            //}).ToList();

            //await _normContext.InventoryCards.AddRangeAsync(newInventoryCards);



            //This isn't handling deck data :(



            //await _normContext.SaveChangesAsync();
        }

        #endregion
    }
}
