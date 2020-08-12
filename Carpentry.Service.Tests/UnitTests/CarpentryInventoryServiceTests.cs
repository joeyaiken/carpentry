using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryInventoryServiceTests
    {
        #region Inventory Card add/update/delete
        //Task<int> AddInventoryCard(InventoryCardDto dto);
        //Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);
        //Task UpdateInventoryCard(InventoryCardDto dto);
        //Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch);
        //Task DeleteInventoryCard(int id);
        //Task DeleteInventoryCardBatch(IEnumerable<int> batch);

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCard_Test()
        {
            Assert.Fail();
            ////Assemble
            //int idToExpect = 1;

            //InventoryCardDto newCard = new InventoryCardDto()
            //{
            //    MultiverseId = 1,
            //    VariantName = "normal"
            //};

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardData>()))
            //    .ReturnsAsync(idToExpect);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //mockDataUpdateService
            //    .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i == newCard.MultiverseId)))
            //    .Returns(Task.CompletedTask);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //DataReferenceValue<int> expectedVariantType = new DataReferenceValue<int>() { Id = 1, Name = "normal" };

            //mockCoreRepo
            //    .Setup(p => p.GetCardVariantTypeByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(expectedVariantType);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //var newId = await inventoryService.AddInventoryCard(newCard);

            ////Assert
            //Assert.AreEqual(idToExpect, newId);
        }

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCardBatch_Test()
        {
            Assert.Fail();
            ////Assemble
            //List<InventoryCardDto> cardBatch = new List<InventoryCardDto>()
            //{
            //    new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            //    new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            //    new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            //    new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            //};

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardData>>()))
            //    .Returns(Task.CompletedTask);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //mockDataUpdateService
            //    .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i > 0)))
            //    .Returns(Task.CompletedTask);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //List<DataReferenceValue<int>> variantTypes = new List<DataReferenceValue<int>>()
            //{
            //    new DataReferenceValue<int>() { Id = 1, Name = "normal" },
            //    new DataReferenceValue<int>() { Id = 2, Name = "showcase" }
            //};

            //mockCoreRepo
            //    .Setup(p => p.GetAllCardVariantTypes())
            //    .ReturnsAsync(variantTypes);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //await inventoryService.AddInventoryCardBatch(cardBatch);

            ////Assert
            ////Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCard_Test()
        {
            Assert.Fail();
            ////Assemble
            //InventoryCardDto cardToUpdate = new InventoryCardDto()
            //{
            //    Id = 1,
            //    MultiverseId = 1,
            //    VariantName = "normal"
            //};

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardData>()))
            //    .Returns(Task.CompletedTask);

            //InventoryCardData inventoryDbCard = new InventoryCardData() { Id = 1 };

            //mockInventoryRepo
            //    .Setup(p => p.GetInventoryCardById(It.Is<int>(i => i == cardToUpdate.Id)))
            //    .ReturnsAsync(inventoryDbCard);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //await inventoryService.UpdateInventoryCard(cardToUpdate);

            ////Assert
            ////Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCardBatch_Test()
        {
            Assert.Fail();
            ////Assemble
            //var cardsToUpdate = new List<InventoryCardDto>()
            //{
            //    new InventoryCardDto()
            //    {
            //        Id = 1,
            //        MultiverseId = 1,
            //        VariantName = "normal"
            //    },
            //    new InventoryCardDto()
            //    {
            //        Id = 2,
            //        MultiverseId = 2,
            //        VariantName = "normal"
            //    },
            //    new InventoryCardDto()
            //    {
            //        Id = 3,
            //        MultiverseId = 3,
            //        VariantName = "normal"
            //    },
            //};

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardData>()))
            //    .Returns(Task.CompletedTask);

            //InventoryCardData inventoryDbCard = new InventoryCardData() { Id = 1 };

            //mockInventoryRepo
            //    .Setup(p => p.GetInventoryCardById(It.IsAny<int>()))
            //    .ReturnsAsync(inventoryDbCard);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //await inventoryService.UpdateInventoryCardBatch(cardsToUpdate);

            ////Assert
            ////Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCard_Test()
        {
            Assert.Fail();
            ////Assemble
            //int idToDelete = 1;

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToDelete)))
            //    .Returns(Task.CompletedTask);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //await inventoryService.DeleteInventoryCard(idToDelete);

            ////Assert
            ////Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCardBatch_Test()
        {
            Assert.Fail();
            ////Assemble
            //var idsToDelete = new List<int> { 1, 2, 3 };

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.DeleteInventoryCard(It.IsAny<int>()))
            //    .Returns(Task.CompletedTask);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //await inventoryService.DeleteInventoryCardBatch(idsToDelete);

            ////Assert
            ////Task returns void, nothing to assert
        }

        #endregion

        #region Search
        //Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);
        //Task<InventoryDetailDto> GetInventoryDetail(int cardId);
        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryOverviews_Test()
        {
            Assert.Fail();
            ////Assemble
            //InventoryQueryParameter queryParamToRequest = new InventoryQueryParameter()
            //{
            //    //GroupBy = "unique",
            //    //Sort = "price",
            //    //SortDescending = true,
            //    //Skip = 0,
            //    //Take = 100,
            //    //Rarity = new List<string>() { "common", "uncommon" },
            //};
            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //List<CardOverviewResult> inventoryOverviewResult = new List<CardOverviewResult>()
            //{

            //};

            //mockInventoryRepo
            //    .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(inventoryOverviewResult);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //IEnumerable<InventoryOverviewDto> overivews = await inventoryService.GetInventoryOverviews(queryParamToRequest);

            ////Assert
            //Assert.IsNotNull(overivews);
        }

        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryDetail_Test()
        {
            Assert.Fail();
            ////Assemble
            //int idToRequest = 1;

            //string nameToExpect = "Opt";

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //List<InventoryCardResult> inventoryQueryResult = new List<InventoryCardResult>()
            //{
            //    new InventoryCardResult(),
            //    new InventoryCardResult(),
            //    new InventoryCardResult(),
            //    new InventoryCardResult(),
            //    //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
            //    //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
            //    //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
            //    //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
            //};

            //mockInventoryRepo
            //    .Setup(p => p.GetInventoryCardsByName(It.Is<string>(s => s == nameToExpect)))
            //    .ReturnsAsync(inventoryQueryResult);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //CardData getCardDataResult = new CardData()
            //{
            //    Name = nameToExpect
            //};

            //mockCardDatarepo
            //    .Setup(p => p.GetCardData(It.Is<int>(i => i == idToRequest)))
            //    .ReturnsAsync(getCardDataResult);

            //List<CardData> cardDataResult = new List<CardData>()
            //{
            //    new CardData()
            //    {
            //        Legalities = new List<CardLegalityData>(),
            //        Variants = new List<CardVariantData>(),
            //        Rarity = new CardRarityData(),
            //        Set = new CardSetData(),
            //        CardColorIdentities = new List<CardColorIdentityData>(),
            //        CardColors = new List<CardColorData>(),
            //    },
            //    new CardData()
            //    {
            //        Legalities = new List<CardLegalityData>(),
            //        Variants = new List<CardVariantData>(),
            //        Rarity = new CardRarityData(),
            //        Set = new CardSetData(),
            //        CardColorIdentities = new List<CardColorIdentityData>(),
            //        CardColors = new List<CardColorData>(),
            //    },
            //    new CardData()
            //    {
            //        Legalities = new List<CardLegalityData>(),
            //        Variants = new List<CardVariantData>(),
            //        Rarity = new CardRarityData(),
            //        Set = new CardSetData(),
            //        CardColorIdentities = new List<CardColorIdentityData>(),
            //        CardColors = new List<CardColorData>(),
            //    },
            //};

            //mockCardDatarepo
            //    .Setup(p => p.GetCardsByName(It.Is<string>(s => s == nameToExpect)))
            //    .ReturnsAsync(cardDataResult);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////Act
            //InventoryDetailDto result = await inventoryService.GetInventoryDetail(idToRequest);

            ////Assert
            //Assert.IsNotNull(result);
        }

        #endregion

        #region Collection Builder

        [TestMethod]
        public async Task InventoryService_GetCollectionBuilderSuggestions_ThrowsNotImplemented_Test()
        {
            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);

            var mockSuggestions = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
            };

            mockCollectionBuilderService
                .Setup(p => p.GetCollectionBuilderSuggestions())
                .ReturnsAsync(mockSuggestions);

            var inventoryService = new CarpentryInventoryService(
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object
                );

            //Act
            var result = await inventoryService.GetCollectionBuilderSuggestions();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockSuggestions.Count, result.Count);

        }

        [TestMethod]
        public async Task InventoryService_HideCollectionBuilderSuggestion_ThrowsNotImplemented_Test()
        {
            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);

            mockCollectionBuilderService
                .Setup(p => p.HideCollectionBuilderSuggestion(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object
                );

            var payloadToSubmit = new InventoryOverviewDto() { };

            //Act
            await inventoryService.HideCollectionBuilderSuggestion(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
        }

        #endregion

        #region Trimming Tips

        [TestMethod]
        public async Task InventoryService_GetTrimmingTips_Test()
        {
            
            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);
            
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);

            var mockTrimmingTips = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
            };

            mockTrimmingTipsService
                .Setup(p => p.GetTrimmingTips())
                .ReturnsAsync(mockTrimmingTips);

            var inventoryService = new CarpentryInventoryService(
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object
                );

            //Act
            var result = await inventoryService.GetTrimmingTips();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockTrimmingTips.Count, result.Count);

        }

        [TestMethod]
        public async Task InventoryService_HideTrimmingTip_Test()
        {
            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);

            mockTrimmingTipsService
                .Setup(p => p.HideTrimmingTip(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object
                );

            var payloadToSubmit = new InventoryOverviewDto() { };

            //Act
            await inventoryService.HideTrimmingTip(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
        }
        
        #endregion

        #region Import/Export

        //Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto cardImportDto);
        [TestMethod]
        public async Task InventoryService_ValidateCarpentryImport_Test()
        {
            Assert.Fail();
            ////Assemble

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            //var payloadToSubmit = new CardImportDto() { };

            ////List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            ////{
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////};

            //////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            ////var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            ////mockInventoryRepo
            ////    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            ////    .ReturnsAsync(expectedSearchResult);

            ////var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            ////var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            ////InventoryQueryParameter filters = new InventoryQueryParameter()
            ////{

            ////};

            ////Act
            //var result = await inventoryService.ValidateCarpentryImport(payloadToSubmit);

            //Assert.IsNotNull(result);


            ////IEnumerable<MagicCardDto> result = await cardSearchService.SearchCardsFromInventory(filters);

            //////Assert
            ////Assert.IsNotNull(result);
            ////Assert.AreEqual(expectedSearchResult.Count, result.Count());
            //Assert.Fail("Must confirm this is working as expected");
        }

        //Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto);
        [TestMethod]
        public async Task InventoryService_AddValidatedCarpentryImport_Test()
        {
            Assert.Fail();

            ////Assemble

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            //var payloadToSubmit = new ValidatedCarpentryImportDto() { };

            ////List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            ////{
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////};

            //////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            ////var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            ////mockInventoryRepo
            ////    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            ////    .ReturnsAsync(expectedSearchResult);

            ////var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            ////var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            ////InventoryQueryParameter filters = new InventoryQueryParameter()
            ////{

            ////};

            ////Act
            //await inventoryService.AddValidatedCarpentryImport(payloadToSubmit);

            ////Assert
            ////Returns void, nothing to assert
            ////Assert.IsNotNull(result);
        }

        //Task<byte[]> ExportInventoryBackup();
        [TestMethod]
        public async Task InventoryService_ExportInventoryBackup_Test()
        {
            Assert.Fail();
            ////Assemble

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            //var inventoryService = new InventoryService(
            //    mockInventoryRepo.Object,
            //    mockDataUpdateService.Object,
            //    mockCoreRepo.Object,
            //    mockCardDatarepo.Object);

            ////List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            ////{
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////    CardInstance(),
            ////};

            //////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            ////var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            ////mockInventoryRepo
            ////    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            ////    .ReturnsAsync(expectedSearchResult);

            ////var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            ////var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            ////InventoryQueryParameter filters = new InventoryQueryParameter()
            ////{

            ////};

            ////Act
            //var result = await inventoryService.ExportInventoryBackup();

            ////Assert
            //Assert.IsNotNull(result);

            //Assert.Fail("Must decide how to validate the response object");
        }
        #endregion


    }
}
