import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FilterService } from "src/app/common/filter-service";
import { AppFiltersDto } from "src/app/settings/models";
import { CardSearchResultDto, InventoryFilterProps } from "../models";



@Component({
    selector: 'app-inventory-add-cards',
    templateUrl: 'inventory-add-cards.component.html',
    styleUrls: ['inventory-add-cards.component.less'],
})
export class InventoryAddCardsComponent implements OnInit {
    // @Input() cardItem: InventoryOverviewDto;

    // @Output() onCardSelected = new EventEmitter<number>(); //: (cardId: number) => void;
    // @Output() onToggleViewClick = new EventEmitter<void>();

    selectedCard: CardSearchResultDto | null;
    viewMode: string;
    searchFilter: InventoryFilterProps;
    filterOptions: AppFiltersDto;
    
    constructor(
        private filterService: FilterService,
    ) {

    }

    ngOnInit(): void {
        this.searchFilter = new InventoryFilterProps();
        this.loadFilterOptions();
    }

    onToggleView(): void {
        // this.onToggleViewClick.emit();


    }
    
    loadFilterOptions(): void {
        // this.isLoading = true;
        this.filterService.getAppFilterOptions().subscribe(
            data => {
                this.filterOptions = data;
                // this.isLoading = false;
            }

        )
    }
    // handleSaveClick(){
    //     this.props.dispatch(requestSavePendingCards());
    // }

    // handleCancelClick(){
    //     this.props.dispatch(cardSearchClearPendingCards());
    // }

    // handleSearchMethodTabClick(name: string): void {
    //     this.props.dispatch(cardSearchSearchMethodChanged(name));
    // }

    // handleToggleViewClick(): void {
    //     this.props.dispatch(toggleCardSearchViewMode());
    // }

    // handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
    //     this.props.dispatch(addPendingCard(name, cardId, isFoil));
    // }

    // handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
    //     this.props.dispatch(removePendingCard(name, cardId, isFoil));
    // }

    // handleCardSelected(item: CardListItem){
    //     this.props.dispatch(cardSearchSelectCard(item.data));
    // }

    // handleSearchButtonClick(){
    //     this.props.dispatch(requestSearch())
    // }

    // handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    // }

    // handleBoolFilterChange(filter: string, value: boolean): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
    // }
}