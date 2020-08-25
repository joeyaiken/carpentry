CREATE VIEW [dbo].[vwInventoryCardsByName]
AS
	--What are the unique card names, and how many do I own of each?
	--inventory cards can have statuses, maybe this should filter out buy-list/sell-list cards

	--Note that if I don't own the most recent printing of a card, this will still show the most recent
	--For each cardn name, I want the most recent card, and owned count for each name

	SELECT	RecentCard.Name
			,RecentCard.Type
			,RecentCard.ManaCost
			,RecentCard.Cmc
			,RecentCard.ImageUrl
			,RecentCard.Color
			,RecentCard.ColorIdentity
			,Counts.OwnedCount
	FROM	(
		SELECT		c.Name
					,COUNT(ic.Id) AS OwnedCount
		FROM		Cards c
		LEFT JOIN	InventoryCards ic
				ON	ic.CardId = c.Id
		GROUP BY	c.Name
	) AS Counts
	JOIN	(
		SELECT	ROW_NUMBER() OVER(PARTITION BY c.Name ORDER BY s.ReleaseDate DESC) AS CardRank
				--,s.Name
				,c.Name
				,c.Type
				,c.ManaCost
				,c.Cmc
				,c.ImageUrl
				,c.Color
				,c.ColorIdentity
		FROM	Cards c
		JOIN	Sets s
			ON	c.SetId = s.Id
	) AS RecentCard
		ON	Counts.Name = RecentCard.Name
		AND	RecentCard.CardRank = 1
--GO


