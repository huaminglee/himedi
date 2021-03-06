﻿using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditMemberPrices : AdminPage
    {

        string productIds = string.Empty;


        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                ShowMsg("没有要修改的商品", false);
            }
            else if (!ddlMemberPrice.SelectedValue.HasValue)
            {
                ShowMsg("请选择要修改的价格", false);
            }
            else if (!ddlMemberPrice2.SelectedValue.HasValue)
            {
                ShowMsg("请选择要修改的价格", false);
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(txtOperationPrice.Text.Trim(), out result))
                {
                    ShowMsg("请输入正确的价格", false);
                }
                else if ((ddlOperation.SelectedValue == "*") && (result <= 0M))
                {
                    ShowMsg("必须乘以一个正数", false);
                }
                else
                {
                    if ((ddlOperation.SelectedValue == "+") && (result < 0M))
                    {
                        decimal checkPrice = -result;
                        if (ProductHelper.CheckPrice(productIds, ddlSalePrice.SelectedValue, checkPrice))
                        {
                            ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
                            return;
                        }
                    }
                    if (ProductHelper.UpdateSkuMemberPrices(productIds, ddlMemberPrice2.SelectedValue.Value, ddlSalePrice.SelectedValue, ddlOperation.SelectedValue, result))
                    {
                        ShowMsg("修改商品的价格成功", true);
                    }
                }
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {
            DataSet skuPrices = GetSkuPrices();
            if (((skuPrices == null) || (skuPrices.Tables["skuPriceTable"] == null)) || (skuPrices.Tables["skuPriceTable"].Rows.Count == 0))
            {
                ShowMsg("没有任何要修改的项", false);
            }
            else if (ProductHelper.UpdateSkuMemberPrices(skuPrices))
            {
                ShowMsg("修改商品的价格成功", true);
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                ShowMsg("没有要修改的商品", false);
            }
            else if (!ddlMemberPrice.SelectedValue.HasValue)
            {
                ShowMsg("请选择要修改的价格", false);
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(txtTargetPrice.Text.Trim(), out result))
                {
                    ShowMsg("请输入正确的价格", false);
                }
                else if (result <= 0M)
                {
                    ShowMsg("直接调价必须输入正数", false);
                }
                else if (ProductHelper.UpdateSkuMemberPrices(productIds, ddlMemberPrice.SelectedValue.Value, result))
                {
                    ShowMsg("修改商品的价格成功", true);
                }
            }
        }

        private DataSet GetSkuPrices()
        {
            DataSet set = null;
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(txtPrices.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                set = new DataSet();
                DataTable table = new DataTable("skuPriceTable");
                table.Columns.Add("skuId");
                table.Columns.Add("costPrice");
                table.Columns.Add("salePrice");
                DataTable table2 = new DataTable("skuMemberPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("memberPrice");
                foreach (XmlNode node in list)
                {
                    DataRow row = table.NewRow();
                    row["skuId"] = node.Attributes["skuId"].Value;
                    row["costPrice"] = string.IsNullOrEmpty(node.Attributes["costPrice"].Value) ? 0M : decimal.Parse(node.Attributes["costPrice"].Value);
                    row["salePrice"] = decimal.Parse(node.Attributes["salePrice"].Value);
                    table.Rows.Add(row);
                    foreach (XmlNode node2 in node.SelectSingleNode("skuMemberPrices").ChildNodes)
                    {
                        DataRow row2 = table2.NewRow();
                        row2["skuId"] = node.Attributes["skuId"].Value;
                        row2["gradeId"] = int.Parse(node2.Attributes["gradeId"].Value);
                        row2["memberPrice"] = decimal.Parse(node2.Attributes["memberPrice"].Value);
                        table2.Rows.Add(row2);
                    }
                }
                set.Tables.Add(table);
                set.Tables.Add(table2);
            }
            catch
            {
            }
            return set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            productIds = Page.Request.QueryString["productIds"];
            btnSavePrice.Click += new EventHandler(btnSavePrice_Click);
            btnTargetOK.Click += new EventHandler(btnTargetOK_Click);
            btnOperationOK.Click += new EventHandler(btnOperationOK_Click);
            if (!Page.IsPostBack)
            {
                ddlMemberPrice.DataBind();
                ddlMemberPrice.SelectedValue = -3;
                ddlMemberPrice2.DataBind();
                ddlMemberPrice2.SelectedValue = -3;
                ddlSalePrice.DataBind();
                ddlSalePrice.SelectedValue = "CostPrice";
                ddlOperation.DataBind();
                ddlOperation.SelectedValue = "+";
            }
        }
    }
}

