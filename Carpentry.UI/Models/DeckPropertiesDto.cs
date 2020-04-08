using Carpentry.Logic.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Models
{
    public class DeckPropertiesDto
    {
        public DeckPropertiesDto()
        {

        }

        public DeckPropertiesDto(DeckProperties props)
        {
            BasicB = props.BasicB;
            BasicG = props.BasicG;
            BasicR = props.BasicR;
            BasicU = props.BasicU;
            BasicW = props.BasicW;
            Format = props.Format;
            Id = props.Id;
            Name = props.Name;
            Notes = props.Notes;
        }

        public DeckProperties ToModel()
        {
            DeckProperties result = new DeckProperties()
            {
                BasicB = BasicB,
                BasicG = BasicG,
                BasicR = BasicR,
                BasicU = BasicU,
                BasicW = BasicW,
                Format = Format,
                Id = Id,
                Name = Name,
                Notes = Notes,
            };
            return result;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("basicW")]
        public int BasicW { get; set; }

        [JsonProperty("basicU")]
        public int BasicU { get; set; }

        [JsonProperty("basicB")]
        public int BasicB { get; set; }

        [JsonProperty("basicR")]
        public int BasicR { get; set; }

        [JsonProperty("basicG")]
        public int BasicG { get; set; }
    }
}
