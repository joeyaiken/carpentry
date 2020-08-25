CREATE VIEW [dbo].[vwSetTotals]
AS
	SELECT	Sets.Id AS SetId
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
				,COUNT(Id) AS TotalCount

		FROM (
			SELECT		Cards.Id
						,Cards.SetId
						,CASE	WHEN COUNT(InventoryCards.Id) > 0 
								THEN 1 
								ELSE 0
						END AS IsCollected
			FROM		Cards
			LEFT JOIN	InventoryCards
				ON		Cards.Id = InventoryCards.CardId
			GROUP BY	Cards.Id, Cards.SetId

		) AS CollectedCards

		GROUP BY SetId

	) AS CollectedBySet
		ON	Sets.Id = CollectedBySet.SetId

	--inventory count per-set
	LEFT JOIN (

		SELECT		Cards.SetId
					,COUNT(InventoryCards.Id) AS InventoryCount

		FROM		InventoryCards
		INNER JOIN	Cards
			ON		InventoryCards.CardId = Cards.Id

		GROUP BY	SetId

	) AS InventoryCounts
		ON	InventoryCounts.SetId = Sets.Id
--GO


