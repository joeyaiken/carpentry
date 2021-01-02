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
			,ISNULL(Counts.TotalCount,0) AS TotalCount
			,ISNULL(Counts.DeckCount,0) AS DeckCount
			,ISNULL(Counts.InventoryCount,0) AS InventoryCount
			,ISNULL(Counts.SellCount,0) AS SellCount
			--
			,NULL AS IsFoil
	FROM	Cards c
	JOIN	Sets s
		ON	c.SetId = s.SetId
	LEFT JOIN vwCardTotals AS Counts
		ON	c.CardId = Counts.CardId
