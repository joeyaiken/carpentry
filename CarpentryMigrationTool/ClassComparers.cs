using System;
using System.Collections.Generic;
using System.Text;

namespace CarpentryMigrationTool
{
    //class ClassComparers
    //{
    //}
    //public class CardComparer : EqualityComparer<MagicCardDto>
    //{
    //    public override bool Equals(MagicCardDto c1, MagicCardDto c2)
    //    {
    //        return (c1.MultiverseId == c2.MultiverseId);
    //    }
    //    public override int GetHashCode(MagicCardDto c)
    //    {
    //        return c.MultiverseId.GetHashCode();
    //    }
    //}

    public class LegacySetComparer : EqualityComparer<Carpentry.Data.DataContextLegacy.CardSet>
    {
        public override bool Equals(Carpentry.Data.DataContextLegacy.CardSet s1, Carpentry.Data.DataContextLegacy.CardSet s2)
        {
            return (s1.Code == s2.Code);
        }
        public override int GetHashCode(Carpentry.Data.DataContextLegacy.CardSet s)
        {
            return s.Code.GetHashCode();
        }
    }
}
