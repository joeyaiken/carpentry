USE [CarpentryData]
GO

/****** Object:  View [dbo].[vwInventoryCardsByMid]    Script Date: 6/22/2020 2:25:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwInventoryCardsByMid] AS
	SELECT	c.Id AS [MultiverseId]
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			--the image of the most recent card
			,cv.ImageUrl
			,Counts.OwnedCount
	FROM (
		SELECT		c.Id
					,COUNT(ic.Id) AS OwnedCount
		FROM		Cards c
		LEFT JOIN	InventoryCards ic
			ON		c.Id = ic.MultiverseId
		GROUP BY	c.Id
	) AS	Counts
	JOIN	Cards c
		ON	Counts.Id = c.Id
	JOIN	CardVariants cv
		ON	c.Id = cv.CardId
		AND cv.CardVariantTypeId = 1 -- 1 == normal, every card will have a normal variant
	JOIN	Sets s
		ON	c.SetId = s.Id
GO

CREATE VIEW [dbo].[vwInventoryCardsByName]
AS
	--What are the unique card names, and how many do I own of each?
	--inventory cards can have statuses, maybe this should filter out buy-list/sell-list cards
		SELECT	c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			,cv.ImageUrl
			,Counts.OwnedCount

	FROM (

		SELECT		MAX(c.Id) AS MostRecentId
					,c.Name
					,COUNT(ic.Id) AS OwnedCount

		FROM		Cards c
		LEFT JOIN	InventoryCards ic
			ON		c.Id = ic.MultiverseId

		GROUP BY	c.Name

	) as Counts

	JOIN	Cards c
		ON	Counts.MostRecentId = c.Id

	JOIN	CardVariants cv
		ON	Counts.MostRecentId = cv.CardId
		AND cv.CardVariantTypeId = 1 -- 1 == normal, every card will have a normal variant
GO

CREATE VIEW [dbo].[vwInventoryCardsUniquePrints] AS
	/*
	Should this include things I DO NOT own?
		Do I care that I don't have a FOIL SHOWCASE of some shit?
		I think it should only include things I do actually own
	*/
	SELECT	c.Id AS [MultiverseId]
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			--,r.Name AS Rarity
			,c.RarityId
			,vt.Name AS VariantName
			,Totals.IsFoil
			,CASE WHEN Totals.IsFoil = 1
				THEN PriceFoil
				ELSE Price
			END AS Price
			,Totals.CardCount
			,cv.ImageUrl
	FROM (
		SELECT	ic.MultiverseId
				,ic.IsFoil
				,VariantTypeId
				,COUNT(ic.Id) as CardCount

		FROM	InventoryCards ic
		JOIN	VariantTypes
			ON	ic.VariantTypeId = VariantTypes.Id

		GROUP BY ic.MultiverseId, ic.VariantTypeId, ic.IsFoil
	) AS Totals
	JOIN		Cards c
		ON		c.Id = Totals.MultiverseId
	LEFT JOIN	CardVariants cv
		ON		Totals.VariantTypeId = cv.CardVariantTypeId
		AND		Totals.MultiverseId = cv.CardId
	LEFT JOIN	VariantTypes vt
		ON		Totals.VariantTypeId = vt.Id
	JOIN		Rarities r
		ON		c.RarityId = r.Id
	JOIN		Sets s
		ON		c.SetId = s.Id
GO

CREATE VIEW [dbo].[vwInventoryTotalsByStatus] AS
	SELECT	cs.Id AS StatusId
			,cs.Name AS StatusName
			,ISNULL(SUM(Price), 0) AS TotalPrice
			,COUNT(PricedItems.Id) AS TotalCount
	FROM (

		SELECT	ic.Id
				,MultiverseId
				,CASE WHEN ic.IsFoil = 1
					THEN PriceFoil
					ELSE Price
				END AS Price
				,ic.InventoryCardStatusId

		FROM	InventoryCards ic
		JOIN	CardVariants cv
			ON	ic.VariantTypeId = cv.CardVariantTypeId
			AND	ic.MultiverseId = cv.CardId

	) As PricedItems
	RIGHT JOIN	CardStatuses cs
		ON		PricedItems.InventoryCardStatusId = cs.Id
	GROUP BY cs.Name, cs.Id
GO
