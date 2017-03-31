<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SplitAudienceEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.SplitAudienceEditor" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<tr >
	<td colspan="4" width="50%" style="text-align:left;border-bottom:1px solid #5397f0;">
		<asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="true"/>
		<%=Me.Audience.Name%>
			<% 
                If Me.Audience.hasDescription Then
                    Me.lbtnToggleDesc.Visible = True

                    If Me.isDescVisible Then
                        Me.lbtnToggleDesc.Text = "(" & DealerDirect.Resources.getString("LITERAL_Hide") & ")"
                        'Me.lblDesc.Visible = True
                        'Me.lblDesc.Text = "<br/>" & Me.Audience.Description
                    Else
                        Me.lbtnToggleDesc.Text = "(" & DealerDirect.Resources.getString("LITERAL_Details") & ")"
                        'Me.lblDesc.Visible = False
                    End If
                Else
                    Me.lbtnToggleDesc.Visible = False
                End If
			%>
          <% If Me.Audience.hasDescription AndAlso Me.isDescVisible Then %>
        <span class="audDescription"><%=Me.Audience.Description %></span>
        <% End if %>

<%--		<asp:Label ID="lblDesc" Font-Italic="true" runat="server" CssClass="audDescription"/>--%>
		<asp:LinkButton ID="lbtnToggleDesc" CausesValidation="false" runat="server" CssClass="audDescriptionLink"></asp:LinkButton>	
		<div><asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true"/></div>
	</td>
	<%	If Not m_Audience.hasSplits Then%>
	<td colspan="1" rowspan="<%=iif(me.hasEditableFilters, 2, 1)%>" width="100px" nowrap style="text-align:center; white-space:nowrap;border-bottom:1px solid #5397f0;">&nbsp;<%=IIf(Me.DMailGrossQuantity.HasValue, If(Me.DMailGrossQuantity, 0), "-") & IIf(Me.DMailGrossQuantity.HasValue AndAlso m_Program.PricingDisplayMode <> DealerDirect.CampaignManagement.Programs.PricingDisplayMode.None, String.Format(" ({0:c})", Me.DMailGrossPrice), "")%>&nbsp;</td>
	<td colspan="1" rowspan="<%=iif(me.hasEditableFilters, 2, 1)%>" width="100px" nowrap style="text-align:center; white-space:nowrap;border-bottom:1px solid #5397f0;">&nbsp;<%=IIf(Me.EMailGrossQuantity.HasValue, If(Me.EMailGrossQuantity, 0), "-") & IIf(Me.EMailGrossQuantity.HasValue AndAlso m_Program.PricingDisplayMode <> DealerDirect.CampaignManagement.Programs.PricingDisplayMode.None, String.Format(" ({0:c})", Me.EMailGrossPrice), "")%>&nbsp;</td>
	<%	Else %>
	<td colspan="2" ></td>
	<% end if %>
</tr>

<asp:Panel ID="pnlFilters" runat="server">
<tr align="left">
	<td width="20px" style="background-color:Transparent;">&nbsp;</td>
	<td align="left" colspan="3" width="420px" style="text-align:left; padding:0px">
		<table width="429px" style="text-align:left; padding:0px;">
			<asp:PlaceHolder ID="phFilters" runat="server"/>
		</table>
	</td>	
</tr>
</asp:Panel>

<asp:panel id="pnlAudience" runat="server">
<asp:PlaceHolder ID="phAudienceSplits" runat="server"></asp:PlaceHolder>
<% if Me.Audience.ShowLimits then %>
<tr style="<%=Me.RowStyleString%>">
	<td width="20px">&nbsp;</td>
	<td width="20px">&nbsp;</td>
	<td colspan="1" width="200px" >&nbsp;</td>
	<td colspan="1" style="text-align:right; white-space: nowrap; padding: 4px">
		<%=DealerDirect.Resources.getStringColon("LITERAL_Audience_Limit")%></td>
	<td colspan="1" width="100px" style="text-align:center; vertical-align: middle">
		<% if Me.Audience.AllowDMailLimit then %>
		<div align="center">
		<dx:ASPxTextBox ID="txtDMaiLimit" runat="server" AutoPostBack="true" OnValueChanged="txtDMaiLimit_ValueChanged"
			Font-Size="8" Height="10px" Paddings-Padding="0px" HorizontalAlign="Center" Width="50px"/>
		</div>
		<% else %> - <% end if %>
	</td>
	<td colspan="1" width="100px" style="text-align:center;vertical-align:middle;border-width:1px;border-right-style:solid;border-bottom-style:solid;border-color:#A8A8A8;">
		<% if Me.Audience.AllowEMailLimit then %>
		<div align="center">
		<dx:ASPxTextBox ID="txtEMaiLimit" runat="server" AutoPostBack="true" OnValueChanged="txtEMaiLimit_ValueChanged"
			Font-Size="8" Height="10px" Paddings-Padding="0px" HorizontalAlign="Center" Width="50px"/>
		</div>
		<% else %> - <% end if %>
	</td>
</tr>
<% end if %>
<% if Me.Program.PricingDisplayMode = DealerDirect.CampaignManagement.Programs.PricingDisplayMode.Audience andalso m_Audience.hasSplits then %>
<tr style="<%=Me.RowStyleString%>">
	<td width="20px">&nbsp;</td>
	<td width="20px">&nbsp;</td>
	<td colspan="1" width="400px" >&nbsp;</td>
	<td colspan="1" style="text-align: right; white-space:nowrap;padding:4px; border-top:1px solid #5397f0;">
		<%=DealerDirect.Resources.getStringColon("LITERAL_Audience_Total")%></td>
	<td colspan="1" width="100px" nowrap style="text-align:center; white-space:nowrap; border-top:1px solid #5397f0;">&nbsp;<%=iif(me.DMailNetQuantity.HasValue, If(Me.DMailNetQuantity, 0),  "-") & iif(me.DMailNetQuantity.HasValue, String.Format(" ({0:c})", Me.DMailNetPrice), "") %>&nbsp;</td>
	<td colspan="1" width="100px" nowrap style="text-align:center; white-space:nowrap; border-top:1px solid #5397f0;">&nbsp;<%=iif(me.EMailNetQuantity .HasValue, If(Me.EMailNetQuantity, 0), "-") & iif(me.EMailNetQuantity.HasValue, String.Format(" ({0:c})", Me.EMailNetPrice), "") %>&nbsp;</td>
</tr>
<% end if %>
</asp:panel>
<% if not Me.isLastAudience then %>
<tr style="height:5px"><td colspan="6"></td></tr>
<% end if %>