﻿CREATE VIEW [dbo].[vwInventoryTotalsByStatus] AS
	SELECT	cs.Id AS StatusId
			,cs.Name AS StatusName
			,ISNULL(SUM(Price), 0) AS TotalPrice
			,COUNT(PricedItems.Id) AS TotalCount
	FROM (

		SELECT	ic.Id
				,ic.CardId --what should this be?
				,CASE WHEN ic.IsFoil = 1
					THEN c.PriceFoil
					ELSE c.Price
				END AS Price
				,ic.InventoryCardStatusId
		FROM	InventoryCards ic
		JOIN	Cards c
			ON	ic.CardId = c.Id


	) As PricedItems
	RIGHT JOIN	CardStatuses cs
		ON		PricedItems.InventoryCardStatusId = cs.Id
	GROUP BY cs.Name, cs.Id
--GO


