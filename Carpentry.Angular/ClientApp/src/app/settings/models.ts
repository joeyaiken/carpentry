export class AppFiltersDto
{
    sets: FilterOption[];
    types: FilterOption[];
    formats: FilterOption[];
    colors: FilterOption[];
    rarities: FilterOption[];
    statuses: FilterOption[];
    groupBy: FilterOption[];
    sortBy: FilterOption[];
    searchGroups: FilterOption[];
}

export class FilterOption
{
    name: string;
    value: string;
}

export class InventoryTotalsByStatusResult
{
    statusId: number;
    statusName: string;
    totalPrice: number;
    totalCount: number | null;
}

export class SetDetailDto
{
    setId: number;
    code: string;
    name: string;
    releaseDate: Date;
    dataLastUpdated: Date | null;
    isTracked: boolean;
    inventoryCount: number | null;
    collectedCount: number | null;
    totalCount: number | null;
}