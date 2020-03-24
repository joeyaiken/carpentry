using Carpentry.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Tests
{
    public class TestDataService
    {
        readonly SqliteDataContext _cardContext;

        public TestDataService(SqliteDataContext cardContext)
        {
            _cardContext = cardContext;
        }

        public async Task EnsureDefaultRecordsExist()
        {
            await EnsureDbCardStatusesExist();
            EnsureDbRaritiesExist();
            EnsureDbManaTypesExist();
            EnsureDbMagicFormatsExist();
            EnsureDbVariantTypesExist();

        }

        //How do I handle the inventory unit tests that don't need to be re-staged every time because they don't modify records?
        //  Might not really matter in the long run...


        //Called after every inventory test that involves modifying things
        public void ResetInventoryTestRecords()
        {
            //clear the database?
            //ClearDb();

            //ensure all defaults exist



            //add mock data



        }

        public void ResetDeckTestRecords()
        {

            int breakpoint = 1;
        }


        public async Task ResetDbAsync()
        {
            //await _cardContext.Database.EnsureDeletedAsync();
            //await _cardContext.Database.EnsureCreatedAsync();
            //await EnsureDefaultRecordsExist();
        }

        public async Task SeedDeckTestRecords()
        {
            await GenerateMockDecks();
        }


        #region private

        private async Task EnsureDbCardStatusesExist()
        {
            List<InventoryCardStatus> allStatuses = new List<InventoryCardStatus>()
            {
                new InventoryCardStatus { Id = 1, Name = "Inventory" },
                new InventoryCardStatus { Id = 2, Name = "Buy List" },
                new InventoryCardStatus { Id = 3, Name = "Sell List" },
            };

            //List<Task> something = allStatuses.Select(x =>  TryAddCardStatus(x));

            #region approach 1
            var statusTasks = allStatuses.Select(x => TryAddCardStatus(x)).ToList();

            await Task.WhenAll(statusTasks);
            #endregion

            #region approach 2

            var somethign = allStatuses.Select(async status =>
            {
                var existingStatus = _cardContext.CardStatuses.FirstOrDefault(x => x.Name == status.Name);
                if (existingStatus == null)
                {
                    return await _cardContext.CardStatuses.AddAsync(status);
                }
                return null;
            }).ToList();

            await Task.WhenAll(somethign);

            #endregion

            await _cardContext.SaveChangesAsync();
        }

        private async Task TryAddCardStatus(InventoryCardStatus status)
        {
            var existingStatus = _cardContext.CardStatuses.FirstOrDefault(x => x.Name == status.Name);
            if (existingStatus == null)
            {
                await _cardContext.CardStatuses.AddAsync(status);
            }
        }

        private void EnsureDbRaritiesExist()
        {
            List<CardRarity> allRarities = new List<CardRarity>()
            {
                new CardRarity
                {
                    Id = 'M',
                    Name = "mythic",
                },
                new CardRarity
                {
                    Id = 'R',
                    Name = "rare",
                },
                new CardRarity
                {
                    Id = 'U',
                    Name = "uncommon",
                },
                new CardRarity
                {
                    Id = 'C',
                    Name = "common",
                },
            };

            allRarities.ForEach(rarity =>
            {
                var existingRecord = _cardContext.Rarities.FirstOrDefault(x => x.Id == rarity.Id);
                if (existingRecord == null)
                {
                    _cardContext.Rarities.Add(rarity);
                }
            });

            _cardContext.SaveChanges();

            //TODO: Remove any non-default values
        }

        private void EnsureDbManaTypesExist()
        {
            List<ManaType> allManaTypes = new List<ManaType>()
            {
                new ManaType{
                    Id = 'W',
                    Name = "White"
                },
                new ManaType{
                    Id = 'U',
                    Name = "Blue"
                },
                new ManaType{
                    Id = 'B',
                    Name = "Black"
                },
                new ManaType{
                    Id = 'R',
                    Name = "Red"
                },
                new ManaType{
                    Id = 'G',
                    Name = "Green"
                },
            };

            allManaTypes.ForEach(type =>
            {
                var existingRecord = _cardContext.ManaTypes.FirstOrDefault(x => x.Id == type.Id);
                if (existingRecord == null)
                {
                    _cardContext.ManaTypes.Add(type);
                }
            });

            _cardContext.SaveChanges();
        }

        private void EnsureDbMagicFormatsExist()
        {
            List<MagicFormat> allFormats = new List<MagicFormat>()
            {
                new MagicFormat { Name = "standard" },
                new MagicFormat { Name = "pioneer" },
                new MagicFormat { Name = "modern" },
                new MagicFormat { Name = "pauper" },
                new MagicFormat { Name = "commander" },
                new MagicFormat { Name = "brawl" },
            };

            allFormats.ForEach(format =>
            {
                var existingFormat = _cardContext.MagicFormats.FirstOrDefault(x => x.Name == format.Name);
                if (existingFormat == null)
                {
                    _cardContext.MagicFormats.Add(format);
                }
            });

            _cardContext.SaveChanges();
        }

        private void EnsureDbVariantTypesExist()
        {
            List<CardVariantType> allVariants = new List<CardVariantType>()
            {
                new CardVariantType { Name = "normal" },
                new CardVariantType { Name = "borderless" },
                new CardVariantType { Name = "showcase" },
                new CardVariantType { Name = "extendedart" },
                new CardVariantType { Name = "inverted" },
                new CardVariantType { Name = "promo" },
                new CardVariantType { Name = "ja" },
            };

            allVariants.ForEach(variant =>
            {
                var existingVariant = _cardContext.VariantTypes.FirstOrDefault(x => x.Name == variant.Name);
                if (existingVariant == null)
                {
                    _cardContext.VariantTypes.Add(variant);
                }
            });

            _cardContext.SaveChanges();
        }

        //


        private async Task InsertInventoryMockRecords()
        {

            CardSet mockSet = new CardSet
            {
                Code = "M01",
                Name = "Standard Mock Set"
            };

            CardSet mocSet2 = new CardSet
            {
                Code = "M02",
                Name = "Modern Mock Set"
            };

            _cardContext.Sets.AddRange(new List<CardSet>() { mockSet, mocSet2 });

            //Cards

            //What if...

            //2 knights, 1 non-knight, 1 instant, 1 sorcery
            //1 knight is rare, 1 is uncommon
            //instant is common
            //other creature is rare
            //sorcery is mythic (board wipe)



            //Should also probably add a second set
            //Enchant (2x?) M / R
            //Instant U
            //Sorcery U
            //PW ? Creature? C

            List<Card> mockCards = new List<Card>()
            {
                //Rare knight creature - M01
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mockSet,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //uncommon knight creature - M01
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mockSet,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //rare non-knight creature - M01
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mockSet,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //common instant - M01
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mockSet,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //rare sorcery - M01
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mockSet,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //Enchant M - M02
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mocSet2,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //Enchant R - M02
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mocSet2,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //Instant U - M02
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mocSet2,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //Sorcery U - M02
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    InventoryCards = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mocSet2,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
                //Creature C - M02
                new Card()
                {
                    CardColorIdentities = null,
                    CardColors = null,
                    Cmc = null,
                    Legalities = null,
                    ManaCost = null,
                    Name = null,
                    RarityId = 'C',
                    Set = mocSet2,
                    Text = null,
                    Type = null,
                    Variants = null,
                },
            };

            await _cardContext.AddRangeAsync(mockCards);

            await _cardContext.SaveChangesAsync();
        }

        private async Task GenerateMockDecks()
        {
            CardSet mockSet = new CardSet
            {
                Code = "MOC",
                Name = "Standard Mock Set"
            };
            await _cardContext.Sets.AddAsync(mockSet);
            await InsertMockDeck_Red(mockSet);
            await InsertMockDeck_Blue(mockSet);
            await InsertMockDeck_Green(mockSet);
            await InsertMockDeck_White(mockSet);
            await InsertMockDeck_Black(mockSet);
            await _cardContext.SaveChangesAsync();
        }


        #endregion


        #region mock deck data

        private async Task InsertMockDeck_Red(CardSet mockSet)
        {
            var mockDeck = new Deck()
            {
                Name = "Red Mock Deck",
                Notes = "Mono-red 30 card mock deck",
                BasicR = 12,
                Cards = new List<DeckCard>()
            };

            //cards
            var creature_1 = CreateMockCreature("Goblin", 'C', 1, mockSet, 'R');
            var creature_2 = CreateMockCreature("Ogre", 'U', 3, mockSet, 'R');
            var creature_3 = CreateMockCreature("Dragon", 'R', 6, mockSet, 'R');
            var spell_1 = CreateMockSpell("Shock", 'C', 1, "Instant", mockSet, 'R');
            var spell_2 = CreateMockSpell("Scary Spell", 'M', 7, "Sorcery", mockSet, 'R');

            //Create 4 copies of each creature, each copy gets added to the deck
            for (int i = 0; i < 4; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(creature_1));
                mockDeck.Cards.Add(CreateCardForDeck(creature_2));
                mockDeck.Cards.Add(CreateCardForDeck(creature_3));
            }

            //create 3 copies of the other spells, each copy gets added to the deck
            for (int i = 0; i < 3; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(spell_1));
                mockDeck.Cards.Add(CreateCardForDeck(spell_2));
            }

            //what happens if I now try to add the deck?....
            await _cardContext.Decks.AddAsync(mockDeck);
        }

        private async Task InsertMockDeck_Blue(CardSet mockSet)
        {
            var mockDeck = new Deck()
            {
                Name = "Blue Mock Deck",
                Notes = "Mono-blue 30 card mock deck",
                BasicU = 12,
                Cards = new List<DeckCard>()
            };

            //cards
            var creature_1 = CreateMockCreature("Wall", 'C', 1, mockSet, 'U');
            var creature_2 = CreateMockCreature("Spirit", 'U', 3, mockSet, 'U');
            var creature_3 = CreateMockCreature("Whale", 'R', 6, mockSet, 'U');
            var spell_1 = CreateMockSpell("Counter", 'C', 2, "Instant", mockSet, 'U');
            var spell_2 = CreateMockSpell("Draw Cards", 'R', 5, "Sorcery", mockSet, 'U');

            //Create 4 copies of each creature, each copy gets added to the deck
            for (int i = 0; i < 4; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(creature_1));
                mockDeck.Cards.Add(CreateCardForDeck(creature_2));
                mockDeck.Cards.Add(CreateCardForDeck(creature_3));
            }

            //create 3 copies of the other spells, each copy gets added to the deck
            for (int i = 0; i < 3; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(spell_1));
                mockDeck.Cards.Add(CreateCardForDeck(spell_2));
            }

            //what happens if I now try to add the deck?....
            await _cardContext.Decks.AddAsync(mockDeck);
        }

        private async Task InsertMockDeck_Green(CardSet mockSet)
        {
            var mockDeck = new Deck()
            {
                Name = "Green Mock Deck",
                Notes = "Mono-green 30 card mock deck",
                BasicG = 12,
                Cards = new List<DeckCard>()
            };

            //cards
            var creature_1 = CreateMockCreature("Elf", 'C', 1, mockSet, 'G');
            var creature_2 = CreateMockCreature("Bear", 'U', 3, mockSet, 'G');
            var creature_3 = CreateMockCreature("Elephant", 'R', 6, mockSet, 'G');
            var spell_1 = CreateMockSpell("Prey Upon", 'C', 1, "Instant", mockSet, 'G');
            var spell_2 = CreateMockSpell("Creature Buff Aura", 'M', 7, "Enchantment", mockSet, 'G');

            //Create 4 copies of each creature, each copy gets added to the deck
            for (int i = 0; i < 4; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(creature_1));
                mockDeck.Cards.Add(CreateCardForDeck(creature_2));
                mockDeck.Cards.Add(CreateCardForDeck(creature_3));
            }

            //create 3 copies of the other spells, each copy gets added to the deck
            for (int i = 0; i < 3; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(spell_1));
                mockDeck.Cards.Add(CreateCardForDeck(spell_2));
            }

            //what happens if I now try to add the deck?....
            await _cardContext.Decks.AddAsync(mockDeck);
        }

        private async Task InsertMockDeck_White(CardSet mockSet)
        {
            var mockDeck = new Deck()
            {
                Name = "White Mock Deck",
                Notes = "Mono-white 30 card mock deck",
                BasicW = 12,
                Cards = new List<DeckCard>()
            };

            //cards
            var creature_1 = CreateMockCreature("Bird", 'C', 1, mockSet, 'W');
            var creature_2 = CreateMockCreature("Soldier", 'U', 2, mockSet, 'W');
            var creature_3 = CreateMockCreature("Angel", 'R', 5, mockSet, 'W');
            var spell_1 = CreateMockSpell("Protection", 'C', 1, "Instant", mockSet, 'W');
            var spell_2 = CreateMockSpell("Prison", 'M', 3, "Enchantment", mockSet, 'W');

            //Create 4 copies of each creature, each copy gets added to the deck
            for (int i = 0; i < 4; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(creature_1));
                mockDeck.Cards.Add(CreateCardForDeck(creature_2));
                mockDeck.Cards.Add(CreateCardForDeck(creature_3));
            }

            //create 3 copies of the other spells, each copy gets added to the deck
            for (int i = 0; i < 3; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(spell_1));
                mockDeck.Cards.Add(CreateCardForDeck(spell_2));
            }

            //what happens if I now try to add the deck?....
            await _cardContext.Decks.AddAsync(mockDeck);
        }

        private async Task InsertMockDeck_Black(CardSet mockSet)
        {
            var mockDeck = new Deck()
            {
                Name = "Black Mock Deck",
                Notes = "Mono-black 30 card mock deck",
                BasicB = 12,
                Cards = new List<DeckCard>()
            };

            //cards
            var creature_1 = CreateMockCreature("Goblin", 'C', 1, mockSet, 'B');
            var creature_2 = CreateMockCreature("Ogre", 'U', 3, mockSet, 'B');
            var creature_3 = CreateMockCreature("Dragon", 'R', 6, mockSet, 'B');
            var spell_1 = CreateMockSpell("Shock", 'C', 1, "Instant", mockSet, 'B');
            var spell_2 = CreateMockSpell("Scary Spell", 'M', 7, "Sorcery", mockSet, 'B');

            //Create 4 copies of each creature, each copy gets added to the deck
            for (int i = 0; i < 4; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(creature_1));
                mockDeck.Cards.Add(CreateCardForDeck(creature_2));
                mockDeck.Cards.Add(CreateCardForDeck(creature_3));
            }

            //create 3 copies of the other spells, each copy gets added to the deck
            for (int i = 0; i < 3; i++)
            {
                mockDeck.Cards.Add(CreateCardForDeck(spell_1));
                mockDeck.Cards.Add(CreateCardForDeck(spell_2));
            }

            //what happens if I now try to add the deck?....
            await _cardContext.Decks.AddAsync(mockDeck);
        }

        private static Card CreateMockCreature(string name, char rarity, int cmc, CardSet set, char color)
        {
            return CreateCard(name, set, color, rarity, $"A mock {name}", $"Creature - {name}", cmc);
        }

        private static Card CreateMockSpell(string name, char rarity, int cmc, string type, CardSet set, char color)
        {
            return CreateCard(name, set, color, rarity, $"A mock {type}", type, cmc);
        }

        private static Card CreateCard(string name, CardSet set, char color, char rarity, string text, string type, int cmc)
        {
            return new Card()
            {
                Cmc = cmc,
                ManaCost = (cmc > 1) ? $"{{{cmc - 1}}}{{{color}}}" : $"{{{color}}}",
                Name = name,
                RarityId = rarity,
                Text = text,
                Type = type,
                Set = set,
                CardColorIdentities = new List<CardColorIdentity>() { new CardColorIdentity() { ManaTypeId = color } },
                CardColors = new List<CardColor>() { new CardColor() { ManaTypeId = color } },
                //List<CardVariant> Variants
                //List<CardLegality> Legalities
            };
        }

        //Creates a new deck card and new inventory card for a deck
        private static DeckCard CreateCardForDeck(Card cardDefinition)
        {
            var newDeckCard = new DeckCard()
            {
                InventoryCard = new InventoryCard()
                {
                    IsFoil = false,
                    //MultiverseId = cardDefinition.Id,
                    InventoryCardStatusId = 1, //status 1 == owned
                    Card = cardDefinition,
                    //do I have to populate variant?
                },
            };

            return newDeckCard;
        }


        #endregion

        #region default data




        #endregion


    }
}
