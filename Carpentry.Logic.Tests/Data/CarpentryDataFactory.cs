using Carpentry.CarpentryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Tests.Data
{
    public class CarpentryDataFactory
    {


        public CardSetData CardSet(string name, string code, DateTime releasedAt, params CardData[] cards)
        {
            return new CardSetData()
            {
                Name = name,
                Code = code,
                Cards = cards,
            };
        }

        public CardData Card(string name, string color, char rarityId = 'R')
        {
            return new CardData()
            {
                Name = name,
                RarityId = rarityId,
                Color = color,
                ColorIdentity = color,
            };
        }

        public InventoryCardData InventoryCard()
        {
            return new InventoryCardData()
            {

            };
        }

    }
}
