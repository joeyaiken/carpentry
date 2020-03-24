using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.BackupTool.Models
{
    class BackupDataProps
    {
        //backup date
        public DateTime TimeStamp { get; set; }

        //app version?

        //set codes in backup
        public List<string> SetCodes { get; set; }
        //total # of cards?

        //total # of decks?

        //# of cards per set? (for removing sets that I don't actually care about)

    }
}
