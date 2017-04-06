Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Namespace DealerDirect.WebServices.CampaignManagement
    <ServiceContract(Namespace:="DealerDirect.WebServices.CampaignManagement")>
    <AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
    Public Class CSSR

        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(CSSR))

        ' To use HTTP GET, add <WebGet()> attribute. (Default ResponseFormat is WebMessageFormat.Json)
        ' To create an operation that returns XML,
        '     add <WebGet(ResponseFormat:=WebMessageFormat.Xml)>,
        '     and include the following line in the operation body:
        '         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml"
        ' To use HTTP POST, add <WebInvoke()> attribute.

        <OperationContract()>
        <WebInvoke(ResponseFormat:=WebMessageFormat.Json)>
        Public Function testDemo(ByVal dealerID As Int32, ByVal userID As Int32) As Object
            Dim result As String = ""

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim sl As System.Collections.SortedList = New System.Collections.SortedList
                sl.Add("Code", "Success")
                sl.Add("Message", "Web Service test successfuuly")

                result = New JavaScriptSerializer().Serialize(sl)

                'result = Utility.WebUtils.GetJson(dt)
            End If

            Return result
        End Function

        ' Add more operations here and mark them with <OperationContract()>

        <OperationContract()>
        <WebInvoke(ResponseFormat:=WebMessageFormat.Json)>
        Public Function getSummary(ByVal dealerID As Int32, ByVal userID As Int32) As Object
            Dim result As String = ""

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim prgms As New List(Of DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary)

                prgms = DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary.loadAll(DealerDirect.Security.CurrentSession.SecurityContext.ID)

                result = New JavaScriptSerializer().Serialize(prgms)
            End If

            Return result
        End Function

        <OperationContract()>
        <WebInvoke(ResponseFormat:=WebMessageFormat.Json)>
        Public Function getNews(ByVal dealerID As Int32, ByVal userID As Int32) As Object
            Dim result As String = ""

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim nil As New System.Collections.Generic.List(Of CSSRNewsItem)
                Dim cmd As Devart.Data.Oracle.OracleCommand
                Dim rdr As Devart.Data.Oracle.OracleDataReader
                Dim ni As CSSRNewsItem
                Dim iter As Integer = 0

                cmd = New Devart.Data.Oracle.OracleCommand
                cmd.Connection = DealerDirect.Data.createUserConnectionD
                cmd.CommandType = System.Data.CommandType.StoredProcedure
                cmd.CommandText = "gmclwsd2.wsdp066_p200.getRecentRuns"
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("recent_runs", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
                cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID

                rdr = cmd.ExecuteReader
                While rdr.Read
                    ni = New CSSRNewsItem
                    ni.ContentType = rdr("content_type_cd")
                    ni.ContentDate = String.Format("{0:MMMM d, yyyy}", rdr("content_dt"))
                    ni.ContentText = DealerDirect.Resources.getString(rdr("program_name_resource_cd")) & " - " & rdr("run_qty") & " contacts sent"
                    ni.SortOrder = iter
                    ni.DetailURL = "leads.aspx?blah=123"
                    nil.Add(ni)
                    iter += 1
                End While

                rdr.Close()
                rdr.Dispose()
                cmd.Connection.Close()
                cmd.Connection.Dispose()
                cmd.Dispose()

                result = New JavaScriptSerializer().Serialize(nil)
            End If

            Return result
        End Function

        <OperationContract()>
        <WebInvoke>
        Public Function toggleProgramStatus(ByVal dealerID As Int32, ByVal userID As Int32, ByVal programID As Int32) As Boolean
            Dim m_Enabled As Boolean = False

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim m_Program As New DealerDirect.CampaignManagement.Programs.ProgramBase(programID)

                m_Enabled = m_Program.setProgramStatus(Not m_Program.Enabled)
            End If

            Return m_Enabled
        End Function

        <OperationContract()>
        <WebInvoke>
        Public Function getPreviewUrl(ByVal dealerID As Int32, ByVal userID As Int32, ByVal requestID As Int32, ByVal channel As String) As String
            Dim m_Url As String = ""

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim nRequestID As Integer = requestID
                Dim sChannel As String = channel
                Dim sFileName As String = System.Guid.NewGuid.ToString ' & ".pdf"

                Select Case sChannel.ToUpper
                    Case "D"
                        sFileName &= ".pdf"
                    Case "E"
                        sFileName &= ".html"
                End Select

                If Not System.IO.File.Exists(System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, sFileName)) Then
                    CM2.GMCL.DealerDirect.VDP.Client.Rendering.saveRequestOutput(nRequestID, System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, sFileName))
                End If

                m_Url = DealerDirect.Configuration.Item("vdp_output_url").Text1Value & sFileName
            End If

            Return m_Url
        End Function

        <OperationContract()>
        <WebInvoke>
        Public Function createpreview(ByVal dealerID As Int32, ByVal userID As Int32,
                                      ByVal subCampaignID As Int32, ByVal programID As Int32,
                                      ByVal styleGroup As String, ByVal language As String,
                                      ByVal channel As String, ByVal version As String,
                                      ByVal target As String, ByVal opt As String) As String
            Dim m_Result As String = "0"

            If DealerDirect.Utilities.WebServices.authenticatedRequest(dealerID, userID) Then
                Dim p As DealerDirect.CampaignManagement.Programs.AudienceProgram
                Dim nSubCampaignID As Integer = subCampaignID
                Dim nProgramID As Integer = programID
                Dim sStyleGroup As String = styleGroup
                Dim sLanguage As String = language
                Dim sChannel As String = channel
                Dim sVersion As String = version
                Dim sTarget As String = target
                Dim sOption As String = opt
                Dim prv As DealerDirect.CampaignManagement.Programs.VDP.Preview

                Select Case nProgramID
                    Case 201
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.WelcomeNew.Program
                    Case 202
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.ServiceReminders.Program
                    Case 203
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.ThankYou.Program
                    Case 204
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.Anniversary.Program
                    Case 205
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.DexCool.Program
                    Case 206
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.Warranty.Program
                    Case 207
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.Lease.Program
                    Case 208
                        p = New DealerDirect.CampaignManagement.Programs.CSSR.Finance.Program
                    Case 303
                        p = New DealerDirect.CampaignManagement.Programs.CSSR2017.ThankYou.Program
                    Case 304
                        p = New DealerDirect.CampaignManagement.Programs.CSSR2017.Anniversary.Program
                    Case 305
                        p = New DealerDirect.CampaignManagement.Programs.CSSR2017.DexCool.Program
                End Select

                'If bResult Or True Then
                Dim pvFound As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion = Nothing
                Dim pv As DealerDirect.CampaignManagement.Programs.VDP.PreviewTargetVersion

                For Each pv In p.getPreviewVersions(sTarget, sStyleGroup)
                    If pv.VersionCode = sVersion Then
                        pvFound = pv
                        Exit For
                    End If
                Next

                If pvFound IsNot Nothing Then
                    If TypeOf (p) Is DealerDirect.CampaignManagement.Programs.CSSR.WelcomeNew.Program Then
                        prv = DirectCast(p, DealerDirect.CampaignManagement.Programs.CSSR.WelcomeNew.Program).createPreview(sLanguage, sChannel, pvFound, sOption)
                    Else
                        prv = p.createPreview(sLanguage, sChannel, pvFound)
                    End If

                    If prv IsNot Nothing Then prv.QueueCreate()
                    m_Result = prv.RequestID
                End If
            End If

            Return m_Result
        End Function
    End Class

    Public Class CSSRNewsItem
        Public ContentType As String
        Public ContentText As String
        Public DetailURL As String
        Public SortOrder As String
        Public ContentDate As String
    End Class
End Namespace
