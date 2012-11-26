﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditShippingType" CodeFile="EditShippingType.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/Hishopv5.css" Media="screen" />
    <Hi:Script runat="server" Src="/admin/js/Hishopv5.js" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
<div class="columnleft clearfix"><ul><li><a href="ShippingTypes.aspx"><span>配送方式列表</span></a></li></ul>
</div>
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/05.gif" width="32" height="32" /></em>
		    <h1>编辑配送方式</h1>
		    <span>每一个配送方式都是针对一个物流公司并且结合物流公司的到达地区和收费标准设置的，您可以为不同的到达地区设置不同的收费标准</span></div>
		  <div class="formitem validator4">
		    <ul>
		      <li> <span class="formitemtitle Pw_110">配送方式名称：<em >*</em></span>
		        <asp:TextBox ID="txtModeName" runat="server" class="forminput"></asp:TextBox>
		        <p id="ctl00_contentHolder_txtModeNameTip">配送方式名称不能为空，长度限制在60字符以内</p>
	          </li>
	            <li> <span class="formitemtitle Pw_110">选择物流公司：<em >*</em></span>
		          <span style="float:left;"><Hi:ExpressCheckBoxList ID="expressCheckBoxList" RepeatDirection="Horizontal" RepeatColumns="6" runat="server" /></span>
		        <p id="P1">请选择此配送方式对应的物流公司</p>
	          </li>
	          <li>
	            <span class="formitemtitle Pw_110">选择配送模板：<em>*</em></span>
	            <hi:ShippingTemplatesDropDownList runat="server" ID="shippingTemplatesDropDownList" NullToDisplay="请选择配送模板" />
	            <a href="AddShippingTemplate.aspx" target="_blank">添加配送模板</a>
		        <p id="P2">请选择此配送方式对应的配送模板</p>
	          </li>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
          <span><Kindeditor:KindeditorControl ID="fck" runat="server" Width="550"  Height="200px" /></span>
          </li>
	    </ul>
    <ul class="btn Pa_110 clear">
    <asp:Button ID="btnUpDate" runat="server"  CssClass="submit_DAqueding inbnt"  OnClientClick="return ValidateAll();"  Text="确 定"/>
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
        initValid(new InputValidator('ctl00_contentHolder_txtModeName', 1, 60, false, null, '配送方式名称不能为空，长度限制在60字符以内'))
    }
    $(document).ready(function() { InitValidators(); IsFlagDate(); });
    function ValidateAll() {
        if (PageIsValid()) {
            if ($("#ctl00_contentHolder_shippingTemplatesDropDownList").val() != ""
        && $("#ctl00_contentHolder_shippingTemplatesDropDownList").val() != undefined) {
                return true;
            }
            else {
                alert("必须要选择一个配送模板。");
                return false;
            }
        }
        return false;
    }
    
    </script>
 </asp:Content>
