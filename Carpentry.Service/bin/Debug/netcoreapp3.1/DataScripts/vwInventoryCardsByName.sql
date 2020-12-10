CREATE OR ALTER VIEW [dbo].[vwInventoryCardsByName]
AS
	--What are the unique card names, and how many do I own of each?
	--inventory cards can have statuses, maybe this should filter out buy-list/sell-list cards

	--Note that if I don't own the most recent printing of a card, this will still show the most recent
	--For each cardn name, I want the most recent card, and owned count for each name

	SELECT	RecentCard.CardId
			,RecentCard.Name
			,RecentCard.Type
			,RecentCard.Text
			,RecentCard.ManaCost
			,RecentCard.Cmc
			,RecentCard.ImageUrl
			,RecentCard.Color
			,RecentCard.ColorIdentity
			,RecentCard.RarityId
			,RecentCard.CollectorNumber
			--,RecentCard.SetId
			,s.Code AS [SetCode]
			,RecentCard.MultiverseId
			,RecentCard.Price
			,RecentCard.PriceFoil
			,RecentCard.TixPrice
			,Counts.OwnedCount
			,Counts.DeckCount
	FROM	(
		SELECT		c.Name
					,COUNT(ic.InventoryCardId) AS OwnedCount
					,SUM(CASE WHEN dc.DeckCardId IS NULL THEN 0 ELSE 1 END) AS DeckCount
		FROM		Cards c
		LEFT JOIN	InventoryCards ic
				ON	ic.CardId = c.CardId
		LEFT JOIN	DeckCards dc
				ON	ic.InventoryCardId = dc.InventoryCardId
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
