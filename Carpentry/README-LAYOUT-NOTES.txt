Carpentry - Components: {
	Inventory: {
		Available Modes: {
			Primary: {
				Layout: null,
				Available Secondary Components: [
					Card Search
				],
				Some Actions: {
					Search inventory (& handling filter changes)
					Delete inventory card
					ScrySearch alt versions (same name diff sets)
				
				}
			},
			Secondary: {
				Layout: null,
				Sources: [Deck Editor],
				Some Actions: {
					Search cards & handle filter changes,
					Add a card to the selected deck (& either associate with inv card or add new buylist card),
					(only adding 1 card at a time, not maintaining a pending add list)
					(this isn't the place to delete cards)
				}
			}
		}
	},
	
	Deck Editor: {
		Available Modes: {
			Primary: {
				Layout: null,
				Available Secondary Components: [
					Inventory,
					Card Search
				],
				Some actions: {(so many),
					Change view settings (sort/filter/group/layout),
					Remove card,
					"I want to look for more copies of this card" {
						Search inv for selected card name,
						Search Scry for selected card name,
					}
					Delete entire deck
					Update deck props
				}
			}
		},
	},
	
	Card Search: {
		Available Modes: {
			Secondary: {
				Layout: null,
				Sources: [Inventory, Deck Editor, Buylist],
				Some actions: {
					(what sources allow the pending cardlist?)
					Add pending card,
					Save pending cards to current context
					add single pending card to current context?
						DE: Add buylist/inv card & deck card
						Inv: Add inv/buylist card
						Buylist: Add buylist card
					Handle filter changes
					Search scry
					handle search mode change?
					Can't delete anything from here
						Can't really UPDATE anything from here
						Can only add
				}
			}
		},
	},

	Buy List: {
		Available Modes: {
			Primary: {
				Layout: null,
				Available Secondary Components: [
					Card Search
				]
				Some Actions: {
					Update buylist card
					Delete buylist card
					Get full buylist
				}
			}
		},	
	},
	
	DeckQuickAdd: {
		Available Modes: {
			Modal: {
				Layout: null,
				Some actions: {
					Handle field change,
					Save self to DB
						Request Save
						SaveNewDeck Requested
						SaveNewDeck Resolved
							(cant fire off 'deck added' if an error occurs)
						Deck Added
				}
			}
		},
	}
}
