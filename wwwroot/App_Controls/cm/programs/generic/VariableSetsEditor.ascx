<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VariableSetsEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.VariableSetsEditor" %>
<%@ Register Assembly="DevExpress.Web.v11.1" Namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v11.1" Namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>
<table width="100%" cellspacing="0" cellpadding="0" style="border-style:solid; border-width:1; border-color:#A8A8A8;">
	<tr style="background-color:#E0E0E0;">
		<td colspan="2" style="padding:2px; border-bottom-width:1px; border-bottom-style:solid;">
			<b><%=DealerDirect.Resources.getString("LITERAL_Customize_Creative")%></b>
		</td>
	</tr>
	<tr style="padding:10px; padding-bottom:0px;">
		<td colspan="2" width="100%">
			<asp:PlaceHolder id="phVariablePrompts" runat="server"></asp:PlaceHolder>
		</td>
	</tr>
	<tr style="padding:10px;">
		<td colspan="2" width="100%">
			<dxtc:aspxpagecontrol id="axpcCreative" runat="server" width="100%" activetabindex="1" backcolor="transparent">
				<ContentStyle BackColor="Transparent"></ContentStyle>
				<ActiveTabStyle Font-Bold="True" BackColor="#003399" ForeColor="White"></ActiveTabStyle>
				<TabPages>
				</TabPages>
			</dxtc:aspxpagecontrol>
		</td>
	</tr>
	<tr style="padding:10px;">
		<td colspan="2" width="100%">
			<dxtc:aspxpagecontrol id="axpcLocations" runat="server" width="100%" activetabindex="1" backcolor="transparent">
				<ContentStyle BackColor="Transparent"></ContentStyle>
				<ActiveTabStyle Font-Bold="True" BackColor="#003399" ForeColor="White"></ActiveTabStyle>
				<TabPages>
				</TabPages>
			</dxtc:aspxpagecontrol>
		</td>
	</tr>
</table>