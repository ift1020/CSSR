<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AudienceSplitEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.AudienceSplitEditor" %>
<tr style="<%=Me.RowStyleString%>">
	<td width="20px">&nbsp;</td>
	<td width="20px" valign="top" style="padding:0px;">
		<asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="true" CausesValidation="false" /></td>
	<td colspan="2" style="text-align:left;">
		<%= me.AudienceSplit.Name %>
		<% 
            If Me.showDescToggle Then
                Me.lbtnToggleDesc.Visible = True

                If Me.isDescVisible Then
                    Me.lbtnToggleDesc.Text = "(" & DealerDirect.Resources.getString("LITERAL_Hide") & ")"
                    'Me.lblDesc.Visible = True
                    Me.lblDesc.Text = "<br/>" & Me.AudienceSplit.Description
                Else
                    Me.lbtnToggleDesc.Text ="(" & DealerDirect.Resources.getString("LITERAL_Details") & ")"
                    Me.lblDesc.Visible = False
                End If
            Else
                Me.lbtnToggleDesc.Visible = False
            End If
			%>
        <% If Me.showDescToggle AndAlso Me.isDescVisible Then %>
        <span class="audDescription"><%=Me.AudienceSplit.Description %></span>
        <% End if %>
			<asp:Label ID="lblDesc" Font-Italic="true" runat="server" CssClass="audDescription"/>
			<asp:LinkButton ID="lbtnToggleDesc" CausesValidation="false" runat="server" CssClass="audDescriptionLink"></asp:LinkButton>			
		</td>
	<td colspan="1" width="100px" style="<%=Me.QuantityRowStyleString%>text-align:center;vertical-align:middle;"><%=If(Me.AudienceSplit.DMailQuantity, 0) & IIf(Me.AudienceSplit.DMailQuantity.HasValue AndAlso Me.PriceVisible, String.Format(" ({0:c})", Me.AudienceSplit.DMailTotalPrice), "")%></td>
	<td colspan="1" width="100px" style="<%=Me.QuantityRowStyleString%>text-align:center;vertical-align:middle;"><%=If(Me.AudienceSplit.EMailQuantity, 0) & IIf(Me.AudienceSplit.EMailQuantity.HasValue AndAlso Me.PriceVisible, String.Format(" ({0:c})", Me.AudienceSplit.EMailTotalPrice), "")%></td>
</tr>