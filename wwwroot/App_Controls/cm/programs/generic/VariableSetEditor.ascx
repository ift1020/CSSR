<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VariableSetEditor.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.VariableSetEditor" %>
<%@ Register Assembly="DevExpress.Web.v11.1" Namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v11.1" Namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>
<asp:panel ID="pnlMain" runat="server">
<table cellspacing="2" cellpadding="2" >
	<asp:PlaceHolder id="phVariablePrompts" runat="server"></asp:PlaceHolder>
</table>
</asp:panel>