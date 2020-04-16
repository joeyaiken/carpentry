using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //THIS needs an association to all inventory items of the variant type
    //The actual inventory card will have a FK referencing a specific Card Variant
    //(what if variant type was a CHAR and not an INT??)
    //NOTE: FOIL is no longer a variant type
    //  Does this allow for CHAR IDs ?
    /*
        N   N - Normal
        B   BL - Borderless (plainswalker)
        E   EA - Extended Art (non-PW)
        S   SC - Showcase (storybook)
        F   FP - FatPack alternative art promo
        
    */
    public class CardVariantTypeData
    {
        //id
        //Should ID be a char?
        //why not
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //name
        public string Name { get; set; }

        //associations
        //cards of this variant
        //do I really need an association in this direction?
        //'all cards of a given variant'
        public List<CardVariantData> VariantCards { get; set; }
        public List<InventoryCardData> InventoryCards { get; set; }
    }
}
