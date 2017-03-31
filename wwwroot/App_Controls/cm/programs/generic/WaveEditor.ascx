<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WaveEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.WaveEditor" %>
<tr style="text-align:center;<%=Me.RowStyleString%>">
	<%
		Me.chkSelected.Enabled = Me.Wave.Enabled AndAlso Me.Wave.AllowDeSelect AndAlso Not Me.Wave.hasMailed
		Me.chkSelected.Checked = Me.Wave.Selected
		'Me.txtLimit.Visible = Me.Wave.Enabled
		'If Me.Wave.MailLimit.HasValue Then Me.txtLimit.Text = Me.Wave.MailLimit
	%>
	<%--<td width="0px" style="border-bottom-style:solid;border-bottom-width:1;">
		<asp:CheckBox ID="chkSelected" runat="server"/></td>--%>
	<td style="text-align:left; border-bottom-style:solid;border-bottom-width:1;">
		<asp:CheckBox ID="chkSelected" runat="server"/><%=Me.Wave.Name%></td>
	<td style="white-space:nowrap; padding-left:5px;padding-right:5px; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<%=Me.Wave.PeriodRange%></td>	
		
	<td style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">??</td>
	
	<td style="white-space:nowrap; padding-left:5px;padding-right:5px; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		July 1, 2011
	</td>
	<%--
	<% If Me.Wave.isExpired Then%>
	<td colspan="<%=iif(me.HideLimit, 2, 3)%>" style="font-style:italic; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<%=DealerDirect.Resources.getString("CM_PSCPN:ACR10:GWE:GROUP_EXPIRED")%></td>	
	<%
		ElseIf Me.Wave.hasMailed Then		
			Me.lblLimit.Text = Me.Wave.MailLimitText
			%>	
	<td style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<%=me.Wave.MailQuantityText %> </td>
	<% If Not Me.HideLimit then %>
	<td style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<asp:Label ID="lblLimit" runat="server"/>&nbsp;</td>
	<% end if %>
	<td colspan="1" style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<%=Me.Wave.MailDateText%>
		</td>
	<% else %>	
	<td style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		&nbsp;<%=me.Wave.MailQuantityText %>&nbsp;</td>
	<% If Not Me.HideLimit then %>
	<td style="border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<asp:TextBox ID="txtLimit" runat="server" Width="50px"/>&nbsp;</td>
	<% end if %>
	<td style="font-style:italic; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
		<%=Me.Wave.MailDateText%></td>	
	<% end if %>--%>
</tr>