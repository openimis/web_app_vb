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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master"  CodeBehind="OverviewFamily.aspx.vb" Inherits="IMIS.OverviewFamily" 
title='<%$ Resources:Resource,L_FAMILY%>'%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<script type="text/javascript" language="javascript" >
    var url = "";
    var premiumAmountSum = 0;

    /* Initializing the popup object properties */
        popup.shadeBG_ID = "SelectPic";
        popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
        popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';

        $(document).ready(function() {
            $("#<%=gvPolicies.ClientID %> tr").each(function(i) {
                if (i == 0) return;
                $(this).children().eq(4).find("a").click(function(e) { openPopupWindow($(this)); e.preventDefault(); });
            });

            $("#<%=gvPremiums.ClientID %> tr").each(function(i) {
                if (i == 0) return;
                $(this).children().eq(1).find("a").click(function(e) { openPopupWindow($(this)); e.preventDefault(); });
                if ($(this).children().eq($(this).children().length - 1).html() == "Contribution")
                    premiumAmountSum += parseFloat($(this).children().eq(2).html().replace(",", ""));
            });


            $("#<%=AddPremium.ClientID %>").click(function() {
                var serverDate = ('<%= Format(Date.Today, "MM/dd/yyyy") %>');
                var policyExpDate = $("#<%=gvPolicies.ClientID %> tr.srs").eq(0).find("td").eq(3).html();
                //alert("ServerDate: " + new Date(serverDate) + " policy: " + new Date(formatServerDateToJSDate(policyExpDate)));
                if ($.trim(policyExpDate) != "") {
                    policyExpDate = formatServerDateToJSDate(policyExpDate);
                    if (new Date(policyExpDate) <= new Date(serverDate)) {
                        popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
                        popup.alert('<%=imisgen.getMessage("M_EXPIRYPOLICYNOPAY", True )  %>');
                        return false;
                    }
                }

                var arr = new Array(); arr.push($(this))
                var fn = function(btn, r) {
                    if (btn == "ok")
                        rePostBack(r[0].attr("id"), 'AddPremium');
                }
                //alert(isNaN($("#<%=hfPolicyValue.ClientID %>")));
                var policyValue = $("#<%=hfPolicyValue.ClientID %>").val();
                //alert(parseFloat(format.numberWithoutCommas(policyValue)) + " : " + premiumAmountSum);

                if (premiumAmountSum >= parseFloat(format.numberWithoutCommas(policyValue)))
                    popup.confirm('<%=imisgen.getMessage("M_PREMIUMCOVEREDPROMPT", True)%>', fn, arr);
                else
                    rePostBack(arr[0].attr("id"), 'AddPremium');

                return false;
            });

            $("#<%=btnRenewPolicy.ClientID %>").click(function() {
               $me = $(this);
                popup.confirm('<%=imisgen.getMessage("M_RENEWPOLICY", True)%>', function(btn) {                 
                     if( btn == "ok" )
                         rePostBack($me.attr("id"), "RenewButton");
                });
                return false;
            });
            $(".DeleteButton").click(function() {
                var fn = function(btn, args) {
                    if (btn == "ok")
                        rePostBack(args[0].attr("id"), args[1]);
                }

                argArr = new Array();
                argArr[0] = $(this);
                switch ($(this).attr("id")) {
                    case "<%=DeleteFamily.ClientID %>":
                        argArr[1] = "deletefamily"
                        popup.confirm('<%=imisgen.getMessage("M_DELETEFAMILYPROMPT", True)%>', fn, argArr);
                        break;
                    case "<%=DeleteInsuree.ClientID %>":
                        argArr[1] = "deleteinsuree"
                        popup.confirm('<%=imisgen.getMessage("M_DELETEINSUREEPROMPT", True)%>', fn, argArr);
                        break;
                    case "<%=DeletePolicy.ClientID %>":
                        argArr[1] = "deletepolicy"
                        popup.confirm('<%=imisgen.getMessage("M_DELETEPOLICYPROMPT", True)%>', fn, argArr);
                        break;
                    case "<%=DeletePremium.ClientID %>":
                        argArr[1] = "deletepremium"
                        popup.confirm('<%=imisgen.getMessage("M_DELETEPREMIUMPROMPT", True ) %>', fn, argArr);
                        break;
                }
                return false;
            });
        });
    
    function rePostBack(evTarget,evArg){
        theForm.__EVENTTARGET.value = evTarget;
        theForm.__EVENTARGUMENT.value = evArg;
        theForm.submit();
    }

    function openPopupWindow($a) {
        var url = $a.attr("href");
        var width = 1010;
        var height = 690;
        var top = 0;
        var left = $(window).width() / 2 - width / 2;
        //alert(left +" : " + url);return;
        win = window.open(url, "popup", "width=" + width + ",height=" + height + ",top=" + top + ",left=" + left + ",toolbar=false");
        win.focus()
    }

</script>

    <table class="catlabel">
         <tr>
            <td >
                <asp:Label  ID="L_POLICYHOLDER" runat="server" Text='<%$ Resources:Resource,L_POLICYHOLDER %>'></asp:Label>   
        </td>
        <td  align="right" style="padding-left:10px; vertical-align:bottom">
            <asp:ImageButton  class="ImageButton" ImageUrl="~/Images/Add.gif"  ID="AddFamily" runat="server"></asp:ImageButton>
            <asp:ImageButton  class="ImageButton" ImageUrl="~/Images/Modify.png" ID="EditFamily" runat="server"></asp:ImageButton>
            <asp:ImageButton  class="ImageButton DeleteButton" ImageUrl="~/Images/Erase.png" ID="DeleteFamily" runat="server"> </asp:ImageButton>
        </td>
        </tr>
         <%--  <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                            ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                            <HeaderStyle CssClass="HideButton" />
                            <ItemStyle CssClass="HideButton" />
                        </asp:CommandField>--%>
    </table> 
    <asp:Panel ID="L_FAMILYPANEL" runat="server"  height="100px" 
             CssClass="panel" >
           
               <table >
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
                        <td class="FormLabel" >
                            <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                         </td>
                        <td class="ReadOnlyText"   >
                            <asp:Label ID="txtRegion" runat="server" />
                         </td>
                        <td class="FormLabel">
                            <asp:Label ID="lblPoverty" runat="server" Text="<%$ Resources:Resource,L_POVERTY %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPoverty" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel" style="text-align:left;">
                             <asp:Label ID="L_ADDRESS0" runat="server" Text="<%$ Resources:Resource, L_ADDRESS %>"></asp:Label>
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
                           <asp:Label ID="txtHeadLastName" runat="server" />
                       </td>
                       <td class="FormLabel">
                           <asp:Label ID="L_DISTRICT0" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>">
                            </asp:Label>
                       </td>
                       <td class="ReadOnlyText">
                           <asp:Label ID="txtDistrict" runat="server" />
                       </td>
                       <td class="FormLabel">
                           <asp:Label ID="lblConfirmation" runat="server" Text="<%$ Resources:Resource,L_CONFIRMATIONTYPE %>"></asp:Label>
                       </td>
                       <td class="ReadOnlyText">
                           <asp:Label ID="txtConfirmationType" runat="server"></asp:Label>
                       </td>
                       <td class="ReadOnlyText"   style="vertical-align: top; direction: ltr;">
                           <asp:Label ID="txtPermanentAddress" runat="server"></asp:Label>
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
                            <asp:Label ID="txtHeadOtherNames" runat="server"  />
                        </td>
                          <td class="FormLabel">
                              <asp:Label ID="L_WARD" runat="server" Text="<%$ Resources:Resource,L_WARD %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtWard" runat="server"></asp:Label>
                        </td>
                     
                         <td class="FormLabel">
                          
                             <asp:Label ID="lblConfirmationNo" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                                                      <asp:Label ID="txtConfirmationNo" runat="server" style="direction: ltr" />
                        </td>
                        <td>

                        </td>
                    </tr>
                
                    <tr>
                        <td class="FormLabel">
                            <asp:Label
                               ID="lblGroupType"
                               runat="server"
                               Text='<%$ Resources:Resource,L_GROUPTYPE %>'>
                           </asp:Label>
                        </td>
                        <td class="ReadOnlyText"><asp:Label ID="txtHeadGroupType" runat="server"  /></td>
                        <td class="FormLabel">
                            <asp:Label ID="L_VILLAGE" runat="server" Text="<%$ Resources:Resource,L_VILLAGE %>">
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtVillage" runat="server">
                            </asp:Label>
                        </td>
                        <td class="FormLabel">&nbsp;</td>
                        <td class="ReadOnlyText">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>      
                    
                    
         </asp:Panel>
    <table class="catlabel">
                    <tr>
                        <td >
                            <asp:Label  ID="Label2" runat="server"  Text='<%$ Resources:Resource,L_INSUREES %>'></asp:Label>   
                        </td>
                        <td  align="right" style="padding-left:10px; vertical-align:bottom">
                        
                        <asp:ImageButton class="ImageButton" ImageUrl="~/Images/Add.gif"  ID="AddInsuree" runat="server"></asp:ImageButton>
                        <asp:ImageButton class="ImageButton" ImageUrl="~/Images/Modify.png"  ID="EditInsuree" runat="server"></asp:ImageButton>
                        <asp:ImageButton class="ImageButton DeleteButton" ImageUrl="~/Images/Erase.png"  ID="DeleteInsuree" runat="server"></asp:ImageButton>
                        
                        </td>
                    </tr>
                    </table>
    <asp:Panel ID="Panel1" runat="server" height="155px" ScrollBars="Auto" CssClass="panel" >
                 
                   
                <asp:GridView  ID="gvInsurees" runat="server"  
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true" PagerSettings-FirstPageText = "First Page" PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast" 
                    CssClass="mGrid"
                    PagerStyle-CssClass="pgr"
                    DataKeyNames="InsureeID" >
                       
                    <Columns>
                        <asp:CommandField  SelectText ="Select" ShowSelectButton="true" ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >  <HeaderStyle CssClass="HideButton" /> <ItemStyle CssClass="HideButton" />  </asp:CommandField>
                        <asp:HyperLinkField DataNavigateUrlFields="InsureeUUID,FamilyUUID" DataNavigateUrlFormatString="Insuree.aspx?i={0}&f={1}" DataTextField="CHFID" HeaderText='<%$ Resources:Resource,L_CHFID %>' HeaderStyle-Width="60px"/>
                        <asp:BoundField DataField="LastName" HeaderStyle-Width="110px" HeaderText='<%$ Resources:Resource,L_LASTNAME %>' SortExpression="LastName">   </asp:BoundField>
                        <asp:BoundField DataField="OtherNames" HeaderStyle-Width="110px" HeaderText='<%$ Resources:Resource,L_OTHERNAMES %>' SortExpression="OtherNames"> </asp:BoundField>
                        <asp:BoundField DataField="Gender" HeaderStyle-Width="50px" HeaderText='<%$ Resources:Resource,L_GENDER %>' SortExpression="Gender"> </asp:BoundField>
                        <asp:BoundField DataField="DOB" DataFormatString="{0:d}"  HeaderStyle-Width="70px" HeaderText='<%$ Resources:Resource,L_BIRTHDATE %>' SortExpression="DOB"> </asp:BoundField>
                        <asp:CheckBoxField DataField="CardIssued" HeaderStyle-Width="50px" HeaderText='<%$ Resources:Resource,L_CARD %>' SortExpression="CardIssued"> </asp:CheckBoxField>
                    </Columns>
                    <SelectedRowStyle CssClass="srs" />
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <RowStyle CssClass="normal" />
                       
                </asp:GridView>
                    
                     </asp:Panel> 
    <table class="catlabel">
                    <tr>
                        <td >
                            <asp:Label  ID="L_POLICIES" runat="server" Text='<%$ Resources:Resource,L_POLICIES %>'></asp:Label>   
                        </td>
                        <td  align="right" style="padding-left:10px; vertical-align:bottom">                        
                                               <asp:ImageButton class="ImageButton" ImageUrl="~/Images/Add.gif"  ID="AddPolicy" runat="server"></asp:ImageButton>
                        <asp:ImageButton class="ImageButton" ImageUrl="~/Images/Modify.png"  ID="EditPolicy" runat="server"></asp:ImageButton>
                        <asp:ImageButton class="ImageButton DeleteButton" ImageUrl="~/Images/Erase.png"  ID="DeletePolicy" runat="server"></asp:ImageButton>
                         <asp:ImageButton class="ImageButton" ImageUrl="~/Images/Renew.png"  
                                ID="btnRenewPolicy" runat="server"></asp:ImageButton>
                        </td>
                    </tr>
                    </table>
  
<asp:Panel ID="Panel2" runat="server"  height="104px" 
                                            ScrollBars="Auto" BorderStyle ="Groove" 
                     CssClass="panel">
                     
                      <asp:GridView ID="gvPolicies" runat="server"  
                        AutoGenerateColumns="False"
                        EmptyDataText='<%$ Resources:Resource,M_NOPOLICIES %>'
                        DataKeyNames="PolicyID,ProdID,ProdUUID, ExpiryDate"
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
                         <asp:HyperLinkField DataNavigateUrlFields="PolicyUUID,FamilyUUID" 
                        DataNavigateUrlFormatString="Policy.aspx?po={0}&f={1}" 
                        DataTextField="EnrollDate" DataTextFormatString="{0:d}" 
                        HeaderText='<%$ Resources:Resource,L_ENROLDATE %>' />
                    <asp:BoundField DataField="EffectiveDate" HeaderText='<%$ Resources:Resource,L_EFFECTIVEDATE %>'  SortExpression="EffectiveDate" DataFormatString="{0:d}" ></asp:BoundField>
                    <asp:BoundField DataField="StartDate" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_STARTDATE %>' SortExpression="StartDate" />
                    <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_EXPIRYDATE %>' SortExpression="ExpiryDate" />
                    <asp:HyperLinkField DataNavigateUrlFields = "ProdUUID"  DataTextField="ProductCode" DataNavigateUrlFormatString = "Product.aspx?p={0}&x=1" HeaderText='<%$ Resources:Resource,L_PRODUCT %>'  HeaderStyle-Width ="80px" >  </asp:HyperLinkField> 
                    <%--<asp:BoundField DataField="ProductCode"  HeaderText="PRODUCT" SortExpression="ProductCode" />--%>
                    <asp:BoundField DataField="OfficerName"  HeaderText='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>' SortExpression="OfficerName" />                 
                    <asp:BoundField DataField="PolicyStatus" HeaderText='<%$ Resources:Resource,L_POLICYSTATUS %>' SortExpression="PolicyStatus" ></asp:BoundField>
                    <asp:BoundField DataField="PolicyValue" DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_POLICYVALUE %>' SortExpression="PolicyValue"  ></asp:BoundField>
                    <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" >  </asp:BoundField>
                <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="DOB" >  </asp:BoundField>   
                    
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
                    <asp:Label  ID="Label1" runat="server" Text='<%$ Resources:Resource,L_PREMIUMS %>'></asp:Label>   
                </td>
                <td  align="right" style="padding-left:10px; vertical-align:bottom">
            <asp:ImageButton  class="ImageButton" ImageUrl="~/Images/Add.gif"  ID="AddPremium" runat="server"></asp:ImageButton>
            <asp:ImageButton  class="ImageButton" ImageUrl="~/Images/Modify.png" ID="EditPremium" runat="server"></asp:ImageButton>
            <asp:ImageButton class="ImageButton DeleteButton" ImageUrl="~/Images/Erase.png" text= "Delete" ID="DeletePremium" runat="server"></asp:ImageButton>
        </td>
            </tr>
            
        </table>
    <asp:Panel ID="pnlPremiums" runat="server"  height="104px" 
                                            ScrollBars="Auto" BorderStyle ="Groove" 
                     CssClass="panel">
                    <asp:GridView ID="gvPremiums" runat="server"  
                        AutoGenerateColumns="False"
                        EmptyDataText='<%$ Resources:Resource,M_NOPREMIUMS %>'
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt"
                        SelectedRowStyle-CssClass = "srs" 
                         DataKeyNames="PremiumID" PageSize="2">
                  
                    <Columns>
                      <%--  <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                            ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                            <HeaderStyle CssClass="HideButton" />
                            <ItemStyle CssClass="HideButton" />
                        </asp:CommandField>--%>
                        <%--<asp:BoundField DataField="PayDate" DataFormatString="{0:d}" HeaderText="PAY DATE" SortExpression="PayDate" />--%>
                         <asp:HyperLinkField DataNavigateUrlFields = "PremiumUUID,FamilyUUID,PolicyUUID" DataTextField="PayDate" DataTextFormatString="{0:d}" DataNavigateUrlFormatString = "Premium.aspx?p={0}&f={1}&po={2}" HeaderText='<%$ Resources:Resource,L_PAYDATE %>'  ></asp:HyperLinkField>  
                         <asp:HyperLinkField DataNavigateUrlFields = "PayerUUID,FamilyUUID" Target="_search" DataTextField="PayerName" DataNavigateUrlFormatString = "Payer.aspx?p={0}&f={1}&x=1" HeaderText='<%$ Resources:Resource,L_PAYER %>'  ></asp:HyperLinkField>  
                      
                        
                      <%--  <asp:BoundField DataField="PayerName"  HeaderText="PAID BY" 
                            SortExpression="PayerName" />--%>
                        <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_AMOUNT %>' SortExpression="Amount"  /> 
                        <asp:BoundField DataField="PayType" HeaderText='<%$ Resources:Resource,L_PAYMENTTYPE %>' SortExpression="PayType" />
                        <asp:BoundField DataField="Receipt" HeaderText='<%$ Resources:Resource,L_RECEIPT %>' SortExpression="Receipt" />  
                        <asp:BoundField DataField="PayCategory" HeaderText='<%$ Resources:Resource,L_CONTRIBUTIONCATEGORY%>' SortExpression="PayCategory" />  
                        <asp:BoundField DataField="MatchedAmount" HeaderText='<%$ Resources:Resource,L_MATCHEDAMOUNT%>' SortExpression="MatchedAmount" />  
                    </Columns>  
                        <PagerStyle CssClass="pgr" />
                        <SelectedRowStyle CssClass="srs" />
                        <AlternatingRowStyle CssClass="alt" />
                        <RowStyle CssClass="normal" />
                    </asp:GridView>
                     </asp:Panel>               
    <asp:Panel ID="pnlButtons" runat="server" height="30px"  CssClass="panel" >
        <table width="100%" cellpadding="10 10 10 10">
            <tr>
                    
                     <td  align="left">
                    <%--<asp:Button 
                        ID="B_ADD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_ADD%>'
                          />--%>
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
