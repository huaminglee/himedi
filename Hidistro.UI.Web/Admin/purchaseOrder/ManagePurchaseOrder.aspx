<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManagePurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManagePurchaseOrder"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--ѡ�-->
	<div class="optiongroup mainwidth">
		<ul>
			<li id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server"><span>���вɹ���</span></asp:HyperLink></li>
			<li id="anchors1"><asp:HyperLink ID="hlinkNotPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
			<li id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
            <li id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server"><span>�ѷ���</span></asp:HyperLink></li>     
            <li  id="anchors5"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>�ɹ��ɹ���</span></asp:HyperLink></li>       
            <li id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server"><span>�ѹر�</span></asp:HyperLink></li>
            <li id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server"><span>��ʷ�ɹ���</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--ѡ�-->
<input type="hidden" runat="server" id="lblPurchaseOrderId" />
<div class="dataarea mainwidth">
		<!--����-->
		<div class="searcharea clearfix br_search">
		  <ul  class="a_none_left">
		    <li> <span>ѡ��ʱ��Σ�</span><span>
		     <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">��</span> <span>
		       <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>���������ƣ�</span><span>
		      <asp:TextBox ID="txtDistributorName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>��Ʒ���ƣ�</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>������ţ�</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" />
		      </span></li>		      
		    <li><span>�ɹ�����ţ�</span><span>
		      <asp:TextBox ID="txtPurchaseOrderId" runat="server" cssclass="forminput"></asp:TextBox> <asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>
		      <li><span>�ջ��ˣ�</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput"></asp:TextBox>
		      </span></li> 
		      <li><span>���ͷ�ʽ��</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li style="width:150px;"><span>��ӡ״̬��</span><span>
		        <abbr class="formselect">
		        <asp:DropDownList runat="server" ID="ddlIsPrinted" /></abbr>
		      </span></li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="��ѯ" />
	        </li>
	      </ul>
  </div>
		<!--����-->
      <div class="functionHandleArea clearfix m_none">
			<!--��ҳ����-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
           </div>
			</div>
			<!--����-->
			 <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="ɾ��" IsShow="true"/></span>
		  <Hi:ImageLinkButton ID="btnBatchPrintData" runat="server" Text="������ӡ��ݵ�" DeleteMsg="����ǰѡ�н��������ݵ���ӡ�����б��Ƿ������" IsShow="true"/>
          <Hi:ImageLinkButton ID="btnBatchSendGoods" runat="server" Text="��������" DeleteMsg="����ǰѡ�н����ɸѡ���Ѹ���δ�����Ķ������������������Ƿ������" IsShow="true"/>
		  </li>
        </ul>
      </div>
		</div>
		 <input type="hidden" id="hidPurchaseOrderId" runat="server" />
        		<!--�����б�����-->
	  <div class="datalist">	  
	   <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="24%" class="td_right td_left">������</td>
		      <td width="20%" class="td_right td_left">�ջ���</td>
		      <td width="12%" class="td_right td_left">����ʵ�տ�(Ԫ)</td>
		      <td width="12%" class="td_right td_left">�ɹ���ʵ�տ�(Ԫ)</td>
		      <td width="18%" class="td_right td_left">�ɹ�״̬</td>
		      <td width="12%" class="td_left td_right_fff">����</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>	   
	   <tr class="td_bg">
		      <td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("PurchaseOrderId") %>'>�ɹ�����ţ�<%#Eval("PurchaseOrderId") %></td>
		      <td>�ɽ�ʱ�䣺<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td><%# (bool)Eval("IsPrinted")?"�Ѵ�ӡ":"δ��ӡ" %></td>
		      <td>&nbsp;</td>
		      <td>&nbsp;</td>
		      <td align="right"><a href="javascript:RemarkPurchaseOrder('<%#Eval("PurchaseOrderId") %>','<%#Eval("OrderId") %>','<%#Eval("PurchaseDate") %>','<%#Eval("PurchaseTotal") %>','<%#Eval("ManagerMark") %>','<%#  Eval("ManagerRemark") %>')"><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a></td>
	        </tr>   
	    
		    <tr>
		      <td><%#Eval("Distributorname") %> <Hi:WangWangConversations runat="server" ID="WangWangConversations"  WangWangAccounts='<%#Eval("DistributorWangwang") %>'/>  </td>
		      <td><%#Eval("ShipTo") %>&nbsp;</td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td><Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
		        <span class="Name"><div runat="server" visible="false" id="lkbtnEditPurchaseOrder"><a href="javascript:void(0);" onclick="OpenWindow('<%# Eval("PurchaseOrderId")%>','<%# Eval("PurchaseTotal")%>')">�޸Ĳɹ����۸�</a></div></span>
		        <a href="javascript:ClosePurchaseOrder('<%#Eval("PurchaseOrderId") %>');"><asp:Literal runat="server" ID="litClosePurchaseOrder" Visible="false" Text="�رղɹ���" /></a> 
		      </td>
		      <td>
		      <table border="0" cellpadding="0" cellspacing="0" style="border:none;">
		      <tr><td style="border:none;"><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  /> </td>
		      <td rowspan="2" style="border:none;"><Hi:OrderRefundStatusMark runat="server" ID="OrderRefundStatusMark" NavigateUrl='<%#"RefundPurchaseOrderDetails.aspx?PurchaseOrderId="+Eval("PurchaseOrderId" )%>' Status='<%# Eval("RefundStatus") %>' /></td></tr>
		      <tr><td style="border:none;"> <span class="Name"><Hi:PurchaseOrderDetailsHyperLink ID="lkbtnPurchaseOrderDetails" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' PurchaseOrderId ='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="�鿴����" runat="server"/></span>
		        <Hi:PurchaseOrderItemUpdateHyperLink Target="_blank" runat="server" PurchaseOrderId='<%# Eval("PurchaseOrderId") %>' PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' DistorUserId='<%# Eval("DistributorId") %>' Text="�޸Ĳɹ���Ʒ" />
		      </td></tr>
		      </table>	      
		      
	             </td>
		      <td>
		        <a href='<%# "../sales/PurchasePrintData.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>' target="_blank">��ӡ��ݵ�</a>
		        <a href='<%#" PrintPurchaseOrder.aspx?purchaseOrderId="+Eval("purchaseOrderId") %>' target="_blank">��</a>
		        <a href='<%#" PrintPurchasePackingOrder.aspx?purchaseOrderId="+Eval("purchaseOrderId") %>' target="_blank">��</a>
		        <span class="submit_faihuo"><asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# "SendPurchaseOrderGoods.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>' Target="_blank" Text="����" Visible="false"  ForeColor="Red"></asp:HyperLink></span>
		           <span class="Name"><Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="���������տ�" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false" ForeColor="Red"></Hi:ImageLinkButton>
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="��ɲɹ���" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  DeleteMsg="ȷ��Ҫ��ɸòɹ�����" Visible="false" ForeColor="Red" />
		      </td>
	        </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>
	   </asp:DataList>                
      <div class="instantstat clearfix" id="divSendOrders">
				ע���ɹ���״̬�����С��ˡ��ִ���òɹ����˹���
			</div>
      <div class="blank12 clearfix"></div>
    <!--�����б�ײ���������-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
               </div>

			</div>
		</div>
      </div>
  </div>
</div>

	<div class="databottom"></div>
	
<div class="bottomarea testArea">
  <!--����logo����-->
</div>


 
 <!--�޸ļ۸�-->
<div class="Pop_up" id="EditPurchaseOrder" style="display:none;">
  <h1>�޸ļ۸� </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
    <ul>
              <li> <span class="formitemtitle Pw_128">�ɹ���ԭʵ�տ</span>
                   <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount" Text="22"  runat="server"/></strong> Ԫ 
              </li>
              <li> <span class="formitemtitle Pw_128">�Ǽۻ��ۿۣ�<em>*</em></span>
                  <asp:TextBox ID="txtPurchaseOrderDiscount" runat="server" cssclass="forminput" onblur="ChargeAmount()" /> ���������ۿۣ����������Ǽ�
              </li>
              <li> <span class="formitemtitle Pw_128">������ʵ����</span>
                    <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
                    <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
                    <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount3" Text="22"  runat="server" /></strong>Ԫ
              </li>
              <li> <span class="formitemtitle Pw_128">������ʵ����</span>
                  �ɹ���ԭʵ�տ�+�Ǽۻ��ۿ�
              </li>
        </ul>
        <ul class="up Pa_128">
          <asp:Button ID="btnEditOrder"  runat="server"  Text="ȷ��" CssClass="submit_DAqueding"   />  
       </ul>
      </div>
</div>          
      
<!--�༭��ע��Ϣ-->
   <div class="Pop_up" id="RemarkPurchaseOrder"  style="display:none;">
  <h1>�༭��ע��Ϣ </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">������ţ�</span><span id="spanOrderId" runat="server" /></li>
              <li><span class="formitemtitle Pw_128">�ɹ�����ţ�</span><span id="spanpurcharseOrderId" runat="server" /></li>
       <li><span class="formitemtitle Pw_128">�ɽ�ʱ�䣺</span><span runat="server" ID="lblpurchaseDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">�ɹ���ʵ�տ�(Ԫ)��</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">��־��</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">����¼��</span>
          <span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></span>
              </li>
        </ul>
         <ul class="up Pa_128 clear">
         <asp:Button runat="server" ID="btnRemark" Text="ȷ��" CssClass="submit_DAqueding"/>
       </ul>
  </div> 
</div>

<div class="Pop_up" id="closePurchaseOrder" style="display:none;">
  <h1>�رղɹ��� </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>�رս���?����ȷ���Ѿ�֪ͨ������,���Ѵ��һ�����,��������رս���,�����ܵ��½��׾���</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">�رոòɹ��������ɣ�</span> <abbr class="formselect">
        <Hi:ClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="ȷ ��"   />
    </ul>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript">
     function ConfirmPayOrder() {
         return confirm("����������Ѿ�ͨ������;��֧���˲ɹ������������ʹ�ô˲����޸Ĳɹ���״̬\n\n�˲����ɹ�����Ժ󣬲ɹ����ĵ�ǰ״̬����Ϊ�Ѹ���״̬��ȷ�Ϸ������Ѹ��");
     }
     
     function showOrderState() {
         var status;
         if (navigator.appName.indexOf("Explorer") > -1) {

             status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

         } else {

             status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

         }
         if (status != "0") {
             document.getElementById("anchors0").className = 'optionstar';
         }
         if (status != "99") {
             document.getElementById("anchors99").className = 'optionend';
         }
         document.getElementById("anchors" + status).className = 'menucurrent';
         if ($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val("")) {
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");
         }
         initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '�ۿ�ֻ������ֵ���Ҳ��ܳ���2λС��'))
         appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '�ۿ�ֻ������ֵ�����ܳ���10000000���Ҳ��ܳ���2λС��'));
     }

         $(document).ready(function() { showOrderState(); });


         function OpenWindow(PurchaseOrderId, PurchaseTotal) {
             $("#ctl00_contentHolder_lblPurchaseOrderId").val(PurchaseOrderId);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount1").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(PurchaseTotal);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");
             
             DivWindowOpen(520,300,'EditPurchaseOrder');
         }

         function RemarkPurchaseOrder(purchaseOrderId, orderId, purchaseDate, purchaseTotal, managerMark, managerRemark) {
             $("#ctl00_contentHolder_spanOrderId").html(orderId);
             $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
             $("#ctl00_contentHolder_spanpurcharseOrderId").html(purchaseOrderId);
             $("#ctl00_contentHolder_lblpurchaseDateForRemark").html(purchaseDate);
             $("#ctl00_contentHolder_lblpurchaseTotalForRemark").html(purchaseTotal);
             $("#ctl00_contentHolder_txtRemark").val(managerRemark);

             for (var i = 0; i < 6; i++) {
                 if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark)
                     document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = true;
                 else
                     document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).checked = false;
             }
          DivWindowOpen(520,400,'RemarkPurchaseOrder');
         }

         function ChargeAmount() {
                     var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
                     if (($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val() != "") && reg.test($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val())) {
                         $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                         var amount1 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount").html());
                         var amount2 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount2").html());

                         var amount3 = amount1 + amount2;
                         $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(amount3);
                     }
                 }

        function ClosePurchaseOrder(purchaseOrderId)
         {
              $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
              DivWindowOpen(560,250,'closePurchaseOrder');
         }
         
         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "��ѡ���˻ص�����") {
                 alert("��ѡ���˻ص�����");
                 return false;
             }

             return true;
         }   
     </script>
</asp:Content>
