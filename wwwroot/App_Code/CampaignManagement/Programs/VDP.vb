Namespace DealerDirect.CampaignManagement.Programs.VDP
    Public Module VDP
        Private ReadOnly Property AssetCacheSessionKey(ByVal program_id As Integer, ByVal sub_campaign_id As Integer, ByVal context_id As Integer) As String
            Get
                Return String.Format("CM_PRGM_ASSETS:{0}/{1}/{2}", program_id, sub_campaign_id, context_id)
            End Get
        End Property

        Private Function getStaticAssets(ByVal program_id As Integer, ByVal sub_campaign_id As Integer) As StaticAssetCollection
            Dim sKey As String = VDP.AssetCacheSessionKey(program_id, sub_campaign_id, DealerDirect.Security.CurrentSession.SecurityContext.ID)

            If System.Web.HttpContext.Current.Session(sKey) Is Nothing Then
                Dim cmd As Devart.Data.Oracle.OracleCommand
                Dim rdr As Devart.Data.Oracle.OracleDataReader
                Dim sac As New StaticAssetCollection

                cmd = New Devart.Data.Oracle.OracleCommand
                cmd.Connection = DealerDirect.Data.createUserConnection
                cmd.CommandType = System.Data.CommandType.StoredProcedure
                cmd.CommandText = "wsdp066_core_vdp.getAvailStaticAssets"
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("assets", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
                cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
                cmd.Parameters("n_program_id").Value = program_id
                cmd.Parameters("n_sub_campaign_id").Value = sub_campaign_id

                rdr = cmd.ExecuteReader
                While rdr.Read
                    sac.Add(New StaticAsset(rdr))
                End While

                rdr.Close()
                rdr.Dispose()
                cmd.Connection.Close()
                cmd.Connection.Dispose()
                cmd.Dispose()

                System.Web.HttpContext.Current.Session(sKey) = sac
            End If

            Return System.Web.HttpContext.Current.Session(sKey)
        End Function

        Public Function getStaticAssets(ByVal program_id As Integer, ByVal sub_campaign_id As Integer, ByVal size_code As String, ByVal append_empty_option As Boolean) As StaticAssetCollection
            Dim sac As StaticAssetCollection

            sac = getStaticAssets(program_id, sub_campaign_id).FilterByAssetSizeCode(size_code)
            If append_empty_option Then sac.Insert(0, New StaticAsset(0, "(" & DealerDirect.Resources.getString("LITERAL_None") & ")"))
            Return sac
        End Function

        <Serializable()>
        Public Class StaticAsset
            Protected m_StaticAssetID As Integer
            Protected m_AssetSizeCode As String
            Protected m_NameEN As String
            Protected m_NameFR As String
            Protected m_Desc1 As String
            Protected m_Desc2 As String
            Protected m_Desc3 As String
            Protected m_Desc4 As String
            Protected m_PreviewFile As String
            Protected m_Preferred As Boolean

            Public Sub New(ByVal static_asset_id As Integer, ByVal asset_name As String)
                m_StaticAssetID = static_asset_id
                m_NameEN = asset_name
                m_NameFR = asset_name
            End Sub

            Public Sub New(ByRef irdr_on_row As System.Data.IDataReader)
                m_StaticAssetID = irdr_on_row("static_asset_id")
                m_AssetSizeCode = irdr_on_row("asset_size_cd")
                m_NameEN = Utility.Data.NothingIfNull(irdr_on_row("asset_name_en"))
                m_NameFR = Utility.Data.NothingIfNull(irdr_on_row("asset_name_fr"))
                m_Preferred = Utility.Data.FlagToBoolean(irdr_on_row("preferred_fl"))
                m_Desc1 = Utility.Data.NothingIfNull(irdr_on_row("desc1_text"))
                m_PreviewFile = Utility.Data.NothingIfNull(irdr_on_row("preview_file"))
            End Sub

            Public ReadOnly Property ID() As Integer
                Get
                    Return m_StaticAssetID
                End Get
            End Property
            Public ReadOnly Property AssetSizeCode() As String
                Get
                    Return m_AssetSizeCode
                End Get
            End Property
            Public ReadOnly Property Name() As String
                Get
                    If DealerDirect.Security.CurrentSession.Language = "fr" Then
                        Return m_NameFR
                    Else
                        Return m_NameEN
                    End If
                End Get
            End Property
            Public ReadOnly Property Description1() As String
                Get
                    Return m_Desc1
                End Get
            End Property

            Public ReadOnly Property PreviewFile() As String
                Get
                    Return m_PreviewFile
                End Get
            End Property
            Public ReadOnly Property Preferred() As Boolean
                Get
                    Return m_Preferred
                End Get
            End Property
        End Class
        <Serializable()>
        Public Class StaticAssetCollection
            Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, StaticAsset)
            Protected Overrides Function GetKeyForItem(ByVal item As StaticAsset) As Integer
                Return item.ID
            End Function

            Public Function FilterByAssetSizeCode(ByVal asset_size_code As String) As StaticAssetCollection
                Dim sac As New StaticAssetCollection

                For Each sa As StaticAsset In MyBase.Items
                    If sa.AssetSizeCode = asset_size_code Then sac.Add(sa)
                Next

                Return sac
            End Function
        End Class


        Public Function createPreview(ByVal program_id As Integer, ByVal sub_campaign_id As Integer, ByVal prog_treatment_id As Integer) As Programs.VDP.Preview
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim p As Programs.VDP.Preview = Nothing
            Dim sChannelTypeCode As String
            Dim sLanguageCode As String
            Dim nTreatmentID As Integer
            Dim nTemplateID As Integer

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_vdp.createPreviewTreatment"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_prog_treatment_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("treatment_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("template_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("language_cd", Devart.Data.Oracle.OracleDbType.VarChar, 2)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("channel_type_cd", Devart.Data.Oracle.OracleDbType.VarChar, 1)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_program_id").Value = program_id
            cmd.Parameters("n_sub_campaign_id").Value = sub_campaign_id
            cmd.Parameters("n_prog_treatment_id").Value = prog_treatment_id
            cmd.ExecuteNonQuery()

            nTreatmentID = cmd.Parameters("treatment_id").Value.ToString
            If nTreatmentID > 0 Then
                nTemplateID = cmd.Parameters("template_id").Value.ToString
                sLanguageCode = cmd.Parameters("language_cd").Value.ToString
                sChannelTypeCode = cmd.Parameters("channel_type_cd").Value.ToString

                p = New Programs.VDP.Preview(nTreatmentID, nTemplateID, sLanguageCode, sChannelTypeCode)
            End If

            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return p
        End Function

        Public Function getPreviewOptions(ByVal program_id As Integer, ByVal sub_campaign_id As Integer) As PreviewVersionItemCollection
            Return getPreviewVersions(program_id, sub_campaign_id)
        End Function

        Public Function getPreviewVersions(ByVal program_id As Integer, ByVal sub_campaign_id As Integer) As PreviewVersionItemCollection
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim rdr As Devart.Data.Oracle.OracleDataReader
            Dim pvic As PreviewVersionItemCollection

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure

            cmd.CommandText = "wsdp066_core_vdp.getPreviewVersions"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("preview_versions", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_program_id").Value = program_id
            cmd.Parameters("n_sub_campaign_id").Value = sub_campaign_id
            rdr = cmd.ExecuteReader

            pvic = New PreviewVersionItemCollection
            While rdr.Read
                pvic.Add(New PreviewVersionItem(rdr))
            End While

            rdr.Close()
            rdr.Dispose()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return pvic
        End Function
    End Module


    <Serializable()>
    Public Class PreviewTarget
        Protected m_Code As String
        Protected m_KeyCode As String
        Protected m_NameResourceCode As String
        Protected m_LanguageCode As String
        Protected m_ChannelTypeCode As String
        Protected m_AudienceID As Integer?
        Protected m_AudienceSplitID As Integer?

        Public Sub New(ByVal irdr_on_record As System.Data.IDataReader)
            Me.New(irdr_on_record("preview_target_cd"), irdr_on_record("preview_target_key_cd"),
             irdr_on_record("channel_type_cd"), irdr_on_record("language_cd"),
            irdr_on_record("name_resource_cd"))
        End Sub

        Public Sub New(ByVal code As String, ByVal key_code As String, ByVal channel_type_code As String, ByVal language_code As String,
        ByVal name_resource_code As String, ByVal audience_id As Integer?, ByVal audience_split_id As Integer?)
            m_Code = code
            m_KeyCode = key_code
            m_NameResourceCode = name_resource_code
            m_LanguageCode = language_code
            m_ChannelTypeCode = channel_type_code
            m_AudienceID = audience_id
            m_AudienceSplitID = audience_split_id
        End Sub

        Public Sub New(ByVal code As String, ByVal key_code As String, ByVal channel_type_code As String, ByVal language_code As String,
        ByVal name_resource_code As String)
            m_Code = code
            m_KeyCode = key_code
            m_NameResourceCode = name_resource_code
            m_LanguageCode = language_code
            m_ChannelTypeCode = channel_type_code
        End Sub

        Public ReadOnly Property Code() As String
            Get
                Return m_Code
            End Get
        End Property
        Public ReadOnly Property KeyCode() As String
            Get
                Return m_KeyCode
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
        Public ReadOnly Property LanguageCode() As String
            Get
                Return m_LanguageCode
            End Get
        End Property
        Public ReadOnly Property ChannelTypeCode() As String
            Get
                Return m_ChannelTypeCode
            End Get
        End Property
        Public ReadOnly Property AudienceID() As Integer?
            Get
                Return m_AudienceID
            End Get
        End Property
        Public ReadOnly Property AudienceSplitID() As Integer
            Get
                Return m_AudienceSplitID
            End Get
        End Property

        Public ReadOnly Property LanguageName() As String
            Get
                Return DealerDirect.Resources.getListItem("LANGUAGE", m_LanguageCode)
            End Get
        End Property
        Public ReadOnly Property ChannelTypeName() As String
            Get
                Return DealerDirect.Resources.getListItem("CHANNEL", m_ChannelTypeCode)
            End Get
        End Property
    End Class
    <Serializable()>
    Public Class PreviewTargetList
        Inherits System.Collections.Generic.SortedList(Of String, PreviewTarget)
    End Class
    <Serializable()>
    Public Class PreviewTargetCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of String, PreviewTarget)

        Protected Overrides Function GetKeyForItem(ByVal item As PreviewTarget) As String
            Return item.KeyCode
        End Function

        Public Function getTargets() As PreviewTargetList
            Dim l As New PreviewTargetList
            For Each pt As PreviewTarget In MyBase.Items
                If Not l.Keys.Contains(pt.Code) Then
                    l.Add(pt.Code, pt)
                End If
            Next

            Return l
        End Function

        Public Function getLanguages(ByVal code As String) As PreviewTargetList
            Dim l As New PreviewTargetList
            For Each pt As PreviewTarget In MyBase.Items
                If pt.Code = code AndAlso Not l.Keys.Contains(pt.LanguageCode) Then
                    l.Add(pt.LanguageCode, pt)
                End If
            Next

            Return l
        End Function

        Public Function getChannels(ByVal code As String) As PreviewTargetList
            Dim l As New PreviewTargetList
            For Each pt As PreviewTarget In MyBase.Items
                If pt.Code = code AndAlso Not l.Keys.Contains(pt.ChannelTypeCode) Then
                    l.Add(pt.ChannelTypeCode, pt)
                End If
            Next

            Return l
        End Function
    End Class


    <Serializable()>
    Public Class PreviewTargetVersion
        Protected m_Code As String
        Protected m_NameResourceCode As String
        Protected m_TargetCode As String
        Protected m_StyleGroupCode As String
        Protected m_StyleOptionCode As String
        Protected m_Option1 As String
        Protected m_Option2 As String
        Protected m_Option3 As String

        Public Property Option1 As String
            Get
                Return m_Option1
            End Get
            Set(value As String)
                m_Option1 = value
            End Set
        End Property
        Public Property Option2 As String
            Get
                Return m_Option2
            End Get
            Set(value As String)
                m_Option2 = value
            End Set
        End Property
        Public Property Option3 As String
            Get
                Return m_Option3
            End Get
            Set(value As String)
                m_Option3 = value
            End Set
        End Property

        Public ReadOnly Property VersionCode() As String
            Get
                Return m_Code
            End Get
        End Property
        Public ReadOnly Property TargetCode() As String
            Get
                Return m_TargetCode
            End Get
        End Property
        Public Property StyleGroupCode() As String
            Get
                Return m_StyleGroupCode
            End Get
            Set(value As String)
                m_StyleGroupCode = value
            End Set
        End Property
        Public Property StyleOptionCode() As String
            Get
                Return m_StyleOptionCode
            End Get
            Set(value As String)
                m_StyleOptionCode = value
            End Set
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

        Public Sub New()
        End Sub
        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_Code = irdr_on_row("preview_version_cd")
            m_NameResourceCode = irdr_on_row("name_resource_cd")
            m_TargetCode = irdr_on_row("preview_target_cd")
            m_StyleGroupCode = Utility.Data.NothingIfNull(irdr_on_row("style_group_cd"))
            m_Option1 = Utility.Data.NothingIfNull(irdr_on_row("option_group1_cd"))
        End Sub
    End Class
    Public Class PreviewTargetVersionCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of String, PreviewTargetVersion)

        Protected Overrides Function GetKeyForItem(ByVal item As PreviewTargetVersion) As String
            Return item.VersionCode
        End Function
    End Class



    <Serializable()>
    Public Class PreviewVersionItem
        Protected m_ProgramTreatmentID As Integer
        Protected m_VersionCode As String
        Protected m_NameResourceCode As String
        Protected m_LanguageCode As String
        Protected m_ChannelTypeCode As String
        Protected m_PreviewTypeCode As String
        Protected m_StaticPreviewURL As String

        Protected Sub New()
        End Sub

        Public Function createPreview(ByVal program_id As Integer, ByVal sub_campaign_id As Integer) As Programs.VDP.Preview
            Return Programs.VDP.createPreview(program_id, sub_campaign_id, m_ProgramTreatmentID)
        End Function

        Public Property ID() As Integer
            Get
                Return m_ProgramTreatmentID
            End Get
            Set(ByVal value As Integer)
            End Set
        End Property

        Public Property VersionCode() As String
            Get
                Return m_VersionCode
            End Get
            Set(ByVal value As String)
                m_VersionCode = value
            End Set
        End Property
        Public ReadOnly Property VersionName() As String
            Get
                If DealerDirect.Resources.doesStringExist(m_VersionCode) Then
                    Return DealerDirect.Resources.getString(m_VersionCode)
                Else
                    Return m_VersionCode
                End If
            End Get
        End Property

        Public Property LanguageCode() As String
            Get
                Return m_LanguageCode
            End Get
            Set(ByVal value As String)
                m_LanguageCode = value
            End Set
        End Property
        Public ReadOnly Property LanguageName() As String
            Get
                Return DealerDirect.Resources.getListItem("LANGUAGE", m_LanguageCode)
            End Get
        End Property

        Public Property ChannelTypeCode() As String
            Get
                Return m_ChannelTypeCode
            End Get
            Set(ByVal value As String)
                m_ChannelTypeCode = value
            End Set
        End Property
        Public ReadOnly Property ChannelTypeName() As String
            Get
                Return DealerDirect.Resources.getListItem("CHANNEL", m_ChannelTypeCode)
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

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_ProgramTreatmentID = irdr_on_row("prog_treatment_id")
            m_VersionCode = irdr_on_row("preview_version_resource_cd")
            m_NameResourceCode = irdr_on_row("preview_version_resource_cd")
            m_LanguageCode = irdr_on_row("language_cd")
            m_ChannelTypeCode = irdr_on_row("channel_type_cd")
            m_PreviewTypeCode = irdr_on_row("preview_type_cd")
            m_StaticPreviewURL = Utility.Data.NothingIfNull(irdr_on_row("static_preview_url"))
        End Sub
    End Class
    <Serializable()>
    Public Class PreviewVersionItemCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, PreviewVersionItem)

        Protected Overrides Function GetKeyForItem(ByVal item As PreviewVersionItem) As Integer
            Return item.ID
        End Function

        Public Function getVersions() As PreviewVersionItemList
            Dim l As New PreviewVersionItemList
            For Each pvi As PreviewVersionItem In MyBase.Items
                If Not l.Keys.Contains(pvi.VersionCode) Then
                    l.Add(pvi.VersionCode, pvi)
                End If
            Next

            Return l
        End Function

        Public Function getLanguages(ByVal version_code As String) As PreviewVersionItemList
            Dim l As New PreviewVersionItemList
            For Each pvi As PreviewVersionItem In MyBase.Items
                If pvi.VersionCode = version_code AndAlso Not l.Keys.Contains(pvi.LanguageCode) Then
                    l.Add(pvi.LanguageCode, pvi)
                End If
            Next

            Return l
        End Function

        Public Function getChannels(ByVal version_code As String, ByVal language_code As String) As PreviewVersionItemList
            Dim l As New PreviewVersionItemList
            For Each pvi As PreviewVersionItem In MyBase.Items
                If pvi.VersionCode = version_code AndAlso pvi.LanguageCode = language_code AndAlso Not l.Keys.Contains(pvi.ChannelTypeCode) Then
                    l.Add(pvi.ChannelTypeCode, pvi)
                End If
            Next

            Return l
        End Function
    End Class
    <Serializable()>
    Public Class PreviewVersionItemList
        Inherits System.Collections.Generic.SortedList(Of String, PreviewVersionItem)
    End Class

    <Serializable()>
    Public Class Preview
        Protected m_ChannelCode As String
        Protected m_LanguageCode As String
        Protected m_TemplateID As Integer
        Protected m_TreatmentID As Integer
        Protected m_VDPRequestID As Integer = 0
        Protected m_PreviewName As String
        Protected m_VDPStatus As CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus

        Public Enum VDPStatus As Integer
            Unknown = 0
            Running = 1
            Complete = 2
            Failed = 3
        End Enum

        Public ReadOnly Property TreatmentID() As Integer
            Get
                Return m_TreatmentID
            End Get
        End Property

        Public ReadOnly Property ChannelTypeCode() As String
            Get
                Return m_ChannelCode
            End Get
        End Property

        Public ReadOnly Property PreviewFileName() As String
            Get
                Return m_PreviewName
            End Get
        End Property

        Public ReadOnly Property RequestID As Int64
            Get
                Return m_VDPRequestID
            End Get
        End Property

        Public ReadOnly Property URL() As String
            Get
                If Me.isAvailable(True) Then
                    Return DealerDirect.Configuration.Item("vdp_output_url").Text1Value & m_PreviewName
                    'If Me.ChannelTypeCode = "E" Then
                    '	Return "https://secure.edealerdirect.ca/HTMLPreviews/" & m_PreviewName
                    'Else
                    '	Return DealerDirect.Configuration.Item("vdp_output_url").Text1Value & m_PreviewName
                    'End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub New(ByVal treatment_id As Integer, ByVal template_id As Integer, ByVal language_code As String, ByVal channel_code As String)
            m_TreatmentID = treatment_id
            m_TemplateID = template_id
            m_LanguageCode = language_code
            m_ChannelCode = channel_code
        End Sub

        Private Function NewVDPRequestID() As Integer
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim nRequestID As Integer

            If m_VDPRequestID > 0 Then
                CM2.GMCL.DealerDirect.VDP.Client.Rendering.CancelRequest(m_VDPRequestID)
            End If

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp048.getNewVDPRequestID"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("request_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Output
            cmd.ExecuteNonQuery()

            nRequestID = cmd.Parameters("request_id").Value

            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            m_VDPRequestID = nRequestID
            Return nRequestID
        End Function

        Public Function CancelCreate() As Boolean
            Try
                If m_VDPRequestID > 0 Then
                    'TODO: restore cancel
                    CM2.GMCL.DealerDirect.VDP.Client.Rendering.CancelRequest(m_VDPRequestID)
                End If

                m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Unknown ' VDPStatus.Unknown
            Catch ex As Exception
            End Try

            Return True
        End Function

        Public Sub QueueCreate()
            Dim sGUID As String

            Try
                If m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Unknown Then
                    sGUID = System.Guid.NewGuid.ToString

                    If m_ChannelCode = "D" Then
                        m_VDPRequestID = CM2.GMCL.DealerDirect.VDP.Client.Rendering.requestPreviewPDF(m_TreatmentID)
                        m_PreviewName = sGUID & ".pdf"

                    ElseIf m_ChannelCode = "E" Then
                        m_VDPRequestID = CM2.GMCL.DealerDirect.VDP.Client.Rendering.requestPreviewHTML(m_TreatmentID)
                        m_PreviewName = sGUID & ".html"
                    End If

                    If m_VDPRequestID > 0 Then
                        m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.Rendering.getRequestStatus(m_VDPRequestID)
                    Else
                        m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Unknown
                    End If
                End If

            Catch ex As Exception
                m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Failed
            End Try
        End Sub

        Public ReadOnly Property isAvailable(Optional ByVal no_queue As Boolean = False) As Boolean
            Get
                If Me.UpdateVDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Complete Then
                    If Not Me.previewFileExists Then
                        CM2.GMCL.DealerDirect.VDP.Client.Rendering.saveRequestOutput(m_VDPRequestID, System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, Me.m_PreviewName))
                    End If

                    Return Me.previewFileExists
                Else
                    Return False
                End If
            End Get
        End Property
        Private Function previewFileExists() As Boolean
            Try
                Return System.IO.File.Exists(System.IO.Path.Combine(DealerDirect.Configuration.Item("vdp_output_folder").Text1Value, Me.m_PreviewName))
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function UpdateVDPStatus() As CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus
            Select Case CM2.GMCL.DealerDirect.VDP.Client.Rendering.getRequestStatus(m_VDPRequestID)
                Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Running
                    m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Running
                Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Cancelled,
                CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Failed,
                CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Unknown
                    m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Failed
                Case CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Complete
                    m_VDPStatus = CM2.GMCL.DealerDirect.VDP.Client.VDPService.RequestStatus.Complete
            End Select

            Return m_VDPStatus
        End Function
    End Class

    <Serializable()>
    Public Class VariableSet
        Protected m_VariableSetID As Integer
        Protected m_NameResourceCode As String
        Protected m_SourceCode As String
        Protected m_LanguageCode As String

        Protected m_Variables As Programs.VDP.VariableCollection

        Public Property ID() As Integer
            Get
                Return m_VariableSetID
            End Get
            Set(ByVal value As Integer)
                m_VariableSetID = value
            End Set
        End Property

        Public Property NameResourceCode() As String
            Get
                Return m_NameResourceCode
            End Get
            Set(ByVal value As String)
                m_NameResourceCode = value
            End Set
        End Property

        Public Property Variables() As VariableCollection
            Get
                Return m_Variables
            End Get
            Set(ByVal value As VariableCollection)
                m_Variables = value
            End Set
        End Property

        Public ReadOnly Property SourceCode As String
            Get
                Return m_SourceCode
            End Get
        End Property

        Public ReadOnly Property LanguageCode As String
            Get
                Return m_LanguageCode
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return DealerDirect.Resources.getString(m_NameResourceCode)
            End Get
        End Property

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_VariableSetID = irdr_on_row("prog_variable_set_id")
            m_NameResourceCode = Utility.Data.NothingIfNull(irdr_on_row("name_resource_cd"))
            m_SourceCode = irdr_on_row("source_cd")
            m_LanguageCode = Utility.Data.NothingIfNull(irdr_on_row("language_cd"))
            m_Variables = New VariableCollection
        End Sub
    End Class
    <Serializable()>
    Public Class VariableSetCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, VariableSet)

        Protected Overrides Function GetKeyForItem(ByVal item As VariableSet) As Integer
            Return item.ID
        End Function
    End Class

    <Serializable()>
    Public Class Variable
        Protected m_VariableID As Integer
        Protected m_VariableSetID As Integer
        Protected m_ProgramID As Integer
        Protected m_SubCampaignID As Integer
        Protected m_CommonVariableCode As String
        Protected m_NameResourceCode As String
        Protected m_SuffixResourceCode As String
        Protected m_DisplayOrderNumber As Integer

        Protected m_VariableTypeCode As String
        Protected m_VariableSizeCode As String
        Protected m_ControlOptions As String

        Protected m_StaticAssetID As Integer?
        Protected m_NumberValue As Double?
        Protected m_TextValue As String

        Protected m_StaticAssets As DealerDirect.VDP.Assets.StaticAssetCollection

        Protected m_ParentVariableID As Integer?
        Protected m_ValidationGroupCode As String
        Protected m_StyleGroupCode As String
        Protected m_ShowMeURL As String

        Public Function getStaticAssets(Optional ByVal append_empty_option As Boolean = False) As Programs.VDP.StaticAssetCollection
            Return Programs.VDP.getStaticAssets(m_ProgramID, m_SubCampaignID, m_VariableSizeCode, append_empty_option)
        End Function

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_VariableID = irdr_on_row("prog_variable_id")
            m_VariableSetID = irdr_on_row("prog_variable_set_id")
            m_ProgramID = irdr_on_row("program_id")
            m_SubCampaignID = irdr_on_row("sub_campaign_id")
            m_CommonVariableCode = irdr_on_row("common_variable_cd")
            m_NameResourceCode = Utility.Data.NothingIfNull(irdr_on_row("name_resource_cd"))
            m_SuffixResourceCode = Utility.Data.NothingIfNull(irdr_on_row("suffix_resource_cd"))
            m_DisplayOrderNumber = irdr_on_row("display_order_nbr")

            m_VariableTypeCode = Utility.Data.NothingIfNull(irdr_on_row("variable_type_cd"))
            m_VariableSizeCode = Utility.Data.NothingIfNull(irdr_on_row("variable_size_cd"))
            m_ControlOptions = Utility.Data.NothingIfNull(irdr_on_row("control_options"))

            m_ValidationGroupCode = Utility.Data.NothingIfNull(irdr_on_row("validation_group_cd"))
            m_StyleGroupCode = Utility.Data.NothingIfNull(irdr_on_row("style_group_cd"))
            m_ShowMeURL = Utility.Data.NothingIfNull(irdr_on_row("show_me_url"))
            If irdr_on_row("parent_prog_variable_id") IsNot System.DBNull.Value Then
                m_ParentVariableID = CInt(irdr_on_row("parent_prog_variable_id"))
            End If
        End Sub

        Public Property ParentVariableID As Integer?
            Get
                Return m_ParentVariableID
            End Get
            Set(value As Integer?)
                m_ParentVariableID = value
            End Set
        End Property

        Public Property ValidationGroupCode As String
            Get
                Return m_ValidationGroupCode
            End Get
            Set(value As String)
                m_ValidationGroupCode = value
            End Set
        End Property

        Public ReadOnly Property StyleGroupCode As String
            Get
                Return m_StyleGroupCode
            End Get
        End Property

        Public ReadOnly Property ShowMeURL As String
            Get
                Return m_ShowMeURL
            End Get
        End Property

        Public Sub attachData(ByVal irdr_on_row As System.Data.IDataReader)
            If irdr_on_row("static_asset_id") IsNot System.DBNull.Value Then m_StaticAssetID = CInt(irdr_on_row("static_asset_id"))
            If irdr_on_row("number_value") IsNot System.DBNull.Value Then m_NumberValue = CDbl(irdr_on_row("number_value"))
            If irdr_on_row("text_value") IsNot System.DBNull.Value Then m_TextValue = irdr_on_row("text_value")
        End Sub

        Public Function Save(Optional ByVal temporary As Boolean = False) As Boolean
            Dim cmd As Devart.Data.Oracle.OracleCommand

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_vdp.saveVariableData"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_set_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_static_asset_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_text_value", Devart.Data.Oracle.OracleDbType.VarChar)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_number_value", Devart.Data.Oracle.OracleDbType.Number)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = IIf(temporary, -Me.m_SubCampaignID, Me.m_SubCampaignID)
            cmd.Parameters("n_variable_set_id").Value = Me.m_VariableSetID
            cmd.Parameters("n_variable_id").Value = Me.m_VariableID

            If Me.m_StaticAssetID.HasValue Then
                cmd.Parameters("n_static_asset_id").Value = Me.m_StaticAssetID.Value
            Else
                cmd.Parameters("n_static_asset_id").Value = System.DBNull.Value
            End If

            If Me.m_NumberValue.HasValue Then
                cmd.Parameters("n_number_value").Value = Me.m_NumberValue.Value
            Else
                cmd.Parameters("n_number_value").Value = System.DBNull.Value
            End If

            If Me.m_TextValue IsNot Nothing AndAlso m_TextValue.Length > 0 Then
                cmd.Parameters("c_text_value").Value = Me.m_TextValue
            Else
                cmd.Parameters("c_text_value").Value = System.DBNull.Value
            End If

            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return True
        End Function

        Public Function getThemeOptions() As ThemeOptionCollection
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim rdr As Devart.Data.Oracle.OracleDataReader
            Dim toc As New ThemeOptionCollection

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_vdp.getVariableThemeOptions"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_set_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("theme_options", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = Me.m_SubCampaignID
            cmd.Parameters("n_program_id").Value = Me.m_ProgramID
            cmd.Parameters("n_variable_set_id").Value = Me.m_VariableSetID
            cmd.Parameters("n_variable_id").Value = Me.m_VariableID

            rdr = cmd.ExecuteReader
            While rdr.Read
                toc.Add(New ThemeOption(rdr))
            End While

            rdr.Close()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return toc
        End Function

        Public Function getVariableOptions() As VariableOptionCollection
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim rdr As Devart.Data.Oracle.OracleDataReader
            Dim voc As New VariableOptionCollection

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = DealerDirect.Data.createUserConnection
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_vdp.getVariableOptions"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_set_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_variable_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("options", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = Me.m_SubCampaignID
            cmd.Parameters("n_program_id").Value = Me.m_ProgramID
            cmd.Parameters("n_variable_set_id").Value = Me.m_VariableSetID
            cmd.Parameters("n_variable_id").Value = Me.m_VariableID

            rdr = cmd.ExecuteReader
            While rdr.Read
                voc.Add(New VariableOption(rdr))
            End While

            rdr.Close()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return voc
        End Function

        Public Property ID() As Integer
            Get
                Return m_VariableID
            End Get
            Set(ByVal value As Integer)
                m_VariableID = value
            End Set
        End Property
        Public Property ProgramID() As Integer
            Get
                Return m_ProgramID
            End Get
            Set(ByVal value As Integer)
                m_ProgramID = value
            End Set
        End Property
        Public Property SubCampaignID() As Integer
            Get
                Return m_SubCampaignID
            End Get
            Set(ByVal value As Integer)
                m_SubCampaignID = value
            End Set
        End Property

        Public Property CommonVariableCode() As String
            Get
                Return m_CommonVariableCode
            End Get
            Set(ByVal value As String)
                m_CommonVariableCode = value
            End Set
        End Property

        Public Property NameResourceCode() As String
            Get
                Return m_NameResourceCode
            End Get
            Set(ByVal value As String)
                m_NameResourceCode = value
            End Set
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return DealerDirect.Resources.getString(m_NameResourceCode)
            End Get
        End Property

        Public ReadOnly Property DisplayOrderNumber() As Integer
            Get
                Return m_DisplayOrderNumber
            End Get
        End Property

        Public Property SuffixResourceCode() As String
            Get
                Return m_SuffixResourceCode
            End Get
            Set(ByVal value As String)
                m_SuffixResourceCode = value
            End Set
        End Property
        Public Property VariableTypeCode() As String
            Get
                Return m_VariableTypeCode
            End Get
            Set(ByVal value As String)
                m_VariableTypeCode = value
            End Set
        End Property
        Public Property VariableSizeCode() As String
            Get
                Return m_VariableSizeCode
            End Get
            Set(ByVal value As String)
                m_VariableSizeCode = value
            End Set
        End Property
        Public Property ControlOptions() As String
            Get
                Return m_ControlOptions
            End Get
            Set(ByVal value As String)
                m_ControlOptions = value
            End Set
        End Property

        Public Function getControlOption(ByVal option_code As String) As String
            If m_ControlOptions IsNot Nothing Then
                Dim sOptions As String = m_ControlOptions.ToLower
                Dim sTemp As String = Nothing
                Dim sOptionCode As String = option_code.ToLower & "="

                If sOptions.Contains(option_code) Then
                    Dim nStart As Integer = sOptions.IndexOf(sOptionCode) + sOptionCode.Length
                    Dim nEnd As Integer = sOptions.IndexOf(";", nStart)
                    sTemp = m_ControlOptions.Substring(nStart, nEnd - nStart)
                End If

                Return sTemp
            Else
                Return Nothing
            End If
        End Function

        Public Property TextValue() As String
            Get
                Return Me.m_TextValue
            End Get
            Set(ByVal value As String)
                Me.m_TextValue = value
            End Set
        End Property

        Public Property StaticAssetID() As Integer?
            Get
                Return m_StaticAssetID
            End Get
            Set(ByVal value As Integer?)
                m_StaticAssetID = value
            End Set
        End Property

        Public Property NumberValue() As Double?
            Get
                Return m_NumberValue
            End Get
            Set(ByVal value As Double?)
                m_NumberValue = value
            End Set
        End Property
    End Class
    <Serializable()>
    Public Class VariableCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, Variable)

        Protected Overrides Function GetKeyForItem(ByVal item As Variable) As Integer
            Return item.ID
        End Function
    End Class

    <Serializable()>
    Public Class VariableOption
        Protected m_OptionCode As String
        Protected m_SequenceNumber As Integer
        Protected m_TextValue As String
        Protected m_NameResourceCode As String

        Public ReadOnly Property OptionCode() As String
            Get
                Return m_OptionCode
            End Get
        End Property

        Public ReadOnly Property SequenceNumber() As Integer
            Get
                Return m_SequenceNumber
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return DealerDirect.Resources.getString(m_NameResourceCode)
            End Get
        End Property

        Public ReadOnly Property Value() As String
            Get
                Return m_TextValue
            End Get
        End Property

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_OptionCode = irdr_on_row("option_cd")
            m_SequenceNumber = irdr_on_row("sequence_nbr")
            m_TextValue = irdr_on_row("text_value")
            m_NameResourceCode = irdr_on_row("name_resource_cd")
        End Sub
    End Class
    <Serializable()>
    Public Class VariableOptionCollection
        Inherits System.Collections.ObjectModel.Collection(Of VariableOption)
    End Class

    <Serializable()>
    Public Class ThemeOption
        Protected m_ThemeID As Integer
        Protected m_NameResourceCode As String

        Public ReadOnly Property Name() As String
            Get
                Return DealerDirect.Resources.getString(m_NameResourceCode)
            End Get
        End Property

        Public ReadOnly Property Value() As String
            Get
                Return m_ThemeID.ToString
            End Get
        End Property

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            m_ThemeID = irdr_on_row("theme_id")
            m_NameResourceCode = irdr_on_row("name_resource_cd")
        End Sub
    End Class
    <Serializable()>
    Public Class ThemeOptionCollection
        Inherits System.Collections.ObjectModel.Collection(Of ThemeOption)
    End Class
End Namespace
