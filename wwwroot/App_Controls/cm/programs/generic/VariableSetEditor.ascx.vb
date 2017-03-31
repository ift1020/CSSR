Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class VariableSetEditor
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor

		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_VariableSet As DealerDirect.CampaignManagement.Programs.VDP.VariableSet

		Protected m_Enabled As Boolean

		Public Property Enabled As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.Enabled
			Get
				Return Me.pnlMain.Enabled
			End Get
			Set(value As Boolean)
				Me.pnlMain.Enabled = value
			End Set
		End Property

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Public Property VariableSet() As DealerDirect.CampaignManagement.Programs.VDP.VariableSet Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.VariableSet
			Get
				Return m_VariableSet
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.VDP.VariableSet)
				m_VariableSet = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.loadData
			Dim ivp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt
			Dim ctrl As System.Web.UI.Control

			For Each v As DealerDirect.CampaignManagement.Programs.VDP.Variable In m_VariableSet.Variables.OrderBy(Function(f) f.DisplayOrderNumber)
				'If v.StyleGroupCode Is Nothing OrElse v.StyleGroupCode = m_Program.StyleGroupCode Then

				ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/VariablePrompt.ascx")
				ivp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
				ivp.Variable = m_Program.getVariableSetVariable(m_VariableSet.ID, v.ID)
				ivp.Visible = (v.StyleGroupCode Is Nothing OrElse v.StyleGroupCode = m_Program.StyleGroupCode)
				ctrl.ID = "vp" & ivp.Variable.ID
				ctrl.Visible = ivp.Visible
				ivp.loadData(initial_load)


				Me.phVariablePrompts.Controls.Add(ctrl)
				'End If
			Next
		End Sub

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.validateData
			Dim b As Boolean = True

			For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In Me.phVariablePrompts.Controls
				If vp.Visible Then	' if not controlled by a parent... 
					If Not vp.Variable.ParentVariableID.HasValue Then
						If Not vp.validateData Then b = False
					End If
				End If
			Next
			Return b
		End Function

		Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.saveData
			Return saveDataB(False)
		End Function

		Public Function savePreviewData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.savePreviewData
			Return saveDataB(True)
		End Function

		Private Function saveDataB(Optional ByVal temporary As Boolean = False) As Boolean
			Dim b As Boolean = True

			For Each vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt In Me.getAllVariablePrompts
				If Not vp.saveData(temporary) Then
					b = False
				End If
			Next

			Return b
		End Function

		Public Function getAllVariablePrompts() As System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt) Implements DealerDirect.UserControls.CampaignManagement.Programs.IVariableSetEditor.getAllVariablePrompts
			Dim l As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
			Dim vp As DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt

			For Each ctrl As System.Web.UI.Control In Me.phVariablePrompts.Controls
				If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt Then
					vp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IVariablePrompt)
					l.Add(vp)
				End If
			Next

			Return l
		End Function
	End Class
End Namespace