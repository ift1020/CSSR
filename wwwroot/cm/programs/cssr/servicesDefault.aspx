<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="servicesDefault.aspx.vb" Inherits="cm_programs_cssr_servicesDefault" %>
<%@ Register TagPrefix="PageControls" TagName="ServiceSettingsComponent" Src="~/app_controls/cm/programs/servicereminders/ServiceSettingsComponentV3.ascx" %>


<form id="form1" runat="server">
   <style>
		html {
			font-family: sans-serif;
		}

		.sectionBar {
			width: 100%;
			height: 3px;
			margin: 1px auto;
			background-color: #5397f0;
			border-radius: 3px;
		}

		.sectionTitle {
			font-weight: bold;
			color: #5397f0;
			font-size: 1.3em;
		}

		.sectionSubTitle {
			font-weight: bold;
			color: #5397f0;
			font-size: 1em;
		}

		.sectionSubBar {
			width: 100%;
			height: 2px;
			margin: 1px auto;
			background-color: #5397f0;
			border-radius: 3px;
		}

		.RadButton_Default.rbSkinnedButton {
			height: 40px;
			background-image: none;
		}

		.RadButton_Default .rbDecorated {
			color: #fff;
			height: 40px;
			border-radius: 3px;
			background-image: none;
			background-color: <%=iif(me.Program.isComponentApproved(4), "#01A453", "rgb(244, 67, 54)")%>;
		}

		.RadButton_Default .rbDecorated:hover {
			box-shadow: 2px 2px 5px 0 rgba(0, 0, 0, 0.16), inset 2px 2px 10px 0 rgba(0, 0, 0, 0.12);
		}

		.rbPrimaryIcon {
			top: 12px;
			display: none;
		}

		.RadButton .rbPrimary {
			padding: 0 15px;
		}
	</style>

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

	<PageControls:ServiceSettingsComponent runat="server" ID="sseMain" ComponentIndex="4"  />
</form>