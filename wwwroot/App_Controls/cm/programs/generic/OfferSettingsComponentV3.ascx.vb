Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class OfferSettingsComponentV3
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent

		Protected m_ComponentIndex As Integer
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_LoadedControls As New System.Collections.SortedList

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
			Dim ioge As DealerDirect.UserControls.CampaignManagement.Programs.IOfferGroupEditor
			Dim ctrl As System.Web.UI.Control

			'If initial_load Then Me.chkSettingsApproved.Checked = m_Program.isComponentApproved(Me.ComponentIndex)

			For Each og As DealerDirect.CampaignManagement.Programs.Offers.OfferGroup In m_Program.OfferGroups.ToList.OrderBy(Function(f) f.DisplayOrderNumber)
				If Not m_LoadedControls.ContainsKey(og.Code) Then
					ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/OfferGroupEditor.ascx")
					ioge = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IOfferGroupEditor)
					ioge.Program = m_Program
					ioge.OfferGroup = og
					ctrl.ID = "oge" & og.Code
					ioge.loadData(initial_load)

					Me.phOfferEditors.Controls.Add(ctrl)
					m_LoadedControls.Add(og.Code, ioge)
				End If
			Next
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.validateData
			Dim b As Boolean = True

			For Each ioge As DealerDirect.UserControls.CampaignManagement.Programs.IOfferGroupEditor In Me.phOfferEditors.Controls
				If Not ioge.validateData Then
					b = False
				End If
			Next

			'Me.lblErrorMessage.Visible = Not b

			Return b
		End Function

		Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.saveData
			Dim b As Boolean = True

			For Each ioge As DealerDirect.UserControls.CampaignManagement.Programs.IOfferGroupEditor In Me.phOfferEditors.Controls
				If Not ioge.saveData Then
					b = False
				End If
			Next

			Return b
		End Function

		Public Sub setErrorMessage(ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
		End Sub

		Public Sub setErrorMessage1(ByVal error_code As String, ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
		End Sub
	End Class
End Namespace