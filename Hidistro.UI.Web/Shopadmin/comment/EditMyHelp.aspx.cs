﻿namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public partial class EditMyHelp : DistributorPage
    {

         int helpId;


        private void btnEditHelp_Click(object sender, EventArgs e)
        {
            HelpInfo target = new HelpInfo();
            if (!this.dropHelpCategory.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择帮助分类", false);
            }
            else
            {
                target.HelpId = this.helpId;
                target.AddedDate = DateTime.Now;
                target.CategoryId = this.dropHelpCategory.SelectedValue.Value;
                target.Title = this.txtHelpTitle.Text;
                target.MetaDescription = this.txtMetaDescription.Text;
                target.MetaKeywords = this.txtMetaKeywords.Text;
                target.Description = this.txtShortDesc.Text;
                target.Content = this.fcContent.Text;
                target.IsShowFooter = this.radioShowFooter.SelectedValue;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpInfo>(target, new string[] { "ValHelpInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMsg(msg, false);
                }
                else if (!(!this.radioShowFooter.SelectedValue || SubsiteCommentsHelper.GetHelpCategory(target.CategoryId).IsShowFooter))
                {
                    this.ShowMsg("当选中的帮助分类设置不在底部帮助显示时，此分类下的帮助主题就不能设置在底部帮助显示", false);
                }
                else if (SubsiteCommentsHelper.UpdateHelp(target))
                {
                    this.ShowMsg("已经成功修改当前帮助", true);
                }
                else
                {
                    this.ShowMsg("编辑底部帮助错误", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["helpId"], out this.helpId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditHelp.Click += new EventHandler(this.btnEditHelp_Click);
                if (!this.Page.IsPostBack)
                {
                    this.dropHelpCategory.DataBind();
                    this.SetControlValue();
                }
            }
        }

        private void SetControlValue()
        {
            HelpInfo help = SubsiteCommentsHelper.GetHelp(this.helpId);
            if (help == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(help, false);
                this.txtHelpTitle.Text = help.Title;
                this.txtMetaDescription.Text = help.MetaDescription;
                this.txtMetaKeywords.Text = help.MetaKeywords;
                this.txtShortDesc.Text = help.Description;
                this.fcContent.Text = help.Content;
                this.lblEditHelp.Text = help.HelpId.ToString(CultureInfo.InvariantCulture);
                this.dropHelpCategory.SelectedValue = new int?(help.CategoryId);
                this.radioShowFooter.SelectedValue = help.IsShowFooter;
            }
        }
    }
}

