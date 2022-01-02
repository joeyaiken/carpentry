using System.Collections.Generic;

namespace Carpentry.PlaywrightTests.Common
{
public class SeedData
    {
        public SeedData()
        {
            KaldheimSet = new SeedSet("khm", "Kaldheim");
            ModernHorizonsSet = new SeedSet("mh1", "Modern Horizons");
            ColdsnapSet = new SeedSet("csp", "Coldsnap");

            SeedCards = new List<SeedCard>()
            {
                new SeedCard("Ascendant Spirit", KaldheimSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                new SeedCard("Frost Augur", KaldheimSet.SetCode, nameof(CardSearchGroup.Blue), 4),
                
                new SeedCard("Blizzard Brawl", KaldheimSet.SetCode, nameof(CardSearchGroup.Green), 4),
                
                new SeedCard("The Three Seasons", KaldheimSet.SetCode, nameof(CardSearchGroup.Multicolored), 2),
                
                new SeedCard("Faceless Haven", KaldheimSet.SetCode, nameof(CardSearchGroup.RareMythic), 2),
                new SeedCard("Rimewood Falls", KaldheimSet.SetCode, nameof(CardSearchGroup.Lands), 4),

                new SeedCard("Marit Lage's Slumber", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                
                new SeedCard("Conifer Wurm", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Green), 4),
                new SeedCard("Glacial Revelation", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Green), 4),
                
                new SeedCard("Ice-Fang Coatl", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.RareMythic), 4),
                new SeedCard("Abominable Treefolk", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Multicolored), 4),
                
                new SeedCard("Snow-Covered Forest", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Lands), 8),
                new SeedCard("Snow-Covered Island", ModernHorizonsSet.SetCode, nameof(CardSearchGroup.Lands), 8),

                new SeedCard("Boreal Druid", ColdsnapSet.SetCode, nameof(CardSearchGroup.Green), 4),
            };

            GroupSearchOrder = new List<CardSearchGroup>()
            {
                CardSearchGroup.Blue,
                CardSearchGroup.Green,
                CardSearchGroup.Multicolored,
                CardSearchGroup.Lands,
                CardSearchGroup.RareMythic,
            };

            SeedSnowDeck = new SeedDeck()
            {
                DeckName = "Simic Snow Stompy",
                DeckFormat = "modern",
                DeckNotes = "From MTG Goldfish",

                ExpectedColors = new List<string>() {"U", "G"},
                ExpectedValidity = "",
                ExpectedTypeBreakdown = new Dictionary<string, int>()
                {
                    {"Creatures", 24},
                    {"Spells", 8},
                    {"Enchantments", 6},
                    {"Lands", 22},
                },
                ExpectedCurveBreakdown = new Dictionary<int, int>()
                {
                    {1, 16},
                    {2, 10},
                    {3, 4},
                    {4, 4},
                    {5, 4},
                },
                ExpectedCardCount = 60,
            };
        }

        public SeedSet ColdsnapSet { get; set; }
        public SeedSet ModernHorizonsSet { get; set; }
        public SeedSet KaldheimSet { get; set;  }
        
        public List<SeedCard> SeedCards { get; }
        
        public List<CardSearchGroup> GroupSearchOrder { get; }
        
        public SeedDeck SeedSnowDeck { get; set; }

        public List<SeedSet> SeedSets => new List<SeedSet>()
        {
            KaldheimSet, ModernHorizonsSet, ColdsnapSet
        };
        
    }

    public class SeedSet
    {
        public SeedSet(string setCode, string setName)
        {
            SetCode = setCode;
            SetName = setName;
        }
        
        public string SetCode { get; set; }
        public string SetName { get; set; }
    }

    public class SeedCard
    {
        public SeedCard(string cardName, string setCode, string group, int count)
        {
            SetCode = setCode;
            Group = group;
            CardName = cardName;
            Count = count;
        }
        public string SetCode { get; }
        public string Group { get; }
        public string CardName { get; }
        public int Count { get; }
    }

    /// <summary>
    /// A deck that should be created & tested against
    /// </summary>
    public class SeedDeck
    {
        public string DeckName { get; set; }
        public string DeckFormat { get; set; }
        public string DeckNotes { get; set; }
        
        public List<string> ExpectedColors { get; set; }
        public string ExpectedValidity { get; set; }
        public Dictionary<string, int> ExpectedTypeBreakdown { get; set; }
        public Dictionary<int, int> ExpectedCurveBreakdown { get; set; }
        public int ExpectedCardCount { get; set; }
    }
}