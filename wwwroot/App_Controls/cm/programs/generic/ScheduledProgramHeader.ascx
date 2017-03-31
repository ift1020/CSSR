<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ScheduledProgramHeader.ascx.vb" Inherits="DealerDirect.UserControls.CampaignManagement.Programs.Generic.ScheduledProgramHeader" %>
<% If Me.Program.EndDate.HasValue andalso Me.Program.EndDate.Value >= System.DateTime.Today Then%>
<div align="center"><i><%=String.Format(DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:PROG_AVAIL"), Me.Program.EndDate.Value.ToLongDateString)%></i></div><br />
<% elseif Me.Program.EndDate.HasValue then %>
<div align="center"><i>
	<%=String.Format(DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:PROG_COMP"), Me.Program.EndDate.Value.ToLongDateString)%>	
</i></div><br />
<% End If%>
<div align="left">
	<table>
		<tr style="font-weight:bold; font-size:13px">
			<td valign="top" align="right"><%=DealerDirect.Resources.getStringColon("LITERAL_Status")%></td>
			<td><span style="color:<%=Me.Program.StatusTextColor%>"><asp:label runat="Server" ID="lblStatus"/></span> (<asp:LinkButton runat="server" ID="lbtnToggleStatus"/>)</td>
			<td><span style="color:Red"><asp:label runat="Server" ID="lblError"/></span></td>
		</tr>
<% if me.program.batchexecstartdate.hasvalue orelse Me.Program.BatchExecEndDate.HasValue then  %>
		<tr style="font-weight:bold; font-size:13px">
			<td valign="top" align="right"><%=DealerDirect.Resources.getStringColon("LITERAL_Schedule")%></td>
			<td colspan="2"><span style="color:<%=Me.Program.StatusTextColor%>"><asp:label runat="Server" ID="lblSchedule"/></span></td>
		</tr>
	<% end if %>
	</table>
</div>