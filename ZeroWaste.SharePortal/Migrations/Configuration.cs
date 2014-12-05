using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ZeroWaste.SharePortal.Models.Data.ZeroWasteData>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ZeroWaste.SharePortal.Models.Data.ZeroWasteData context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.ListingCategories.AddOrUpdate(
                category => category.Name,
                new ListingCategory { Name = ListingCategory.ShareAndSwap },
                new ListingCategory { Name = ListingCategory.DoThingsTogether },
                new ListingCategory { Name = ListingCategory.BorrowThings});

            context.SaveChanges();

            var shareCat = context.ListingCategories.Single(x => x.Name == ListingCategory.ShareAndSwap);
            var togetherCat = context.ListingCategories.Single(x => x.Name == ListingCategory.DoThingsTogether);
            var borrowCat = context.ListingCategories.Single(x => x.Name == ListingCategory.BorrowThings);

            context.ListingIcons.AddOrUpdate(
                i => i.Name,
                new ListingIcon { Name = "BorrowBikeShare01", Description = "Borrow - Bikes", Category = borrowCat, IconPath = "~/content/images/icons/BorrowBikeShare01.png" },
                new ListingIcon { Name = "BorrowToolLibrary02", Description = "Borrow - Tools", Category = borrowCat, IconPath = "~/content/images/icons/BorrowToolLibrary02.png" },
                new ListingIcon { Name = "BorrowNappyLibrary03", Description = "Borrow - Baby", Category = borrowCat, IconPath = "~/content/images/icons/BorrowNappyLibrary03.png" },
                new ListingIcon { Name = "BorrowToyLibrary04", Description = "Borrow - Toys", Category = borrowCat, IconPath = "~/content/images/icons/BorrowToyLibrary04.png" },
                new ListingIcon { Name = "ShareSwapBookSwap01", Description = "Share & Swap - Books", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapBookSwap01.png" },
                new ListingIcon { Name = "ShareSwapAutoPart02", Description = "Share & Swap - Auto Parts", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapAutoPart02.png" },
                new ListingIcon { Name = "ShareSwapRepairReuse03", Description = "Share & Swap - Repair", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapRepairReuse03.png" },
                new ListingIcon { Name = "ShareSwapClothes04", Description = "Share & Swap - Clothes", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapClothes04.png" },
                new ListingIcon { Name = "ShareSwapFruitVeg05", Description = "Share & Swap - Fruit & Veg", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapFruitVeg05.png" },
                new ListingIcon { Name = "ShareSwapProduce06", Description = "Share & Swap - Produce", Category = shareCat, IconPath = "~/content/images/icons/ShareSwapProduce06.png" },
                new ListingIcon { Name = "TogetherCommGarden01", Description = "Together - Community Garden", Category = togetherCat, IconPath = "~/content/images/icons/TogetherCommGarden01.png" },
                new ListingIcon { Name = "TogetherCommKitchen02", Description = "Together - Community Kitchen", Category = togetherCat, IconPath = "~/content/images/icons/TogetherCommKitchen02.png" },
                new ListingIcon { Name = "TogetherBackyard03", Description = "Together - Backyard", Category = togetherCat, IconPath = "~/content/images/icons/TogetherBackyard03.png" },
                new ListingIcon { Name = "TogetherLand04", Description = "Together - Land", Category = togetherCat, IconPath = "~/content/images/icons/TogetherLand04.png" },
                new ListingIcon { Name = "TogetherMensShed05", Description = "Together - Mens Shed", Category = togetherCat, IconPath = "~/content/images/icons/TogetherMensShed05.png" },
                new ListingIcon { Name = "TogetherSharedSpace06", Description = "Together - Shared Space", Category = togetherCat, IconPath = "~/content/images/icons/TogetherSharedSpace06.png" },
                new ListingIcon { Name = "TogetherSkillShare07", Description = "Together - Skill Share", Category = togetherCat, IconPath = "~/content/images/icons/TogetherSkillShare07.png" }
            );
        }
    }
}
