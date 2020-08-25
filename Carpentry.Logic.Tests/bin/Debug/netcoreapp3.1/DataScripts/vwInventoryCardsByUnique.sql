CREATE VIEW [dbo].[vwInventoryCardsByUnique] AS
	/*
	Should this include things I DO NOT own?
		Do I care that I don't have a FOIL SHOWCASE of some shit?
		I think it should only include things I do actually own
	*/
	SELECT	c.Id AS CardId
			,s.Code AS SetCode
			,c.Name
			,c.Type
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
			,Totals.CardCount
			,c.ImageUrl
	FROM (
		SELECT	ic.CardId
				,ic.IsFoil
				,COUNT(ic.Id) as CardCount
		FROM	InventoryCards ic
		GROUP BY ic.CardId, ic.IsFoil
	) AS Totals
	JOIN		Cards c
		ON		c.Id = Totals.CardId
	JOIN		Rarities r
		ON		c.RarityId = r.Id
	JOIN		Sets s
		ON		c.SetId = s.Id
--GO


