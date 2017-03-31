Namespace DealerDirect.CampaignManagement.Programs.CSSR2017.ThankYou
	Public Module ThankYou
		Public Const PROGRAM_ID As Integer = 303
	End Module

	<Serializable()> _
	Public Class Program
		Inherits DealerDirect.CampaignManagement.Programs.AudienceProgram

		Public Sub New()
			MyBase.New(PROGRAM_ID)
		End Sub

		Public Sub New(ByVal noload As Boolean)
			MyBase.New(PROGRAM_ID, True)
		End Sub

        Public Overrides Function createPreview(ByVal language_code As String, ByVal channel_type_code As String, ByVal ptv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion) As Programs.VDP.Preview
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim p As Programs.VDP.Preview = Nothing
            Dim nTreatmentID As Integer
            Dim nTemplateID As Integer

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_p303.createPreviewTreatment"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_language_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_channel_type_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_style_group_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_target_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_preview_version_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("treatment_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("template_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = Me.m_SubCampaignID
            cmd.Parameters("c_language_cd").Value = language_code
            cmd.Parameters("c_channel_type_cd").Value = channel_type_code
            cmd.Parameters("c_style_group_cd").Value = ptv.StyleGroupCode
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


        Public Overrides Function getEstimateData() As System.Data.DataSet
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim da As Devart.Data.Oracle.OracleDataAdapter
            Dim ds As System.Data.DataSet

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = Data.createUserConnectionD
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_p303.getEstimateData"
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
    End Class
End Namespace
