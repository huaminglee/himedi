<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:DataList ID="dataListOrderItems" runat="server"  Width="100%">
         <HeaderTemplate>
            <table cellspacing="0" border="0" >
                <tr class="GridViewHeaderStyle" style="color:#858585; text-align:left;">
                    <th class="content_table_title" width="60px">��ƷͼƬ</th>
                     <th class="content_table_title" width="130px">����</th>
                    <th class="content_table_title" width="350">��Ʒ����</th>
                    <th class="content_table_title" width="60">��������</th>
                     <th class="content_table_title" width="70">��Ʒ����</th>
                     <th class="content_table_title" width="60">��������</th>
                     <th class="content_table_title" width="70">С��</th>
                </tr>
                 </HeaderTemplate>
         <ItemTemplate>
            <tr>
                <td >
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>                            
                </td>
                <td>
                    <asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU")+"&nbsp;" %>'></asp:Literal></td>
                <td>
                <Hi:ProductDetailsLink ID="productNavigationDetails"  ProductName='<%# Eval("ItemDescription") %>'  ProductId='<%# Eval("ProductId") %>' runat="server"/>
                <br />
                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                <br />
                <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PurchaseGiftId"))%>' Text='<%# Eval("PurchaseGiftName")%>' Target="_blank"></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hlinkWholesaleDiscount" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("WholesaleDiscountId"))%>' Text='<%# Eval("WholesaleDiscountName")%>' Target="_blank"></asp:HyperLink>
                </td>
                <td>
                    <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                <td>
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server"  Money='<%# Eval("ItemListPrice") %>' />                 
                </td>
                <td>
                    <asp:Literal ID="lblShipQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
                <td>
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server"  Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' />         
                </td>
            </tr>
     </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>