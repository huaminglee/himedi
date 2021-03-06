﻿namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using System;
    using System.Data;

    public abstract class CategoryProvider
    {
        protected CategoryProvider()
        {
        }

        public abstract DataTable GetBrandCategories(int maxNum);
        public abstract DataTable GetBrandCategories(int categoryId, int maxNum);
        public abstract BrandCategoryInfo GetBrandCategory(int brandId);
        public abstract DataTable GetCategories();
        public abstract DataSet GetThreeLayerCategories();
        public static CategoryProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return CategorySubsiteProvider.CreateInstance();
            }
            return CategoryMasterProvider.CreateInstance();
        }
    }
}

