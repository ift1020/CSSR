Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class CampaignBuildStatusItem
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IBuildStatusItem

		Protected m_BuildStatusItem As DealerDirect.CampaignManagement.Programs.CampaignBuilder.BuildStatusItem

		Public Property BuildStatusItem() As DealerDirect.CampaignManagement.Programs.CampaignBuilder.BuildStatusItem Implements IBuildStatusItem.BuildStatusItem
			Get
				Return m_BuildStatusItem
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.CampaignBuilder.BuildStatusItem)
				m_BuildStatusItem = value
			End Set
		End Property

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			If m_BuildStatusItem IsNot Nothing Then
				If m_BuildStatusItem.StatusCode = "ACTV" Then
					Me.lblProcessName.Text = String.Format("{0}...", DealerDirect.Resources.getListItem("CM_BUILDING_PROCESS", m_BuildStatusItem.ProcessCode))
					Me.lblProcessName.Font.Bold = True

					Me.lblStatus.Text = String.Format("({0}%)", m_BuildStatusItem.PercentComplete)
					Me.lblStatus.Font.Bold = True

				Else
					Me.lblProcessName.Text = String.Format("{0}", DealerDirect.Resources.getListItem("CM_BUILDING_PROCESS", m_BuildStatusItem.ProcessCode))
					Me.lblProcessName.Font.Bold = False	'(m_BuildStatusItem.StatusCode = "COMP")

					Me.lblStatus.Text = String.Format("({0})", DealerDirect.Resources.getListItem("CM_BUILDING_STATUS", m_BuildStatusItem.StatusCode))
					Select Case m_BuildStatusItem.StatusCode
						Case "COMP"
							Me.lblProcessName.Font.Bold = True
							Me.lblStatus.ForeColor = Drawing.Color.Green
							Me.lblStatus.Font.Bold = True

						Case "FAIL"
							Me.lblProcessName.ForeColor = Drawing.Color.Red
							Me.lblProcessName.Font.Bold = True
							Me.lblStatus.ForeColor = Drawing.Color.Red
							Me.lblStatus.Font.Bold = True
					End Select
				End If
			End If
		End Sub
	End Class
End Namespace
