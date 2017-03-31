<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OfferTargetPrompt.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.OfferTargetPrompt" %>
<table style="margin-bottom:5px" cellspacing="0" width="100%">
	<tr >
		<td colspan="1" style="" width="20px">
			<asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="true" /></td>
		<td colspan="2" width="100%" style="border-bottom:1px solid #5397f0; text-align: left; color:#5397f0;">
			<b><%=m_OfferTarget.Name%></b></td>
	</tr>
	<%	If Me.chkSelected.Checked OrElse Not m_OfferTarget.AllowDeSelect Then%>
	<tr>
		<td></td>
		<td colspan="2" style="padding: 5px;"" align="left">
			<table cellpadding="0" cellspacing="0" border="0" style="padding: 0px">
				<tr>
					<td>
						<asp:DropDownList runat="server" ID="ddlOffers" AutoPostBack="true" />
					</td>
					<td style="padding-left: 5px">
						<asp:Panel ID="pnlExpiryDate" runat="server"><%=DealerDirect.Resources.getString("LITERAL_expiring")%>
							<asp:TextBox ID="txtExpiryDays" runat="server" Width="30px" />
							<%=Dealerdirect.Resources.getString("LITERAL_days_after_mailing") %></asp:Panel>
					</td>
				</tr>
				<tr align="left">
					<td colspan="2">
						<asp:Label runat="server" ID="lblOfferError" ForeColor="Red" Font-Bold="true"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<%	Else%>
	<tr>
		<td></td>
		<td colspan="2" align="left" style="padding: 5px;">
			<%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:GROUP_NO_OFFER")%>
		</td>
	</tr>
	<%	End If%>
</table>

