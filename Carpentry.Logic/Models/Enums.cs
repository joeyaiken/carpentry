namespace Carpentry.Logic.Models
{
    public enum CardSearchGroupBy
    {
        name,
        print,
        unique,
    }

    public enum InventoryCardStatus
    {
        Inventory = 1,
        BuyList = 2,
        SellList = 3,
        Deck = 4,
    }
    
    public enum CardSearchGroup
    {
        Red,
        Blue,
        Green,
        White,
        Black,
        Multicolored,
        Colorless,
        Lands,
        RareMythic,
    }
}
