export class ApiDeckCardDetail {
    id: number;
    deckId: number;
    overviewId: number;
    //multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    //variantName: string;
    collectorNumber: number | null;
    category: string;
    inventoryCardId: number | null;
    cardId: number | null;
    availabilityId: number;
}

export class ApiDeckCardOverview {
    id: number;
    name: string;
    type: string;
    cost: string;
    cmc: number;
    img: string;
    count: number;
    category: string;
    cardId: number;
    details: ApiDeckCardDetail[];
    tags: string[];
}

export class CardImportDto {
    importType: number; //should really be removed
    importPayload: string;
    //Thoughts: props can just be added on a validated payload
}

//Old approach, was being used in react app
export class CardOverviewGroup {
    name: string;
    cardOverviews: ApiDeckCardOverview[];
}
//new approach: some class that can be used as a group OR row
export class GroupedCardOverview extends ApiDeckCardOverview {
    // constructor(){
    //     super();
    // }

    isGroup: boolean;
}

export class CardTagDto {
    deckId: number;
    cardName: string;
    tag: string;
}
export class CardTagDetailDto {
    cardId: number;
    cardName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];
}

export class CardTagDetailTag {
    cardTagId: number;
    tag: string;
}

export class DeckCardDto {
    id: number;
    deckId: number;
    cardName: string;
    categoryId: string | null;

    inventoryCardId: number | null;
    cardId: number;
    isFoil: boolean;
    inventoryCardStatusId: number;
    // inventoryCard: InventoryCard;
}

export class DeckCardDetail {
    id: number;
    deckId: number;
    name: string;
    inventoryCardId: number | null;
    isFoil: boolean;
    inventoryCardStatusId: number | null;
    cardId: number | null;
    set: string;
    collectorNumber: number | null;
    overviewId: number;
    category: string;
    availabilityId: number;
}

export class DeckCardOverview {
    id: number;
    name: string;
    type: string;
    cost: string;
    cmc: number;
    category: string;
    img: string;
    count: number;
    cardId: number;
    detailIds: number[];
    tags: string[];
}

export class DeckDetailDto {
    props: DeckPropertiesDto;
    // cardOverviews: DeckCardOverview[];
    // cards: DeckCard[];
    cards: ApiDeckCardOverview[];
    stats: DeckStats;
}

export class DeckOverviewDto {
    id: number;
    name: string;
    format: string;
    colors: string[];
    isValid: boolean;
    validationIssues: string;
}

export class DeckPropertiesDto {
    id: number;
    name: string;
    //format: DeckFormats;
    format: null | string; //DeckFormatOption;
    notes: string;

    basicW: number;
    basicU: number;
    basicB: number;
    basicR: number;
    basicG: number;
}

export class DeckStats {
    totalCount: number;
    typeCounts: {[type: string]: number};
    costCounts: {[cost: string]: number};
    tagCounts: {[tag: string]: number}
    totalCost: number;
    colorIdentity: string[];
}

export class NamedCardGroup {
    name: string;
    cardOverviewIds: number[];
}

export class ValidatedCardDto
{
    sourceString: string;
    isValid: boolean;
    isBasicLand: boolean;
    isEmpty: boolean;

    count: number;
    name: string;
    category: string | null;

    cardId: number;
    setCode: string;
    collectorNumber: number;
    isFoil: boolean;
}

export class ValidatedDeckImportDto {
    isValid: boolean;
    deckProps: DeckPropertiesDto;
    validatedCards: ValidatedCardDto[];
    untrackedSets: ValidatedDtoUntrackedSet[];
}

export class ValidatedDtoUntrackedSet {
    setId: number;
    setCode: string;
}