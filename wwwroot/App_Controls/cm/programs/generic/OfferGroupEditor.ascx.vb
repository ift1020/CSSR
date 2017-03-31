Namespace DealerDirect.UserControls.CampaignManagement.Programs
	Partial Class OfferGroupEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IOfferGroupEditor

		Protected m_VariableSetEditors As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
		Protected m_AddedVariableSets As New System.Collections.Specialized.StringDictionary
		Protected m_OfferGroup As DealerDirect.CampaignManagement.Programs.Offers.OfferGroup
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_LoadedControls As New System.Collections.SortedList
		Protected m_OfferChanged As Boolean
		Protected m_Loading As Boolean

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements ISettingsEditor.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property
		Public Property OfferGroup() As DealerDirect.CampaignManagement.Programs.Offers.OfferGroup Implements IOfferGroupEditor.OfferGroup
			Get
				Return m_OfferGroup
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Offers.OfferGroup)
				m_OfferGroup = value
			End Set
		End Property

		Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
			Dim ogo As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOffer
			ogo = m_OfferGroup.AvailableOffers.Where(Function(f) f.Code = Me.ddlSimpleOffer.SelectedValue).FirstOrDefault
			If ogo IsNot Nothing Then
				If ogo.hasVariableSet Then loadVariableSet(ogo.VariableSetID, False)
			End If
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Dim ogo As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOffer
			Dim alControlsToRemove As New System.Collections.ArrayList

			Me.rblSetupMode.Items(0).Text = DealerDirect.Resources.getString("LITERAL_Simple")
			Me.rblSetupMode.Items(1).Text = DealerDirect.Resources.getString("LITERAL_Advanced")

            Me.rblSetupMode.Enabled = (Me.chkSelected.Checked OrElse Not m_OfferGroup.AllowDeSelect)
            Me.pnlVariableSet.Visible = False

			ogo = m_OfferGroup.AvailableOffers.Where(Function(f) f.Code = Me.ddlSimpleOffer.SelectedValue).FirstOrDefault
			If ogo IsNot Nothing Then
				Me.pnlExpiryDate.Visible = (Not ogo.isAutoOffer) AndAlso ogo.OfferExpires
				If ogo.hasVariableSet Then loadVariableSet(ogo.VariableSetID, m_OfferChanged)
			End If

			If Me.pnlExpiryDate.Visible AndAlso Me.txtExpiryDays.Text.Length = 0 Then
				Me.txtExpiryDays.Text = m_OfferGroup.ExpiryDays
			End If
		End Sub

		Public Sub loadVariableSet(variable_set_id As Integer, initial_load As Boolean)
			Dim vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor
			Dim alControlsToRemove As New System.Collections.ArrayList
			Dim ctrl As System.Web.UI.Control

			Me.pnlVariableSet.Visible = True

			If initial_load Then
				m_AddedVariableSets.Clear()
				phVariableSet.Controls.Clear()
			End If

			ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
			vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
			vse.Program = Me.Program
			vse.VariableSet = Me.Program.VariableSets(variable_set_id)
			ctrl.ID = "vse" & m_OfferGroup.Code & vse.VariableSet.ID
			vse.loadData(initial_load)

			If m_VariableSetEditors.Where(Function(f) f.VariableSet.ID = vse.VariableSet.ID).Count = 0 Then
				m_VariableSetEditors.Add(vse)
				Me.phVariableSet.Controls.Add(ctrl)
			End If

			For Each ctrl In Me.phVariableSet.Controls
				If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor Then
					vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
					If vse.VariableSet.ID <> variable_set_id Then
						alControlsToRemove.Add(ctrl)
						m_VariableSetEditors.Remove(vse)
					End If
				End If
			Next

			For Each ctrl In alControlsToRemove				
				Me.phVariableSet.Controls.Remove(ctrl)
			Next
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements ISettingsEditor.loadData
			Dim otp As DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt
			Dim ogo As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOffer
			Dim ctrl As System.Web.UI.Control

			Me.ddlSimpleOffer.DataSource = m_OfferGroup.AvailableOffers.ToList.OrderBy(Function(f) f.DisplayOrderNumber)
			Me.ddlSimpleOffer.DataTextField = "Name"
			Me.ddlSimpleOffer.DataValueField = "Code"
			Me.ddlSimpleOffer.DataBind()

			If initial_load Then
				Me.chkSelected.Checked = m_OfferGroup.Selected
				Me.rblSetupMode.SelectedValue = m_OfferGroup.SetupModeCode
				Me.loadDataB()
			End If

			ogo = m_OfferGroup.AvailableOffers.Where(Function(f) f.Code = Me.ddlSimpleOffer.SelectedValue).FirstOrDefault
			If ogo IsNot Nothing Then
				If ogo.hasVariableSet Then loadVariableSet(ogo.VariableSetID, initial_load)
			End If

			Me.rblSetupMode.Visible = m_OfferGroup.AllowAdvancedSetupMode
            Me.chkSelected.Visible = m_OfferGroup.AllowDeSelect
            'Me.chkSelected.Enabled = m_OfferGroup.AllowDeSelect

            For Each ot As DealerDirect.CampaignManagement.Programs.Offers.OfferTarget In m_OfferGroup.OfferTargets.OrderBy(Function(f) f.DisplayOrderNumber)
                If Not m_LoadedControls.ContainsKey(ot.Code) Then
                    ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/OfferTargetPrompt.ascx")
                    otp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt)
                    otp.Program = m_Program
                    otp.OfferTarget = ot
                    otp.AvailableOffers = m_OfferGroup.AvailableOffers
                    ctrl.ID = "otp" & ot.Code
                    otp.loadData(initial_load)

                    Me.phOfferTargets.Controls.Add(ctrl)
                    m_LoadedControls.Add(ot.Code, otp)
                End If
            Next
        End Sub

		Private Sub loadDataB()
			Me.ddlSimpleOffer.SelectedValue = m_OfferGroup.SimpleOfferCode
			If Me.txtExpiryDays.Text.Trim.Length = 0 Then Me.txtExpiryDays.Text = m_OfferGroup.ExpiryDays
		End Sub

		Public Function validateData() As Boolean Implements ISettingsEditor.validateData
			Dim b As Boolean = True

            If Not (Me.chkSelected.Checked OrElse Not m_OfferGroup.AllowDeSelect) Then
                Return True

            Else
                If Not m_OfferGroup.AllowAdvancedSetupMode OrElse Me.rblSetupMode.SelectedValue = "S" Then
					If Me.ddlSimpleOffer.SelectedValue <> "" Then
						Dim n As Integer

						If Me.pnlExpiryDate.Visible Then
							If Integer.TryParse(Me.txtExpiryDays.Text, n) Then
								b = (n >= 30)
							Else : b = False
							End If

							If Not b Then Me.lblSimpleOfferError.Text = "<br/>" & DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:MISSING_EXPIRY")
						End If

						If b Then
							Dim ogo As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOffer
							ogo = m_OfferGroup.AvailableOffers.Where(Function(f) f.Code = Me.ddlSimpleOffer.SelectedValue).FirstOrDefault
							If ogo IsNot Nothing AndAlso ogo.hasVariableSet Then
								For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
									If vse.VariableSet.ID = ogo.VariableSetID Then
										If Not vse.validateData Then
											b = False
										End If
									End If
								Next
							End If
						End If

					Else
						b = False
						Me.lblSimpleOfferError.Text = "<br/>" & DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:MISSING_OFFER")
					End If

				ElseIf Me.rblSetupMode.SelectedValue = "A" Then
					For Each iotp As DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt In Me.phOfferTargets.Controls
						If Not iotp.validateData Then b = False
					Next
				End If
			End If

			If b Then Me.lblSimpleOfferError.Text = Nothing

			Return b
		End Function

		Public Function saveData() As Boolean Implements ISettingsEditor.saveData
            Dim b As Boolean = True
            Dim bChecked As Boolean

            bChecked = Me.chkSelected.Checked OrElse Not (m_OfferGroup.AllowDeSelect)

            For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
				If Not vse.saveData Then Return False
			Next

            If Not bChecked Then 'Me.chkSelected.Checked Then
                Return m_OfferGroup.setValues(bChecked, Me.rblSetupMode.SelectedValue, Nothing, 0)

            ElseIf bChecked AndAlso Me.rblSetupMode.SelectedValue = "S" Then
                Dim nOfferExpiryDays As Integer
                If Me.txtExpiryDays.Visible Then
                    If Integer.TryParse(Me.txtExpiryDays.Text, nOfferExpiryDays) Then
                        Return m_OfferGroup.setValues(bChecked, Me.rblSetupMode.SelectedValue, Me.ddlSimpleOffer.SelectedValue, nOfferExpiryDays)
                    Else
                        Return False
                    End If
                Else
                    Return m_OfferGroup.setValues(bChecked, Me.rblSetupMode.SelectedValue, Me.ddlSimpleOffer.SelectedValue, 0)
                End If

            ElseIf bChecked AndAlso Me.rblSetupMode.SelectedValue = "A" Then
                If m_OfferGroup.setValues(bChecked, Me.rblSetupMode.SelectedValue, Me.ddlSimpleOffer.SelectedValue, 0) Then
                    For Each iotp As DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt In Me.phOfferTargets.Controls
                        If Not iotp.saveData Then b = False
                    Next
                    Return b
                Else
                    Return False
                End If
            Else
                Return False
			End If
		End Function

		Protected Sub rblSetupMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSetupMode.SelectedIndexChanged
			If rblSetupMode.SelectedValue = "A" Then
				For Each iotp As DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt In Me.phOfferTargets.Controls
					iotp.loadData(True)
				Next
			Else
				Me.loadDataB()
			End If

            Me.chkSelected.Checked = Me.chkSelected.Checked OrElse Not (m_OfferGroup.AllowDeSelect)
        End Sub

		Protected Sub ddlSimpleOffer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSimpleOffer.SelectedIndexChanged
			If Not m_Loading Then m_OfferChanged = True
		End Sub
	End Class
End Namespace
