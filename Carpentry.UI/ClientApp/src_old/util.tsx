

export interface CardSet{
    code: string;
    name: string;
}




export function GetSetFilters(): FilterOption[] {
    const store = 
        [

            {
                    "code": "thb",
                    "name": "Theros Beyond Death",
                    "type": "core",
                    "releaseDate": "2007-07-13",
                    "block": "Core Set",
                    "onlineOnly": false
                },
        // {
        //     "code": "10E",
        //     "name": "Tenth Edition",
        //     "type": "core",
        //     "releaseDate": "2007-07-13",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "5DN",
        //     "name": "Fifth Dawn",
        //     "type": "expansion",
        //     "releaseDate": "2004-06-04",
        //     "block": "Mirrodin",
        //     "onlineOnly": false
        // },\
        // {
        //     "code": "8ED",
        //     "name": "Eighth Edition",
        //     "type": "core",
        //     "releaseDate": "2003-07-28",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "9ED",
        //     "name": "Ninth Edition",
        //     "type": "core",
        //     "releaseDate": "2005-07-29",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        {
            "code": "ELD",
            "name": "Throne of Eldraine",
            "type": "expansion",
            "releaseDate": "2019-10-04",
            "onlineOnly": false
        },
         {
             "code": "A25",
             "name": "Masters 25",
             "type": "masters",
             "releaseDate": "2018-03-16",
             "onlineOnly": false
         },
         {
             "code": "AER",
             "name": "Aether Revolt",
             "type": "expansion",
             "releaseDate": "2017-01-20",
             "block": "Kaladesh",
             "onlineOnly": false
         },
        {
            "code": "AKH",
            "name": "Amonkhet",
            "type": "expansion",
            "releaseDate": "2017-04-28",
            "block": "Amonkhet",
            "onlineOnly": false
        },
        // {
        //     "code": "ALA",
        //     "name": "Shards of Alara",
        //     "type": "expansion",
        //     "releaseDate": "2008-10-03",
        //     "block": "Alara",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "ANA",
        //     "name": "Arena New Player Experience",
        //     "type": "starter",
        //     "releaseDate": "2018-07-14",
        //     "onlineOnly": true
        // },\
        // {
        //     "code": "ARB",
        //     "name": "Alara Reborn",
        //     "type": "expansion",
        //     "releaseDate": "2009-04-30",
        //     "block": "Alara",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "ARC",
        //     "name": "Archenemy",
        //     "type": "archenemy",
        //     "releaseDate": "2010-06-18",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "AVR",
        //     "name": "Avacyn Restored",
        //     "type": "expansion",
        //     "releaseDate": "2012-05-04",
        //     "block": "Innistrad",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "BBD",
        //     "name": "Battlebond",
        //     "type": "draft_innovation",
        //     "releaseDate": "2018-06-08",
        //     "onlineOnly": false
        // },
         {
             "code": "BFZ",
             "name": "Battle for Zendikar",
             "type": "expansion",
             "releaseDate": "2015-10-02",
             "block": "Battle for Zendikar",
             "onlineOnly": false
         },
         {
             "code": "BNG",
             "name": "Born of the Gods",
             "type": "expansion",
             "releaseDate": "2014-02-07",
             "block": "Theros",
             "onlineOnly": false
         },
        // {
        //     "code": "BOK",
        //     "name": "Betrayers of Kamigawa",
        //     "type": "expansion",
        //     "releaseDate": "2005-02-04",
        //     "block": "Kamigawa",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "C13",
        //     "name": "Commander 2013",
        //     "type": "commander",
        //     "releaseDate": "2013-11-01",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "C14",
        //     "name": "Commander 2014",
        //     "type": "commander",
        //     "releaseDate": "2014-11-07",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "C15",
        //     "name": "Commander 2015",
        //     "type": "commander",
        //     "releaseDate": "2015-11-13",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "C16",
        //     "name": "Commander 2016",
        //     "type": "commander",
        //     "releaseDate": "2016-11-11",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
         {
             "code": "C17",
             "name": "Commander 2017",
             "type": "commander",
             "releaseDate": "2017-08-25",
             "block": "Commander",
             "onlineOnly": false
         },
         {
             "code": "C18",
             "name": "Commander 2018",
             "type": "commander",
             "releaseDate": "2018-08-09",
             "block": "Commander",
             "onlineOnly": false
         },
        // {
        //     "code": "CHK",
        //     "name": "Champions of Kamigawa",
        //     "type": "expansion",
        //     "releaseDate": "2004-10-01",
        //     "block": "Kamigawa",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CM1",
        //     "name": "Commander's Arsenal",
        //     "type": "commander",
        //     "releaseDate": "2012-11-02",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CM2",
        //     "name": "Commander Anthology Volume II",
        //     "type": "commander",
        //     "releaseDate": "2018-06-08",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CMA",
        //     "name": "Commander Anthology",
        //     "type": "commander",
        //     "releaseDate": "2017-06-09",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CMD",
        //     "name": "Commander 2011",
        //     "type": "commander",
        //     "releaseDate": "2011-06-17",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CN2",
        //     "name": "Conspiracy: Take the Crown",
        //     "type": "draft_innovation",
        //     "releaseDate": "2016-08-26",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CNS",
        //     "name": "Conspiracy",
        //     "type": "draft_innovation",
        //     "releaseDate": "2014-06-06",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CON",
        //     "name": "Conflux",
        //     "type": "expansion",
        //     "releaseDate": "2009-02-06",
        //     "block": "Alara",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CP1",
        //     "name": "Magic 2015 Clash Pack",
        //     "type": "starter",
        //     "releaseDate": "2014-07-18",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CP2",
        //     "name": "Fate Reforged Clash Pack",
        //     "type": "starter",
        //     "releaseDate": "2015-01-23",
        //     "block": "Khans of Tarkir",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CP3",
        //     "name": "Magic Origins Clash Pack",
        //     "type": "starter",
        //     "releaseDate": "2015-07-17",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CSP",
        //     "name": "Coldsnap",
        //     "type": "expansion",
        //     "releaseDate": "2006-07-21",
        //     "block": "Ice Age",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "CST",
        //     "name": "Coldsnap Theme Decks",
        //     "type": "box",
        //     "releaseDate": "2006-07-21",
        //     "block": "Ice Age",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DD1",
        //     "name": "Duel Decks: Elves vs. Goblins",
        //     "type": "duel_deck",
        //     "releaseDate": "2007-11-16",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DD2",
        //     "name": "Duel Decks: Jace vs. Chandra",
        //     "type": "duel_deck",
        //     "releaseDate": "2008-11-07",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDC",
        //     "name": "Duel Decks: Divine vs. Demonic",
        //     "type": "duel_deck",
        //     "releaseDate": "2009-04-10",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDD",
        //     "name": "Duel Decks: Garruk vs. Liliana",
        //     "type": "duel_deck",
        //     "releaseDate": "2009-10-30",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDE",
        //     "name": "Duel Decks: Phyrexia vs. the Coalition",
        //     "type": "duel_deck",
        //     "releaseDate": "2010-03-19",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDF",
        //     "name": "Duel Decks: Elspeth vs. Tezzeret",
        //     "type": "duel_deck",
        //     "releaseDate": "2010-09-03",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDG",
        //     "name": "Duel Decks: Knights vs. Dragons",
        //     "type": "duel_deck",
        //     "releaseDate": "2011-04-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDH",
        //     "name": "Duel Decks: Ajani vs. Nicol Bolas",
        //     "type": "duel_deck",
        //     "releaseDate": "2011-09-02",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDI",
        //     "name": "Duel Decks: Venser vs. Koth",
        //     "type": "duel_deck",
        //     "releaseDate": "2012-03-30",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDJ",
        //     "name": "Duel Decks: Izzet vs. Golgari",
        //     "type": "duel_deck",
        //     "releaseDate": "2012-09-07",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDK",
        //     "name": "Duel Decks: Sorin vs. Tibalt",
        //     "type": "duel_deck",
        //     "releaseDate": "2013-03-15",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDL",
        //     "name": "Duel Decks: Heroes vs. Monsters",
        //     "type": "duel_deck",
        //     "releaseDate": "2013-09-06",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDM",
        //     "name": "Duel Decks: Jace vs. Vraska",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-03-14",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDN",
        //     "name": "Duel Decks: Speed vs. Cunning",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-09-05",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDO",
        //     "name": "Duel Decks: Elspeth vs. Kiora",
        //     "type": "duel_deck",
        //     "releaseDate": "2015-02-27",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDP",
        //     "name": "Duel Decks: Zendikar vs. Eldrazi",
        //     "type": "duel_deck",
        //     "releaseDate": "2015-08-28",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDQ",
        //     "name": "Duel Decks: Blessed vs. Cursed",
        //     "type": "duel_deck",
        //     "releaseDate": "2016-02-26",
        //     "onlineOnly": false
        // },
         {
             "code": "DDR",
             "name": "Duel Decks: Nissa vs. Ob Nixilis",
             "type": "duel_deck",
             "releaseDate": "2016-09-02",
             "onlineOnly": false
         },
        // {
        //     "code": "DDS",
        //     "name": "Duel Decks: Mind vs. Might",
        //     "type": "duel_deck",
        //     "releaseDate": "2017-03-31",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDT",
        //     "name": "Duel Decks: Merfolk vs. Goblins",
        //     "type": "duel_deck",
        //     "releaseDate": "2017-10-24",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DDU",
        //     "name": "Duel Decks: Elves vs. Inventors",
        //     "type": "duel_deck",
        //     "releaseDate": "2018-04-06",
        //     "onlineOnly": false
        // },
         {
             "code": "DGM",
             "name": "Dragon's Maze",
             "type": "expansion",
             "releaseDate": "2013-05-03",
             "block": "Return to Ravnica",
             "onlineOnly": false
         },
        // {
        //     "code": "DIS",
        //     "name": "Dissension",
        //     "type": "expansion",
        //     "releaseDate": "2006-05-05",
        //     "block": "Ravnica",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DKA",
        //     "name": "Dark Ascension",
        //     "type": "expansion",
        //     "releaseDate": "2012-02-03",
        //     "block": "Innistrad",
        //     "onlineOnly": false
        // },\
         {
             "code": "DOM",
             "name": "Dominaria",
             "type": "expansion",
             "releaseDate": "2018-04-27",
             "onlineOnly": false
         },
        // {
        //     "code": "DPA",
        //     "name": "Duels of the Planeswalkers",
        //     "type": "box",
        //     "releaseDate": "2010-06-04",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DRB",
        //     "name": "From the Vault: Dragons",
        //     "type": "from_the_vault",
        //     "releaseDate": "2008-08-29",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "DST",
        //     "name": "Darksteel",
        //     "type": "expansion",
        //     "releaseDate": "2004-02-06",
        //     "block": "Mirrodin",
        //     "onlineOnly": false
        // },
         {
             "code": "DTK",
             "name": "Dragons of Tarkir",
             "type": "expansion",
             "releaseDate": "2015-03-27",
             "block": "Khans of Tarkir",
             "onlineOnly": false
         },
        // {
        //     "code": "DVD",
        //     "name": "Duel Decks Anthology: Divine vs. Demonic",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-12-05",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "E01",
        //     "name": "Archenemy: Nicol Bolas",
        //     "type": "archenemy",
        //     "releaseDate": "2017-06-16",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "E02",
        //     "name": "Explorers of Ixalan",
        //     "type": "box",
        //     "releaseDate": "2017-11-24",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "EMA",
        //     "name": "Eternal Masters",
        //     "type": "masters",
        //     "releaseDate": "2016-06-10",
        //     "onlineOnly": false
        // },
         {
             "code": "EMN",
             "name": "Eldritch Moon",
             "type": "expansion",
             "releaseDate": "2016-07-22",
             "block": "Shadows Over Innistrad",
             "onlineOnly": false
         },
        // {
        //     "code": "EVE",
        //     "name": "Eventide",
        //     "type": "expansion",
        //     "releaseDate": "2008-07-25",
        //     "block": "Shadowmoor",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "EVG",
        //     "name": "Duel Decks Anthology: Elves vs. Goblins",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-12-05",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "EXP",
        //     "name": "Zendikar Expeditions",
        //     "type": "masterpiece",
        //     "releaseDate": "2015-10-02",
        //     "block": "Battle for Zendikar",
        //     "onlineOnly": false
        // },
        {
            "code": "FRF",
            "name": "Fate Reforged",
            "type": "expansion",
            "releaseDate": "2015-01-23",
            "block": "Khans of Tarkir",
            "onlineOnly": false
        },
        // {
        //     "code": "FUT",
        //     "name": "Future Sight",
        //     "type": "expansion",
        //     "releaseDate": "2007-05-04",
        //     "block": "Time Spiral",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "G17",
        //     "name": "2017 Gift Pack",
        //     "type": "box",
        //     "releaseDate": "2017-10-20",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "G18",
        //     "name": "M19 Gift Pack",
        //     "type": "box",
        //     "releaseDate": "2018-11-16",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "GK1",
        //     "name": "GRN Guild Kit",
        //     "type": "box",
        //     "releaseDate": "2018-11-02",
        //     "onlineOnly": false
        // },
        {
            "code": "GK2",
            "name": "RNA Guild Kit",
            "type": "box",
            "releaseDate": "2019-02-15",
            "onlineOnly": false
        },
        // {
        //     "code": "GNT",
        //     "name": "Game Night",
        //     "type": "box",
        //     "releaseDate": "2018-11-16",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "GPT",
        //     "name": "Guildpact",
        //     "type": "expansion",
        //     "releaseDate": "2006-02-03",
        //     "block": "Ravnica",
        //     "onlineOnly": false
        // },
        {
            "code": "GRN",
            "name": "Guilds of Ravnica",
            "type": "expansion",
            "releaseDate": "2018-10-05",
            "block": "Guilds of Ravnica",
            "onlineOnly": false
        },
        // {
        //     "code": "GS1",
        //     "name": "Global Series Jiang Yanggu & Mu Yanling",
        //     "type": "duel_deck",
        //     "releaseDate": "2018-06-22",
        //     "onlineOnly": false
        // },
         {
             "code": "GTC",
             "name": "Gatecrash",
             "type": "expansion",
             "releaseDate": "2013-02-01",
             "block": "Return to Ravnica",
             "onlineOnly": false
         },
        // {
        //     "code": "GVL",
        //     "name": "Duel Decks Anthology: Garruk vs. Liliana",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-12-05",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "H09",
        //     "name": "Premium Deck Series: Slivers",
        //     "type": "premium_deck",
        //     "releaseDate": "2009-11-20",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "HOP",
        //     "name": "Planechase",
        //     "type": "planechase",
        //     "releaseDate": "2009-09-04",
        //     "onlineOnly": false
        // },
        {
            "code": "HOU",
            "name": "Hour of Devastation",
            "type": "expansion",
            "releaseDate": "2017-07-14",
            "block": "Amonkhet",
            "onlineOnly": false
        },
        // {
        //     "code": "HTR17",
        //     "name": "2017 Heroes of the Realm",
        //     "type": "memorabilia",
        //     "releaseDate": "2018-08-01",
        //     "onlineOnly": false
        // },
         {
             "code": "IMA",
             "name": "Iconic Masters",
             "type": "masters",
             "releaseDate": "2017-11-17",
             "onlineOnly": false
         },
        // {
        //     "code": "ISD",
        //     "name": "Innistrad",
        //     "type": "expansion",
        //     "releaseDate": "2011-09-30",
        //     "block": "Innistrad",
        //     "onlineOnly": false
        // },
         {
             "code": "JOU",
             "name": "Journey into Nyx",
             "type": "expansion",
             "releaseDate": "2014-05-02",
             "block": "Theros",
             "onlineOnly": false
         },
        // {
        //     "code": "JVC",
        //     "name": "Duel Decks Anthology: Jace vs. Chandra",
        //     "type": "duel_deck",
        //     "releaseDate": "2014-12-05",
        //     "onlineOnly": false
        // },
         {
             "code": "KLD",
             "name": "Kaladesh",
             "type": "expansion",
             "releaseDate": "2016-09-30",
             "block": "Kaladesh",
             "onlineOnly": false
         },
        // {
        //     "code": "KTK",
        //     "name": "Khans of Tarkir",
        //     "type": "expansion",
        //     "releaseDate": "2014-09-26",
        //     "block": "Khans of Tarkir",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L12",
        //     "name": "League Tokens 2012",
        //     "type": "token",
        //     "releaseDate": "2012-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L13",
        //     "name": "League Tokens 2013",
        //     "type": "token",
        //     "releaseDate": "2013-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L14",
        //     "name": "League Tokens 2014",
        //     "type": "token",
        //     "releaseDate": "2014-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L15",
        //     "name": "League Tokens 2015",
        //     "type": "token",
        //     "releaseDate": "2015-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L16",
        //     "name": "League Tokens 2016",
        //     "type": "token",
        //     "releaseDate": "2016-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "L17",
        //     "name": "League Tokens 2017",
        //     "type": "token",
        //     "releaseDate": "2017-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "LGN",
        //     "name": "Legions",
        //     "type": "expansion",
        //     "releaseDate": "2003-02-03",
        //     "block": "Onslaught",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "LRW",
        //     "name": "Lorwyn",
        //     "type": "expansion",
        //     "releaseDate": "2007-10-12",
        //     "block": "Lorwyn",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "M10",
        //     "name": "Magic 2010",
        //     "type": "core",
        //     "releaseDate": "2009-07-17",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "M11",
        //     "name": "Magic 2011",
        //     "type": "core",
        //     "releaseDate": "2010-07-16",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "M12",
        //     "name": "Magic 2012",
        //     "type": "core",
        //     "releaseDate": "2011-07-15",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "M13",
        //     "name": "Magic 2013",
        //     "type": "core",
        //     "releaseDate": "2012-07-13",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
         {
             "code": "M14",
             "name": "Magic 2014",
             "type": "core",
             "releaseDate": "2013-07-19",
             "block": "Core Set",
             "onlineOnly": false
         },
        // {
        //     "code": "M15",
        //     "name": "Magic 2015",
        //     "type": "core",
        //     "releaseDate": "2014-07-18",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        {
            "code": "M19",
            "name": "Core Set 2019",
            "type": "core",
            "releaseDate": "2018-07-13",
            "block": "Core Set",
            "onlineOnly": false
        },
        {
            "code": "M20",
            "name": "Core Set 2020",
            "type": "core",
            "releaseDate": "2019-07-12",
            "block": "Core Set",
            "onlineOnly": false
        },
         {
             "code": "MBS",
             "name": "Mirrodin Besieged",
             "type": "expansion",
             "releaseDate": "2011-02-04",
             "block": "Scars of Mirrodin",
             "onlineOnly": false
         },
        // {
        //     "code": "MD1",
        //     "name": "Modern Event Deck 2014",
        //     "type": "box",
        //     "releaseDate": "2014-05-30",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "ME1",
        //     "name": "Masters Edition",
        //     "type": "masters",
        //     "releaseDate": "2007-09-10",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "ME2",
        //     "name": "Masters Edition II",
        //     "type": "masters",
        //     "releaseDate": "2008-09-22",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "ME3",
        //     "name": "Masters Edition III",
        //     "type": "masters",
        //     "releaseDate": "2009-09-07",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "ME4",
        //     "name": "Masters Edition IV",
        //     "type": "masters",
        //     "releaseDate": "2011-01-10",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "MED",
        //     "name": "Mythic Edition",
        //     "type": "masterpiece",
        //     "releaseDate": "2018-10-05",
        //     "onlineOnly": false
        // },
        {
            "code": "MH1",
            "name": "Modern Horizons",
            "type": "draft_innovation",
            "releaseDate": "2019-06-14",
            "onlineOnly": false
        },
        // {
        //     "code": "MM2",
        //     "name": "Modern Masters 2015",
        //     "type": "masters",
        //     "releaseDate": "2015-05-22",
        //     "onlineOnly": false
        // },
         {
             "code": "MM3",
             "name": "Modern Masters 2017",
             "type": "masters",
             "releaseDate": "2017-03-17",
             "onlineOnly": false
         },
        // {
        //     "code": "MMA",
        //     "name": "Modern Masters",
        //     "type": "masters",
        //     "releaseDate": "2013-06-07",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "MOR",
        //     "name": "Morningtide",
        //     "type": "expansion",
        //     "releaseDate": "2008-02-01",
        //     "block": "Lorwyn",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "MP2",
        //     "name": "Amonkhet Invocations",
        //     "type": "masterpiece",
        //     "releaseDate": "2017-04-28",
        //     "block": "Amonkhet",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "MPS",
        //     "name": "Kaladesh Inventions",
        //     "type": "masterpiece",
        //     "releaseDate": "2016-09-30",
        //     "block": "Kaladesh",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "MRD",
        //     "name": "Mirrodin",
        //     "type": "expansion",
        //     "releaseDate": "2003-10-02",
        //     "block": "Mirrodin",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "NPH",
        //     "name": "New Phyrexia",
        //     "type": "expansion",
        //     "releaseDate": "2011-05-13",
        //     "block": "Scars of Mirrodin",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OARC",
        //     "name": "Archenemy Schemes",
        //     "type": "archenemy",
        //     "releaseDate": "2010-06-18",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC13",
        //     "name": "Commander 2013 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2013-11-01",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC14",
        //     "name": "Commander 2014 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2014-11-07",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC15",
        //     "name": "Commander 2015 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2015-11-13",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC16",
        //     "name": "Commander 2016 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2016-11-11",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC17",
        //     "name": "Commander 2017 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2017-08-25",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OC18",
        //     "name": "Commander 2018 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2018-08-09",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OCM1",
        //     "name": "Commander's Arsenal Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2012-11-02",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OCMD",
        //     "name": "Commander 2011 Oversized",
        //     "type": "memorabilia",
        //     "releaseDate": "2011-06-17",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OE01",
        //     "name": "Archenemy: Nicol Bolas Schemes",
        //     "type": "archenemy",
        //     "releaseDate": "2017-06-16",
        //     "onlineOnly": false
        // },
         {
             "code": "OGW",
             "name": "Oath of the Gatewatch",
             "type": "expansion",
             "releaseDate": "2016-01-22",
             "block": "Battle for Zendikar",
             "onlineOnly": false
         },
        // {
        //     "code": "OHOP",
        //     "name": "Planechase Planes",
        //     "type": "planechase",
        //     "releaseDate": "2009-09-04",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OLGC",
        //     "name": "Legacy Championship",
        //     "type": "memorabilia",
        //     "releaseDate": "2011-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OPC2",
        //     "name": "Planechase 2012 Planes",
        //     "type": "planechase",
        //     "releaseDate": "2012-06-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OPCA",
        //     "name": "Planechase Anthology Planes",
        //     "type": "planechase",
        //     "releaseDate": "2018-12-25",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "ORI",
        //     "name": "Magic Origins",
        //     "type": "core",
        //     "releaseDate": "2015-07-17",
        //     "block": "Core Set",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "OVNT",
        //     "name": "Vintage Championship",
        //     "type": "memorabilia",
        //     "releaseDate": "2003-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PARC",
        //     "name": "Promotional Schemes",
        //     "type": "archenemy",
        //     "releaseDate": "2010-06-18",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PC2",
        //     "name": "Planechase 2012",
        //     "type": "planechase",
        //     "releaseDate": "2012-06-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PCA",
        //     "name": "Planechase Anthology",
        //     "type": "planechase",
        //     "releaseDate": "2016-11-25",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PCMD",
        //     "name": "Commander 2011 Launch Party",
        //     "type": "memorabilia",
        //     "releaseDate": "2011-06-17",
        //     "block": "Commander",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PD2",
        //     "name": "Premium Deck Series: Fire and Lightning",
        //     "type": "premium_deck",
        //     "releaseDate": "2010-11-19",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PD3",
        //     "name": "Premium Deck Series: Graveborn",
        //     "type": "premium_deck",
        //     "releaseDate": "2011-11-18",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PHEL",
        //     "name": "Open the Helvault",
        //     "type": "memorabilia",
        //     "releaseDate": "2012-04-28",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PHUK",
        //     "name": "Hachette UK",
        //     "type": "box",
        //     "releaseDate": "2006-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PLC",
        //     "name": "Planar Chaos",
        //     "type": "expansion",
        //     ],
        //     "releaseDate": "2007-02-02",
        //     "block": "Time Spiral",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PMOA",
        //     "name": "Magic Online Avatars",
        //     "type": "vanguard",
        //     "releaseDate": "2003-01-01",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "PPC1",
        //     "name": "M15 Prerelease Challenge",
        //     "type": "memorabilia",
        //     "releaseDate": "2014-07-12",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PS11",
        //     "name": "Salvat 2011",
        //     "type": "box",
        //     "releaseDate": "2011-01-01",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PSAL",
        //     "name": "Salvat 2005",
        //     "type": "box",
        //     "releaseDate": "2005-08-22",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PUMA",
        //     "name": "Ultimate Box Topper",
        //     "type": "masterpiece",
        //     "releaseDate": "2018-12-07",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "PZ1",
        //     "name": "Legendary Cube Prize Pack",
        //     "type": "treasure_chest",
        //     "releaseDate": "2015-11-18",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "PZ2",
        //     "name": "Treasure Chest",
        //     "type": "treasure_chest",
        //     "releaseDate": "2016-11-16",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "RAV",
        //     "name": "Ravnica: City of Guilds",
        //     "type": "expansion",
        //     "releaseDate": "2005-10-07",
        //     "block": "Ravnica",
        //     "onlineOnly": false
        // },
        {
            "code": "RIX",
            "name": "Rivals of Ixalan",
            "type": "expansion",
            "releaseDate": "2018-01-19",
            "block": "Ixalan",
            "onlineOnly": false
        },
        {
            "code": "RNA",
            "name": "Ravnica Allegiance",
            "type": "expansion",
            "releaseDate": "2019-01-25",
            "block": "Guilds of Ravnica",
            "onlineOnly": false
        },
        // {
        //     "code": "ROE",
        //     "name": "Rise of the Eldrazi",
        //     "type": "expansion",
        //     "releaseDate": "2010-04-23",
        //     "block": "Zendikar",
        //     "onlineOnly": false
        // },
         {
             "code": "RTR",
             "name": "Return to Ravnica",
             "type": "expansion",
             "releaseDate": "2012-10-05",
             "block": "Return to Ravnica",
             "onlineOnly": false
         },
        // {
        //     "code": "SCG",
        //     "name": "Scourge",
        //     "type": "expansion",
        //     "releaseDate": "2003-05-26",
        //     "block": "Onslaught",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "SHM",
        //     "name": "Shadowmoor",
        //     "type": "expansion",
        //     "releaseDate": "2008-05-02",
        //     "block": "Shadowmoor",
        //     "onlineOnly": false
        // },
         {
             "code": "SOI",
             "name": "Shadows over Innistrad",
             "type": "expansion",
             "releaseDate": "2016-04-08",
             "block": "Shadows Over Innistrad",
             "onlineOnly": false
         },
        // {
        //     "code": "SOK",
        //     "name": "Saviors of Kamigawa",
        //     "type": "expansion",
        //     "releaseDate": "2005-06-03",
        //     "block": "Kamigawa",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "SOM",
        //     "name": "Scars of Mirrodin",
        //     "type": "expansion",
        //     "releaseDate": "2010-10-01",
        //     "block": "Scars of Mirrodin",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "TBTH",
        //     "name": "Battle the Horde",
        //     "type": "token",
        //     "releaseDate": "2014-03-01",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "TD0",
        //     "name": "Magic Online Theme Decks",
        //     "type": "box",
        //     "releaseDate": "2010-11-08",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "TD2",
        //     "name": "Duel Decks: Mirrodin Pure vs. New Phyrexia",
        //     "type": "duel_deck",
        //     "releaseDate": "2011-05-14",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "TDAG",
        //     "name": "Defeat a God",
        //     "type": "token",
        //     "releaseDate": "2014-05-25",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "TFTH",
        //     "name": "Face the Hydra",
        //     "type": "token",
        //     "releaseDate": "2013-10-19",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "THP1",
        //     "name": "Theros Hero's Path",
        //     "type": "token",
        //     "releaseDate": "2013-09-27",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "THP2",
        //     "name": "Born of the Gods Hero's Path",
        //     "type": "token",
        //     "releaseDate": "2014-02-07",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "THP3",
        //     "name": "Journey into Nyx Hero's Path",
        //     "type": "token",
        //     "releaseDate": "2014-05-02",
        //     "block": "Theros",
        //     "onlineOnly": false
        // },
         {
             "code": "THS",
             "name": "Theros",
             "type": "expansion",
             "releaseDate": "2013-09-27",
             "block": "Theros",
             "onlineOnly": false
         },
        // {
        //     "code": "TPR",
        //     "name": "Tempest Remastered",
        //     "type": "masters",
        //     "releaseDate": "2015-05-06",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "TSB",
        //     "name": "Time Spiral Timeshifted",
        //     "type": "expansion",
        //     "releaseDate": "2006-10-06",
        //     "block": "Time Spiral",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "TSP",
        //     "name": "Time Spiral",
        //     "type": "expansion",
        //     "releaseDate": "2006-10-06",
        //     "block": "Time Spiral",
        //     "onlineOnly": false
        // },
         {
             "code": "UMA",
             "name": "Ultimate Masters",
             "type": "masters",
             "releaseDate": "2018-12-07",
             "onlineOnly": false
         },
        // {
        //     "code": "VMA",
        //     "name": "Vintage Masters",
        //     "type": "masters",
        //     "releaseDate": "2014-06-16",
        //     "onlineOnly": true
        // },
        // {
        //     "code": "W16",
        //     "name": "Welcome Deck 2016",
        //     "type": "starter",
        //     "releaseDate": "2016-04-08",
        //     "onlineOnly": false
        // },
        // {
        //     "code": "W17",
        //     "name": "Welcome Deck 2017",
        //     "type": "starter",
        //     "releaseDate": "2017-04-15",
        //     "onlineOnly": false
        // },
        {
            "code": "WAR",
            "name": "War of the Spark",
            "type": "expansion",
            "releaseDate": "2019-05-03",
            "block": "Guilds of Ravnica",
            "onlineOnly": false
        },
        // {
        //     "code": "WWK",
        //     "name": "Worldwake",
        //     "type": "expansion",
        //     "releaseDate": "2010-02-05",
        //     "block": "Zendikar",
        //     "onlineOnly": false
        // },
        {
            "code": "XLN",
            "name": "Ixalan",
            "type": "expansion",
            "releaseDate": "2017-09-29",
            "block": "Ixalan",
            "onlineOnly": false
        },
        // {
        //     "code": "ZEN",
        //     "name": "Zendikar",
        //     "type": "expansion",
        //     "releaseDate": "2009-10-02",
        //     "block": "Zendikar",
        //     "onlineOnly": false
        // }
    ];
    
    return store.map((item) => ({ value: item["code"], name: item["name"] }));

}


export function GetRarityFilters(): FilterOption[] {
    const rarityFilters: FilterOption[] = [
        // { name: "All", value: "" },
        // { name: "", value: "" },
        // "Mythic|Rare",
        // { name: "", value: "" },
        // "Uncommon|Common",
        { name: "Mythic", value: "mythic" },
        { name: "Rare", value: "rare" },
        { name: "Uncommon", value: "uncommon" },
        { name: "Common", value: "common" },
    ];
    return rarityFilters;
}

export function GetTypeFilters(): FilterOption[] {
    const typeFilters: FilterOption[] = [
        { name: "", value: "" },
        { name: "Creature", value: "Creature" },
        { name: "Instant", value: "Instant" },
        { name: "Sorcery", value: "Sorcery" },
        { name: "Enchantment", value: "Enchantment" },
        { name: "Artifact", value: "Artifact" },
        { name: "Planeswalker", value: "Planeswalker" },
        { name: "Land", value: "Land" },
        { name: "Legendary", value: "Legendary" },
    ];
    return typeFilters;
}

export function GetColorFilters(): FilterOption[] {
    const colors: FilterOption[] = [
        { name: "Red", value: "R" },
        { name: "Blue", value: "U" },
        { name: "Green", value: "G" },
        { name: "White", value: "W" },
        { name: "Black", value: "B" },
    ];
    return colors;
}



