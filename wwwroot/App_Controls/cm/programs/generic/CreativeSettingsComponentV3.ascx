<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CreativeSettingsComponentV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.CreativeSettingsComponentV3" %>
<%@ Register TagPrefix="PageControls" TagName="StyleSettingsEditor" Src="~/app_controls/cm/programs/generic/StyleSettingsEditor.ascx" %>
<%@ Register TagPrefix="PageControls" TagName="VariableSetsEditor" Src="~/app_controls/cm/programs/generic/VariableSetsEditorV3.ascx" %>
<table style="width:900px">
	<tr>
		<td style="text-align:right">
			<telerik:RadButton ID="rbtnComponentStatus" runat="server" Font-Bold="true" AutoPostBack="true" icon-PrimaryIconWidth="20" icon-PrimaryIconHeight="25" ButtonType="StandardButton" Skin="Default">				
			</telerik:RadButton>
		</td>	
		<% If Me.HelpLink IsNot Nothing Then%>
		<td style="text-align: right; font-weight: bold;"><a href="<%=me.HelpLink%>" target="_new">(<%=DealerDirect.Resources.getString("LITERAL_Help")%>)</a></td>
		<% End If%>
	</tr>
</table>
<br />
<% If Me.InlineHelp IsNot Nothing Then%>
<table width="85%" align="center" cellspacing="0" cellpadding="4" style="border-width: 1px; border-style: solid; border-color: #A8A8A8;">
	<tr style="border-width: 0px">
		<td style="background-color: #E0E0E0;">
			<%=Me.InlineHelp%>
		</td>
	</tr>
</table>
<br />
<% End If%>
<asp:Panel ID="pnlMain" runat="server">
	<asp:Panel ID="pnlStyle" runat="server">
		<PageControls:StyleSettingsEditor runat="server" ID="sseMain" />
		<br />
	</asp:Panel>
	<PageControls:VariableSetsEditor runat="server" ID="vseMain" />
</asp:Panel>
<asp:PlaceHolder ID="phPreviewGenerator" runat="server" />
