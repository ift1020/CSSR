Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class OfferTargetPrompt
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IOfferTargetPrompt

		Protected m_OfferTarget As DealerDirect.CampaignManagement.Programs.Offers.OfferTarget
		Protected m_AvailableOffers As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOfferCollection

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements ISettingsEditor.Program
			Get
				Return Nothing
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
			End Set
		End Property

		Public Property OfferTarget() As DealerDirect.CampaignManagement.Programs.Offers.OfferTarget Implements IOfferTargetPrompt.OfferTarget
			Get
				Return m_OfferTarget
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Offers.OfferTarget)
				m_OfferTarget = value
			End Set
		End Property
		Public Property AvailableOffers() As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOfferCollection Implements IOfferTargetPrompt.AvailableOffers
			Get
				Return m_AvailableOffers
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOfferCollection)
				m_AvailableOffers = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			'If Me.m_Variable Is Nothing Then Me.m_Variable = Me.ViewState("variable")
			'DealerDirect.UserControls.CampaignManagement.Programs.Specialized.SMSP. 
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Dim ogo As DealerDirect.CampaignManagement.Programs.Offers.OfferGroupOffer

			ogo = m_AvailableOffers.Where(Function(f) f.Code = Me.ddlOffers.SelectedValue).FirstOrDefault
			If ogo IsNot Nothing Then
				Me.pnlExpiryDate.Visible = Not ogo.isAutoOffer
			Else
				Me.pnlExpiryDate.Visible = True
			End If

			If Me.pnlExpiryDate.Visible AndAlso Me.txtExpiryDays.Text.Length = 0 Then
				Me.txtExpiryDays.Text = m_OfferTarget.ExpiryDays
			End If
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements ISettingsEditor.loadData
			Me.ddlOffers.DataSource = m_AvailableOffers.ToList.OrderBy(Function(f) f.DisplayOrderNumber)
			Me.ddlOffers.DataTextField = "Name"
			Me.ddlOffers.DataValueField = "Code"
			Me.ddlOffers.DataBind()

			If initial_load Then
                Me.chkSelected.Checked = m_OfferTarget.Selected OrElse Not m_OfferTarget.AllowDeSelect
                Me.ddlOffers.SelectedValue = m_OfferTarget.OfferCode
				Me.txtExpiryDays.Text = m_OfferTarget.ExpiryDays
			End If

            'enbled 
            Me.chkSelected.Visible = m_OfferTarget.AllowDeSelect
        End Sub

		Public Function saveData() As Boolean Implements ISettingsEditor.saveData
			If Not Me.chkSelected.Checked Then
                Return m_OfferTarget.setValues((Me.chkSelected.Checked OrElse Not m_OfferTarget.AllowDeSelect), Me.ddlOffers.SelectedValue, 45)
            Else
				Dim nOfferExpiryDays As Integer
				If Me.txtExpiryDays.Visible Then
					If Integer.TryParse(Me.txtExpiryDays.Text, nOfferExpiryDays) Then
                        Return m_OfferTarget.setValues((Me.chkSelected.Checked OrElse Not m_OfferTarget.AllowDeSelect), Me.ddlOffers.SelectedValue, nOfferExpiryDays)
                    Else
						Return False
					End If
				Else
                    Return m_OfferTarget.setValues((Me.chkSelected.Checked OrElse Not m_OfferTarget.AllowDeSelect), Me.ddlOffers.SelectedValue, 0)
                End If
			End If
		End Function

		Public Function validateData() As Boolean Implements ISettingsEditor.validateData
			Dim b As Boolean

            If Not (Me.chkSelected.Checked OrElse Not m_OfferTarget.AllowDeSelect) Then
                Return True

            ElseIf Me.ddlOffers.SelectedValue <> "" Then
                If Me.pnlExpiryDate.Visible Then
                    Dim n As Integer
                    If Integer.TryParse(Me.txtExpiryDays.Text, n) Then
                        b = (n >= 30)
                    Else : b = False
                    End If

                    If Not b Then Me.lblOfferError.Text = "<br/>" & DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:MISSING_EXPIRY")

                Else
                    b = True
                End If

                If Not b Then Me.lblOfferError.Text = "<br/>" & DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:MISSING_EXPIRY")

            Else
                b = False
				Me.lblOfferError.Text = "<br/>" & DealerDirect.Resources.getString("CM_PRGM:GENERIC:OFFERS:MISSING_OFFER")
			End If

			If b Then Me.lblOfferError.Text = Nothing

			Return b
		End Function

		Protected Sub ddlOffers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOffers.SelectedIndexChanged
		End Sub
	End Class
End Namespace