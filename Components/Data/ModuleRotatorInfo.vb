Namespace DNNStuff.ModuleRotator
    Public Class ModuleRotatorInfo

        ' local property declarations
        Private _ModuleRotatorId As Integer
        Private _ModuleId As Integer
        Private _TabModuleId As Integer
        Private _TabOrder As Integer

        ' initialization
        Public Sub New()
        End Sub

        ' public properties
        Public Property ModuleRotatorId() As Integer
            Get
                Return _ModuleRotatorId
            End Get
            Set(ByVal Value As Integer)
                _ModuleRotatorId = Value
            End Set
        End Property

        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        Public Property TabModuleId() As Integer
            Get
                Return _TabModuleId
            End Get
            Set(ByVal Value As Integer)
                _TabModuleId = Value
            End Set
        End Property
        Public Property TabOrder() As Integer
            Get
                Return _TabOrder
            End Get
            Set(ByVal Value As Integer)
                _TabOrder = Value
            End Set
        End Property
    End Class

End Namespace
