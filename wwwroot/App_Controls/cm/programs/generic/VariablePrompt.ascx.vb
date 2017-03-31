Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class VariablePrompt
		Inherits System.Web.UI.UserControl
        Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt


        Protected m_Variable As DealerDirect.CampaignManagement.Programs.VDP.Variable
		Protected m_VariableSetID As Integer
		Protected m_VariableID As Integer

		Protected m_Enabled As Boolean
		Protected m_Visible As Boolean
		Protected m_ParentVariablePrompt As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt
		Protected m_ChildVariablePrompts As New DealerDirect.UserControls.CampaignManagement.Programs.IVariablePromptDictionary

		Public Property ParentVariablePrompt As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt.ParentVariablePrompt
			Get
				Return m_ParentVariablePrompt
			End Get
			Set(value As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
				m_ParentVariablePrompt = value
			End Set
		End Property

		Public Property ChildVariablePrompts As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePromptDictionary Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt.ChildVariablePrompts
			Get
				Return m_ChildVariablePrompts
			End Get
			Set(value As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePromptDictionary)
				m_ChildVariablePrompts = value
			End Set
		End Property

		Public Property Enabled As Boolean Implements IVariablePrompt.Enabled
			Get
				Return m_Enabled
			End Get
			Set(value As Boolean)
				m_Enabled = value

				Me.pnlFixedText.Enabled = m_Enabled
				Me.pnlImage.Enabled = m_Enabled
				Me.pnlTheme.Enabled = m_Enabled
			End Set
		End Property

		Public Property Visible() As Boolean Implements IVariablePrompt.Visible
			Get
				Return m_Visible
			End Get
			Set(ByVal value As Boolean)
				m_Visible = value
			End Set
		End Property

		Public Property VariableSetID() As Integer Implements IVariablePrompt.VariableSetID
			Get
				Return m_VariableSetID
			End Get
			Set(ByVal value As Integer)
				m_VariableSetID = value
			End Set
		End Property
		Public Property VariableID() As Integer Implements IVariablePrompt.VariableID
			Get
				Return m_VariableID
			End Get
			Set(ByVal value As Integer)
				m_VariableID = value
			End Set
		End Property
		Public Property Variable() As DealerDirect.CampaignManagement.Programs.VDP.Variable Implements IVariablePrompt.Variable
			Get
				Return m_Variable
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.VDP.Variable)
				m_Variable = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			'If Me.m_Variable Is Nothing Then Me.m_Variable = Me.ViewState("variable")
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			If m_Variable IsNot Nothing Then
				Me.lblVariableName.Text = DealerDirect.Resources.getStringColon(Me.Variable.NameResourceCode)

				Select Case Me.Variable.VariableTypeCode
					Case "IMAGE"
						Me.pnlImage.Visible = True

					Case "MPICK"
						Me.pnlMultiPick.Visible = True
						Me.updateMultiPickText()

					Case "THEME", "OPGRP"
						Me.pnlTheme.Visible = True

					Case "FTEXT", "MONEY"
						Me.pnlFixedText.Visible = True
						If Me.Variable.SuffixResourceCode IsNot Nothing Then Me.lblFixedTextSuffix.Text = DealerDirect.Resources.getString(Me.Variable.SuffixResourceCode)

						If Me.Variable.ControlOptions IsNot Nothing Then
							Dim sOptionValue As String

							sOptionValue = Me.Variable.getControlOption("maxlength")
							If sOptionValue IsNot Nothing Then Me.txtFixedText.MaxLength = sOptionValue

							sOptionValue = Me.Variable.getControlOption("width")
							If sOptionValue IsNot Nothing Then Me.txtFixedText.Width = sOptionValue

							sOptionValue = Me.Variable.getControlOption("rows")
							If sOptionValue IsNot Nothing Then
								Me.txtFixedText.Rows = sOptionValue
								If Me.txtFixedText.Rows > 1 Then Me.txtFixedText.TextMode = TextBoxMode.MultiLine
							End If
						End If
				End Select

				Me.ViewState("variable") = Me.m_Variable
			End If
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements IVariablePrompt.loadData		
			Dim sOptionValue As String

			Select Case Me.Variable.VariableTypeCode
				Case "MPICK"
					Me.rcpMultiPick.Items.Clear()
					For Each sa As DealerDirect.CampaignManagement.Programs.VDP.VariableOption In m_Variable.getVariableOptions.Where(Function(f) f.OptionCode = "OPTION_VALUE").ToList
						Me.rcpMultiPick.Items.Add(New Telerik.Web.UI.RadComboBoxItem(sa.Name, sa.Value))
					Next

					If initial_load Then
						If Me.Variable.TextValue IsNot Nothing Then
							For Each cbi As Telerik.Web.UI.RadComboBoxItem In Me.rcpMultiPick.Items
								cbi.Checked = Me.Variable.TextValue.Contains(cbi.Value & ";")
							Next
						End If
					End If

					Me.updateMultiPickText()

				Case "IMAGE"
						Me.ddlImage.Items.Clear()
						Utility.WebControls.AddEmptyValue(Me.ddlImage, 0, String.Format("({0})", DealerDirect.Resources.getString("LITERAL_None")))
						For Each sa As DealerDirect.CampaignManagement.Programs.VDP.StaticAsset In DealerDirect.CampaignManagement.Programs.VDP.getStaticAssets(m_Variable.ProgramID, m_Variable.SubCampaignID, Me.m_Variable.VariableSizeCode, False) 'm_Variable.getStaticAssets.Values
							Me.ddlImage.Items.Add(New System.Web.UI.WebControls.ListItem(sa.Name, sa.ID))
						Next

						If initial_load Then
							If Me.Variable.StaticAssetID.HasValue Then Me.ddlImage.SelectedValue = Me.Variable.StaticAssetID
						End If

				Case "THEME"
						Me.ddlTheme.Items.Clear()

						sOptionValue = Me.Variable.getControlOption("empty-name")
						If sOptionValue IsNot Nothing Then
							Utility.WebControls.AddEmptyValue(Me.ddlTheme, 0, String.Format("{0}", DealerDirect.Resources.getString(sOptionValue)))
						Else
							Utility.WebControls.AddEmptyValue(Me.ddlTheme, 0, String.Format("({0})", DealerDirect.Resources.getString("LITERAL_None")))
						End If

						For Each sa As DealerDirect.CampaignManagement.Programs.VDP.ThemeOption In m_Variable.getThemeOptions
							Me.ddlTheme.Items.Add(New System.Web.UI.WebControls.ListItem(sa.Name, sa.Value))
						Next

						If initial_load Then
							If Me.Variable.NumberValue.HasValue Then Me.ddlTheme.SelectedValue = Me.Variable.NumberValue
						End If

				Case "OPGRP"
						Me.ddlTheme.Items.Clear()
						For Each sa As DealerDirect.CampaignManagement.Programs.VDP.VariableOption In m_Variable.getVariableOptions.Where(Function(f) f.OptionCode = "OPTION_GROUP").ToList
							Me.ddlTheme.Items.Add(New System.Web.UI.WebControls.ListItem(sa.Name, sa.Value))
						Next

						If initial_load Then
							If Me.Variable.TextValue IsNot Nothing Then Me.ddlTheme.SelectedValue = Me.Variable.TextValue
						End If

				Case "FTEXT"
						If initial_load Then Me.txtFixedText.Text = Me.Variable.TextValue

				Case "MONEY"
						If initial_load AndAlso Me.Variable.NumberValue.HasValue Then Me.txtFixedText.Text = Me.Variable.NumberValue
			End Select
		End Sub

		Public Function validateData() As Boolean Implements IVariablePrompt.validateData			
			Me.cvMain.Validate()
			Return Me.cvMain.IsValid
		End Function

		Public Function saveData(Optional ByVal temporary As Boolean = False) As Boolean Implements IVariablePrompt.saveData
			Select Case Me.Variable.VariableTypeCode
				Case "MPICK"
					Dim sValue As String = ""
					For Each cbi As Telerik.Web.UI.RadComboBoxItem In Me.rcpMultiPick.Items
						If cbi.Checked Then sValue &= cbi.Value & ";"
					Next

					Me.Variable.TextValue = sValue

				Case "FTEXT"
					Me.Variable.TextValue = Me.txtFixedText.Text

				Case "MONEY"
					Dim cv As DealerDirect.VDP.DataConverters.ConversionResult

					cv = DealerDirect.VDP.DataConverters.Convert("MONEY", Me.txtFixedText.Text)
					If cv.Success Then
						Me.Variable.TextValue = cv.InvariantData
						Me.Variable.NumberValue = CDbl(cv.LocalizedData)
					Else
						Return False
					End If

				Case "THEME"
					If Me.ddlTheme.SelectedValue > 0 Then
						Me.Variable.NumberValue = Me.ddlTheme.SelectedValue
					Else
						Me.Variable.NumberValue = Nothing
					End If

				Case "OPGRP"
					Me.Variable.TextValue = Me.ddlTheme.SelectedValue

				Case "IMAGE"
					If Me.ddlImage.SelectedValue > 0 Then
						Me.Variable.StaticAssetID = Me.ddlImage.SelectedValue
					Else
						Me.Variable.StaticAssetID = Nothing
					End If
			End Select

			Return Me.Variable.Save(temporary)
		End Function

		Protected Sub cvMain_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMain.ServerValidate
			Dim bValid As Boolean = True
			Dim sOptionValue As String

			If Me.Variable.getControlOption("required") = "y" Then
				Select Case Me.Variable.VariableTypeCode
					Case "FTEXT"
						bValid = Me.txtFixedText.Text.Trim.Length > 0
					Case "IMAGE"
						bValid = Me.ddlImage.SelectedValue > 0
					Case "THEME"
						bValid = Me.ddlTheme.SelectedValue > 0
					Case "MONEY"
						bValid = Me.txtFixedText.Text.Trim.Length > 0
					Case "MPICK"
						bValid = Me.rcpMultiPick.CheckedItems.Count > 0
				End Select

				If Not bValid Then
					Me.cvMain.ErrorMessage = DealerDirect.Resources.getString("LITERAL_Required")
					args.IsValid = False
					Exit Sub
				End If
			End If

			sOptionValue = Me.Variable.getControlOption("datatype")
			If sOptionValue IsNot Nothing AndAlso Me.Variable.VariableTypeCode = "FTEXT" Then
				Select Case sOptionValue.ToLower
					Case "nbr-natural"
						Dim n As Integer

						If Not (Int32.TryParse(Me.txtFixedText.Text, n) AndAlso n > 0 AndAlso CDbl(n) = CDbl(Me.txtFixedText.Text)) Then
							Me.cvMain.ErrorMessage = DealerDirect.Resources.getString("LITERAL_Non-Zero_Integer_Required")
							args.IsValid = False
							Exit Sub
						End If
				End Select
			End If

			If Me.Variable.VariableTypeCode = "MPICK" Then
				Dim n As Integer

				sOptionValue = Me.Variable.getControlOption("min-items")
				If sOptionValue IsNot Nothing AndAlso Integer.TryParse(sOptionValue, n) Then
					If Me.rcpMultiPick.CheckedItems.Count < n Then
						Me.cvMain.ErrorMessage = String.Format(DealerDirect.Resources.getString("VDP_MPICK_TOO_FEW_ITEMS"), n)
						args.IsValid = False
						Exit Sub
					End If
				End If

				sOptionValue = Me.Variable.getControlOption("max-items")
				If sOptionValue IsNot Nothing AndAlso Integer.TryParse(sOptionValue, n) Then
					If Me.rcpMultiPick.CheckedItems.Count > n Then
						Me.cvMain.ErrorMessage = String.Format(DealerDirect.Resources.getString("VDP_MPICK_TOO_MANY_ITEMS"), n)
						args.IsValid = False
						Exit Sub
					End If
				End If
			End If

			If Me.Variable.VariableTypeCode = "MONEY" Then
				If Me.txtFixedText.Text = "" OrElse Me.txtFixedText.Text = Nothing Then
					Me.cvMain.ErrorMessage = Nothing
				ElseIf DealerDirect.VDP.DataConverters.Convert("MONEY", Me.txtFixedText.Text).Success Then
					Me.cvMain.ErrorMessage = Nothing
				Else
					Me.cvMain.ErrorMessage = DealerDirect.Resources.getString("VDP_MP_ERROR_INVALID_FORMAT")
					args.IsValid = False
					Exit Sub
				End If
			End If

			Dim b As Boolean = True

			If Me.ChildVariablePrompts IsNot Nothing AndAlso Me.ChildVariablePrompts.Count > 0 Then
				For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In Me.ChildVariablePrompts
					If Me.Variable.ValidationGroupCode = "CUSTOM_CTA" Then
						If Me.ddlTheme.SelectedIndex > 0 AndAlso Not vp.validateData() Then
							b = False
						End If

					ElseIf Me.Variable.ValidationGroupCode = "MULTI_LOCATION" Then
						Dim ctrl As System.Web.UI.Control
						Dim nValue As Integer = Me.ddlTheme.SelectedValue

						ctrl = FindControlRecursive(Me.Page, "axpcLocations")
						If ctrl IsNot Nothing Then
							Dim axpc As DevExpress.Web.ASPxTabControl.ASPxPageControl
							axpc = ctrl

							For i As Integer = 0 To nValue - 2
								For Each ctrl2 As System.Web.UI.Control In axpc.TabPages(i).Controls
									If TypeOf (ctrl2) Is DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor Then
										Dim vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor
										vse = DirectCast(ctrl2, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)

										For Each vp2 As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In vse.getAllVariablePrompts
											If Not vp2.validateData Then
												b = False
											End If
										Next
									End If
								Next
							Next
						End If

					ElseIf Not vp.validateData() Then
						b = False
					End If
				Next
			End If

			Me.cvMain.ErrorMessage = Nothing
			args.IsValid = b
		End Sub

		Protected Sub ddlTheme_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTheme.SelectedIndexChanged
			Select Case Me.Variable.VariableTypeCode
				Case "THEME", "OPGRP"
					If Me.Variable.ValidationGroupCode = "CUSTOM_CTA" Then
						For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In Me.ChildVariablePrompts
							vp.Enabled = (Me.ddlTheme.SelectedIndex > 0)
						Next

					ElseIf Me.Variable.ValidationGroupCode = "MULTI_LOCATION" Then
						Dim ctrl As System.Web.UI.Control
						Dim nValue As Integer = Me.ddlTheme.SelectedValue

						ctrl = FindControlRecursive(Me.Page, "axpcLocations")
						If ctrl IsNot Nothing Then
							Dim axpc As DevExpress.Web.ASPxTabControl.ASPxPageControl
							axpc = ctrl

							For i As Integer = 0 To axpc.TabPages.Count - 1
								For Each ctrl2 As System.Web.UI.Control In axpc.TabPages(i).Controls
									If TypeOf (ctrl2) Is DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor Then
										Dim vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor
										vse = DirectCast(ctrl2, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
										vse.Enabled = (i + 2 <= nValue)
									End If
								Next
							Next
						End If
					End If
			End Select
		End Sub

		Private Function FindControlRecursive(
			ByVal rootControl As Control, ByVal controlID As String) As Control

			If rootControl.ID = controlID Then
				Return rootControl
			End If

			For Each controlToSearch As Control In rootControl.Controls
				Dim controlToReturn As Control =
						FindControlRecursive(controlToSearch, controlID)
				If controlToReturn IsNot Nothing Then
					Return controlToReturn
				End If
			Next
			Return Nothing
		End Function

		Protected Sub rcpMultiPick_TextChanged(sender As Object, e As System.EventArgs) Handles rcpMultiPick.TextChanged
			Me.updateMultiPickText()
		End Sub

		Private Sub updateMultiPickText()
			Dim sValue As String = ""
			For Each cbi As Telerik.Web.UI.RadComboBoxItem In Me.rcpMultiPick.Items
				If cbi.Checked Then
					If sValue.Length > 0 Then sValue &= "; "
					sValue &= cbi.Text
				End If
			Next

			Me.lblCheckedList.Text = "<br/>" & sValue
		End Sub
	End Class
End Namespace
