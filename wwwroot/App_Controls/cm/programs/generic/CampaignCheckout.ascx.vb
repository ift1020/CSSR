Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class CampaignCheckout
		Inherits System.Web.UI.UserControl

		Protected m_CampaignBuilder As DealerDirect.CampaignManagement.Programs.CampaignBuilder
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_CampaignID As Integer
		Private m_PageStep As PageStep

		Private Enum PageStep As Integer
			NotSet = 0
			Build = 1
			Approve = 2
			SubmitComplete = 3
			BuildFailed = -1
			SubmitFailed = -2
		End Enum

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Public Property CampaignID() As Integer
			Get
				Return m_CampaignID
			End Get
			Set(ByVal value As Integer)
				m_CampaignID = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Dim cbs As DealerDirect.CampaignManagement.Programs.CampaignBuilder.CampaignBuildStatus

			m_CampaignBuilder = Me.ViewState("campaign_builder")

			Me.pnlCreateCampaign.Visible = False
			Me.tmrRefresh.Enabled = False
			Me.pnlApprove.Visible = False

			If m_CampaignID > 0 AndAlso m_PageStep = PageStep.NotSet Then
				If m_CampaignBuilder IsNot Nothing Then
					If m_CampaignBuilder.CampaignID <> m_CampaignID Then
						m_CampaignBuilder = New DealerDirect.CampaignManagement.Programs.CampaignBuilder(m_Program.ProgramID, m_Program.SubCampaignID, m_CampaignID)
					End If
				Else
					m_CampaignBuilder = New DealerDirect.CampaignManagement.Programs.CampaignBuilder(m_Program.ProgramID, m_Program.SubCampaignID, m_CampaignID)
				End If

				cbs = m_CampaignBuilder.BuildStatus
				Select Case cbs
					Case DealerDirect.CampaignManagement.Programs.CampaignBuilder.CampaignBuildStatus.NotStarted
						m_CampaignBuilder.BeginBuild()
						m_PageStep = PageStep.Build

					Case DealerDirect.CampaignManagement.Programs.CampaignBuilder.CampaignBuildStatus.Complete
						m_PageStep = PageStep.Approve

						Me.pnlApprove.Visible = True
						Me.generateOrderSummary()
						Me.btnSubmit.ImageUrl = IIf(Me.pnlApprove.Visible AndAlso Me.chkApproveCampaign.Checked, DealerDirect.Resources.getString("IMAGE_BUTTON:SUBMIT_ORDER:ON"), DealerDirect.Resources.getString("IMAGE_BUTTON:SUBMIT_ORDER:OFF"))
						Me.btnSubmit.Enabled = (Me.pnlApprove.Visible AndAlso Me.chkApproveCampaign.Checked)

					Case DealerDirect.CampaignManagement.Programs.CampaignBuilder.CampaignBuildStatus.Failed
						m_PageStep = PageStep.BuildFailed

					Case DealerDirect.CampaignManagement.Programs.CampaignBuilder.CampaignBuildStatus.Submitted
						m_PageStep = PageStep.SubmitComplete

					Case Else
						m_PageStep = PageStep.Build
				End Select

				Me.pnlCreateCampaign.Visible = (m_PageStep = PageStep.Build OrElse m_PageStep = PageStep.BuildFailed)
				Me.imgPreparingCampaign.Visible = (m_PageStep = PageStep.Build)
				Me.tmrRefresh.Enabled = (m_PageStep = PageStep.Build)

				If m_PageStep = PageStep.Build Or m_PageStep = PageStep.BuildFailed Then
					Dim ibsi As DealerDirect.UserControls.CampaignManagement.Programs.IBuildStatusItem
					Dim ctrl As System.Web.UI.Control
					Dim sErrorResourceCode As String = Nothing

					Me.phBuildStatus.Controls.Clear()
					For Each bsi As DealerDirect.CampaignManagement.Programs.CampaignBuilder.BuildStatusItem In m_CampaignBuilder.getBuildLog
						ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/CampaignBuildStatusItem.ascx")
						ibsi = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IBuildStatusItem)
						ibsi.BuildStatusItem = bsi

						If bsi.ReturnMessageResourceCode IsNot Nothing Then sErrorResourceCode = bsi.ReturnMessageResourceCode

						Me.phBuildStatus.Controls.Add(ctrl)
					Next

					If sErrorResourceCode IsNot Nothing Then
						Me.lblErrorMessage.ForeColor = Drawing.Color.Red
						Me.lblErrorMessage.Text = DealerDirect.Resources.getString(sErrorResourceCode)
					Else
						Me.lblErrorMessage.Text = Nothing
					End If
				End If
			End If

			Me.ViewState("campaign_builder") = m_CampaignBuilder
		End Sub

		Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
			If DealerDirect.CampaignManagement.Payment.submitCampaign(m_CampaignID) Then
				m_PageStep = PageStep.SubmitComplete
				Response.Redirect("/cm/programs/r.aspx?m=c", True)
			Else
				m_PageStep = PageStep.SubmitFailed
				Response.Redirect("/cm/programs/r.aspx?m=f", True)
			End If
		End Sub

		Private Function generateOrderSummary() As Boolean
			Dim cosr As DealerDirect.CampaignManagement.Payment.CampaignOrderSummaryResult
			Dim tr As System.Web.UI.HtmlControls.HtmlTableRow
			Dim tc As System.Web.UI.HtmlControls.HtmlTableCell
			Dim nTotalQuantity As Integer
			Dim nTotalCost As Double
			Dim nTotalDiscount As Double = 0.0
			Dim sProofRoot As String

			cosr = DealerDirect.CampaignManagement.Payment.getCampaignOrderSummary(m_CampaignID)
			sProofRoot = DealerDirect.Configuration.Item("vdp_proof_url").Text1Value

			nTotalQuantity = 0
			nTotalCost = 0
			For Each oi As DealerDirect.CampaignManagement.Payment.OrderItem In cosr.OrderItems
				tr = New System.Web.UI.HtmlControls.HtmlTableRow

				' code
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerText = oi.Code
				tr.Cells.Add(tc)

				' name
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerHtml = oi.Name & String.Format(" (<a href=""{0}"" target=""_new"">{1}</a>)", sProofRoot & oi.ProofFileName, DealerDirect.Resources.getString("LITERAL_View_Proof"))
				tr.Cells.Add(tc)

				' lang
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerText = "EN"
				tc.Align = "center"
				tr.Cells.Add(tc)

				' quantity
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerText = oi.Quantity
				tc.Align = "center"
				tr.Cells.Add(tc)

				nTotalQuantity += oi.Quantity

				' unit price
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerText = String.Format("{0:C}", oi.UnitPrice)
				tc.Align = "right"
				tr.Cells.Add(tc)

				' total cost
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerText = String.Format("{0:C}", oi.TotalPrice)
				tc.Align = "right"
				tr.Cells.Add(tc)

				nTotalCost += oi.TotalPrice

				phOrderItems.Controls.Add(tr)
			Next

			' order total
			tr = New System.Web.UI.HtmlControls.HtmlTableRow

			' name
			tc = New System.Web.UI.HtmlControls.HtmlTableCell
			tc.InnerHtml = String.Format("<b>{0}</b>", DealerDirect.Resources.getStringColon("LITERAL_Total"))
			tc.ColSpan = 3
			tc.Align = "right"
			tr.Cells.Add(tc)

			' quantity
			tc = New System.Web.UI.HtmlControls.HtmlTableCell
			tc.InnerHtml = String.Format("<b>{0}</b>", nTotalQuantity)
			tc.Align = "center"
			tr.Cells.Add(tc)

			' blank (unit cost)
			tc = New System.Web.UI.HtmlControls.HtmlTableCell
			tr.Cells.Add(tc)
			tr.BorderColor = "black"

			' total cost
			tc = New System.Web.UI.HtmlControls.HtmlTableCell
			tc.InnerHtml = String.Format("<b>{0:C}</b>", nTotalCost)
			tc.Align = "right"
			tr.Cells.Add(tc)

			phTotal.Controls.Add(tr)

			' Find Total discount
			Dim allAccountAmounts As Double = 0
			For Each ai As DealerDirect.CampaignManagement.Payment.AccountItem In cosr.AccountItems.ValuesSorted
				allAccountAmounts = allAccountAmounts + ai.Amount
			Next
			nTotalDiscount = nTotalCost - allAccountAmounts

			If nTotalDiscount > 0 Then
				'Discount Row
				' order total
				tr = New System.Web.UI.HtmlControls.HtmlTableRow

				' name
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerHtml = "<b>" & DealerDirect.Resources.getStringColon("LITERAL_Total_Discounts") & "</b>"
				tc.ColSpan = 3
				tc.Align = "right"
				tr.Cells.Add(tc)

				' blank (quantity)
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tr.Cells.Add(tc)
				tr.BorderColor = "black"

				' blank (unit cost)
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tr.Cells.Add(tc)
				tr.BorderColor = "black"

				' total discount
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerHtml = String.Format("<b>{0:C}</b>", nTotalDiscount)
				tc.Align = "right"
				tr.Cells.Add(tc)

				phTotal.Controls.Add(tr)
			End If

			' account charges
			tr = New System.Web.UI.HtmlControls.HtmlTableRow
			For Each ai As DealerDirect.CampaignManagement.Payment.AccountItem In cosr.AccountItems.ValuesSorted
				tr = New System.Web.UI.HtmlControls.HtmlTableRow

				' name
				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerHtml = "<b>" & String.Format(DealerDirect.Resources.getStringColon("LITERAL_To_be_billed_to_{0}"), ai.Name) & "</b>"
				tc.ColSpan = 5
				tc.Align = "right"
				tr.Cells.Add(tc)

				tc = New System.Web.UI.HtmlControls.HtmlTableCell
				tc.InnerHtml = String.Format("<b>{0:C}</b>", ai.Amount)
				tc.Align = "right"
				tr.Cells.Add(tc)

				phAccountItems.Controls.Add(tr)
			Next

			Return (cosr.UnpaidAmount = 0)
		End Function

		Protected Sub tmrRefresh_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick
		End Sub
	End Class
End Namespace