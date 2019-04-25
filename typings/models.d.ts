declare interface ICardDeck {
    //an index
    id: number;

    //some settings / properties
    details: IDeckDetail;

    //some statistics
    stats?: IDeckStats | null;

    //a collection of cards
    cards: IDeckCard[];
    //basic lands (for now)
    basicLands: ILandCount;
}

declare interface IDeckDetail {
    id: number;
    //
    name: string;
    description: string;
    type: string;
    isUpToDate: boolean;
}

declare interface IDeckStats {
    id: number;
    //
    colors: string;
}

declare interface IDeckCard {
    name: string;
    set: string;
}

declare interface ICardIndex {
    [set: string]: ICardDictionary;
}

declare interface ICardDictionary {
    [name: string]: ICard;
}

declare interface IWebSearchFilter {
    name?: string;
    layout?: string;
    cmc?: number;
    colors?: string;
    colorIdentity?: string;
    type?: string;
    supertypes?: string;
    types?: string;
    subtypes?: string;
    rarity?: string;
    set?: string;
    setName?: string;
    text?: string;
    flavor?: string;
    artist?: string;
    number?: string;
    power?: string;
    toughness?: string;
    loyalty?: number;
    foreignName?: string;
    language?: string;
    gameFormat?: string;
    legality?: keyof typeof Legality;
    page?: number;
    pageSize?: number;
    orderBy?: string;
    random?: boolean;
    contains?: string;
}


declare interface ICardSet{
    code: string,
    name: string
}