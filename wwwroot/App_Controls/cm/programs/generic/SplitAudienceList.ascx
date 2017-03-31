<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SplitAudienceList.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.SplitAudienceList" %>

<table class="sal-table" width="900px" cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-left:2px; text-align:left; vertical-align:bottom;" width="50%">
            <div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_Audiences")%></div>
            </td>
        <td style="padding-right:2px; text-align:right; vertical-align:bottom;" width="50%">
            <div class="sectionTitle" style="font-size:smaller;">
                Estimates represent average expected weekly contacts and
                do not account for any seasonal or promotional variability.
            </div>
        </td>
    </tr>
    <tr>
        <td style="padding: 0px; text-align: left;" colspan="2">
            <div class="sectionBar"></div>
        </td>
    </tr>
    <tr>
        <td style="padding: 5px; padding-left: 15px; padding-right: 0px;" colspan="2" >
            <table cellspacing="0" cellpadding="2" style="border-spacing: 5px; padding-right: 0px;">
                <tr style="font-weight: bold;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="2" width="400px">&nbsp;</td>
                    <td colspan="1" width="100px" style="text-align: center;text-decoration:underline;">
                        <%=DealerDirect.Resources.getString("LITERAL_Mail")%></td>
                    <td colspan="1" width="100px" style="text-align: center;text-decoration:underline;">
                        <%=DealerDirect.Resources.getString("LITERAL_eMail")%></td>
                </tr>
                <tr style="height: 5px">
                    <td colspan="6"></td>
                </tr>
                <asp:PlaceHolder ID="phAudiences" runat="server"></asp:PlaceHolder>
                <asp:Panel ID="pnlNoAudiences" runat="server">
                    <tr style="font-weight: bold;">
                        <td colspan="6" style="text-align: center">
                            <br />
                            <%=Dealerdirect.Resources.getString("CM_PRGM:GENERIC:SAL:NO_DATA_FOUND") %>
                            <br />
                            <br />
                        </td>
                    </tr>
                </asp:Panel>
                <tr style="height: 25px">
                    <td colspan="3"></td>
                    <td colspan="3">
                        <div class="sectionSubBar"></div>
                    </td>
                </tr>
                <tr style="font-weight: bold;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="1" width="400px">&nbsp;</td>
                    <td colspan="1" style="text-align: right; white-space: nowrap; padding-right: 4px">
                        <%= DealerDirect.Resources.getStringColon("LITERAL_Piece_Sub_Total")%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <%=iif(Me.DMailTotalQuantity = 0, "-", Me.DMailTotalQuantity) %></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <%=iif(Me.EMailTotalQuantity = 0, "-", Me.EMailTotalQuantity )%></td>
                </tr>
                <% If Me.Program.PricingDisplayMode = DealerDirect.CampaignManagement.Programs.PricingDisplayMode.Campaign Then%>
                <tr style="font-weight: bold;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="1" width="400px">&nbsp;</td>
                    <td colspan="1" style="text-align: right; white-space: nowrap; padding-right: 4px">
                        <%=DealerDirect.Resources.getStringColon("LITERAL_Price_Per_Piece")%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <% If Me.DMailUnitPrice.HasValue Then%>
                        <%=iif(me.DMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", me.DMailUnitPrice))%>
                        <% Else%> - <%	End If%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <% If Me.EMailUnitPrice.HasValue Then%>
                        <%=iif(me.EMailUnitPrice.hasvalue, iif(me.EMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", me.EMailUnitPrice)), "?") %>
                        <% Else%> - <%	End If%></td>
                </tr>
                <tr style="font-weight: bold;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="1" width="400px">&nbsp;</td>
                    <td colspan="1" style="text-align: right; white-space: nowrap; padding-right: 4px">
                        <%=DealerDirect.Resources.getStringColon("LITERAL_Price_Sub_Total")%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <% If Me.DMailTotalQuantity = 0 Then%>-
				<% ElseIf Me.DMailUnitPrice.HasValue Then%>
                        <%=iif(me.DMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", Me.DMailTotalPrice))%>
                        <% Else%> - <%	End If%>	</td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <% If Me.EMailTotalQuantity = 0 Then%>-
				<% ElseIf Me.EMailUnitPrice.HasValue Then%>
                        <%=iif(me.EMailUnitPrice = 0, "<i>" & DealerDirect.Resources.getString("LITERAL_Free") & "</i>", string.Format("{0:c}", Me.EMailTotalPrice))%>
                        <% Else%> - <%	End If%></td>
                </tr>
                <%		ElseIf Me.Program.PricingDisplayMode = DealerDirect.CampaignManagement.Programs.PricingDisplayMode.Audience Then%>
                <tr style="font-weight: bold;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="1" width="400px">&nbsp;</td>
                    <td colspan="1" style="text-align: right; white-space: nowrap; padding-right: 4px">
                        <%=DealerDirect.Resources.getStringColon("LITERAL_Price_Sub_Total")%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <%=String.Format("{0:c}", If(Me.DMailTotalPrice, 0))%></td>
                    <td colspan="1" width="100px" style="text-align: center">
                        <%=String.Format("{0:c}", If(Me.EMailTotalPrice, 0))%></td>
                </tr>
                <% End If%>
                <% If Me.Program.PricingDisplayMode <> DealerDirect.CampaignManagement.Programs.PricingDisplayMode.None Then%>
                <tr style="height: 5px">
                    <td colspan="3"></td>
                    <td colspan="3">
                        <div class="sectionBar"></div>
                    </td>
                </tr>
                <tr style="font-weight: bold; border-color: #A8A8A8;">
                    <td width="20px">&nbsp;</td>
                    <td width="20px">&nbsp;</td>
                    <td colspan="1" width="400px">&nbsp;</td>
                    <td colspan="1" style="text-align: right; white-space: nowrap; padding-right: 4px">
                        <%=DealerDirect.Resources.getStringColon("LITERAL_Total_Price")%></td>
                    <td colspan="2" width="200px" style="text-align: center;">
                        <%=String.Format("{0:c}", If(Me.DMailTotalPrice, 0) + If(Me.EMailTotalPrice, 0))%>
                    </td>
                </tr>
                <% End If%>
            </table>
        </td>
    </tr>
</table>
