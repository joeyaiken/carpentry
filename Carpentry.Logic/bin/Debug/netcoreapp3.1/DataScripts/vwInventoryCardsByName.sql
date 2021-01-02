CREATE OR ALTER VIEW [dbo].[vwInventoryCardsByName]
AS
	--SELECT * FROM [dbo].[vwInventoryCardsByName]
	--What are the unique card names, and how many do I own of each?
	--inventory cards can have statuses, maybe this should filter out buy-list/sell-list cards

	--Note that if I don't own the most recent printing of a card, this will still show the most recent
	--For each cardn name, I want the most recent card, and owned count for each name

	SELECT	RecentCard.CardId AS Id
			--card props
			,RecentCard.CardId
			,s.Code AS [SetCode]
			,RecentCard.Name
			,RecentCard.Type
			,RecentCard.Text
			,RecentCard.ManaCost
			,RecentCard.Cmc
			,RecentCard.RarityId
			,RecentCard.ImageUrl
			,RecentCard.CollectorNumber
			,RecentCard.Color
			,RecentCard.ColorIdentity
			--prices
			,RecentCard.Price
			,RecentCard.PriceFoil
			,RecentCard.TixPrice
			--counts
			,Counts.TotalCount
			,Counts.DeckCount
			,Counts.InventoryCount
			,Counts.SellCount
			--
			,NULL AS IsFoil
	FROM	(
		SELECT		c.Name
					,SUM(t.DeckCount) AS DeckCount
					,SUM(t.InventoryCount) AS InventoryCount
					,SUM(t.SellCount) AS SellCount
					,SUM(t.TotalCount) AS TotalCount

		FROM		Cards c
		LEFT JOIN	vwCardTotals t
			ON		c.CardId = t.CardId

		GROUP BY	c.Name
	) AS Counts
	JOIN	(
		SELECT	ROW_NUMBER() OVER(PARTITION BY c.Name ORDER BY s.ReleaseDate DESC, c.CollectorNumber) AS CardRank
				--,s.Name
				,c.CardId AS CardId
				,c.Name
				,c.Type
				,c.ManaCost
				,c.Cmc
				,c.ImageUrl
				,c.Color
				,c.ColorIdentity
				,c.Text
				,c.RarityId
				,c.CollectorNumber
				,c.SetId
				,c.MultiverseId
				,c.Price
				,c.PriceFoil
				,c.TixPrice
		FROM	Cards c
		JOIN	Sets s
			ON	c.SetId = s.SetId
	) AS RecentCard
		ON	Counts.Name = RecentCard.Name
		AND	RecentCard.CardRank = 1
	JOIN	dbo.Sets s
		ON	RecentCard.SetId = s.SetId