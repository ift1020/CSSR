Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class VariableSetsEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor

		Protected m_ComponentIndex As Integer
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_VariableSetEditors As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
		Protected m_VariableSetTapPages As New System.Collections.Generic.Dictionary(Of Integer, Object)

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
			Dim tp As DevExpress.Web.ASPxTabControl.TabPage
			Dim ctrl As System.Web.UI.Control
			Dim nIndex As Integer
			Dim bHasOptions As Boolean = False

			vsc = m_Program.VariableSets
			m_VariableSetEditors.Clear()
			m_VariableSetTapPages.Clear()
			Me.phVariablePrompts.Controls.Clear()
			Me.axpcCreative.TabPages.Clear()
Me.axpcLocations.TabPages.Clear()

			' main (global settings)
			vs = vsc.Where(Function(f) f.SourceCode = "PROG").OrderBy(Function(f) f.ID).FirstOrDefault
			If vs IsNot Nothing Then
				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				vse.Program = Me.Program
				vse.VariableSet = vs
				vse.loadData(initial_load)
				m_VariableSetEditors.Add(vse)

				ctrl.ID = "vse" & vse.VariableSet.ID
				Me.phVariablePrompts.Controls.Add(ctrl)
			End If

			' split
			For Each vs In vsc.Where(Function(f) f.SourceCode = "TRTMT").OrderBy(Function(f) f.ID) ' vsc.OrderBy(Function(f) f.ID).Skip(1)
				nIndex = Me.axpcCreative.TabPages.IndexOfName("tp" & vs.ID)
				If nIndex < 0 Then
					tp = New DevExpress.Web.ASPxTabControl.TabPage(vs.Name, "tp" & vs.ID)
				Else
					tp = Me.axpcCreative.TabPages(nIndex)
				End If

				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				vse.Program = Me.Program
				vse.VariableSet = vs
				ctrl.ID = "vse" & vse.VariableSet.ID
				vse.loadData(initial_load)
				tp.Controls.Add(ctrl)

				m_VariableSetEditors.Add(vse)
				Me.axpcCreative.TabPages.Add(tp)
				m_VariableSetTapPages.Add(vse.VariableSet.ID, tp)
			Next

			For Each vs In vsc.Where(Function(f) f.SourceCode = "LOCATION").OrderBy(Function(f) f.ID)
				nIndex = Me.axpcLocations.TabPages.IndexOfName("tpLocation" & vs.ID)
				If nIndex < 0 Then
					tp = New DevExpress.Web.ASPxTabControl.TabPage(vs.Name, "tpLocation" & vs.ID)
				Else
					tp = Me.axpcLocations.TabPages(nIndex)
				End If

				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariableSetEditor.ascx")
				vse = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor)
				vse.Program = Me.Program
				vse.VariableSet = vs
				ctrl.ID = "vse" & vse.VariableSet.ID
				vse.loadData(initial_load)
				tp.Controls.Add(ctrl)

				m_VariableSetEditors.Add(vse)
				Me.axpcLocations.TabPages.Add(tp)
				m_VariableSetTapPages.Add(vse.VariableSet.ID, tp)
			Next

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
			Dim tp As DevExpress.Web.ASPxTabControl.TabPage
			Dim b As Boolean = True

			For Each vse As DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor In m_VariableSetEditors
				If Not vse.validateData() Then
					b = False

					If m_VariableSetTapPages.TryGetValue(vse.VariableSet.ID, tp) Then
						Me.axpcCreative.ActiveTabPage = tp
					End If
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