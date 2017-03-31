Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class TargetingSettingsComponentV3
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent

		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_ComponentIndex As Integer
		Protected m_EstimatesReady As Boolean
		Protected m_InitialLoadDone As Boolean

		Protected m_BottomText As String
		Public Property BottomText() As String
			Get
				If DealerDirect.Resources.doesStringExist(m_BottomText) Then
					Return DealerDirect.Resources.getString(m_BottomText)
				Else
					Return Nothing ' m_BottomText
				End If
			End Get
			Set(ByVal value As String)
				m_BottomText = value
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

		Public Event ApprovalStateChanged(ByVal sender As DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent, ByVal new_approval_state As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.ApprovalStateChanged

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

		Public ReadOnly Property isApproved() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.isApproved
			Get
				Return m_Program.isComponentApproved(Me.ComponentIndex)
			End Get
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			m_EstimatesReady = Me.Program.EstimatesReady

			Me.gflMain.Program = Me.Program

			If TypeOf (Me.Program) Is DealerDirect.CampaignManagement.Programs.AudienceProgram Then
				Me.gsalMain.Program = DirectCast(Me.Program, DealerDirect.CampaignManagement.Programs.AudienceProgram)
			End If

			Me.pnlEstimating.Visible = Not m_EstimatesReady
			Me.pnlMain.Visible = m_EstimatesReady

			Me.tmrRefresh.Enabled = Not m_EstimatesReady

			Me.lblErrorMessage.Text = DealerDirect.Resources.getString("CM_PRGM:GENERIC:MUST_APPROVE_SETTINGS")
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			' Common calls
			DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved) ' Program.isComponentApproved(Me.ComponentIndex))
			Me.pnlMain.Enabled = Not Me.isApproved

			m_InitialLoadDone = Me.ViewState("initial_load_done")

			If m_EstimatesReady AndAlso Not m_InitialLoadDone Then
				Me.loadData(True)
				m_InitialLoadDone = True
			End If


			'Me.lblErrorMessage.Visible = Not Me.chkSettingsApproved.Checked

			'If Me.chkSettingsApproved.Checked Then
			'	Me.chkSettingsApproved.Text = DealerDirect.Resources.getString("CM_PRGM:GENERIC:UNCHECK_TO_EDIT")
			'Else
			'	Me.chkSettingsApproved.Text = DealerDirect.Resources.getString( _
			'	 IIf(Me.Program.CampaignExecutionMode = DealerDirect.CampaignManagement.Programs.ExecutionMode.Batch, _
			'	 "CM_PRGM:GENERIC:CHECK_TO_APPROVE_AND_SAVE", "CM_PRGM:GENERIC:CHECK_TO_APPROVE_AND_CONTINUE"))
			'End If

			'Me.pnlApprove.Visible = Me.gflMain.Visible


			Me.ViewState("estimates_ready") = m_EstimatesReady
			Me.ViewState("initial_load_done") = m_InitialLoadDone
		End Sub

		Protected Sub rbtnComponentStatus_Click(sender As Object, e As EventArgs) Handles rbtnComponentStatus.Click
			Dim bInitialValue = Me.isApproved	 'chkSettingsApproved.Checked

			If Not bInitialValue AndAlso Me.validateData AndAlso Me.saveData Then
				m_Program.setComponentApproval(Me.ComponentIndex, True)	
			Else
				m_Program.setComponentApproval(Me.ComponentIndex, False)
			End If

			DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved)

			If bInitialValue <> Me.isApproved Then 'Me.chkSettingsApproved.Checked Then
				RaiseEvent ApprovalStateChanged(Me, Me.isApproved)

				Response.Redirect(Page.Request.Url.PathAndQuery)
			End If
		End Sub


		Public Event onValidateData(ByRef sender As Object, ByRef valid As Boolean)

		Public Sub bubbleOnValidateData(ByRef sender As Object, ByRef valid As Boolean)
			RaiseEvent onValidateData(sender, valid)
		End Sub


		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.loadData
			
			Me.gflMain.Program = Me.Program
			Me.gflMain.loadData(initial_load)

			If TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.AudienceProgram Then
				Select Case Me.Program.AudienceMode
					Case DealerDirect.CampaignManagement.Programs.AudienceMode.Split
						Me.gsalMain.Program = DirectCast(Me.Program, DealerDirect.CampaignManagement.Programs.AudienceProgram)
						AddHandler Me.gsalMain.onValidateData, AddressOf bubbleOnValidateData
						Me.gsalMain.loadData(initial_load)
				End Select

			ElseIf TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.WaveProgram Then
				Me.wlMain.Program = DirectCast(Me.Program, DealerDirect.CampaignManagement.Programs.WaveProgram)
			End If
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.validateData
			' clear error
			'Me.lblErrorMessage.Visible = False

			If Me.gflMain.validateData Then
				If TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.AudienceProgram Then
					Select Case Me.Program.AudienceMode
						Case DealerDirect.CampaignManagement.Programs.AudienceMode.Split
							Return gsalMain.validateData
					End Select
				ElseIf TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.WaveProgram Then
					Return True
				End If
			Else
				Me.lblErrorMessage.Text = DealerDirect.Resources.getString("LITERAL_Please_correct_the_indicated_errors")
				Return False
			End If
		End Function

		Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.saveData
			If Me.gflMain.Visible Then If Not Me.gflMain.saveData() Then Return False

			If Me.gsalMain.saveDataToObject Then
				If TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.AudienceProgram Then
					If Not DirectCast(Me.Program, DealerDirect.CampaignManagement.Programs.AudienceProgram).Audiences.SaveAll Then Return False

				ElseIf TypeOf Me.Program Is DealerDirect.CampaignManagement.Programs.WaveProgram Then
					'If Not DirectCast(Me.Program, DealerDirect.CampaignManagement.Programs.WaveProgram).Waves.sa Then	'Audiences.SaveAll) Then return false
				End If
			Else
				Return False
			End If

			If m_Program.SetupFilterSet IsNot Nothing AndAlso Not m_Program.SetupFilterSet.Save Then Return False
			Return True
		End Function

		Public Sub setErrorMessage(ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
			'Me.lblErrorMessage.Visible = True
			Me.lblErrorMessage.Text = error_message
		End Sub

		Public Sub setErrorMessage1(ByVal error_code As String, ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
		End Sub
	End Class
End Namespace