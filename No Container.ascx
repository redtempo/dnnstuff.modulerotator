<%@ Control language="vb" CodeBehind="~/admin/Containers/container.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="SolPartActions" Src="~/admin/Containers/SolPartActions.ascx"%>
<table cellpadding="0" cellspacing="0" summary="Module Design Table" width="98%">
  <tr>
    <td runat="server" id="ContentPane">
      <dnn:SolPartActions runat="server" id="dnnSolPartActions" />
    </td>
  </tr>
</table>