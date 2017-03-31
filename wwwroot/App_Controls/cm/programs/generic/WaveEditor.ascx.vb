Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class WaveEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericWaveEditor

		Protected m_Wave As DealerDirect.CampaignManagement.Programs.Wave

		Public ReadOnly Property RowStyleString() As String
			Get
				Dim s As String = ""
				If Not Me.Wave.Enabled Then s &= "background-color:#F3F3F3;"
				If Me.Wave.isExpired Then s &= "color:#C1C1C1;"
				Return s
			End Get
		End Property

		Public Property Wave() As DealerDirect.CampaignManagement.Programs.Wave Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericWaveEditor.Wave
			Get
				Return m_Wave
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Wave)
				m_Wave = value
			End Set
		End Property

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		End Sub

		Public Property HideLimit() As Boolean
			Get
				Return True
			End Get
			Set(ByVal value As Boolean)
			End Set
		End Property

		Public Function saveDataToObject() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericWaveEditor.saveDataToObject
			m_Wave.Selected = Me.chkSelected.Checked

			'If Me.txtLimit.Text.Length > 0 AndAlso CInt(Me.txtLimit.Text) > 0 Then
			'	m_Wave.MailLimit = CInt(Me.txtLimit.Text)
			'Else
			'	m_Wave.MailLimit = Nothing
			'End If
		End Function
	End Class
End Namespace