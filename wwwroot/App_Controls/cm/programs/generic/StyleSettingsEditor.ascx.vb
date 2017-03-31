Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
    Partial Class StyleSettingsEditor
        Inherits System.Web.UI.UserControl
        Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor

        Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
        Protected m_StyleOptionPrompts As New System.Collections.Generic.Dictionary(Of String, String)
        Protected m_LastStyleGroupCode As String

        Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.Program
            Get
                Return m_Program
            End Get
            Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
                m_Program = value
            End Set
        End Property

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Me.ViewState("last_style_group_code") = m_LastStyleGroupCode
        End Sub

        Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.loadData
            Dim ctrl As System.Web.UI.Control
            Dim bHasOptions As Boolean = False
            Dim sStyleGroupCode As String

            If initial_load Then
                rblStyles.DataSource = m_Program.StyleGroups.ToList.OrderBy(Function(f) f.DisplayOrderNumber)
                Me.rblStyles.DataTextField = "Name"
                Me.rblStyles.DataValueField = "Code"
                Me.rblStyles.DataBind()

                If initial_load Then Me.rblStyles.SelectedValue = m_Program.StyleGroupCode
            End If

            For Each li As System.Web.UI.WebControls.ListItem In Me.rblStyles.Items
                If li.Value = "DEALER" AndAlso Me.Program.ProgramID < 300 Then
                    li.Enabled = False
                End If
            Next

            sStyleGroupCode = Me.rblStyles.SelectedValue
            If sStyleGroupCode IsNot Nothing Then
                Dim sop As DealerDirect.UserControls.CampaignManagement.Programs.IStyleOptionPrompt

                If m_Program.StyleGroups.Contains(sStyleGroupCode) Then
                    m_LastStyleGroupCode = Me.ViewState("last_style_group_code")

                    For Each so As DealerDirect.CampaignManagement.Programs.Styles.StyleOption In m_Program.StyleGroups(sStyleGroupCode).StyleOptions.ToList.OrderBy(Function(f) f.DisplayOrderNumber)
                        ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/StyleOptionPrompt.ascx")
                        sop = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IStyleOptionPrompt)

                        If Not m_StyleOptionPrompts.ContainsKey(so.Code) AndAlso so.Styles.Count > 0 Then
                            sop.Program = Me.Program
                            sop.StyleOption = so
                            sop.loadData(initial_load OrElse (m_LastStyleGroupCode <> sStyleGroupCode))
                            ctrl.ID = "sop" & sop.StyleOption.Code
                            Me.phStyleOptions.Controls.Add(ctrl)

                            m_StyleOptionPrompts.Add(so.Code, so.Code)
                        End If

                        bHasOptions = True
                    Next
                End If
            End If

            If m_Program.StyleGroups.Contains("SM_SEAS") OrElse m_Program.StyleGroups.Contains("SEASONAL") Then
                Me.pnlSeasonMappings.Visible = True
                Me.renderSeasonMappings(initial_load)
            Else
                Me.pnlSeasonMappings.Visible = False
            End If

            m_LastStyleGroupCode = sStyleGroupCode

            Me.pnlStyleOptions.Visible = bHasOptions
        End Sub

        Private Sub renderSeasonMappings(ByVal initial_load As Boolean)
            Dim td As System.Web.UI.HtmlControls.HtmlTableCell
            Dim rbl As System.Web.UI.WebControls.RadioButtonList
            Dim ctrl As System.Web.UI.Control

            For i As Integer = 1 To 12
                ctrl = Me.phMonthSettings.FindControl("rblMonth" & i)
                If ctrl Is Nothing Then
                    td = New System.Web.UI.HtmlControls.HtmlTableCell
                    'td.Attributes.Add("style", "text-align:center; font-weight:bold; padding:3px;  border-right:1px solid #A8A8A8; border-top:1px solid #A8A8A8; border-bottom:1px solid #A8A8A8;")
                    td.InnerHtml = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i)

                    If i = 1 Then
                        td.Attributes.Add("style", "text-align:center; padding:3px; border-bottom:1px solid #5397f0;")
                    Else
                        td.Attributes.Add("style", "text-align:center; padding:3px; border-left:1px solid #5397f0; border-bottom:1px solid #5397f0;")
                    End If

                    Me.phMonthLabels.Controls.Add(td)

                    rbl = New System.Web.UI.WebControls.RadioButtonList
                    rbl.RepeatDirection = RepeatDirection.Vertical
                    rbl.CellPadding = 0
                    rbl.ID = "rblMonth" & i
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "P"))
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "U"))
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "F"))
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "W"))
                    If initial_load Then rbl.SelectedValue = CStr(m_Program.SeasonMappings.Chars(i - 1))

                    td = New System.Web.UI.HtmlControls.HtmlTableCell

                    If i = 1 Then
                        td.Attributes.Add("style", "width:30px;padding-left:7px;text-align:center;")
                    Else
                        td.Attributes.Add("style", "width:30px;padding-left:7px;text-align:center; border-left:1px solid #5397f0;")
                    End If

                    td.RowSpan = 4
                    td.Controls.Add(rbl)
                    Me.phMonthSettings.Controls.Add(td)
                End If
            Next
        End Sub

        Public ReadOnly Property HasStyleOptions() As Boolean
            Get
                Return Me.pnlStyleOptions.Visible
            End Get
        End Property

        Public ReadOnly Property StyleGroupCode() As String
            Get
                Return Me.rblStyles.SelectedValue
            End Get
        End Property

        Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.validateData
            Me.cvMain.Validate()
            Return Me.cvMain.IsValid
        End Function

        Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.saveData
            Dim bSuccess As Boolean = True

            If m_Program.setGenericSetting("STYLES", "SEASON_MAPPINGS", 0, getSeasonMappingsString, Nothing) AndAlso m_Program.setStyleGroup(Me.rblStyles.SelectedValue) Then
                For Each sop As DealerDirect.UserControls.CampaignManagement.Programs.IStyleOptionPrompt In Me.phStyleOptions.Controls
                    bSuccess = sop.saveData()
                    If Not bSuccess Then Return False
                Next
            End If

            If bSuccess Then

            End If

            Return bSuccess
        End Function

        Public Function savePreviewData() As Boolean
            Dim bSuccess As Boolean = True

            For Each sop As DealerDirect.UserControls.CampaignManagement.Programs.IStyleOptionPrompt In Me.phStyleOptions.Controls
                bSuccess = sop.savePreviewData()
                If Not bSuccess Then Return False
            Next

            Return bSuccess
        End Function

        Protected Sub cvMain_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMain.ServerValidate
            Dim bValid As Boolean = True

            If Me.rblStyles.SelectedIndex < 0 Then
                Me.cvMain.ErrorMessage = DealerDirect.Resources.getString("LITERAL_Please_select_an_option")
                args.IsValid = False
                Exit Sub
            End If

            args.IsValid = True
        End Sub

        Private Function getSeasonMappingsString() As String
            Dim rbl As System.Web.UI.WebControls.RadioButtonList
            Dim ctrl As System.Web.UI.Control
            Dim s As String = ""

            For i As Integer = 1 To 12
                ctrl = Me.phMonthSettings.FindControl("rblMonth" & i)

                If ctrl IsNot Nothing Then
                    rbl = DirectCast(ctrl, System.Web.UI.WebControls.RadioButtonList)
                    s &= IIf(rbl.SelectedValue = "", "P", rbl.SelectedValue)
                Else
                    s &= "P"
                End If
            Next i

            Return s
        End Function
    End Class
End Namespace