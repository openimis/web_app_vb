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



<%@ Page Language="vb" MasterPageFile="~/IMIS.Master" AutoEventWireup="false" CodeBehind="PaymentOverview.aspx.vb" Inherits="IMIS.PaymentOverview" Title='<%$ Resources:Resource,L_PAYMENTOVERVIEW %>' %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<script type="text/javascript">
    $(document).ready(function () {
        
    
        $("#<%=hfClaimAdminAdjustibility.ClientID %>").val(("<%= IMIS.General.getControlSetting("PaymentOverview") %>"));

    });
   
</script>
      <asp:HiddenField ID="hfClaimAdminAdjustibility" runat="server" Value="" />
    <table class="catlabel">
         <tr>
            <td >
                <asp:Label  ID="L_POLICYHOLDER" runat="server" Text='<%$ Resources:Resource,L_PAYMENTOVERVIEW %>'></asp:Label>   
        </td>
        </tr>
         <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>
    </table> 



    <asp:Panel ID="L_FAMILYPANEL" runat="server"  height="200px" 
             CssClass="panel" >
           
               <table>
                    <tr>
                        
                         <td class="FormLabel">
                             <asp:Label ID="lblOfficerCode" runat="server" Text="<%$ Resources:Resource,L_ENROLMENTOFFICERS %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtOfficerCode" runat="server" />
                        </td>
                     
                        <td class="FormLabel">
                            <asp:Label ID="lblPaymentDate" runat="server" Text="<%$ Resources:Resource,L_PAYDATE %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPaymentDate" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel" style="text-align:right;">
                             <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Resource,L_PAYMENTSTATUS %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtStatus" runat="server"></asp:Label>
                        </td>
                        
                    </tr>
                    
                   <tr>
                        <td class="FormLabel">
                            <asp:Label 
                                ID="lblReceiptNo"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_RECEIPT %>'></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtReceiptNo" runat="server" />
                        </td>

                    <td class="FormLabel">
                            <asp:Label ID="lblReceivedDate" runat="server" Text="<%$ Resources:Resource,L_RECEIVEDDATE %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtReceivedDate" runat="server"></asp:Label>
                        </td>
                        

                        <td class="FormLabel" style="text-align:right;">
                             <asp:Label ID="lblReceivedAmount" runat="server" Text="<%$ Resources:Resource,L_RECEIVEDAMOUNT %>"></asp:Label>
                         </td>
                         <td class="ReadOnlyText">
                             <asp:Label ID="txtReceivedAmount" runat="server"></asp:Label>
                         </td>



                   </tr>
                    <tr>

                          <td class="FormLabel">
                              <asp:Label ID="lblControlNo" runat="server" Text="<%$ Resources:Resource,L_CONTROLNUMBER %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtControlNo" runat="server" />
                        </td>

                    <td class="FormLabel">
                            <asp:Label ID="lblMatchedDate" runat="server" Text="<%$ Resources:Resource,L_MATCHEDDATED %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtMatchedDate" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel" style="text-align:right;">
                             <asp:Label ID="lblPaymentOrigin" runat="server" Text="<%$ Resources:Resource,L_PAYMENTORIGIN %>"></asp:Label>
                          </td>
                        
                          <td class="ReadOnlyText">
                              <asp:Label ID="txtPaymentOrigin" runat="server"></asp:Label>
                          </td>
                        
                    </tr>
                
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="lblTransactionNo" runat="server" Text="<%$ Resources:Resource,L_TRANSACTIONNUMBER %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtTransactionNo" runat="server" />
                        </td>
                       
                        <td class="FormLabel">
                            <asp:Label ID="lblMatchedDate0" runat="server" Text="<%$ Resources:Resource,L_PAYMENTID %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPaymentId" runat="server"></asp:Label>
                        </td>

                        <td class="FormLabel">
                            <asp:Label ID="lblExpectedAmount" runat="server" Text="<%$ Resources:Resource,L_EXPECTEDAMOUNT %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtExpectedAmounts" runat="server"></asp:Label>
                        </td>
                    </tr>
                   <tr>

                   </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:Resource,L_PHONE %>"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="txtPhoneNo" runat="server" />
                        </td>
                        <td class="auto-style2" >
                            </td>
                        <td class="auto-style3">
                            </td>
                        <td class="auto-style4">
                            </td>
                        <td class="auto-style3">
                            </td>
                    </tr>
                </table>      
                    
                <div style="text-align:center;">
                    <div style="width:50%; margin: 0 auto; text-align:center;">
                        <table align="center">
                            <tr>
                                <th class="auto-style3">
                                </th>
                                <th class="auto-style2" style="text-align:left">
                                    <asp:Label ID="lblRejectedReason" runat="server" Text="<%$ Resources:Resource,L_REJECTEDREASON %>"></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                </td>
                                <td class="auto-style3" style="text-align:left">
                                    <asp:Label ID="txtRejectedReason" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                    
         </asp:Panel>
    <table class="catlabel">
                    <tr>
                        <td class="auto-style5" >
                            <asp:Label  ID="L_PAYMENTDETAILS" runat="server"  Text='<%$ Resources:Resource,L_PAYMENTDETAILS %>'></asp:Label>  
                           &nbsp;  &nbsp;
                             <asp:CheckBox class="checkbox" ID="chkLegacy" runat="server" style="color:white;" Text='<%$ Resources:Resource,L_ALL %>'  AutoPostBack="true" />
                        </td>
                        
                       
                        
                    </tr>
                    </table>
    <asp:Panel ID="Panel4" runat="server"  height="104px" 
                                            ScrollBars="Auto" BorderStyle ="Groove" 
                     CssClass="panel">
                     
                      <asp:GridView ID="gvPaymentDetails" runat="server"  
                        AutoGenerateColumns="False"
                       AutoGenerateSelectButton="false" 
                        DataKeyNames="PaymentDetailsID"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt"
                        SelectedRowStyle-CssClass  = "srs" 
                        >
                  <%--  <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="LightSteelBlue"/>
                    <AlternatingRowStyle BackColor="white" />--%>
                   
                    <Columns>
                   <%--<asp:HyperLinkField DataNavigateUrlFields = "PolicyID,FamilyID" DataTextField="PolicyID" DataNavigateUrlFormatString = "CreatePolicy.aspx?p={0}&f={1}" HeaderText="Policy ID"  HeaderStyle-Width ="60px" >
                        <HeaderStyle Width="60px" />
                        </asp:HyperLinkField> --%> 
              <%--      <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                            ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                        <HeaderStyle CssClass="HideButton" />
                        <ItemStyle CssClass="HideButton" />
                        </asp:CommandField>--%>
                 
                   <asp:CommandField  SelectText ="Select" ShowSelectButton="false" ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >  </asp:CommandField>
                    <%--       <asp:BoundField DataField="PaymentID" HeaderStyle-Width="110px" HeaderText='<%$ Resources:Resource,L_PAYMENTID %>' SortExpression="PaymentID">   </asp:BoundField>--%>
                        <asp:BoundField DataField="InsuranceNumber" HeaderStyle-Width="110px" HeaderText='<%$ Resources:Resource,L_CHFID %>' SortExpression="InsuranceNumber">   </asp:BoundField>
                        <asp:BoundField DataField="ProductCode" HeaderStyle-Width="110px" HeaderText='<%$ Resources:Resource,L_PRODUCT %>' SortExpression="ProductCode"> </asp:BoundField>
                        <asp:BoundField DataField="PolicyStage" HeaderStyle-Width="50px" HeaderText='<%$ Resources:Resource,L_POLICYSTAGE %>' SortExpression="PolicyStage"> </asp:BoundField>
                        <asp:BoundField DataField="Amount"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_AMOUNT %>' SortExpression="Amount"> </asp:BoundField>
                        <asp:BoundField DataField="ExpectedAmount"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_EXPECTEDAMOUNT %>' SortExpression="Amount"> </asp:BoundField>
                        <asp:BoundField DataField="MatchedDate" DataFormatString="{0:d}"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_MATCHEDDATED %>' SortExpression="MatchedDate"> </asp:BoundField>
                        <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom"> </asp:BoundField>
                        <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="ValidityTo"> </asp:BoundField>
                    
                    </Columns>
                          <PagerStyle CssClass="pgr" />
                          <SelectedRowStyle CssClass="srs" />
                          <AlternatingRowStyle CssClass="alt" />
                          <RowStyle CssClass="normal" />
                    </asp:GridView>
                    
                   
                     </asp:Panel>

   
    <table class="catlabel">
                    <tr>
                        <td >
                            <asp:Label  ID="L_EDITPAYMENTDETAILS" runat="server" Text='<%$ Resources:Resource,L_EDITPAYMENTDETAILS %>'></asp:Label>   
                        </td>
                        <td  align="right" style="padding-left:10px; vertical-align:bottom">                        
                                           <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%><%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%><%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>                         <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>
                    </tr>
                    </table>
  
<asp:Panel ID="Panel2" runat="server"  height="300px" 
                                            ScrollBars="Auto" BorderStyle ="Groove" 
                     CssClass="panel">
                     
                      
                    <table >
                    <tr>
                        
                         <td class="FormLabel">
                            <asp:Label 
                                ID="Label1"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_CHFID %>'>
                            </asp:Label>
                        </td>
                          <td class="DataEntry">
                                            <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtEditInsuranceNumber" runat="server" AutoPostBack="True" CssClass="numbersOnly" MaxLength="12" Width="150px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldCHFID0" runat="server" ControlToValidate="txtEditInsuranceNumber" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>


                                        </td>
                        <%--  <td class="FormLabel">
                            <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Resource,L_RECEIVEDAMOUNT %>"></asp:Label>
                        </td>--%>
                 
                    
                                     <td class="FormLabel">
                                         &nbsp;</td>
                        <td class="DataEntry">
                            

                                        </td>
           
                        
                     
                        
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
                                                &nbsp;</td>
                        <td class="DataEntry">
                                            
                                            
                                        </td>

                   
                   </tr>
                    <tr>

                          <td class="FormLabel">
                            <asp:Label 
                                ID="Label10"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_DISTRICT %>'>
                            </asp:Label>
                        </td>
                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>

                   
                        
                    </tr>
                
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource,L_PRODUCT %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlProduct" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                                    </asp:RequiredFieldValidator>
                        </td>
                       
                        <td class="FormLabel">&nbsp;</td>
                        <td class="ReadOnlyText">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="FormLabel" runat="server" Text="<%$ Resources:Resource,L_POLICYSTAGE %>">
                            </asp:Label>
                            </td>
                            <td class="ReadOnlyText">
                                <asp:DropDownList ID="ddlPolicyStage" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPolicyStage" SetFocusOnError="True" Text="*" ValidationGroup="check">
                                                    </asp:RequiredFieldValidator>
                            </td>
                            <td class="FormLabel">&nbsp;</td>
                            <td class="ReadOnlyText">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                </table>
                   
                     </asp:Panel>

             
    <asp:Panel ID="pnlButtons" runat="server" height="30px"  CssClass="panel" >
        <table width="100%" cellpadding="10 10 10 10">
            <tr>
                    
                     <td  align="left">
                    <asp:Button 
                        ID="BtnMatchPayment" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_MATCHPAYMENT%>' Visible="false"
                          />
                    </td>
                     <td align="left">
                       <asp:Button 
                        ID="btnSaveEditedPaymentDetails" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_SAVE%>' CausesValidation="true" ValidationGroup ="check"
                          />                  
                    </td>
                    
                    <td align="center">
                   <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>
                    </td>
                <asp:Button 
                     Visible ="false"
                    ID="B_VIEW" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_VIEW%>'
                      />
                </td>
                <td>
                       <td align="right">
                 <asp:Button 
                    
                    ID="B_CANCEL" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_CANCEL%>'
                      />
                    </td>
                    
                    
                   
                    
                </tr>
        </table>             
    </asp:Panel> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:HiddenField runat="server" ID="hfPolicyValue" Value="0" />
    <asp:Panel ID="pnlMsgHolder" runat="server" ></asp:Panel>
</asp:Content>

<asp:Content ID="Content4" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2 {
            height: 22px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }
        .auto-style3 {
            width: 170px;
            height: 22px;
            font-family: Arial, Helvetica, sans-serif;
            color: #000080;
            text-align: left;
        }
        .auto-style4 {
            height: 22px;
        }
        .auto-style5 {
            height: 27px;
        }
    </style>
</asp:Content>


