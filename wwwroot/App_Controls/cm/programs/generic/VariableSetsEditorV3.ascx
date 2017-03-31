<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VariableSetsEditorV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.VariableSetsEditorV3" %>
<table cellspacing="0" cellpadding="0" style="width:900px;border:none;">
	<tr >
		<td colspan="2" style="padding: 2px;">
			<div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_Customize_Creative")%></div>
			<div class="sectionBar"></div>
		</td>
	</tr>
	<tr>
		<td colspan="2" style="padding:5px;">
			<table>		
				<tr style="padding-left:25px;">
					<td colspan="2" style="width:100pc">
						<asp:PlaceHolder ID="phVariablePrompts" runat="server"></asp:PlaceHolder>
					</td>
				</tr>
				<tr style="padding:20px;" >
					<td colspan="2" style="width:400px;padding-top:15px;padding-bottom:15px;padding-left:50px">
						<div class="sectionSubTitle"><%=DealerDirect.Resources.getString("LITERAL_English")%></div>
						<div class="sectionSubBar"></div>
						<asp:PlaceHolder ID="phVariablePromptsEN" runat="server"></asp:PlaceHolder>
						<%--<telerik:RadTabStrip ID="rtsCreative" runat="server" MultiPageID="rmpCreative" Skin="Default"></telerik:RadTabStrip>
						<telerik:RadMultiPage ID="rmpCreative" runat="server" />--%>
					</td>
					</tr>
				<tr style="padding:20px;" >
					<td colspan="2" style="width:400px;padding-top:15px;padding-bottom:15px;padding-left:50px">
						<div class="sectionSubTitle"><%=DealerDirect.Resources.getString("LITERAL_French")%></div>
						<div class="sectionSubBar"></div>
						<asp:PlaceHolder ID="phVariablePromptsFR" runat="server"></asp:PlaceHolder>
						<%--<telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="rmpCreative" Skin="Default"></telerik:RadTabStrip>
						<telerik:RadMultiPage ID="RadMultiPage1" runat="server" />--%>
					</td>
				</tr>
				<tr style="padding: 10px;">
					<td colspan="2">
						<br />
						<telerik:RadTabStrip ID="rtsLocations" runat="server" MultiPageID="rmpLocations" Skin="Default"></telerik:RadTabStrip>
						<telerik:RadMultiPage ID="rmpLocations" runat="server" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
