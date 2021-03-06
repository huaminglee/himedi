﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ManageMyLeaveComments.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyLeaveComments" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

   	<!--选项卡-->
	<div class="dataarea mainwidth td_top_ccc">
    <div class="toptitle"> 
    <em><img src="../images/07.gif" width="32" height="32" /></em> 
    <h1 class="title_height">管理客户的留言</h1>
     </div>
    <!--搜索-->
    <div class="functionHandleArea m_none">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
        </ul>
      </div>
     <div class="bottomPageNumber clearfix">
      <div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
			</div>
    </div>
      <!--结束-->
      <div class="blank8 clearfix"></div>
        <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span>
           <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
            <span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span> 
            <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect1" IsShow="true" runat="server" Text="删除" /></span></li>
        </ul>
      </div>
      <div class="filterClass"> <span><b>回复状态：</b></span> <span class="formselect">
        <Hi:MessageStatusDropDownList ID="statusList" class="formselect input100" runat="server"  />
      </span></div>
    </div>
    <!--数据列表区域-->
    <div class="datalist">
      <UI:Grid ID="leaveList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="LeaveId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
            <Columns>                 
                <UI:CheckBoxColumn HeaderStyle-CssClass="table_title"/>
                 
                 <asp:TemplateField HeaderText="标题" HeaderStyle-CssClass="td_right td_left">
                  <itemstyle  CssClass="Name" />
                    <ItemTemplate >
                        <a href='<%#  Globals.ApplicationPath+string.Format("/Shopadmin/comment/ReplyedLeaveCommentsSuccsed.aspx?LeaveId={0}", Eval("LeaveId"))%>'  %>
                            <asp:Literal ID="litTitle" runat="server" Text='<%# Globals.HtmlDecode(Eval("Title").ToString())%>'></asp:Literal>
                        </a>
                    </ItemTemplate>
                 </asp:TemplateField>    
                
                <asp:TemplateField HeaderText="发件人" HeaderStyle-CssClass="td_right td_left">
                 <itemstyle  CssClass="Name" />
                    <ItemTemplate>
                        <a href='<%# Globals.ApplicationPath+(string.Format("/ShopAdmin/Underling/ManageUnderlings.aspx?Username={0}",Eval("UserName"))) %>'><asp:Literal ID="litUserName" runat="server" Text='<%#  Globals.HtmlDecode(Eval("UserName").ToString())%>' /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="最后回复" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                       <Hi:FormatedTimeLabel ID="litDateTime" ShopTime="true" runat="server" Time='<%#Eval("LastDate") %>' /><span class="colorG"><asp:Label ID="UserName" runat="server" Text="管理员" Visible ='<%# Eval("IsReply")%> '></asp:Label></span>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
             
                    <ItemTemplate>
                            <span class="submit_shanchu"> <Hi:ImageLinkButton runat="server" ID="lkbtnDelete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>                    
            </Columns>
        </UI:Grid>         
      <div class="blank12 clearfix"></div>
    </div>
     <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect" IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
	  </div>
    <!--数据列表底部功能区域-->
         <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
			</div>
			</div>
		</div>
		</div>
</div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
