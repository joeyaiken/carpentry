CREATE VIEW [dbo].[vwInventoryTotalsByStatus] AS
	SELECT	cs.CardStatusId AS StatusId
			,cs.Name AS StatusName
			,ISNULL(SUM(Price), 0) AS TotalPrice
			,COUNT(PricedItems.InventoryCardId) AS TotalCount
	FROM (

		SELECT	ic.InventoryCardId
				,ic.CardId --what should this be?
				,CASE WHEN ic.IsFoil = 1
					THEN c.PriceFoil
					ELSE c.Price
				END AS Price
				,ic.InventoryCardStatusId
		FROM	InventoryCards ic
		JOIN	Cards c
			ON	ic.CardId = c.CardId


	) As PricedItems
	RIGHT JOIN	CardStatuses cs
		ON		PricedItems.InventoryCardStatusId = cs.CardStatusId
	GROUP BY cs.Name, cs.CardStatusId
--GO


