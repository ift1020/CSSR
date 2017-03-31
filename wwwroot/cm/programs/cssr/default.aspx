<%@ Page Language="VB" MasterPageFile="~/resources/masterpages/CSSRMain.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="cm_programs_cssr_CSSRMain" title="CSSR Main Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterContent" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/resources/StyleSheets/multiple-select.min.css" >
    <link rel="stylesheet" type="text/css" media="screen,projection,print" href="/resources/StyleSheets/cssr/cssr-main.css?_=<%=Date.Now.Ticks.ToString()%>">

	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnableHistory="true" EnablePageMethods="true">
		<Services>
            <asp:ServiceReference Path="~/ws/cm/cssr/CSSR.svc"/>
		</Services>
	</asp:ScriptManager>
	
    <style>
        section.loading {
            display: none;
            width: 16rem;
            height: 16rem;
            position: fixed;
            left: 55%;
            top: 60%;
            transform: translate(-50%, -50%);
            z-index: 1099;
        }

        .back-to-top {
            position: fixed;
            bottom: 1.6em;
            right: 20px;
            text-decoration: none;
            padding-top: 3px;
            padding-left: 1px;
            display: none;
        }

        .back-to-top:hover {
            box-shadow: 0 5px 5px rgba(34, 25, 25, 0.2);
        }

        .back-to-top > i {
            position: relative;
            font-size: 2rem;
            font-weight: 800;
            /*left: 13%;*/
        }

        .btn-floating {
            display: inline-block;
            color: #FFF;
            /*position: relative;*/
            z-index: 1;
            width: 37px;
            height: 37px;
            line-height: 37px;
            padding: 0;
            background-color: #26a69a;
            -webkit-border-radius: 50%;
            -moz-border-radius: 50%;
            border-radius: 50%;
            background-clip: padding-box;
            -webkit-transition: 0.3s;
            -moz-transition: 0.3s;
            -o-transition: 0.3s;
            -ms-transition: 0.3s;
            transition: 0.3s;
        }

        .waves-effect {
            overflow: hidden;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            -webkit-tap-highlight-color: transparent;
            vertical-align: middle;
            z-index: 1;
            will-change: opacity, transform;
            -webkit-transition: all 0.3s ease-out;
            -moz-transition: all 0.3s ease-out;
            -o-transition: all 0.3s ease-out;
            -ms-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
        }

    </style>

    <script>
        var myVar = {
            dealerID : <%=DealerDirect.Security.CurrentSession.SecurityContext.WorkSpace.ID%>,
            userID   : <%=DealerDirect.Security.CurrentSession.SecurityContext.UserID%>,
            language: "<%=DealerDirect.Security.CurrentSession.Language.ToLower%>",
            ms_activate: "<%=DealerDirect.Resources.getString("LITERAL_Activate")%>",
            ms_deactivate: "<%=DealerDirect.Resources.getString("LITERAL_Deactivate")%>"
        }
    </script>
    
    <div id="master-container" data-ng-app="" data-ng-init="msActivate='<%=DealerDirect.Resources.getString("LITERAL_Activate")%>'; msDeactivate='<%=DealerDirect.Resources.getString("LITERAL_Deactivate")%>'">
        <div class="right-pane">
            <div class="rightPane-inner">
                <div id="cardsview" class="pane-content">
                    <div class="header">
                        <i class="fa fa-lg fa-fw fa-th" style="color: #FFF"></i>
                        <span><%=DealerDirect.Resources.getString("CM_PRGM:CSSR:TITLE") %></span>
                    </div>

                    <div class="col-full">
                        
                    </div>
                </div>

                <div id="cssrDocument" class="pane-content">
                    <div class="header">
                        <i class="fa fa-lg fa-fw fa-th fa-gears" style="color: #FFF"></i>
                        <span><%=DealerDirect.Resources.getString("CM_PRGM:CSSR:TITLE") %></span>
                    </div>

                    <div class="col-full">
                        <div id="cssrbody">
                            CSSR is a dynamic lifecycle customer retention communication program, and also one of the core elements of the GM Retail Excellence Program.<br/>To be compliant for REP and have a Green status for CSSR you must complete each of the tabs on the left for each of the communications included in the program. Once all the communications have been completed and activated the status beside each tab as well as the CSSR logo will turn to a green check mark.
                        </div>
                        <div id="cssrimg" style="text-align: center">
                            <img src="/resources/images/cssr/cssr_wheel_en.png" style="width: 50%"/>
                        </div>
                        <div id="cssrfooter">
                            For additional information on the CSSR program, please click <a href="/content/static/cm/programs/cssr/CSSR_en.pdf" target="_new"><b>here</b></a> to view the CSSR program overview.
                        </div>
                    </div>
                </div>

                <div id="control-panes"></div>
            </div>
        </div>
    </div>

    <!-- loading animation present block -->
    <section class="loading">
        <div class="dd-loading">
            <img src="/resources/images/buttons/animated_loading.gif" />
        </div>
    </section>

    <!-- back to top button -->
    <a href="#" class="back-to-top btn-floating waves-effect" title="<%= DealerDirect.Resources.getString("LITERAL_Back_to_top")%>"
        style="display: none; width: 55.5px; height: 55.5px; line-height: 56px; background-color: #F44336">
        <i class="fa fa-lg fa-fw fa-angle-double-up"></i>
    </a>

    <script src="/resources/scripts/jquery.multiple.select.min.js"></script>
    <script src="/resources/scripts/cssr/cm-main.js?_=<%=Date.Now.Ticks.ToString()%>"></script>

    <script>
        var paneIndex = 0;
        var summaryData = [];

        if (dealerdirect.getParameterByName("_i")) {    // come from entrance page
            paneIndex = parseInt(dealerdirect.decode_utf8(window.atob(dealerdirect.getParameterByName("_i"))));

            var url = window.location.href.split('?')[0];
            history.replaceState({}, null, url);

            if (sessionStorage.getItem("_s")) {
                summaryData = JSON.parse(dealerdirect.decode_utf8(window.atob(sessionStorage.getItem("_s"))));
                console.log("summary: " + JSON.stringify(summaryData, null, 2));
            }
            else {
                var url = "/site/CSSR/default.aspx" ;
                window.location = url;
            }
        }
        else {
            cssr.getSummary();
        }
    </script>

</asp:Content>