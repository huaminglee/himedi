﻿using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SMSSettings)]
    public partial class SMSSettings : AdminPage
    {
        private void btnSaveSMSSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = LoadConfig(out str);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                masterSettings.SMSSender = string.Empty;
                masterSettings.SMSSettings = string.Empty;
            }
            else
            {
                if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    ShowMsg(msg, false);
                    return;
                }
                masterSettings.SMSSender = str;
                masterSettings.SMSSettings = Cryptographer.Encrypt(data.SettingsXml);
            }
            SettingsManager.Save(masterSettings);
            Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
        }

        private void btnTestSend_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = LoadConfig(out str);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                ShowMsg("请先选择发送方式并填写配置信息", false);
            }
            else if (!data.IsValid)
            {
                string msg = "";
                foreach (string str3 in data.ErrorMsgs)
                {
                    msg = msg + Formatter.FormatErrorMessage(str3);
                }
                ShowMsg(msg, false);
            }
            else if ((string.IsNullOrEmpty(txtTestCellPhone.Text) || string.IsNullOrEmpty(txtTestSubject.Text)) || ((txtTestCellPhone.Text.Trim().Length == 0) || (txtTestSubject.Text.Trim().Length == 0)))
            {
                ShowMsg("接收手机号和发送内容不能为空", false);
            }
            else if (!Regex.IsMatch(txtTestCellPhone.Text.Trim(), @"^(13|15)\d{9}$"))
            {
                ShowMsg("请填写正确的手机号码", false);
            }
            else
            {
                string str4;
                bool success = SMSSender.CreateInstance(str, data.SettingsXml).Send(txtTestCellPhone.Text.Trim(), txtTestSubject.Text.Trim(), out str4);
                ShowMsg(str4, success);
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlSms"];
            txtSelectedName.Value = selectedName;
            txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = SMSSender.CreateInstance(selectedName);
            if (plugin == null)
            {
                return null;
            }
            ConfigData configData = plugin.GetConfigData(base.Request.Form);
            if (configData != null)
            {
                txtConfigData.Value = configData.SettingsXml;
            }
            return configData;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSaveSMSSettings.Click += new EventHandler(btnSaveSMSSettings_Click);
            btnTestSend.Click += new EventHandler(btnTestSend_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.SMSEnabled)
                {
                    txtSelectedName.Value = masterSettings.SMSSender.ToLower();
                    ConfigData data = new ConfigData(Cryptographer.Decrypt(masterSettings.SMSSettings));
                    txtConfigData.Value = data.SettingsXml;
                }
            }
        }
    }
}

