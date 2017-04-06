Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class CreativeSettingsComponentV3
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent

		Protected m_ComponentIndex As Integer
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase

		Protected m_DisablePreviewGenerator As Boolean = False
		Protected m_PreviewGenerator As DealerDirect.UserControls.CampaignManagement.Programs.IPreviewGenerator
		Protected m_PreviewGeneratorControlPath As String = "~/app_controls/cm/programs/generic/PreviewGeneratorV3.ascx"

		Public Event ApprovalStateChanged(ByVal sender As DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent, ByVal new_approval_state As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.ApprovalStateChanged

		Public Property PreviewGeneratorControlPath As String
			Get
				Return m_PreviewGeneratorControlPath
			End Get
			Set(value As String)
				m_PreviewGeneratorControlPath = value
			End Set
		End Property


		Protected m_HelpLink As String
		Public Property HelpLink() As String Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.HelpLink
			Get
				If DealerDirect.Resources.doesStringExist(m_HelpLink) Then
					Return DealerDirect.Resources.getString(m_HelpLink)
				Else
					Return m_HelpLink
				End If
			End Get
			Set(ByVal value As String)
				m_HelpLink = value
			End Set
		End Property

		Protected m_InlineHelp As String
		Public Property InlineHelp() As String Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.InlineHelp
			Get
				If DealerDirect.Resources.doesStringExist(m_InlineHelp) Then
					Return DealerDirect.Resources.getString(m_InlineHelp)
				Else
					Return m_InlineHelp
				End If
			End Get
			Set(ByVal value As String)
				m_InlineHelp = value
			End Set
		End Property

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Public Property ComponentIndex() As Integer Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.ComponentIndex
			Get
				Return m_ComponentIndex
			End Get
			Set(ByVal value As Integer)
				m_ComponentIndex = value
			End Set
		End Property

		Public Property DisablePreviewGenerator() As Boolean
			Get
				Return m_DisablePreviewGenerator
			End Get
			Set(ByVal value As Boolean)
				m_DisablePreviewGenerator = value
			End Set
		End Property

		Public ReadOnly Property isApproved() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.isApproved
			Get
				Return m_Program.isComponentApproved(Me.ComponentIndex)
			End Get
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Me.sseMain.Program = Me.Program
			Me.sseMain.Visible = (Me.Program.StyleGroups.Count > 1)

			If Not m_DisablePreviewGenerator Then createPreviewGenerator()
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved)
			Me.pnlMain.Enabled = Not Me.isApproved
		End Sub

		Protected Sub rbtnComponentStatus_Click(sender As Object, e As EventArgs) Handles rbtnComponentStatus.Click
			Dim bInitialValue = Me.isApproved

			If Not bInitialValue AndAlso Me.validateData AndAlso Me.saveData Then
				m_Program.setComponentApproval(Me.ComponentIndex, True)
			Else
				m_Program.setComponentApproval(Me.ComponentIndex, False)
			End If

			DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved)

			If bInitialValue <> Me.isApproved Then
				RaiseEvent ApprovalStateChanged(Me, Me.isApproved)
				Response.Redirect(Page.Request.Url.PathAndQuery)
			End If
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.loadData
			Me.sseMain.Program = m_Program
			Me.sseMain.loadData(initial_load)
			Me.pnlStyle.Visible = (m_Program.StyleGroups.Count > 1)

			m_Program.StyleGroupCode = Me.sseMain.StyleGroupCode
			Me.vseMain.Program = m_Program
            'Me.vseMain.StyleGroupCode = Me.sseMain.StyleGroupCode
            Me.vseMain.loadData(initial_load OrElse Me.isApproved)

            If Not Me.DisablePreviewGenerator Then
				createPreviewGenerator()
				m_PreviewGenerator.loadData(initial_load)
			End If
		End Sub

		Private Sub createPreviewGenerator()
			If m_PreviewGenerator Is Nothing Then
				Dim ctrl As System.Web.UI.Control

                If m_Program.PreviewGeneratorControlPath Is Nothing OrElse m_Program.PreviewGeneratorControlPath.Length = 0 Then
                    ctrl = Page.LoadControl(m_PreviewGeneratorControlPath) 'm_PreviewGeneratorControlPath)
                Else
                    ctrl = Page.LoadControl(m_Program.PreviewGeneratorControlPath) 'm_PreviewGeneratorControlPath)
                End If

                m_PreviewGenerator = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IPreviewGenerator)
				m_PreviewGenerator.Program = m_Program
				m_PreviewGenerator.StyleGroupCode = Me.sseMain.StyleGroupCode
				AddHandler m_PreviewGenerator.PrepareVariablesForPreview, AddressOf pgMain_PrepareVariablesForPreview
				ctrl.ID = "pgMain"

				Me.phPreviewGenerator.Controls.Add(ctrl)
			End If
		End Sub


		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.validateData
			Dim b As Boolean = True

			b = (Not Me.pnlStyle.Visible) OrElse Me.sseMain.validateData()
			b = b And Me.vseMain.validateData
			'Me.lblErrorMessage.Visible = Not b

			Return b
		End Function

		Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.saveData
			If Me.pnlStyle.Visible Then
				Return Me.sseMain.saveData AndAlso Me.vseMain.saveData
			Else
				Return Me.vseMain.saveData
			End If
		End Function

		Private Function savePreviewData() As Boolean
			Dim bSuccess As Boolean = True
			Return vseMain.saveDataPreviewData AndAlso ((Not Me.pnlStyle.Visible) OrElse sseMain.savePreviewData)
		End Function

		Protected Sub pgMain_PrepareVariablesForPreview(ByVal sender As Object, ByRef success As Boolean)
			success = (Me.validateData AndAlso Me.savePreviewData)
		End Sub

		Public Sub setErrorMessage(ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
			'Me.lblErrorMessage.Text = error_message
		End Sub

		Public Sub setErrorMessage1(ByVal error_code As String, ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
		End Sub
	End Class
End Namespace