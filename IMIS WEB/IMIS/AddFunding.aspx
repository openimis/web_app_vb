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
<%@ Page Title='<%$ Resources:Resource,L_FUNDING %>' Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="AddFunding.aspx.vb" Inherits="IMIS.AddFunding" EnableEventValidation="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">


</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="divBody">
        <asp:Panel ID="pnlBody" runat="server" ScrollBars="Auto" CssClass="panel" GroupingText="Funding">

            <table>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_Region" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:UpdatePanel runat="server" ID="UpRegion">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValRegion" runat="server" ControlToValidate="ddlRegion" ErrorMessage="*" InitialValue="0" ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_Payer" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:UpdatePanel runat="server" ID="UpDistrict">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorPremiumPaid0" runat="server" ControlToValidate="ddlDistrict" ErrorMessage="*" InitialValue="0" ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="Label1"
                            runat="server"
                            Text='<%$ Resources:Resource,L_PRODUCT %>'></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:UpdatePanel runat="server" ID="UpProduct">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProduct" runat="server">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPremiumPaid1" runat="server" ControlToValidate="ddlProduct" ErrorMessage="*" InitialValue="0" ValidationGroup="check"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="Label2"
                            runat="server"
                            Text='<%$ Resources:Resource,L_PAYER %>'></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlPayer" runat="server">
                        </asp:DropDownList>
                                 </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="Label3"
                            runat="server"
                            Text='<%$ Resources:Resource,L_PAYMENTDATE %>'></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:TextBox ID="txtPaymentDate" runat="server" Width="120px" MaxLength="10"></asp:TextBox>


                        <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                            PopupButtonID="Button1" TargetControlID="txtPaymentDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>

                        <asp:RequiredFieldValidator ID="RequiredFieldPaymentDate" runat="server" ControlToValidate="txtPaymentDate" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPaymentDate" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="Label4"
                            runat="server"
                            Text='<%$ Resources:Resource,L_PREMIUMPAID %>'></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:TextBox ID="txtPremiumPaid" runat="server" Style="text-align: right; padding-right: 4px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPremiumPaid" runat="server" ControlToValidate="txtPremiumPaid" ErrorMessage="*" ValidationGroup="check"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="Label5"
                            runat="server"
                            Text='<%$ Resources:Resource,L_RECEIPTNUMBER %>'></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:TextBox ID="txtReceiptNumber" runat="server" Style="text-align: right; padding-right: 4px"></asp:TextBox>
                    </td>
                    <td>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorReceiptNumber" runat="server" ControlToValidate="txtReceiptNumber" ErrorMessage="*" ValidationGroup="check"></asp:RequiredFieldValidator>

                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlButtons" runat="server" CssClass="panel">
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
<asp:Content ID="Content4" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
