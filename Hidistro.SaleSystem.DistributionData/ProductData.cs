﻿namespace Hidistro.SaleSystem.DistributionData
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Member;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ProductData : ProductSubsiteProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

        public override IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId)
        {
            IList<AttributeInfo> list = new List<AttributeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM distro_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2) AND ValueId IN (SELECT ValueId FROM Hishop_ProductAttributes WHERE ProductId IN (SELECT ProductId FROM distro_Products WHERE DistributorUserId = @DistributorUserId)) ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM distro_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2 AND AttributeId IN (SELECT AttributeId FROM Hishop_ProductAttributes WHERE ProductId IN (SELECT ProductId FROM distro_Products WHERE DistributorUserId = @DistributorUserId)) ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
                while (reader.Read())
                {
                    AttributeValueInfo item = new AttributeValueInfo();
                    item.ValueId = (int) reader["ValueId"];
                    item.AttributeId = (int) reader["AttributeId"];
                    item.DisplaySequence = (int) reader["DisplaySequence"];
                    item.ValueStr = (string) reader["ValueStr"];
                    if (reader["ImageUrl"] != DBNull.Value)
                    {
                        item.ImageUrl = (string) reader["ImageUrl"];
                    }
                    list2.Add(item);
                }
                if (!reader.NextResult())
                {
                    return list;
                }
                while (reader.Read())
                {
                    AttributeInfo info2 = new AttributeInfo();
                    info2.AttributeId = (int) reader["AttributeId"];
                    info2.AttributeName = (string) reader["AttributeName"];
                    info2.DisplaySequence = (int) reader["DisplaySequence"];
                    info2.TypeId = (int) reader["TypeId"];
                    info2.UsageMode = (AttributeUseageMode) ((int) reader["UsageMode"]);
                    info2.UseAttributeImage = (bool) reader["UseAttributeImage"];
                    foreach (AttributeValueInfo info3 in list2)
                    {
                        if (info2.AttributeId == info3.AttributeId)
                        {
                            info2.AttributeValues.Add(info3);
                        }
                    }
                    list.Add(info2);
                }
            }
            return list;
        }

        public override DataTable GetBrandsByCategoryId(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_ProductTypeBrands P inner join Hishop_BrandCategories B ON P.BrandId=B.BrandId WHERE ProductTypeId=(SELECT AssociatedProductType FROM distro_Categories WHERE CategoryId=@CategoryId) AND B.BrandId IN (SELECT BrandId FROM distro_Products WHERE DistributorUserId = @DistributorUserId AND SaleStatus = 1)");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            int num = HiContext.Current.SiteSettings.UserId.Value;
            string filter = ProductSubsiteProvider.BuildProductBrowseQuerySearch(query) + string.Format(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
            string selectFields = "ProductId,ProductName,ProductCode,SaleCounts,ShortDescription,MarketPrice,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,Stock";
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                selectFields = (selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + string.Format(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", user.GradeId, num)) + string.Format(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num) + string.Format(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", num, memberDiscount);
            }
            else
            {
                selectFields = selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + " , 0 AS RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", filter, selectFields);
        }

        public override DataTable GetCounDownProducList(int maxnum)
        {
            DataTable table = new DataTable();
            string query = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,EndDate,ThumbnailUrl160 from vw_distro_CountDown where datediff(dd,EndDate,getdate())<0 AND DistributorUserId={0} ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={1}) order by EndDate desc", HiContext.Current.SiteSettings.UserId.Value, 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CountDownInfo GetCountDownInfo(int productId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM distro_CountDown WHERE EndDate>'{0}' AND ProductId=@ProductId AND DistributorUserId = @DistributorUserId", DateTime.Now));
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            string filter = string.Format(" EndDate>'{0}' AND DistributorUserId={1} AND ProductId IN(SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={2})", DateTime.Now, HiContext.Current.SiteSettings.UserId.Value, HiContext.Current.SiteSettings.UserId.Value);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_CountDown", "CountDownId", filter, "*");
        }

        public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity AND DistributorUserId=@DistributorUserId;if @price IS NULL SELECT @price = max(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;select @price");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            return (decimal) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override GiftInfo GetGift(int giftId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select d_Name as [Name],d_Title as Title,d_Meta_Description as Meta_Description,d_Meta_Keywords as Meta_Keywords,d_NeedPoint as NeedPoint,GiftId,ShortDescription,Unit, LongDescription,CostPrice,ImageUrl, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410, PurchasePrice,MarketPrice,IsDownLoad from vw_distro_Gifts where GiftId=@GiftId and d_DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            GiftInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGift(reader);
                }
            }
            return info;
        }

        public override DataTable GetGroupByProductList(int maxnum)
        {
            DataTable table = new DataTable();
            string query = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220, P.ProductId,G.[Count],G.Price from vw_distro_BrowseProductList as P inner join distro_GroupBuy as S on P.ProductId=s.ProductId inner join  distro_GroupBuyCondition as G on G.GroupBuyId=S.GroupBuyId where datediff(dd,S.EndDate,getdate())<0 and  S.DistributorUserId={0} and P.SaleStatus={0} order by S.EndDate desc", HiContext.Current.SiteSettings.UserId.Value, 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataSet GetGroupByProductList(ProductBrowseQuery query, out int count)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_GroupBuyProducts_Get");
            string str = string.Format("SELECT GroupBuyId,ProductId,EndDate FROM distro_GroupBuy WHERE EndDate>'{0}' AND Status={1} AND DistributorUserId={2} AND ProductId IN(SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={2}) ORDER BY DisplaySequence DESC", DateTime.Now, 1, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, str);
            this.database.AddOutParameter(storedProcCommand, "TotalGroupBuyProducts", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            count = (int) this.database.GetParameterValue(storedProcCommand, "TotalGroupBuyProducts");
            return set;
        }

        public override DataTable GetLineItems(int productId, int maxNum)
        {
            DataTable table = new DataTable();
            string query = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.distro_OrderItems as items left join distro_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  and items.DistributorUserId=@DistributorUserId order by orders.PayDate desc", 1, 4);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetOnlineGifts(Pagination page)
        {
            string selectFields = "GiftId,d_Name as [Name],ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220, MarketPrice,d_NeedPoint as NeedPoint";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_Gifts", "GiftId", "d_NeedPoint > 0 and d_DistributorUserId=" + HiContext.Current.SiteSettings.UserId.Value, selectFields);
        }

        public override int GetOrderCount(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4 AND DistributorUserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            Member user;
            int memberDiscount = 100;
            int gradeId = 0;
            int num3 = HiContext.Current.SiteSettings.UserId.Value;
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                user = HiContext.Current.User as Member;
                memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                gradeId = user.GradeId;
            }
            ProductBrowseInfo info = new ProductBrowseInfo();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE distro_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId;");
            builder.Append(" SELECT dp.*, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5, p.LowestSalePrice, p.PenetrationStatus");
            builder.Append(",CASE WHEN dp.BrandId IS NULL THEN NULL ELSE (SELECT bc.BrandName FROM Hishop_BrandCategories bc WHERE bc.BrandId=dp.BrandId) END AS BrandName");
            builder.Append(" FROM distro_Products dp JOIN Hishop_Products p ON dp.ProductId = p.ProductId  where dp.ProductId=@ProductId AND dp.DistributorUserId = @DistributorUserId;");
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                user = HiContext.Current.User as Member;
                memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1", user.GradeId, num3);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num3);
                builder.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0})*{1}/100 END AS SalePrice", num3, memberDiscount);
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS SalePrice", num3);
            }
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            if (maxReviewNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM distro_ProductReviews where ProductId=@ProductId AND DistributorUserId=@DistributorUserId ORDER BY ReviewId DESC;", maxReviewNum);
            }
            if (maxConsultationNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM distro_ProductConsultations where ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
            }
            builder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
            builder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            builder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
            builder.AppendFormat(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180 FROM distro_Products WHERE SaleStatus = {0}", 1);
            builder.AppendFormat(" AND DistributorUserId = {0}  AND ProductId IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {1} AND DistributorUserId = {0})", num3, productId);
            builder.AppendFormat(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180 FROM distro_Products WHERE SaleStatus = {0}", 1);
            builder.AppendFormat(" AND DistributorUserId = {0} AND ProductId<>{1} AND CategoryId = (SELECT CategoryId FROM distro_Products WHERE ProductId={1} AND SaleStatus = {2} AND DistributorUserId = {0})", num3, productId, 1);
            builder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {0} AND DistributorUserId = {1})", productId, num3);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, num3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataRow current;
                if (reader.Read())
                {
                    info.Product = DataMapper.PopulateProduct(reader);
                    if (reader["BrandName"] != DBNull.Value)
                    {
                        info.BrandName = (string) reader["BrandName"];
                    }
                }
                if (reader.NextResult() && (info.Product != null))
                {
                    while (reader.Read())
                    {
                        info.Product.Skus.Add((string) reader["SkuId"], DataMapper.PopulateSKU(reader));
                    }
                }
                if (maxReviewNum.HasValue && reader.NextResult())
                {
                    info.DBReviews = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (maxConsultationNum.HasValue && reader.NextResult())
                {
                    info.DBConsultations = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && (info.Product != null))
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        DataTable table2 = table.Clone();
                        IEnumerator enumerator = table.Rows.GetEnumerator();
                        {
                            while (enumerator.MoveNext())
                            {
                                current = (DataRow) enumerator.Current;
                                bool flag = false;
                                if (table2.Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in table2.Rows)
                                    {
                                        if (((int) row2["AttributeId"]) == ((int) current["AttributeId"]))
                                        {
                                            DataRow row4;
                                            flag = true;
                                            (row4 = row2)["ValueStr"] = row4["ValueStr"] + ", " + current["ValueStr"];
                                        }
                                    }
                                }
                                if (!flag)
                                {
                                    DataRow row = table2.NewRow();
                                    row["AttributeId"] = current["AttributeId"];
                                    row["AttributeName"] = current["AttributeName"];
                                    row["ValueStr"] = current["ValueStr"];
                                    table2.Rows.Add(row);
                                }
                            }
                        }
                        info.DbAttribute = table2;
                    }
                }
                if (reader.NextResult())
                {
                    info.DbSKUs = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult())
                {
                    info.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                while (reader.Read())
                {
                    current = info.DbCorrelatives.NewRow();
                    current["ProductId"] = reader["ProductId"];
                    current["ProductName"] = reader["ProductName"];
                    current["ThumbnailUrl60"] = reader["ThumbnailUrl60"];
                    current["ThumbnailUrl100"] = reader["ThumbnailUrl100"];
                    current["ThumbnailUrl160"] = reader["ThumbnailUrl160"];
                    current["ThumbnailUrl180"] = reader["ThumbnailUrl180"];
                    info.DbCorrelatives.Rows.Add(current);
                }
            }
            return info;
        }

        public override int GetProductConsultationNumber(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductConsultations WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetProductConsultations(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId = {0}", productId);
            builder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
            builder.Append(" AND ReplyUserId IS NOT NULL ");
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_ProductConsultations", "ConsultationId", builder.ToString(), "*");
        }

        public override GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND Status = @Status; SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=(SELECT GroupBuyId FROM distro_GroupBuy WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND Status=@Status)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGroupBuy(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    GropBuyConditionInfo item = new GropBuyConditionInfo();
                    item.Count = (int) reader["Count"];
                    item.Price = (decimal) reader["Price"];
                    if (info != null)
                    {
                        info.GroupBuyConditions.Add(item);
                    }
                }
            }
            return info;
        }

        public override int GetProductReviewNumber(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductReviews WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DataTable GetProductReviews(int maxNum)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top " + maxNum + " review.*,products.ProductName,products.ThumBnailUrl40,products.ThumBnailUrl60,products.ThumBnailUrl100,products.ThumBnailUrl160,products.ThumBnailUrl180,products.ThumBnailUrl220 from distro_ProductReviews as review inner join distro_Products as products on review.ProductId=products.ProductId where review.DistributorUserId={0} order by review.ReviewDate desc", HiContext.Current.SiteSettings.UserId.Value));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetProductReviews(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId = {0}", productId);
            builder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_ProductReviews", "reviewId", builder.ToString(), "*");
        }

        public override ProductInfo GetProductSimpleInfo(int productId)
        {
            ProductInfo info = null;
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT dp.*, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,p.LowestSalePrice, p.PenetrationStatus");
            builder.Append(",CASE WHEN dp.BrandId IS NULL THEN NULL ELSE (SELECT bc.BrandName FROM Hishop_BrandCategories bc WHERE bc.BrandId=dp.BrandId) END AS BrandName");
            builder.Append(" FROM distro_Products dp JOIN Hishop_Products p ON dp.ProductId = p.ProductId  where dp.ProductId=@ProductId AND dp.DistributorUserId = @DistributorUserId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
            }
            return info;
        }

        public override DataTable GetSaleProductRanking(int maxNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TOP {0} ProductId, ProductName, ProductCode, SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, SalePrice, MarketPrice FROM vw_distro_BrowseProductList", maxNum) + string.Format(" WHERE SaleStatus = {0} and DistributorUserId = {1} ORDER BY SaleCounts DESC", 1, HiContext.Current.SiteSettings.UserId.Value));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSubjectList(SubjectListQuery query)
        {
            StringBuilder builder = new StringBuilder();
            int num = HiContext.Current.SiteSettings.UserId.Value;
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,SaleCounts,ShortDescription,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220, MarketPrice", query.MaxNum);
                builder.AppendFormat(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num);
                builder.AppendFormat(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", user.GradeId, num);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num);
                builder.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", num, memberDiscount);
            }
            else
            {
                builder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,SaleCounts,ShortDescription,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220, MarketPrice", query.MaxNum);
                builder.AppendFormat(" ,(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num);
                builder.Append(" , 0 AS RankPrice");
            }
            builder.Append(" FROM vw_distro_BrowseProductList p WHERE ");
            builder.Append(ProductSubsiteProvider.BuildProductSubjectQuerySearch(query));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), DataHelper.CleanSearchString(query.SortOrder.ToString()));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetSubjectProduct(SubjectListQuery query)
        {
            int num = HiContext.Current.SiteSettings.UserId.Value;
            string filter = ProductSubsiteProvider.BuildProductSubjectQuerySearch(query) + string.Format(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
            string selectFields = "ProductId,ProductName,ProductCode,SaleCounts,ShortDescription,MarketPrice,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,Stock";
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                selectFields = (selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + string.Format(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", user.GradeId, num)) + string.Format(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num) + string.Format(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", num, memberDiscount);
            }
            else
            {
                selectFields = selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + " , 0 AS RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", filter, selectFields);
        }

        public override DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
        {
            int num = HiContext.Current.SiteSettings.UserId.Value;
            string filter = ProductSubsiteProvider.BuildUnSaleProductBrowseQuerySearch(query) + string.Format(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
            string selectFields = "ProductId,ProductName,ProductCode,SaleCounts, ShortDescription,MarketPrice,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220, Stock";
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                selectFields = (selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + string.Format(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", user.GradeId, num)) + string.Format(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num) + string.Format(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", num, memberDiscount);
            }
            else
            {
                selectFields = selectFields + string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", num) + " , 0 AS RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", filter, selectFields);
        }

        public override DataTable GetVistiedProducts(IList<int> productIds)
        {
            if (productIds.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            for (int i = 0; i < productIds.Count; i++)
            {
                str = str + productIds[i] + ",";
            }
            str = str.Remove(str.Length - 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,SaleCounts, ShortDescription,MarketPrice,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100 FROM  distro_Products WHERE DistributorUserId = {0} AND ProductId IN({1})", HiContext.Current.SiteSettings.UserId.Value, str));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductConsultations(ProductId, UserId,DistributorUserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId,@DistributorUserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productConsultation.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, productConsultation.UserId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, productConsultation.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, productConsultation.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ConsultationText", DbType.String, productConsultation.ConsultationText);
            this.database.AddInParameter(sqlStringCommand, "ConsultationDate", DbType.DateTime, productConsultation.ConsultationDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool InsertProductReview(ProductReviewInfo review)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductReviews (ProductId, UserId,DistributorUserId, ReviewText, UserName, UserEmail, ReviewDate) VALUES(@ProductId, @UserId,@DistributorUserId, @ReviewText, @UserName, @UserEmail, @ReviewDate)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, review.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ReviewText", DbType.String, review.ReviewText);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, review.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, review.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ReviewDate", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool IsBuyProduct(int productId)
        {
            bool flag = false;
            try
            {
                string query = "select top 1 orders.UserId from distro_OrderItems as items left join distro_Orders orders on items.OrderId=orders.OrderId where ProductId=@ProductId and orders.OrderStatus=@OrderStatus and  orders.DistributorUserId=@DistributorUserId and orders.UserId=@UserId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
                this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (reader.Read())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception)
            {
                flag = true;
            }
            return flag;
        }

        public override void LoadProductReview(int productId, out int buyNum, out int reviewNum)
        {
            buyNum = 0;
            reviewNum = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId AND DistributorUserId=@DistributorUserId SELECT ISNULL(SUM(Quantity), 0) FROM distro_OrderItems WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND OrderId IN" + string.Format(" (SELECT OrderId FROM distro_Orders WHERE UserId = @UserId AND DistributorUserId=@DistributorUserId AND OrderStatus != {0} AND OrderStatus != {1})", 1, 4));
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    reviewNum = (int) reader[0];
                }
                reader.NextResult();
                if (reader.Read())
                {
                    buyNum = (int) reader[0];
                }
            }
        }
    }
}

