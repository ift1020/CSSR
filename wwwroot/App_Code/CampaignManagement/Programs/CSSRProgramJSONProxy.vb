Namespace DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON
    Public Class Check
        Public ComponentCode As String
        Public ComponetID As Integer
        Public Status As String
        Public Title As String
    End Class

    'Public Class Format
    '    Public Title As String
    '    Public Indicator As String
    '    Public Price As String
    'End Class

    Public Class Sample
        Public SampleSetID As Integer
        Public Brand As String
        Public Channel As String
        Public Version As String
        Public Name As String
        Public Images As New System.Collections.Generic.List(Of SampleImage)
    End Class

    Public Class SampleImage
        Public OrderNumber As Integer
        Public Name As String
        Public Language As String
        Public URL As String
    End Class


    Public Class CSSRProgramSummary
        Public ProgramID As Integer
        Public SubCampaignID As Integer
        Public Status As String
        Public Title As String
        Public Description As String
        Public Enabled As Boolean
        Public Cadence As String
        Public Format As String

        'Public Formats As New System.Collections.Generic.List(Of Format)
        Public Checks As New System.Collections.Generic.List(Of Check)


        Public Shared Function loadAll(ByVal context_id As Int64) As System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary)
            Dim prgm As DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary
            Dim prgms As New System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary)
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim rdr As Devart.Data.Oracle.OracleDataReader

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnectionD
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_cssr.getCSSRPrograms"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("programs", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID

            rdr = cmd.ExecuteReader
            While rdr.Read
                prgm = New DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary(New DealerDirect.CampaignManagement.Programs.AudienceProgram(rdr("program_id")))
                prgms.Add(prgm)
            End While

            rdr.Close()
            rdr.Dispose()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return prgms
        End Function

        Public Sub New(ByVal program As DealerDirect.CampaignManagement.Programs.AudienceProgram)
            Me.ProgramID = program.ProgramID
            Me.SubCampaignID = program.SubCampaignID
            Me.Status = program.StatusText
            Me.Title = program.Name
            Me.Description = program.Description
            Me.Enabled = program.Enabled
            Me.Cadence = program.Cadence
            Me.Format = program.Format

            For i As Integer = 1 To program.ComponentCount
                Dim chk As New Check

                chk.ComponetID = i

                Select Case i
                    Case 1
                        chk.ComponentCode = "T"
                        chk.Title = DealerDirect.Resources.getString("LITERAL_Targeting")
                    Case 2
                        chk.ComponentCode = "O"
                        chk.Title = DealerDirect.Resources.getString("LITERAL_Offers")
                    Case 3
                        chk.ComponentCode = "C"
                        chk.Title = DealerDirect.Resources.getString("LITERAL_Creative")
                    Case 4
                        chk.ComponentCode = "S"
                        chk.Title = DealerDirect.Resources.getString("LITERAL_Services")
                End Select

                chk.Status = IIf(program.isComponentApproved(i), "Check", "Uncheck")

                Me.Checks.Add(chk)
            Next


            'Dim f As Format
            'f = New Format
            'f.Title = "eMail"
            'f.Indicator = "em"
            'f.Price = "Free"
            'Me.Formats.Add(f)

            'f = New Format
            'f.Title = "Letter"
            'f.Indicator = "lt"
            'f.Price = "$1.15"
            'Me.Formats.Add(f)

        End Sub
    End Class


    Public Class CSSRProgramDetails
        Inherits CSSRProgramSummary

        Public VariableSets As Programs.VDP.VariableSetCollection
        Public StyleGroups As New DealerDirect.CampaignManagement.Programs.Styles.StyleGroupCollection
        Public StyleGroupCode As String
        Public OfferGroups As New Programs.Offers.OfferGroupCollection
        Public Audiences As DealerDirect.CampaignManagement.Programs.AudienceCollection
        Public Samples As New System.Collections.Generic.List(Of Sample)

        Public Sub New(ByVal program As DealerDirect.CampaignManagement.Programs.AudienceProgram)
            MyBase.New(program)

            Me.VariableSets = program.VariableSets
            Me.StyleGroups = program.StyleGroups
            Me.StyleGroupCode = program.StyleGroupCode
            Me.OfferGroups = program.OfferGroups
            Me.Audiences = program.Audiences
        End Sub
    End Class

End Namespace
