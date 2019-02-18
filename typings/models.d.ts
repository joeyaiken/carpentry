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
    [set: string]: {
        [name: string]: ICard;
    }
}