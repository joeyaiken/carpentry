/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

interface InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
    id: number;
    name: string;
    type: string;
    cost: string;
    img: string;
    count: number;
    description: string;
    cmc: number | null;    
    category: string;
    price: number | null;
    isFoil: boolean;
    variant: string;

}

declare interface InventoryQueryParameter {
    groupBy: InventorySearchMethod;
    colors: string[];
    types: string[];
    type: string;
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];
    set: string;
    text: string;
    skip: number;
    take: number;
    format: string | null;
    sort: string;
    sortDescending: boolean;
    minCount: number;
    maxCount: number;
}