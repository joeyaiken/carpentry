declare interface ICardDeck {
    //an index
    id: number;

    //some settings / properties
    details: IDeckDetail;

    //some statistics
    stats: IDeckStats;

    //a collection of cards
    cards: IDeckCard[];
}

declare interface IDeckDetail {
    id: number;
    //
    name: string;
    description: string;
    type: string;

    basicLands: ILandCount;

    //"gimick" ?
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