Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class PreviewGeneratorV3
		Inherits System.Web.UI.UserControl
        Implements DealerDirect.UserControls.CampaignManagement.Programs.IPreviewGenerator

        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(PreviewGeneratorV3))

		Protected m_Preview As DealerDirect.CampaignManagement.Programs.VDP.Preview
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_LastStyleGroupCode As String
		Protected m_StyleGroupCode As String

		Public Event PrepareVariablesForPreview(ByVal sender As Object, ByRef success As Boolean) Implements IPreviewGenerator.PrepareVariablesForPreview

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements IPreviewGenerator.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Public Property StyleGroupCode() As String Implements IPreviewGenerator.StyleGroupCode
			Get
				Return m_StyleGroupCode
			End Get
			Set(ByVal value As String)
				m_StyleGroupCode = value
			End Set
		End Property

		Public ReadOnly Property ShowStyleOption() As Boolean
			Get
				Return (m_StyleGroupCode IsNot Nothing)
			End Get
		End Property

		Public ReadOnly Property PreviewURL() As String
			Get
				If m_Preview IsNot Nothing Then Return "https://www.gmdealerdirect.ca/vdp/previews/" & m_Preview.URL Else Return Nothing
			End Get
		End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			m_Preview = Me.ViewState("preview")
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'If m_Preview Is Nothing Then m_Preview = Me.ViewState("preview")

            'Me.pnlCreatingPreview.Visible = False
            'Me.pnlViewPreview.Visible = False

            'If m_Preview IsNot Nothing Then
            '	If m_Preview.isAvailable(True) Then
            '		Me.pnlViewPreview.Visible = True
            '	Else
            '		Me.pnlCreatingPreview.Visible = True
            '	End If
            'End If

            Me.ddlSubVersion.Enabled = (Me.ddlSubVersion.Items.Count > 1)

            'Me.ViewState("preview") = m_Preview
        End Sub


		Public Sub loadData(ByVal initial_load As Boolean) Implements IPreviewGenerator.loadData
			If initial_load OrElse m_LastStyleGroupCode <> m_StyleGroupCode Then

                Me.ddlTarget.Items.Clear()
                For Each pvi As DealerDirect.CampaignManagement.Programs.VDP.PreviewTarget In m_Program.getPreviewTargets.getTargets.Values
                    Me.ddlTarget.Items.Add(New System.Web.UI.WebControls.ListItem(pvi.Name, pvi.Code))
                Next

                If Me.ddlTarget.SelectedItem Is Nothing Then Me.ddlTarget.SelectedIndex = 0

				Me.loadLanguageOptions(Me.ddlTarget.SelectedItem.Value)
				Me.loadChannelOptions(Me.ddlTarget.SelectedItem.Value)
				Me.loadVersions(Me.ddlTarget.SelectedItem.Value, m_StyleGroupCode)

				m_LastStyleGroupCode = m_StyleGroupCode
			End If
		End Sub

        Private Sub loadLanguageOptions(ByVal target_code As String)
            Me.chkEnglish.Enabled = False
            Me.chkFrench.Enabled = False

            For Each pt As DealerDirect.CampaignManagement.Programs.VDP.PreviewTarget In m_Program.getPreviewTargets.getLanguages(target_code).Values
                If pt.LanguageCode = "EN" Then
                    Me.chkEnglish.Enabled = True
                ElseIf pt.LanguageCode = "FR" Then
                    Me.chkFrench.Enabled = True
                End If
            Next
        End Sub

        Private Sub loadChannelOptions(ByVal target_code As String)
            Me.chkMail.Enabled = False
            Me.chkEMail.Enabled = False

            For Each pt As DealerDirect.CampaignManagement.Programs.VDP.PreviewTarget In m_Program.getPreviewTargets.getChannels(target_code).Values
                If pt.ChannelTypeCode = "D" Then
                    Me.chkMail.Enabled = True
                ElseIf pt.ChannelTypeCode = "E" Then
                    Me.chkEMail.Enabled = True
                End If
            Next
        End Sub

        Private Sub loadVersions(ByVal target_code As String, ByVal style_group_code As String)
            Dim sCurrent As String = ""

            If Me.ddlSubVersion.SelectedItem IsNot Nothing Then
                sCurrent = Me.ddlSubVersion.SelectedItem.Value
            End If

            Me.rcbMultiSV.Items.Clear()
            For Each pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion In m_Program.getPreviewVersions(target_code, style_group_code)
                Me.rcbMultiSV.Items.Add(New Telerik.Web.UI.RadComboBoxItem(pv.Name, pv.VersionCode))
            Next

            If Me.rcbMultiSV.Items.Count > 0 Then
                log.Debug("Loaded " & Me.rcbMultiSV.Items.Count & " items")
            End If
        End Sub

        Protected Sub ddlTarget_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTarget.SelectedIndexChanged
            Me.loadLanguageOptions(Me.ddlTarget.SelectedItem.Value)
            Me.loadChannelOptions(Me.ddlTarget.SelectedItem.Value)
            Me.loadVersions(Me.ddlTarget.SelectedItem.Value, m_StyleGroupCode)
        End Sub
    End Class
End Namespace