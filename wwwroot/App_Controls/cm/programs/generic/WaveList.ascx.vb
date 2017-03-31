Namespace DealerDirect.UserControls.CampaignManagement.Programs.Generic
	Partial Class WaveList
		Inherits System.Web.UI.UserControl

		Protected m_Program As DealerDirect.CampaignManagement.Programs.WaveProgram
		Protected m_InitialLoad As Boolean

		'Protected m_DMailTotalQuantity As Integer?
		'Protected m_DMailTotalPrice As Double?
		'Protected m_EMailTotalQuantity As Integer?
		'Protected m_EMailTotalPrice As Double?

		Public Event onValidateData(ByRef sender As Object, ByRef valid As Boolean)

		Public Sub bubbleOnValidateData(ByRef sender As Object, ByRef valid As Boolean)
			RaiseEvent onValidateData(sender, valid)
		End Sub

		Public Property Program() As DealerDirect.CampaignManagement.Programs.WaveProgram
			Get
				Return m_Program
			End Get
			Set(ByVal value As DealerDirect.CampaignManagement.Programs.WaveProgram)
				m_Program = value
			End Set
		End Property

		Public ReadOnly Property HideLimits() As Boolean
			Get
				Return True
			End Get
		End Property

		'Public ReadOnly Property DMailUnitPrice() As Double?
		'	Get
		'		Return m_Program.AverageDMailUnitPrice
		'	End Get
		'End Property
		'Public ReadOnly Property EMailUnitPrice() As Double?
		'	Get
		'		Return m_Program.AverageEMailUnitPrice
		'	End Get
		'End Property

		'Public ReadOnly Property DMailTotalQuantity() As Integer?
		'	Get
		'		Return m_DMailTotalQuantity
		'	End Get
		'End Property
		'Public ReadOnly Property EMailTotalQuantity() As Integer?
		'	Get
		'		Return m_EMailTotalQuantity
		'	End Get
		'End Property

		'Public ReadOnly Property DMailTotalPrice() As Double?
		'	Get
		'		Return m_DMailTotalPrice
		'	End Get
		'End Property
		'Public ReadOnly Property EMailTotalPrice() As Double?
		'	Get
		'		Return m_EMailTotalPrice
		'	End Get
		'End Property


		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Me.loadControls()
		End Sub

		Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			'Dim isae As DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor
			'Dim ctrl As System.Web.UI.Control
			'Dim nTotalDMQuantity As Integer = 0
			'Dim nTotalDMPrice As Double = 0
			'Dim nTotalEMQuantity As Integer = 0
			'Dim nTotalEMPrice As Double = 0
			'Dim bValid As Boolean = True

			'' update the audience filter data
			'For Each ctrl In Me.phAudiences.Controls
			'	If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor Then
			'		isae = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor)
			'		isae.updateCurrentFilterData(m_Program)
			'	End If
			'Next

			'' load the esimates
			'If m_Program IsNot Nothing Then m_Program.loadEstimates(m_Program.CurrentFilterData)

			'' set the totals
			'For Each ctrl In Me.phAudiences.Controls
			'	If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor Then
			'		isae = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor)

			'		If isae.isSelected Then
			'			nTotalDMQuantity += If(isae.DMailTotalQuantity, 0)
			'			nTotalEMQuantity += If(isae.EMailTotalQuantity, 0)

			'			nTotalDMPrice += If(isae.DMailTotalPrice, 0)
			'			nTotalEMPrice += If(isae.EMailTotalPrice, 0)
			'		End If
			'	End If
			'Next

			'm_DMailTotalQuantity = nTotalDMQuantity
			'm_DMailTotalPrice = nTotalDMPrice

			'm_EMailTotalQuantity = nTotalEMQuantity
			'm_EMailTotalPrice = nTotalEMPrice
		End Sub

		Public Sub loadData(ByVal inital_load As Boolean)
			'm_InitialLoad = inital_load

			'If inital_load Then
			'	Dim isae As DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor

			'	For Each ctrl In Me.phAudiences.Controls
			'		If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor Then
			'			isae = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor)
			'			isae.loadData(inital_load)
			'		End If
			'	Next
			'End If
		End Sub

		Public Sub loadControls()
			Dim iwv As DealerDirect.UserControls.CampaignManagement.Programs.IGenericWaveEditor
			Dim ctrl As System.Web.UI.Control

			If m_Program IsNot Nothing Then
				For Each wv As DealerDirect.CampaignManagement.Programs.Wave In m_Program.Waves
					ctrl = Page.LoadControl("~/app_controls/cm/programs/generic/WaveEditor.ascx")
					ctrl.ID = "we" & wv.ID
					iwv = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericWaveEditor)
					iwv.Wave = wv

					phWaveRows.Controls.Add(ctrl)
				Next
			End If
		End Sub

		Public Function validateData() As Boolean
			'Dim isae As DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor
			'Dim ctrl As System.Web.UI.Control
			'Dim bValid As Boolean = True

			'For Each ctrl In Me.phAudiences.Controls
			'	If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor Then
			'		isae = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor)

			'		If Not isae.validateData Then bValid = False
			'	End If
			'Next

			'If bValid Then
			'	Dim bCancel As Boolean = False
			'	RaiseEvent onValidateData(Me, bCancel)
			'	Return Not bCancel
			'Else
			'	Return False
			'End If
		End Function

		Public Function saveDataToObject() As Boolean
			'Dim isae As DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor

			'For Each ctrl As System.Web.UI.Control In Me.phAudiences.Controls
			'	If TypeOf (ctrl) Is DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor Then
			'		isae = DirectCast(ctrl, DealerDirect.UserControls.CampaignManagement.Programs.IGenericSplitAudienceEditor)
			'		If Not isae.saveDataToObject Then Return False
			'	End If
			'Next

			'Return True
		End Function
	End Class
End Namespace