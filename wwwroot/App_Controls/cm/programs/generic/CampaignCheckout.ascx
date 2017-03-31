<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CampaignCheckout.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.CampaignCheckout" %>
<asp:Timer runat="server" id="tmrRefresh" interval="3000" Enabled="false"></asp:Timer>
<asp:panel id="pnlCreateCampaign" runat="server">
	<asp:panel runat="server" id="pnlPreparingCampaign">
		<br />
		<div align="center"><table width="75%" cellspacing="0">
			<tr><td align="center" style="text-decoration:underline; font-weight:bold;"><%=DealerDirect.Resources.getString("LITERAL_Finalizing_Campaign")%></td></tr>
			<tr>
				<td align="center">
					<%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:PREPARING_CAMPAIGN")%>				
				</td>
			</tr>
			<tr><td align="center">
				<br /><br />
				<asp:image ID="imgPreparingCampaign" runat="Server" imageurl="/resources/images/cm/build/indicator_progress_blu.gif"/>
				<br />
				<table width="100%">
					<asp:PlaceHolder ID="phBuildStatus" runat="server"></asp:PlaceHolder>
				</table>
				<br />
				<asp:label id="lblErrorMessage" runat="server" Font-Bold="true"></asp:label>
			</td></tr>
		</table></div>
		<br /><br />
	</asp:panel>

	<asp:panel runat="server" id="pnlNoDataAvailable" Visible="false">
		<br />
		<table width="50%" align="center" cellspacing="0" cellpadding="4" border="1" BorderColor="#cccccc">
			<tr><td colspan="5" align="center">
			 <b><%=DealerDirect.Resources.getString("CM.ONESTEP.BUILD.ERROR_NO_DATA_FOUND")%></b>
			</td></tr>
		</table>
	</asp:panel>
</asp:panel> 	
	
<asp:Panel id="pnlApprove" runat="server">
  <table width="100%" cellspacing="0" style="border-color:#A8A8A8;">
		<tr><td colspan="6" style="border-bottom:1 solid"><b><%=DealerDirect.Resources.getString("LITERAL_Order_Summary")%></b></td></tr>
		<tr><td colspan="6" style="font-size:3pt">&nbsp;</td></tr>
    <tr>
			<td><b><%=DealerDirect.Resources.getString("LITERAL_Item_#")%></b></td>
			<td><b><%=DealerDirect.Resources.getString("LITERAL_Item_Description")%></b></td>
			<td align="center"><b><%=DealerDirect.Resources.getString("LITERAL_Lang")%></b></td>
			<td align="center"><b><%=DealerDirect.Resources.getString("LITERAL_Quantity")%></b></td>
			<td align="right"><b><%=DealerDirect.Resources.getString("LITERAL_Unit_Price")%></b></td>
			<td align="right"><b><%=DealerDirect.Resources.getString("LITERAL_Total_Price")%></b></td>
		</tr>
    <asp:placeholder id="phOrderItems" runat="server"/>
    <asp:placeholder id="phTotal" runat="server"/>
    <tr><td>&nbsp</td></tr>
    <asp:placeholder id="phAccountItems" runat="server"/>
    <tr><td colspan="6" style="border-bottom:1 solid"><b>&nbsp;</b></td></tr>
  </table>
	<br/>
	<table width="100%" align="center" cellspacing="0" cellpadding="4" style="border-width:1px; border-style:solid; border-color:#A8A8A8;">
		<tr style="border-width:0px"><td style="text-align:center; color:Red;">
			<b><%=DealerDirect.Resources.getString("CM.ONESTEP.BUILD.SUMMARY_NOTE")%></b>
		</td></tr>
	</table> 
	<br />
  <table width="100%" align="center" cellpadding="5px" style="border-width:1px; border-style:solid; border-color:#A8A8A8;">
		<tr><td align="center" style="text-decoration:underline; font-weight:bold;"><%=DealerDirect.Resources.getString("LITERAL_Order_Approval")%></td></tr>
    <tr><td align="center">
			<%=DealerDirect.Resources.getString("CM.BUILD.TREATMENT.REVIEW_APPROVE_COPY")%>
			<br /><br />
				<% 	Me.chkApproveCampaign.Text = DealerDirect.Resources.getString("LITERAL_Approve")%>
			<div align="center">
				<asp:Checkbox id="chkApproveCampaign" runat="server" autopostback="true"/>
				<br />
				<font color="red"><b><asp:Label runat="server" id="lblCheckApprove"></asp:Label></b></font>
			</div>
      <br/>
      <asp:ImageButton runat="server" ID="btnSubmit" CausesValidation="False"/>
      <br />
    </td></tr>
  </table>
</asp:Panel>