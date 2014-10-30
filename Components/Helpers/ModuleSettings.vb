'***************************************************************************/
'* ModuleSettings.vb
'*
'* Copyright (c) 2007 by DNNStuff.
'* All rights reserved.
'*
'* Date:        March 19,2007
'* Author:      Richard Edwards
'* Description: Class to organize module settings
'*************/
Imports DotNetNuke

Namespace DNNStuff.ModuleRotator

    Public Class ModuleSettings
        Private _ModuleId As Integer = 0

#Region " Properties"
        Private Const SETTING_HIDETITLES As String = "HideTitles"
        Private _HideTitles As Boolean = True
        Public Property HideTitles() As Boolean
            Get
                Return _HideTitles
            End Get
            Set(ByVal value As Boolean)
                _HideTitles = value
            End Set
        End Property


#End Region

#Region " Derived Properties"
#End Region

#Region "Methods"
        Public Sub New(ByVal moduleId As Integer)
            _ModuleId = moduleId

            LoadSettings()
        End Sub

        Private Sub LoadSettings()
            Dim ctrl As New DotNetNuke.Entities.Modules.ModuleController
            Dim settings As Hashtable = ctrl.GetModuleSettings(_ModuleId)

            _HideTitles = Convert.ToBoolean(DNNUtilities.GetSetting(settings, SETTING_HIDETITLES, "True"))

        End Sub

        Public Sub UpdateSettings()
            Dim ctrl As New DotNetNuke.Entities.Modules.ModuleController
            With ctrl
                .UpdateModuleSetting(_ModuleId, SETTING_HIDETITLES, HideTitles.ToString)

            End With

        End Sub

#End Region

    End Class
End Namespace
