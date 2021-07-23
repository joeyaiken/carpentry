using Carpentry.CarpentryData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Carpentry.CarpentryData
{

	public partial class CarpentryDataContext : DbContext
	{
		#region DbSets

		//Tables
		public DbSet<CardLegalityData> CardLegalities { get; set; }
		public DbSet<CardData> Cards { get; set; }
		public DbSet<InventoryCardStatusData> CardStatuses { get; set; }
		public DbSet<DeckCardCategoryData> DeckCardCategories { get; set; }
		public DbSet<DeckCardData> DeckCards { get; set; }
		public DbSet<DeckData> Decks { get; set; }
		public DbSet<InventoryCardData> InventoryCards { get; set; }
		public DbSet<MagicFormatData> MagicFormats { get; set; }
		public DbSet<CardRarityData> Rarities { get; set; }
		public DbSet<CardSetData> Sets { get; set; }
		public DbSet<DeckCardTagData> CardTags { get; set; }

		//Views
		public DbSet<InventoryCardByNameResult> InventoryCardByName { get; set; }
		public DbSet<InventoryCardByPrintResult> InventoryCardByPrint { get; set; }
		public DbSet<InventoryCardByUniqueResult> InventoryCardByUnique { get; set; }
		public DbSet<InventoryTotalsByStatusResult> InventoryTotalsByStatus { get; set; }

		#endregion

		protected readonly ILogger<CarpentryDataContext> _logger;

		public CarpentryDataContext(DbContextOptions<CarpentryDataContext> options, ILogger<CarpentryDataContext> logger) : base(options)
		{
			_logger = logger;
			//DB should be set on the configuration page
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Associations

			//Deck -- DeckCard
			modelBuilder.Entity<DeckData>()
				.HasMany(x => x.Cards)
				.WithOne(x => x.Deck)
				.HasForeignKey(x => x.DeckId);

			//DeckCard -- InventoryCard
			modelBuilder.Entity<DeckCardData>()
				.HasOne(x => x.InventoryCard)
				.WithMany(x => x.DeckCards)
				.HasForeignKey(x => x.InventoryCardId);

			//InventoryCard -- Card
			modelBuilder.Entity<InventoryCardData>()
				.HasOne(x => x.Card)
				.WithMany(x => x.InventoryCards)
				.HasForeignKey(x => x.CardId);

			//InventoryCard -- Status
			modelBuilder.Entity<InventoryCardData>()
				.HasOne(x => x.Status)
				.WithMany(x => x.Cards)
				.HasForeignKey(x => x.InventoryCardStatusId);

			//Card -- Set
			modelBuilder.Entity<CardData>()
				.HasOne(x => x.Set)
				.WithMany(x => x.Cards)
				.HasForeignKey(x => x.SetId);

			//Card -- Rarity
			modelBuilder.Entity<CardData>()
				.HasOne(x => x.Rarity)
				.WithMany(x => x.Cards)
				.HasForeignKey(x => x.RarityId);

			//magic format -- card legality
			modelBuilder.Entity<CardLegalityData>()
				.HasOne(x => x.Format)
				.WithMany(x => x.LegalCards)
				.HasForeignKey(x => x.FormatId);

			//card -- card legality
			modelBuilder.Entity<CardLegalityData>()
				.HasOne(x => x.Card)
				.WithMany(x => x.Legalities)
				.HasForeignKey(x => x.CardId);

			//deck card -- category
			modelBuilder.Entity<DeckCardData>()
				.HasOne(x => x.Category)
				.WithMany(x => x.Cards)
				.HasForeignKey(x => x.CategoryId);

			//Deck -- MagicFormat
			modelBuilder.Entity<DeckData>()
				.HasOne(x => x.Format)
				.WithMany(x => x.Decks)
				.HasForeignKey(x => x.MagicFormatId);

			modelBuilder.Entity<DeckCardTagData>()
				.HasOne(x => x.Deck)
				.WithMany(x => x.Tags)
				.HasForeignKey(x => x.DeckId);

			modelBuilder.Entity<DeckCardData>()
				.HasIndex(dc => dc.InventoryCardId)
				.IsUnique();

			#endregion

			#region Views

			modelBuilder.Entity<InventoryCardByNameResult>(eb =>
			{
				eb.HasNoKey();
				eb.ToView("vwInventoryCardsByName");
			});

			modelBuilder.Entity<InventoryCardByPrintResult>(eb =>
			{
				eb.HasNoKey();
				eb.ToView("vwInventoryCardsByPrint");
			});
			modelBuilder.Entity<InventoryCardByUniqueResult>(eb =>
			{
				eb.HasNoKey();
				eb.ToView("vwInventoryCardsByUnique");
			});
			//modelBuilder.Entity<InventoryTotalsByStatusResult>(eb =>
			//{
			//    eb.HasNoKey();
			//    eb.ToView("vwInventoryTotalsByStatus");
			//});

			modelBuilder.Entity<InventoryTotalsByStatusResult>(eb =>
			{
				eb.HasNoKey();
				eb.ToView("vwInventoryTotalsByStatus");
			});

			#endregion

			#region procs?

			//modelBuilder.Entity<DataTrimmingTipsResult>(eb => eb.HasNoKey());
			//modelBuilder.Entity<DataTrimmingTipsCountResult>(eb => eb.HasNoKey());

			#endregion
		}
	}
}
