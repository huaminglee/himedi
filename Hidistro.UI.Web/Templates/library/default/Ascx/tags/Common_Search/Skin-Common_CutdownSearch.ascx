<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>		    
<%-- <span>���� <asp:TextBox ID="txtSKU" CssClass="cut_down_input" runat="server" width="200"/>
                    </span>--%> <span> �ؼ���
					<asp:TextBox ID="txtKeywords" CssClass="cut_down_input"   runat="server" MaxLength="50" />
                    </span> <span>�۸�Χ
                  <asp:TextBox ID="txtStartPrice" CssClass="cut_down_input" runat="server" width="30" />
                      ��
             <asp:TextBox ID="txtEndPrice" CssClass="cut_down_input" width="30" runat="server" />
                    </span> <span class="search_exact_input">
                    <asp:CheckBoxList ID="ckbListproductSearchType" BorderWidth="0" RepeatLayout="Flow" runat="server"></asp:CheckBoxList></span>
		    <asp:Button ID="btnSearch" runat="server" Text="����" CssClass="cut_down_button" />
 