Namespace DealerDirect.CampaignManagement.Programs.CSSR

	Public Class Helper
		Public Shared Sub x()

		End Sub

		Public Shared Sub updateComponentStatusVisuals(ByVal rbtn As Telerik.Web.UI.RadButton, ByVal approved As Boolean)
			Select Case approved
				Case True
					rbtn.Icon.PrimaryIconUrl = "/resources/images/cssr/cssr_status_okay_16.png"
					rbtn.Text = DealerDirect.Resources.getString("LITERAL_Complete_(Click_to_Edit)")
				Case Else
					rbtn.Icon.PrimaryIconUrl = "/resources/images/cssr/cssr_status_error_16.png"
					rbtn.Text = DealerDirect.Resources.getString("LITERAL_Editing_(Click_to_Save)")
			End Select
		End Sub

		Public Shared Function getProgramStatus() As CSSRProgramCollection
			Dim cmd As Devart.Data.Oracle.OracleCommand
			Dim rdr As Devart.Data.Oracle.OracleDataReader
			Dim cpc As CSSRProgramCollection

			cmd = New Devart.Data.Oracle.OracleCommand
			cmd.Connection = DealerDirect.Data.createUserConnection
			cmd.CommandType = System.Data.CommandType.StoredProcedure
			cmd.CommandText = "wsdp066.getProgramGroupStatus"
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_program_group_cd", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
			cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("program_group_status", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
			cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
			cmd.Parameters("c_program_group_cd").Value = "CSSR"

			cpc = New CSSRProgramCollection

			rdr = cmd.ExecuteReader
			While rdr.Read
				cpc.Add(New CSSRProgram With {.ProgramID = rdr("program_id"), .NameResourceCode = rdr("name_resource_cd"), .StatusCode = rdr("status_cd")})
			End While

			rdr.Close()
			cmd.Connection.Close()
			cmd.Connection.Dispose()
			cmd.Dispose()

			Return cpc
		End Function
	End Class




	Public Class CSSRProgram
		Public ProgramID As Integer
		Public NameResourceCode As String
		Public StatusCode As String
	End Class
	Public Class CSSRProgramCollection
		Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, CSSRProgram)

		Protected Overrides Function GetKeyForItem(item As CSSRProgram) As Integer
			Return item.ProgramID
		End Function
	End Class

End Namespace
