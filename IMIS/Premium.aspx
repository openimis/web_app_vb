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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Premium.aspx.vb" Inherits="IMIS.Premium" 
    title='<%$ Resources:Resource,L_PREMIUM %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server" >
   
    <script type="text/javascript" >
    
        var premiumContributionSum = 0;
        var policyValue = 0;
        var policyStatus = "";
        var PrevBalance = 0;
        var PrevContribution = 0;
        var LastDate = "";
        var TotalInstallments = 0;
        var MaxInstallments = 0;
        var PayDate = "";
        var L = M = false;
        var P = 0;
        var Msg = "";
        var PremiudId = $.trim('<%=HttpContext.Current.Request.QueryString("p") %>') == '' ? 0 : '<%=HttpContext.Current.Request.QueryString("p") %>';
        /* Initializing the popup object properties */
        popup.shadeBG_ID = "SelectPic";
        //popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
        //popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True) %>';



        function categoryOnChange($ddl) {
            if ($ddl.val() == '')
                $("#<%=txtPremiumPaid.ClientID %>").attr('readonly', 'readonly');
            else {
                if ($("#<%=ddlCategory.ClientID %>").val() == 'P') {
                        $("#<%=txtPremiumPaid.ClientID %>").val(0);
                        $("#<%=txtBalance.ClientID %>").val(PrevBalance);
                        $("#<%=txtPremiumContribution.ClientID %>").val(PrevContribution);
                    //return false;
                } else {
                    if ($("#<%=txtPremiumPaid.ClientID %>").val() == '')
                    {
                        if (parseFloat(PrevBalance.replace(",", "")) >= 0)
                        {
                            $("#<%=txtPremiumPaid.ClientID %>").val(parseFloat(PrevBalance.replace(",", "")));
                        }
                        else
                            $("#<%=txtPremiumPaid.ClientID %>").val(0);
                       
                    } 
                    $("#<%=txtPremiumPaid.ClientID %>").trigger('change');
                }
                $("#<%=txtPremiumPaid.ClientID %>").removeAttr('readonly');
            }
            $("#<%=txtPremiumPaid.ClientID %>").focus();
        }
        $(document).ready(function() {

            PrevBalance = $("#<%=txtBalance.ClientID %>").val();
            PrevContribution = $("#<%=txtPremiumContribution.ClientID %>").val();
            LastDate = new Date(formatServerDateToJSDate($("#<%=hfLastDate.ClientID %>").val()));
            PayDate = "";
            MaxInstallments = $("#<%=hfMaxInstallments.ClientID %>").val();
            if (PremiudId == 0)
                TotalInstallments = parseInt($("#<%=hfTotalInstallments.ClientID %>").val()) + 1; // plus the one being made now if it is addition
            else
                TotalInstallments = $("#<%=hfTotalInstallments.ClientID %>").val();


            $("#<%=ddlCategory.ClientID %>").change(function(e) {
                categoryOnChange($(this));
            });
            if (parseInt(PremiudId) === 0)
                categoryOnChange($("#<%=ddlCategory.ClientID %>"));

            //popup.alert(format.numberWithCommas("2000896593555.46678898.67733333333333333333333"));
            $("#<%=B_SAVE.ClientID %>").click(function () {

                premiumContributionSum = 0;
                policyValue = 0;
                policyStatus = "";
                PayDate = "";
                L = M = false;
                P = 0;
                Msg = "";
                var isValid = validateControls();
                if (isValid == false) return false;


                premiumContributionSum = parseFloat($("#<%=hfPremiumContribution.ClientID %>").val());
                policyValue = parseFloat($("#<%=hfPolicyValue.ClientID %>").val());
                policyStatus = $("#<%=hfPolicyStatus.ClientID %>").val(); //alert($("#<%=hfPolicyStatus.ClientID %>").val() + " fetched");
                PayDate = new Date(formatServerDateToJSDate($("#<%=txtPaymentDate.ClientID %>").val()));
                if ($("#<%=ddlCategory.ClientID %>").val() == 'C' || $("#<%=ddlCategory.ClientID %>").val() == null || $("#<%=ddlCategory.ClientID %>").val() == '') {
                    premiumContributionSum += parseFloat($("#<%=txtPremiumPaid.ClientID %>").val().replace(",", "")) //?0:$("#<%=txtPremiumPaid.ClientID %>").val());
                   
                    
                    // checking installments details >> START
                    if (TotalInstallments == MaxInstallments && premiumContributionSum < policyValue) {
                        M = true;
                        P = 0;
                        Msg += '<%=imisgen.getMessage("M_LASTINSTALLMENT", True)%> ';
                    } if (TotalInstallments > MaxInstallments && premiumContributionSum < policyValue) {
                        M = true;
                        Msg += '<%=imisgen.getMessage("M_MAXINSTALLMENTEXCEED", True)%>. ';
                    } if (PayDate > LastDate && premiumContributionSum < policyValue) {
                        L = true;
                        Msg += '<%=imisgen.getMessage("M_LATEPAYMENT", True)%>. ';
                    } if (premiumContributionSum < policyValue) {
                        P = 0;
                        Msg += '<%=imisgen.getMessage("M_POLICYNOTCOVERED", True)%>. ';
                    } else if (premiumContributionSum == policyValue) {
                        P = 1;
                        Msg += ' <%=imisgen.getMessage("M_PREMIUMMATCHESPOLICY", True)%>. ';
                       
                    } else {
                        P = 2;
                        Msg += ' <%=imisgen.getMessage("M_PREMIUMEXCEEDSPOLICY", True) %>. ';
                    }
                    if (Msg != "" )
                        reportPolicyPremiumContribution();
                    // checking installments details >> End
                    
                    return false;
                }
            });

           

            $("#<%=txtPremiumPaid.ClientID %>").change(function() { // on value change, recalculate policy details values..
                if ($("#<%=ddlCategory.ClientID %>").val() == 'P') {
                    //$("#<%=txtBalance.ClientID %>").val($("#<%=txtBalance.ClientID %>").data('prevVal'));
                    return false;
                }
               
                $txtPVal = $("#<%=txtPolicyValue.ClientID %>"); // get the textbox obj with policyValue
                $txtContrVal = $("#<%=txtPremiumContribution.ClientID %>"); // get the textbox obj with current contribution, current premium inclusive
                $hfContrVal = $("#<%=hfPremiumContribution.ClientID %>"); // get the hiddenfield obj with current contribution, current premium exclusive
                $txtBal = $("#<%=txtBalance.ClientID %>"); // get the textbox obj with current balance,current premium inclusive

                $txtContrVal.val(parseFloat($hfContrVal.val().replace(",", "")) + parseFloat($(this).val().replace(",", "")));

                $txtBal.val(parseFloat($txtPVal.val().replace(",", "")) - $txtContrVal.val());

                $txtContrVal.val(format.numberWithCommas($txtContrVal.val()));
                $txtBal.val(format.numberWithCommas($txtBal.val()));
                
            });
        });

       

        function reportPolicyPremiumContribution() {
            var evArgs = new Array(); evArgs.push($("#<%=B_SAVE.ClientID %>"));
            var actionMsg = '<%=imisgen.getMessage("M_PLEASESELECTACTION", True)%>';             
             switch( true ){
                 case M == false && L == false && P == 0:
                     evArgs.push(4);
                     evArgs.push("");
                     popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
                     popup.middleBTN_Text = '<%=imisgen.getMessage("L_ENFORCE", True)%>';
                     popup.rejectBTN_Text = '<%=imisgen.getMessage("L_CANCEL", True)%>';
                     Msg += "<br/>" + actionMsg;
                     popup.confirm(Msg, "reDoPostBack", evArgs,"", true);
                     break;
                        
                case M == true || L == true :
                    evArgs.push(5);
                    evArgs.push("");
                    popup.acceptBTN_Text = '<%=imisgen.getMessage("L_WAIT", True)%>';
                    popup.middle1BTN_Text = '<%=imisgen.getMessage("L_SUSPEND", True)%>';
                    popup.middle2BTN_Text = '<%=imisgen.getMessage("L_ENFORCE", True) %>';
                    popup.rejectBTN_Text = '<%=imisgen.getMessage("L_CANCEL", True)%>';
                    Msg += "<br/>" + actionMsg;
                    popup.confirm(Msg, "reDoPostBack", evArgs, "", false, true);
                    break;

              case M == false && L == false && P != 0:
                    evArgs.push(6);
                    evArgs.push("ActivateInsuree");
                    popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
                    popup.rejectBTN_Text = '<%=imisgen.getMessage("L_CANCEL", True)%>';
                    popup.confirm(Msg, "reDoPostBack", evArgs, false);
                  


             }
             
}

        function reDoPostBack(popupBtnSource, evArgs, popupBtnText) {
            if (popupBtnSource != "cancel")
            {
                if (!validateControls())
                    return;
                if (evArgs[1] === 4 || evArgs[1] === 5)
                    theForm.__EVENTARGUMENT.value = popupBtnText;
                else
                    theForm.__EVENTARGUMENT.value = evArgs[2];

                theForm.__EVENTARGUMENT_PREMIUM.value = evArgs[1];
                theForm.__EVENTTARGET.value = evArgs[0].attr("id");
                theForm.submit();
            }
                
            }

            function validateControls() {
                if (Page_Validators != undefined && Page_Validators != null) {

                    for (i = 0; i < Page_Validators.length; i++) {//alert(Page_Validators[i]);
                         
                        ValidatorEnable(Page_Validators[i])
                               
                        if (!Page_Validators[i].isvalid);
                    }
                }//alert(Page_IsValid);
                if (!Page_IsValid)
                    return false;

                return true;
            }
         
    </script>
    
    <style type="text/css" >
    .pnlPolicyDetails
    {
        border:0px solid #CCC;
        margin-top: -30px;
        margin-left: 30px;
    }
    
    .tblPolicyDetails tr td
    {
        padding:5px;
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
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <input type="hidden" name="__EVENTARGUMENT_PREMIUM" />
    <div class="divBody" >  
    <asp:Panel ID="L_FAMILYPANEL" runat="server" height="150px" ScrollBars="Auto" 
             CssClass="panel" 
         GroupingText='<%$ Resources:Resource,L_FAMILYPANEL %>'  >
           
               <table class="style15">
                    <tr>
                       
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadCHFID"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_CHFID %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadCHFID" runat="server"  />
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtRegion" runat="server" />
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_CONFIRMATIONNO0" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONTYPE %>"></asp:Label>
                         </td>
                         <td class="ReadOnlyText">
                             <asp:Label ID="txtConfirmationType" runat="server" />
                         </td>
                    </tr>
                    
                    <tr>
                      
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadLastName"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_LASTNAME %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                        <asp:label ID="txtHeadLastName" runat="server"  />
                        </td>
                            <td class="FormLabel">
                                <asp:Label ID="L_DISTRICT" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtDistrict" runat="server" />
                        </td>
                      
                         <td class="FormLabel">
                             <asp:Label ID="L_CONFIRMATIONNO" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>"></asp:Label>
                         </td>
                         <td class="ReadOnlyText" style="vertical-align:top;padding-top:5px;">
                             <asp:Label ID="txtConfirmationNo1" runat="server" style="direction: ltr" />
                         </td>
                      
                    </tr>
                    <tr>
                        
                     
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadOtherNames"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_OTHERNAMES %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadOtherNames" runat="server" width="150px" />
                        </td>
                          <td class="FormLabel">
                              <asp:Label ID="L_WARD" runat="server" Text="<%$ Resources:Resource,L_WARD %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtWard" runat="server"></asp:Label>
                        </td>
                       
                         <td class="FormLabel">
                             <asp:Label ID="L_ADDRESS0" runat="server" Text="<%$ Resources:Resource, L_PARMANENTADDRESS %>"></asp:Label>
                         </td>
                       
                         <td class="ReadOnlyText" rowspan="2" style="vertical-align:top;padding-top:5px;">
                             <asp:Label ID="txtPermanentAddress" runat="server"></asp:Label>
                         </td>
                       
                    </tr>
                    <tr>
                       
                        <td class="FormLabel">
                            <asp:Label ID="lblPoverty" runat="server" Text="<%$ Resources:Resource,L_POVERTY %>">
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPoverty" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_VILLAGE" runat="server" Text="<%$ Resources:Resource,L_VILLAGE %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtVillage" runat="server"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">&nbsp;</td>
                    </tr>
                </table>      
                    
                    
         </asp:Panel>
   <asp:Panel ID="pnlBody" runat="server"  ScrollBars="Auto" CssClass="panel" GroupingText="<%$ Resources:Resource, L_PREMIUM %>">
           <table>
                 <tr>  
                   <td>
                     <table>
                        <tr>
                        <td class="FormLabel">
                            <asp:Label 
                            ID="L_Payer"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_PAYER %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
           
                            <asp:DropDownList ID="ddlPayer" runat="server">
                            </asp:DropDownList>
                        </td>
                       
                
                    </tr>
                     <tr id="trContributionCategory" runat="server">
                        <td class="FormLabel">
                            <asp:Label ID="L_CATEGORY" runat="server" 
                                Text="<%$ Resources:Resource, L_CONTRIBUTIONCATEGORY %>"></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlCategory" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldTypeOfPayment0" runat="server" 
                                ControlToValidate="ddlCategory"  
                                ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text="*"
                                ></asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                        <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_PremiumPaid" runat="server" Text='<%$ Resources:Resource,L_PREMIUMPAID %>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:TextBox ID="txtPremiumPaid" runat="server" style="text-align: right; padding-right:4px" class="numbersOnly"></asp:TextBox>
                        </td>
                        <td>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidatorPremiumPaid" 
                                runat="server" ControlToValidate="txtPremiumPaid" 
                                ValidationGroup="check" ForeColor="Red" Display="Dynamic"  Text="*"
                                ></asp:RequiredFieldValidator>
                           <asp:CompareValidator ControlToValidate="txtPremiumPaid" ID="CompareValidator2"  runat="server" SetFocusOnError ="true"  Type="Currency" Operator="DataTypeCheck"  ValidationGroup ="check" ForeColor="Red" Display="Dynamic"  Text="*"> </asp:CompareValidator>
                            
                        </td>
                    </tr>
                        <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_ReceiptNumber" runat="server" Text='<%$ Resources:Resource,L_RECEIPT %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtReceiptNumber" runat="server" style="text-align:right;  padding-right:4px" ></asp:TextBox></td>
                             <td>                              
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorReceiptNumber" 
                                runat="server" ControlToValidate="txtReceiptNumber" 
                                ValidationGroup="check" ForeColor="Red" Display="Dynamic"  Text="*"
                                ></asp:RequiredFieldValidator>
                            </td>
                            

                        
                    </tr>
                        <tr>
                        <td class="FormLabel">
                        <asp:Label 
                        ID="L_PaymentDate"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_PAYDATE %>'> </asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtPaymentDate" runat="server" Width="120px" MaxLength="10" ></asp:TextBox>

                        
                            <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                PopupButtonID="Button1" TargetControlID="txtPaymentDate">
                            </asp:CalendarExtender>

                          </td>
                          <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldPaymentDate" runat="server" 
                                ControlToValidate="txtPaymentDate" 
                                ValidationGroup="check" SetFocusOnError="True" 
                                ForeColor="Red" Display="Dynamic"
                                Text='*'></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtPaymentDate" SetFocusOnError="True" 
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                    ValidationGroup="check" ForeColor="Red" Display="Dynamic"  Text="*">  </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                        <tr>
                        <td class="auto-style1">
                            <asp:Label ID="L_TypeOfPayment" runat="server" Text='<%$ Resources:Resource,L_TYPEOFPAYMENT %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlTypeOfPayment" runat="server">
                               <%-- <asp:ListItem Value="C">Cash</asp:ListItem>
                                <asp:ListItem Value="B">Bank Transfer</asp:ListItem>
                                <asp:ListItem Value="M">Mobile Phone</asp:ListItem>
                                <asp:ListItem Value="">-- Select Payment Type --</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style2">
                            <asp:RequiredFieldValidator 
                            ID="RequiredFieldTypeOfPayment" runat="server" 
                            ControlToValidate="ddlTypeOfPayment" 
                            SetFocusOnError="True" 
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                            Text='*'></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     </table>
                   </td>
                   <td>
                     <asp:Panel ID="pnlPolicyDetails" runat="server" class="pnlPolicyDetails">
                      <h4 style="text-align:center;text-decoration:underline;"><asp:label ID="lblPolicyDetails" runat="server" Text='<%$ Resources:Resource,L_POLICYDETAILS %>' ></asp:label></h4>
                      <asp:Table runat="server" ID="tblPolicyDetails" class="tblPolicyDetails">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPolicyValue" runat="server" Text="<%$ Resources:Resource,L_POLICYVALUE%>"></asp:Label><br />
                                <asp:TextBox ID="txtPolicyValue" runat="server" Text="0.00" style="text-align:right;padding-right:4px" Enabled="false" ></asp:TextBox>
                            </asp:TableCell>
                            
                            <asp:TableCell>
                                <asp:Label ID="lblBalance" runat="server" Text="<%$ Resources:Resource,L_BALANCE%>"></asp:Label><br />
                                <asp:TextBox ID="txtBalance" runat="server" Text="0.00" style="text-align:right;padding-right:4px" Enabled="false" ></asp:TextBox>
                            </asp:TableCell>
                           
                        </asp:TableRow> 
                        <asp:TableRow>    
                            <asp:TableCell>
                                <asp:Label ID="lblPremiumContribution" runat="server" Text="<%$ Resources:Resource,L_PREMIUMPAID%>"></asp:Label><br />
                                <asp:TextBox ID="txtPremiumContribution" runat="server" Text="0.00" style="text-align:right;padding-right:4px" Enabled="false" ></asp:TextBox>
                            </asp:TableCell>
                            
                            <asp:TableCell>
                                <asp:Label ID="lblPolicyStatus" runat="server" Text="<%$ Resources:Resource,L_POLICYSTATUS%>"></asp:Label><br />
                                <asp:TextBox ID="txtPolicyStatus" runat="server" Text="" style="text-align:left;padding-left:4px" Enabled="false" ></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                      </asp:Table>
                     </asp:Panel>
                   </td>  
                 </tr>                     
           </table>            
         </asp:Panel> 
        <asp:Panel ID="pnlGridPremium" runat="server" height="150px" 
             CssClass="panel" GroupingText="<%$ Resources:Resource, L_PAYMENTGRIDVIEW %>">
           


               <asp:GridView ID="gvPremium" runat="server"
                AutoGenerateColumns="False"
                GridLines="None"
                AllowPaging="False" PagerSettings-FirstPageText="First Page" PagerSettings-LastPageText="Last Page" PagerSettings-Mode="NumericFirstLast"
                CssClass="mGrid"
                EmptyDataText='<%$ Resources:Resource,L_NORECORDS %>'
                PagerStyle-CssClass="pgr" PageSize="15"
                AlternatingRowStyle-CssClass="alt"
                SelectedRowStyle-CssClass="srs">
                <Columns>   
                    <asp:BoundField DataField="Transactions" HeaderText='<%$ Resources:Resource,L_TRANSACTIONNUMBER %>'>
                        <HeaderStyle Width="130px" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Receipt" HeaderText='<%$ Resources:Resource,L_RECEIPTNO %>' 
                         SortExpression="Receipt">
                        <HeaderStyle Width="130px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MatchedAmount" HeaderText='<%$ Resources:Resource,L_MATCHEDAMOUNT %>'
                         SortExpression="Amount" DataFormatString="{0:n2}">
                        <HeaderStyle Width="130px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReceiveDate" HeaderText='<%$ Resources:Resource,L_RECEIVEDATE %>'
                        SortExpression="PayType" />
                    <asp:BoundField DataField="MatchingDate" HeaderText='<%$ Resources:Resource,L_MATCHINGDATE %>'
                        SortExpression="PayType" />
                    <asp:BoundField DataField="PaymentOrigin" HeaderText='<%$ Resources:Resource,L_PAYMENTORIGIN %>'> 
                        </asp:BoundField>
                </Columns> 
            </asp:GridView>
         </asp:Panel> 

  <asp:Panel ID="pnlButtons" runat="server"  CssClass="panel" >
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
         </asp:Panel>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server" Visible="true" >
    <asp:HiddenField ID="hfPolicyID" runat="server" Value="0" />
    <asp:HiddenField ID="hfPolicyValue" runat="server" Value="0" />
    <asp:HiddenField ID="hfPremiumContribution" runat="server" Value="0" />    
    <asp:HiddenField ID="hfPolicyStatus" runat="server" Value="0" />
    <asp:HiddenField ID="hfInsurancePeriod" runat="server" Value="0" />
    <asp:HiddenField ID="hfPolicyStartDate" runat="server" Value="" />
    <asp:HiddenField ID="hfPolicyEffectiveDate" runat="server" Value="" />
    <asp:HiddenField ID="hfPolicyIsOffline" runat="server" Value="" />
    <asp:HiddenField ID="hfPremiumIsOffline" runat="server" Value="" />
     <asp:HiddenField ID="hfLastDate" runat="server" Value="" />
      <asp:HiddenField ID="hfTotalInstallments" runat="server" Value="0" />
       <asp:HiddenField ID="hfMaxInstallments" runat="server" Value="0" />
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>
