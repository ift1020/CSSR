<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OfferGroupEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.OfferGroupEditor" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<table cellspacing="0" cellpadding="0" width="100%">
	<tr>
		<td style="vertical-align: middle; white-space: nowrap;" width="20px">
			<asp:CheckBox runat="server" ID="chkSelected" AutoPostBack="true"/>
		</td>
		<td style="text-align: left; vertical-align: middle; white-space: nowrap; border-bottom: 2px solid #5397f0" width="100%">
			<div class="sectionSubTitle"><%=me.OfferGroup.Name%></div>
		</td>
		<td style="text-align: right; vertical-align: middle; white-space:nowrap; border-bottom: 2px solid #5397f0">
			<%If m_OfferGroup.AllowAdvancedSetupMode Then %>
			<div class="sectionSubTitle" style="font-weight:normal;"><% =DealerDirect.Resources.getStringColon("LITERAL_Setup_Mode")%></div>				
			<%End If%>
		</td>
		<td style="text-align: right; white-space:nowrap; border-bottom: 2px solid #5397f0; color:#5397f0;" width="25%">
			 <asp:RadioButtonList CellPadding="0" ID="rblSetupMode" runat="server" CssClass="radioWithProperWrap sectionSubTitle" Font-Bold="false" 
				RepeatDirection="Horizontal" AutoPostBack="true" >
				<asp:ListItem Value="S" Text="Simple"></asp:ListItem>
				<asp:ListItem Value="A" Text="Advanced"></asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
	<% If Not (Me.chkSelected.Checked OrElse Not m_OfferGroup.AllowDeSelect) Then %>
	<tr>
		<td style="padding: 5px; padding-left:25px;text-align:left;" colspan="4">			
			<%=string.Format(DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:CUST_NO_OFFER"), me.m_OfferGroup.OfferTargets.toListString("<b>","</b>"), iif(m_OfferGroup.OfferTargets.Count = 1, DealerDirect.Resources.getString ("LITERAL_group"), DealerDirect.Resources.getString("LITERAL_groups")))  %>
			<div style="font-size:smaller;text-align:right;">
			<%	If m_OfferGroup.AllowAdvancedSetupMode Then%>
				<i><%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:SWITCH_TO_ADVANCED")%></i>
			<%           End If %>
				</div>
		</td>
	</tr>
	<% elseif Me.rblSetupMode.SelectedValue = "A" then %>
	<tr>
		<td style="padding:5px; padding-left:35px;" colspan="4">			
			<asp:PlaceHolder ID="phOfferTargets" runat="server"/>			
			<div style="text-align:right;font-size:smaller;">
			<% If m_OfferGroup.AllowAdvancedSetupMode Then%>
				<i><%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:SWITCH_TO_SIMPLE")%></i>
			<% end if %>
			</div>
		</td>
	</tr>	
	<% else %>
	<tr>
		<td style="text-align:left; padding:10px; padding-left:25px;padding-right:0px;" colspan="4">
			<%=string.Format(DealerDirect.Resources.getStringColon("CM_PRGM:GENERIC:OFFERS:CUST_OFFER"), me.m_OfferGroup.OfferTargets.toListString("<b>","</b>"), iif(m_OfferGroup.OfferTargets.Count = 1, DealerDirect.Resources.getString ("LITERAL_group"), DealerDirect.Resources.getString("LITERAL_groups")))  %>
			<br />
			<table style="padding:5px; padding-left:25px;text-align:left;padding-right:0px" cellspacing="0" border="0"  >
				<tr>
					<td style="text-align:right;">
						<asp:DropDownList runat="server" ID="ddlSimpleOffer" AutoPostBack="true"/>
					</td>
					<td style="padding-left:5px;text-align:left;width:100pc">
						<asp:panel ID="pnlExpiryDate" runat="server"><%=DealerDirect.Resources.getString("LITERAL_expiring")%> <asp:TextBox ID="txtExpiryDays" runat="server" width="30px"/> <%=Dealerdirect.Resources.getString("LITERAL_days_after_mailing") %></asp:panel>
					</td>
				</tr>
				<asp:panel ID="pnlVariableSet" runat="server">
				<tr><td colspan="2" style="text-align:left" >
					<br />
					<table width="100%" cellspacing="0" cellpadding="2" style="text-align:left;">
						<tr>
							<td style="padding:2px; border-bottom: solid 1px #5397f0; color: #5397f0">
								<b><%=DealerDirect.Resources.getString("LITERAL_Offer_Options")%></b>
							</td>
						</tr>
						<tr><td align="left"><asp:PlaceHolder ID="phVariableSet" runat="server" /></td></tr>
					</table> 
				</td></tr>
				</asp:panel>
				<tr align="center"><td colspan="2"><asp:label runat="server" ID="lblSimpleOfferError" ForeColor="Red" Font-Bold="true"></asp:label>
				</td></tr>
			</table>
			<br />
			<div style="text-align:right;font-size:smaller">
			<%	If m_OfferGroup.AllowAdvancedSetupMode Then%>
			<i><%=DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:SWITCH_TO_ADVANCED")%></i>
			<% end if %>
				</div>
		</td>
	</tr>
	<% End If %>	
</table>
<br />