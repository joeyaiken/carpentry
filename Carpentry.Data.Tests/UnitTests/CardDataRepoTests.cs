using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Tests.UnitTests
{
    [TestClass]
    public class CardDataRepoTests
    {

        [TestMethod]
        public async Task CardDataRepo__Test()
        {
            //Arrange


            //Act


            //Assert
            Assert.Fail();
        }

        //Task<List<string>> GetAllCardSetCodes();
        //Task<DateTime?> GetCardSetLastUpdated(string setCode); //why isn't this a GetSetByCode ?
        //Task<int> AddOrUpdateCardSet(CardSetData setData); //This probably doesn't actually have to return an ID
        //Task AddOrUpdateCardDefinition(CardDataDto cardDto);
        //Task<CardData> GetCardById(int multiverseId);
        //Task<IEnumerable<CardData>> GetCardsByName(string cardName);
        //Task EnsureDatabaseExists();
    }
}
