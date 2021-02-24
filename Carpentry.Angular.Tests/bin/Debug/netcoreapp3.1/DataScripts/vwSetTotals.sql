CREATE OR ALTER VIEW [dbo].[vwSetTotals]
AS
	SELECT	Sets.SetId AS SetId
			,Sets.Code
			,Sets.Name
			,Sets.ReleaseDate
			,Sets.LastUpdated
			,Sets.IsTracked
			,InventoryCounts.InventoryCount
			,CollectedBySet.CollectedCount
			,CollectedBySet.TotalCount
	FROM	Sets
	--collected per-set
	LEFT JOIN ( 
		SELECT	SetId
				,SUM(IsCollected) AS CollectedCount
				,COUNT(CardId) AS TotalCount

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

	--inventory count per-set
	LEFT JOIN (

		SELECT		Cards.SetId
					,COUNT(InventoryCards.InventoryCardId) AS InventoryCount

		FROM		InventoryCards
		INNER JOIN	Cards
			ON		InventoryCards.CardId = Cards.CardId

		GROUP BY	SetId

	) AS InventoryCounts
		ON	InventoryCounts.SetId = Sets.SetId
