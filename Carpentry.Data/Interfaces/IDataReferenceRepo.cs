using Carpentry.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    public interface IDataReferenceRepo
    {

        Task TryAddInventoryCardStatus(InventoryCardStatusData data);

        Task TryAddCardRarity(CardRarityData data);
        Task TryAddManaType(ManaTypeData data);
        Task TryAddMagicFormat(MagicFormatData data);
        Task TryAddCardVariantType(CardVariantTypeData data);
        Task TryAddDeckCardCategory(DeckCardCategoryData data);
    }
}
