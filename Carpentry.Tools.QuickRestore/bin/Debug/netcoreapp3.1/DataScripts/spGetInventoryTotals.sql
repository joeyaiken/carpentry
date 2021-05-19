CREATE OR ALTER PROCEDURE [dbo].[spGetInventoryTotals] 
AS
	SELECT	SUM(Price) AS TotalPrice
			,COUNT(InventoryCardId) AS TotalCount
	FROM (

		SELECT	ic.InventoryCardId
				,ic.CardId
				,CASE WHEN ic.IsFoil = 1
					THEN PriceFoil
					ELSE Price
				END AS Price

		FROM	InventoryCards ic
		JOIN	Cards c
			ON	ic.CardId = c.CardId
		--JOIN	CardVariants cv
		--	ON	ic.VariantTypeId = cv.CardVariantTypeId
		--	AND	ic.MultiverseId = cv.CardId

	) As PricedItems