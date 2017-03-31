Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class ExecutionSummary
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IExecutionSummary

		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements IExecutionSummary.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			pnlPreviousCampaign.Visible = (m_Program.CampaignExecutionMode = DealerDirect.CampaignManagement.Programs.ExecutionMode.User AndAlso m_Program.hasPreviousInstance)
			If Me.pnlPreviousCampaign.Visible Then
				If m_Program.SameSourceSuppress Then
					Me.lblMessage.Text = DealerDirect.Resources.getString("CM_PRGM:GENERIC:PRIOR_EXEC_SUPR_YES")
				Else
					Me.lblMessage.Text = DealerDirect.Resources.getString("CM_PRGM:GENERIC:PRIOR_EXEC_SUPR_NO")
				End If
			End If
		End Sub
	End Class
End Namespace
