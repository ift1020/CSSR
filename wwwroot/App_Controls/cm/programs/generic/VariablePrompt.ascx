<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VariablePrompt.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.VariablePrompt" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<tr style="padding:3px; border-bottom:2px dotted #5397f0">
	<td style="text-align:right; vertical-align:top; white-space:nowrap; padding:3px;" >
		<asp:label ID="lblVariableName" runat="server"/>
	</td>
	<td style="text-align:left; vertical-align:top; padding:3px;">
		<asp:panel ID="pnlFixedText" runat="server" Visible="false">
			<asp:TextBox runat="server" ID="txtFixedText" Width="300px"></asp:TextBox><asp:label ID="lblFixedTextSuffix" runat="server"/>
		</asp:panel>
		<asp:panel ID="pnlImage" runat="server" Visible="false">
			<asp:DropDownList runat="server" ID="ddlImage" />
		</asp:panel>
		<asp:panel ID="pnlTheme" runat="server" Visible="false">
			<asp:DropDownList runat="server" ID="ddlTheme" AutoPostBack="true"/>
		</asp:panel>
		<asp:panel ID="pnlMultiPick" runat="server" Visible="false">
			<telerik:RadComboBox runat="server" CheckBoxes="true" Width="300" id="rcpMultiPick" AutoPostBack="true" Skin="Default">
			</telerik:RadComboBox>
			<asp:Label ID="lblCheckedList" runat="server" Font-Size="Small" />
		</asp:panel>
        <%  If Me.Variable.ShowMeURL IsNot Nothing AndAlso Me.Variable.ShowMeURL.Length > 0 Then
             %>
        <a href="blaug" class="showMeLink">(<%=DealerDirect.Resources.getString("LITERAL_Show_me_this_region")%>)</a>
        <%  End if %>
		<b><asp:customvalidator id="cvMain" runat="Server"/></b>
	</td>
</tr>

