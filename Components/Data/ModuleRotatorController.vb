Imports System
Imports System.Data
Imports DotNetNuke

Namespace DNNStuff.ModuleRotator

    Public Class ModuleRotatorController

        Public Function DNNStuff_ModuleRotator_GetModuleRotator(ByVal ModuleId As Integer) As IDataReader
            Return DataProvider.Instance().DNNStuff_ModuleRotator_GetModuleRotator(ModuleId)
        End Function

        Public Sub DNNStuff_ModuleRotator_UpdateModuleRotator(ByVal obj As ModuleRotatorInfo)
            DataProvider.Instance().DNNStuff_ModuleRotator_UpdateModuleRotator(obj.ModuleRotatorId, obj.ModuleId, obj.TabModuleId)
        End Sub

        Public Sub DNNStuff_ModuleRotator_DeleteModuleRotator(ByVal ModuleRotatorId As Integer)
            DataProvider.Instance().DNNStuff_ModuleRotator_DeleteModuleRotator(ModuleRotatorId)
        End Sub

        Public Sub DNNStuff_ModuleRotator_UpdateTabOrder(ByVal ModuleRotatorId As Integer, ByVal Increment As Integer)
            DataProvider.Instance().DNNStuff_ModuleRotator_UpdateTabOrder(ModuleRotatorId, Increment)
        End Sub

        Public Function DNNStuff_ModuleRotator_GetTabModules(ByVal TabId As Integer, ByVal ModuleId As Integer) As IDataReader
            Return DataProvider.Instance().DNNStuff_ModuleRotator_GetTabModules(TabId, ModuleId)
        End Function

    End Class

End Namespace
