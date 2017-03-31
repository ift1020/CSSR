Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class StyleOptionPrompt
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IStyleOptionPrompt

		Protected m_StyleOption As DealerDirect.CampaignManagement.Programs.Styles.StyleOption

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements ISettingsEditor.Program
			Get
				Return Nothing
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
			End Set
		End Property

		Public Property StyleOption() As DealerDirect.CampaignManagement.Programs.Styles.StyleOption Implements IStyleOptionPrompt.StyleOption
			Get
				Return m_StyleOption
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Styles.StyleOption)
				m_StyleOption = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements ISettingsEditor.loadData
			'If initial_load Then
			Me.ddlStyles.DataSource = m_StyleOption.Styles.ToList
			Me.ddlStyles.DataTextField = "Name"
			Me.ddlStyles.DataValueField = "Code"
			Me.ddlStyles.DataBind()
			'End If

			If initial_load Then Me.ddlStyles.SelectedValue = m_StyleOption.StyleCode
		End Sub

		Public Function saveData() As Boolean Implements ISettingsEditor.saveData
			Return m_StyleOption.setStyle(Me.ddlStyles.SelectedValue)
		End Function

		Public Function savePreviewData() As Boolean Implements IStyleOptionPrompt.savePreviewData
			Return m_StyleOption.setStyle(Me.ddlStyles.SelectedValue, True)
		End Function

		Public Function validateData() As Boolean Implements ISettingsEditor.validateData
		End Function
	End Class
End Namespace
