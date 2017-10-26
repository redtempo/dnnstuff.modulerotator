Option Explicit On
Option Strict On

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions

Namespace DNNStuff.ModuleRotator

    Partial Class EditModuleRotator

        Inherits Entities.Modules.PortalModuleBase

        ' buttons
        ' timer

        Private NumberOfModules As Integer = 0

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            MyBase.HelpURL = "http://www.dnnstuff.com/"
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If DNNUtilities.SafeDNNVersion().Major = 5 Then
                    DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit_5.css"))
                Else
                    DNNUtilities.InjectCSS(Me.Page, ResolveUrl("Resources/Support/edit.css"))
                End If
                Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "yeti", ResolveUrl("resources/support/yetii-min.js"))

                ' Obtain PortalSettings from Current Context
                Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

                If Page.IsPostBack = False Then

                    BindData()

                    ' Store URL Referrer to return to portal
                    ViewState("UrlReferrer") = Replace(Convert.ToString(Request.UrlReferrer), "insertrow=true&", "")

                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                If Page.IsValid Then
                    ' style
                    objModules.UpdateModuleSetting(ModuleId, "HideTitles", chkHideTitles.Checked.ToString)
                    ' timer
                    objModules.UpdateModuleSetting(ModuleId, "TimerEnabled", chkTimerEnabled.Checked.ToString)
                    objModules.UpdateModuleSetting(ModuleId, "TimerDelay", txtTimerDelay.Text)
                    objModules.UpdateModuleSetting(ModuleId, "HoverEnabled", chkHoverEnabled.Checked.ToString)
                    objModules.UpdateTabModuleOrder(TabId)

                    Entities.Modules.ModuleController.SynchronizeModule(ModuleId)

                    ' Redirect back to the portal home page
                    Response.Redirect(NavigateURL(), True)
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub grdModules_CancelEdit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            Try
                grdModules.EditItemIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub grdModules_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            Try
                grdModules.EditItemIndex = e.Item.ItemIndex
                grdModules.SelectedIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Sub grdModules_Item_Bound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            Try
                Dim itemType As ListItemType = CType(e.Item.ItemType, ListItemType)
                If (itemType = ListItemType.EditItem) Then
                    Dim drv As DataRowView = CType(e.Item.DataItem, DataRowView)

                    Dim dr As IDataReader
                    dr = DataProvider.Instance().DNNStuff_ModuleRotator_GetTabModules(TabId, ModuleId)

                    Dim cboModule As WebControls.DropDownList
                    cboModule = CType(e.Item.FindControl("cboModule"), WebControls.DropDownList)
                    cboModule.DataSource = dr
                    cboModule.DataValueField = "TabModuleId"
                    cboModule.DataTextField = "ModuleTitle"
                    cboModule.DataBind()

                    ' add existing item
                    If Convert.ToInt32(drv("TabModuleId")) <> -1 Then
                        cboModule.Items.Add(New ListItem(drv("ModuleTitle").ToString, drv("TabModuleId").ToString))
                    End If

                    Dim item As ListItem = cboModule.Items.FindByValue(drv("TabModuleId").ToString)
                    If Not item Is Nothing Then item.Selected = True

                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub grdModules_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

            Try
                Dim cboModule As DropDownList = CType(e.Item.FindControl("cboModule"), WebControls.DropDownList)

                If cboModule.SelectedValue <> "" Then
                    Dim ctrl As New ModuleRotatorController
                    Dim info As New ModuleRotatorInfo

                    If Integer.Parse(Convert.ToString(grdModules.DataKeys(e.Item.ItemIndex))) = -1 Then
                        ' add
                        With info
                            .ModuleRotatorId = -1
                            .ModuleId = ModuleId
                            .TabModuleId = Convert.ToInt32(cboModule.SelectedItem.Value)
                        End With
                    Else
                        ' update
                        With info
                            .ModuleRotatorId = Convert.ToInt32(grdModules.DataKeys(e.Item.ItemIndex))
                            .ModuleId = ModuleId
                            .TabModuleId = Convert.ToInt32(cboModule.SelectedItem.Value)
                        End With
                    End If
                    ctrl.DNNStuff_ModuleRotator_UpdateModuleRotator(info)

                    grdModules.EditItemIndex = -1
                    BindData()
                Else
                    grdModules.EditItemIndex = -1
                    BindData()
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub grdModules_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            Try
                Dim ctrl As New ModuleRotatorController

                ctrl.DNNStuff_ModuleRotator_DeleteModuleRotator(Integer.Parse(Convert.ToString(grdModules.DataKeys(e.Item.ItemIndex))))

                grdModules.EditItemIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub grdModules_Move(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
            Try
                Dim ctrl As New ModuleRotatorController

                Select Case e.CommandArgument.ToString
                    Case "Up"
                        ctrl.DNNStuff_ModuleRotator_UpdateTabOrder(Integer.Parse(Convert.ToString(grdModules.DataKeys(e.Item.ItemIndex))), -1)
                        BindData()
                    Case "Down"
                        ctrl.DNNStuff_ModuleRotator_UpdateTabOrder(Integer.Parse(Convert.ToString(grdModules.DataKeys(e.Item.ItemIndex))), 1)
                        BindData()
                End Select
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdAddModule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddModule.Click
            Try
                grdModules.EditItemIndex = 0
                BindData(True)
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub BindData(Optional ByVal blnInsertField As Boolean = False)

            Dim ctrl As New ModuleRotatorController
            Dim dr As IDataReader = ctrl.DNNStuff_ModuleRotator_GetModuleRotator(ModuleId)
            Dim ds As DataSet

            ds = ConvertDataReaderToDataSet(dr)
            dr.Close()

            ' save number of modules
            NumberOfModules = ds.Tables(0).Rows.Count

            ' inserting a new field
            If blnInsertField Then
                Dim row As DataRow
                row = ds.Tables(0).NewRow()
                row("ModuleRotatorId") = "-1"
                row("TabModuleId") = "-1"
                ds.Tables(0).Rows.InsertAt(row, 0)
                grdModules.EditItemIndex = 0
            End If

            grdModules.DataSource = ds
            grdModules.DataBind()

            ' hide if nothing and not inserting
            grdModules.Visible = Not (NumberOfModules = 0 And Not blnInsertField)

            ' get module settings
            Dim modController As New Entities.Modules.ModuleController
            Dim settings As Hashtable = modController.GetModuleSettings(ModuleId)

            ' style settings
            chkHideTitles.Checked = Convert.ToBoolean(DNNUtilities.GetSetting(settings, "HideTitles", "True"))

            ' timer settings
            chkTimerEnabled.Checked = Convert.ToBoolean(DNNUtilities.GetSetting(settings, "TimerEnabled", "False"))
            txtTimerDelay.Text = DNNUtilities.GetSetting(settings, "TimerDelay", "1000")
            chkHoverEnabled.Checked = Convert.ToBoolean(DNNUtilities.GetSetting(settings, "HoverEnabled", "False"))
        End Sub

        Private Sub grdModules_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdModules.ItemCreated
            Try
                Dim cmdDeleteModule As Control = e.Item.FindControl("cmdDeleteModule")

                If Not cmdDeleteModule Is Nothing Then
                    CType(cmdDeleteModule, ImageButton).Attributes.Add("onClick", "javascript: return confirm('Are You Sure You Wish To Delete This Module ?')")
                End If

                If e.Item.ItemType = ListItemType.Header Then
                    e.Item.Cells(1).Attributes.Add("Scope", "col")
                    e.Item.Cells(2).Attributes.Add("Scope", "col")
                    e.Item.Cells(3).Attributes.Add("Scope", "col")
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdModules_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdModules.ItemDataBound
            Dim ib As ImageButton
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If e.Item.ItemIndex = 0 Then
                    ib = CType(e.Item.FindControl("cmdMoveTabModuleUp"), ImageButton)
                    If Not ib Is Nothing Then
                        ib.Visible = False
                    End If
                Else
                    If e.Item.ItemIndex = NumberOfModules - 1 Then
                        ib = CType(e.Item.FindControl("cmdMoveTabModuleDown"), ImageButton)
                        If Not ib Is Nothing Then
                            ib.Visible = False
                        End If
                    End If

                End If
            End If
        End Sub
    End Class

End Namespace