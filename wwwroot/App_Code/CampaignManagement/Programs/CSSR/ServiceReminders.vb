Namespace DealerDirect.CampaignManagement.Programs.CSSR.ServiceReminders
	Public Module ServiceReminders
		Public Const PROGRAM_ID As Integer = 202
	End Module

	<Serializable()> _
	Public Class Program
		Inherits DealerDirect.CampaignManagement.Programs.AudienceProgram

		Protected m_ServiceGroups As New Programs.Specialized.SMSP.Services.ServiceGroupCollection
		Protected m_TireMessagingSettings As String

		Public Sub New()
			MyBase.New(ServiceReminders.PROGRAM_ID)
			'Me.loadServices()
		End Sub


		Public Overrides Function createPreview(ByVal language_code As String, ByVal channel_type_code As String, ByVal ptv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion) As Programs.VDP.Preview
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim p As Programs.VDP.Preview = Nothing
			Dim nTreatmentID As Integer
			Dim nTemplateID As Integer

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = DealerDirect.Data.createUserConnection
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_p202.createPreviewTreatment"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_language_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_channel_type_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_target_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_version_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_group_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_option_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_brand_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			'cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_additional_services_fl", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("treatment_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("template_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_sub_campaign_id").Value = Me.m_SubCampaignID
			cmd.Parameters("c_language_cd").Value = language_code
			cmd.Parameters("c_channel_type_cd").Value = channel_type_code
			cmd.Parameters("c_style_group_cd").Value = ptv.StyleGroupCode
			cmd.Parameters("c_style_option_cd").Value = ptv.StyleOptionCode
			cmd.Parameters("c_brand_cd").Value = ptv.Option1
			'cmd.Parameters("c_additional_services_fl").Value = ptv.Option2
			cmd.Parameters("c_preview_target_cd").Value = ptv.TargetCode
			cmd.Parameters("c_preview_version_cd").Value = ptv.VersionCode
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


		Public Overrides Function getPreviewTargets() As Programs.VDP.PreviewTargetCollection
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim ptc As Programs.VDP.PreviewTargetCollection
			Dim pt As Programs.VDP.PreviewTarget

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = DealerDirect.Data.createUserConnection
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_p202.getPreviewTargets"
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

		Public Overrides Function getPreviewVersions(ByVal preview_target_code As String, ByVal style_group_code As String) As Programs.VDP.PreviewTargetVersionCollection
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
			cmd.Connection = DealerDirect.Data.createUserConnection
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_p202.getPreviewVersions"
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

		Public Property TireMessagingSettings() As String
			Get
				Return m_TireMessagingSettings
			End Get
			Set(ByVal value As String)
				m_TireMessagingSettings = value
			End Set
		End Property

		Public Property ServiceGroups() As Programs.Specialized.SMSP.Services.ServiceGroupCollection
			Get
				Return m_ServiceGroups
			End Get
			Set(ByVal value As Programs.Specialized.SMSP.Services.ServiceGroupCollection)
				m_ServiceGroups = value
			End Set
		End Property
		Private Function loadServices() As Boolean
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim s As Programs.Specialized.SMSP.Services.Service

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = DealerDirect.Data.createUserConnection
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066_smsp.getServiceSettings"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("service_groups", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("service_group_services", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("service_options", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("n_program_id").Value = MyBase.ProgramID
			cmd.Parameters("n_sub_campaign_id").Value = MyBase.SubCampaignID

			rdr = cmd.ExecuteReader
			While rdr.Read
				Me.ServiceGroups.Add(New Programs.Specialized.SMSP.Services.ServiceGroup(rdr))
			End While

			rdr.NextResult()
			While rdr.Read
				s = New Programs.Specialized.SMSP.Services.Service(rdr)
				If m_ServiceGroups.Contains(s.ServiceGroupCode) Then
					m_ServiceGroups(s.ServiceGroupCode).Services.Add(s)
				End If
			End While

			rdr.NextResult()
			While rdr.Read
				If CStr(rdr("setting_cd")) = "TIRE_MESSAGING" Then
					Me.m_TireMessagingSettings = CStr(rdr("text_value"))
				End If
			End While

			rdr.Close()
			rdr.Dispose()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return True
		End Function
	End Class
End Namespace