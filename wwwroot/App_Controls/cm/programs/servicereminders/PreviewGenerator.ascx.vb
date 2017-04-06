Namespace DealerDirect.UserControls.CampaignManagement.Programs.ServiceReminders
    Partial Class PreviewGenerator
        Inherits System.Web.UI.UserControl
        Implements DealerDirect.UserControls.CampaignManagement.Programs.IPreviewGenerator

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
            Me.ddlSubVersion.Enabled = (Me.ddlSubVersion.Items.Count > 1)
            Me.ddlStyleOption.Enabled = (Me.StyleGroupCode = "SEASONAL")
            ' Me.ViewState("preview") = m_Preview
        End Sub


        Public Sub loadData(ByVal initial_load As Boolean) Implements IPreviewGenerator.loadData
            If initial_load OrElse m_LastStyleGroupCode <> m_StyleGroupCode Then
                'Dim lei As Telerik.Web.UI.DropDownListItem
                'Dim lei As DevExpress.Web.ASPxEditors.ListEditItem

                Me.ddlTarget.Items.Clear()
                For Each pvi As DealerDirect.CampaignManagement.Programs.VDP.PreviewTarget In m_Program.getPreviewTargets.getTargets.Values
                    'lei = New DevExpress.Web.ASPxEditors.ListEditItem(pvi.Name, pvi.Code)
                    'Me.ddlTarget.Items.Add(lei)
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





        'Private Sub loadVersions(ByVal target_code As String, ByVal style_group_code As String)
        '    Dim sCurrent As String = ""

        '    If Me.ddlSubVersion.SelectedItem IsNot Nothing Then
        '        sCurrent = Me.ddlSubVersion.SelectedItem.Value
        '    End If

        '    Me.rcbMultiSV.Items.Clear()
        '    For Each pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion In m_Program.getPreviewVersions(target_code, style_group_code)
        '        Me.rcbMultiSV.Items.Add(New Telerik.Web.UI.RadComboBoxItem(pv.Name, pv.VersionCode))
        '    Next

        '    If Me.rcbMultiSV.Items.Count > 0 Then
        '        log.Debug("Loaded " & Me.rcbMultiSV.Items.Count & " items")
        '    End If
        'End Sub



        Private Sub loadVersions(ByVal target_code As String, ByVal style_group_code As String)
            'Dim lei As DevExpress.Web.ASPxEditors.ListEditItem
            'Dim slei As DevExpress.Web.ASPxEditors.ListEditItem
            'Dim lei As Telerik.Web.UI.DropDownListItem
            'Dim slei As Telerik.Web.UI.DropDownListItem
            Dim sCurrent As String = ""

            If Me.ddlSubVersion.SelectedItem IsNot Nothing Then
                sCurrent = Me.ddlSubVersion.SelectedItem.Value
            End If

            Me.rcbMultiSV.Items.Clear()
            For Each pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion In m_Program.getPreviewVersions(target_code, style_group_code)
                Me.rcbMultiSV.Items.Add(New Telerik.Web.UI.RadComboBoxItem(pv.Name, pv.VersionCode))
            Next

            'Try
            '    For i As Integer = 0 To Me.ddlSubVersion.Items.Count - 1
            '        If Me.ddlSubVersion.Items(i).Value = slei.Value Then
            '            Me.ddlSubVersion.SelectedIndex = i
            '            Exit For
            '        End If
            '    Next

            '    'Me.rddlSubVersion.SelectedIndex = Me.rddlSubVersion.FindItemByValue(slei.Value).Index
            '    If Me.ddlSubVersion.SelectedItem Is Nothing Then Me.ddlSubVersion.SelectedIndex = 0

            'Catch ex As Exception
            '    If Me.ddlSubVersion.SelectedItem Is Nothing Then Me.ddlSubVersion.SelectedIndex = 0
            'End Try
        End Sub

        Protected Sub ddlTarget_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTarget.SelectedIndexChanged
            Me.loadLanguageOptions(Me.ddlTarget.SelectedItem.Value)
            Me.loadChannelOptions(Me.ddlTarget.SelectedItem.Value)
            Me.loadVersions(Me.ddlTarget.SelectedItem.Value, m_StyleGroupCode)
        End Sub

        'Protected Sub ddlLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLanguage.SelectedIndexChanged
        '    Me.loadChannelOptions(Me.ddlTarget.SelectedItem.Value)
        '    Me.loadVersions(Me.ddlTarget.SelectedItem.Value, m_StyleGroupCode)
        'End Sub

        'Protected Sub ibtnCreatePreview_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCreatePreview.Click
        '    Dim bResult As Boolean

        '    If m_Preview IsNot Nothing Then m_Preview.CancelCreate()

        '    RaiseEvent PrepareVariablesForPreview(Me, bResult)

        '    If bResult Or True Then
        '        Dim pvFound As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion = Nothing
        '        Dim pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion

        '        For Each pv In m_Program.getPreviewVersions(Me.ddlTarget.SelectedItem.Value, Me.StyleGroupCode)
        '            If pv.VersionCode = Me.ddlSubVersion.SelectedItem.Value Then
        '                pvFound = pv
        '                Try
        '                    pvFound.Option1 = Me.ddlBrand.SelectedValue
        '                Catch ex As Exception
        '                End Try
        '                pvFound.StyleGroupCode = Me.StyleGroupCode
        '                pvFound.StyleOptionCode = Me.ddlStyleOption.SelectedValue
        '                Exit For
        '            End If
        '        Next

        '        If pvFound IsNot Nothing Then
        '            m_Preview = m_Program.createPreview(Me.ddlLanguage.SelectedValue, Me.ddlChannel.SelectedValue, pvFound)
        '            If m_Preview IsNot Nothing Then m_Preview.QueueCreate()
        '        End If
        '    End If

        '    Me.ViewState("preview") = m_Preview
        'End Sub
    End Class
End Namespace