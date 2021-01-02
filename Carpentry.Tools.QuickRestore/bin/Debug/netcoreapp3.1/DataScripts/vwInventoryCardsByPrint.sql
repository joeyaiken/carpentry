CREATE OR ALTER VIEW [dbo].[vwInventoryCardsByPrint] AS
	--SELECT * FROM [dbo].[vwInventoryCardsByPrint]
	SELECT	c.CardId AS Id
			,c.CardId
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.Text
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			,c.ImageUrl
			,c.CollectorNumber
			,c.Color
			,c.ColorIdentity
			--prices
			,c.Price
			,c.PriceFoil
			,c.TixPrice
			--counts
			,ISNULL(Counts.OwnedCount,0) AS OwnedCount
			,0 AS DeckCount
			,NULL AS IsFoil
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
