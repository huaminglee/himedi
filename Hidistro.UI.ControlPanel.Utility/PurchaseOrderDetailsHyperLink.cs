﻿namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PurchaseOrderDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus) this.PurchaseStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/UnPaymentPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
                    break;

                case OrderStatus.SellerAlreadySent:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/DeliveredPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
                    break;

                case OrderStatus.Closed:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
                    break;

                case OrderStatus.Finished:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/FinishedPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
                    break;
            }
            base.Render(writer);
        }

        public object PurchaseOrderId
        {
            get
            {
                if (this.ViewState["PurchaseOrderId"] == null)
                {
                    return null;
                }
                return this.ViewState["PurchaseOrderId"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["PurchaseOrderId"] = value;
                }
            }
        }

        public object PurchaseStatusCode
        {
            get
            {
                if (this.ViewState["purchaseStatusCode"] == null)
                {
                    return null;
                }
                return this.ViewState["purchaseStatusCode"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["purchaseStatusCode"] = value;
                }
            }
        }
    }
}

