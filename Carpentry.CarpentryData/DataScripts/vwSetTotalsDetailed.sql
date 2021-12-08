CREATE OR ALTER VIEW [dbo].[vwSetTotalsDetailed]
AS
SELECT	Sets.SetId AS SetId
     ,Sets.Code
     ,Sets.Name
     ,Sets.ReleaseDate
     ,Sets.LastUpdated
     ,Sets.IsTracked
     --,InventoryCounts.InventoryCount
     ,PivotTable.[Inventory] AS InventoryCount
     ,PivotTable.[Buy List] AS WishCount
			,PivotTable.[Sell List] AS SellCount
			,PivotTable.[Deck] AS DeckCount

			,PivotTable.[Inventory]+PivotTable.[Buy List]+PivotTable.[Sell List]+PivotTable.[Deck] AS TotalCount


			,CollectedBySet.CollectedCount
			,CollectedBySet.SetCount --total cards in a set
FROM	Sets

    --collected per-set
    LEFT JOIN (
    SELECT	SetId
        ,SUM(IsCollected) AS CollectedCount
        ,COUNT(CardId) AS SetCount

    FROM (
    SELECT		Cards.CardId
        ,Cards.SetId
        ,CASE	WHEN COUNT(InventoryCards.InventoryCardId) > 0
    THEN 1
    ELSE 0
    END AS IsCollected
    FROM		Cards
    LEFT JOIN	InventoryCards
    ON		Cards.CardId = InventoryCards.CardId
    GROUP BY	Cards.CardId, Cards.SetId

    ) AS CollectedCards

    GROUP BY SetId

    ) AS CollectedBySet
ON	Sets.SetId = CollectedBySet.SetId
    LEFT JOIN (
    SELECT		c.SetId
    ,ic.InventoryCardId
    ,CASE WHEN dc.DeckCardId IS NULL THEN s.Name ELSE 'Deck' END AS Location
    FROM		InventoryCards ic
    JOIN		Cards c
    ON		ic.CardId = c.CardId
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
    ON	Sets.SetId = PivotTable.SetId