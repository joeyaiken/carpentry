declare interface ICardDeck_Legacy {
    //an index
    id: number;

    //some settings / properties
    details: IDeckDetail_Legacy;

    //some statistics
    stats: IDeckStats | null;

    //a collection of cards
    cards: IDeckCard[];
    
}

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

declare interface IDeckDetail_Legacy {
    id: number;
    //
    name: string;
    description: string;
    type: string;

    basicLands: ILandCount; //needs to be removed

    isUpToDate: boolean;
    //"gimick" ?
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