﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class InventoryCardData
    {
        //Fields
        public int Id { get; set; }

        public int MultiverseId { get; set; }

        public int InventoryCardStatusId { get; set; }

        public int VariantTypeId { get; set; }

        public bool IsFoil { get; set; }

        //Associations
        public virtual CardData Card { get; set; }

        public virtual ICollection<DeckCardData> DeckCards { get; set; }

        public virtual InventoryCardStatusData Status { get; set; }

        public virtual CardVariantTypeData VariantType { get; set; }
    }

}
