/**
** Gets total value of inventory, and total card count
**/
CREATE PROCEDURE [dbo].[spGetInventoryTotals] 
AS
	SELECT	SUM(Price) AS TotalPrice
			,COUNT(Id) AS TotalCount
	FROM (

		SELECT	ic.Id
				,CardId
				,CASE WHEN ic.IsFoil = 1
					THEN PriceFoil
					ELSE Price
				END AS Price

		FROM	InventoryCards ic
		JOIN	Cards c
			ON	ic.CardId = c.Id
		--JOIN	CardVariants cv
		--	ON	ic.VariantTypeId = cv.CardVariantTypeId
		--	AND	ic.MultiverseId = cv.CardId

	) As PricedItems
--GO


