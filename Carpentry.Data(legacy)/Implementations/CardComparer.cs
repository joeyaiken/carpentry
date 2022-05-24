using Carpentry.Data.LegacyModels;
using Carpentry.Data.Models;
using System.Collections.Generic;

namespace Carpentry.Data.Implementations
{
    //for getting distinct data cards
    public class CardComparer : EqualityComparer<MagicCardDto>
    {
        public override bool Equals(MagicCardDto c1, MagicCardDto c2)
        {
            return (c1.MultiverseId == c2.MultiverseId);
        }
        public override int GetHashCode(MagicCardDto c)
        {
            return c.MultiverseId.GetHashCode();
        }
    }

    //public class DataCardComparer : EqualityComparer<Data.Card>
    //{
    //    public override bool Equals(Data.Card c1, Data.Card c2)
    //    {
    //        return (c1.MultiverseId == c2.MultiverseId);
    //    }
    //    public override int GetHashCode(Data.Card c)
    //    {
    //        return c.MultiverseId.GetHashCode();
    //    }
    //}

    //public class DataCardDetailComparer : EqualityComparer<Data.CardDetail>
    //{
    //    public override bool Equals(Data.CardDetail c1, Data.CardDetail c2)
    //    {
    //        return (c1.MultiverseId == c2.MultiverseId);
    //    }
    //    public override int GetHashCode(Data.CardDetail c)
    //    {
    //        return c.MultiverseId.GetHashCode();
    //    }
    //}
}
