Namespace DealerDirect.UserControls.CampaignManagement.Programs.ServiceReminders
    Partial Class ServiceSettingsComponentV3
        Inherits System.Web.UI.UserControl
        Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent

        Protected m_ComponentIndex As Integer
        Protected m_Program As DealerDirect.CampaignManagement.Programs.CSSR.ServiceReminders.Program

        Public Event ApprovalStateChanged(ByVal sender As DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent, ByVal new_approval_state As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.ApprovalStateChanged

        Protected m_HelpLink As String
        Public Property HelpLink() As String Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.HelpLink
            Get
                If DealerDirect.Resources.doesStringExist(m_HelpLink) Then
                    Return DealerDirect.Resources.getString(m_HelpLink)
                Else
                    Return m_HelpLink
                End If
            End Get
            Set(ByVal value As String)
                m_HelpLink = value
            End Set
        End Property

        Protected m_InlineHelp As String
        Public Property InlineHelp() As String Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.InlineHelp
            Get
                If DealerDirect.Resources.doesStringExist(m_InlineHelp) Then
                    Return DealerDirect.Resources.getString(m_InlineHelp)
                Else
                    Return m_InlineHelp
                End If
            End Get
            Set(ByVal value As String)
                m_InlineHelp = value
            End Set
        End Property

        Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.Program
            Get
                Return m_Program
            End Get
            Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
                m_Program = value
            End Set
        End Property

        Public Property ComponentIndex() As Integer Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.ComponentIndex
            Get
                Return m_ComponentIndex
            End Get
            Set(ByVal value As Integer)
                m_ComponentIndex = value
            End Set
        End Property

        Public ReadOnly Property isApproved() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.isApproved
            Get
                Return m_Program.isComponentApproved(Me.ComponentIndex)
            End Get
        End Property


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved)
            Me.pnlMain.Enabled = Not Me.isApproved

            Me.pnlTireRotation.Visible = Not Me.chkEnableTireChangeMessaging.Checked
            Me.pnlTireChange.Visible = Me.chkEnableTireChangeMessaging.Checked
        End Sub

        Private Sub renderTireSettings(ByVal initial_load As Boolean)
            Dim td As System.Web.UI.HtmlControls.HtmlTableCell
            Dim rbl As System.Web.UI.WebControls.RadioButtonList
            Dim ctrl As System.Web.UI.Control

            If initial_load Then
                Dim s As String = m_Program.getGenericSettingText("SERVICES", "TIRE_MESSAGING", 0)
                'If m_Program.getGenericSettingText("SERVICES", "TIRE_MESSAGING", 0) Then
                Me.chkEnableTireChangeMessaging.Checked = (m_Program.getGenericSettingText("SERVICES", "TIRE_MESSAGING", 0).Chars(0) = "Y")
            End If

            For i As Integer = 1 To 12
                ctrl = Me.phMonthSettings.FindControl("rblMonth" & i)
                If ctrl Is Nothing Then
                    td = New System.Web.UI.HtmlControls.HtmlTableCell
                    If i = 1 Then
                        td.Attributes.Add("style", "text-align:center; padding:3px; border-bottom:1px solid #5397f0;")
                    Else
                        td.Attributes.Add("style", "text-align:center; padding:3px; border-left:1px solid #5397f0; border-bottom:1px solid #5397f0;")
                    End If

                    td.InnerHtml = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i)
                    Me.phMonthLabels.Controls.Add(td)

                    rbl = New System.Web.UI.WebControls.RadioButtonList
                    rbl.RepeatDirection = RepeatDirection.Vertical
                    rbl.CellPadding = 0
                    rbl.ID = "rblMonth" & i
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "R"))
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "L"))
                    rbl.Items.Add(New System.Web.UI.WebControls.ListItem("", "S"))
                    If initial_load Then rbl.SelectedValue = m_Program.getGenericSettingText("SERVICES", "TIRE_MESSAGING", 0).Chars(i)

                    td = New System.Web.UI.HtmlControls.HtmlTableCell
                    If i = 1 Then
                        td.Attributes.Add("style", "width:35px;padding-left:10px;text-align:center;")
                    Else
                        td.Attributes.Add("style", "width:35px;padding-left:10px;text-align:center; border-left:1px solid #5397f0;")
                    End If

                    td.RowSpan = 3
                    td.Controls.Add(rbl)
                    Me.phMonthSettings.Controls.Add(td)
                End If
            Next
        End Sub

        Protected Sub rbtnComponentStatus_Click(sender As Object, e As EventArgs) Handles rbtnComponentStatus.Click
            Dim bInitialValue = Me.isApproved

            If Not bInitialValue AndAlso Me.validateData AndAlso Me.saveData Then
                m_Program.setComponentApproval(Me.ComponentIndex, True)
            Else
                m_Program.setComponentApproval(Me.ComponentIndex, False)
            End If

            DealerDirect.CampaignManagement.Programs.CSSR.Helper.updateComponentStatusVisuals(Me.rbtnComponentStatus, Me.isApproved)

            If bInitialValue <> Me.isApproved Then
                RaiseEvent ApprovalStateChanged(Me, Me.isApproved)
                Response.Redirect(Page.Request.RawUrl)
            End If
        End Sub

        Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.loadData
            If initial_load Then
                'Me.chkSettingsApproved.Checked = m_Program.isComponentApproved(Me.ComponentIndex)
            End If

            Me.renderTireSettings(initial_load)
        End Sub

        Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.validateData
            Dim b As Boolean = True

            'Me.lblErrorMessage.Visible = Not b
            Return b
        End Function

        Public Function saveData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsComponent.saveData
            Dim b As Boolean = True

            If m_Program.setGenericSetting("SERVICES", "TIRE_MESSAGING", 0, Me.getTireSettingsString, Nothing) Then
            Else
                b = False
            End If

            Return b
        End Function

        Private Function getTireSettingsString() As String
            Dim rbl As System.Web.UI.WebControls.RadioButtonList
            Dim ctrl As System.Web.UI.Control
            Dim s As String

            s = IIf(Me.chkEnableTireChangeMessaging.Checked, "Y", "N")
            For i As Integer = 1 To 12
                ctrl = Me.phMonthSettings.FindControl("rblMonth" & i)

                If ctrl IsNot Nothing Then
                    rbl = DirectCast(ctrl, System.Web.UI.WebControls.RadioButtonList)
                    s &= IIf(rbl.SelectedValue = "", "R", rbl.SelectedValue)
                Else
                    s &= "R"
                End If
            Next i

            Return s
        End Function

        Public Sub setErrorMessage(ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
        End Sub

        Public Sub setErrorMessage1(ByVal error_code As String, ByVal error_message As String) Implements ISettingsComponent.setErrorMessage
        End Sub
    End Class
End Namespace