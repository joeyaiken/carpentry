CREATE OR ALTER VIEW [dbo].[vwInventoryCardsByUnique] AS
	/*
	Should this include things I DO NOT own?
		Do I care that I don't have a FOIL SHOWCASE of some shit?
		I think it should only include things I do actually own
	*/
	SELECT	c.CardId
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.Text
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			,c.CollectorNumber
			,c.Color
			,c.ColorIdentity
			,Totals.IsFoil
			,CASE WHEN Totals.IsFoil = 1
				THEN PriceFoil
				ELSE Price
			END AS Price
			,Totals.CardCount AS OwnedCount
			,Totals.DeckCount
			,c.ImageUrl
	FROM (
		SELECT	ic.CardId
				,ic.IsFoil
				,COUNT(ic.InventoryCardId) as CardCount
				,SUM(CASE WHEN dc.DeckCardId IS NULL THEN 0 ELSE 1 END) AS DeckCount
		FROM	InventoryCards ic
	LEFT JOIN	DeckCards dc
			ON	ic.InventoryCardId = dc.InventoryCardId
		GROUP BY ic.CardId, ic.IsFoil
	) AS Totals
	JOIN		Cards c
		ON		c.CardId = Totals.CardId
	JOIN		Rarities r
		ON		c.RarityId = r.RarityId
	JOIN		Sets s
		ON		c.SetId = s.SetId
