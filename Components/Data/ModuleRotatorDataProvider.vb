'***************************************************************************/
'* DataProvider.vb
'*
'* COPYRIGHT (c) 2004 by DNNStuff
'* ALL RIGHTS RESERVED.
'*
'* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'* DEALINGS IN THE SOFTWARE.
'*************/
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
