Namespace DealerDirect.CampaignManagement.Programs.Filters
	<Serializable()> _
 Public Class FilterConfigCollection
		Inherits System.Collections.ObjectModel.Collection(Of FilterConfig)
	End Class

	<Serializable()> _
 Public Class FilterConfig
		Protected m_FilterCode As String
		Protected m_ContainerTypeCode As String
		Protected m_ContainerID As Integer
		Protected m_TableRow As Integer
		Protected m_TableColumn As Integer
		Protected m_TableRowSpan As Integer
		Protected m_TableColSpan As Integer
		Protected m_OptionString As String
		Protected m_EditControl As String
        Protected m_AllowEdit As Boolean = False
        Protected m_ShowFilter As Boolean

        Public Sub New()
		End Sub

		Public Sub New(ByVal irdr_on_row As System.Data.IDataReader)
			MyBase.New()
			Me.attachBaseProperties(irdr_on_row)
		End Sub

		Public Sub attachBaseProperties(ByVal irdr_on_row As System.Data.IDataReader)
			m_FilterCode = irdr_on_row("filter_cd")
			m_ContainerTypeCode = irdr_on_row("container_type_cd")
			m_ContainerID = irdr_on_row("container_id")

			m_TableRow = irdr_on_row("table_row_nbr")
			m_TableColumn = irdr_on_row("table_column_nbr")
			m_TableRowSpan = irdr_on_row("table_rowspan")
			m_TableColSpan = irdr_on_row("table_colspan")
			m_OptionString = Utility.Data.NothingIfNull(irdr_on_row("filter_options"))
			m_EditControl = Utility.Data.NothingIfNull(irdr_on_row("edit_control"))

            m_AllowEdit = Utility.Data.FlagToBoolean(irdr_on_row("allow_edit_fl"))
            m_ShowFilter = Utility.Data.FlagToBoolean(irdr_on_row("show_filter_fl"))
        End Sub

		Public Property Code() As String
			Get
				Return m_FilterCode
			End Get
			Set(ByVal value As String)
				m_FilterCode = value
			End Set
		End Property

		Public ReadOnly Property ContainerTypeCode() As String
			Get
				Return m_ContainerTypeCode
			End Get
		End Property

		Public ReadOnly Property ContainerID() As Integer
			Get
				Return m_ContainerID
			End Get
		End Property

		Public Property TableRow() As Integer
			Get
				Return m_TableRow
			End Get
			Set(ByVal value As Integer)
				m_TableRow = value
			End Set
		End Property
		Public Property TableColumn() As Integer
			Get
				Return m_TableColumn
			End Get
			Set(ByVal value As Integer)
				m_TableColumn = value
			End Set
		End Property
		Public Property TableRowSpan() As Integer
			Get
				Return m_TableRowSpan
			End Get
			Set(ByVal value As Integer)
				m_TableRowSpan = value
			End Set
		End Property
		Public Property TableColSpan() As Integer
			Get
				Return m_TableColSpan
			End Get
			Set(ByVal value As Integer)
				m_TableColSpan = value
			End Set
		End Property

		Public ReadOnly Property EditControl() As String
			Get
				Return m_EditControl
			End Get
		End Property

		Public ReadOnly Property OptionString() As String
			Get
				Return m_OptionString
			End Get
		End Property
		Public ReadOnly Property getOption(ByVal option_code As String) As String
			Get
				Return Programs.Utilities.getOption(m_OptionString, option_code)
			End Get
		End Property

        Public ReadOnly Property AllowEdit As Boolean
            Get
                Return m_AllowEdit
            End Get
        End Property

        Public ReadOnly Property ShowFilter As Boolean
            Get
                Return m_ShowFilter
            End Get
        End Property
    End Class
End Namespace

