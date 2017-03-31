Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class VariableSetsEditorV3
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor

		Protected m_ComponentIndex As Integer
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_VariableSetEditors As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
		Protected m_VariableSetTapPages As New System.Collections.Generic.Dictionary(Of Integer, Object)
		'Protected m_StyleGroupCode As String

		'Public Property StyleGroupCode As String
		'	Get
		'		Return m_StyleGroupCode
		'	End Get
		'	Set(value As String)
		'		m_StyleGroupCode = value
		'	End Set
		'End Property


		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.loadData
			Dim vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor
			Dim vsc As DealerDirect.CampaignManagement.Programs.VDP.VariableSetCollection
			Dim vs As DealerDirect.CampaignManagement.Programs.VDP.VariableSet
			Dim rt As Telerik.Web.UI.RadTab
			Dim rpv As Telerik.Web.UI.RadPageView
			Dim ctrl As System.Web.UI.Control
			Dim bHasOptions As Boolean = False

			vsc = m_Program.VariableSets
			m_VariableSetEditors.Clear()
			m_VariableSetTapPages.Clear()
			Me.phVariablePrompts.Controls.Clear()

			'Me.rmpCreative.PageViews.Clear()
			'Me.rtsCreative.Tabs.Clear()

			Me.rmpLocations.PageViews.Clear()
			Me.rtsLocations.Tabs.Clear()

			' main (global settings)
			vs = vsc.Where(Function(f) f.SourceCode = "PROG").OrderBy(Function(f) f.ID).FirstOrDefault
			If vs IsNot Nothing Then
				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				'Me.Program.StyleGroupCode = m_StyleGroupCode
				vse.Program = Me.Program
				vse.VariableSet = vs
				vse.loadData(initial_load)
				m_VariableSetEditors.Add(vse)

				ctrl.ID = "vse" & vse.VariableSet.ID
				Me.phVariablePrompts.Controls.Add(ctrl)
			End If

			' lang split
			For Each vs In vsc.Where(Function(f) f.SourceCode = "TRTMT").OrderBy(Function(f) f.ID)
				'rpv = Me.rmpCreative.FindPageViewByID("pv" & vs.ID)
				'If rpv Is Nothing Then
				'	rpv = New Telerik.Web.UI.RadPageView
				'	rpv.ID = "pv" & vs.ID
				'	rmpCreative.PageViews.Add(rpv)
				'End If

				'If vs.LanguageCode 
				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				vse.Program = Me.Program
				vse.VariableSet = vs
				ctrl.ID = "vse" & vse.VariableSet.ID
				vse.loadData(initial_load)

				'rpv.Controls.Add(ctrl)
				m_VariableSetEditors.Add(vse)
				If vs.LanguageCode = "EN" Then
					Me.phVariablePromptsEN.Controls.Add(ctrl)
					'rmpCreative.PageViews.Add(rpv)
				ElseIf vs.LanguageCode = "FR" Then
					Me.phVariablePromptsFR.Controls.Add(ctrl)
				End If

				'rt = New Telerik.Web.UI.RadTab(vs.Name)
				''rt.ID = "tp" & vse.VariableSet.ID
				'rt.PageViewID = rpv.ID

				'Me.rtsCreative.Tabs.Add(rt)
				'm_VariableSetTapPages.Add(vse.VariableSet.ID, rt)
			Next

			'Me.rtsCreative.SelectedIndex = 0
			'Me.rtsCreative.MultiPage.SelectedIndex = 0

			'multi location
			For Each vs In vsc.Where(Function(f) f.SourceCode = "LOCATION").OrderBy(Function(f) f.ID)
				rpv = Me.rmpLocations.FindPageViewByID("pvLocation" & vs.ID)
				If rpv Is Nothing Then
					rpv = New Telerik.Web.UI.RadPageView
					rpv.ID = "pvLocation" & vs.ID
					rmpLocations.PageViews.Add(rpv)
				End If

				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				vse.Program = Me.Program
				vse.VariableSet = vs
				ctrl.ID = "vse" & vse.VariableSet.ID
				vse.loadData(initial_load)

				rpv.Controls.Add(ctrl)
				m_VariableSetEditors.Add(vse)
				rmpLocations.PageViews.Add(rpv)

				rt = New Telerik.Web.UI.RadTab(vs.Name)
				'rt.ID = "tpLocation" & vse.VariableSet.ID
				rt.PageViewID = rpv.ID

				Me.rtsLocations.Tabs.Add(rt)
				m_VariableSetTapPages.Add(vse.VariableSet.ID, rt)
			Next

			Me.rtsLocations.SelectedIndex = 0
			Me.rtsLocations.MultiPage.SelectedIndex = 0

			Dim vpc As New System.Collections.Generic.SortedDictionary(Of Integer, DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
			For Each vse In m_VariableSetEditors
				For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In vse.getAllVariablePrompts
					vpc.Add(vp.Variable.ID, vp)
				Next
			Next

			For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In vpc.Values
				If vp.Variable.ParentVariableID.HasValue Then
					vp.ParentVariablePrompt = vpc(vp.Variable.ParentVariableID)
					vpc(vp.Variable.ParentVariableID).ChildVariablePrompts.Add(vp)
				End If
			Next
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.validateData
			Dim rtp As Telerik.Web.UI.RadTab
			Dim b As Boolean = True

			For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
				If Not vse.validateData() Then
					b = False

					'If m_VariableSetTapPages.TryGetValue(vse.VariableSet.ID, rtp) Then
					'	'TODO: restore
					'	Me.rmpCreative.SelectedIndex = rtp.Index
					'	'Me.axpcCreative.ActiveTabPage = tp
					'End If
				End If
			Next

			'Me.lblErrorMessage.Visible = Not b

			Return b
		End Function

		Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.saveData
			Dim bSuccess As Boolean = True

			For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
				If Not vse.saveData Then bSuccess = False
			Next

			Return bSuccess
		End Function

		Public Function saveDataPreviewData() As Boolean
			Dim bSuccess As Boolean = True

			For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
				If Not vse.savePreviewData Then bSuccess = False
			Next

			Return bSuccess
		End Function

		'Protected Sub pgMain_PrepareVariablesForPreview(ByVal sender As Object, ByRef success As Boolean) Handles pgMain.PrepareVariablesForPreview
		'	success = (Me.validateData AndAlso Me.saveDataPreviewData)
		'End Sub
	End Class
End Namespace