<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="TaobaoOrders.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoOrders" Title="�ޱ���ҳ" %>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div runat="server" id="content1" class="dataarea mainwidth td_top_ccc">		<!--����-->
        		<div class="toptitle">
		  <em><img src="../images/03.gif" width="32" height="32" /></em>
		  
          <h1 class="title_height"><strong>�Ա��������ɲɹ���</strong></h1>
          <span class="title_height"><font color="red">1.��ѡ���ڴ˴����Ա��������ɲɹ������벻Ҫ�ٴ�ʹ�ú���ץץȡ�Ա��������ύ�ɹ�����������ͬ�Ա������ظ����ɲɹ�����<br />2.���Ա�������ͬʱ��������վ�������Ա�����Ʒ�ͷǷ���վ��Ʒ������Ա��������ɲɹ���ʱ���ɹ�����ֻ��������վ�������Ա�����Ʒ��</font></span> 
		</div>
		<div class="searcharea clearfix br_search">
		  <ul>
		    <li> <span>ѡ��ʱ��Σ�</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">��</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>����ǳƣ�</span><span>
		      <asp:TextBox ID="txtBuyerName" runat="server" cssclass="forminput" />
		    </span></li>
		    
		    <li>
		      <asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="��ѯ" />
	        </li>
	      </ul>
  </div>
		<!--����-->

      <div class="functionHandleArea clearfix m_none">
          <span>���ͷ�ʽ��</span>
            <hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" NullToDisplay="--��ѡ�����ͷ�ʽ--" />  
        <span style="color:#888; margin-left:10px;">(��Ϊѡ�е��Ա�����ѡ�����ͷ�ʽ���Ա����ɲɹ���ʱ���˷Ѽ��㡣)</span>
      </div>

         <div class="functionHandleArea clearfix m_none">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
           </div>
			</div>
			<!--����-->

  <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>  
		   <span class="purchaseSelect"><a href="javascript:void(0)" onclick="ConvertPurchaseOrder()">��ѡ�ж������ɲɹ���</a></span> 
		</li>
        </ul>
      </div>
      <div class="filterClass">
        </div>  
      </div>
		<!--�����б�����-->
	  <div class="datalist">
	  <div>
	  <asp:Repeater ID="rptTrades" runat="server">
        <ItemTemplate>   
            <table>
	        <tr class="td_bg">
		          <td>
		            <Hi:ConvertStatusLabel runat="server" />
		             ������ţ�<%#Eval("tid")%>
		          </td>
		          <td colspan="3">�ɽ�ʱ�䣺<%#Eval("created") %></td>
		     </tr>
		     <tr>
		        <td style="width:50%;">
		            <asp:Repeater ID="dlstOrders" runat="server" DataSource='<%#Eval("orders") %>'>
		             <ItemTemplate>   
		              <table>
		                  <tr>
		                      <td style="width:20%;">
		                            <a target="_blank" href='<%# "http://item.taobao.com/item.htm?id=" + Eval("NumIid") %>'><img src='<%#Eval("PicPath") %>'width="78" height="62"/></a>
		                       </td>
		                       <td style="width:50%;">
		                           <a target="_blank" href='<%# "http://item.taobao.com/item.htm?id=" + Eval("NumIid") %>'> <%#Eval("title")%> </a>
		                           <div><%#Eval("OuterSkuId")%></div>
		                           <div><%#Eval("SkuPropertiesName")%></div>
		                       </td>
		                       <td style="width:20%;">
		                            <%#Eval("price")%> 
		                      </td>
		                      <td style="width:10%;"><%#Eval("num")%> </td>
		                </tr>
		            </table>
		            </ItemTemplate>
		            </asp:Repeater>
		        </td>
		        <td style="width:20%;">
		            <%#Eval("BuyerNick")%><br />
		             <%#Eval("ReceiverName")%>
		        </td>
		        <td style="width:15%;">
		            <%#Eval("status")%><br />
		            <a target="_blank" href='<%# "http://trade.taobao.com/trade/detail/trade_item_detail.htm?bizOrderId=" + Eval("tid") %>'>����</a>
		         </td>
		        <td style="width:15%;">
		            <%#Eval("payment")%><br />
		            (��<%#Eval("ShippingType")%>��<%#Eval("PostFee")%>)
		        </td>
	        </tr>
	        </table>
	    </ItemTemplate>
      </asp:Repeater>
      <div class="blank5 clearfix"></div>
  </div>
     </div>
    <!--�����б�ײ���������-->
	  <div class="page">
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

</div>
<div runat="server" id="content12" visible="false" class="dataarea mainwidth td_top_ccc">
    <asp:Literal ID="txtMsg" runat="server" />
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" >       
    function ConvertPurchaseOrder(){
        var shippingModeId = $("#ctl00_contentHolder_shippingModeDropDownList").val();
        if(shippingModeId == ""){
            alert("��ѡ��һ�����ͷ�ʽ");
            return "";
        }
        var orderIds = GetOrderId();
        if(orderIds.length > 0)
            window.open("TaobaoOrderConvertPurchaseOrder.aspx?OrderIds=" + orderIds + "&shippingModeId=" + shippingModeId);
    }    
    
    function GetOrderId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("��ѡ��Ҫת���Ķ���");
            return "";
        }
        return v_str.substring(0, v_str.length - 1);        
    }
</script>
</asp:Content>
