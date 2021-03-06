﻿<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="UnderlingBalanceDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnderlingBalanceDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1 class="title_height"><strong class="colorE">“<asp:Literal ID="litUser" runat="server"></asp:Literal>”</strong> 会员账户明细</h1>
  </div>
		
  <div class="VIPbg fonts">
  <ul>
     <li>预付款总额：<strong class="colorG"><asp:Literal ID="litBalance" runat="server" /></strong></li>
     <li>可用余额：<strong class="colorB"><asp:Literal ID="litUserBalance" runat="server" /></strong></li>
     <li>冻结金额：<strong class="colorQ"><asp:Literal ID="litDrawBalance" runat="server" /></strong></li>
     <li><asp:LinkButton runat="server" ID="lbtnDrawRequest" Text="查看提现记录" /></li>
  </ul>
  </div>
  <div class="searcharea clearfix br_search">
	  <ul>
	    <li>
                <span>选择时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" CssClass="forminput" ID="calendarStart" runat="server" /></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar CalendarType="EndDate" CssClass="forminput" ID="calendarEnd" runat="server" /></span>
        </li>
        <li>    
            <span>类型：</span>
            <Hi:TradeTypeDropDownList ID="dropTradeType" runat="server" />
        </li>
		<li>
		    <span><asp:Button ID="btnQueryBalanceDetails" runat="server" class="searchbutton" Text="查询" /></span>
		</li>
			</ul>
	</div>
		
<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber">
		   <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
          </div>
		  <!--结束-->
</div>
		<!--数据列表区域-->
		<div class="datalist">
		    <UI:Grid ID="grdBalanceDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                            <Columns>
                                <asp:BoundField HeaderText="流水号" DataField="JournalNumber" HeaderStyle-CssClass="td_right td_left" />	
                           		<asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                       <a href='<%# "EditUnderling.aspx?userId="+ Eval("UserId")%>'><asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>
                                    </ItemTemplate>
                               </asp:TemplateField>	  		                		    			      
			                    <asp:TemplateField HeaderText="时间" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                        <Hi:FormatedTimeLabel ID="lblTradeDate" Time='<%#Eval("TradeDate")%>' runat="server" ></Hi:FormatedTimeLabel>
                                    </ItemTemplate>
                                </asp:TemplateField>
			                    <asp:TemplateField HeaderText = "类型" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                            <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server"  TradeType="TradeType" />
			                        </ItemTemplate>
			                    </asp:TemplateField>
			                    <Hi:MoneyColumnForAdmin HeaderText="收入" DataField="Income" HeaderStyle-CssClass="td_right td_left"/>
			                    <Hi:MoneyColumnForAdmin HeaderText="支出" DataField="Expenses" HeaderStyle-CssClass="td_right td_left" />
			                    <Hi:MoneyColumnForAdmin HeaderText="账户余额" DataField="Balance" HeaderStyle-CssClass="td_right td_left" />	
			                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_left td_right_fff" >
                                    <itemtemplate>
                                        <img src="../images/xi.gif" onmouseover="showRemark(this,'<%# Globals.HtmlEncode(Eval("Remark").ToString())%>')" onmouseout="CloseRemark()" />
                                    </itemtemplate>
                                </asp:TemplateField> 		      	        
	                        </Columns> 
	                      </UI:Grid>		
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
		</div>
	</div>
     
     <div class="Visa" id="remark" style="display:none; z-index:1000;background-color:White;">
<h1>备注信息</h1>

<table width="100%" border="0" cellspacing="0" class="colorQ">
  <tr>
    <td colspan="2"><span id="spanRemark" runat="server" /></td>
  </tr>
  </table>
</div>
  
<script language="javascript" type="text/javascript">
function CloseRemark(){$("#remark").fadeOut(800);}
function showRemark(objThis,remark) 
    {
        if (remark == "")
         {
            $("#ctl00_contentHolder_spanRemark").html("无备注信息");
        }
        else 
        {
            $("#ctl00_contentHolder_spanRemark").html(remark);
        }
        var BandMessage = document.getElementById("remark")
        
        var WinElementPos = getWinElementPos(objThis) //公用方法来源 globals.js
		 var MouseX =WinElementPos.x; 
		 var MouseY =WinElementPos.y;
        var pltsoffsetX = 0; // 弹出窗口位于鼠标左侧或者右侧的距离；
        var pltsoffsetY =-120; // 弹出窗口位于鼠标下方的距离；
        BandMessage.style.position="absolute";
        BandMessage.style.left = MouseX + pltsoffsetX+"px";
        BandMessage.style.top = MouseY + pltsoffsetY+"px";
        BandMessage.style.display = "block";  
    } 
</script>
</asp:Content>
