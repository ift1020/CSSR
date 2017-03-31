<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ServiceSettingsComponentV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.ServiceReminders.ServiceSettingsComponentV3" %>
<table style="width:900px"><tr>
	<td style="text-align: right">
		<telerik:RadButton ID="rbtnComponentStatus" runat="server" Font-Bold="true" AutoPostBack="true" icon-PrimaryIconWidth="16" icon-PrimaryIconHeight="16" ButtonType="StandardButton" Skin="Default"  >
		</telerik:RadButton>
		<asp:Label Font-Bold="true" ForeColor="Red" Visible="false" runat="server" ID="lblErrorMessage"></asp:Label>
	</td>
	
	<%         If Me.HelpLink IsNot Nothing Then %>
	<td style="text-align:right; font-weight:bold;"><a href="<%=Me.HelpLink%>" target="_new">(<%=DealerDirect.Resources.getString("LITERAL_Help")%>)</a></td>
	<%         End If %>
</tr>
</table>
<br />
<% if me.InlineHelp isnot Nothing then %>
<table width="85%" align="center" cellspacing="0" cellpadding="4" style="border-width:1px; border-style:solid; border-color:#A8A8A8;">
	<tr style="border-width:0px"><td style="background-color:#E0E0E0;">
		<%=Me.InlineHelp%>
	</td></tr>
</table>
<br />
<% end if %>
<asp:panel ID="pnlMain" runat="server">


    <table style="width: 900px; border-spacing: 0; padding: 2px;">
	<tr>
        <td style="vertical-align:middle; white-space:nowrap;">
			<asp:CheckBox runat="server" ID="chkEnableTireChangeMessaging" AutoPostBack="true"/>
		</td>
		<td width="100%" style="padding:2px; text-align:left;">
            <div class="sectionTitle"><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TIRE_MESSAGING:NAME")%></div>
		</td>
</tr>
        <tr>

		<td colspan="2" style="padding: 0px;">
			
			<div class="sectionBar"></div>
		</td>
	</tr>

	<tr>
		<td style="padding:10px" colspan="2">
<asp:panel ID="pnlTireRotation" runat="server">
			<%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TR:SELECTED")%>			
</asp:panel>
<asp:panel ID="pnlTireChange" runat="server">
					<div style="text-align:center;"> 
					<i><%=DealerDirect.Resources.getStringColon("CM_PRGM:SMSP:SERVICES:TC:SELECTED")%></i><br/><br />
					<table style="border:0px;" cellpadding="0" cellspacing="0">
						<tr>
							<td style="border:0px;">&nbsp;</td>
							<asp:PlaceHolder ID="phMonthLabels" runat="server"></asp:PlaceHolder>
						</tr><tr>
							<td style="text-align:right; white-space:nowrap; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TR:LABEL")%></td>
							<asp:PlaceHolder ID="phMonthSettings" runat="server"></asp:PlaceHolder>
						</tr><tr>
							<td style="text-align:right; white-space:nowrap; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TC-L:LABEL")%></td>
						</tr><tr>
							<td style="text-align:right; white-space:nowrap; padding-left:3px; padding-right:3px;"><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TC-S:LABEL")%></td>
						</tr>
					</table>
					</div> 
					<br />
				<table width="100%" border="0">
					<tr><td style="text-align:right; white-space:nowrap; vertical-align:top; font-weight:bold;"><%=DealerDirect.Resources.getStringColon("CM_PRGM:SMSP:SERVICES:TR:LABEL")%></td>
						<td><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TR:DESC")%>
					</td></tr>
					<tr><td style="text-align:right; white-space:nowrap; vertical-align:top; font-weight:bold;"><%=DealerDirect.Resources.getStringColon("CM_PRGM:SMSP:SERVICES:TC-L:LABEL")%></td>
						<td><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TC-L:DESC")%>
					</td></tr>
					<tr><td style="text-align:right; white-space:nowrap; vertical-align:top; font-weight:bold;"><%=DealerDirect.Resources.getStringColon("CM_PRGM:SMSP:SERVICES:TC-S:LABEL")%></td>
						<td><%=DealerDirect.Resources.getString("CM_PRGM:SMSP:SERVICES:TC-S:DESC")%>
					</td></tr>
				</table>	
</asp:panel>
		</td>
	</tr>
</table>
<br />

</asp:panel>