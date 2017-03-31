Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class FilterPrompt
		Inherits System.Web.UI.UserControl
		Implements DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt

		Protected m_ContainerFilter As DealerDirect.CampaignManagement.DataFilters.ContainerFilter
		Protected m_FilterConfig As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig
		Protected m_ErrorCode As String

		Public Property Program() As DealerDirect.CampaignManagement.Programs.ProgramBase Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.Program
			Get
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.ProgramBase)
			End Set
		End Property

		Public Property ContainerFilter() As DealerDirect.CampaignManagement.DataFilters.ContainerFilter Implements DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt.ContainerFilter
			Get
				Return m_ContainerFilter
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.DataFilters.ContainerFilter)
				m_ContainerFilter = value
			End Set
		End Property

		Public Property FilterConfig() As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig Implements DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt.FilterConfig
			Get
				Return m_FilterConfig
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig)
				m_FilterConfig = value
			End Set
		End Property

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		End Sub

		Public Sub loadData(ByVal initial_load As Boolean) Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.loadData
			Dim sel As System.Collections.Generic.IEnumerable(Of DealerDirect.CampaignManagement.DataFilters.IContainerFilterValueListItem)
			Dim sKey As String
			Dim sOption As String
			Dim sPrompt As String

			sPrompt = m_FilterConfig.getOption("prompt-type")
			Me.cblMain.Visible = (m_ContainerFilter.Format = "L" AndAlso (sPrompt = "CBL" Or sPrompt Is Nothing))
			Me.rblMain.Visible = (m_ContainerFilter.Format = "V")
			Me.ddlMain.Visible = (m_ContainerFilter.Format = "L" AndAlso sPrompt = "DDL")

			If Not m_ContainerFilter.ValueListLoaded Then m_ContainerFilter.loadValueList(AddressOf ValueListLoader, AddressOf DataItemListCoder)

			Select Case m_ContainerFilter.Format
				Case "L"
					If Me.cblMain.Visible Then
						Me.cblMain.DataSource = m_ContainerFilter.ValueList.Values.OrderBy(Function(f As DealerDirect.CampaignManagement.DataFilters.IContainerFilterValueListItem) f.DisplayOrderNumber)
						Me.cblMain.DataTextField = "description"
						Me.cblMain.DataValueField = "code"
						Me.cblMain.DataBind()
						Me.cblMain.Enabled = m_FilterConfig.AllowEdit

						sOption = m_FilterConfig.getOption("cbl-repeatdirection")
						If sOption IsNot Nothing Then
							Select Case sOption.ToUpper
								Case "H"
									Me.cblMain.RepeatDirection = RepeatDirection.Horizontal
								Case "V"
									Me.cblMain.RepeatDirection = RepeatDirection.Vertical
							End Select
						End If

						sOption = m_FilterConfig.getOption("reqd-option")
						Dim slRequiredOptions As New System.Collections.Specialized.StringCollection
						If sOption IsNot Nothing Then
							For Each s As String In sOption.Split(",")
								slRequiredOptions.Add(s)
							Next
						End If

						sel = m_ContainerFilter.ValueList.Values.Where(Function(f) f.Selected = True)
						For Each li As System.Web.UI.WebControls.ListItem In Me.cblMain.Items
							sKey = li.Value
							If sel.Count(Function(f) f.Code = sKey) > 0 Then
								li.Selected = True
							End If

							If slRequiredOptions.Contains(li.Value) Then
								li.Enabled = False
								li.Selected = True
							End If
						Next

					ElseIf Me.ddlMain.Visible Then

					End If

				Case "V"
					Me.rblMain.DataSource = m_ContainerFilter.ValueList.Values.OrderBy(Function(f As DealerDirect.CampaignManagement.DataFilters.IContainerFilterValueListItem) f.DisplayOrderNumber)
					Me.rblMain.DataTextField = "description"
					Me.rblMain.DataValueField = "code"
					Me.rblMain.DataBind()
					Me.rblMain.Enabled = m_FilterConfig.AllowEdit

					sOption = m_FilterConfig.getOption("rbl-repeatdirection")
					If sOption IsNot Nothing Then
						Select Case sOption.ToUpper
							Case "H"
								Me.rblMain.RepeatDirection = RepeatDirection.Horizontal
							Case "V"
								Me.rblMain.RepeatDirection = RepeatDirection.Vertical
						End Select
					End If

					Dim o As Object = m_ContainerFilter.ValueList

					sel = m_ContainerFilter.ValueList.Values.Where(Function(f As DealerDirect.CampaignManagement.DataFilters.IContainerFilterValueListItem) f.Selected = True)
					If sel.Count > 0 Then
						Me.rblMain.SelectedValue = sel.FirstOrDefault.Code
					Else
						Me.rblMain.SelectedValue = m_ContainerFilter.ValueList.Values.OrderBy(Function(f) f.DisplayOrderNumber).FirstOrDefault.Code
					End If
			End Select
		End Sub

		Public Function saveDataToObject() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.saveData
			Dim fdic As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItemCollection
			Dim fdi As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem
			Dim idx As Integer = 1

			' Clear the previous selections
			fdic = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItemCollection

			Select Case m_ContainerFilter.Format
				Case "L"
					For Each li As System.Web.UI.WebControls.ListItem In Me.cblMain.Items
						If li.Selected Then
							fdi = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem(idx)

							Select Case m_ContainerFilter.Filter.DataType
								Case "T"
									fdi.Text1Value = li.Value
								Case "N"
									fdi.Number1Value = li.Value
							End Select

							fdic.Add(fdi)
							idx += 1
						End If
					Next

				Case "V"
					For Each li As System.Web.UI.WebControls.ListItem In Me.rblMain.Items
						If li.Selected Then
							fdi = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem(idx)

							Select Case m_ContainerFilter.Filter.DataType
								Case "T"
									fdi.Text1Value = li.Value
								Case "N"
									fdi.Number1Value = li.Value
							End Select

							fdic.Add(fdi)
							idx += 1
						End If
					Next
			End Select
			m_ContainerFilter.DataItems = fdic
		End Function

		Public Function getCurrentData() As DealerDirect.CampaignManagement.DataFilters.ContainerFilter Implements DealerDirect.UserControls.CampaignManagement.Programs.IFilterPrompt.getCurrentData
			Dim fdic As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItemCollection
			Dim fdi As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem
			Dim idx As Integer = 1
			Dim fdc As DealerDirect.CampaignManagement.DataFilters.ContainerFilter

			' Clear the previous selections
			fdic = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItemCollection

			Select Case m_ContainerFilter.Format
				Case "L"
					For Each li As System.Web.UI.WebControls.ListItem In Me.cblMain.Items
						If li.Selected Then
							fdi = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem(idx)

							Select Case m_ContainerFilter.Filter.DataType
								Case "T"
									fdi.Text1Value = li.Value
								Case "N"
									fdi.Number1Value = li.Value
							End Select

							fdic.Add(fdi)
							idx += 1
						End If
					Next

				Case "V"
					For Each li As System.Web.UI.WebControls.ListItem In Me.rblMain.Items
						If li.Selected Then
							fdi = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem(idx)

							Select Case m_ContainerFilter.Filter.DataType
								Case "T"
									fdi.Text1Value = li.Value
								Case "N"
									fdi.Number1Value = li.Value
							End Select

							fdic.Add(fdi)
							idx += 1
						End If
					Next
			End Select

			fdc = New DealerDirect.CampaignManagement.DataFilters.ContainerFilter(m_ContainerFilter.Filter, _
				m_ContainerFilter.Format, m_ContainerFilter.CompareOp, m_ContainerFilter.ID, _
				m_ContainerFilter.OptionalConditionEnabled, fdic)

			Return fdc
		End Function

		Public Function validateData() As Boolean Implements DealerDirect.UserControls.CampaignManagement.Programs.ISettingsEditor.validateData
			Dim bValid As Boolean = True
			Dim sOption As String

			sOption = m_FilterConfig.getOption("min-items")
			If sOption IsNot Nothing Then
				Dim nMinItems As Integer = sOption
				Dim nItems As Integer = 0

				Select Case m_ContainerFilter.Format
					Case "L"
						For Each li As System.Web.UI.WebControls.ListItem In Me.cblMain.Items
							If li.Selected Then nItems += 1
						Next

					Case "V"
						For Each li As System.Web.UI.WebControls.ListItem In Me.rblMain.Items
							If li.Selected Then nItems += 1
						Next
				End Select

				If bValid Then
					bValid = nItems >= nMinItems
				End If

				If Not bValid Then
					Me.m_ErrorCode = "NOT_ENOUGH_ITEMS"
					Me.lblError.Text = DealerDirect.Resources.getString("LITERAL_Required")
				End If
			End If

			Return bValid
		End Function

		Public Function ValueListLoader(ByRef irdr As System.Data.IDataReader) As DealerDirect.CampaignManagement.DataFilters.ContainerFilterValueListItemCollection
			Dim vlic As New DealerDirect.CampaignManagement.DataFilters.ValueListItemCollection
			Dim slColumns As System.Collections.SortedList
			Dim sDisplayOrderColumnName As String = Nothing
			Dim bIsOrdered As Boolean = False
			Dim nDisplayOrderNumber As Integer

			slColumns = New System.Collections.SortedList
			For i As Integer = 0 To irdr.FieldCount - 1
				slColumns.Add(irdr.GetName(i).ToLower, i)
			Next

			If slColumns.Contains("order_nbr") Then
				bIsOrdered = True
				sDisplayOrderColumnName = "order_nbr"
			End If

			If slColumns.Contains("resource_cd") Then
				Dim sCode As String = ""

				While irdr.Read
					If bIsOrdered Then
						nDisplayOrderNumber = Utility.Data.ZeroIfNull(irdr(sDisplayOrderColumnName))
					Else
						nDisplayOrderNumber = 0
					End If

					If sCode <> irdr("code").ToString Then
						If irdr("resource_cd") Is System.DBNull.Value Then
							vlic.Add(New DealerDirect.CampaignManagement.DataFilters.ValueListItem(irdr("code").ToString, Nothing, irdr("description").ToString, nDisplayOrderNumber))
						Else
							vlic.Add(New DealerDirect.CampaignManagement.DataFilters.ValueListItem(irdr("code").ToString, irdr("resource_cd").ToString, nDisplayOrderNumber))
						End If
					End If

					sCode = irdr("code").ToString
				End While

			Else ' use explicit columns
				Dim sLang As String = DealerDirect.Security.CurrentSession.Language.ToLower
				Dim sDescColumn As String = Nothing

				For Each s As String In slColumns.Keys
					If s.EndsWith("_" & sLang) Then
						sDescColumn = s
						Exit For
					End If
				Next

				While irdr.Read
					If bIsOrdered Then
						nDisplayOrderNumber = Utility.Data.ZeroIfNull(irdr(sDisplayOrderColumnName))
					Else
						nDisplayOrderNumber = 0
					End If

					If Not (sDescColumn Is Nothing) Then
						vlic.Add(New DealerDirect.CampaignManagement.DataFilters.ValueListItem(irdr("code").ToString, Nothing, irdr(sDescColumn).ToString, nDisplayOrderNumber))
					Else
						vlic.Add(New DealerDirect.CampaignManagement.DataFilters.ValueListItem(irdr("code").ToString, Nothing, irdr("code").ToString, nDisplayOrderNumber))
					End If
				End While
			End If

			Return vlic
		End Function

		Public Function DataItemListCoder(ByVal data_items As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItemCollection) As System.Collections.Hashtable
			Dim ht As System.Collections.Hashtable

			ht = New System.Collections.Hashtable
			For i As Integer = 0 To data_items.Count - 1
				Select Case m_ContainerFilter.Filter.DataType
					Case "T"
						ht.Add(data_items(i).Text1Value, True)
					Case "N"
						ht.Add(CStr(data_items(i).Number1Value), True)
					Case "D"
						ht.Add(data_items(i).Date1Value, True)
				End Select
			Next
			Return ht
		End Function
	End Class
End Namespace