Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class AudienceSplitEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor

		Protected m_AudienceSplit As DealerDirect.CampaignManagement.Programs.AudienceSplit
		Protected m_DescriptionVisible As Boolean
		Protected m_AudienceSelected As Boolean
		Protected m_PriceVisible As Boolean

		Public ReadOnly Property showDescToggle() As Boolean
			Get
                Return m_AudienceSelected AndAlso Me.AudienceSplit.hasDescription
                Return Me.AudienceSplit.hasDescription
            End Get
		End Property

		Public Property PriceVisible() As Boolean Implements IGenericAudienceSplitEditor.PriceVisible
			Get
				Return m_PriceVisible
			End Get
			Set(value As Boolean)
				m_PriceVisible = value
			End Set
		End Property

		Public ReadOnly Property isDescVisible() As Boolean
			Get
				Return m_DescriptionVisible
			End Get
		End Property

		Public ReadOnly Property RowStyleString() As String
			Get
				Dim s As String = ""
				If Not m_AudienceSelected Then s &= "color:#C1C1C1;"

				Return s
			End Get
		End Property

		Public ReadOnly Property QuantityRowStyleString() As String
			Get
				Dim s As String = ""
				If Not Me.chkSelected.Checked Then s &= "color:#C1C1C1;"

				Return s
			End Get
		End Property

		Public Property AudienceSplit() As DealerDirect.CampaignManagement.Programs.AudienceSplit Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor.AudienceSplit
			Get
				Return m_AudienceSplit
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.AudienceSplit)
				m_AudienceSplit = value
			End Set
		End Property

		Public Property AudienceSelected() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor.AudienceSelected
			Get
				Return m_AudienceSelected
			End Get
			Set(ByVal value As Boolean)
				m_AudienceSelected = value
			End Set
		End Property

		Public Function isSelected() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor.isSelected
			Return Me.chkSelected.Checked
		End Function

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			m_DescriptionVisible = Me.ViewState("desc_visible")
            'Me.chkSelected.Enabled = Me.AudienceSplit.AllowDeSelect
            Me.chkSelected.Visible = Me.AudienceSplit.AllowDeSelect
            Me.chkSelected.Checked = Me.AudienceSplit.Selected
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Me.ViewState("desc_visible") = m_DescriptionVisible
		End Sub

		Protected Sub lbtnToggleDesc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnToggleDesc.Click
			m_DescriptionVisible = Not m_DescriptionVisible
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor.validateData
			Return True
		End Function

		Public Function saveDataToObject() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IGenericAudienceSplitEditor.saveDataToObject
			Me.AudienceSplit.Selected = Me.chkSelected.Checked
			Return True
		End Function
	End Class
End Namespace