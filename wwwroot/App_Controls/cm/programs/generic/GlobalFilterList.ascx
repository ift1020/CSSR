<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GlobalFilterList.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.GlobalFilterList" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<table style="width: 900px; border-spacing: 0; padding: 2px;">
	<tr>
		<td colspan="<%=me.TableColumnCount %>" style="padding: 0px;">
			<div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_General")%></div>
			<div class="sectionBar"></div>
		</td>
	</tr>
	<asp:PlaceHolder ID="phFilters" runat="server"></asp:PlaceHolder>
</table>
