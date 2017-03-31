<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OfferSettingsComponentV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.OfferSettingsComponentV3" %>

<style type="text/css">
	table.radioWithProperWrap input {
		float: left;
	}

	table.radioWithProperWrap label {
		margin-left: 25px;
		display: block;
	}
</style>

<table style="width: 900px;">
	<tr>
		<td style="text-align: right;">
			<telerik:RadButton ID="rbtnComponentStatus" runat="server" Font-Bold="true" AutoPostBack="true" Icon-PrimaryIconWidth="20" Icon-PrimaryIconHeight="25" ButtonType="StandardButton" Skin="Default">
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
	<tr style="border-width: 0px;">
		<td style="background-color: #E0E0E0;">
			<%=Me.InlineHelp%>
		</td>
	</tr>
</table>
<br />
<% End If%>

<asp:Panel ID="pnlMain" runat="server">
	<table style="width:900px;" cellspacing="0" cellpadding="0">
		<tr>
			<td colspan="2" style="padding: 2px;">
				<div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_Offers")%></div>
				<div class="sectionBar"></div>
			</td>
		</tr>
		<tr style="padding: 15px; padding-right: 0px;">
			<td colspan="2" width="100%" style="padding: 15px; padding-right: 0px;">
				<asp:PlaceHolder ID="phOfferEditors" runat="server"></asp:PlaceHolder>
			</td>
		</tr>
	</table>
</asp:Panel>
