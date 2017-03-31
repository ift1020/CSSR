<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StyleOptionPrompt.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.StyleOptionPrompt" %>
<tr style="padding:3px">
	<td colspan="1" nowrap style="text-align:right; padding-left:10px; padding-right:3px;"><%=DealerDirect.Resources.getStringColon(me.StyleOption.NameResourceCode)%></td>
	<td colspan="1" width="100%" style="padding-left:3px; padding-right:3px; padding-bottom:3px; padding-top:3px; text-align:left"><asp:DropDownList ID="ddlStyles" runat="server"/></td>
</tr>