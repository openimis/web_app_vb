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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Family.aspx.vb" MasterPageFile="~/IMIS.Master" Inherits="IMIS.Family" Title='<%$ Resources:Resource,L_ADDFAMILY %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="cHead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #SelectPic {
            padding-top: 10%;
            width: 100%;
            margin: auto;
            text-align: center;
            vertical-align: bottom;
            position: fixed;
            top: 0;
            left: 0;
            height: 100%;
            z-index: 1001;
            background-color: Gray;
            opacity: 0.9;
            display: none;
        }
        .auto-style1 {
            height: 27px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }
        .auto-style2 {
            height: 27px;
        }
    </style>
    <script type="text/javascript" language="javascript">


        function pageLoadExtend() {
            $(document).ready(function () {

                $('#<%=btnBrowse.ClientID %>').click(function (e) {

                    $("#SelectPic").show();

                    e.preventDefault();
                });

            });
        }

        $(document).ready(function () {

            $("#btnCancel").hide();
            $('#<%=btnBrowse.ClientID %>').click(function (e) {

            $("#SelectPic").show();
            $("#btnCancel").show();
            e.preventDefault();
        });



        $("#btnCancel").click(function () {
            //alert('called');
            $("#SelectPic").hide();
            $("#btnCancel").hide();
        });




    });


    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <div class="divBody" >
     <asp:Panel ID="Panel1" runat="server"  
             CssClass="panel" GroupingText='<%$ Resources:Resource,L_FAMILYPANEL %>' EnableTheming="True" >
            <asp:UpdatePanel ID="upDistrict" runat="server"  >                                
               <ContentTemplate>      
                  <table >
                      <tr>
                          <td class="auto-style1">
                              <asp:Label
                                  ID="L_REGION"
                                  runat="server"
                                  Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                          </td>
                          <td class="DataEntry">
                              <asp:DropDownList ID="ddlRegion" runat="server" Width="150px" AutoPostBack="true" />
                          </td>
                          <td class="auto-style2">
                              <asp:RequiredFieldValidator ID="RequiredFieldRegion" runat="server"
                                  ControlToValidate="ddlRegion" InitialValue="0"
                                  SetFocusOnError="True"
                                  ValidationGroup="check"
                                  Text="*"></asp:RequiredFieldValidator>
                          </td>
                          <td class="auto-style1">
                              <asp:Label ID="L_ADDRESS" runat="server"
                                  Text="<%$ Resources:Resource, L_PARMANENTADDRESS %>" Style="direction: ltr"></asp:Label>
                          </td>
                          <td class="DataEntry">
                              <asp:TextBox ID="txtAddress" runat="server" Height="40px" MaxLength="25"
                                  TextMode="MultiLine" Width="150px" Style="resize: none;"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfAddress" runat="server" ControlToValidate="txtAddress" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                          </td>
                      </tr>
                      <tr>
                          <td class="auto-style1">
                              <asp:Label ID="L_DISTRICT" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                          </td>
                          <td class="DataEntry">
                              <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" Width="150px" />
                          </td>
                          <td class="auto-style2">
                              <asp:RequiredFieldValidator ID="RequiredFieldDistrict" runat="server" ControlToValidate="ddlDistrict" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                          </td>
                          <td class="auto-style1">&nbsp;</td>
                          <td class="DataEntry">&nbsp;</td>
                      </tr>
                    <tr>
                    
                        <td class="FormLabel">
                            <asp:Label 
                                ID="L_WARD"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_WARD %>'>
                            </asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlWard" runat="server" Width="150px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td> 
                            <asp:RequiredFieldValidator 
                                ID="RequiredFieldWard" 
                                runat="server" 
                                ControlToValidate="ddlWard" 
                                InitialValue="0"
                                ErrorMessage="Please select a Ward." 
                                SetFocusOnError="True" 
                                ValidationGroup="check" 
                                Text="*"></asp:RequiredFieldValidator>
                                 <asp:RequiredFieldValidator 
                                ID="RequiredFieldWard1" 
                                runat="server" 
                                ControlToValidate="ddlWard" 
                                 ErrorMessage="Please select a Ward." 
                                SetFocusOnError="True" 
                                ValidationGroup="check" 
                                Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                      <td class="FormLabel">
                            <asp:Label 
                                ID="L_VILLAGE"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_VILLAGE %>'>
                            </asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList 
                                ID="ddlVillage" 
                                runat="server" 
                                Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator 
                                ID="RequiredFieldVillage" 
                                runat="server" 
                                ControlToValidate="ddlVillage" 
                                InitialValue="0" 
                                SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator>
                             <asp:RequiredFieldValidator 
                                ID="RequiredFieldVillage1" 
                                runat="server" 
                                ControlToValidate="ddlVillage" 
                                 SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator>
                           
                        </td>
                    </tr>
                    <tr id="trPoverty" runat="server">
                        <td class="FormLabel"> 
                            <asp:Label ID="lblConfirmationType0" runat="server" Text="<%$ Resources:Resource,L_POVERTY %>"></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlPoverty" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                           <%--<asp:RequiredFieldValidator 
                                ID="RequiredFieldValidator1" 
                                runat="server" 
                                ControlToValidate="ddlPoverty" 
                                InitialValue="" 
                                SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator ID="rfPoverty" runat="server" ControlToValidate="ddlPoverty" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                        <td class="FormLabel">
                                    &nbsp;</td>
                                <td class="DataEntry">
                                    &nbsp;</td>
                    </tr>
                      <tr id="trConfirmation" runat="server" >
                          <td class="FormLabel">
                              <asp:Label ID="lblConfirmationType" runat="server" Text="<%$ Resources:Resource,L_CONFIRMATIONTYPE %>"></asp:Label>
                          </td>
                          <td class="DataEntry">
                              <asp:DropDownList ID="ddlConfirmationType" runat="server" Width="150px">
                              </asp:DropDownList>
                          </td>
                          <td><asp:RequiredFieldValidator 
                                ID="rfConfirmation" 
                                runat="server" 
                                ControlToValidate="ddlConfirmationType" 
                                InitialValue="" 
                                SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator></td>
                          <td class="FormLabel">
                              <asp:Label ID="L_CONFIRMATIONNO" runat="server" style="direction: ltr" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>"></asp:Label>
                          </td>
                          <td class="DataEntry">
                              <asp:TextBox ID="txtConfirmationNo" runat="server" MaxLength="12" style="direction: ltr" Width="150px"></asp:TextBox>
                              <asp:RequiredFieldValidator 
                                ID="rfConfirmationNo" 
                                runat="server" 
                                ControlToValidate="txtConfirmationNo" 
                                SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator>
                          </td>
                      </tr>
                      <tr id="trType" runat="server">
                                <td class="FormLabel">
                                    <asp:Label ID="L_TYPE" runat="server" Text="<%$ Resources:Resource, L_GROUPTYPE %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlType" runat="server" Width="150px" >
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfType" runat="server" 
                                        ControlToValidate="ddlType" InitialValue="" SetFocusOnError="True" Text="*" 
                                        ValidationGroup="check"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                      <tr style="display:none;">
                        <td class="FormLabel"> <asp:Label ID="lblEthnicity" runat="server" Visible="false" Text='<%$ Resources:Resource,L_ETHNICITY %>'> </asp:Label> </td>
                        <td class="DataEntry">
                            <asp:DropDownList 
                                ID="ddlEthnicity"
                                runat="server"
                                Width="150px" Visible="false" >
                            </asp:DropDownList>
                        </td>
                        <td>
                           <%--<asp:RequiredFieldValidator 
                                ID="rfEthnicity" 
                                runat="server" 
                                ControlToValidate="ddlEthnicity" 
                                InitialValue="" 
                                SetFocusOnError="True" 
                                ValidationGroup="check"
                                Text="*"></asp:RequiredFieldValidator>--%></td>
                    </tr>
                </table>    
                                           
               </ContentTemplate>      
            </asp:UpdatePanel>                      
         </asp:Panel>
        <asp:Panel ID="pnlBody" runat="server" Style="height: auto;" CssClass="panelBody" GroupingText='<%$ Resources:Resource,L_POLICYHOLDER_%>'>
            <asp:UpdatePanel ID="upCHFID" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_CHFID" runat="server" Text="<%$ Resources:Resource,L_CHFID%>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtCHFID" runat="server" AutoPostBack="True" CssClass="numbersOnly" MaxLength="12" Width="150px"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldCHFID0" runat="server" ControlToValidate="txtCHFID" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                </asp:RequiredFieldValidator>
                                <td></td>
                                <td></td>
                            </td>
                            <td class="FormLabel"></td>
                            <td rowspan="18" valign="top">
                                <asp:UpdatePanel ID="upImage" runat="server">
                                    <ContentTemplate>
                                        <asp:Image ID="Image1" runat="server" Width="200px" Height="200px" ImageAlign="Middle" onerror="NoImage(this)" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Button runat="server" ID="btnBrowse" Text='<%$ Resources:Resource,B_BROWSE%>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_OTHERNAMES0" runat="server" Text="<%$ Resources:Resource,L_OTHERNAMES %>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtOtherNames" runat="server"   MaxLength="100" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldOtherNames1" runat="server" ControlToValidate="txtOtherNames" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                </asp:RequiredFieldValidator>
                                 
                            </td>
                            <td></td>
                            <td></td>
                            
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_LASTNAME0" runat="server" Text="<%$ Resources:Resource,L_LASTNAME %>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtLastName" runat="server"   MaxLength="100" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldLastName2" runat="server" ControlToValidate="txtLastName" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                </asp:RequiredFieldValidator>
                                 
                            </td>
                            <td></td>

                            <td class="DataEntry"></td>

                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_BIRTHDATE" runat="server" Text="<%$ Resources:Resource,L_BIRTHDATE %>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtBirthDate" runat="server" Width="120px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="Button1" TargetControlID="txtBirthDate">
                                </asp:CalendarExtender>
                                <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldBirthDate0" runat="server" ControlToValidate="txtBirthDate" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="*" SetFocusOnError="false" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check"></asp:RegularExpressionValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry"></td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_GENDER" runat="server" Text="<%$ Resources:Resource,L_GENDER %>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlGender" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldGender0" runat="server" ControlToValidate="ddlGender" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry"></td>
                        </tr>
                        <tr id="trMaritalStatus" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_MARITAL" runat="server" Text="<%$ Resources:Resource,L_MARITAL %>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlMarital" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfMaritalStatus" runat="server" ControlToValidate="ddlMarital" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trBeneficiary" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_CARD" runat="server" Text="<%$ Resources:Resource,L_CARD%>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlCardIssued" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfBeneficiary" runat="server" ControlToValidate="ddlCardIssued" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trCurrentRegion" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblCurrentRegion" runat="server" Text="<%$ Resources:Resource, L_CREGION %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCurrentRegion" Width="150px" runat="server" AutoPostBack="True">

                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfCurrentRegion" runat="server" ControlToValidate="ddlCurrentRegion" InitialValue="0" SetFocusOnError="True" style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                        <tr id="trCurrentDistrict" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblCurrentDistrict0" runat="server" Text="<%$ Resources:Resource, L_CDISTRICT %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel ID="upCurDistrict" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCurDistrict" runat="server" Width="150px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfCurrentDistrict" runat="server" ControlToValidate="ddlCurDistrict" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trCurrentMunicipality" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblCurrentVDC0" runat="server" Text="<%$ Resources:Resource, L_CVDC %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel ID="upCurVDC" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCurVDC" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfCurrentVDC" runat="server" ControlToValidate="ddlCurVDC" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trCurrentVillage" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblCurrentWard0" runat="server" Text="<%$ Resources:Resource, L_CWARD %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel ID="upCurWard" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCurWard" runat="server" Width="150px"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfCurrentVillage" runat="server" ControlToValidate="ddlCurWard" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trCurrentAddress" runat="server">
                            <td class="FormLabel" style="vertical-align: top">
                                <asp:Label ID="lblCurrentAddress0" runat="server" Text="<%$ Resources:Resource, L_CURRENTADDRESS %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtCurrentAddress" runat="server" Height="40px" MaxLength="25" Style="resize: none;" TextMode="MultiLine" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfCurrentAddress" runat="server" ControlToValidate="txtCurrentAddress" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>

                        <tr id="trProfession" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_PROFESSION0" runat="server" Text="<%$ Resources:Resource, L_PROFESSION %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlProfession" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfProfession" runat="server" ControlToValidate="ddlProfession" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trEducation" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_EDUCATION0" runat="server" Text="<%$ Resources:Resource, L_EDUCATION %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlEducation" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfEducation" runat="server" ControlToValidate="ddlEducation" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_PHONE" runat="server" Text="<%$ Resources:Resource,L_PHONE%>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trEmail" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_EMAIL0" runat="server" Text="<%$ Resources:Resource, L_EMAIL %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtEmail" runat="server" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check">*</asp:RegularExpressionValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trIdentificationType" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_IDTYPE" runat="server" Text="<%$ Resources:Resource, L_IDTYPE %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlIdType" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfIdType" runat="server" ControlToValidate="ddlIdType" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trIdentificationNo" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="L_PASSPORT" runat="server" Text="<%$ Resources:Resource,L_PASSPORT%>">
                                </asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtPassport" runat="server" MaxLength="25" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfIdNo1" runat="server" ControlToValidate="txtPassport" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trFSPRegion" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblFSPRegion" runat="server" Text="<%$ Resources:Resource, L_FSPREGION %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlFSPRegion" runat="server" AutoPostBack="True" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfRegionFSP" runat="server" ControlToValidate="ddlFSPRegion" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trFSPDistrict" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblFSPDistrict" runat="server" Text="<%$ Resources:Resource, L_FSPDISTRICT %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlFSPDistrict" runat="server" AutoPostBack="True" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfDistrictFSP" runat="server" ControlToValidate="ddlFSPDistrict" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trFSPCategory" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblFSPCategory" runat="server" Text="<%$ Resources:Resource, L_FSPCATEGORY %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlFSPCateogory" runat="server" AutoPostBack="True" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfFSPCategory" runat="server" ControlToValidate="ddlFSPCateogory" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry">&nbsp;</td>
                        </tr>
                        <tr id="trFSP" runat="server">
                            <td class="FormLabel">
                                <asp:Label ID="lblFSP" runat="server" Text="<%$ Resources:Resource, L_FSP %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlFSP" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfFSP" runat="server" ControlToValidate="ddlFSP" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td class="DataEntry"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
     <asp:UpdatePanel ID="upDL" runat="server">
        <ContentTemplate>
            <table id="SelectPic">
                <tr>
                    <td align="center">
                    <asp:Panel ID="pnlImages" runat="server" Width="500px" Height="450px" BackColor="White" ScrollBars="Auto">
                        <asp:DataList ID="dlImages" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="ImagePath" OnSelectedIndexChanged="dlImages_SelectedIndexChanged"> 
                            <ItemTemplate>
                                <table width="100px" style="height:100px">
                                    <tr>
                                        <td align="center">
                                            On: <%#Eval("TakenDate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <img alt="" width="100px" height="100px" src='Images\Submitted\<%#eval("ImagePath") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select">Select</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate> 
                            </asp:DataList>                                                     
                    </asp:Panel>
                    <br />  
                    <input type="button" id="btnCancel" value="Cancel" />
                    </td>
                </tr>
            </table>    
        </ContentTemplate>
    </asp:UpdatePanel>
   </div>
   <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                               <asp:Button 
                            
                            ID="B_SAVE" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_SAVE%>'
                            ValidationGroup="check"  />
                        </td>
                        
                        
                        <td  align="right">
                       <asp:Button 
                            
                            ID="B_CANCEL" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_CANCEL%>'
                              />
                        </td>
                        
                    </tr>
                </table>   
                 <asp:HiddenField ID="hfInsureeIsOffline" runat="server" Value="" />
               <asp:HiddenField ID="hfFamilyIsOffline" runat="server" Value="" />
                    </asp:Panel>
</asp:Content>
