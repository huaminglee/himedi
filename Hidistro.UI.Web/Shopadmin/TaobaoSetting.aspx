<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="TaobaoSetting.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.TaobaoSetting" Title="�ޱ���ҳ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
        initValid(new InputValidator('ctl00_contentHolder_txtTopkey', 8, 8, true, null, '�Ա�Appkey����Ϊ�գ�Ϊ8λ����ID'))
        initValid(new InputValidator('ctl00_contentHolder_txtTopSecret', 32, 32, true, null, '�Ա�AppSecret��ʽ����ȷ,Ϊ32λ�ַ�'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title td_bottom"> <em><img src="images/01.gif" width="32" height="32" /></em>
        <h1 class="title_line" style="line-height:38px;">�Ա�ͬ������</h1>
         <div class="clear"></div>
        <span > ���Ƚ���<a href="http://my.open.taobao.com/app/app_list.htm" target="_blank">�Ա�����ƽ̨��Ӧ���б��</a>ѡ��һ��Ӧ�ò���ȡ��App Key��App Secret
        <br />�������û�д����κ�Ӧ�ã���<a href="http://my.open.taobao.com/common/applyIsv.htm" target="_blank">����Ӧ��</a>������Ӧ�ú󼴿ɻ��App Key��App Secret</span>
        <br /> <span style="color:Red;"> ע����ѡ��Ҫ�󶨵�Ӧ�ûص�ҳ��URLһ��Ҫд�ɣ�<asp:Literal runat="server" ID="litReturnUrl" /></span>
      </div>
     
      <div class="datafrom">
      <asp:Literal runat="server" ID="litshowmsg"></asp:Literal>
        <div class="formitem validator1" id="settaobao" runat="server">
          <ul>
            <li><span class="formitemtitle Pw_198">�Ա�App key��<em >*</em></span>
              <asp:TextBox ID="txtTopkey" CssClass="forminput" runat="server"  />
              <p id="txtTopkeyTip" runat="server">�Ա�Appkey����Ϊ�գ�Ϊ8λ����ID</p>
            </li>
            <li><span class="formitemtitle Pw_198">�Ա�App Secret��<em >*</em></span>
              <asp:TextBox ID="txtTopSecret" CssClass="forminput" Width="300px" runat="server"  />
              <p id="txtTopSecretTip" runat="server">�Ա�AppSecret����Ϊ��,Ϊ32λ�ַ�</p>
            </li>
            </ul>
           <ul class="btntf Pa_198 clearfix">
		    <asp:Button ID="btnOK" runat="server" Text="���Ա�" CssClass="submit_DAqueding inbnt" OnClick="btnOK_Click" OnClientClick="return PageIsValid();" />
			</ul>
        </div>
      </div>
           <div class="clear"></div>
</div>

</asp:Content>
