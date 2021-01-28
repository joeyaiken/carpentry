using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests.SnowDeckTests
{
    [TestClass]
    public class SnowDeckTests
    {
        //constructor
        readonly CarpentryFactory _factory;
        readonly HttpClient _client;

        public SnowDeckTests()
        {
            _factory = new CarpentryFactory();
            _client = _factory.CreateClient();
        }

        //


        /* End-to-end test idea
            Clear DB
                Should export current DB to a local folder everytime an export happens
            Set base tracked sets
                refresh list
                add (those 4 snow sets)
                    khm
                    mh1
                    csp
                    ice

            Import Jorn Snow deck, fully empty
                validate tags got created in addition to empty cards
                Inventory should be empty at that point
            Create the bant snow deck
                Import chulane deck, adding new deck cards
                Clone the chulane deck
                Dissasemble the original
            Rename the clone, change to EDH, set snow desc
         */

        [TestMethod]
        public async Task SnowDeck_EndToEnd_Works()
        {
            //In this scenario, do I want to reset the DB everytime I run the test?
            //  Why not...the goal is to just ensure I can run this test on a test DB
            //  DB should be swapped to sqlite

            //await ResetDatabase();

            //Set base tracked sets (khm,mh1,csp,ice)
            //await TrackSnowSets();

            //Import Jorn Snow deck, fully empty
            //await ImportDeckJornSnow();
            //    validate tags got created in addition to empty cards
            //    Inventory should be empty at that point


            //Create the bant snow deck

            //Import chulane deck, adding new deck cards
            var chulaneDeckId = 2;// await ImportDeckChulane(); // 2

            //Clone the chulane deck
            var clonedDeckId = 3; // await ApiCloneDeck(chulaneDeckId);

            //Dissasemble the original
            //await ApiDissassembleDeck(chulaneDeckId);

            //Rename the clone, change to EDH, set snow desc

            //get the chulane deck (props?)
            var chulaneDeck = await ApiGetDeckDetail(clonedDeckId);

            //update chulane deck props
            chulaneDeck.Props.Name = "";
            chulaneDeck.Props.Format = "";
            chulaneDeck.Props.Notes = "";

            await ApiUpdateDeckProps(chulaneDeck.Props);

            //start asserting things
        }

        private async Task ResetDatabase()
        {

            //ensure DB exists
            //await ApiValidateDatabase();

            //Ensure not production
            //  (not sure how to accomplish this yet, could just ensure backed up instead)

            //Clear DB
            //    Should export current DB to a local folder everytime an export happens

            throw new NotImplementedException();
        }

        private async Task TrackSnowSets()
        {
            //Set base tracked sets
            
            //get list, update, include untracked
            var allSets = await ApiGetSets();

            //add (those 4 snow sets)

            //khm
            var kaldheim = allSets.First(s => s.Code == "khm");
            await ApiAddTrackedSet(kaldheim.SetId);
            //mh1
            var modernhorizons = allSets.First(s => s.Code == "mh1");
            await ApiAddTrackedSet(modernhorizons.SetId);
            //csp
            var coldsnap = allSets.First(s => s.Code == "csp");
            await ApiAddTrackedSet(coldsnap.SetId);
            //ice
            var iceage = allSets.First(s => s.Code == "ice");
            await ApiAddTrackedSet(iceage.SetId);
        }

        private async Task ImportDeckJornSnow()
        {
            var deckString = @"
1 Abominable Treefolk (mh1) 194 {Snow%2CStompy}
1 Adarkar Windform (csp) 26 {Snow}
1 Arcum's Astrolabe (mh1) 220 {Snow%2CDraw}
1 Ascendant Spirit (khm) 43 {Snow%2CDraw}
1 Avalanche Caller (khm) 45
1 Balduvian Frostwaker (csp) 28
1 Berg Strider (khm) 47
1 Blessing of Frost (khm) 161
1 Blizzard Brawl (khm) 162 {Removal%2CProtection}
1 Blizzard Specter (csp) 126
1 Blizzard Strix (mh1) 42
1 Blood on the Snow (khm) 79 {Snow+Support%2CBoard+Wipe}
1 Boreal Centaur (csp) 104
1 Boreal Druid (csp) 105 {Ramp}
1 Boreal Outrider (khm) 163
1 Chillerpillar (mh1) 43
1 Chilling Shade (csp) 53
1 Coldsteel Heart (csp) 136 {Ramp%2CSnow}
1 Conifer Wurm (mh1) 159 {Stompy}
1 Dark Depths (csp) 145 {Land}
1 Dead of Winter (mh1) 85 {Board+Wipe}
1 Draugr Necromancer (khm) 86 {Snow}
1 Faceless Haven (khm) 255 {Land}
1 Forest
1 Frost Augur (khm) 56 {Draw%2CSnow}
1 Frost Marsh (csp) 146 {Land}
1 Frost Raptor (csp) 34 {Snow}
1 Frostpeak Yeti (khm) 57 {Snow}
1 Frostwalk Bastion (mh1) 240 {Land%2CSnow}
1 Frostwalla (mh1) 165
1 Frostweb Spider (csp) 109
1 Glacial Revelation (mh1) 167 {Draw}
1 Glittering Frost (khm) 171 {Ramp}
1 Graven Lore (khm) 61 {Draw}
1 Grim Draugr (khm) 96
1 Gutless Ghoul (csp) 60
1 Hailstorm Valkyrie (khm) 97 {Stompy}
1 Heidar, Rimewind Master (csp) 36 {Removal}
1 Ice Tunnel (khm) 262 {Land}
1 Iceberg Cancrix (mh1) 54 {Snow%2CMill}
1 Icebind Pillar (khm) 62
1 Icebreaker Kraken (khm) 63
1 Ice-Fang Coatl (mh1) 203 {Draw%2CRemoval}
1 Icehide Golem (mh1) 224
1 Icehide Troll (khm) 176
1 Into the North (csp) 111 {Ramp}
1 Island
1 Marit Lage's Slumber (mh1) 56
1 Moritte of the Frost (khm) 223 {Snow}
1 Mouth of Ronom (csp) 148 {Land}
1 Narfi, Betrayer King (khm) 224
1 Ohran Viper (csp) 115
1 Phyrexian Ironfoot (csp) 139
1 Phyrexian Snowcrusher (csp) 140
1 Phyrexian Soulgorger (csp) 141
1 Pilfering Hawk (khm) 71
1 Priest of the Haunted Edge (khm) 104
1 Replicating Ring (khm) 244 {Ramp%2CSnow}
1 Rime Tender (mh1) 176
1 Rime Transfusion (csp) 68
1 Rimebound Dead (csp) 69
1 Rimefeather Owl (csp) 42
1 Rimehorn Aurochs (csp) 118
1 Rimewind Cryomancer (csp) 43
1 Rimewind Taskmage (csp) 44
1 Rimewood Falls (khm) 266 {Land}
1 Ronom Hulk (csp) 119
1 Ronom Serpent (csp) 45
1 Saddled Rimestag (mh1) 177
1 Scrying Sheets (csp) 149 {Land}
1 Sculptor of Winter (khm) 193 {Snow%2CRamp}
1 Shimmerdrift Vale (khm) 267
8 Snow-Covered Forest (khm) 284 {Land%2CSnow}
8 Snow-Covered Island (khm) 278 {Land%2CSnow}
8 Snow-Covered Swamp (khm) 280 {Land%2CSnow}
1 Spirit of the Aldergard (khm) 195
1 Swamp
1 The Three Seasons (khm) 231
1 Winter's Rest (mh1) 78 {Removal}
1 Withering Wisps (ice) 168
1 Woodland Chasm (khm) 274 {Snow%2CLand}

Commander
1 Jorn, God of Winter // Kaldring, the Rimestaff (khm) 179 {Snow%2CSnow+Support}

Sideboard
1 Chill to the Bone (csp) 52
1 Drift of the Dead (ice) 123
1 Freyalise's Radiance (csp) 108
1 Sunstone (ice) 341
1 Thermal Flux (csp) 49
1 Zombie Musher (csp) 75
";
            var dto = new CardImportDto() { ImportPayload = deckString };
            var validationResult = await ApiValidateImport(dto);

            Assert.AreEqual(0, validationResult.UntrackedSets?.Count);

            Assert.IsTrue(validationResult.IsValid);



            validationResult.DeckProps.Name = "You Know Nothing";
            validationResult.DeckProps.Format = "commander";
            validationResult.DeckProps.Notes = "Jorn Snow";

            var newId = await ApiAddValidatedImport(validationResult);
            Assert.IsNotNull(newId); //Wait, it's a non-nullable int, would this ever fail?
            Assert.AreNotEqual(0, newId);
        }

        private async Task<int> ImportDeckChulane()
        {
            var deckString = @"
1 Arcane Signet (eld) 331
1 Azorius Guildgate (rna) 243
1 Beanstalk Giant // Fertile Footsteps (eld) 149
1 Biomancer's Familiar (rna) 158
1 Blossoming Sands (m20) 243
1 Circuitous Route (grn) 125
1 Command Tower (eld) 333
1 District Guide (grn) 128
1 End-Raze Forerunners (rna) 124
1 Evolving Wilds (rix) 186
1 Faerie Formation (eld) 316
1 Faerie Vandal (eld) 45
1 Firemind Vessel (war) 237
1 Flower // Flourish (grn) 226
1 Forbidding Spirit (rna) 9
6 Forest
1 Frilled Mystic (rna) 174
1 Growth Spiral (rna) 178
1 Gyre Engineer (rna) 180
1 Hallowed Fountain (rna) 251
1 Incubation // Incongruity (rna) 226
1 Incubation Druid (rna) 131
4 Island
1 Keeper of Fables (eld) 163
1 Kraul Harpooner (grn) 136
1 Leafkin Druid (m20) 178
1 Maraleaf Pixie (eld) 196
1 Meteor Golem (m19) 241
1 Paradise Druid (war) 171
1 Parhelion II (war) 24
5 Plains
1 Prison Realm (war) 26
1 Risen Reef (m20) 217
1 Rosethorn Acolyte // Seasonal Ritual (eld) 174
1 Run Away Together (eld) 62
1 Selesnya Guildgate (grn) 255
1 Sharktocrab (rna) 206
1 Silhana Wayfinder (rna) 141
1 Spectral Sailor (m20) 76
1 Simic Guildgate (rna) 257
1 Steelbane Hydra (eld) 322
1 Temple of Mystery (m20) 255
1 Thorn Mammoth (eld) 323
1 Thornwood Falls (m20) 258
1 Time Wipe (war) 223
1 Tome of Legends (eld) 332
1 Tranquil Cove (m20) 259

Commander
1 Chulane, Teller of Tales (eld) 326 FOIL
";
            var dto = new CardImportDto() { ImportPayload = deckString };
            var validationResult = await ApiValidateImport(dto);

            if (validationResult.UntrackedSets?.Count > 0)
            {
                foreach (var set in validationResult.UntrackedSets)
                {
                    await ApiAddTrackedSet(set.SetId);
                }
                validationResult = await ApiValidateImport(dto);
            }

            validationResult.DeckProps.Name = "Wild Bounty";
            validationResult.DeckProps.Format = "brawl";
            validationResult.DeckProps.Notes = "Chulane brawl precon";

            //Assert.IsTrue(validationResult.UntrackedSets?.Count > 0);

            var newId = await ApiAddValidatedImport(validationResult);
            return newId;
        }


        private async Task<List<SetDetailDto>> ApiGetSets()
        {
            //public async Task<ActionResult<List<SetDetailDto>>> GetTrackedSets(bool showUntracked, bool update = false)

            var apiEndpoint = $"api/Core/GetTrackedSets?showUntracked=true&update=true";

            ////var client = _factory.CreateClient();

            //var queryParamStringBody = JsonConvert.SerializeObject(importPayload, Formatting.None);

            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.GetAsync(apiEndpoint);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<SetDetailDto>>(responseContent);

            return result;
        }

        private async Task ApiAddTrackedSet(int setId)
        {
            var apiEndpoint = $"api/Core/AddTrackedSet?setId={setId}";
            var response = await _client.GetAsync(apiEndpoint);
            // Assert that call was successful?
        }

        private async Task ApiValidateDatabase()
        {
            var apiEndpoint = $"api/Core/ValidateDatabase";
            var response = await _client.GetAsync(apiEndpoint);
            // Assert that call was successful?
        }

        private async Task<ValidatedDeckImportDto> ApiValidateImport(CardImportDto importPayload)
        {
            var apiEndpoint = "api/Decks/ValidateDeckImport";

            //var client = _factory.CreateClient();

            var queryParamStringBody = JsonConvert.SerializeObject(importPayload, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiEndpoint, queryParamStringContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ValidatedDeckImportDto>(responseContent);

            return result;
        }
        
        private async Task<int> ApiAddValidatedImport(ValidatedDeckImportDto validatedPayload)
        {
            var apiEndpoint = "api/Decks/AddValidatedDeckImport";

            //var client = _factory.CreateClient();

            var queryParamStringBody = JsonConvert.SerializeObject(validatedPayload, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiEndpoint, queryParamStringContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<int>(responseContent);

            return result;
        }

        private async Task<int> ApiCloneDeck(int deckId)
        {
            var apiEndpoint = $"api/Decks/CloneDeck?deckId={deckId}";

            var response = await _client.GetAsync(apiEndpoint);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<int>(responseContent);

            return result;
        }

        private async Task ApiDissassembleDeck(int deckId)
        {
            var apiEndpoint = $"api/Decks/DissassembleDeck?deckId={deckId}";

            var response = await _client.GetAsync(apiEndpoint);

            //var responseContent = await response.Content.ReadAsStringAsync();

            //var result = JsonConvert.DeserializeObject<int>(responseContent);

        }

        private async Task<DeckDetailDto> ApiGetDeckDetail(int deckId)
        {
            var apiEndpoint = $"api/Decks/GetDeckDetail?deckId={deckId}";

            var response = await _client.GetAsync(apiEndpoint);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<DeckDetailDto>(responseContent);

            return result;
        }

        private async Task ApiUpdateDeckProps(DeckPropertiesDto dto)
        {
            var apiEndpoint = "api/Decks/UpdateDeck";

            //var client = _factory.CreateClient();

            var queryParamStringBody = JsonConvert.SerializeObject(dto, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiEndpoint, queryParamStringContent);

            //var responseContent = await response.Content.ReadAsStringAsync();

            //var result = JsonConvert.DeserializeObject<int>(responseContent);

            //return result;
        }

        //private static async Task<string> ReadFileContents(string directory)
        //{
        //    string fileContents = await File.ReadAllTextAsync(directory);
        //    return fileContents;
        //}


    }
}
