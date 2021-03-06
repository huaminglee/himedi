﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditOrderLookup.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditOrderLookup" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="OrderLookupLists.aspx"><span>订单可选项列表</span></a></li>
                  </ul>
</div>
      <div class="columnright">
          <div class="title">
            <em><img src="../images/05.gif" width="32" height="32" /></em>
            <h1>编辑订单可选项</h1>
            <span>订单可选项是顾客在下订单时可以额外选择的一些项目，您可以自定义这些项目供顾客选择，比如：是否需要发票等。</span>
</div>
          <div class="formtab Pg_45">
                   <ul>
                      <li class="visited">基本信息</li>                                      
                      <li><a href='<%="EditOrderLookupItem.aspx?LookupListId=" +Page.Request.QueryString["LookupListId"] %>'>可选项内容</a></li>
            </ul>
          </div>
          <div class="formitem validator3">
            <ul>
              <li> <span class="formitemtitle Pw_128">订单可选项名称：<em>*</em></span>
                <asp:TextBox ID="txtListName" runat="server" CssClass="forminput"></asp:TextBox>
                <p id="ctl00_contentHolder_txtListNameTip">长度限制在1-60个字符之间</p>
              </li>
              <li> <span class="formitemtitle Pw_128">显示方式：</span>
                  <abbr class="formselect">
                    <Hi:SelectModeDropDownList ID="dropSelectMode" runat="server" SelectedValue="1" />
                </abbr>
              </li>
              <li><span class="formitemtitle Pw_128">备注：</span>
                   <asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="450" Height="120" runat="server" MaxLength="50" CssClass="forminput"></asp:TextBox>
                </li>
            </ul>
            <ul class="btn Pa_128 clear">
               <asp:Button ID="btnSave" Text="保 存" CssClass="submit_DAqueding" runat="server" OnClientClick="return PageIsValid();"/>
            </ul>
          </div>
  </div>
        
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

 <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtListName', 1, 60, false, null, '限制名称不能为空，长度限制在60个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>
