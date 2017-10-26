Imports System
Imports System.Web.Caching
Imports System.Reflection
Imports DotNetNuke

Namespace DNNStuff.ModuleRotator

    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "DNNStuff.ModuleRotator", "DNNStuff.ModuleRotator"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

        Public MustOverride Function DNNStuff_ModuleRotator_GetModuleRotator(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Sub DNNStuff_ModuleRotator_UpdateModuleRotator(ByVal ModuleRotatorId As Integer, ByVal ModuleId As Integer, ByVal TabModuleId As Integer)
        Public MustOverride Sub DNNStuff_ModuleRotator_DeleteModuleRotator(ByVal ModuleRotatorId As Integer)
        Public MustOverride Sub DNNStuff_ModuleRotator_UpdateTabOrder(ByVal ModuleRotatorId As Integer, ByVal Increment As Integer)
        Public MustOverride Function DNNStuff_ModuleRotator_GetTabModules(ByVal TabId As Integer, ByVal ModuleId As Integer) As IDataReader

#End Region

    End Class

End Namespace
