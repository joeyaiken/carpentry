
interface CardFilterProps {
    set: string;

    type: string;

    text: string;

    colorIdentity: string[];
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;

    rarity: string[];

    cardName: string;
    exclusiveName: boolean;

    format: string;

    minCount: number | null;
    maxCount: number | null;
}

declare interface DeckCardDto {
    id: number;
    deckId: number;
    categoryId: string | null;
    inventoryCard: InventoryCard;
}

declare interface DeckDto {
    props: DeckProperties;
    // cards: Card[];
    // data: { [key: number]: MagicCard };
    cardOverviews: InventoryOverviewDto[];
    cardDetails: InventoryCard[];
    stats: DeckStats;
}

declare interface DeckProperties {
    id: number;
    name: string;
    //format: DeckFormats;
    format: null | 'Standard' | 'Legacy' | 'Modern' | 'Commander' | 'Oathbreaker';
    notes: string;

    basicW: number;
    basicU: number;
    basicB: number;
    basicR: number;
    basicG: number;
}

declare interface DeckStats {
    totalCount: number;
    typeCounts: { [type: string]: number };
    costCounts: { [type: string]: number };
    totalCost: number;
}

declare interface FilterDescriptor {
    name: string;
    value: any;
}


declare interface FilterOption {
    name: string;
    value: string;
}



interface FilterOptionDto {
    sets: FilterOption[];
    types: FilterOption[];
    formats: FilterOption[];
    colors: FilterOption[];
    rarities: FilterOption[];
    statuses: FilterOption[];
}

declare interface InventoryCard {
    id: number;
    multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    variantName: string;
    statusId: number; //normal === 1, buylist === 2, sellList === 3
    deckCards: InventoryDeckCardDto[];
}

declare interface InventoryDeckCardDto {
    id: number;
    deckId: number;
    deckName: string;
    inventoryCardId: number;
    category: string;
}

interface InventoryDetailDto {
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
    // deckCards: any[];
}


interface InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
    id: number;
    multiverseId: number;
    name: string;
    type: string;
    cost: string;
    img: string;
    count: number;
    description: string;
}

declare interface InventoryQueryParameter {
    //

    groupBy: string;
    colors: string[];
    types: string[];
    type: string;
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];

    //sets: string[];
    set: string;
    text: string;
    skip: number;
    take: number;
    format: string | null;
    sort: string
    //other things to add?
    //Format / Legality
    minCount: number;
    maxCount: number;

}



declare interface MagicCard {
    cmc: number | null;
    colorIdentity: string[];
    colors: string[];
    manaCost: string;
    multiverseId: number;
    name: string;
    prices: { [key: string]: number | null }
    variants: { [key: string]: string | null }
    legalities: string[];
    rarity: string;
    set: string;
    text: string;
    type: string;
}