<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control Language="vb" CodeBehind="EditModuleRotator.ascx.vb" AutoEventWireup="false"
    Explicit="True" Inherits="DNNStuff.ModuleRotator.EditModuleRotator" %>

<div class="dnnForm dnnClear">
    <div id="editsettings" class="tabslayout">
        <ul id="editsettings-nav" class="tabslayout">
            <li><a href="#tab1"><span>
                <%=Localization.GetString("TabCaption_Tab1", LocalResourceFile)%></span></a></li>
            <li><a href="#tab2"><span>
                <%=Localization.GetString("TabCaption_Tab2", LocalResourceFile)%></span></a></li>
        </ul>
        <div class="tabs-container">
            <div class="tab" id="tab1">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblTabs" Suffix=":" ControlName="cmdAddModule" runat="server" CssClass="SubHead"></dnn:Label>
                    <asp:LinkButton class="CommandButton" ID="cmdAddModule" runat="server" BorderStyle="none"
                        CausesValidation="False" resourcekey="cmdAddModule" Text="Add New Module"></asp:LinkButton><br>
                    <asp:DataGrid ID="grdModules" runat="server" CssClass="Normal"
                        DataKeyField="ModuleRotatorId" Width="100%" GridLines="Horizontal" Border="0"
                        AutoGenerateColumns="False" OnItemDataBound="grdModules_Item_Bound" OnItemCommand="grdModules_Move"
                        OnDeleteCommand="grdModules_Delete" OnEditCommand="grdModules_Edit" OnUpdateCommand="grdModules_Update"
                        OnCancelCommand="grdModules_CancelEdit">
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdEditTabModule" runat="server" CausesValidation="false" CommandName="Edit"
                                        ImageUrl="~/images/edit.gif" AlternateText="Edit"></asp:ImageButton>&nbsp;
                                    <asp:ImageButton ID="cmdDeleteTabModule" runat="server" CausesValidation="false"
                                        CommandName="Delete" ImageUrl="~/images/delete.gif" AlternateText="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="cmdSaveTabModule" runat="server" CausesValidation="false" CommandName="Update"
                                        ImageUrl="~/images/save.gif" AlternateText="Save"></asp:ImageButton>&nbsp;
                                    <asp:ImageButton ID="cmdCancelTabModule" runat="server" CausesValidation="false"
                                        CommandName="Cancel" ImageUrl="~/images/cancel.gif" AlternateText="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Module">
                                <HeaderStyle CssClass="NormalBold"></HeaderStyle>
                                <ItemStyle CssClass="Normal"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ModuleTitle") %>'
                                        ID="Label2" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cboModule" runat="server" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdMoveTabModuleUp" runat="server" CausesValidation="false"
                                        CommandName="Item" CommandArgument="Up" ImageUrl="~/images/up.gif" resourcekey="cmdMoveTabModuleUp"
                                        AlternateText="Move Module Up"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdMoveTabModuleDown" runat="server" CausesValidation="false"
                                        CommandName="Item" CommandArgument="Down" ImageUrl="~/images/dn.gif" resourcekey="cmdMoveTabModuleDown"
                                        AlternateText="Move Module Down"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
                <div class="dnnFormItem">
                                <dnn:Label ID="lblAutoHide" Suffix=":" ControlName="chkAutoHide" runat="server" CssClass="SubHead">
                                </dnn:Label>
                            <asp:CheckBox ID="chkHideTitles" runat="server" CssClass="Normal" Text=""></asp:CheckBox>
                </div>  
            </div>
            <div class="tab" id="tab2">
                <div class="dnnFormItem">
                                <dnn:Label ID="lblTimerEnabled" CssClass="SubHead" runat="server" ControlName="chkTimerEnabled"
                                    Suffix=":" />
                            <asp:CheckBox ID="chkTimerEnabled" Text="" runat="server" CssClass="Normal" />
                </div>  
                <div class="dnnFormItem">
                                <dnn:Label ID="lblTimerDelay" CssClass="SubHead" runat="server" ControlName="txtTimerDelay"
                                    Suffix=":" />
                            <asp:TextBox ID="txtTimerDelay" runat="server" Columns="5" CssClass="NormalTextBox"></asp:TextBox>
                            <asp:CompareValidator ID="valTimerDelayInteger" runat="server" Display="Dynamic"
                                CssClass="NormalRed" ControlToValidate="txtTimerDelay" Type="Integer" Operator="DataTypeCheck"
                                ErrorMessage="Timer delay must be an integer" />
                            <asp:CompareValidator ID="valTimerDelayGreaterThanEqualToZero" runat="server" Display="Dynamic"
                                CssClass="NormalRed" ControlToValidate="txtTimerDelay" ValueToCompare="0" Operator="GreaterThanEqual"
                                ErrorMessage="Timer delay must be greater than or equal to 0" />
                </div>  
                <div class="dnnFormItem">
                    <dnn:Label ID="lblHoverEnabled" CssClass="SubHead" runat="server" ControlName="chkHoverEnabled" Suffix=":" />
                    <asp:CheckBox ID="chkHoverEnabled" Text="" runat="server" CssClass="Normal" />
                </div>  
            </div>
        </div>
    </div>
</div>

<ul class="dnnActions dnnClear">
	<li><asp:LinkButton ID="cmdUpdate" Text="Update" resourcekey="cmdUpdate" CausesValidation="True" runat="server" CssClass="dnnPrimaryAction"  /></li>
	<li><asp:LinkButton ID="cmdCancel" Text="Cancel" resourcekey="cmdCancel" CausesValidation="False" runat="server" CssClass="dnnSecondaryAction" /></li>
</ul>

<script type="text/javascript">
    var tabber1 = new Yetii({
        id: 'editsettings',
        persist: true
    });
</script>

