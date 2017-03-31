Imports System.Web.Script.Serialization.JavaScriptSerializer
Imports System.Web.Services.WebService

Public Class Format
    Public Title As String
    Public Indicator As String
    Public Price As String
End Class

Public Class Check
    Public Status As String
    Public Title As String
End Class

Public Class CSSRNewsItem
    Public ContentType As String
    Public ContentText As String
    Public DetailURL As String
    Public SortOrder As String
    Public ContentDate As String
End Class

Partial Class cm_programs_cssr_dataservice
    Inherits System.Web.UI.Page

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(cm_programs_cssr_dataservice))

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim sOp As String = Request.Params("op")
        Dim sResponse As String = ""

        If sOp Is Nothing Then sOp = ""

        Select Case sOp.ToLower
            Case "getpreview"
                Dim nRequestID As Integer = Request.Params("rid")
                Dim sChannel As String = Request.Params("c")
                Dim sFileName As String = System.Guid.NewGuid.ToString ' & ".pdf"

                Select Case sChannel.ToUpper
                    Case "D"
                        sFileName &= ".pdf"
                    Case "E"
                        sFileName &= ".html"
                End Select

                'If Not System.IO.File.Exists(System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, sFileName)) Then
                'CM2.GMCL.DealerDirect.VDP.Client.Rendering..saveRequestOutput(nRequestID, System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, sFileName))
                sFileName = CM2.GMCL.DealerDirect.VDP.Client.Rendering.getRequestOutputFileName(nRequestID)
                'End If

                sResponse = "https://vdp.gmdealerdirect.ca/previews/" & sFileName 'DealerDirect.Configuration.Item("vdp_output_url").Text1Value

            Case "getpreviewstatus"
                Dim nRequestID As Integer = Request.Params("rid")
                Dim nStatus As CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus

                Select Case CM2.GMCL.DealerDirect.VDP.Client.Rendering.getRequestStatus(nRequestID)
                    Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Running
                        nStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Running
                    Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Cancelled,
                         CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Failed,
                         CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Unknown
                        nStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Failed
                    Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Complete
                        nStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Complete
                End Select

                sResponse = nStatus.ToString

            Case "createpreview"
                Dim p As DealerDirect.CampaignManagement.Programs.AudienceProgram
                Dim nSubCampaignID As Integer = Request.Params("sid")
                Dim nProgramID As Integer = Request.Params("pid")
                Dim sStyleGroup As String = Request.Params("sg")
                Dim sLanguage As String = Request.Params("l")
                Dim sChannel As String = Request.Params("c")
                Dim sVersion As String = Request.Params("v")
                Dim sTarget As String = Request.Params("t")
                Dim sOption As String = Request.Params("o")
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
                    sResponse = prv.RequestID
                Else
                    sResponse = "0"
                End If

            Case "getnews"
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

                sResponse = jss.Serialize(nil)

            Case "getsummary"
                Dim prgms As New System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary)

                prgms = DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary.loadAll(DealerDirect.Security.CurrentSession.SecurityContext.ID)

                sResponse = jss.Serialize(prgms)

            Case "getdetails"
                Dim prgm As DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramDetails
                Dim prgms As New System.Collections.Generic.List(Of DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramDetails)

                Dim n As Integer
                If Integer.TryParse(Request.Params("p"), n) Then
                    prgm = New DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramDetails(New DealerDirect.CampaignManagement.Programs.AudienceProgram(n))
                    prgms.Add(prgm)

                Else
                    For Each p As DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary In DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramSummary.loadAll(DealerDirect.Security.CurrentSession.SecurityContext.ID)
                        prgm = New DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.CSSRProgramDetails(New DealerDirect.CampaignManagement.Programs.AudienceProgram(p.ProgramID))
                        prgms.Add(prgm)
                    Next
                End If

                Dim cmd As Devart.Data.Oracle.OracleCommand
                Dim rdr As Devart.Data.Oracle.OracleDataReader

                cmd = New Devart.Data.Oracle.OracleCommand
                cmd.Connection = DealerDirect.Data.createUserConnectionD
                cmd.CommandType = System.Data.CommandType.StoredProcedure
                cmd.CommandText = "wsdp066_core_base.getProgramSamples"
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("samples", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
                cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID

                If prgms.Count = 1 Then
                    cmd.Parameters("n_program_id").Value = prgms(0).ProgramID
                Else
                    cmd.Parameters("n_program_id").Value = 0
                End If

                Dim smpl As DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.Sample

                rdr = cmd.ExecuteReader
                prgm = Nothing
                While rdr.Read
                    If prgm Is Nothing OrElse prgm.ProgramID <> rdr("program_id") Then
                        prgm = prgms.Where(Function(f) f.ProgramID = rdr("program_id")).FirstOrDefault
                    End If

                    smpl = prgm.Samples.Where(Function(f) f.SampleSetID = rdr("sample_set_id")).FirstOrDefault
                    If smpl Is Nothing Then
                        smpl = New DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.Sample
                        smpl.SampleSetID = rdr("sample_set_id")
                        smpl.Brand = rdr("brand_cd")
                        smpl.Channel = rdr("channel_type_cd")
                        smpl.Version = rdr("version_cd")
                        smpl.Name = rdr("sample_set_name_resource_cd")
                        prgm.Samples.Add(smpl)
                    End If

                    If smpl IsNot Nothing Then
                        Dim si As New DealerDirect.CampaignManagement.Programs.CSSR.Proxies.JSON.SampleImage
                        si.Name = rdr("image_name_resource_cd")
                        si.Language = rdr("language_cd")
                        si.OrderNumber = rdr("order_nbr")
                        si.URL = rdr("image_url")
                        smpl.Images.Add(si)
                    End If
                End While

                rdr.Close()
                rdr.Dispose()
                cmd.Connection.Close()
                cmd.Connection.Dispose()
                cmd.Dispose()

                sResponse = jss.Serialize(prgms)
        End Select

        Context.Response.Clear()
        Context.Response.ClearContent()
        Context.Response.ClearHeaders()
        Context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Context.Response.Cache.SetExpires(DateTime.Now.AddDays(-1))
        Context.Response.Cache.SetNoStore()
        Context.Response.Cache.SetProxyMaxAge(New TimeSpan(0, 0, 0))
        Context.Response.AppendHeader("Access-Control-Allow-Headers", "*")
        Context.Response.AppendHeader("Access-Control-Allow-Methods", "*")
        Context.Response.ContentType = "text/plain; charset=utf-8"  '"text/json"
        Context.Response.Write(sResponse)
        Context.Response.Flush()
        Context.Response.End()
    End Sub
End Class
