Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class GlobalFilterList
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor

		Protected m_LoadedFilters As New System.Collections.Generic.List(Of DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
		Protected m_Program As DealerDirect.CampaignManagement.Programs.ProgramBase
		Protected m_TableColumnCount As Integer

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.Program
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
				m_Program = value
			End Set
		End Property

		Public ReadOnly Property TableColumnCount() As Integer
			Get
				Return m_TableColumnCount
			End Get
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements ISettingsEditor.loadData
			Dim fcl As System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.Filters.FilterConfig)
			Dim fcc As New DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt
			Dim fc As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig
			Dim tc As System.Web.UI.HtmlControls.HtmlTableCell
			Dim tr As System.Web.UI.HtmlControls.HtmlTableRow
			Dim ctrl As System.Web.UI.Control
			Dim nMaxColumn As Integer
			Dim nMaxRow As Integer
			Dim nRow As Integer
			Dim nCol As Integer
			Dim sVAlign As String

			If initial_load Then
				m_LoadedFilters.Clear()
				Me.phFilters.Controls.Clear()
			End If

            fcl = m_Program.FilterConfigs.Where(Function(f) f.ContainerTypeCode = "PRGM" AndAlso f.ShowFilter = True).ToList()
            If fcl.Count > 0 Then
				nMaxRow = fcl.Max(Function(f As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig) f.TableRow)
				nMaxColumn = fcl.Max(Function(f As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig) f.TableColumn)
				m_TableColumnCount = nMaxColumn * 2

				For r As Integer = 1 To nMaxRow
					tr = New System.Web.UI.HtmlControls.HtmlTableRow
					nRow = r

					For c As Integer = 1 To nMaxColumn
						nCol = c

						fc = fcl.Where(Function(f) f.TableRow = nRow And f.TableColumn = nCol).FirstOrDefault
						If fc IsNot Nothing Then
							tc = New System.Web.UI.HtmlControls.HtmlTableCell

							If m_Program.SetupFilterSet.Filters.ContainsKey(fc.Code) Then
								tc.InnerHtml = DealerDirect.Resources.getStringColon(m_Program.SetupFilterSet.Filters(fc.Code).Filter.NameResourceCode)
							Else
								tc.InnerHtml = fc.Code
							End If

							If fc.TableRowSpan > 1 Then
								tc.RowSpan = fc.TableRowSpan
								sVAlign = "top"
							Else
								sVAlign = "top"
							End If

							tc.Attributes.Add("width", CInt(30 / nMaxColumn) & "%")

							If c = nMaxColumn AndAlso r < nMaxRow Then
								tc.Attributes.Add("style", "text-align:right;border-left:solid 1px;border-bottom:solid 1px;border-color:#5397f0;")
							ElseIf c = nMaxColumn Then
								tc.Attributes.Add("style", "text-align:right;border-left:solid 1px;border-color:#5397f0;")
							ElseIf r < nMaxRow Then
								tc.Attributes.Add("style", "text-align:right;border-bottom:solid 1px;border-color:#5397f0;")
							Else
								tc.Attributes.Add("style", "text-align:right;")
							End If
							'tc.Attributes.Add("style", "text-align:right;vertical-align:" & sVAlign & "; font-weight:bold;padding-left:5px;padding-right:0px;padding-top:4px;padding-bottom:4px;border-bottom-width:1px;border-bottom-style:solid;border-color:#A8A8A8;")
							'tc.Attributes.Add("style", "font-weight: bold; padding: 0 20px 0 10px")

							tr.Cells.Add(tc)

							tc = New System.Web.UI.HtmlControls.HtmlTableCell
							tc.Attributes.Add("width", CInt(70 / nMaxColumn) & "%")

							If r < nMaxRow Then
								tc.Attributes.Add("style", "border-bottom:solid 1px;border-color:#5397f0;")
							End If

							'tc.Attributes.Add("style", "vertical-align:" & sVAlign & ";text-align:left;padding-left:0px;padding-right:5px;padding-top:0px;padding-bottom:1px;border-bottom-width:1px;border-bottom-style:solid;border-right-width:1px;border-right-style:solid;border-color:#A8A8A8;")

							ctrl = Page.LoadControl(If(fc.EditControl, "~/app_controls/cm/programs/generic/FilterPrompt.ascx"))
							ctrl.ID = "gfp" & fc.Code
							ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)


							If Not m_Program.SetupFilterSet.Filters.ContainsKey(fc.Code) Then
								m_Program.SetupFilterSet.Filters.Add(New DealerDirect.CampaignManagement.DataFilters.ContainerFilter(m_Program.SetupFilterSet.FilterSet.Filters(fc.Code)))
							End If

							ifp.ContainerFilter = m_Program.SetupFilterSet.Filters(fc.Code)

							ifp.FilterConfig = fc
							ifp.loadData(initial_load)
							m_LoadedFilters.Add(ifp)

							tc.Controls.Add(ctrl)
							If fc.TableRowSpan > 1 Then tc.RowSpan = fc.TableRowSpan
							If fc.TableColSpan > 1 Then tc.ColSpan = (fc.TableColSpan * 2) - 1

							tr.Cells.Add(tc)
						End If
					Next

					Me.phFilters.Controls.Add(tr)
				Next
			Else
				Me.Visible = False
			End If
		End Sub

		Public Function saveData() As Boolean Implements ISettingsEditor.saveData
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt

			For Each ctrl As System.Web.UI.Control In m_LoadedFilters
				If TypeOf ctrl Is DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt Then
					ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
					ifp.saveData()
				End If
			Next

			Return True
		End Function

		Public Function validateData() As Boolean Implements ISettingsEditor.validateData
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt

			For Each ctrl As System.Web.UI.Control In m_LoadedFilters
				If TypeOf ctrl Is DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt Then
					ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
					If Not ifp.validateData Then Return False
				End If
			Next

			Return True
		End Function

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Dim ifp As DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt

			m_Program.CurrentFilterData.Clear()

			For Each ctrl As System.Web.UI.Control In m_LoadedFilters
				If TypeOf ctrl Is DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt Then
					ifp = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt)
					m_Program.CurrentFilterData.Add(ifp.getCurrentData)
				End If
			Next
		End Sub
	End Class
End Namespace
