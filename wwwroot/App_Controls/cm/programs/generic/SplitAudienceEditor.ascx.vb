Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class SplitAudienceEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor

		Protected m_Audience As DealerDirect.CampaignManagement.Programs.Audience
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_DescriptionVisible As Boolean
		Protected m_TotalsLoaded As Boolean = False
		Protected m_Last As Boolean

		Protected m_DMailGrossQuantity As Integer?
		Protected m_DMailGrossPrice As Double?
		Protected m_DMailNetQuantity As Integer?
		Protected m_DMailNetPrice As Double?

		Protected m_EMailGrossQuantity As Integer?
		Protected m_EMailGrossPrice As Double?
		Protected m_EMailNetQuantity As Integer?
		Protected m_EMailNetPrice As Double?

		Protected m_LoadedFilters As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)

		Public Event onValidateData(ByRef sender As Object, ByRef valid As Boolean) Implements IGenericSplitAudienceEditor.onValidateData

        Public ReadOnly Property hasEditableFilters() As Boolean
            Get
                Return m_LoadedFilters.Count > 0
            End Get
        End Property

        Public ReadOnly Property isDescVisible() As Boolean
			Get
				Return m_DescriptionVisible
			End Get
		End Property

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements IGenericSplitAudienceEditor.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property
		Public Property Audience() As DealerDirect.CampaignManagement.Programs.Audience Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.Audience
			Get
				Return m_Audience
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Audience)
				m_Audience = value
			End Set
		End Property

		Public ReadOnly Property AudienceSelected() As Boolean
			Get
				Return Me.chkSelected.Checked
			End Get
		End Property

		Public Property isLastAudience() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.isLastAudience
			Get
				Return m_Last
			End Get
			Set(ByVal value As Boolean)
				m_Last = value
			End Set
		End Property

		Public Function isSelected() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.isSelected
			Return Me.chkSelected.Checked
		End Function

		Public ReadOnly Property RowStyleString() As String
			Get
				Dim s As String = ""
				If Not Me.AudienceSelected Then s &= "color:#C1C1C1;"

				Return s
			End Get
		End Property

		Public ReadOnly Property DMailGrossQuantity() As Integer?
			Get
				loadTotals()
				Return m_DMailGrossQuantity
			End Get
		End Property
		Public ReadOnly Property EMailGrossQuantity() As Integer?
			Get
				loadTotals()
				Return m_EMailGrossQuantity
			End Get
		End Property

		Public ReadOnly Property DMailGrossPrice() As Double?
			Get
				loadTotals()
				Return m_DMailGrossPrice
			End Get
		End Property
		Public ReadOnly Property EMailGrossPrice() As Integer?
			Get
				loadTotals()
				Return m_EMailGrossPrice
			End Get
		End Property

		Public ReadOnly Property DMailNetQuantity() As Integer? Implements IGenericSplitAudienceEditor.DMailTotalQuantity
			Get
				Dim n As Integer

				loadTotals()
				If m_DMailGrossQuantity.HasValue AndAlso Integer.TryParse(Me.txtDMaiLimit.Text, n) Then
					Return System.Math.Min(m_DMailGrossQuantity.Value, n)
				Else
					Return m_DMailGrossQuantity
				End If
			End Get
		End Property
		Public ReadOnly Property EMailNetQuantity() As Integer? Implements IGenericSplitAudienceEditor.EMailTotalQuantity
			Get
				Dim n As Integer

				loadTotals()
				If m_EMailGrossQuantity.HasValue AndAlso Integer.TryParse(Me.txtEMaiLimit.Text, n) Then
					Return System.Math.Min(m_EMailGrossQuantity.Value, n)
				Else
					Return m_EMailGrossQuantity
				End If
			End Get
		End Property

		Public ReadOnly Property DMailNetPrice() As Double? Implements IGenericSplitAudienceEditor.DMailTotalPrice
			Get
				loadTotals()
				Return m_DMailNetPrice
			End Get
		End Property
		Public ReadOnly Property EMailNetPrice() As Double? Implements IGenericSplitAudienceEditor.EMailTotalPrice
			Get
				loadTotals()
				Return m_EMailNetPrice
			End Get
		End Property

		Public Sub loadTotals()
			Dim nTotalDMQuantity As Integer = 0
			Dim nTotalEMQuantity As Integer = 0
			Dim nTotalDMPrice As Double = 0
			Dim nTotalEMPrice As Double = 0

			If Not m_TotalsLoaded Then
				If m_Audience.hasSplits Then
					For Each iase As DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor In Me.phAudienceSplits.Controls
						If iase.isSelected Then
							nTotalDMQuantity += If(iase.AudienceSplit.DMailQuantity, 0)
							nTotalDMPrice += If(iase.AudienceSplit.DMailTotalPrice, 0)

							nTotalEMQuantity += If(iase.AudienceSplit.EMailQuantity, 0)
							nTotalEMPrice += If(iase.AudienceSplit.EMailTotalPrice, 0)
						End If
					Next

					m_DMailGrossQuantity = nTotalDMQuantity
					m_DMailGrossPrice = nTotalDMPrice
					m_EMailGrossQuantity = nTotalEMQuantity
					m_EMailGrossPrice = nTotalEMPrice

				Else
					m_DMailGrossQuantity = m_Audience.DMailQuantity
					m_DMailGrossPrice = m_Audience.DMailTotalPrice
					m_EMailGrossQuantity = m_Audience.EMailQuantity
					m_EMailGrossPrice = m_Audience.EMailTotalPrice
				End If

				Dim nLimit As Integer
				Dim nAveragePrice As Double

				If m_DMailGrossQuantity > 0 AndAlso Integer.TryParse(Me.txtDMaiLimit.Text, nLimit) AndAlso nLimit < m_DMailGrossQuantity Then
					nAveragePrice = m_DMailGrossPrice / m_DMailGrossQuantity
					m_DMailNetQuantity = nLimit
					m_DMailNetPrice = nLimit * nAveragePrice
				Else
					m_DMailNetQuantity = m_DMailGrossQuantity
					m_DMailNetPrice = m_DMailGrossPrice
				End If

				If m_EMailGrossQuantity > 0 AndAlso Integer.TryParse(Me.txtEMaiLimit.Text, nLimit) AndAlso nLimit < m_EMailGrossQuantity Then
					nAveragePrice = m_EMailGrossPrice / m_EMailGrossQuantity
					m_EMailNetQuantity = nLimit
					m_EMailNetPrice = nLimit * nAveragePrice
				Else
					m_EMailNetQuantity = m_EMailGrossQuantity
					m_EMailNetPrice = m_EMailGrossPrice
				End If

				m_TotalsLoaded = True
			End If
		End Sub

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Me.loadControls()
			m_DescriptionVisible = Me.ViewState("desc_visible")

            'Me.chkSelected.Visible = Me.Audience.ShowSelect
            'Me.chkSelected.Enabled = Me.Audience.AllowDeSelect
            Me.chkSelected.Visible = Me.Audience.AllowDeSelect
            Me.chkSelected.Checked = Me.Audience.Selected
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Me.ViewState("desc_visible") = m_DescriptionVisible
			Me.pnlAudience.Enabled = Me.chkSelected.Checked

			For Each iase As DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor In Me.phAudienceSplits.Controls
				iase.AudienceSelected = Me.chkSelected.Checked
			Next
		End Sub

		Public Sub updateCurrentFilterData(ByRef program As DealerDirect.CampaignManagement.Programs.AudienceProgram) Implements IGenericSplitAudienceEditor.updateCurrentFilterData
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt

			m_Audience.CurrentFilterData.Clear()
            For Each ctrl As System.Web.UI.Control In m_LoadedFilters
                If TypeOf ctrl Is DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt Then
                    ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
                    program.Audiences(m_Audience.ID).CurrentFilterData.Add(ifp.getCurrentData)
                End If
            Next
        End Sub

		Protected Sub lbtnToggleDesc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnToggleDesc.Click
			m_DescriptionVisible = Not m_DescriptionVisible
		End Sub

		Protected Sub chkSelected_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelected.CheckedChanged
		End Sub

		Public Sub loadControls()
			Dim iase As DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor
			Dim ctrl As System.Web.UI.Control

			For Each s As DealerDirect.CampaignManagement.Programs.AudienceSplit In m_Audience.Splits.OrderBy(Function(f) f.DisplayOrder)
				ctrl = Page.LoadControl("~/app_controls/cm/programs/Generic/AudienceSplitEditor.ascx")
				ctrl.ID = "as" & s.ID
				iase = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor)
				iase.AudienceSplit = s
				iase.PriceVisible = (m_Program.PricingDisplayMode <> DealerDirect.CampaignManagement.Programs.PricingDisplayMode.None)

				phAudienceSplits.Controls.Add(ctrl)
			Next
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.loadData
			Dim fcl As System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.Filters.FilterConfig)
			Dim fcc As New DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt
			Dim tc As System.Web.UI.HtmlControls.HtmlTableCell
			Dim tr As System.Web.UI.HtmlControls.HtmlTableRow
			Dim ctrl As System.Web.UI.Control

			If initial_load Then
				If m_Audience.DMailLimit.HasValue Then Me.txtDMaiLimit.Text = m_Audience.DMailLimit.Value
				If m_Audience.EMailLimit.HasValue Then Me.txtEMaiLimit.Text = m_Audience.EMailLimit.Value
			End If

			If initial_load Then
				m_LoadedFilters.Clear()
				Me.phFilters.Controls.Clear()
			End If

            fcl = m_Audience.FilterConfigs.Where(Function(f) f.ShowFilter = True).ToList
            For Each fc As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig In fcl
				tr = New System.Web.UI.HtmlControls.HtmlTableRow
                'tr.Attributes.Add("style", "border-bottom-width:1px;border-bottom-style:solid; border-right-width:1px;border-right-style:solid;")

                tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.Attributes.Add("width", "130px")
                'tc.Attributes.Add("style", "font-weight:bold;text-align:right;vertical-align:middle;white-space:nowrap; border-bottom-width:1px;border-bottom-style:solid;")
                tc.Attributes.Add("style", "text-align:right;vertical-align:middle;white-space:nowrap;")
                If m_Audience.SetupFilterSet.Filters.ContainsKey(fc.Code) Then
					tc.InnerHtml = DealerDirect.Resources.getStringColon(m_Audience.SetupFilterSet.Filters(fc.Code).Filter.NameResourceCode)
				Else
					tc.InnerHtml = fc.Code & ":"
				End If
				tr.Controls.Add(tc)

				ctrl = Page.LoadControl(If(fc.EditControl, "~/app_controls/cm/programs/generic/FilterPrompt.ascx"))
				ctrl.ID = String.Format("a{0}fp", m_Audience.ID) & fc.Code
				ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
				ifp.ContainerFilter = m_Audience.SetupFilterSet.Filters(fc.Code)
				ifp.FilterConfig = fc
				ifp.loadData(initial_load)
				m_LoadedFilters.Add(ifp)

				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.Attributes.Add("width", "279px")
                'tc.Attributes.Add("style", "text-align:left;vertical-align:middle; border-bottom-width:1px;border-bottom-style:solid;")
                tc.Attributes.Add("style", "text-align:left;vertical-align:middle;")
                tc.Controls.Add(ctrl)
				tr.Controls.Add(tc)

				Me.phFilters.Controls.Add(tr)
			Next

			Me.pnlFilters.Visible = (m_LoadedFilters.Count > 0)
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.validateData
			Dim iase As DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor
			Dim bAnySelected As Boolean = False
			Dim bValid As Boolean = True

			If m_Audience.hasSplits Then
				For Each ctrl As System.Web.UI.Control In Me.phAudienceSplits.Controls
					If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor Then
						iase = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor)
						If Not iase.validateData Then bValid = False
						If iase.isSelected Then bAnySelected = True
					End If
				Next

				Me.lblError.Visible = Not bAnySelected
				If Not bAnySelected Then
					Me.lblError.Text = DealerDirect.Resources.getString("LITERAL_Please_select_one_or_more_options")
				End If

			Else
				bAnySelected = True
			End If

			If bValid AndAlso bAnySelected Then
				Dim bCancel As Boolean = False
				RaiseEvent onValidateData(Me, bCancel)
				Return Not bCancel
			Else
				Return False
			End If
		End Function

		Public Function saveDataToObject() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor.saveDataToObject			
			Dim n As Integer

			Dim iase As DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor
			For Each ctrl As System.Web.UI.Control In Me.phAudienceSplits.Controls
				If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor Then
					iase = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor)
					iase.saveDataToObject()
				End If
			Next

			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt
			For Each ctrl As System.Web.UI.Control In m_LoadedFilters
				If TypeOf ctrl Is DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt Then
					ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
					ifp.saveData()
				End If
			Next

			Me.Audience.Selected = Me.chkSelected.Checked

			If Integer.TryParse(Me.txtEMaiLimit.Text, n) Then
				Me.Audience.EMailLimit = n
			Else
				Me.Audience.EMailLimit = Nothing
			End If

			If Integer.TryParse(Me.txtDMaiLimit.Text, n) Then
				Me.Audience.DMailLimit = n
			Else
				Me.Audience.DMailLimit = Nothing
			End If

			Return True
		End Function

		Protected Sub txtDMaiLimit_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDMaiLimit.ValueChanged
			Dim n As Integer
			If Not Integer.TryParse(Me.txtDMaiLimit.Text, n) Then
				Me.txtDMaiLimit.Text = Nothing
			ElseIf n = 0 Then
				Me.txtDMaiLimit.Text = Nothing
			End If
		End Sub
		Protected Sub txtEMaiLimit_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEMaiLimit.ValueChanged
			Dim n As Integer
			If Not Integer.TryParse(Me.txtEMaiLimit.Text, n) Then
				Me.txtEMaiLimit.Text = Nothing
			ElseIf n = 0 Then
				Me.txtEMaiLimit.Text = Nothing
			End If
		End Sub
	End Class
End Namespace