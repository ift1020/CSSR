<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PreviewGeneratorV3.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.PreviewGeneratorV3" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<style>
    .previewRowLabel {
        padding: 3px;
        padding-left: 55px;
        font-size: smaller;
    }

    .previewRowStatus {
        padding: 3px;
        text-align: center;
        font-size: smaller;
    }
</style>

<br />

<script type="text/javascript">
    var item = {
        vt: "",
        vv: "",
        lt: "",
        lv: "",
        ct: "",
        cv: ""
    }
    var items = [];

    function createPreviewRow(label, lang, channel, rid) {
        var table = document.getElementById("previewTable");

        // Create an empty <tr> element and add it to the 1st position of the table:
        var row = table.insertRow(-1);

        // Insert new cells (<td> elements) at the 1st and 2nd position of the "new" <tr> element:
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);

        // Add some text to the new cells:
        cell1.colSpan = 2;
        cell1.className = 'previewRowLabel';
        cell1.innerHTML = label + rid;
        cell1.id = 'label' + rid;

        cell2.className = 'previewRowStatus';
        cell2.innerText = lang;
		
        cell3.innerText = channel;
        cell3.className = 'previewRowStatus';

        cell4.innerHTML = '';
        cell4.className = 'previewRowStatus';
        cell4.id = 'status' + rid;
        return false;
    };

    function createPreview() {      
        prepareVersions();

        window.top.toastr["info"]("Please wait while your previews are generated");

        var time = new Date();
        var hh = ("0" + time.getHours()).slice(-2);
        var mm = ("0" + time.getMinutes()).slice(-2); 
        var ss = ("0" + time.getSeconds()).slice(-2); 
        var label, ddl;	

        label = hh + ":" + mm + ":" + ss;

        var hdr = document.getElementById("sampleHeaderRow");
        hdr.style.display = '';

        ddl = document.getElementById('<%=Me.ddlTarget.ClientID%>');
        var t = ddl.options[ddl.selectedIndex].value;
        label += ' - ' + ddl.options[ddl.selectedIndex].text;

        if (items.length > 0) {
            for (var i = 0, l = items.length; i < l; i++) {
                var _label = label + ' - ' + items[i].vt;
                console.log("doPreview " + _label);
                doPreview(items[i], _label, t);
            }
        }
		
        return false;
    };

    function theLoop(i, rid, c) {
        var cell = document.getElementById("status" + rid);
        
        setTimeout(function () {
            $.get("dataservice.aspx?op=getpreviewstatus&rid=" + rid + "&rnd=" + Math.random(),
				function (data) {
				    if (data !== 'Complete') {
				        cell.innerHTML = data;
				        if (data === "Failed") return;

				        i--;
				        if (i > 0) theLoop(i, rid, c);
				        else {
				            console.log("rid=" + rid + " timeout (>100)");
				            cell.innerHTML = "Failed";
				        }
				    }
				    else {
				        $.get("dataservice.aspx?op=getpreview&rid=" + rid + "&c=" + c,
							function (data) {
							    console.log(data);
							    cell.innerHTML = '<a href="' + data + '" target="_new">View</a>';
							});
				    }				
				});
        }, 500);
    }
	
    function doPreview(item, label, t) {
        $.get("dataservice.aspx?op=createpreview&pid=<%=Me.Program.ProgramID%>&sid=<%=Me.Program.SubCampaignID%>&sg=<%=Me.StyleGroupCode %>&l=" + item.lv + "&c=" + item.cv + "&v=" + item.vv + "&t=" + t + "&o=" + "&rnd=" + Math.random(),
		     function (rid) {
		         if (rid > 0) {
		             createPreviewRow(label, item.lt, item.ct, rid);

		             theLoop(100, rid, item.cv);
		         } else {
		             var msg = label + "-" + item.lt + "-" + item.ct + ' failed to assign request!';
		             console.error(msg);
		             window.top.toastr["error"](msg);
		         }
		     });
    }
</script>

<table cellspacing="0" cellpadding="0" style="border-style: none; width: 900px; border-color: #5397f0;" id="previewTable">
    <tr>
        <td colspan="5" style="text-align: left; padding: 2px;">
            <div class="sectionTitle"><%=DealerDirect.Resources.getString("LITERAL_Preview_Creative")%></div>
            <div class="sectionBar"></div>
        </td>
    </tr>
    <tr style="text-align: center; vertical-align: middle; padding: 3px; padding-bottom: 0px;">
        <td style="white-space: nowrap; text-align: right; padding: 3px;">
            <%=DealerDirect.Resources.getStringColon("LITERAL_Audience")%>	
        </td>
        <td style="text-align: left; border-right: 1px solid #5397f0; padding: 3px;">
            <asp:DropDownList ID="ddlTarget" runat="server" AutoPostBack="true" Width="300px" />
        </td>
        <td width="150" style="white-space: nowrap; padding-bottom: 0px; border-right: 1px solid #5397f0;">
            <%=DealerDirect.Resources.getStringColon("LITERAL_Language")%>
        </td>
        <td width="150" style="white-space: nowrap; padding-bottom: 0px; border-right: 1px solid #5397f0;">
            <%=DealerDirect.Resources.getStringColon("LITERAL_Channel")%>
        </td>
        <td width="150" rowspan="2" valign="middle">            
            <asp:Button ID="btnCreatePreview" runat="server" Text="Preview" CssClass="previewButton" OnClientClick="return createPreview();" />
        </td>
    </tr>
    <tr style="text-align: center; vertical-align: middle; padding: 3px;">
        <td style="white-space: nowrap; text-align: right; padding: 3px;">
            <%=DealerDirect.Resources.getStringColon("LITERAL_Version")%>	
        </td>
        <td style="white-space: nowrap; text-align: left; border-right: 1px solid #5397f0; padding: 3px;">
            <asp:DropDownList ID="ddlSubVersion" runat="server" AutoPostBack="false" Width="300px" Style="display: none" />
            <telerik:RadComboBox runat="server" CheckBoxes="true" ID="rcbMultiSV" RenderMode="Lightweight" Width="300px" AutoPostBack="false" Skin="Default">
            </telerik:RadComboBox>
        </td>
        <td style="border-right: 1px solid #5397f0; vertical-align: top; padding: 3px;" rowspan="1">
            <asp:CheckBox ID="chkEnglish" runat="server" Text="En" />
            <asp:CheckBox ID="chkFrench" runat="server" Text="Fr" />           
        </td>
        <td style="white-space: nowrap; border-right: 1px solid #5397f0; vertical-align: top; padding: 3px;" rowspan="1">
            <asp:CheckBox ID="chkMail" runat="server" Text="Mail" />
            <asp:CheckBox ID="chkEMail" runat="server" Text="eMail" />
        </td>
    </tr>
    <tr id="sampleHeaderRow" style="display: none;">
        <td colspan="5" style="text-align: left; padding-left: 50px; padding-top: 15px; padding-bottom: 5px;">
            <div class="sectionSubTitle"><%=DealerDirect.Resources.getString("LITERAL_Samples")%></div>
            <div class="sectionSubBar"></div>
        </td>
    </tr>
</table>
<br />
<br />

<script type="text/javascript">
    function OnClientLoadHandler(sender) {
        var combo;
        combo = sender;
        var items = sender.get_checkedItems();
        alert(items.length);
        if (items.length > 0) {
            items[0].select();
        }
    }
    function prepareVersions() {
        var combo1 = $find("<%= rcbMultiSV.ClientID%>");

        var chkLangEN = document.getElementById("<%= Me.chkEnglish.ClientID%>");
        var chkLangFR = document.getElementById("<%= Me.chkFrench.ClientID%>");

        var chkChanD = document.getElementById("<%= Me.chkMail.ClientID%>");
        var chkChanE = document.getElementById("<%= Me.chkEMail.ClientID%>");

        var items1 = combo1.get_checkedItems();
        //var items3 = combo3.get_checkedItems();

        items.length = 0;
        for (var i = 0, len1 = items1.length; i < len1; i++) {
            if (chkLangEN.checked) {
                if (chkChanD.checked) {
                    var node = $.extend(true, {}, item);
                    node.vt = items1[i].get_text();
                    node.vv = items1[i].get_value();
                    node.lt = "English"; //items2[j].get_text();
                    node.lv = "EN"; //items2[j].get_value();
                    node.ct = "Mail";
                    node.cv = "D";
                    items.push(node);
                }

                if (chkChanE.checked) {
                    var node = $.extend(true, {}, item);
                    node.vt = items1[i].get_text();
                    node.vv = items1[i].get_value();
                    node.lt = "English";
                    node.lv = "EN";
                    node.ct = "eMail";
                    node.cv = "E";
                    items.push(node);
                }
            }

            if (chkLangFR.checked) {
                if (chkChanD.checked) {
                    var node = $.extend(true, {}, item);
                    node.vt = items1[i].get_text();
                    node.vv = items1[i].get_value();
                    node.lt = "French"; //items2[j].get_text();
                    node.lv = "FR"; //items2[j].get_value();
                    node.ct = "Mail";
                    node.cv = "D";
                    items.push(node);
                }

                if (chkChanE.checked) {
                    var node = $.extend(true, {}, item);
                    node.vt = items1[i].get_text();
                    node.vv = items1[i].get_value();
                    node.lt = "French";
                    node.lv = "FR";
                    node.ct = "eMail";
                    node.cv = "E";
                    items.push(node);
                }
            }
        }
    };
    
</script>
