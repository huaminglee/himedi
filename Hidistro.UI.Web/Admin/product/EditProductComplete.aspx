<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditProductComplete.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditProductComplete" Title="�ޱ���ҳ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>�༭��Ʒ�ɹ�</h1>
            <span>��Ʒ�༭�ɹ����������Խ������²�����</span>
</div>
          <div class="formitem">
          <span class="msg">��Ʒ�༭�ɹ���</span>
         </div>
          <div class="Pg_15 Pg_45 fonts"><span class="float">�����</span>
            <asp:HyperLink ID="hlinkProductDetails" runat="server" Target="_blank" Text="�鿴" />��Ʒ
        </div>
		  <div class="Pg_15 Pg_45 fonts">��������ʱ��  <span class="Name"><a href="ProductOnSales.aspx">�����е���Ʒ</a> �� <a href="ProductInStock.aspx">�ֿ������Ʒ</a> </span>�����༭������Ʒ��</div>
		  <div class="Pg_15 Pg_45 fonts"><asp:Button runat="server" ID="btnSave" Text="�� ��" OnClientClick="javascript:window.close();" CssClass="submit_DAqueding inbnt" /></div>
      </div>
        
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
