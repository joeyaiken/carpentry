CREATE OR ALTER VIEW [dbo].[vwCardTotals] AS 
	--SELECT * FROM [dbo].[vwCardTotals]
	--view of all totals per location, grouped by CardId+IsFoil
	--Any unowned cardId+IsFoil pairs are not included
	SELECT	PivotTable.CardId
			,PivotTable.IsFoil
			,PivotTable.[Inventory] AS InventoryCount
			,PivotTable.[Buy List] AS WishCount
			,PivotTable.[Sell List] AS SellCount
			,PivotTable.[Deck] AS DeckCount
			,PivotTable.[Inventory]+PivotTable.[Buy List]+PivotTable.[Sell List]+PivotTable.[Deck] AS TotalCount
	FROM (
		SELECT		ic.InventoryCardId
					,ic.CardId
					,ic.IsFoil			
					,CASE WHEN dc.DeckCardId IS NULL THEN s.Name ELSE 'Deck' END AS Location
		FROM		InventoryCards ic
		LEFT JOIN	DeckCards dc
				ON	ic.InventoryCardId = dc.InventoryCardId
		LEFT JOIN	CardStatuses s
				ON	ic.InventoryCardStatusId = s.CardStatusId
	) AS SourceTable
	PIVOT
	(
		COUNT(InventoryCardId)
		FOR Location IN ([Inventory],[Buy List],[Sell List],[Deck])
	) AS PivotTable