﻿namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class GiftDetails : HtmlTemplatedWebControl
    {
       Button btnChage;
       int giftId;
       HiImage imgGiftImage;
       FormatedMoneyLabel lblMarkerPrice;
       Label litCurrentPoint;
       Literal litDescription;
       Literal litGiftName;
       Literal litGiftTite;
       Label litNeedPoint;
       Literal litShortDescription;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
            {
                base.GotoResourceNotFound();
            }
            this.litGiftTite = (Literal) this.FindControl("litGiftTite");
            this.litGiftName = (Literal) this.FindControl("litGiftName");
            this.lblMarkerPrice = (FormatedMoneyLabel) this.FindControl("lblMarkerPrice");
            this.litNeedPoint = (Label) this.FindControl("litNeedPoint");
            this.litCurrentPoint = (Label) this.FindControl("litCurrentPoint");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.imgGiftImage = (HiImage) this.FindControl("imgGiftImage");
            this.btnChage = (Button) this.FindControl("btnChage");
            this.btnChage.Click += new EventHandler(this.btnChage_Click);
            GiftInfo gift = null;
            gift = ProductBrowser.GetGift(this.giftId);
            if (!this.Page.IsPostBack)
            {
                this.litGiftTite.Text = gift.Name;
                this.litGiftName.Text = gift.Name;
                this.lblMarkerPrice.Money = gift.MarketPrice;
                this.litNeedPoint.Text = gift.NeedPoint.ToString();
                this.litShortDescription.Text = gift.ShortDescription;
                this.litDescription.Text = gift.LongDescription;
                this.imgGiftImage.ImageUrl = gift.ThumbnailUrl310;
                this.LoadPageSearch(gift);
            }
            if (((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)) && (gift.NeedPoint > 0))
            {
                this.btnChage.Enabled = true;
                this.btnChage.Text = "立即兑换";
                this.litCurrentPoint.Text = ((Member) HiContext.Current.User).Points.ToString();
            }
            else if (gift.NeedPoint <= 0)
            {
                this.btnChage.Enabled = false;
                this.btnChage.Text = "礼品不允许兑换";
            }
            else
            {
                this.btnChage.Enabled = false;
                this.btnChage.Text = "请登录方能兑换";
                this.litCurrentPoint.Text = string.Format("<a href=\"{0}\">请登录</a>", Globals.ApplicationPath + "/Login.aspx");
            }
        }

       void btnChage_Click(object sender, EventArgs e)
        {
            if ((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("请登录后才能购买"));
            }
            else if ((int.Parse(this.litNeedPoint.Text) <= int.Parse(this.litCurrentPoint.Text)) && ShoppingCartProcessor.AddGiftItem(this.giftId, 1))
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

       void LoadPageSearch(GiftInfo gift)
        {
            if (!string.IsNullOrEmpty(gift.Meta_Keywords))
            {
                MetaTags.AddMetaKeywords(gift.Meta_Keywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(gift.Meta_Description))
            {
                MetaTags.AddMetaDescription(gift.Meta_Description, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(gift.Title))
            {
                PageTitle.AddTitle(gift.Title, HiContext.Current.Context);
            }
            else
            {
                PageTitle.AddTitle(gift.Name, HiContext.Current.Context);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-GiftDetails.html";
            }
            base.OnInit(e);
        }
    }
}

