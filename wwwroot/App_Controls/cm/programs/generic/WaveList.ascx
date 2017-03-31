<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WaveList.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.WaveList" %>
<table width="650px" cellspacing="0" cellpadding="0" style="border-style:solid; border-width:1; border-color:#A8A8A8;">
	<tr style="background-color:#E0E0E0;">
		<td style="padding:2px; border-bottom-width:1px; border-bottom-style:solid;" width="100%">
			<b><%=DealerDirect.Resources.getString("LITERAL_Schedule")%></b>
		</td>
	</tr>
	<tr><td style="padding:5px">
		<table cellspacing="0" cellpadding="2" style="border-color:#A8A8A8; border-style:solid; border-width:1; border-bottom-width:0px;">		
			<tr style="font-weight:bold; text-align:center;">
				<td width="100%" style="text-align:left; white-space:nowrap; border-bottom-style:solid;border-bottom-width:1;">
					<%--<%=DealerDirect.Resources.getString("LITERAL_Select")%>
				</td>
				<td width="200" style="white-space:nowrap; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">--%>
					<%=DealerDirect.Resources.getString("LITERAL_Group")%></td>
				<td style="white-space:nowrap; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
					<%=DealerDirect.Resources.getString("LITERAL_Campaign_Period")%></td>
				<td style="white-space:nowrap; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
					&nbsp;<%=DealerDirect.Resources.getString("LITERAL_Mail_Qty")%>*&nbsp;</td>
			<%--	<% if not me.HideLimits then %>
				<td width="85" style="white-space:nowrap; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
					<%=DealerDirect.Resources.getString("LITERAL_Mail_Limit")%></td>
				<% end if %>--%>
				<td   style="white-space:nowrap; border-left-style:solid;border-left-width:1; border-bottom-style:solid;border-bottom-width:1;">
					<%=DealerDirect.Resources.getString("LITERAL_Mail_Date")%></td>
			</tr>
			<asp:placeholder ID="phWaveRows" runat="server"></asp:placeholder>
			<%--<tr>
				<td colspan="4" style="padding:5px;">
					Note: Quantities subject to change.
				</td>
			</tr>--%>
		</table>		
	</td></tr>
</table>
	
	
	
	
	<%--	<table cellspacing="0" cellpadding="2" style="border-color:#A8A8A8;">	
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="2" width="400px">&nbsp;</td>				
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;">
					<%=DealerDirect.Resources.getString("LITERAL_Mail")%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;">
					<%=DealerDirect.Resources.getString("LITERAL_eMail")%></td>
			</tr>
			<tr style="height:5px"><td colspan="6"></td></tr>
			<asp:PlaceHolder ID="phAudiences" runat="server"></asp:PlaceHolder>
			<asp:panel ID="pnlNoAudiences" runat="server">
				<tr style="font-weight:bold;">
					<td colspan="6" style="text-align:center; border-style:solid;border-width:1;border-right-style:solid;">
						<br />
						<%=Dealerdirect.Resources.getString("CM_PRGM:GENERIC:SAL:NO_DATA_FOUND") %>
						<br /><br />
					</td>
				</tr>
			</asp:panel>
			<tr style="height:5px"><td colspan="6"></td></tr>
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="1" width="400px">&nbsp;</td>
				<td colspan="1" style="background-color:#F3F3F3;text-align:right;white-space:nowrap; padding-right:4px; border-width:1;border-bottom-style:solid;border-left-style:solid; border-top-width:2;border-top-style:solid;">
					 <%= DealerDirect.Resources.getStringColon("LITERAL_Piece_Sub_Total")%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-left-style:solid;border-right-style:solid; border-top-width:2;border-top-style:solid;">
					<%=iif(Me.DMailTotalQuantity = 0, "-", Me.DMailTotalQuantity) %></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-right-style:solid; border-top-width:2;border-top-style:solid;">
					<%=iif(Me.EMailTotalQuantity = 0, "-", Me.EMailTotalQuantity )%></td>
			</tr>
<% 	If Me.Program.PricingDisplayMode = DealerDirect.CampaignManagement.Programs.PricingDisplayMode.Campaign Then%>
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="1" width="400px">&nbsp;</td>
				<td colspan="1" style="background-color:#F3F3F3;text-align:right;white-space:nowrap; padding-right:4px; border-width:1;border-bottom-style:solid;border-left-style:solid;">
					 <%=DealerDirect.Resources.getStringColon("LITERAL_Price_Per_Piece")%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-left-style:solid;border-right-style:solid;">
				<% If Me.DMailUnitPrice.HasValue then %>
					<%=iif(me.DMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", me.DMailUnitPrice))%>
				<% else %> - <% end if %></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-right-style:solid;">
				<% If Me.EMailUnitPrice.HasValue then %>
					<%=iif(me.EMailUnitPrice.hasvalue, iif(me.EMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", me.EMailUnitPrice)), "?") %> 
				<% else %> - <% end if %></td>
			</tr>
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="1" width="400px">&nbsp;</td>
				<td colspan="1" style="background-color:#F3F3F3;text-align:right;white-space:nowrap; padding-right:4px; border-width:1;border-bottom-style:solid;border-left-style:solid;">
					<%=DealerDirect.Resources.getStringColon("LITERAL_Price_Sub_Total")%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-left-style:solid;border-right-style:solid;">
				<% if Me.DMailTotalQuantity = 0 then %>-
				<% elseIf Me.DMailUnitPrice.HasValue then %>
					<%=iif(me.DMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", Me.DMailTotalPrice))%>
				<% else %> - <% end if %>	</td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-right-style:solid;">
				<% if Me.EMailTotalQuantity = 0 then %>-
				<% elseIf Me.EMailUnitPrice.HasValue then %>
					<%=iif(me.EMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", Me.EMailTotalPrice))%>
				<% else %> - <% end if %></td>
			</tr>
<% 	ElseIf Me.Program.PricingDisplayMode = DealerDirect.CampaignManagement.Programs.PricingDisplayMode.Audience Then%>
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="1" width="400px">&nbsp;</td>
				<td colspan="1" style="background-color:#F3F3F3;text-align:right;white-space:nowrap; padding-right:4px; border-width:1;border-bottom-style:solid;border-left-style:solid;">
					<%=DealerDirect.Resources.getStringColon("LITERAL_Price_Sub_Total")%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-left-style:solid;border-right-style:solid;">
				<%=String.Format("{0:c}", If(Me.DMailTotalPrice, 0))%></td>
				<td colspan="1" width="100px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-right-style:solid;">
				<%=String.Format("{0:c}", If(Me.EMailTotalPrice, 0))%></td>
			</tr>
<% end if %>
			<tr style="height:5px"><td colspan="6"></td></tr>
			<tr style="font-weight:bold;">
				<td width="20px">&nbsp;</td>
				<td width="20px">&nbsp;</td>
				<td colspan="1" width="400px">&nbsp;</td>
				<td colspan="1" style="background-color:#F3F3F3;text-align:right;white-space:nowrap; padding-right:4px; border-width:1;border-bottom-style:solid;border-left-style:solid; border-top-width:2;border-top-style:solid;">
					<%=DealerDirect.Resources.getStringColon("LITERAL_Total_Price")%></td>
				<td colspan="2" width="200px" style="text-align:center;vertical-align:middle; border-width:1;border-bottom-style:solid;border-left-style:solid;border-right-style:solid; border-top-width:2;border-top-style:solid;">
					<%=String.Format("{0:c}", If(Me.DMailTotalPrice, 0) + If(Me.EMailTotalPrice, 0))%>
				</td>
			</tr>--%>
