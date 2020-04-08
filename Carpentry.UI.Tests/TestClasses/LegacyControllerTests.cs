using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.TestClasses
{
    //[TestClass]
    public class LegacyControllerTests
    {
        readonly CarpentryFactory _factory;

        public LegacyControllerTests()
        {
            _factory = new CarpentryFactory();
        }
        
        #region Card Search legacy

        //Search by name
        //Verify returns data
        //Verify all results actually match the provided name
        [TestMethod]
        public void Cards_SearchByName_ReturnsValidCards_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Cards_SearchByName_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //Search by set
        //Verify returns data
        //I guess that all results belong to the expected set
        [TestMethod]
        public void Cards_SearchBySet_ReturnsFullSet_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Cards_SearchBySet_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }


        #endregion

        #region /Decks/Add

        //Decks/Add
        //  Add a new deck (w/ or w/o basic lands)?
        [TestMethod]
        public async Task Decks_Add_CanAddSuccessfully_Test()
        {
            Assert.Fail();
        }

        //  Invalid add?
        [TestMethod]
        public async Task Decks_Add_NamesMustBeUnique_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public async Task Decks_Add_MustHaveRequiredFields_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Update

        //Decks/Update
        //  Update the properties of a deck
        [TestMethod]
        public async Task Decks_Update_ProperlySavesChanges_Test()
        {
            Assert.Fail();
        }

        //  Invalid update?
        [TestMethod]
        public async Task Decks_Update_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Delete

        //Decks/Delete
        //  Delete the blue deck
        [TestMethod]
        public async Task Decks_Delete_CanDeleteBlueDeck_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public async Task Decks_Delete_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Search

        //Decks/Search
        //  Search deck properties
        [TestMethod]
        public async Task Decks_Search_ReturnsSampleDecks_Test()
        {
            Assert.Fail();
            ////Assemble
            //string API_ENDPOINT = "api/Decks/Search";
            //List<DeckProperties> searchResults;

            ////Act
            //using (var client = _factory.CreateClient())
            //{


            //    //var response = await client.PostAsync(API_ENDPOINT, queryParamStringContent);
            //    var response = await client.PostAsync(API_ENDPOINT, null);

            //    var responseContent = await response.Content.ReadAsStringAsync();

            //    //List<DeckProperties> searchResult = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);

            //    searchResults = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);
            //}

            ////Assert

            ////Expecting 5 mock decks
            //Assert.IsTrue(searchResults.Count == 5);

            ////TODO: Add more assertions
        }

        #endregion

        #region /Decks/Get

        //Get

        //  Get the details of a deck
        [TestMethod]
        public async Task Decks_Get_SelectRedDeck_Test()
        {
            Assert.Fail();

            ////Assemble
            //string API_ENDPOINT = "api/Decks/Get";
            //List<DeckProperties> searchResults;

            ////Act
            //using (var client = _factory.CreateClient())
            //{


            //    //var response = await client.PostAsync(API_ENDPOINT, queryParamStringContent);
            //    var response = await client.PostAsync(API_ENDPOINT, null);

            //    var responseContent = await response.Content.ReadAsStringAsync();

            //    //List<DeckProperties> searchResult = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);

            //    searchResults = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);
            //}

            ////Assert

            ////Expecting 5 mock decks
            //Assert.IsTrue(searchResults.Count == 5);

            ////TODO: Add more assertions
        }

        //  Invalid deck?
        [TestMethod]
        public async Task Decks_Get_InvalidIdReturnsError_Test()
        {

            //Assemble
            string API_ENDPOINT = $"api/Decks/Get?{6}";
            HttpStatusCode responseCode;

            //Act
            using (var client = _factory.CreateClient())
            {
                //There shouldn't be a 6th deck
                var response = await client.GetAsync(API_ENDPOINT);

                responseCode = response.StatusCode;
            }

            //Assert

            //Expecting 5 mock decks
            Assert.AreEqual(HttpStatusCode.BadRequest, responseCode);
        }

        #endregion

        #region /Decks/AddCard


        //AddCard
        //  Add a card & buylist card
        [TestMethod]
        public async Task Decks_AddCard_AddsNewBuylistCard_Test()
        {
            Assert.Fail();
        }

        //  Add deck card and NEW inventory card
        [TestMethod]
        public async Task Decks_AddCard_AddsNewInventoryCard_Test()
        {
            Assert.Fail();
        }

        //  Add existing inventory card to deck
        [TestMethod]
        public async Task Decks_AddCard_AddInventoryCardToDeck_Test()
        {
            Assert.Fail();
        }

        //  Invalid add? Maybe a bad Inventory record?
        [TestMethod]
        public async Task Decks_AddCard_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/RemoveCard

        //RemoveCard
        //  Remove a card from a deck
        [TestMethod]
        public async Task Decks_RemoveCard_LeavesInventoryCardIntact_Test()
        {
            Assert.Fail();
        }

        //  Remove a deck card AND inventory card?
        [TestMethod]
        public async Task Decks_RemoveCard_AlsoDeletesInventoryCard_Test()
        {
            Assert.Fail();
        }

        //  Remove a deck card AND move inventory card to the buylist?
        [TestMethod]
        public async Task Decks_RemoveCard_MovesInventoryCardToBuylist_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Add

        //Add
        //  Add buylist card
        [TestMethod]
        public void Inventory_Add_AddBuylistCard_Test()
        {
            Assert.Fail();
        }

        //  Add inventory card w/ a Card data obj
        [TestMethod]
        public void Inventory_Add_AddNewInventoryCardWithData_Test()
        {
            Assert.Fail();
        }

        //  Add inventory card w/o Card data obj
        [TestMethod]
        public void Inventory_Add_AdditionalInventoryCard_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Inventory_Add_AdditionalInventoryCardWithoutData_Test()
        {
            Assert.Fail();
        }

        //  add invalid card?
        [TestMethod]
        public void Inventory_Add_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //  Add sellList card?
        [TestMethod]
        public void Inventory_Add_AddSelllistCard_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Update
        //Update
        //  Move a buylist card to the inventory
        [TestMethod]
        public void Inventory_Update_MoveBuylistCardToInventory_Test()
        {
            Assert.Fail();
        }
        //  Move an inventory card to the sell list
        [TestMethod]
        public void Inventory_Update_MoveInventoryCardToSellList_Test()
        {
            Assert.Fail();
        }
        //  Invalid move?
        [TestMethod]
        public void Inventory_Update_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //  Move a card, contained in a deck, to the sellList (incorporate into the above test?)
        [TestMethod]
        public void Inventory_Update_MoveDeckCardToSelllist_ThrowsAnError_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Delete
        //Delete
        //  Delete a card that exists in a deck
        [TestMethod]
        public void Inventory_Delete_CardExistsInADeck_Test()
        {
            Assert.Fail();
        }
        //  Delete a card that doesn't exist in a deck
        [TestMethod]
        public void Inventory_Delete_CardNotInADeck_Test()
        {
            Assert.Fail();
        }

        //Expected errors
        [TestMethod]
        public void Inventory_Delete_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Search

        //private async Task<List<InventoryQueryResult>> CallCardSearch(InventoryQueryParameter queryParameter)
        //{
        //    string API_ENDPOINT_SearchInventory = "api/Inventory/Search";

        //    //var client = _factory.CreateClient();

        //    var queryParamStringBody = JsonConvert.SerializeObject(queryParameter, Formatting.None);

        //    var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

        //    var response = await _client.PostAsync(API_ENDPOINT_SearchInventory, queryParamStringContent);

        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    List<InventoryQueryResult> searchResult = JsonConvert.DeserializeObject<List<InventoryQueryResult>>(responseContent);

        //    return searchResult;
        //}

        //Get all unique names, order by count, 50
        //"What named cards do I have the most of? (top 50)"
        [TestMethod]
        public async Task InventorySearch_MostNamedCards_Test()
        {
            //InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            //{
            //    //group by unique names
            //    GroupBy = "name",
            //    //sort by count DESC
            //    Sort = "count",
            //    Skip = 0,
            //    Take = 50,
            //};

            //var result = await CallCardSearch(queryParameter);

            //Assert.IsTrue(result.Count == 50);

            Assert.Fail();
        }


        //What Standard-Legal Knights do I own?
        [TestMethod]
        public async Task InventorySearch_StandardKnights_Test()
        {


            //InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            //{
            //    Types = new List<string>() { "Knight" }, //Should this be a Type filter or Text filter?
            //    Format = "standard",
            //    GroupBy = "unique",
            //};

            //var result = await CallCardSearch(queryParameter);

            //Assert.IsTrue(result.Count > 0);

            //var totalCount = result.Select(x => x.Items.Count).Sum();

            //result.ForEach(item =>
            //{

            //    //Ensure none of the results aren't standard-legal

            //    //Ensure all of the results are knights


            //    //If there are multiple 'cards', they should be instances / variants of the same card?
            //    //Has to do with how items are grouped

            //    Assert.IsTrue(item.Cards.Count == 1);

            //    Assert.IsTrue(item.Items.Count > 0);

            //    var firstCard = item.Cards[0];

            //    Assert.IsTrue(firstCard.Type.ToLower().Contains("knight"));

            //    //TODO: Update MagicCardDto to include formats so I can validate all cards are standard
            //    //Assert.IsTrue(firstCard.)

            //    Assert.IsTrue(firstCard.Legalities.Contains("standard"));

            //});

            Assert.Fail();
        }

        //What unique set/name cards do I have the most of?
        [TestMethod]
        public async Task InventorySearch_MostUniqueSetCards_Test()
        {
            //InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            //{
            //    //group by unique name / set combos
            //    GroupBy = "unique",
            //    //sort by count DESC
            //    Sort = "count_DESC",
            //    Skip = 0,
            //    Take = 50,
            //};

            //var result = await CallCardSearch(queryParameter);

            //var totalCount = result.Select(x => x.Items.Count).Sum();

            //Assert.IsTrue(result.Count == 50);

            //How do I verify the rest?
            Assert.Fail();

        }

        [TestMethod]
        public async Task InventorySearch_SnowCards_Test()
        {
            //InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            //{
            //    GroupBy = "unique",
            //    Text = "snow"
            //};

            //var result = await CallCardSearch(queryParameter);

            //Assert.IsTrue(result.Count > 0);

            //result.ForEach(item =>
            //{

            //    //Ensure all results contain 'snow' in one of name/type/text

            //    Assert.IsTrue(item.Cards.Count == 1);

            //    Assert.IsTrue(item.Items.Count > 0);

            //    var firstCard = item.Cards[0];

            //    Assert.IsTrue(
            //        firstCard.Type.ToLower().Contains("snow")
            //        ||
            //        firstCard.Text.ToLower().Contains("snow")
            //        ||
            //        firstCard.Name.ToLower().Contains("snow")
            //        );
            //});

            Assert.Fail();
        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByColors_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByType_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterBySet_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByText_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanGroupResults_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_EmptyFilter_ReturnsData_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_NullFilter_ReturnsBadRequest_Test()
        {

            Assert.Fail();

            ////InventoryQueryParameter queryParameter = null;
            //InventoryQueryParameter queryParameter = new InventoryQueryParameter();
            //string API_ENDPOINT_SearchInventory = "api/Inventory/Search";

            ////List<FilterDescriptor> queryParameter = new List<FilterDescriptor>()
            ////{
            ////    new FilterDescriptor{ Name="Set", Value="MH1" },
            ////    new FilterDescriptor{ Name="ColorIdentity", Value="R" },
            ////    new FilterDescriptor{ Name="Rarity", Value="Uncommon" },
            ////    new FilterDescriptor{ Name="Type", Value="Creature" }

            ////};


            //var queryParamStringBody = JsonConvert.SerializeObject(queryParameter, Formatting.None);
            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            //var client = _factory.CreateClient();

            //var response = await client.PostAsync(API_ENDPOINT_SearchInventory, queryParamStringContent);

            ////var responseContent = response.Content.ReadAsStringAsync();

            //Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            ////List<InventoryQueryResult> searchResult = JsonConvert.DeserializeObject<List<InventoryQueryResult>>(responseContent.Result);


        }

        [TestMethod]
        public async Task SearchInventory_PaginationWorks_Test()
        {

            Assert.Fail();

        }

        #endregion

    }
}
