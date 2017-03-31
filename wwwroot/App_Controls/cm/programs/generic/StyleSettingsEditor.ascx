<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StyleSettingsEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.StyleSettingsEditor" %>
<%@ Register tagPrefix="PageControls" tagName="PreviewGenerator" src="~/app_controls/cm/programs/generic/PreviewGeneratorV3.ascx" %>

<table cellspacing="0" cellpadding="0" style="width:900px;">
	<tr>
		<td colspan="2" style="text-align:left;">
			<div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_Style")%></div>
			<div class="sectionBar"></div>
		</td>
	</tr>
	<tr style="">
		<td colspan="1" style="vertical-align:top; text-align:left">
			<asp:RadioButtonList runat="server" ID="rblStyles" AutoPostBack="true" RepeatDirection="Vertical">
			</asp:RadioButtonList>
			<asp:CustomValidator id="cvMain" Font-Bold="true" runat="Server"/>
		</td>
		<td style="padding-top:10px" >
			<asp:panel ID="pnlStyleOptions" runat="server">
			<table style="border:0px;" width="100%" cellspacing="0">
				<tr>
					<td colspan="2" style="padding:2px;text-align:left;">
					    <div class="sectionSubTitle"><%=DealerDirect.Resources.getString("LITERAL_Style_Options")%></div>
		           	    <div class="sectionSubBar"></div>          
					</td>
				</tr>
				<asp:PlaceHolder ID="phStyleOptions" runat="server"></asp:PlaceHolder>
			<asp:Panel ID="pnlSeasonMappings" runat="server">
				<tr><td colspan="2">&nbsp;</td></tr>
				<tr style="">
					<td colspan="2" style="text-align:left;">
						<div class="sectionSubTitle"><%=DealerDirect.Resources.getString("LITERAL_Season_Mappings")%></div>
			<div class="sectionSubBar"></div>
                      
					</td>
				</tr>
				<tr><td colspan="2" style="padding-top:5px">     
					<table cellpadding="0" cellspacing="0" border="0">
						<tr>
							<td >&nbsp;</td>
							<asp:PlaceHolder ID="phMonthLabels" runat="server"></asp:PlaceHolder>
						</tr><tr>
							<td style="white-space:nowrap; text-align:right; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("LITERAL_Spring")%></td>
							<asp:PlaceHolder ID="phMonthSettings" runat="server"></asp:PlaceHolder>
						</tr><tr>
							<td style="white-space:nowrap; text-align:right; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("LITERAL_Summer")%></td>
						</tr><tr>
							<td style="white-space:nowrap; text-align:right; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("LITERAL_Fall")%></td>
						</tr><tr>
							<td style="white-space:nowrap; text-align:right; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("LITERAL_Winter")%></td>
						</tr>
					</table>
					</td></tr>
				</asp:panel>
				</table>
			</asp:panel>
		</td>
	</tr>
</table> 