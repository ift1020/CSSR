Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class ScheduledProgramHeader
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericControl

		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements IGenericControl.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			If m_Program.Enabled Then
				Me.lblStatus.Text = DealerDirect.Resources.getListItem(Me.Program.StatusTextValueListCode, "Y")
				Me.lbtnToggleStatus.Text = DealerDirect.Resources.getListItem(Me.Program.StatusTextValueListCode, "D")
				Me.lblStatus.ForeColor = Drawing.Color.Green

				If m_Program.BatchExecStartDate.HasValue AndAlso m_Program.BatchExecEndDate.HasValue Then
					Me.lblSchedule.Text = String.Format(DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:RUN_BW"), m_Program.BatchExecStartDate.Value.ToLongDateString, m_Program.BatchExecEndDate.Value.ToLongDateString)
				ElseIf m_Program.BatchExecStartDate.HasValue Then
					Me.lblSchedule.Text = String.Format(DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:RUN_OA"), m_Program.BatchExecStartDate.Value.ToLongDateString)
				ElseIf m_Program.BatchExecEndDate.HasValue Then
					Me.lblSchedule.Text = String.Format(DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:RUN_OB"), m_Program.BatchExecEndDate.Value.ToLongDateString)
				Else
					Me.lblSchedule.Text = ""
				End If

			Else
				Me.lblStatus.Text = DealerDirect.Resources.getListItem(Me.Program.StatusTextValueListCode, "N")
				Me.lbtnToggleStatus.Text = DealerDirect.Resources.getListItem(Me.Program.StatusTextValueListCode, "A")
				Me.lblStatus.ForeColor = Drawing.Color.Red
				Me.lblSchedule.Text = DealerDirect.Resources.getString("CM_PRGM:BASE:SCHED:RUN_NO")
			End If
		End Sub

		Protected Sub lbtnToggleStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnToggleStatus.Click
			Dim bDesiredStatus As Boolean

			bDesiredStatus = Not m_Program.Enabled
			If Not m_Program.setProgramStatus(Not m_Program.Enabled) AndAlso bDesiredStatus = True Then
				Me.lblError.Text = DealerDirect.Resources.getString("LITERAL_You_must_first_approve_each_tab")
			Else
				Me.lblError.Text = Nothing
			End If
		End Sub
	End Class
End Namespace
