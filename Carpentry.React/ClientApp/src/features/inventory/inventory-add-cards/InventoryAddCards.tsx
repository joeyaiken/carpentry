import React from 'react';
import {InventoryAddCardsLayout} from "./components/InventoryAddCardsLayout";
// import {
//   addPendingCard,
//   cardSearchClearPendingCards, cardSearchFilterValueChanged, cardSearchSelectCard, removePendingCard,
//   requestSavePendingCards, requestSearch,
//   toggleCardSearchViewMode
// } from "./state/InventoryAddCardsActions";



export const InventoryAddCards = (): JSX.Element => {
  
  // handleSaveClick(){
  //   this.props.dispatch(requestSavePendingCards());
  // }
  //
  // handleCancelClick(){
  //   this.props.dispatch(cardSearchClearPendingCards());
  // }
  //
  // handleToggleViewClick(): void {
  //   this.props.dispatch(toggleCardSearchViewMode());
  // }
  //
  // handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
  //   this.props.dispatch(addPendingCard(name, cardId, isFoil));
  // }
  //
  // handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
  //   this.props.dispatch(removePendingCard(name, cardId, isFoil));
  // }
  //
  // handleCardSelected(item: CardListItem){
  //   this.props.dispatch(cardSearchSelectCard(item.data));
  // }
  //
  // handleSearchButtonClick(){
  //   this.props.dispatch(requestSearch())
  // }
  //
  // handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
  //   this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
  // }
  //
  // handleBoolFilterChange(filter: string, value: boolean): void {
  //   this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
  // }



  // return(
  //   <InventoryAddCardsLayout
  //     filterOptions={this.props.filterOptions}
  //     pendingCards={this.props.pendingCards}
  //     searchFilterProps={this.props.searchFilterProps}
  //     searchResults={this.props.searchResults}
  //     selectedCard={this.props.selectedCard}
  //     viewMode={this.props.viewMode}
  //     isLoading={this.props.isLoading}
  //     handleCancelClick={this.handleCancelClick}
  //     handleSaveClick={this.handleSaveClick}
  //     handleToggleViewClick={this.handleToggleViewClick}
  //     handleAddPendingCard={this.handleAddPendingCard}
  //     handleBoolFilterChange={this.handleBoolFilterChange}
  //     handleCardSelected={this.handleCardSelected}
  //     handleFilterChange={this.handleFilterChange}
  //     handleRemovePendingCard={this.handleRemovePendingCard}
  //     handleSearchButtonClick={this.handleSearchButtonClick}
  //   />);
  
  return (
    <InventoryAddCardsLayout />
  )
}