﻿CREATE OR ALTER VIEW [dbo].[vwAllInventoryCards]
AS

	SELECT	ic.InventoryCardId
			,ic.CardId
			,ic.InventoryCardStatusId
			,ic.IsFoil
			--,c.CardId
			,c.Cmc
			,c.ManaCost
			,c.Name
			,c.RarityId
			,c.SetId
			,c.Text
			,c.Type
			,c.MultiverseId
			,CASE WHEN ic.IsFoil = 1 THEN c.PriceFoil ELSE c.Price END AS Price
			,c.ImageUrl
			,c.CollectorNumber
			,c.TixPrice
			,c.Color
			,c.ColorIdentity
			,d.Name AS DeckName

	FROM	InventoryCards ic
	JOIN	Cards c
		ON	ic.CardId = c.CardId
	LEFT JOIN	DeckCards dc
		ON		dc.InventoryCardId = ic.InventoryCardId
	LEFT JOIN	Decks d
		ON		dc.DeckId = d.DeckId