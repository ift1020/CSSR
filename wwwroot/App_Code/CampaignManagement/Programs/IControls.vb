Namespace DealerDirect.UserControls.CampaignManagement.Programs
	Public Interface ISettingsComponent
		Property ComponentIndex() As Integer
		ReadOnly Property isApproved() As Boolean
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
		Function saveData() As Boolean
		Function validateData() As Boolean
		Property HelpLink() As String
		Property InlineHelp() As String
		Sub loadData(ByVal initial_load As Boolean)

		Sub setErrorMessage(ByVal error_message As String)
		Sub setErrorMessage(ByVal error_code As String, ByVal error_message As String)

		Event ApprovalStateChanged(ByVal sender As ISettingsComponent, ByVal new_approval_state As Boolean)
	End Interface

	Public Interface IExecutionSummary
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
	End Interface

	Public Interface IGenericControl
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
	End Interface

	Public Interface IBuildStatusItem
		Property BuildStatusItem() As DealerDirect.CampaignManagement.Programs.CampaignBuilder.BuildStatusItem
	End Interface

	Public Interface ISettingsEditor
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
		Function saveData() As Boolean
		Function validateData() As Boolean
		Sub loadData(ByVal initial_load As Boolean)
	End Interface

	Public Interface IFilterPrompt
		Inherits ISettingsEditor
		Property ContainerFilter() As DealerDirect.CampaignManagement.DataFilters.ContainerFilter
		Property FilterConfig() As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig
		Function getCurrentData() As DealerDirect.CampaignManagement.DataFilters.ContainerFilter
	End Interface

	Public Interface IGenericWaveEditor
		Property Wave() As DealerDirect.CampaignManagement.Programs.Wave
		Function saveDataToObject() As Boolean
	End Interface

	Public Interface IGenericSplitAudienceList
		Property Program() As DealerDirect.CampaignManagement.Programs.AudienceProgram
		'Property Audience() As DealerDirect.CampaignManagement.Programs.Audience
	End Interface

	Public Interface IGenericSplitAudienceEditor
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
		Property Audience() As DealerDirect.CampaignManagement.Programs.Audience
		Property isLastAudience() As Boolean
		Function validateData() As Boolean
		Function saveDataToObject() As Boolean
		Sub loadData(ByVal initial_load As Boolean)
		Sub updateCurrentFilterData(ByRef program As DealerDirect.CampaignManagement.Programs.AudienceProgram)
		Function isSelected() As Boolean
		ReadOnly Property DMailTotalQuantity() As Integer?
		ReadOnly Property DMailTotalPrice() As Double?
		ReadOnly Property EMailTotalQuantity() As Integer?
		ReadOnly Property EMailTotalPrice() As Double?
		Event onValidateData(ByRef sender As Object, ByRef valid As Boolean)
	End Interface

	Public Interface IGenericAudienceSplitEditor
		Property AudienceSplit() As DealerDirect.CampaignManagement.Programs.AudienceSplit
		Function validateData() As Boolean
		Function saveDataToObject() As Boolean
		Property AudienceSelected() As Boolean
		Function isSelected() As Boolean
		Property PriceVisible As Boolean
	End Interface

	Public Interface IVariablePrompt
		Property VariableSetID() As Integer
		Property VariableID() As Integer
		Property Variable() As DealerDirect.CampaignManagement.Programs.VDP.Variable
		Function saveData(Optional ByVal temporary As Boolean = False) As Boolean
		Function validateData() As Boolean
		Property Enabled As Boolean
        Property Visible As Boolean
        Property ParentVariablePrompt As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt
		Property ChildVariablePrompts As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePromptDictionary
		Sub loadData(ByVal initial_load As Boolean)
	End Interface

	Public Class IVariablePromptDictionary
		Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, IVariablePrompt)

		Protected Overrides Function GetKeyForItem(item As IVariablePrompt) As Integer
			Return item.Variable.ID
		End Function
	End Class


	Public Interface IVariableSetEditor
		Inherits ISettingsEditor
		Property VariableSet() As DealerDirect.CampaignManagement.Programs.VDP.VariableSet
		Function savePreviewData() As Boolean
		Property Enabled As Boolean
		Function getAllVariablePrompts() As System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
	End Interface

	Public Interface IStyleOptionPrompt
		Inherits ISettingsEditor
		Property StyleOption() As DealerDirect.CampaignManagement.Programs.Styles.StyleOption
		Function savePreviewData() As Boolean
	End Interface

	Public Interface IOfferGroupEditor
		Inherits ISettingsEditor
		Property OfferGroup() As DealerDirect.CampaignManagement.Programs.Offers.OfferGroup
	End Interface

	Public Interface IOfferTargetPrompt
		Inherits ISettingsEditor
		Property OfferTarget() As DealerDirect.CampaignManagement.Programs.Offers.OfferTarget
		Property AvailableOffers() As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOfferCollection
	End Interface

	Public Interface IPreviewGenerator
		Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
		Property StyleGroupCode() As String
		Sub loadData(ByVal initial_load As Boolean)
		Event PrepareVariablesForPreview(ByVal sender As Object, ByRef success As Boolean)
	End Interface
End Namespace
