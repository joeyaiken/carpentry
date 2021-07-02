using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
{
    public class CardLegalityData
    {
        [Key]
        public int CardLegalityId { get; set; }
        public int CardId { get; set; }
        public int FormatId { get; set; }

        //Associations
        public virtual CardData Card { get; set; }
        public virtual MagicFormatData Format { get; set; }
    }
}
