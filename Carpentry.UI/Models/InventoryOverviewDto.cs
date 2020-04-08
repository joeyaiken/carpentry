using Carpentry.Logic.Models;

namespace Carpentry.UI.Models
{
    public class InventoryOverviewDto // TODO : Rename to CardOverviewDto
    {
        public InventoryOverviewDto(InventoryOverview model)
        {
            Cmc = model.Cmc;
            Cost = model.Cost;
            Count = model.Count;
            Description = model.Description;
            Id = model.Id;
            Img = model.Img;
            Name = model.Name;
            Type = model.Type;
        }

        public InventoryOverview ToModel()
        {
            InventoryOverview result = new InventoryOverview
            {
                Cmc = Cmc,
                Cost = Cost,
                Count = Count,
                Description = Description,
                Id = Id,
                Img = Img,
                Name = Name,
                Type = Type,
            };
            return result;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Cost { get; set; }

        public int? Cmc { get; set; }

        public string Img { get; set; }

        public int Count { get; set; }

        //category / status / group
        public string Description { get; set; }
    }
}
