<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TargetingSettingsComponentV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.TargetingSettingsComponentV3" %>
<%@ Register TagPrefix="PageControls" TagName="GlobalFilterList" Src="~/App_Controls/cm/programs/generic/GlobalFilterList.ascx" %>
<%@ Register TagPrefix="PageControls" TagName="SplitAudienceList" Src="~/App_Controls/cm/programs/generic/SplitAudienceList.ascx" %>
<%@ Register TagPrefix="PageControls" TagName="WaveList" Src="~/App_Controls/cm/programs/generic/WaveList.ascx" %>
<script type="text/javascript">
	function AlertOnEditSettings_TSC()
	{ alert('<%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:ALERT_ON_EDIT_SETTINGS")%>'); }
</script> 
<table style="width:900px"><tr>
	<td style="text-align: right">
		<telerik:RadButton ID="rbtnComponentStatus" runat="server" Font-Bold="true" AutoPostBack="true" icon-PrimaryIconWidth="16" icon-PrimaryIconHeight="16" ButtonType="StandardButton" Skin="Default"  >
		</telerik:RadButton>
		<asp:Label Font-Bold="true" ForeColor="Red" Visible="false" runat="server" ID="lblErrorMessage"></asp:Label>
	</td>
	
	<% if me.HelpLink isnot Nothing then %>
	<td style="text-align:right; font-weight:bold;"><a href="<%=me.HelpLink%>" target="_new">(<%=DealerDirect.Resources.getString("LITERAL_Help")%>)</a></td>
	<% end if %>
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
<asp:panel ID="pnlEstimating" runat="server">
	<asp:Timer runat="server" id="tmrRefresh" interval="2000" Enabled="false"></asp:Timer>
	<div align="center"><b><%=DealerDirect.Resources.getString("LITERAL_Please_wait_while_estimates_are_prepared")%>...</b></div>
	<br /><br />
</asp:panel>

<asp:panel ID="pnlMain" runat="server">
	<PageControls:GlobalFilterList runat="server" ID="gflMain"/>
	<% If Me.gflMain.Visible then %>
		<br /><br />
	<% end if %>
	<% If typeof (Me.Program) is DealerDirect.CampaignManagement.Programs.AudienceProgram then %>
	    <PageControls:SplitAudienceList id="gsalMain" runat="server" />
	<% elseIf typeof (Me.Program) is DealerDirect.CampaignManagement.Programs.WaveProgram then %>
	    <PageControls:WaveList id="wlMain" runat="server" />
	<% end if %>	
	<%If Me.BottomText isnot Nothing then %>
	<br />
	<%=Me.BottomText%>
<% end if %>
</asp:panel> 