CREATE OR ALTER PROCEDURE [dbo].[spGetTotalTrimCount]
	@UsedCardsToKeep INT,
	@UnusedCardsToKeeep INT,
	@Set VARCHAR(MAX)
AS

	SELECT	SUM(RecomendTrimming) AS TotalRecomendedTrimming
	FROM	(
		SELECT		(u.OwnedCount - @UnusedCardsToKeeep) AS RecomendTrimming
		FROM		vwInventoryCardsByName n
		LEFT JOIN	vwInventoryCardsByUnique u
				ON	n.Name = u.Name
		WHERE		u.OwnedCount IS NOT NULL
				AND	n.DeckCount = 0
				AND	u.OwnedCount > @UnusedCardsToKeeep
				AND	(@Set IS NULL OR u.SetCode = @Set)

		UNION ALL

		SELECT		(u.OwnedCount - @UsedCardsToKeep) AS RecomendTrimming
		FROM		vwInventoryCardsByName n
		LEFT JOIN	vwInventoryCardsByUnique u
				ON	n.Name = u.Name
		WHERE		u.OwnedCount IS NOT NULL
				AND	n.DeckCount > 0
				AND	(u.OwnedCount - u.DeckCount) > @UsedCardsToKeep
				AND	(@Set IS NULL OR u.SetCode = @Set)
	) AS Totals