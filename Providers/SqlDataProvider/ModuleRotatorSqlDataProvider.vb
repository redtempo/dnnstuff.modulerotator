Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke

Namespace DNNStuff.ModuleRotator

    Public Class SqlDataProvider
        Inherits DNNStuff.ModuleRotator.DataProvider

#Region "Private Members"
        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property
#End Region

#Region "Public Methods"
        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

        Public Overrides Function DNNStuff_ModuleRotator_GetModuleRotator(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DNNStuff_ModuleRotator_GetModuleRotator", ModuleId), IDataReader)
        End Function

        Public Overrides Sub DNNStuff_ModuleRotator_UpdateModuleRotator(ByVal ModuleRotatorId As Integer, ByVal ModuleId As Integer, ByVal TabModuleId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DNNStuff_ModuleRotator_UpdateModuleRotator", ModuleRotatorId, ModuleId, TabModuleId)
        End Sub

        Public Overrides Sub DNNStuff_ModuleRotator_DeleteModuleRotator(ByVal ModuleRotatorId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DNNStuff_ModuleRotator_DeleteModuleRotator", ModuleRotatorId)
        End Sub

        Public Overrides Sub DNNStuff_ModuleRotator_UpdateTabOrder(ByVal ModuleRotatorId As Integer, ByVal Increment As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DNNStuff_ModuleRotator_UpdateTabOrder", ModuleRotatorId, Increment)
        End Sub

        Public Overrides Function DNNStuff_ModuleRotator_GetTabModules(ByVal TabId As Integer, ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DNNStuff_ModuleRotator_GetTabModules", TabId, ModuleId), IDataReader)
        End Function

#End Region

    End Class

End Namespace
