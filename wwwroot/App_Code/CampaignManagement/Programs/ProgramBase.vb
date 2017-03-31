Namespace DealerDirect.CampaignManagement.Programs
	Public Enum AudienceMode As Integer
		Split = 1
		Merged = 2
	End Enum

	Public Enum PricingDisplayMode As Integer
		Campaign = 0
		Audience = 1
		None = 2
	End Enum

	Public Enum ExecutionMode As Integer
		Batch = 1
		User = 2
	End Enum

	<Serializable()> _
	Public Class ProgramBase
		Protected m_ProgramID As Integer
		Protected m_SubCampaignID As Integer
		Protected m_Enabled As Boolean
		Protected m_OwnerContainer As DealerDirect.Containers.GenericContainer
		Protected m_NameResourceCode As String
		Protected m_DescResourceCode As String
		Protected m_ComponentApprovalMask As String
		Protected m_SetupFilterSetCode As String
		Protected m_SetupFilterSet As DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet
		Protected m_FilterConfigs As New DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection
		Protected m_CurrentFilterData As New DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection
		Protected m_AudienceModeCode As String
		Protected m_AudienceMode As AudienceMode = Programs.AudienceMode.Split
        Protected m_AudienceEstSourceCode As String
        Protected m_AudienceEstCadenceCode As String
        Protected m_AudienceEstScaleFactor As Double
        Protected m_CampaignExecutionMode As ExecutionMode
		Protected m_VariableSets As Programs.VDP.VariableSetCollection
		Protected m_PreviousInstance As Boolean
		Protected m_SameSourceSuppress As Boolean
		Protected m_EstimatesReady As Boolean = False
		Protected m_AverageDMailUnitPrice As Double?
		Protected m_AverageEMailUnitPrice As Double?

		Protected m_StyleGroups As New DealerDirect.CampaignManagement.Programs.Styles.StyleGroupCollection
		Protected m_StyleGroupCode As String

		Protected m_OfferGroups As New Programs.Offers.OfferGroupCollection
		Protected m_OfferTargets As New Programs.Offers.OfferTargetCollection

		Protected m_StatusTextValueListCode As String
		Protected m_PagePanelResourceCode As String

		Protected m_StartDate As System.DateTime
		Protected m_EndDate As System.DateTime?

		Protected m_BatchExecStartDate As System.DateTime?
		Protected m_BatchExecEndDate As System.DateTime?

		Protected m_FormatResourceCode As String
		Protected m_CadenceResourceCode As String

        Protected m_PricingDisplayMode As PricingDisplayMode

        Public Overridable Property PreviewGeneratorControlPath As String


        Public ReadOnly Property PricingDisplayMode() As PricingDisplayMode
			Get
				Return m_PricingDisplayMode
			End Get
		End Property

		Protected m_SeasonMappings As String

        Public Property SeasonMappings As String
            Get
                Return m_SeasonMappings
            End Get
            Set(value As String)
                m_SeasonMappings = value
            End Set
        End Property


#Region "Properties"
        Public ReadOnly Property StartDate() As System.DateTime
			Get
				Return m_StartDate
			End Get
		End Property

		Public ReadOnly Property EndDate() As System.DateTime?
			Get
				Return m_EndDate
			End Get
		End Property
		Public ReadOnly Property BatchExecStartDate() As System.DateTime?
			Get
				Return m_BatchExecStartDate
			End Get
		End Property
		Public ReadOnly Property BatchExecEndDate() As System.DateTime?
			Get
				Return m_BatchExecEndDate
			End Get
		End Property

		Public ReadOnly Property PagePanelResourceCode() As String
			Get
				Return m_PagePanelResourceCode
			End Get
		End Property

		Public ReadOnly Property PagePanelText() As String
			Get
				If m_PagePanelResourceCode IsNot Nothing Then
					Return DealerDirect.Resources.getString(m_PagePanelResourceCode)
				Else
					Return Nothing
				End If
			End Get
		End Property

		Public ReadOnly Property StatusTextValueListCode() As String
			Get
				Return m_StatusTextValueListCode
			End Get
		End Property

		Public ReadOnly Property AverageDMailUnitPrice() As Double?
			Get
				Return m_AverageDMailUnitPrice
			End Get
		End Property
		Public ReadOnly Property AverageEMailUnitPrice() As Double?
			Get
				Return m_AverageEMailUnitPrice
			End Get
		End Property

		Public ReadOnly Property hasPreviousInstance() As Boolean
			Get
				Return m_PreviousInstance
			End Get
		End Property

		Public ReadOnly Property SameSourceSuppress() As Boolean
			Get
				Return m_SameSourceSuppress
			End Get
		End Property

		Public ReadOnly Property allComponentsApproved() As Boolean
			Get
				Dim s As String = m_ComponentApprovalMask
				Dim b As Boolean = m_ComponentApprovalMask.IndexOf("N") < 0
				Return b
			End Get
		End Property

		Public Property StyleGroups() As DealerDirect.CampaignManagement.Programs.Styles.StyleGroupCollection
			Get
				Return m_StyleGroups
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Styles.StyleGroupCollection)
				m_StyleGroups = value
			End Set
		End Property
		Public Property StyleGroupCode() As String
			Get
				Return m_StyleGroupCode
			End Get
			Set(ByVal value As String)
				m_StyleGroupCode = value
			End Set
		End Property

		Public ReadOnly Property ProgramID() As Integer
			Get
				Return m_ProgramID
			End Get
		End Property
		Public ReadOnly Property SubCampaignID() As Integer
			Get
				Return m_SubCampaignID
			End Get
		End Property
		Public ReadOnly Property Enabled() As Boolean
			Get
				Return m_Enabled
			End Get
		End Property

		Public ReadOnly Property AudienceMode() As AudienceMode
			Get
				Return m_AudienceMode
			End Get
		End Property

		Public ReadOnly Property AudienceEstSourceCode() As String
			Get
				Return m_AudienceEstSourceCode
			End Get
		End Property

		Public ReadOnly Property CampaignExecutionMode() As ExecutionMode
			Get
				Return m_CampaignExecutionMode
			End Get
		End Property

Public ReadOnly Property Format() As String
			Get
				Return DealerDirect.Resources.getString(m_FormatResourceCode)
			End Get
		End Property

		Public ReadOnly Property Cadence() As String
			Get
				Return DealerDirect.Resources.getString(m_CadenceResourceCode)
			End Get
		End Property

		Public ReadOnly Property Name() As String
			Get
				If DealerDirect.Resources.doesStringExist(m_NameResourceCode) Then
					Return DealerDirect.Resources.getString(m_NameResourceCode)
				Else
					Return m_NameResourceCode
				End If
			End Get
		End Property

		Public ReadOnly Property Description() As String
			Get
				Return DealerDirect.Resources.getString(m_DescResourceCode)
			End Get
		End Property

		Public ReadOnly Property HasSetupFilterSet() As Boolean
			Get
				Return (m_SetupFilterSetCode IsNot Nothing)
			End Get
		End Property

		Public ReadOnly Property SetupFilterSetCode() As String
			Get
				Return m_SetupFilterSetCode
			End Get
		End Property

		Public Property SetupFilterSet() As DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet
			Get
				Return m_SetupFilterSet
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet)
				m_SetupFilterSet = value
			End Set
		End Property

		Public Property FilterConfigs() As DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection
			Get
				Return m_FilterConfigs
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection)
				m_FilterConfigs = value
			End Set
		End Property

		Public Property CurrentFilterData() As DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection
			Get
				Return m_CurrentFilterData
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection)
				m_CurrentFilterData = value
			End Set
		End Property

		Public ReadOnly Property StatusText() As String
			Get
				If Me.Enabled Then
					Return DealerDirect.Resources.getListItem(Me.StatusTextValueListCode, "Y")
				Else
					Return DealerDirect.Resources.getListItem(Me.StatusTextValueListCode, "N")
				End If
			End Get
		End Property
		Public ReadOnly Property StatusTextColor() As String
			Get
				Return IIf(Me.Enabled, "green", "red")
			End Get
		End Property

		Public ReadOnly Property VariableSets() As DealerDirect.CampaignManagement.Programs.VDP.VariableSetCollection
			Get
				Return m_VariableSets
			End Get
		End Property

		Public Property OfferGroups() As Programs.Offers.OfferGroupCollection
			Get
				Return m_OfferGroups
			End Get
			Set(ByVal value As Programs.Offers.OfferGroupCollection)
				m_OfferGroups = value
			End Set
		End Property
#End Region

		Public Sub New()
		End Sub

Public Sub New(ByVal program_id As Integer, Optional ByVal skip_load As Boolean = False)
			m_ProgramID = program_id
			If Not (skip_load) Then Me.loadProgramBase()

			If Me.SubCampaignID = 0 Then
				System.Web.HttpContext.Current.Response.Redirect("/cm/programs/error.aspx?e=notfound", True)
			End If

			If Not (skip_load) Then Programs.Styles.loadProgramStyles(Me)
			If Not (skip_load) Then Programs.Offers.loadProgramOffers(Me)
		End Sub


		Private Sub attachBaseProperties(ByVal irdr_on_row As System.Data.IDataReader)
			m_SubCampaignID = irdr_on_row("sub_campaign_id")
			m_Enabled = Utility.Data.FlagToBoolean(irdr_on_row("enabled_fl"))
			m_SetupFilterSetCode = Utility.Data.NothingIfNull(irdr_on_row("setup_filter_set_cd"))
			m_NameResourceCode = Utility.Data.NothingIfNull(irdr_on_row("name_resource_cd"))
			m_DescResourceCode = Utility.Data.NothingIfNull(irdr_on_row("desc_resource_cd"))
			m_ComponentApprovalMask = irdr_on_row("setup_component_approval_mask")
			m_PreviousInstance = Utility.Data.FlagToBoolean(irdr_on_row("prev_instance_fl"))
			m_SameSourceSuppress = Utility.Data.FlagToBoolean(irdr_on_row("same_source_suppress_fl"))

            m_StyleGroupCode = Utility.Data.NothingIfNull(irdr_on_row("style_group_cd"))

			If irdr_on_row("exec_mode_cd") = "USER" Then
				m_CampaignExecutionMode = ExecutionMode.User
			Else
				m_CampaignExecutionMode = ExecutionMode.Batch
			End If

			If irdr_on_row("pricing_display_mode_cd") = "AUD" Then
				m_PricingDisplayMode = Programs.PricingDisplayMode.Audience
			ElseIf irdr_on_row("pricing_display_mode_cd") = "NONE" Then
				m_PricingDisplayMode = Programs.PricingDisplayMode.None
			Else
				m_PricingDisplayMode = Programs.PricingDisplayMode.Campaign
			End If

			m_AudienceMode = AudienceMode.Split
			m_AudienceEstSourceCode = irdr_on_row("aud_est_quantity_source_cd")
			m_StatusTextValueListCode = irdr_on_row("status_text_value_list_cd")
			m_PagePanelResourceCode = Utility.Data.NothingIfNull(irdr_on_row("page_panel_resource_cd"))

			If irdr_on_row("avg_d_mail_unit_price") IsNot System.DBNull.Value Then m_AverageDMailUnitPrice = CDbl(irdr_on_row("avg_d_mail_unit_price"))
			If irdr_on_row("avg_e_mail_unit_price") IsNot System.DBNull.Value Then m_AverageEMailUnitPrice = CDbl(irdr_on_row("avg_e_mail_unit_price"))
			If irdr_on_row("start_dt") IsNot System.DBNull.Value Then m_StartDate = CDate(irdr_on_row("start_dt"))
			If irdr_on_row("end_dt") IsNot System.DBNull.Value Then m_EndDate = CDate(irdr_on_row("end_dt"))
			If irdr_on_row("batch_exec_start_dt") IsNot System.DBNull.Value Then m_BatchExecStartDate = CDate(irdr_on_row("batch_exec_start_dt"))
			If irdr_on_row("batch_exec_end_dt") IsNot System.DBNull.Value Then m_BatchExecEndDate = CDate(irdr_on_row("batch_exec_end_dt"))

			m_FormatResourceCode = Utility.Data.NothingIfNull(irdr_on_row("format_resource_cd"))
			m_CadenceResourceCode = Utility.Data.NothingIfNull(irdr_on_row("cadence_resource_cd"))
		End Sub

		Public Function loadProgramBase() As Boolean
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim vs As CampaignManagement.Programs.VDP.VariableSet
			Dim v As CampaignManagement.Programs.VDP.Variable

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_base.getProgramBase"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("prog_base", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("filters", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("variable_sets", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("variable_set_vars", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("variable_data", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("options", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = m_ProgramID

			rdr = cmd.ExecuteReader
			If rdr.Read Then
				' program base
				Me.attachBaseProperties(rdr)

				' filters
				rdr.NextResult()
				While rdr.Read
					m_FilterConfigs.Add(New DealerDirect.CampaignManagement.Programs.Filters.FilterConfig(rdr))
				End While

				If Me.HasSetupFilterSet Then
					m_SetupFilterSet = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet( _
					 New DealerDirect.Containers.GenericContainer("SBCPN", m_SubCampaignID), m_SetupFilterSetCode)
					m_SetupFilterSet.loadContainerFilters(m_SetupFilterSetCode)
				End If

				' variable sets
				rdr.NextResult()
				m_VariableSets = New CampaignManagement.Programs.VDP.VariableSetCollection
				While rdr.Read
					m_VariableSets.Add(New CampaignManagement.Programs.VDP.VariableSet(rdr))
				End While

				' variables
				rdr.NextResult()
				While rdr.Read
					If m_VariableSets.Contains(CInt(rdr("prog_variable_set_id"))) Then
						m_VariableSets(CInt(rdr("prog_variable_set_id"))).Variables.Add(New CampaignManagement.Programs.VDP.Variable(rdr))
					End If
				End While

				' variable data
				rdr.NextResult()
				While rdr.Read
					If m_VariableSets.Contains(CInt(rdr("prog_variable_set_id"))) Then
						vs = m_VariableSets(CInt(rdr("prog_variable_set_id")))

						If vs.Variables.Contains(CInt(rdr("prog_variable_id"))) Then
							v = vs.Variables(CInt(rdr("prog_variable_id")))
							v.attachData(rdr)
						End If
					End If
				End While

				' options
				rdr.NextResult()
				While rdr.Read
					If rdr("component_cd") = "STYLES" And rdr("setting_cd") = "SEASON_MAPPINGS" Then
						m_SeasonMappings = rdr("text_value")
					End If
				End While
			End If

			rdr.Close()
			rdr.Dispose()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return True
		End Function

		Protected Function SaveBase() As Boolean
			Dim cmd As Devart.Data.Oracle.OracleCommand

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_base.saveProgramSettings"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_enabled_fl", Devart.Data.Oracle.OracleDbType.Char)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_setup_comp_approval_mask", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.Parameters("c_enabled_fl").Value = Utility.Data.BooleanToFlag(Me.m_Enabled)
			cmd.Parameters("c_setup_comp_approval_mask").Value = m_ComponentApprovalMask
			cmd.ExecuteNonQuery()

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return True
		End Function

		Public Function setProgramStatus(ByVal active As Boolean) As Boolean
			If active Then : m_Enabled = (Not m_ComponentApprovalMask.Contains("N"))
			Else : m_Enabled = active : End If

			Me.SaveBase()

			Return m_Enabled
		End Function
		Public Sub setComponentApproval(ByVal component_index As Integer, ByVal approval As Boolean)
			Dim s As String = ""
			Dim ca As Char()

			ca = m_ComponentApprovalMask.ToCharArray()
			ca.SetValue(IIf(approval, "Y", "N").ToString.Chars(0), component_index - 1)

			For i As Integer = 0 To ca.Length - 1 : s &= ca(i) : Next

			m_ComponentApprovalMask = s
			If Not approval Then m_Enabled = False

			Me.SaveBase()
		End Sub
		Public ReadOnly Property isComponentApproved(ByVal component_index As Integer) As Boolean
            Get
                ' added by RayL on 2017/03/08, only when 208 without offer tab
                If m_ProgramID = 208 And component_index = 3 Then
                    component_index = 2
                End If

                Return m_ComponentApprovalMask.Substring(component_index - 1, 1) = "Y"
            End Get
        End Property

		Public ReadOnly Property ComponentCount As Integer
			Get
				Return m_ComponentApprovalMask.Length
			End Get
		End Property

		Public Function getVariableSetVariable(ByVal variable_set_id As Integer, ByVal variable_id As Integer) As DealerDirect.CampaignManagement.Programs.VDP.Variable
			Dim vs As CampaignManagement.Programs.VDP.VariableSet
			Dim v As CampaignManagement.Programs.VDP.Variable = Nothing

			If m_VariableSets.Contains(variable_set_id) Then
				vs = m_VariableSets(variable_set_id)
				If vs.Variables.Contains(variable_id) Then v = vs.Variables(variable_id)
			End If

			Return v
		End Function

		Public Function setGenericSetting(ByVal component_code As String, ByVal setting_code As String, ByVal sequence_nbr As Integer, ByVal text_value As String, ByVal number_value As Double?) As Boolean
			Dim cmd As Devart.Data.Oracle.OracleCommand

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_base.saveGenericSetting"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_component_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_setting_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sequence_nbr", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_text_value", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_number_value", Devart.Data.Oracle.OracleDbType.Number)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.Parameters("c_component_cd").Value = component_code
			cmd.Parameters("c_setting_cd").Value = setting_code
			cmd.Parameters("n_sequence_nbr").Value = sequence_nbr
			cmd.Parameters("c_text_value").Value = Utility.Data.NullIfNothing(text_value)
			If number_value.HasValue Then cmd.Parameters("n_number_value").Value = number_value.Value Else cmd.Parameters("n_number_value").Value = System.DBNull.Value
			cmd.ExecuteNonQuery()

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return True
		End Function

		Public Function getGenericSettingText(ByVal component_code As String, ByVal setting_code As String, ByVal sequence_nbr As Integer) As String
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim sValue As String

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_base.getGenericSetting"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_component_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_setting_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sequence_nbr", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("text_value", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("number_value", Devart.Data.Oracle.OracleDbType.Number)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.Parameters("c_component_cd").Value = component_code
			cmd.Parameters("c_setting_cd").Value = setting_code
			cmd.Parameters("n_sequence_nbr").Value = sequence_nbr
			cmd.ExecuteNonQuery()

			If cmd.Parameters("text_value").Value IsNot System.DBNull.Value Then
				sValue = cmd.Parameters("text_value").Value.ToString
			End If

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return sValue
		End Function

		Public Function setStyleGroup(ByVal style_group_code As String) As Boolean
			Dim cmd As Devart.Data.Oracle.OracleCommand

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_base.saveProgramStyleGroup"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_group_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.Parameters("c_style_group_cd").Value = Utility.Data.NullIfNothing(style_group_code)
			cmd.ExecuteNonQuery()

			m_StyleGroupCode = style_group_code

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return True
		End Function

		Public Function createCampaign() As Integer
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim nCampaignID As Integer

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_exec.createInstance"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("owner_container_type_cd", Devart.Data.Oracle.OracleDbType.VarChar, 4)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("owner_container_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("sub_instance_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_program_id").Value = m_ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.ExecuteNonQuery()

			nCampaignID = cmd.Parameters("campaign_id").Value

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return nCampaignID
		End Function

		Public ReadOnly Property EstimatesReady() As Boolean
			Get
				If Me.AudienceEstSourceCode = "STATIC" Then
					m_EstimatesReady = True

				ElseIf Not m_EstimatesInitialized Then
					Me.initEstimates()
					m_EstimatesReady = False

				Else
					Dim cmd As Devart.Data.Oracle.OracleCommand
					Dim sReturn As String

					cmd = New Devart.Data.Oracle.OracleCommand
					cmd.Connection = Data.createUserConnectionD
					cmd.CommandType = System.Data.CommandType.StoredProcedure
					cmd.CommandText = "wsdp066_core_exec.isEstimatesDataReady"
					cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
					cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
					cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
					cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("data_ready_fl", Devart.Data.Oracle.OracleDbType.Char, 1)).Direction = System.Data.ParameterDirection.Output
					cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
					cmd.Parameters("n_program_id").Value = m_ProgramID
					cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
					cmd.ExecuteNonQuery()

					sReturn = cmd.Parameters("data_ready_fl").Value.ToString.Trim

					cmd.Connection.Close()
					cmd.Connection.Dispose()
					cmd.Dispose()

					m_EstimatesReady = Utility.Data.FlagToBoolean(sReturn)
				End If

				Return m_EstimatesReady
			End Get
		End Property

	
		Protected m_EstimatesInitialized As Boolean = False

		Public Overridable Sub initEstimates()
			Dim cmd As Devart.Data.Oracle.OracleCommand

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_exec.initEstimatesA"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = m_ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			cmd.ExecuteNonQuery()

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			m_EstimatesInitialized = True
		End Sub

		Public Overridable Function getEstimateData() As System.Data.DataSet
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim da As Devart.Data.Oracle.OracleDataAdapter
			Dim ds As System.Data.DataSet

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_exec.getEstimateData"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("estimate_data", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("filter_mappings", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = m_ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID

			da = New Devart.Data.Oracle.OracleDataAdapter(cmd)
			ds = New System.Data.DataSet
			da.Fill(ds)

			da.Dispose()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			System.Threading.Thread.Sleep(2)

			Return ds
		End Function


		Public Overridable Function getPreviewTargets() As Programs.VDP.PreviewTargetCollection	' System.Collections.Generic.IList(Of DealerDirect.CampaignManagement.Programs.VDP.PreviewTarget)
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim ptc As Programs.VDP.PreviewTargetCollection
			Dim pt As Programs.VDP.PreviewTarget

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_vdp.getPreviewTargets"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("preview_targets", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = m_ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
			rdr = cmd.ExecuteReader

			ptc = New Programs.VDP.PreviewTargetCollection
			While rdr.Read
				pt = New Programs.VDP.PreviewTarget(rdr)
				ptc.Add(pt)
			End While

			rdr.Close()
			rdr.Dispose()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return ptc
		End Function

		Public Overridable Function getPreviewVersions(ByVal preview_target_code As String, ByVal style_group_code As String) As Programs.VDP.PreviewTargetVersionCollection
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim pvc As Programs.VDP.PreviewTargetVersionCollection
			Dim pv As Programs.VDP.PreviewTargetVersion
			Dim sStyleGroupCode As String

			If style_group_code.Length = 0 AndAlso m_StyleGroups.Count = 1 Then
				sStyleGroupCode = m_StyleGroups(0).Code
			Else
				sStyleGroupCode = style_group_code
			End If

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_core_vdp.getPreviewVersions"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_target_cd", Devart.Data.Oracle.OracleDbType.VarChar, 32)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_group_cd", Devart.Data.Oracle.OracleDbType.VarChar, 10)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("preview_versions", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = Me.m_ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = Me.m_SubCampaignID
			cmd.Parameters("c_preview_target_cd").Value = preview_target_code
			cmd.Parameters("c_style_group_cd").Value = Utility.Data.NothingIfNull(sStyleGroupCode)

			rdr = cmd.ExecuteReader

			pvc = New Programs.VDP.PreviewTargetVersionCollection
			While rdr.Read
				pv = New Programs.VDP.PreviewTargetVersion(rdr)
				pvc.Add(pv)
			End While

			rdr.Close()
			rdr.Dispose()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return pvc
		End Function

		Public Overridable Function createPreview(ByVal language_code As String, ByVal channel_type_code As String, ByVal pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion) As Programs.VDP.Preview
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim p As Programs.VDP.Preview = Nothing
			Dim nTreatmentID As Integer
			Dim nTemplateID As Integer

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = DealerDirect.Data.createUserConnectionD
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "gmclwsd2.wsdp066_core_vdp_dev.createPreviewTreatment"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_language_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_channel_type_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_group_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_target_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_version_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("treatment_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("template_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = Me.ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = Me.SubCampaignID
			cmd.Parameters("c_language_cd").Value = language_code
			cmd.Parameters("c_channel_type_cd").Value = channel_type_code
			cmd.Parameters("c_style_group_cd").Value = pv.StyleGroupCode
			cmd.Parameters("c_preview_target_cd").Value = pv.TargetCode
			cmd.Parameters("c_preview_version_cd").Value = pv.VersionCode
			cmd.ExecuteNonQuery()

			nTreatmentID = cmd.Parameters("treatment_id").Value.ToString
			If nTreatmentID > 0 Then
				nTemplateID = cmd.Parameters("template_id").Value.ToString
				p = New Programs.VDP.Preview(nTreatmentID, nTemplateID, language_code, channel_type_code)
			End If

			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return p
		End Function
	End Class
End Namespace