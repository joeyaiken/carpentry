using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.Models
{
    public class InventoryDetailDto
    {
        public string Name { get; set; }

        //Cards
        public List<ScryfallMagicCard> Cards { get; set; }
        
        //Inventory Cards
        public List<InventoryCardDto> InventoryCards { get; set; }

        //Deck Cards
        //for now, just storing IDs under an inventory card
    }
}
