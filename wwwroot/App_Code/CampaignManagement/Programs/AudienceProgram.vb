Namespace DealerDirect.CampaignManagement.Programs
    <Serializable()>
    Public Class AudienceProgram
        Inherits Programs.ProgramBase

        Protected m_Audiences As Programs.AudienceCollection
        Protected m_CountCampaignID As Integer

        Public Property Audiences() As Programs.AudienceCollection
            Get
                Return m_Audiences
            End Get
            Set(ByVal value As Programs.AudienceCollection)
                m_Audiences = value
            End Set
        End Property

        Public Sub New(ByVal program_id As Integer, Optional ByVal skip_load As Boolean = False)
            MyBase.New(program_id, skip_load)
            If Not (skip_load) Then Me.loadData()
        End Sub

        Private Function loadData() As Boolean
            Dim cmd As Devart.Data.Oracle.OracleCommand
            Dim rdr As Devart.Data.Oracle.OracleDataReader
            Dim ac As New AudienceCollection
            Dim s As AudienceSplit

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = Data.createUserConnectionD
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_aud.getProgramAudiences"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_program_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("audiences", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("aud_splits", Devart.Data.Oracle.OracleDbType.Cursor)).Direction = System.Data.ParameterDirection.Output
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_program_id").Value = MyBase.ProgramID
            cmd.Parameters("n_sub_campaign_id").Value = MyBase.SubCampaignID

            rdr = cmd.ExecuteReader
            While rdr.Read
                ac.Add(New Audience(rdr))
            End While
            m_Audiences = ac

            rdr.NextResult()
            While rdr.Read
                s = New AudienceSplit(rdr)
                If m_Audiences.Contains(s.AudienceID) Then m_Audiences(s.AudienceID).Splits.Add(s)
            End While

            rdr.Close()
            rdr.Dispose()
            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            ' filters
            Dim nAudienceID As Integer
            For Each aud As Audience In Me.m_Audiences
                nAudienceID = aud.ID
                For Each fc As DealerDirect.CampaignManagement.Programs.Filters.FilterConfig In MyBase.FilterConfigs.Where(Function(f) f.ContainerTypeCode = "PGAUD" AndAlso f.ContainerID = nAudienceID)
                    aud.FilterConfigs.Add(fc)
                Next

                If True Then 'aud.HasSetupFilterSet Then
                    aud.SetupFilterSet = New DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet(
                     New DealerDirect.Containers.GenericContainer("SBAUD", aud.SubAudienceID), MyBase.SetupFilterSetCode)
                    aud.SetupFilterSet.loadContainerFilters(MyBase.SetupFilterSetCode)
                End If
            Next

            Return True
        End Function

        Public Function Save() As Boolean
            Dim bSaved As Boolean = True

            For Each aud As Audience In Me.Audiences
                If Not aud.Save() Then bSaved = False
            Next

            Return bSaved
        End Function

        Public Sub loadEstimates(ByVal global_filters As DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection)
            Dim cfdi As DealerDirect.CampaignManagement.DataFilters.ContainerFilterDataItem
            Dim cf As DealerDirect.CampaignManagement.DataFilters.ContainerFilter
            Dim ds As System.Data.DataSet
            Dim dtData As System.Data.DataTable
            Dim dv As System.Data.DataView
            Dim bDMail As Boolean = False
            Dim bEMail As Boolean = False
            Dim sGlobalRowFilter As String = "1=1 "
            Dim sAudienceRowFilter As String = "1=1 "
            Dim sCurrentFilter As String = Nothing
            Dim sFilter As String = Nothing
            Dim dvFilters As System.Data.DataView
            Dim sFilterCode As String
            Dim sFilterFieldName As String
            Dim sSplitFilter As String
            Dim sFinalFilter As String

            ' static's are already loaded
            'If Me.AudienceEstSourceCode = "STATIC" Then
            '	Exit Sub
            'End If

            ds = Me.getEstimateData
            dtData = ds.Tables(0)
            dvFilters = New System.Data.DataView(ds.Tables(1))

            dv = New System.Data.DataView(dtData)

            ' global filters
            dvFilters.RowFilter = String.Format("source_container_type_cd='{0}' AND source_container_id={1} AND dest_container_type_cd='{0}' AND dest_container_id={1} AND show_filter_fl='Y'", "PRGM", Me.ProgramID)
            'dvFilters.RowFilter = String.Format("source_container_type_cd='{0}' AND source_container_id={1} AND dest_container_type_cd='{0}' AND dest_container_id={1} AND allow_edit_fl='Y'", "PRGM", Me.ProgramID)
            For Each drv As System.Data.DataRowView In dvFilters
                sFilterCode = drv("SOURCE_FILTER_CD")
                sFilterFieldName = drv("FILTER_VALUE_FIELD_NAME")
                sCurrentFilter = ""

                cf = global_filters.Values.Where(Function(f) f.FilterCode = sFilterCode).FirstOrDefault
                Select Case cf.FilterCode
                    Case "PRGM_CHANNEL"
                        sCurrentFilter = "(1=2 "
                        For Each cfdi In cf.DataItems
                            If cfdi.Text1Value = "E" Then
                                bEMail = True
                                sCurrentFilter &= " OR usable_e_mail_fl='Y' "
                            ElseIf cfdi.Text1Value = "D" Then
                                bDMail = True
                                sCurrentFilter &= " OR usable_d_mail_fl='Y' "
                            End If
                        Next
                        sCurrentFilter &= ")"

                    Case Else
                        sCurrentFilter = "(1=2 "
                        For Each cfdi In cf.DataItems
                            Select Case cf.Filter.DataType
                                Case "T"
                                    sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(cf.CompareOp) & "'" & cfdi.Text1Value & "'"
                                Case "N"
                                    sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(cf.CompareOp) & cfdi.Number1Value & " "
                            End Select
                        Next
                        sCurrentFilter &= ")"
                End Select

                sGlobalRowFilter &= "AND " & sCurrentFilter
            Next

            For Each a As Audience In Me.Audiences
                sFilter = String.Format("container_type_cd='SBAUD' AND container_id={0}", a.SubAudienceID)

                If sGlobalRowFilter IsNot Nothing Then
                    sFilter &= " AND " & sGlobalRowFilter
                End If

                ' audience filters
                If a.CurrentFilterData IsNot Nothing Then
                    'dvFilters.RowFilter = String.Format("source_container_type_cd='{0}' AND source_container_id={1} AND dest_container_type_cd='{0}' AND dest_container_id={1} AND allow_edit_fl='Y'", "PGAUD", a.ID)
                    dvFilters.RowFilter = String.Format("source_container_type_cd='{0}' AND source_container_id={1} AND dest_container_type_cd='{0}' AND dest_container_id={1} AND show_filter_fl='Y'", "PGAUD", a.ID)
                    For Each drv As System.Data.DataRowView In dvFilters
                        sFilterCode = drv("SOURCE_FILTER_CD")
                        sFilterFieldName = drv("FILTER_VALUE_FIELD_NAME")
                        sCurrentFilter = ""

                        cf = a.CurrentFilterData.Values.Where(Function(f) f.FilterCode = sFilterCode).FirstOrDefault
                        Select Case cf.FilterCode
                            Case "PRGM_CHANNEL"
                                sCurrentFilter = "(1=2 "
                                For Each cfdi In cf.DataItems
                                    If cfdi.Text1Value = "E" Then
                                        bEMail = True
                                        sCurrentFilter &= " OR usable_e_mail_fl='Y' "
                                    ElseIf cfdi.Text1Value = "D" Then
                                        bDMail = True
                                        sCurrentFilter &= " OR usable_d_mail_fl='Y' "
                                    End If
                                Next
                                sCurrentFilter &= ")"

                            Case Else
                                sCurrentFilter = "(1=2 "
                                For Each cfdi In cf.DataItems
                                    Select Case cf.Filter.DataType
                                        Case "T"
                                            sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(cf.CompareOp) & "'" & cfdi.Text1Value & "'"
                                        Case "N"
                                            sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(cf.CompareOp) & cfdi.Number1Value & " "
                                    End Select
                                Next
                                sCurrentFilter &= ")"
                        End Select

                        sFilter &= " AND " & sCurrentFilter
                    Next
                End If

                ' pushed filters from global
                'dvFilters.RowFilter = String.Format("source_container_type_cd='PRGM' AND source_container_id={0} AND dest_container_type_cd='PGAUD' AND dest_container_id={1} AND allow_edit_fl='Y'", Me.ProgramID, a.ID)
                dvFilters.RowFilter = String.Format("source_container_type_cd='PRGM' AND source_container_id={0} AND dest_container_type_cd='PGAUD' AND dest_container_id={1} AND show_filter_fl='Y'", Me.ProgramID, a.ID)
                For Each drv As System.Data.DataRowView In dvFilters
                    sFilterCode = drv("SOURCE_FILTER_CD")
                    sFilterFieldName = drv("FILTER_VALUE_FIELD_NAME")
                    Dim sDestFilterCode As String = drv("DEST_FILTER_CD")
                    Dim sDestCompareOp As String = drv("COMPARE_OP_CD")
                    sCurrentFilter = ""

                    cf = global_filters.Values.Where(Function(f) f.FilterCode = sFilterCode).FirstOrDefault
                    Select Case cf.FilterCode
                        Case Else
                            sCurrentFilter = "(1=2 "
                            For Each cfdi In cf.DataItems
                                Select Case cf.Filter.DataType
                                    Case "T"
                                        sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(sDestCompareOp) & "'" & cfdi.Text1Value & "'"
                                    Case "N"
                                        sCurrentFilter &= " OR " & sFilterFieldName & getCompareOp(sDestCompareOp) & cfdi.Number1Value & " "
                                End Select
                            Next
                            sCurrentFilter &= ")"
                    End Select

                    sFilter &= " AND " & sCurrentFilter
                Next

                If a.hasSplits Then
                    For Each sp As AudienceSplit In a.Splits
                        sSplitFilter = String.Format(" AND split_value='{0}'", sp.SplitValue)

                        If bEMail And bDMail Then
                            sFinalFilter = sFilter & sSplitFilter & " AND usable_e_mail_fl='N'"
                            dv.RowFilter = sFinalFilter
                            sp.DMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))   'dv.Table.Compute("sum(quantity)", sFinalFilter) '..Count
                            sp.DMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(d_mail_price)", sFinalFilter)), 0))

                            sFinalFilter = sFilter & sSplitFilter & " AND usable_e_mail_fl='Y'"
                            dv.RowFilter = sFinalFilter
                            sp.EMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                            sp.EMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(e_mail_price)", sFinalFilter)), 0))

                        ElseIf bEMail Then
                            sp.DMailQuantity = 0
                            sp.DMailTotalPrice = 0

                            sFinalFilter = sFilter & sSplitFilter & " AND usable_e_mail_fl='Y'"
                            dv.RowFilter = sFinalFilter
                            sp.EMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                            sp.EMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(e_mail_price)", sFinalFilter)), 0))

                        ElseIf bDMail Then
                            sFinalFilter = sFilter & sSplitFilter & " AND usable_d_mail_fl='Y'"
                            dv.RowFilter = sFinalFilter
                            sp.DMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                            sp.DMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(d_mail_price)", sFinalFilter)), 0))

                            sp.EMailQuantity = 0
                            sp.EMailTotalPrice = 0

                        Else
                            sp.DMailQuantity = 0
                            sp.DMailTotalPrice = 0
                            sp.EMailQuantity = 0
                            sp.EMailTotalPrice = 0
                        End If
                    Next

                Else 'regular audience
                    dv.RowFilter = sFilter
                    If bEMail And bDMail Then
                        sFinalFilter = sFilter & " AND usable_e_mail_fl='N'"
                        dv.RowFilter = sFinalFilter
                        a.DMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                        a.DMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(d_mail_price)", sFinalFilter)), 0))

                        sFinalFilter = sFilter & " AND usable_e_mail_fl='Y'"
                        dv.RowFilter = sFinalFilter
                        a.EMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                        a.EMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(e_mail_price)", sFinalFilter)), 0))

                    ElseIf bEMail Then
                        a.DMailQuantity = 0
                        a.DMailTotalPrice = 0

                        sFinalFilter = sFilter & " AND usable_e_mail_fl='Y'"
                        dv.RowFilter = sFinalFilter
                        a.EMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0)) ' dv.Count
                        a.EMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(e_mail_price)", sFinalFilter)), 0))

                    ElseIf bDMail Then
                        sFinalFilter = sFilter & " AND usable_d_mail_fl='Y'"
                        dv.RowFilter = sFinalFilter
                        a.DMailQuantity = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(record_count)", sFinalFilter)), 0))    'dv.Count
                        a.DMailTotalPrice = CDbl(If(Utility.Data.NothingIfNull(dtData.Compute("sum(d_mail_price)", sFinalFilter)), 0))

                        a.EMailQuantity = 0
                        a.EMailTotalPrice = 0

                    Else
                        a.DMailQuantity = 0
                        a.DMailTotalPrice = 0
                        a.EMailQuantity = 0
                        a.EMailTotalPrice = 0
                    End If
                End If
            Next
        End Sub

        Private Function getCompareOp(compare_op As String) As String
            Dim sCompareOp As String
            Select Case compare_op
                Case "EQ", "IN"
                    sCompareOp = "="
                Case "NE", "NI"
                    sCompareOp = "<>"
                Case "LT"
                    sCompareOp = "<"
                Case "LE"
                    sCompareOp = "<="
                Case "GT"
                    sCompareOp = ">"
                Case "GE"
                    sCompareOp = ">="
            End Select

            Return sCompareOp
        End Function
    End Class

    <Serializable()>
    Public Class Audience
        Protected m_SubCampaignID As Integer
        Protected m_SubAudienceID As Integer
        Protected m_ProgAudienceID As Integer
        Protected m_ChannelTypeCode As String
        Protected m_LanguageCode As String
        Protected m_NameResourceCode As String
        Protected m_DescResourceCode As String
        Protected m_DisplayOrder As Integer
        Protected m_HideWhenNoData As Boolean = False
        Protected m_ShowSelect As Boolean
        Protected m_AllowDeSelect As Boolean
        Protected m_Selected As Boolean

        Protected m_AllowLimit As Boolean
        Protected m_MailLimit As Integer?
        Protected m_DMailQuantity As Integer?
        Protected m_AllowDMailLimit As Boolean
        Protected m_DMailLimit As Integer?
        Protected m_EMailQuantity As Integer?
        Protected m_AllowEMailLimit As Boolean
        Protected m_EMailLimit As Integer?

        Protected m_DMailUnitPrice As Double?
        Protected m_EMailUnitPrice As Double?
        Protected m_DMailTotalPrice As Double?
        Protected m_EMailTotalPrice As Double?

        Protected m_Splits As New Programs.AudienceSplitCollection

        Protected m_SetupFilterSetCode As String
        Protected m_SetupFilterSet As DealerDirect.CampaignManagement.DataFilters.ContainerFilterSet
        Protected m_FilterConfigs As New DealerDirect.CampaignManagement.Programs.Filters.FilterConfigCollection
        Protected m_CurrentFilterData As New DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection

        Public ReadOnly Property TotalMailQuantity() As Integer
            Get
                Return If(DMailQuantity, 0) + If(EMailQuantity, 0)
            End Get
        End Property
        Public Property DMailQuantity() As Integer?
            Get
                Return m_DMailQuantity
            End Get
            Set(ByVal value As Integer?)
                m_DMailQuantity = value
            End Set
        End Property
        Public Property EMailQuantity() As Integer?
            Get
                Return m_EMailQuantity
            End Get
            Set(ByVal value As Integer?)
                m_EMailQuantity = value
            End Set
        End Property
        Public ReadOnly Property HideWhenNoData() As Boolean
            Get
                Return m_HideWhenNoData
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
        Public Property CurrentFilterData() As DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection
            Get
                Return m_CurrentFilterData
            End Get
            Set(ByVal value As DealerDirect.CampaignManagement.DataFilters.ContainerFilterCollection)
                m_CurrentFilterData = value
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


        Public Sub New()
        End Sub

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            MyBase.New()
            Me.attachBaseProperties(irdr_on_row)
        End Sub

        Public Sub attachBaseProperties(ByVal irdr_on_row As System.Data.IDataReader)
            m_SubCampaignID = irdr_on_row("sub_campaign_id")
            m_SubAudienceID = irdr_on_row("sub_audience_id")
            m_ProgAudienceID = irdr_on_row("prog_audience_id")
            m_NameResourceCode = irdr_on_row("name_resource_cd")
            m_DescResourceCode = Utility.Data.NothingIfNull(irdr_on_row("desc_resource_cd"))
            m_DisplayOrder = irdr_on_row("display_order_nbr")

            m_AllowLimit = Utility.Data.FlagToBoolean(irdr_on_row("allow_limit_fl"))
            m_AllowDeSelect = Utility.Data.FlagToBoolean(irdr_on_row("allow_deselect_fl"))
            m_ShowSelect = Utility.Data.FlagToBoolean(irdr_on_row("show_select_fl"))
            m_Selected = Utility.Data.FlagToBoolean(irdr_on_row("selected_fl"))

            m_HideWhenNoData = Utility.Data.FlagToBoolean(irdr_on_row("hide_when_no_data_fl"))

            If irdr_on_row("d_mail_est_quantity") IsNot System.DBNull.Value Then m_DMailQuantity = CInt(irdr_on_row("d_mail_est_quantity"))
            If irdr_on_row("e_mail_est_quantity") IsNot System.DBNull.Value Then m_EMailQuantity = CInt(irdr_on_row("e_mail_est_quantity"))

            m_AllowDMailLimit = Utility.Data.FlagToBoolean(irdr_on_row("allow_d_mail_limit_fl"))
            m_AllowEMailLimit = Utility.Data.FlagToBoolean(irdr_on_row("allow_e_mail_limit_fl"))
            If irdr_on_row("d_mail_limit") IsNot System.DBNull.Value Then m_DMailLimit = CInt(irdr_on_row("d_mail_limit"))
            If irdr_on_row("e_mail_limit") IsNot System.DBNull.Value Then m_EMailLimit = CInt(irdr_on_row("e_mail_limit"))
        End Sub


        Protected Friend Function Save() As Boolean
            Dim cmd As Devart.Data.Oracle.OracleCommand

            If m_SetupFilterSet IsNot Nothing Then
                m_SetupFilterSet.Save()
            End If

            ' save the splits 
            For Each s As Programs.AudienceSplit In Me.m_Splits
                s.Save()
            Next

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = Data.createUserConnectionD
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_aud.saveAudienceSettings"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_prog_audience_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_selected_fl", Devart.Data.Oracle.OracleDbType.Char)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_mail_limit", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_d_mail_limit", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_e_mail_limit", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
            cmd.Parameters("n_prog_audience_id").Value = m_ProgAudienceID
            cmd.Parameters("c_selected_fl").Value = Utility.Data.BooleanToFlag(Me.m_Selected)
            If m_DMailLimit.HasValue Then cmd.Parameters("n_d_mail_limit").Value = m_DMailLimit.Value
            If m_EMailLimit.HasValue Then cmd.Parameters("n_e_mail_limit").Value = m_EMailLimit.Value

            cmd.ExecuteNonQuery()

            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return True
        End Function

        Public ReadOnly Property ID() As Integer
            Get
                Return Me.m_ProgAudienceID
            End Get
        End Property

        Public ReadOnly Property SubAudienceID() As Integer
            Get
                Return Me.m_SubAudienceID
            End Get
        End Property

        Public ReadOnly Property DisplayOrder() As Integer
            Get
                Return m_DisplayOrder
            End Get
        End Property

        Public ReadOnly Property AllowDeSelect() As Boolean
            Get
                Return m_AllowDeSelect
            End Get
        End Property
        Public Property Selected() As Boolean
            Get
                Return m_Selected
            End Get
            Set(ByVal value As Boolean)
                m_Selected = value
            End Set
        End Property
        Public Property ShowSelect() As Boolean
            Get
                Return m_ShowSelect
            End Get
            Set(ByVal value As Boolean)
                m_ShowSelect = value
            End Set
        End Property

        Public Property DMailTotalPrice() As Double?
            Get
                Return m_DMailTotalPrice
            End Get
            Set(ByVal value As Double?)
                m_DMailTotalPrice = value
            End Set
        End Property
        Public Property EMailTotalPrice() As Double?
            Get
                Return m_EMailTotalPrice
            End Get
            Set(ByVal value As Double?)
                m_EMailTotalPrice = value
            End Set
        End Property

        Public Property DMailUnitPrice() As Double?
            Get
                Return m_DMailUnitPrice
            End Get
            Set(ByVal value As Double?)
                m_DMailUnitPrice = value
            End Set
        End Property
        Public Property EMailUnitPrice() As Double?
            Get
                Return m_EMailUnitPrice
            End Get
            Set(ByVal value As Double?)
                m_EMailUnitPrice = value
            End Set
        End Property

        Public ReadOnly Property AllowLimit() As Boolean
            Get
                Return m_AllowLimit
            End Get
        End Property
        Public Property MailLimit() As Integer?
            Get
                Return m_MailLimit
            End Get
            Set(ByVal value As Integer?)
                m_MailLimit = value
            End Set
        End Property

        Public ReadOnly Property AllowDMailLimit() As Boolean
            Get
                Return m_AllowDMailLimit
            End Get
        End Property
        Public Property DMailLimit() As Integer?
            Get
                Return m_DMailLimit
            End Get
            Set(ByVal value As Integer?)
                m_DMailLimit = value
            End Set
        End Property

        Public ReadOnly Property AllowEMailLimit() As Boolean
            Get
                Return m_AllowEMailLimit
            End Get
        End Property
        Public Property EMailLimit() As Integer?
            Get
                Return m_EMailLimit
            End Get
            Set(ByVal value As Integer?)
                m_EMailLimit = value
            End Set
        End Property
        Public ReadOnly Property ShowLimits() As Boolean
            Get
                Return m_AllowDMailLimit Or m_AllowEMailLimit
            End Get
        End Property

        Public ReadOnly Property hasSplits() As Boolean
            Get
                Return m_Splits.Count > 0
            End Get
        End Property
        Public Property Splits() As Programs.AudienceSplitCollection
            Get
                Return m_Splits
            End Get
            Set(ByVal value As Programs.AudienceSplitCollection)
                m_Splits = value
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

        Public ReadOnly Property hasDescription() As Boolean
            Get
                Return m_DescResourceCode IsNot Nothing
            End Get
        End Property
        Public ReadOnly Property Description() As String
            Get
                Return DealerDirect.Resources.getString(m_DescResourceCode)
            End Get
        End Property
    End Class
    <Serializable()>
    Public Class AudienceCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, Programs.Audience)

        Protected Overrides Function GetKeyForItem(ByVal item As Programs.Audience) As Integer
            Return item.ID
        End Function

        Public Function SaveAll() As Boolean
            Dim b As Boolean = True
            For Each aud As Programs.Audience In MyBase.Dictionary.Values
                If Not aud.Save() Then b = False
            Next

            Return b
        End Function
    End Class

    <Serializable()>
    Public Class AudienceSplit
        Protected m_SubCampaignID As Integer
        Protected m_ProgAudienceID As Integer
        Protected m_ProgAudSplitID As Integer

        Protected m_NameResourceCode As String
        Protected m_DescResourceCode As String
        Protected m_DisplayOrder As Integer

        Protected m_AllowDeSelect As Boolean
        Protected m_Selected As Boolean
        Protected m_SplitValue As String

        Protected m_DMailQuantity As Integer?
        Protected m_EMailQuantity As Integer?

        Protected m_DMailUnitPrice As Double?
        Protected m_EMailUnitPrice As Double?

        Protected m_DMailTotalPrice As Double?
        Protected m_EMailTotalPrice As Double?

        Public Sub New()
        End Sub

        Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
            MyBase.New()
            Me.attachBaseProperties(irdr_on_row)
        End Sub

        Public Sub attachBaseProperties(ByVal irdr_on_row As System.Data.IDataReader)
            m_SubCampaignID = irdr_on_row("sub_campaign_id")
            m_ProgAudienceID = irdr_on_row("prog_audience_id")
            m_ProgAudSplitID = irdr_on_row("prog_aud_split_id")
            m_NameResourceCode = Utility.Data.NothingIfNull(irdr_on_row("name_resource_cd"))
            m_DescResourceCode = Utility.Data.NothingIfNull(irdr_on_row("desc_resource_cd"))
            m_DisplayOrder = irdr_on_row("display_order_nbr")

            m_SplitValue = Utility.Data.NothingIfNull(irdr_on_row("split_value_cd"))

            m_AllowDeSelect = Utility.Data.FlagToBoolean(irdr_on_row("allow_deselect_fl"))
            m_Selected = Utility.Data.FlagToBoolean(irdr_on_row("selected_fl"))
        End Sub

        Protected Friend Function Save() As Boolean
            Dim cmd As Devart.Data.Oracle.OracleCommand

            cmd = New Devart.Data.Oracle.OracleCommand
            cmd.Connection = Data.createUserConnectionD
            cmd.CommandType = System.Data.CommandType.StoredProcedure
            cmd.CommandText = "wsdp066_core_aud.saveAudienceSplitSettings"
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_context_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_sub_campaign_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("n_prog_aud_split_id", Devart.Data.Oracle.OracleDbType.Integer)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Add(New Devart.Data.Oracle.OracleParameter("c_selected_fl", Devart.Data.Oracle.OracleDbType.Char)).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters("n_context_id").Value = DealerDirect.Security.CurrentSession.SecurityContext.ID
            cmd.Parameters("n_sub_campaign_id").Value = m_SubCampaignID
            cmd.Parameters("n_prog_aud_split_id").Value = m_ProgAudSplitID
            cmd.Parameters("c_selected_fl").Value = Utility.Data.BooleanToFlag(Me.m_Selected)
            cmd.ExecuteNonQuery()

            cmd.Connection.Close()
            cmd.Connection.Dispose()
            cmd.Dispose()

            Return True
        End Function

        Public ReadOnly Property SplitValue() As String
            Get
                Return m_SplitValue
            End Get
        End Property
        Public ReadOnly Property ID() As Integer
            Get
                Return Me.m_ProgAudSplitID
            End Get
        End Property

        Public ReadOnly Property AudienceID() As Integer
            Get
                Return Me.m_ProgAudienceID
            End Get
        End Property

        Public ReadOnly Property DisplayOrder() As Integer
            Get
                Return m_DisplayOrder
            End Get
        End Property

        Public ReadOnly Property AllowDeSelect() As Boolean
            Get
                Return m_AllowDeSelect
            End Get
        End Property
        Public Property Selected() As Boolean
            Get
                Return m_Selected
            End Get
            Set(ByVal value As Boolean)
                m_Selected = value
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

        Public ReadOnly Property hasDescription() As Boolean
            Get
                Return m_DescResourceCode IsNot Nothing
            End Get
        End Property
        Public ReadOnly Property Description() As String
            Get
                Return DealerDirect.Resources.getString(m_DescResourceCode)
            End Get
        End Property

        Public Property DMailQuantity() As Integer?
            Get
                Return m_DMailQuantity
            End Get
            Set(ByVal value As Integer?)
                m_DMailQuantity = value
            End Set
        End Property
        Public Property EMailQuantity() As Integer?
            Get
                Return m_EMailQuantity
            End Get
            Set(ByVal value As Integer?)
                m_EMailQuantity = value
            End Set
        End Property

        Public Property DMailTotalPrice() As Double?
            Get
                Return m_DMailTotalPrice
            End Get
            Set(ByVal value As Double?)
                m_DMailTotalPrice = value
            End Set
        End Property
        Public Property EMailTotalPrice() As Double?
            Get
                Return m_EMailTotalPrice
            End Get
            Set(ByVal value As Double?)
                m_EMailTotalPrice = value
            End Set
        End Property

        Public Property DMailUnitPrice() As Double?
            Get
                Return m_DMailUnitPrice
            End Get
            Set(ByVal value As Double?)
                m_DMailUnitPrice = value
            End Set
        End Property
        Public Property EMailUnitPrice() As Double?
            Get
                Return m_EMailUnitPrice
            End Get
            Set(ByVal value As Double?)
                m_EMailUnitPrice = value
            End Set
        End Property
    End Class
    <Serializable()>
    Public Class AudienceSplitCollection
        Inherits System.Collections.ObjectModel.KeyedCollection(Of Integer, Programs.AudienceSplit)

        Protected Overrides Function GetKeyForItem(ByVal item As Programs.AudienceSplit) As Integer
            Return item.ID
        End Function
    End Class
End Namespace