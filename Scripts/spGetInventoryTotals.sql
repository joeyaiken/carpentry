USE [CarpentryDataRefactor]
GO

/****** Object:  StoredProcedure [dbo].[spGetInventoryTotals]    Script Date: 8/16/2020 7:15:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/**
** Gets total value of inventory, and total card count
**/
CREATE PROCEDURE [dbo].[spGetInventoryTotals] 
AS
	SELECT	SUM(Price) AS TotalPrice
			,COUNT(Id) AS TotalCount
	FROM (

		SELECT	ic.Id
				,MultiverseId
				,CASE WHEN ic.IsFoil = 1
					THEN PriceFoil
					ELSE Price
				END AS Price

		FROM	InventoryCards ic
		JOIN	CardVariants cv
			ON	ic.VariantTypeId = cv.CardVariantTypeId
			AND	ic.MultiverseId = cv.CardId

	) As PricedItems
GO


