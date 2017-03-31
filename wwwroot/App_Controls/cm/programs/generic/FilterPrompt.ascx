<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FilterPrompt.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.FilterPrompt" %>
<style type="text/css">.checkBoxListWrap td{text-indent:-23px;padding-left:25px;padding-right:10px;vertical-align:top; }</style>
<asp:CheckBoxList id="cblMain" CssClass="checkBoxListWrap" runat="server" CellSpacing="5" RepeatDirection="Horizontal" TextAlign="Right" AutoPostBack="true"/>
<asp:RadioButtonList id="rblMain" CssClass="checkBoxListWrap" runat="server" CellSpacing="0" RepeatDirection="Horizontal"  TextAlign="Right" AutoPostBack="true"/>
<asp:DropDownList ID="ddlMain" runat="server" TextAlign="Right" AutoPostBack="true"/>
<asp:label runat="server" ID="lblError" ForeColor="Red" Font-Bold="true" />