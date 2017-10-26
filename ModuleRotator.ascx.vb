Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Security.Permissions

Namespace DNNStuff.ModuleRotator

    Partial Class ModuleRotator
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.Communications.IModuleListener
        Implements Entities.Modules.IActionable

        ' currently selected tab
        Private _selectedModuleId As String = ""

        ' Contains list of modulesettings we want to inject
        Private _tabModules As ArrayList = New ArrayList

#Region " Properties"
        Public ReadOnly Property HoverEnabled() As Boolean
            Get
                Return Convert.ToBoolean(DNNUtilities.GetSetting(Settings, "HoverEnabled", "False"))
            End Get
        End Property

        Public ReadOnly Property TimerEnabled() As Boolean
            Get
                Return Convert.ToBoolean(DNNUtilities.GetSetting(Settings, "TimerEnabled", "False"))
            End Get
        End Property

        Public ReadOnly Property TimerDelay() As Integer
            Get
                Return Convert.ToInt32(DNNUtilities.GetSetting(Settings, "TimerDelay", "3000"))
            End Get
        End Property

        Public ReadOnly Property HideTitles() As Boolean
            Get
                Return Convert.ToBoolean(DNNUtilities.GetSetting(Settings, "HideTitles", "True"))
            End Get
        End Property


#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            MyBase.HelpURL = "http://www.dnnstuff.com/"

            ' rendor tabs
            RenderModules()
        End Sub

#End Region

#Region " Main Rendering"
        Private Sub RenderModules()

            ' Data objects
            Dim ctrl As New ModuleRotatorController
            Dim dr As IDataReader = ctrl.DNNStuff_ModuleRotator_GetModuleRotator(ModuleId)
            Dim InnerModuleId As Integer

            ' get selected module
            _selectedModuleId = GetSelectedModule()

            Dim SelectedModuleFound As Boolean = False
            Dim moduleNumber As Integer = 1

            Dim blnLayoutMode As Boolean = DotNetNuke.Common.Globals.IsLayoutMode

            ' Load Tab Modules
            While dr.Read
                InnerModuleId = Convert.ToInt32(dr("ModuleId"))    ' use ModuleId

                For Each _moduleSettings As ModuleInfo In PortalSettings.ActiveTab.Modules

                    If _moduleSettings.ModuleID = InnerModuleId Then

                        If ModulePermissionController.CanViewModule(_moduleSettings) Then

                            ' if current date is within module display schedule or user is admin
                            If (_moduleSettings.StartDate < Now And _moduleSettings.EndDate > Now) Or blnLayoutMode = True Then

                                ' modules which are displayed on all tabs should not be displayed on the Admin or Super tabs
                                If _moduleSettings.AllTabs = False Then

                                    ' add modules we want to render in divs
                                    _tabModules.Add(_moduleSettings)
                                    If _selectedModuleId = Me.ClientID & "_TabPage" & moduleNumber.ToString Then
                                        SelectedModuleFound = True
                                    End If
                                    moduleNumber += 1
                                End If
                            End If
                        End If
                    End If
                Next
            End While
            dr.Close()

            ' inject external javascript
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "util.js", "<script src=""" & ResolveUrl("js/util.js") & """></script>")

            If _tabModules.Count > 0 Then

                ' inject inline javascript
                InjectActivateTabPage(_tabModules)
                InjectSelectTab()
                If TimerEnabled Then
                    InjectTabChangeTimer()
                End If

                If Not SelectedModuleFound Then
                    _selectedModuleId = Me.ClientID & "_TabPage1"
                End If

                ' render modules
                RenderModulesNone(_tabModules)

                ' RDE - removed InjectModules and changed to Load
                HideModulesBeingInjected(_tabModules)

            End If
        End Sub

        Private Sub HideModulesBeingInjected(ByVal modules As ArrayList)
            ' setup to hide modules so that DNN doesn't render them outside our tabs .. we'll render them later in page load
            For Each _moduleSettings As ModuleInfo In modules
                If HideTitles Then
                    _moduleSettings.ContainerSrc = ResolveUrl("~/Portals/_default/Containers/_default/no container.ascx")
                End If
                _moduleSettings.IsDeleted = True
            Next

        End Sub

        Public Function FindControlRecursive(ByVal root As Control, ByVal id As String) As Control
            If root.ID = id Then
                Return root
            End If
            For Each c As Control In root.Controls
                Dim t As Control = FindControlRecursive(c, id)
                If Not (t Is Nothing) Then
                    Return t
                End If
            Next
            Return Nothing
        End Function

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If _tabModules.Count > 0 Then
                InjectInitializeTab(_selectedModuleId)
            End If
        End Sub

        Private Function GetSelectedModule() As String
            Dim selectedModuleId As String = ""

            If Not Request.QueryString("Module" & ModuleId.ToString & "_SelectById") Is Nothing And Not Page.IsPostBack Then
                ' check for query string tab selection first
                selectedModuleId = Me.ClientID & "_TabPage" & Request.QueryString("Module" & ModuleId.ToString & "_SelectById")
            Else
                ' get selected module by checking cookie, this allows us to maintain state
                ' not only between post back but also calls to other pages such as edit pages
                If Not Request.Cookies(ModuleId.ToString & "_selected") Is Nothing Then
                    selectedModuleId = Request.Cookies(ModuleId.ToString & "_selected").Value
                Else
                    ' lets check hidden field, just in case cookies disabled, then at least
                    ' we get postback
                    selectedModuleId = Request.Form(Me.ClientID & "_selected")
                End If
            End If

            ' if we get nothing then start on page 1 else increment by 1
            If selectedModuleId Is Nothing Then
                selectedModuleId = Me.ClientID & "_TabPage1"
            Else
                If Not TimerEnabled Then
                    ' let's grab the next one
                    Dim selectedId As Integer = Int32.Parse(selectedModuleId.Replace(Me.ClientID & "_TabPage", ""))
                    ' check to see if postback was caused by a contained module, if so don't increment
                    If Request("__EVENTTARGET") Is Nothing Then
                        selectedId += 1
                    Else
                        If Not Request("__EVENTTARGET").ToString.StartsWith(Me.UniqueID) Then
                            selectedId += 1
                        End If
                    End If
                    selectedModuleId = Me.ClientID & "_TabPage" & selectedId.ToString
                End If
            End If

            Return selectedModuleId
        End Function
#End Region

#Region " Rendering"
        Public Function ModuleNeeded(ByVal moduleNumber As Integer, ByVal _moduleSettings As ModuleInfo) As Boolean
            If TimerEnabled Then ' have to load all modules now
                Return True
            Else ' only load the module if it's selected
                If _selectedModuleId = Me.ClientID & "_TabPage" & moduleNumber.ToString Then
                    Return True
                End If
            End If
        End Function
        Private Function GetTabPage(ByVal _module As ModuleInfo, ByVal moduleNumber As Integer) As Control
            ' return a hidden div for injecting module into
            Dim div As HtmlControls.HtmlGenericControl
            div = New HtmlControls.HtmlGenericControl("div")

            ' id
            div.ID = "TabPage" & moduleNumber.ToString

            ' style
            div.Attributes.Add("style", "display:none")

            ' add hover code
            If HoverEnabled Then
                div.Attributes.Add("onMouseOver", "javascript:" & Me.ClientID & "_hovering=true;")
                div.Attributes.Add("onMouseOut", "javascript:" & Me.ClientID & "_hovering=false;")
            End If
            Return div
        End Function

        Private Sub RenderModulesNone(ByVal modules As ArrayList)

            ' render tab pages
            Dim moduleNumber As Integer = 1
            For Each _module As ModuleInfo In modules
                If ModuleNeeded(moduleNumber, _module) Then
                    Controls.Add(GetTabPage(_module, moduleNumber))
                End If
                moduleNumber += 1
            Next

        End Sub

#End Region

#Region " Javascript Injection"

        Private Function GetTabPageName(ByVal moduleNumber As Integer) As String
            Return Me.ClientID & "_TabPage" & moduleNumber.ToString
        End Function

        Private Sub InjectActivateTabPage(ByVal modules As ArrayList)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("<script language='javascript'>" & Environment.NewLine)
            sb.Append("function ActivateTabPage_" & Me.ClientID & "(tabpagename)" & Environment.NewLine)
            sb.Append("{" & Environment.NewLine)

            ' wrap all in a test for the surrounding module, just in case its minimized
            sb.AppendFormat("if (document.getElementById('{1}')) {{{0}", Environment.NewLine, Me.Parent.ClientID)

            Dim moduleNumber As Integer = 1
            For Each _module As ModuleInfo In modules
                If ModuleNeeded(moduleNumber, _module) Then
                    sb.Append("document.getElementById('" & GetTabPageName(moduleNumber) & "').style.display='none';" & Environment.NewLine)
                End If
                moduleNumber += 1
            Next

            sb.Append("document.getElementById(tabpagename).style.display='';" & Environment.NewLine)
            ' save state to hidden and cookie
            sb.Append("document.getElementById('" & Me.ClientID & "_selected').value=tabpagename;" & Environment.NewLine)
            sb.AppendFormat("var curCookie = ""{0}"" + "" = "" + escape(tabpagename);{1}", ModuleId & "_selected", Environment.NewLine)
            sb.AppendFormat("document.cookie = curCookie;{0}", Environment.NewLine)

            sb.Append("}" & Environment.NewLine)
            sb.Append("}" & Environment.NewLine)
            sb.Append("</script>" & Environment.NewLine)

            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "ActivateTabPage_" & Me.ClientID, sb.ToString)

            ' inject the hidden field which holds the currently active tab
            Dim hf As New StringBuilder
            If modules.Count > 0 Then
                hf.AppendFormat("<input type=""hidden"" id=""{0}_selected"" name=""{0}_selected"" value=""{0}_TabPage{1}"" />{2}", Me.ClientID, "1", Environment.NewLine)
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "SelectedTabHidden" & Me.ClientID.ToString, hf.ToString)
            Else
                hf.AppendFormat("<input type=""hidden"" id=""{0}_selected"" name=""{0}_selected"" value=""{0}_TabPage{1}"" />{2}", Me.ClientID, "", Environment.NewLine)
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "SelectedTabHidden" & Me.ClientID.ToString, hf.ToString)
            End If
        End Sub

        Private Sub InjectInitializeTab(ByVal SelectedModuleId As String)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("<script language='javascript'>" & Environment.NewLine)
            ' wrap all in a test for the surrounding module, just in case its minimized
            sb.AppendFormat("var {1}_hovering = false;{0}", Environment.NewLine, Me.ClientID)
            sb.AppendFormat("var {1}_maxTab = {2};{0}", Environment.NewLine, Me.ClientID, _tabModules.Count)
            sb.AppendFormat("var {1}_timeoutMilliseconds = {2};{0}", Environment.NewLine, Me.ClientID, TimerDelay.ToString)
            sb.AppendFormat("var {1}_tabTimerId = null;{0}", Environment.NewLine, Me.ClientID)

            sb.AppendFormat("if (document.getElementById('{1}')) {{{0}", Environment.NewLine, Me.Parent.ClientID)

            sb.AppendFormat("SelectTab_{2}({0},false);{1}", SelectedModuleId.Replace(Me.ClientID & "_TabPage", ""), Environment.NewLine, Me.ClientID)
            If TimerEnabled Then
                sb.AppendFormat("TabChangeTimer_{2}(true);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            End If
            sb.Append("}" & Environment.NewLine)
            sb.Append("</script>" & Environment.NewLine)
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "InitializeTab_" & Me.ClientID, sb.ToString)
        End Sub

        Public Sub InjectSelectTab()
            Dim sb As New StringBuilder
            sb.AppendFormat("<script language='javascript'>{0}", Environment.NewLine)
            sb.AppendFormat("function SelectTab_{0}(tabNumber,clear){1}", Me.ClientID, Environment.NewLine)
            sb.AppendFormat("// SelectTab - select a specific tab#, clear=true will stop automatic tab change timer{0}", Environment.NewLine)
            sb.AppendFormat("{{{0}", Environment.NewLine)
            sb.AppendFormat("if ({0}_hovering) {{return;}}{1}", Me.ClientID, Environment.NewLine)
            sb.AppendFormat("{0}ActivateTabPage_{2}('{2}_TabPage'+tabNumber.toString());{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID.ToString)
            sb.AppendFormat("{0}if (clear) {{{0}clearTimeout({2}_tabTimerId); }}{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("}}{0}", Environment.NewLine)
            sb.AppendFormat("</script>{0}", Environment.NewLine)

            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "InjectSelectTab_" & Me.ClientID, sb.ToString)
        End Sub

        Public Sub InjectTabChangeTimer()
            Dim sb As New StringBuilder
            sb.AppendFormat("<script language='javascript'>{0}", Environment.NewLine)
            sb.AppendFormat("function TabChangeTimer_{0}(initTimer) {{{1}", Me.ClientID, Environment.NewLine)

            sb.AppendFormat("// TabChangeTimer - callback for automatic tab changes{0}", Environment.NewLine)
            sb.AppendFormat("{0}var currentTab;{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}var currentTabIndex;{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}currentTab = document.getElementById('{2}_selected').value;{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)

            sb.AppendFormat("{0}currentTabIndex = (currentTab.substr('{2}_TabPage'.length,currentTab.length-'{2}_TabPage'.length)*1);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("{0}{1}", ControlChars.Tab, Environment.NewLine)

            sb.AppendFormat("{0}// if just starting, wait for timeout before processing again{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}if (initTimer){1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{{{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{0}{2}_tabTimerId = self.setTimeout(""TabChangeTimer_{2}(false);"", {2}_timeoutMilliseconds);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("{0}{0}return;{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}}}{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}currentTabIndex += 1;{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}if (currentTabIndex > {2}_maxTab){1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("{0}{{{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{0}currentTabIndex=1;{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{0}{2}_tabTimerId = self.setTimeout(""TabChangeTimer_{2}(false);"", {2}_timeoutMilliseconds);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)

            sb.AppendFormat("{0}}}{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}else{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{{{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{0}{2}_tabTimerId = self.setTimeout(""TabChangeTimer_{2}(false);"", {2}_timeoutMilliseconds);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("{0}}}{1}", ControlChars.Tab, Environment.NewLine)
            sb.AppendFormat("{0}{1}", ControlChars.Tab, Environment.NewLine)

            sb.AppendFormat("{0}SelectTab_{2}(currentTabIndex,false);{1}", ControlChars.Tab, Environment.NewLine, Me.ClientID)
            sb.AppendFormat("}}{0}", Environment.NewLine)
            sb.AppendFormat("</script>{0}", Environment.NewLine)

            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "InjectTabChangeTimer_" & Me.ClientID, sb.ToString)

        End Sub
#End Region

#Region " Action Handler"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString(Entities.Modules.Actions.ModuleActionType.ContentOptions, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", EditUrl(), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

#Region " Intermodule Communication"
        Public Sub OnModuleCommunication(ByVal s As Object, ByVal e As Entities.Modules.Communications.ModuleCommunicationEventArgs) Implements Entities.Modules.Communications.IModuleListener.OnModuleCommunication

            If e.Target = "ModuleRotator" Then
                Select Case e.Type
                    Case "SelectTabByModuleId"
                        _selectedModuleId = Me.ClientID & "_TabPage" & e.Value.ToString()
                    Case "SelectTabByModuleTitle"
                        For Each _moduleSettings As ModuleInfo In _tabModules
                            If _moduleSettings.ModuleTitle.ToUpper = e.Value.ToString.ToUpper Then
                                _selectedModuleId = Me.ClientID & "_TabPage" & _moduleSettings.ModuleID.ToString
                                Exit For
                            End If
                        Next
                End Select
            End If
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ' inject modules
            Compatibility.InjectModules(Me, _tabModules)

        End Sub
    End Class
End Namespace
