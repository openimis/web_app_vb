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
<%@ Page Language="vb" MasterPageFile="~/IMIS.Master" AutoEventWireup="false" CodeBehind="FindPayment.aspx.vb" Inherits="IMIS.FindPayment" Title='<%$ Resources:Resource,L_FINDPAYMENT %>' %>

 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnSearch.ClientID %>').click(function (e) {
                var passed = true;
                $DateControls = $('.dateCheck');
                $DateControls.each(function () {
                    if ($(this).val().length > 0 && !isValidJSDate($(this).val())) {
                        $('#<%=lblMsg.ClientID%>').html('<%= imisgen.getMessage("M_INVALIDDATE", True) %>');
                         $(this).focus();
                         passed = false;
                         return false;
                     }
                 });
                 if (passed == false) {
                     return false;
                 }
            });
        });
    </script>
    <div class="divBody">
    <asp:TextBox
                                                ID="txtHidden"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck" Visible="false" >
                                            </asp:TextBox>
        <table class="catlabel">
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>
                </td>

            </tr>

        </table>
        <asp:UpdatePanel ID="upDistrict" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="chkLegacy" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlTop" runat="server" CssClass="panelTop" Style="height: auto;" GroupingText='<%$ Resources:Resource,L_PAYMENT %>'>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label
                                                ID="Label1"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_CONTROLNUMBER %>'></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtControlNumber" runat="server"></asp:TextBox>
                                        </td>

                                        
                                        <td class="auto-style1">
                                            <asp:Label ID="L_PaymentDate" runat="server" Text='<%$ Resources:Resource,L_PAYDATEFROM %>'></asp:Label>
                                        </td>

                                        <td class="DataEntry" width="150">
                                            <asp:TextBox
                                                ID="txtDateOfPaymentFrom"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck">
                                            </asp:TextBox>
                                            <asp:Button
                                                ID="btnDateOfPaymentFrom"
                                                runat="server"
                                                Class="dateButton"
                                                padding-bottom="3px" />

                                            <asp:CalendarExtender
                                                ID="CalendarExtender3"
                                                runat="server"
                                                TargetControlID="txtDateOfPaymentFrom"
                                                Format="dd/MM/yyyy"
                                                PopupButtonID="btnDateOfPaymentfrom" ClearTime="True">
                                            </asp:CalendarExtender>

                                        </td>
                                         <td class="FormLabel">  
                                            <asp:Label ID="Label11" runat="server" Text='<%$ Resources:Resource,L_RECEIVEDAMOUNTFROM %>'></asp:Label>
                                            <td class="DataEntry">  
                                                <asp:TextBox ID="txtReceivedAmountFrom" runat="server"></asp:TextBox>
                                                <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="BirthDate" Format="dd/MM/yyyy" PopupButtonID="Button1" >
                        </ajax:CalendarExtender>--%>
                                                 </td
                                       
                                    </tr>
                                    <tr>
                                         <td class="FormLabel">
                                            <asp:Label ID="lblRegion" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                                        </td>

                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>

                                        </td>     
                                       
                                            <td class="FormLabel">
                                                <asp:Label ID="L_PAYDATETO" runat="server" Text="<%$ Resources:Resource,L_PAYDATETO %>"></asp:Label>
                                            </td>
                                            <td class="DataEntry">
                                                <asp:TextBox ID="txtDateOfPaymentTo" runat="server" CssClass="dateCheck" Width="120px">
                                            </asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" ClearTime="True" Format="dd/MM/yyyy" PopupButtonID="btnDateOfPaymentTo" TargetControlID="txtDateOfPaymentTo">
                                                </asp:CalendarExtender>
                                                <asp:Button ID="btnDateOfPaymentTo" runat="server" Class="dateButton" padding-bottom="3px" />
                                            </td>
                                        </td>
                            
                                        <td class="FormLabel">
                                            <asp:Label ID="Label12" runat="server" Text='<%$ Resources:Resource,L_RECEIVEDAMOUNTTO %>'></asp:Label>
                                            <td class="DataEntry">
                                                <asp:TextBox ID="txtReceivedAmountTo" runat="server"></asp:TextBox>
                                              
                                                 </td>
                                            </td>

                                          

                                    </tr>
                                    <tr>
                                        
                                               <td class="FormLabel">
                                            <asp:Label 
                                                ID="Label9"
                                                runat="server" 
                                                Text='<%$ Resources:Resource,L_DISTRICT %>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                                            <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>  

                                        
                                        <td class="auto-style1">
                                            <asp:Label ID="Label2" runat="server" Text='<%$ Resources:Resource,L_MATCHINGDATEFROM %>'></asp:Label>
                                        </td>

                                        <td class="DataEntry">
                                            <asp:TextBox
                                                ID="txtMatchingDateFrom"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck">
                                            </asp:TextBox>
                                            <asp:Button
                                                ID="btnMatchingDateFrom" 
                                                runat="server"
                                                Class="dateButton"
                                                padding-bottom="3px" />

                                            <asp:CalendarExtender
                                                ID="CalendarExtender2"
                                                runat="server"
                                                TargetControlID="txtMatchingDateFrom"
                                                Format="dd/MM/yyyy"
                                                PopupButtonID="btnMatchingDateFrom" ClearTime="True">
                                            </asp:CalendarExtender>

                                        </td>
                                        <%--tRANSACTION --%>
                                        
                                        <td class="auto-style1">
                                            <asp:Label ID="lblTransactionNumber" runat="server" Text="<%$ Resources:Resource,L_TRANSACTIONNUMBER %>"></asp:Label>
                                        </td>

                                         <td class="DataEntry">
                                                <asp:TextBox ID="txtTransactionNumber" runat="server"></asp:TextBox>
                                        
                                            </td>

                                   
                                    </tr>
                                    <tr>
                                               
                                      <td class="FormLabel">
                                            <asp:Label ID="Label4" runat="server" Text='<%$ Resources:Resource,L_CHFID %>'></asp:Label>
                                            <td class="DataEntry">
                                                <asp:TextBox ID="txtInsuranceNumber" runat="server"></asp:TextBox>
                                
                                            </td>

                                    
                                       <td class="FormLabel">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource,L_MATCHINGDATETO %>"></asp:Label>
                                            </td>
                                            <td class="DataEntry">
                                            <asp:TextBox
                                                ID="txtMatchingDateTo"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck">
                                            </asp:TextBox>
                                            <asp:Button
                                                ID="btnMatchedAmoubntTo" 
                                                runat="server"
                                                Class="dateButton"
                                                padding-bottom="3px" />

                                            <asp:CalendarExtender
                                                ID="CalendarExtender4"
                                                runat="server"
                                                TargetControlID="txtMatchingDateTo"
                                                Format="dd/MM/yyyy"
                                                PopupButtonID="btnMatchedAmoubntTo" ClearTime="True">
                                            </asp:CalendarExtender>

                                        </td>
                                          <td class="auto-style1">  
                                            <asp:Label ID="L_ReceiptNo" runat="server" Text="<%$ Resources:Resource,L_RECEIPT %>"></asp:Label>
                                        </td>

                                         <td class="DataEntry">  
                                                <asp:TextBox ID="txtReceiptNo" runat="server"></asp:TextBox>
                                        
                                            </td

                                             

                                 
                                           <%-- </td>--%>
                                         
                                    </tr>
                                    <tr>
                                         
                                         <td class="FormLabel">
                                            <asp:Label ID="lblReceiptNo" runat="server" Text="<%$ Resources:Resource,L_PHONE %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                                        </td>


                                        

                                        
                                        
                                        <td class="auto-style1">
                                            <asp:Label ID="Label3" runat="server" Text='<%$ Resources:Resource,L_RECEIVINGDATEFROM %>'></asp:Label>
                                        </td>
<td class="DataEntry">
                                            <asp:TextBox
                                                ID="txtReceivingDateFrom"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck">
                                            </asp:TextBox>
                                            <asp:Button
                                                ID="btnReceivingDateFrom" 
                                                runat="server"
                                                Class="dateButton"
                                                padding-bottom="3px" />

                                            <asp:CalendarExtender
                                                ID="CalendarExtender5"
                                                runat="server"
                                                TargetControlID="txtReceivingDateFrom"
                                                Format="dd/MM/yyyy"
                                                PopupButtonID="btnReceivingDateFrom" ClearTime="True">
                                            </asp:CalendarExtender>

                                        </td>
                                    
                              
                                         <td class="FormLabel">
                                            <asp:Label ID="Label10" runat="server" Text='<%$ Resources:Resource,L_PAYMENTORIGIN %>'></asp:Label>
                                            <td class="DataEntry">
                                                <asp:TextBox ID="txtPaymentOrigin" runat="server"></asp:TextBox>
                                              
                                                 </td>
                                            </td> 

                                    </tr>

                                     <tr>
                                         <td class="FormLabel">
                                            <asp:Label ID="lblProduct" runat="server" Text='<%$ Resources:Resource, L_PRODUCT %>'></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtProductCode" runat="server"></asp:TextBox>
                                        </td>

                                        
                                       
                                        
                                            
                                         <td class="auto-style1">
                                            <asp:Label ID="Label6" runat="server" Text='<%$ Resources:Resource,L_RECEIVINGDATETO %>'></asp:Label>
                                        </td>

                                        <td class="DataEntry">
                                            <asp:TextBox
                                                ID="txtReceivingDateTo"
                                                runat="server"
                                                Width="120px"
                                                CssClass="dateCheck">
                                            </asp:TextBox>
                                            <asp:Button
                                                ID="btnReceivingDateTo" 
                                                runat="server"
                                                Class="dateButton"
                                                padding-bottom="3px" />

                                            <asp:CalendarExtender
                                                ID="CalendarExtender6"
                                                runat="server"
                                                TargetControlID="txtReceivingDateTo"
                                                Format="dd/MM/yyyy"
                                                PopupButtonID="btnReceivingDateTo" ClearTime="True">
                                            </asp:CalendarExtender>


                                             <td class="FormLabel">
                                            <asp:Label ID="lblPaymentStatus" runat="server" Text="<%$ Resources:Resource,L_PAYMENTSTATUS %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlPaymentStatus" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>

                                            
                                    
                                    </tr>
                                    <tr>
                                         <td class="auto-style1">
                                            <asp:Label
                                                ID="L_TypeOfPayment"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtOfficeCode" runat="server"></asp:TextBox>
                                        </td>

                                        
                                        <td class="FormLabel">
                                            <asp:Label ID="Label7" runat="server"   Visible="false"></asp:Label>
                                        </td>
                                         <td class="FormLabel">
                                            <asp:Label ID="Label13" runat="server"   Visible="false"></asp:Label>
                                        </td>

                                         



                                       

                                       

                                        <%-- <td class="FormLabel">
                                            <asp:Label ID="Label11" runat="server"  Visible="false"></asp:Label>
                                        </td>--%>

                                    

                                                    </tr>
                                                </table>
                                            </td>
                            <td>
                                <asp:CheckBox class="checkbox" ID="chkLegacy" runat="server"
                                    Text='<%$ Resources:Resource,L_ALL %>' AutoPostBack="True" />
                                <br />
                                <br />
                                <asp:Button class="button" ID="btnSearch" runat="server"
                                    Text='<%$ Resources:Resource,B_SEARCH %>'></asp:Button>
                                <br />

                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="catlabel">
            <tr>
                <td>
                    <asp:Label
                        ID="L_FOUNDPAYMENTS"
                        runat="server"
                        Text='<%$ Resources:Resource,L_FOUNDPAYMENTS %>'>
                    </asp:Label>
                </td>

            </tr>

        </table>
        <asp:Panel ID="pnlBody" runat="server" CssClass="panelBody">
            <asp:GridView  ID="gvPayments" runat="server"  
                        AutoGenerateColumns="False"
                        GridLines="None"
                       AllowPaging="true" PagerSettings-FirstPageText = "First Page" PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast" PageSize="15"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"
                        DataKeyNames="PaymentID, PaymentUUID" 
                        EmptyDataText='<%$ Resources:Resource,L_NORECORDS %>'>
                       
                        <Columns>
                    <asp:BoundField DataField="ReceivedDate"  DataFormatString="{0:d}"  HeaderText='<%$ Resources:Resource,L_RECEIVEDDATE %>' SortExpression="ReceivedDate" HeaderStyle-Width ="80px" >

                        
                    </asp:BoundField> 
                        <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                                ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                            <HeaderStyle CssClass="HideButton" />
                            <ItemStyle CssClass="HideButton" />
                            </asp:CommandField>
                             <asp:BoundField DataField="ControlNumber"  HeaderText='<%$ Resources:Resource,L_CONTROLNUMBER%>'
                                SortExpression="ControlNumber" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField>
                            <asp:BoundField DataField="PaymentDate" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_PAYDATE%>'
                                SortExpression="PaymentDate" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField>    
                                     <asp:BoundField DataField="PaymentOrigin"  HeaderText='<%$ Resources:Resource,L_PAYMENTORIGIN%>'
                                SortExpression="PaymentOrigin" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField> 
                       
                            
                            <asp:BoundField DataField="OfficerCode"  HeaderText='<%$ Resources:Resource,L_ENROLMENTOFFICERS%>'
                                SortExpression="OfficerCode" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField>

  <asp:BoundField DataField="ReceiptNo"  HeaderText='<%$ Resources:Resource,L_RECEIPT%>'
                                SortExpression="ReceiptNo" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField>
                              <asp:BoundField DataField="TransactionNo"  HeaderText='<%$ Resources:Resource,L_TRANSACTIONNUMBER%>'
                                SortExpression="TransactionNo" HeaderStyle-Width ="70px"> 
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="ReceivedAmount"  HeaderText='<%$ Resources:Resource,L_AMOUNT%>'     
                                SortExpression="ReceivedAmount" HeaderStyle-Width ="70px"> 
                       
                        </asp:BoundField> 
                            <asp:BoundField DataField="PhoneNumber"  HeaderText='<%$ Resources:Resource,L_PHONE%>'     
                                SortExpression="PhoneNumber" HeaderStyle-Width ="70px"> 
                       
                        </asp:BoundField> 
        
                    
                    
                           
                                <asp:BoundField DataField="PaymenyStatusName" HeaderText='<%$ Resources:Resource,L_STATUS %>' 
                                SortExpression="PaymenyStatusName" HeaderStyle-Width="50px" />
                           
                    
                     <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" HeaderStyle-Width="70px">  </asp:BoundField>
                <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="ValidityTo" HeaderStyle-Width="70px">  </asp:BoundField>
                    </Columns>
                   
                        <PagerStyle CssClass="pgr" />
                        <AlternatingRowStyle CssClass="alt" />
                        <SelectedRowStyle CssClass="srs" />
                       <RowStyle CssClass="normal" />
                    </asp:GridView>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <table width="100%" cellpadding="10 10 10 10">
            <tr>

                <td align="left">
                    <%--<asp:Button 
                        ID="B_ADD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_ADD%>'
                          />--%>
                </td>
                <td align="center">

                    <asp:Button
                        Visible="false"
                        ID="B_VIEW"
                        runat="server"
                        Text='<%$ Resources:Resource,B_VIEW%>' />
                </td>

                <td align="center">
                    <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>
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
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server" Visible="true" >
    
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
</asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            height: 27px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 5px;
        }
    </style>
</asp:Content>
