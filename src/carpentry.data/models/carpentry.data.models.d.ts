//this should eventually be separate files
//Should also probably do something more fancy with namespaces eventually

//Okay so, for any of these types, we should be storing both .json and cached version of each object
//When trying to check data, we should always load both timestamps (cache and .json), then load the most recent source
//Yes, this isn't terribly efficient, but it's what I'm going to try for V1

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


export interface ITimestamps {
    cardsLastUpdated: Date;
    cardCollectionsLastUpdated: Date;
    collectionTypesLastUpdated: Date;
    timestampsLastUpdated: Date;
}



//Other interfaces will include a lot of things implementing collection types