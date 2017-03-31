
Partial Class cm_programs_cssr_creativeDefault
    Inherits System.Web.UI.Page

    Protected m_SubscriptionID As Integer
    Protected m_Program As DealerDirect.CampaignManagement.Programs.AudienceProgram
    Protected m_EnablementChanged As Boolean = False

    Public Event SetComponentFocus(ByVal component_index As Integer)

    Public Property Program() As DealerDirect.CampaignManagement.Programs.AudienceProgram
        Get
            Return m_Program
        End Get
        Set(value As DealerDirect.CampaignManagement.Programs.AudienceProgram)
            m_Program = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Program = Me.ViewState("program")

        If Me.Program Is Nothing Then
            Select Case Request("pid")
                Case 303
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR2017.ThankYou.Program
                Case 304
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR2017.Anniversary.Program
                Case 305
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR2017.DexCool.Program
                Case 201
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.WelcomeNew.Program
                Case 202
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.ServiceReminders.Program
                Case 203
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.ThankYou.Program
                Case 204
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.Anniversary.Program
                Case 205
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.DexCool.Program
                Case 206
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.Warranty.Program
                Case 207
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.Lease.Program
                Case 208
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.Finance.Program
                    Me.cseMain.ComponentIndex = 2
                Case 209
                    Me.Program = New DealerDirect.CampaignManagement.Programs.CSSR.WelcomeUsed.Program
            End Select
        End If

        'If Me.Program.PreviewGeneratorControlPath IsNot Nothing Then
        '    Me.cseMain.PreviewGeneratorControlPath = "~/App_Controls/cm/programs/cssr/PreviewGeneratorWelcomeNew.ascx"
        'End If

        Me.cseMain.Program = Me.Program
        Me.cseMain.loadData(Not Me.IsPostBack)
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.ViewState("program") = Me.Program
    End Sub
End Class
