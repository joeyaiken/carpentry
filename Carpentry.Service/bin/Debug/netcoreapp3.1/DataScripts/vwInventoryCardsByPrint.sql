CREATE OR ALTER VIEW [dbo].[vwInventoryCardsByPrint] AS

	SELECT	c.CardId
			,s.Code AS SetCode
			,c.Name
			,c.CollectorNumber
			,c.Type
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			,c.ImageUrl
			--added
			,c.MultiverseId
			,c.Text
			,c.Price
			,c.PriceFoil
			,c.TixPrice
			,c.Color
			,c.ColorIdentity


			--
			,ISNULL(Counts.OwnedCount,0) AS OwnedCount
	FROM	Cards c
	JOIN	Sets s
		ON	c.SetId = s.SetId
	LEFT JOIN (
		SELECT		ic.CardId
					,COUNT(ic.InventoryCardId) AS OwnedCount
		FROM		InventoryCards ic
		GROUP BY	ic.CardId
	) AS Counts
		ON	c.CardId = Counts.CardId
