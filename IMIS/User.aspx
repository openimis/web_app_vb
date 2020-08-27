<%-- Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)

The program users must agree to the following terms:

Copyright notices
This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
Free Software Foundation, version 3 of the License.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.

Disclaimer of Warranty
There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.

Limitation of Liability 
In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
advised of the possibility of such damages.

In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.--%>
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="User.aspx.vb" Inherits="IMIS.User" 
 Title = '<%$ Resources:Resource,R_USER %>'%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <script type="text/javascript">
        var TargetRoleControl = null;
        var TargetDistrictControl = null;
        window.onload = function () {
            try {
                TargetRoleControl = document.getElementById('<%=Me.gvRoles.ClientID%>');
                TargetDistrictControl = document.getElementById('<%=Me.gvDistrict.ClientID%>');
            } catch (e) {
                TargetRoleControl = null;
                TargetDistrictControl = null;
            }
        }

        function validateForm() {
            var isRoleSet = RoleSelected();
            var isDistrictSet = DistrictSelected();

            if (!isRoleSet || !isDistrictSet) {
                //document.getElementById('<%=Me.validationSummary.ID%>').innerHTML = '<%=Me.validationSummary.HeaderText%>';
                return false;
            }
        }


        function RoleSelected() {
            if (TargetRoleControl == null) return false;


            var ChildRoleControl = "chkRoleSelect";

            var chkboxes = TargetRoleControl.getElementsByTagName("input");


            for (var n = 0; n < chkboxes.length; ++n) {
                if (chkboxes[n].type == 'checkbox' && chkboxes[n].id.indexOf(ChildRoleControl, 0) >= 0 && chkboxes[n].checked) {
                    errRoleMsg.innerHTML = "";
                    return true;
                }
            }
            errRoleMsg.innerHTML = "* <asp:Literal ID='Literal1' runat='Server' Text='<%$ Resources:Resource,V_SELECTROLE%>'/>";

            return false;

        }

        function DistrictSelected() {
            if (TargetDistrictControl == null) return false;

            var ChildDistrictControl = "chkDistrictSelect";

            var chkboxes = TargetDistrictControl.getElementsByTagName("input");

            for (var n = 0; n < chkboxes.length; n++) {
                if (chkboxes[n].type == 'checkbox' && chkboxes[n].id.indexOf(ChildDistrictControl, 0) >= 0 && chkboxes[n].checked) {
                    errDistrictMsg.innerHTML = "";
                    return true;
                }
            }
            errDistrictMsg.innerHTML = "* <asp:Literal ID='Literal2' runat='Server' Text='<%$ Resources:Resource,V_SELECTDISTRICT%>'/>";
            return false;

        }
        function toggleCheckRole(status) {
            $('#<%=gvRoles.ClientId %> input').each(function () {
                    $(this).attr("checked", status);
                });
            }
            $(document).ready(function () {
                $('#<%=gvRoles.clientId %> input').click(function () {
                    $('#<%=gvRoles.clientId %> input').each(function () {
                        if (this.checked != true) {
                            $('#<%= checkbox1.ClientId %>').attr("checked", false);
                            return false;
                        } else {
                            $('#<%= checkbox1.ClientId %>').attr("checked", true);
                        }
                    });
                });
            });
            function toggleCheckDistrict(status) {
                $('#<%=gvDistrict.clientId %> input').each(function () {
                    $(this).attr("checked", status);
                });
            }
        function toggleCheckRegions(status) {
            $('#<%= gvRegion.ClientID %> input').each(function () {
                $(this).attr("checked", status);
            });
            toggleCheckDistrict(status);
        }
            $(document).ready(function () {
                $('#<%=gvDistrict.clientId %> input').click(function () {
                    $('#<%=gvDistrict.clientId %> input').each(function () {
                        if (this.checked != true) {
                            $('#<%= checkbox2.ClientId %>').attr("checked", false);
                            return false;
                        } else {
                            $('#<%= checkbox2.ClientId %>').attr("checked", true);
                        }
                    });
                });
            });

            $(document).ready(function () {
                $("#<%=B_SAVE.ClientID %>").click(function () {
                    // alert($("#<%=ddlLanguage.ClientID %>").val());
                    if ($("#<%=ddlLanguage.ClientID %>").val() == -1)
                        return false;
                });
            });

        $(document).ready(function () {
            //Check/Uncheck all the district for the selected region
            $('#<%= gvRegion.ClientID %> input').click(function () {
                var RegionId = $(this).closest("tr").find("input[type=hidden]").val();
                var checked = $(this).is(":checked");
                $('#<%= gvDistrict.ClientID %> input[type=hidden]').each(function () {
                    if ($(this).val() == RegionId){
                        var chkBox = $(this).closest("tr").find("input[type=checkbox]");
                        $(chkBox).attr("checked", checked);
                    }
                });
            });


            //Check/Uncheck region if one/none district is selected for a region
            $('#<%= gvDistrict.ClientID %> input').click(function () {
                var RegionId = $(this).closest("tr").find("input[type=hidden]").val();
                var status = false;

                $('#<%= gvDistrict.ClientID %> input').each(function () {
                    
                    if ($(this).closest("tr").find("input[type=hidden]").val() == RegionId) {
                        if ($(this).is(":checked")) {
                            status = true;
                            return false;
                        }
                    }
                });

                $('#<%= gvRegion.ClientID %> input[type=hidden]').each(function () {
                    if ($(this).val() == RegionId) {
                        $(this).closest("tr").find("input[type=checkbox]").attr("checked", status);
                    }
                });

            });
            $('[id*=gvRoles] tr').each(function () {
                var toolTip = $(this).attr("title");
                $(this).find("td").eq(2).each(function () {
                    $(this).simpletip({
                        content: '<%= Resources.Resource.M_ASSIGNROLE %>'
                    });
                });

                $(this).removeAttr("title");
            });

        });


    </script>
    <style type="text/css" >
    .footer{top:665px;}
     /*.backentry{ height:629px; }*/
     .panelbuttons{ position:relative;top:0px;}

     .tooltip
    {
        position: absolute;
        top: 0;
        left: 0;
        z-index: 3;
        display: none;
        background-color: rgb(102, 102, 102);
        color: White;
        padding: 5px;
        font-size: 10pt;
        font-family: Arial;
    }
    td
    {
        cursor: pointer;
    }

</style>
    <div class="divBody">
        <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto"
            CssClass="panel" GroupingText='<%$ Resources:Resource,G_USER %>'>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Language" runat="server" Text='<%$ Resources:Resource,L_LANGUAGE %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlLanguage" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldLanguage" runat="server"
                                        ControlToValidate="ddlLanguage"
                                        SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        InitialValue="-1" Text="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label
                                        ID="L_OtherName"
                                        runat="server"
                                        Text='<%$ Resources:Resource,L_OTHERNAMES %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtOtherNames" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldOtherNames" runat="server"
                                        ControlToValidate="txtOtherNames"
                                        SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text="*">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_LastName" runat="server" Text='<%$ Resources:Resource,L_LASTNAME %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtLastName" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldLastName" runat="server"
                                        ControlToValidate="txtLastName" Text="*"
                                        ValidationGroup="check" SetFocusOnError="True" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label
                                        ID="L_PHONE"
                                        runat="server"
                                        Text='<%$ Resources:Resource,L_PHONE%>'>
                                    </asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtPhone" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_EMAIL" runat="server" Text='<%$ Resources:Resource,L_EMAIL%>'></asp:Label></td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="150px" MaxLength="200" TextMode="Email"></asp:TextBox>
                                </td>
                                <td style="direction: ltr">
                                    <asp:RequiredFieldValidator ID="rf2" runat="server" ControlToValidate="txtEmail" SetFocusOnError="True" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_LoginName" runat="server" Text='<%$ Resources:Resource,L_USERNAME %>'>></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtLoginName" runat="server" Width="150px" MaxLength="25"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldLoginName" runat="server"
                                        ControlToValidate="txtLoginName" Text="*"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Password" runat="server" Text='<%$ Resources:Resource,L_PASSWORD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="25"
                                        Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldPassword" runat="server"
                                        ValidationGroup="check" ControlToValidate="txtPassword" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="rePasswordStrength" runat="server" ControlToValidate="txtPassword" ErrorMessage='<%$ Resources:Resource, M_WEAKPASSWORD %>' SetFocusOnError="True" ValidationExpression="^(?=.*\d)(?=.*[A-Za-z\W]).{8,}$" ValidationGroup="check" ForeColor="Red" Display="Dynamic">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_ConfirmPassword" runat="server" Text='<%$ Resources:Resource,L_CONFIRMPASSWORD%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"
                                        MaxLength="25" Width="150px"></asp:TextBox>
                                </td>
                                <td class="Validate">
                                    <asp:RequiredFieldValidator ID="RequiredFieldConfirmPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="ComparePassword" runat="server"
                                        ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='<%$ Resources:Resource,V_CONFIRMPASSWORD%>'></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_HFNAME" runat="server" Text='<%$ Resources:Resource,L_HF%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlHFNAME" runat="server"
                                        Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:CheckBox ID="Checkbox1" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckRole(this.checked);" />

                        <asp:Panel ID="pnlRole" runat="server" ScrollBars="Auto" Height="320px" Width="175px"
                            CssClass="panel" GroupingText='<%$ Resources:Resource,L_ROLE%>'>


                            <asp:GridView ID="gvRoles" runat="server" AllowPaging="false" PagerSettings-FirstPageText="First Page" PagerSettings-LastPageText="Last Page" PagerSettings-Mode="NumericFirstLast"
                                AutoGenerateColumns="False" CssClass="mGrid" DataKeyNames="RoleId"
                                EmptyDataText="No roles found" GridLines="None" PagerStyle-CssClass="pgr"
                                PageSize="12" ShowSelectButton="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRoleSelect" runat="server" HeaderStyle-Width="10px" Checked='<%#Eval("HasRight") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RoleName" HeaderStyle-Width="110px"
                                        HeaderText="<%$ Resources:Resource,L_ROLE %>" SortExpression="RoleName">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="A">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAssign" runat="server" HeaderStyle-Width="10px" Checked='<%#Eval("Assign") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfRoleId"  runat="server" value='<%#Eval("RoleId") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:HiddenField ID="UserRoleID"  runat="server" value='<%#Eval("UserRoleId") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                               
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <SelectedRowStyle CssClass="srs" />
                            </asp:GridView>
                            <span style="color: Red" id="errRoleMsg" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkCheckAllR" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckRegions(this.checked);" />
                        <asp:Panel ID="pnlRegion" runat="server" ScrollBars="Auto" Height="320px" Width="175px"
                            CssClass="panel" GroupingText='<%$ Resources:Resource, L_REGION %>'>
                            <asp:GridView ID="gvRegion" runat="server"
                                AutoGenerateColumns="False"
                                ShowSelectButton="True"
                                GridLines="None"
                                CssClass="mGrid"
                                PagerStyle-CssClass="pgr"
                                DataKeyNames="checked,RegionId"
                                EmptyDataText='No regions  found'>

                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRegionSelect" runat="server" HeaderStyle-Width="30px" />

                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfRegion"  runat="server" value='<%#Eval("RegionId") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RegionName" HeaderText='<%$ Resources:Resource,L_REGION %>' SortExpression="RegionName"
                                        HeaderStyle-Width="110px">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>


                                </Columns>

                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <SelectedRowStyle CssClass="srs" />

                            </asp:GridView>
                            <span style="color: Red" id="Span1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckDistrict(this.checked);" />
                        <asp:Panel ID="pnlDistrict" runat="server" ScrollBars="Auto" Height="320px" Width="175px"
                            CssClass="panel" GroupingText='<%$ Resources:Resource, L_DISTRICT %>'>
                            <asp:GridView ID="gvDistrict" runat="server"
                                AutoGenerateColumns="False"
                                ShowSelectButton="True"
                                GridLines="None"
                                CssClass="mGrid"
                                PagerStyle-CssClass="pgr"
                                DataKeyNames="checked,UserDistrictId,DistrictId,Region"
                                EmptyDataText='No District  found'>

                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDistrictSelect"  runat="server" HeaderStyle-Width="30px" />
                                            
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfRegion"  runat="server" value='<%#Eval("Region") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Districtname" HeaderText='<%$ Resources:Resource,L_DISTRICT %>' SortExpression="Districtname"
                                        HeaderStyle-Width="110px">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>


                                </Columns>

                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <SelectedRowStyle CssClass="srs" />

                            </asp:GridView>
                            <span style="color: Red" id="errDistrictMsg" />
                            </span>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

        </asp:Panel>


    </div>
    <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <table width="100%" cellpadding="10 10 10 10">
            <tr>

                <td align="left">

                    <asp:Button
                        ID="B_SAVE"
                        runat="server"
                        Text='<%$ Resources:Resource,B_SAVE%>'
                        ValidationGroup="check" />
                </td>


                <td align="right">
                    <asp:Button
                        ID="B_CANCEL"
                        runat="server"
                        Text='<%$ Resources:Resource,B_CANCEL%>' />
                </td>

            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:label id="lblMsg"  runat="server"></asp:label><asp:ValidationSummary ID="validationSummary" runat="server"  HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>






