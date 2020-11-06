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

<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="PriceListMI.aspx.vb" Inherits="IMIS.PriceListsMI" EnableEventValidation="false"
    Title='<%$ Resources:Resource,G_PRICELISTSMI %>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function toggleCheck(status) {
            $('.mGrid input').each(function () {
                $(this).attr("checked", status);
            });
        }
        $(document).ready(function () {
            $('.mGrid input').click(function () {
                $('.mGrid input').each(function () {
                    if (this.checked != true) {
                        $('#<%= checkbox1.ClientId %>').attr("checked", false);
                        return false;
                    } else {
                        $('#<%= checkbox1.ClientId %>').attr("checked", true);
                    }
                });
            });
            /**** switching textbox and labels in the grid column ***/
            $(".switch").click(function () {
                $(".label").show();
                $(".textbox").hide();
                $label = $(this).find(".label");
                $txtbox = $(this).find(".textbox");
                $label.hide();
                $txtbox.show().focus();
            });
            $(".textbox").change(function () {
                $label = $(this).prev();
                $label.html(format.numberWithCommas($(this).val()));
                $label.show();
                $(this).hide();
            });
            /**** ....end of switching textbox and labels in the grid column ***/
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">


    <div class="divBody">
        <asp:Panel ID="pnlPriceLists" runat="server"
            CssClass="panel" GroupingText='<%$ Resources:Resource,G_PRICELISTSMI%>'>
            <table>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheck(this.checked);" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Name" runat="server" Text='<%$ Resources:Resource,L_NAME%>'></asp:Label>
                                </td>
                                <td class="DataEntryWide">

                                    <asp:TextBox ID="txtName" runat="server" Width="150" MaxLength="100"></asp:TextBox>

                                </td>
                                <td>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldName" runat="server"
                                        ControlToValidate="txtName"
                                        SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Date" runat="server" Text='<%$ Resources:Resource,L_DATE%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtDate" runat="server" Width="125px" MaxLength="10"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server"
                                        CultureDateFormat="dd/MM/YYYY"
                                        TargetControlID="txtDate" Mask="99/99/9999" MaskType="Date"
                                        UserDateFormat="DayMonthYear"></asp:MaskedEditExtender>
                                    <asp:Button ID="btnDate" runat="server" Height="20px"
                                        Width="20px" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" PopupButtonID="btnDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorDate" runat="server"
                                        ControlToValidate="txtDate" Text="*" SetFocusOnError="True"
                                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="txtDate_RequiredFieldValidator"
                                        runat="server" ErrorMessage="*" ControlToValidate="txtDate"
                                        ValidationGroup="check" Visible="True" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_REGION" runat="server" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldRegion" runat="server" ControlToValidate="ddlRegion" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:UpdatePanel ID="UpDistricts" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlDistrict" runat="server">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldDistrict" runat="server" ControlToValidate="ddlDistrict" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                        </table>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upPricelistMI" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlMedicalItems" runat="server" ScrollBars="Auto" Height="500px" Width="650px"
                                    CssClass="panel" GroupingText='<%$ Resources:Resource, L_MEDICALITEMS%>'>
                                    <asp:GridView ID="gvMedicalItems" runat="server"
                                        AutoGenerateColumns="False"
                                        ShowSelectButton="True"
                                        GridLines="None"
                                        CssClass="mGrid"
                                        PagerStyle-CssClass="pgr"
                                        EmptyDataText='<%$ Resources:Resource, M_NOMEDICALITEMS %>'
                                        DataKeyNames="Checked,PriceOverule,ItemID,PLItemDetailID">

                                        <Columns>


                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText='<%$ Resources:Resource,L_CODE %>' SortExpression="ServCode">
                                                <HeaderStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ItemName" HeaderText='<%$ Resources:Resource,L_NAME %>' SortExpression="ServName"
                                                HeaderStyle-Width="200px">
                                                <HeaderStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ItemType" HeaderText='<%$ Resources:Resource,L_TYPE %>'
                                                SortExpression="ServType" HeaderStyle-Width="60px" ApplyFormatInEditMode="true">
                                                <HeaderStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ItemPrice" HeaderText='<%$ Resources:Resource,L_PRICE %>'
                                                SortExpression="ServPrice" HeaderStyle-Width="60px"
                                                DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText='<%$ Resources:Resource,L_OVERRULEPRICE %>' HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" class="label" Width="100%" Style="display: block"
                                                        Text='<%# Bind("PriceOverule", "{0:n2}") %>'></asp:Label>
                                                    <asp:TextBox class="textbox numbersOnly" ID="TextBox1" runat="server" Text='<%# Bind("PriceOverule") %>' Width="50px" Height="12px" Style="display: block; text-align: right; display: none;"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" CssClass="switch" />

                                            </asp:TemplateField>

                                        </Columns>

                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <SelectedRowStyle CssClass="srs" />

                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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

<asp:Content ID="Content2" ContentPlaceHolderID="Footer" runat="Server">
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:HiddenField ID="hfCancel" runat="server" />
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</asp:Content>


