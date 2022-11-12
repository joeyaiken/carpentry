using Carpentry.CarpentryData;
using Carpentry.Logic.Models.Backups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic
{
    public interface IDataExportService
    {
        Task BackupCollectionToDirectory(string directory);
        Task<byte[]> GenerateZipBackup();
    }

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

        //Method called by Quick Backup Tool
        public async Task BackupCollectionToDirectory(string directory)
        {
            _logger.LogInformation("DataBackupService - BackupCollectionToDirectory...");

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException("Directory cannot be blank");
            }

            var deckBackupFilepath = $"{directory}{_config.DeckBackupFilename}";
            var cardBackupFilepath = $"{directory}{_config.CardBackupFilename}";
            var propsBackupFilepath = $"{directory}{_config.PropsBackupFilename}";

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
                    throw new NotImplementedException(
                        "This had a bug in it's initial implementation, and should be re-evaluated");

                    // //Decks
                    // var decksEntry = archive.CreateEntry(_config.DeckBackupFilename, _exportCompressionLevel);
                    // using (var zipStream = decksEntry.Open())
                    // {
                    //     var deckBackupObj = await GetDeckBackups();
                    //     byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                    //     zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    // }
                    //
                    // //Cards
                    // var cardsEntry = archive.CreateEntry(_config.CardBackupFilename, _exportCompressionLevel);
                    // using (var zipStream = cardsEntry.Open())
                    // {
                    //     var deckBackupObj = await GetCardBackups();
                    //     byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                    //     zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    // }
                    //
                    // //Backup Props
                    // var propsEntry = archive.CreateEntry(_config.PropsBackupFilename, _exportCompressionLevel);
                    // using (var zipStream = propsEntry.Open())
                    // {
                    //     var deckBackupObj = await GetDeckBackups();
                    //     byte[] deckBackupContent = Encoding.ASCII.GetBytes(deckBackupObj.ToString());
                    //     zipStream.Write(deckBackupContent, 0, deckBackupContent.Length);
                    // }
                }

                backupFile = archiveStream.ToArray();
            }

            return backupFile;
        }

        private async Task<JArray> GetDeckBackups()
        {
            var deckData = await _cardContext.Decks.Select(d => new
            {
                Props = d,
                Format = d.Format.Name,
                Tags = d.Tags.ToList(),
                Cards = d.Cards.Select(dc => new BackupDeckCard
                {
                    Name = dc.CardName,
                    Category = dc.CategoryId,
                    InventoryCard = dc.InventoryCardId == null
                        ? null
                        : new BackupInventoryCard()
                        {
                            SetCode = dc.InventoryCard.Card.Set.Code,
                            CollectorNumberStr = dc.InventoryCard.Card.CollectorNumberStr,
                            InventoryCardStatusId = dc.InventoryCard.InventoryCardStatusId,
                            IsFoil = dc.InventoryCard.IsFoil,
                        },
                }).ToList(),
            }).ToListAsync();

            var deckExports = deckData
                .Select(d => new BackupDeck
                {
                    Name = d.Props.Name,
                    Format = d.Format,
                    Notes = d.Props.Notes,
                    BasicW = d.Props.BasicW,
                    BasicU = d.Props.BasicU,
                    BasicB = d.Props.BasicB,
                    BasicR = d.Props.BasicR,
                    BasicG = d.Props.BasicG,
                    Cards = d.Cards,
                    Tags = d.Tags.GroupBy(t => t.CardName)
                        .ToDictionary(g => g.Key, g => g.Select(t => t.Description).ToList()),
                }).OrderBy(x => x.Name).ToList();

            return JArray.FromObject(deckExports);
        }

        private async Task<JArray> GetCardBackups()
        {
            var cardExports = await _cardContext.InventoryCards
                .OrderBy(c => c.Card.Set.Code)
                .ThenBy(c => c.Card.CollectorNumber)
                .ThenBy(c => c.Card.CollectorNumberStr)
                .ThenBy(ic => ic.IsFoil)
                .Select(x => new BackupInventoryCard
                {
                    SetCode = x.Card.Set.Code,
                    CollectorNumberStr = x.Card.CollectorNumberStr,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    IsFoil = x.IsFoil,
                })
                // .OrderBy(x => x.SetCode)
                //.ThenBy(x => x.)
                // .ThenBy(x => x.CollectorNumberStr)
                .ToListAsync();

            return JArray.FromObject(cardExports);
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

            return JObject.FromObject(backupProps);
        }

    }
}
