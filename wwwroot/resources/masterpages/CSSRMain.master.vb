Imports DealerDirect.Personalization
Imports System.Web.Script.Serialization

Partial Class resources_masterpages_cssrmain
    Inherits System.Web.UI.MasterPage

    Protected ImageUrl As String
    Protected LinkUrl As String
    Protected Title As String
    Protected NewWindow As String
    Protected HasBanner As String = "No"
    Protected CannotView As Boolean

    Public Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(resources_masterpages_cssrmain))

    Public Shared Function getResource(ByVal resourceCD As String) As String

        Return (New JavaScriptSerializer().Serialize(DealerDirect.Resources.getString(resourceCD)))

    End Function

    Public Shared Function getResourceList(ByVal resourceCD As String) As String

        Return (New JavaScriptSerializer().Serialize(DealerDirect.Resources.getList(resourceCD)))

    End Function

    Public Shared Function getSessionMenu() As String

        Return (New JavaScriptSerializer().Serialize(DealerDirect.Administration.User.UserHelper.getUserMenuItems()))

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            'check visibility based on rules
            Dim dealer_id As Integer = DealerDirect.Security.CurrentSession.SecurityContext.WorkSpace.ID
            Dim default_dealer_id As Integer = DealerDirect.Security.CurrentSession.User.DefaultWorkspace.ID
            Dim hs As Hashtable

            If default_dealer_id <> dealer_id And default_dealer_id = 99901 Or default_dealer_id = 99903 Then
                hs = DealerDirect.Administration.User.UserHelper.getUserPrivilege("DLRSHP", default_dealer_id, "", Page.AppRelativeVirtualPath.Substring(2).Trim.ToLower, "DLRSHP")

                If hs("HaveRule") = "Y" AndAlso hs("HavePrivilege") <> "Y" Then
                    CannotView = True
                End If
            End If

            If CannotView = False Then
                hs = DealerDirect.Administration.User.UserHelper.getUserPrivilege("USER", DealerDirect.Security.CurrentSession.User.UserID, "", Page.AppRelativeVirtualPath.Substring(2).Trim.ToLower, "DLR")

                If hs("HaveRule") = "Y" Then  'the page is controlled by a rule
                    If Not (dealer_id = 80000 Or dealer_id = 99901) Then
                        If hs("HavePrivilege") <> "Y" Then
                            CannotView = True
                        End If
                    End If
                End If
            End If

            If CannotView = True Then
                Response.Redirect("~/NoAccess.aspx")
            End If


            Dim BannerRoot As DealerDirect.Personalization.Links.LinkItem
            Dim Banner As DealerDirect.Personalization.Links.LinkItem

            If Session("CM2:GETCONTEXTLINKS:GMDD_MENU_ITEMS") Is Nothing Then
                BannerRoot = DealerDirect.Personalization.Links.getContextLinks(DealerDirect.Security.CurrentSession.SecurityContext.ID, "BANR_RTR")
                Session("CM2:GETCONTEXTLINKS:GMDD_MENU_ITEMS") = BannerRoot
            Else
                BannerRoot = Session("CM2:GETCONTEXTLINKS:GMDD_MENU_ITEMS")
            End If

            If BannerRoot.SubLinkItems.Count > 0 Then
                If Session("BannerOrder") Is Nothing Then
                    Session("BannerOrder") = 0
                ElseIf Session("BannerOrder") < BannerRoot.SubLinkItems.Count - 1 And Session("BannerOrder") < 4 Then
                    Session("BannerOrder") += 1
                Else
                    Session("BannerOrder") = 0
                End If

                Banner = DirectCast(BannerRoot.SubLinkItems.Item(Session("BannerOrder")), DealerDirect.Personalization.Links.LinkItem)

                ViewState("ImageUrl") = Banner.ImageURL.ToString().Replace("{LANGUAGE_CODE}", DealerDirect.Security.CurrentSession.Language)
                ViewState("LinkUrl") = Banner.Link.ToString()
                ViewState("Title") = Banner.Label
                ViewState("NewWindow") = Banner.UseNewWindow

            End If

        End If 'End of not isPostBack

        If Not ViewState("ImageUrl") Is Nothing Then
            HasBanner = "Yes"
            ImageUrl = ViewState("ImageUrl")
            LinkUrl = ViewState("LinkUrl")
            Title = ViewState("Title")
            NewWindow = ViewState("NewWindow")
        End If

        If Not Page.IsPostBack AndAlso Session("URL") <> Page.AppRelativeVirtualPath AndAlso Not DealerDirect.Security.CurrentSession Is Nothing Then
            Dim Userid As Integer = DealerDirect.Security.CurrentSession.User.UserID
            DealerDirect.ContentManagement.Media.addViewLog(Userid, Page.AppRelativeVirtualPath, "PAGE_LOG")
        End If

        Session("Online_User_Timer") = 0
        Session("URL") = Page.AppRelativeVirtualPath
    End Sub
End Class

