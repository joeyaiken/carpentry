using Carpentry.CarpentryData;
//using Carpentry.Data.LegacyDataContext;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Backups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    /*
    I need to redesign the card backup process to account for Deck Cards potentially being empty (not having a designated Inventory Card)
    Deck Cards will now need to be a child of the deck backup itself, inventory card backups won't have info about if they belong to a deck
    */



    /// <summary>
    /// This class contains the logic for saving relevant DB contents to a text file, or exporting as a zip file
    /// </summary>
    public class DataExportService : IDataExportService
    {
        private readonly ILogger<DataExportService> _logger;
        private readonly CarpentryDataContext _cardContext;
        private readonly CompressionLevel _exportCompressionLevel;
        private readonly IDataBackupConfig _config;

        public DataExportService(
            ILogger<DataExportService> logger,
            CarpentryDataContext cardContext,
            IDataBackupConfig config
            )
        {
            _logger = logger;
            _cardContext = cardContext;
            _config = config;
            _exportCompressionLevel = CompressionLevel.Fastest;
        }

        //public async Task BackupCollection_New(string directory)
        //{
        //    _logger.LogInformation("DataBackupService - BackupCollectionToDirectory...");

        //    if (string.IsNullOrEmpty(directory))
        //    {
        //        throw new ArgumentNullException("Directory cannot be blank");
        //    }

        //    string deckBackupFilepath = $"{directory}{_config.DeckBackupFilename}";
        //    string cardBackupFilepath = $"{directory}{_config.CardBackupFilename}";
        //    string propsBackupFilepath = $"{directory}{_config.PropsBackupFilename}";

        //    var deckBackupObj = await GetDeckBackups();
        //    var cardBackupObj = await GetCardBackups();
        //    var propsBackupObj = await GetBackupProps();

        //    await File.WriteAllTextAsync(deckBackupFilepath, deckBackupObj.ToString());
        //    await File.WriteAllTextAsync(cardBackupFilepath, cardBackupObj.ToString());
        //    await File.WriteAllTextAsync(propsBackupFilepath, propsBackupObj.ToString());

        //    _logger.LogInformation("DataBackupService - BackupCollectionToDirectory...completed successfully");
        //}




        #region legacy public methods

        //Method called by Quick Backup Tool
        public async Task BackupCollectionToDirectory(string directory)
        {
            _logger.LogInformation("DataBackupService - BackupCollectionToDirectory...");

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException("Directory cannot be blank");
            }

            string deckBackupFilepath = $"{directory}{_config.DeckBackupFilename}";
            string cardBackupFilepath = $"{directory}{_config.CardBackupFilename}";
            string propsBackupFilepath = $"{directory}{_config.PropsBackupFilename}";

            var deckBackupObj = await GetDeckBackups();
            var cardBackupObj = await GetCardBackups();
            var propsBackupObj = await GetBackupProps();

            await File.WriteAllTextAsync(deckBackupFilepath, deckBackupObj.ToString());
            await File.WriteAllTextAsync(cardBackupFilepath, cardBackupObj.ToString());
            await File.WriteAllTextAsync(propsBackupFilepath, propsBackupObj.ToString());

            _logger.LogInformation("DataBackupService - BackupCollectionToDirectory...completed successfully");
        }

        //For how I did this: https://stackoverflow.com/questions/53378497/net-core-create-on-memory-zipfile
        public async Task<byte[]> GenerateZipBackup()
        {
            byte[] backupFile;

            using(var archiveStream = new MemoryStream())
            {
                using(var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    //Decks
                    var decksEntry = archive.CreateEntry(_config.DeckBackupFilename, _exportCompressionLevel);
                    using (var zipStream = decksEntry.Open())
                    {
                        var deckBackupObj = await GetDeckBackups();
                        byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                        zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    }

                    //Cards
                    var cardsEntry = archive.CreateEntry(_config.CardBackupFilename, _exportCompressionLevel);
                    using (var zipStream = cardsEntry.Open())
                    {
                        var deckBackupObj = await GetDeckBackups();
                        byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                        zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    }

                    //Backup Props
                    var propsEntry = archive.CreateEntry(_config.PropsBackupFilename, _exportCompressionLevel);
                    using (var zipStream = propsEntry.Open())
                    {
                        var deckBackupObj = await GetDeckBackups();
                        byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                        zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    }
                }

                backupFile = archiveStream.ToArray();
            }

            return backupFile;
        }


        //public async Task<string> GetDeckListExport(int deckId)
        //{
        //    //Format: <amount> <Card Name> (<Set>) <Collector Number>
        //    //Groups: Companion/Commander, Deck, Sideboard
        //    var deckCards = await _cardContext.DeckCards
        //        .Where(dc => dc.DeckId == deckId)
        //        .Select(c => new
        //        {
        //            CardString = $"{c.InventoryCard.Card.Name} ({c.InventoryCard.Card.Set.Code}) {c.InventoryCard.Card.CollectorNumber}" ,
        //            //c.InventoryCard.Card.Name,
        //            //c.InventoryCard.Card.Set.Code,
        //            //c.InventoryCard.Card.CollectorNumber,
        //            Category = c.Category.Name ?? "Deck",
        //        }).ToListAsync();

        //    var fullStrings = deckCards
        //        .GroupBy(dc => new
        //        {
        //            dc.Category,
        //            dc.CardString
        //        })
        //        .Select(g => new
        //        {
        //            g.Key.Category,
        //            CardString = $"{g.Count()} {g.Key.CardString}",
        //        }).ToList();

        //    var groupedStrings = fullStrings
        //        .GroupBy(g => g.Category)
        //        .Select(g => new
        //        {
        //            Category = g.Key,
        //            Cards = g.Select(i => i.CardString),
        //        })
        //        .ToList();

        //    var exportList = new List<string>();

        //    foreach(var group in groupedStrings)
        //    {
        //        exportList.Add(group.Category);
        //        exportList.AddRange(group.Cards);
        //        exportList.Add("");
        //    }

        //    var result = string.Join('\n', exportList);

        //    return result;
        //}

        #endregion

        #region non-public methods

        private async Task<JArray> GetDeckBackups()
        {

            var deckExports = (await _cardContext.Decks.Select(d => new
            {
                Props = d,
                Format = d.Format.Name,
                Tags = d.Tags.ToList(),
                Cards = d.Cards.Select(dc => new BackupDeckCard
                {
                    Name = dc.CardName,
                    Category = dc.CategoryId,
                    InventoryCard = dc.InventoryCardId == null ? null : new BackupInventoryCard()
                    {
                        SetCode = dc.InventoryCard.Card.Set.Code,
                        CollectorNumber = dc.InventoryCard.Card.CollectorNumber,
                        InventoryCardStatusId = dc.InventoryCard.InventoryCardStatusId,
                        IsFoil = dc.InventoryCard.IsFoil,
                    },
                }).ToList(),

            }).ToListAsync())
            .Select(d => new BackupDeck
            {
                //ExportId = x.DeckId,
                Name = d.Props.Name,
                Format = d.Format,
                Notes = d.Props.Notes,
                BasicW = d.Props.BasicW,
                BasicU = d.Props.BasicU,
                BasicB = d.Props.BasicB,
                BasicR = d.Props.BasicR,
                BasicG = d.Props.BasicG,
                Cards = d.Cards,
                Tags = d.Tags.GroupBy(t => t.CardName).ToDictionary(g => g.Key, g => g.Select(t => t.Description).ToList()),
            }).OrderBy(x => x.Name).ToList();

            var result = JArray.FromObject(deckExports);

            return result;
        }

        private async Task<JArray> GetCardBackups()
        {
            //Need to query all inventory cards ////(with  included deck card info)
            //  This will no longer include deck card info

            var cardExports = await _cardContext.InventoryCards
                .Select(x => new BackupInventoryCard
                {
                    SetCode = x.Card.Set.Code,
                    CollectorNumber = x.Card.CollectorNumber,
                    
                    
                    //MultiverseId = x.MultiverseId,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    IsFoil = x.IsFoil,
                    //VariantName = x.VariantType.Name,
                    //DeckCards = x.DeckCards.Select(c => new BackupDeckCard
                    //{
                    //    DeckId = c.DeckId,
                    //    Category = c.CategoryId,
                    //}).ToList(),
                })
                //.OrderBy(x => x.MultiverseId)
                .OrderBy(x => x.SetCode)
                .ThenBy(x => x.CollectorNumber)
                .ToListAsync();

            var result = JArray.FromObject(cardExports);
            
            return result;
        }

        private async Task<JObject> GetBackupProps()
        {
            var allTrackedSetCodes = await _cardContext.InventoryCards
                .Where(x => x.Card.Set.IsTracked)
                .Select(x => x.Card.Set.Code).Distinct().OrderBy(x => x).ToListAsync();

            var backupProps = new BackupDataProps()
            {
                SetCodes = allTrackedSetCodes,
                TimeStamp = DateTime.Now,
            };

            var result = JObject.FromObject(backupProps);

            return result;
        }

        #endregion
    }
}
