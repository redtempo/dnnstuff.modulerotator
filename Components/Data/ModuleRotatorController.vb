'***************************************************************************/
'* ModuleRotatorDB.vb
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
