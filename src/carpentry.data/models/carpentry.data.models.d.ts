//this should eventually be separate files
//Should also probably do something more fancy with namespaces eventually

export interface DataCard {
    //So if this were to ever go in a SQL table, it would most likely want a ID
    //Since it isn't, lets be more hacky
    //cardId: number;
    name: string;
    set: string;
    cardCollectionId: string;
}

export interface DataCardCollection {
    cardCollectionId: number;
    collectionTypeId: number;
}

export interface DataCollectionType {
    collectionTypeId: number;
    type: string;
}

//Other interfaces will include a lot of things implementing collection types