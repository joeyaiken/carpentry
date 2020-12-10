CREATE OR ALTER PROCEDURE [dbo].[spGetTrimmingTips]
	--@UsedCardsToKeep INT = 10,
	--@UnusedCardsToKeeep INT = 6,
	--@Set VARCHAR(MAX) = 'rix'
	@UsedCardsToKeep INT,
	@UnusedCardsToKeeep INT,
	@Set VARCHAR(MAX)
AS

	SELECT		u.CardId
				,u.IsFoil
				,u.SetCode
				,u.Name
				,u.Type
				,u.Price
				,u.OwnedCount
				,u.DeckCount
				,n.OwnedCount AS TotalOwnedCount
				,n.DeckCount AS TotalDeckCount
				,(u.OwnedCount - @UnusedCardsToKeeep) AS RecomendTrimming
				,'Not used in any decks, no nead to have more than a playset' AS Reason
	FROM		vwInventoryCardsByName n
	LEFT JOIN	vwInventoryCardsByUnique u
			ON	n.Name = u.Name
	WHERE		u.OwnedCount IS NOT NULL
			AND	n.DeckCount = 0
			AND	u.OwnedCount > @UnusedCardsToKeeep
			AND	(@Set IS NULL OR u.SetCode = @Set)
	UNION ALL

	SELECT		u.CardId
				,u.IsFoil
				,u.SetCode
				,u.Name
				,u.Type
				,u.Price
				,u.OwnedCount
				,u.DeckCount
				,n.OwnedCount AS TotalOwnedCount
				,n.DeckCount AS TotalDeckCount
				,(u.OwnedCount - @UsedCardsToKeep) AS RecomendTrimming
				,'Used in a deck, but many copies' AS Reason
	FROM		vwInventoryCardsByName n
	LEFT JOIN	vwInventoryCardsByUnique u
			ON	n.Name = u.Name
	WHERE		u.OwnedCount IS NOT NULL
			AND	n.DeckCount > 0 --Exists in a deck
			AND	(u.OwnedCount - u.DeckCount) > @UsedCardsToKeep --More than a playset not in any decks
			AND	(@Set IS NULL OR u.SetCode = @Set)
	ORDER BY	 RecomendTrimming DESC