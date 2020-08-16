USE [CarpentryDataRefactor]
GO

/****** Object:  View [dbo].[vwInventoryCardsByPrint]    Script Date: 8/16/2020 7:14:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vwInventoryCardsByPrint] AS

	SELECT	c.Id AS CardId
			,s.Code AS SetCode
			,c.Name
			,c.CollectorNumber
			,c.Type
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			,c.ImageUrl
			,ISNULL(Counts.OwnedCount,0) AS OwnedCount
	FROM	Cards c
	JOIN	Sets s
		ON	c.SetId = s.Id
	LEFT JOIN (
		SELECT		ic.CardId
					,COUNT(ic.Id) AS OwnedCount
		FROM		InventoryCards ic
		GROUP BY	ic.CardId
	) AS Counts
		ON	c.Id = Counts.CardId

GO


