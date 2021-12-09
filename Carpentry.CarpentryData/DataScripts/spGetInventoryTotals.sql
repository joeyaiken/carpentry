CREATE OR ALTER PROCEDURE [dbo].[spGetInventoryTotals] 
AS
    SELECT	SUM(Price) AS TotalPrice
         ,COUNT(InventoryCardId) AS TotalCount
         , SUM(Price) / COUNT(InventoryCardId) AS PricePerCard
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
    ) As PricedItems