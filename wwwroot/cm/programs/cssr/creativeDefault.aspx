<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="creativeDefault.aspx.vb" Inherits="cm_programs_cssr_creativeDefault" %>
<%@ Register TagPrefix="PageControls" TagName="CreativeSettingsComponent" Src="~/app_controls/cm/programs/generic/CreativeSettingsComponentV3.ascx" %>

<script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.1.1.js" type="text/javascript"></script>

<form id="form1" runat="server">
    <style>
        html {
            font-family: Roboto,sans-serif;
        }

         .showMeLink {
		    font-weight:bold;
            color: #5397f0;
		    font-size: .8em;
            text-decoration: none;
        }


        .previewButton {
          background: #5397f0;
          background-image: -webkit-linear-gradient(top, #5397f0, #5397f0);
          background-image: -moz-linear-gradient(top, #5397f0, #5397f0);
          background-image: -ms-linear-gradient(top, #5397f0, #5397f0);
          background-image: -o-linear-gradient(top, #5397f0, #5397f0);
          background-image: linear-gradient(to bottom, #5397f0, #5397f0);
          -webkit-border-radius: 28;
          -moz-border-radius: 28;
          border-radius: 28px;
          font-family: Arial;
          color: #ffffff;
          font-size: 16px;
	        font-weight:bold;
          padding: 5px 10px 5px 10px;
          text-decoration: none;
        }

        .sectionBar {
            width: 100%;
            height: 3px;
            margin: 1px auto;
            background-color: #5397f0;
            border-radius: 3px;
        }

        .sectionTitle {
		        font-weight:bold;
            color: #5397f0;
		        font-size: 1.3em;
        }

        .sectionSubTitle {
		        font-weight:bold;
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
			background-color: <%=iif(me.Program.isComponentApproved(3), "#01A453", "rgb(244, 67, 54)")%>;
		}

			.RadButton_Default .rbDecorated:hover {
				box-shadow: 2px 2px 5px 0 rgba(0, 0, 0, 0.16), inset 2px 2px 10px 0 rgba(0, 0, 0, 0.12);
			}

        .rbPrimaryIcon {
            top: 12px;
            /*margin-left: 4px;*/
            display: none;
        }

        .RadButton .rbPrimary {
            padding: 0 15px;
        }
    </style>

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

	<PageControls:CreativeSettingsComponent runat="server" ID="cseMain" ComponentIndex="3" DisablePreviewGenerator="false" />

</form>