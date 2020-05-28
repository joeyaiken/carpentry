USE [CarpentryData]
GO

/*	Ideas for adding views to Carpentry

	Data-layer handles querying a view
		Maybe mostly the query service, could be the inventory service too
	
	Logic layer determines what view to query, filtering, & mapping to (shared?) DTOs

*/
--SELECT * FROM vInventoryCardsByName

--SELECT * FROM vInventoryCardsByMid

--SELECT * FROM vInventoryCardsUniquePrints


--'vInventoryCardsByUnique'."}

--SELECT * FROM vInventoryCardsByUnique

ALTER PROCEDURE [dbo].[spGetInventoryTotals] AS
	--Get all inventory cards & their price
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

ALTER VIEW [dbo].[vInventoryCardsByName] AS
	--What are the unique card names, and how many do I own of each?
	--inventory cards can have statuses, maybe this should filter out buy-list/sell-list cards
		SELECT	c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			--Rarity?
			--the image of the most recent card
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

ALTER VIEW [dbo].[vInventoryCardsByMid] AS
	SELECT	c.Id AS [MultiverseId]
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			--Rarity?
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

--vInventoryCardsUniquePrints
ALTER VIEW [dbo].[vInventoryCardsUniquePrints] AS
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
	JOIN Cards c
		ON	c.Id = Totals.MultiverseId
	LEFT JOIN	CardVariants cv
		ON	Totals.VariantTypeId = cv.CardVariantTypeId
		AND	Totals.MultiverseId = cv.CardId
	LEFT JOIN	VariantTypes vt
		ON	Totals.VariantTypeId = vt.Id
	JOIN	Sets s
		ON	c.SetId = s.Id
GO

/*Attempts at repairing bad data*/

--cards in the DB that don't have a matching variant

	--SELECT	c.Id AS [MultiverseId]
	--		,c.Name
	--		,c.Type
	--		,c.ManaCost
	--		,c.Cmc
	--		,vt.Name AS VariantName
	--		,Totals.IsFoil
	--		,CASE WHEN Totals.IsFoil = 1
	--			THEN PriceFoil
	--			ELSE Price
	--		END AS Price
	--		,Totals.CardCount
	--		,cv.ImageUrl
	--FROM (
	--	SELECT	ic.MultiverseId
	--			,ic.IsFoil
	--			,VariantTypeId
	--			--,COUNT(ic.Id) as CardCount

	--	FROM	InventoryCards ic
	--	JOIN	VariantTypes
	--		ON	ic.VariantTypeId = VariantTypes.Id

	--	GROUP BY ic.MultiverseId, ic.VariantTypeId, ic.IsFoil
	--) AS Totals
	--JOIN Cards c
	--	ON	c.Id = Totals.MultiverseId
	--LEFT JOIN	CardVariants cv
	--	ON	Totals.VariantTypeId = cv.CardVariantTypeId
	--	AND	Totals.MultiverseId = cv.CardId
	--LEFT JOIN	VariantTypes vt
	--	ON	Totals.VariantTypeId = vt.Id
	--WHERE cv.Id IS NULL

----------


--UPDATE	InventoryCards
--SET		VariantTypeId = 3
----SELECT			ic.Id

----				,VariantTypeId
----				,vt.Name AS VariantName
--		FROM	InventoryCards ic
--		JOIN	VariantTypes
--			ON	ic.VariantTypeId = VariantTypes.Id

--	LEFT JOIN	CardVariants cv
--		ON	ic.VariantTypeId = cv.CardVariantTypeId
--		AND	ic.MultiverseId = cv.CardId
--	LEFT JOIN	VariantTypes vt
--		ON	ic.VariantTypeId = vt.Id
--	WHERE cv.Id IS NULL


--SELECT * FROM VariantTypes

--3