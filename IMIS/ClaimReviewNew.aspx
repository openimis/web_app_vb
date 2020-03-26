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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClaimReviewNew.aspx.vb" MasterPageFile="~/IMIS.Master"  Inherits="IMIS.ClaimReviewNew" title='<%$ Resources:Resource,L_CLAIMREVIEWPAGETITLE%>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server" >
    <script type="text/javascript" language="javascript">
        var ClaimTotal = 0;
        var PriceValue;
        var QtyProvided;
        var ApprovedTotal = 0;
        function CalculateClaimTotal(x,y) {
            if (x != "" && y != "") {
                ClaimTotal += parseFloat(y) * parseFloat(x)
            }
        }

        function CalculateApprovedValue() {
            $('.appvalue').each(function() {
                $Row = $(this).parent().parent();
                var AppQty = $Row.find("td").eq(4).find("input[type=text]").val();
                var AppValue = $Row.find("td").eq(5).find("input[type=text]").val();
                var Qty = $Row.find("td").eq(1).html();
                var Value = $Row.find("td").eq(2).html();
                if (AppValue == "" && AppQty == "") {
                    ApprovedTotal += parseFloat(Qty) * parseFloat(Value);
                } else if (AppValue != "" && AppQty == "") {
                    ApprovedTotal += parseFloat(Qty) * parseFloat(AppValue);
                } else if (AppValue == "" && AppQty != "") {
                    ApprovedTotal += parseFloat(AppQty) * parseFloat(Value);
                } else if (AppValue != "" && AppQty != "") {
                    ApprovedTotal += parseFloat(AppQty) * parseFloat(AppValue);
                }
                
            });
            $('#<%=txtApproved.ClientID %>').val(ApprovedTotal.toFixed(2));
            $('#<%=hfApprovedValue.ClientID %>').val(ApprovedTotal.toFixed(2)); 
            ApprovedTotal = 0;
        }

        $(document).ready(function () {
            // Change By Purushottam Starts
            $("#RejectItems").click(function () {	
                $("[id^=Body_gvItems_ddlSTATUS_]").val("2").change();		
            });

		    $("#RejectServices").click(function () {			
		        $("[id^=Body_gvService_ddlSTATUS]").val("2").change();				
            });
            // Change By Purushottam Ends
            $('.PriceAsked').each(function() {
                var $Row = $(this).parent();
                PriceValue = $Row.find("td").eq(2).html();
                QtyProvided = $Row.find("td").eq(1).html();
                CalculateClaimTotal(PriceValue, QtyProvided);
            });
            $('#<%=txtCLAIMTOTALData.ClientID %>').val(ClaimTotal.toFixed(2));
            $('#<%=hfClaimedValue.ClientID %>').val(ClaimTotal.toFixed(2));
            CalculateApprovedValue();
            $('.appvalue').change(function() {
                CalculateApprovedValue();
            });
            $('.appQty').change(function() {
                CalculateApprovedValue();
            });
            var ClmStatusFlag = false;
            $(".ClmStatus").change(function() {
                //alert(ClmStatusFlag);
                if (ClmStatusFlag == true && $(this).val() == 1) {
                    var $Row = $(this).parent().parent();
                    $Row.find("td").eq(4).find("input[type=text]").val("");
                    CalculateApprovedValue();
                    //ClmStatusFlag = false;
                }
                if ($(this).val() == 2) {
                    var $Row = $(this).parent().parent();
                    $Row.find("td").eq(4).find("input[type=text]").val(0);
                    CalculateApprovedValue();
                    ClmStatusFlag = true;
                }
                var $rowID = $(this).parent().parent();
                var Rejection = $rowID.find("td").eq(9).html();
                if ($(this).val() == 1 && Rejection == "-1") {
                    $rowID.find("td").eq(4).find("input[type=text]").val("");
                    CalculateApprovedValue();
                }
            });
        });
        $(window).load(function () {
            var newclaimcode = <%=Request.QueryString("c")%>;
            var claimtoken = "<%=System.Configuration.ConfigurationManager.AppSettings("ClaimDocumentToken").ToString()%>";
            var documenthref = "<%=System.Configuration.ConfigurationManager.AppSettings("ClaimDocumentHome").ToString()%>" + "view_documents?claim_id=" + newclaimcode + "&token=" + claimtoken;
            document.getElementById("lnkViewDocument").href = documenthref;
            var newclaimcodeOLD = $('#<%=lblCLAIMData.ClientID%>').html()
            var documenthrefold = "http://202.45.147.57/mobileapp/claims.php?ClaimCode=" + newclaimcodeOLD;
            document.getElementById("lnkViewDocumentOld").href = documenthrefold;
            if ($('#Body_hfOldClaimID').val() != 0) {
                document.getElementById("lnkOldClaim").href = "ClaimReviewNew.aspx?c=" + $('#Body_hfOldClaimID').val();
            }
            else {
                $('#lnkOldClaim').remove();
                //document.getElementById("lnkOldClaim").;
            }

        });
    </script>
<style type="text/css" >
    .footer{top:665px;}
    .FormLabel {width: auto !important;}
     /*.backentry{ height:629px; }*/
     .panelbuttons{ position:relative;top:0px;}
</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server"> 
    
    <asp:Panel ID="pnlPage" runat="server">
         <div class="divBody" >       
          <asp:Repeater ID="rptInsuree" runat="server">
                        <ItemTemplate>
                            <table>                               
                                <tr class="mGrid">                                        
                                    <td><span class="FormLabel">Name:</span> <span class="DataEntry" style="color:black;font-weight:bolder"><%#Eval("OtherNames") %> &nbsp;<%#Eval("LastName") %></span></td>
                                    <td><span class="FormLabel">Gender:</span> <span class="DataEntry"><%#Eval("Gender") %></span></td>
                                    <td><span class="FormLabel">NSHI:</span> <span class="DataEntry" style="color:black;font-weight:bolder"><%#Eval("CHFID") %></span></td>
                                    <td><span class="FormLabel">DOB/Age:</span> <span class="DataEntry"><%# Eval("DOB", "{0:d}") %><asp:Label ID="lblInsureeAge" runat="server"></asp:Label></span></td>                                   
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>     
           <table  class="catlabel">
           <tr>
              <td>
                <asp:Label  ID="Label1" runat="server" Text='Enrollment Details'></asp:Label>   
              </td> 
               </tr>
        </table>
             <table class="mGrid" align="left" cellpadding="0" cellspacing="0" width="100%" style="height:auto !important">  
              <tr>
                  <td><asp:Label ID="lblFSPDATA" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblExpiry" runat="server" Text='Expiry Date'  CssClass="FormLabel" ></asp:Label>: <asp:Label ID="lblExpiryData" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblStatus" runat="server" Text='Status' CssClass="FormLabel" ></asp:Label>: <asp:Label ID="lblStatusData" runat="server" Text="" CssClass="DataEntry" Font-Bold="true" ForeColor="Black"></asp:Label></td>
                  <td><asp:Label ID="lblBalance" runat="server" Text='Balance' CssClass="FormLabel" ></asp:Label>: <asp:Label ID="lblBalacneData" runat="server" Text="" CssClass="DataEntry" Font-Bold="true" ForeColor="Black"></asp:Label></td>
              </tr>
          </table>
           
            
   <table  class="catlabel">
           <tr>
              <td><asp:Label  runat="server" Text='Claim Details'></asp:Label></td> 
           </tr>
        </table>
         <asp:Panel ID="Panel1" runat="server"  CssClass="panel" >
        
          <table class="mGrid" align="left" cellpadding="0" cellspacing="0" width="100%">  
              <tr>
                  <td><asp:Label ID="lblCLAIMCode" runat="server" Text='<%$ Resources:Resource,L_CLAIMCODE %>' CssClass="FormLabel"></asp:Label></td>
                  <td><span style="color:black;font-weight:bolder"><asp:Label ID="lblCLAIMData" runat="server" Text="" CssClass="DataEntry"></asp:Label></span></td>
                  <td><asp:Label ID="lblSTARTDATE" runat="server" Text='<%$ Resources:Resource,L_START %>'  CssClass="FormLabel" ></asp:Label></td>
                  <td><asp:Label ID="lblSTARTData" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblCLAIMTOTAL" runat="server" Text='<%$ Resources:Resource,L_CLAIMTOTAL %>' CssClass="FormLabel" ></asp:Label></td>
                  <td><asp:textbox ID="txtCLAIMTOTALData" runat="server" Text="" BorderStyle="Solid" style="text-align:right" Enabled ="false"  ></asp:textbox></td>
              </tr>
              <tr>               
                  <td><asp:Label ID="lblHFCODE" runat="server" Text='<%$ Resources:Resource,L_HF %>' CssClass="FormLabel" > </asp:Label></td>
                  <td><asp:Label ID="lblHFCODEData" runat="server" Text="" CssClass="DataEntry"></asp:Label></td> 
                  <td><asp:Label ID="lblEND" runat="server" Text='<%$ Resources:Resource,L_END %>'  CssClass="FormLabel"></asp:Label></td>
                  <td><asp:Label ID="lblENDData" runat="server" Text="" CssClass="DataEntry"></asp:Label> <span class="FormLabel">(<asp:Label ID="lblTotalDays" runat="server"></asp:Label> Days)</span> </td>
                  <td><asp:Label ID="L_APPROVED" runat="server" Text='<%$ Resources:Resource,L_APPROVED %>' CssClass="FormLabel" ></asp:Label></td>
                  <td><asp:textbox ID="txtApproved" runat="server" Text="" BorderStyle="Solid" style="text-align:right" Enabled ="false"></asp:textbox></td>
              </tr>
              <tr>
                  <td><asp:Label ID="lblCLAIMDATE" runat="server" Text='<%$ Resources:Resource,L_CLAIMDATE %>'  CssClass="FormLabel"></asp:Label></td>
                  <td><asp:Label ID="lblCLAIMDATEData" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblICD" runat="server" Text="<%$ Resources:Resource,L_ICD %>"  CssClass="FormLabel"  ></asp:Label></td>
                  <td><asp:Label ID="lblICDData" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblVALUATED" runat="server" Text='<%$ Resources:Resource,L_ADJUSTED %>' CssClass="FormLabel" ></asp:Label></td>
                  <td><asp:textbox ID="txtPriceVALUATEDData" runat="server" Text="" BorderStyle="Solid" style="text-align:right" Enabled ="false"></asp:textbox>
                   <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="TZS" 
                                CultureDateFormat="" CultureDatePlaceholder="" 
                                CultureThousandsPlaceholder="," CultureTimePlaceholder="" Enabled="True" 
                                Mask="999,999,999.99" MaskType="Number" TargetControlID="txtPriceVALUATEDData" 
                                InputDirection="RightToLeft" PromptCharacter=" ">
                            </ajax:MaskedEditExtender></td>
              </tr>
              <tr>
                  <td><asp:Label ID="lblDATERELEASED" runat="server" Text='<%$ Resources:Resource,L_DATEPROCESSED %>'  CssClass="FormLabel"></asp:Label></td>
                  <td><asp:Label ID="lblDateProcessed" runat="server" Text="" CssClass="DataEntry"></asp:Label></td>             
                  <td><asp:Label ID="lblICD1" runat="server" Text="<%$ Resources:Resource,L_SECONDARYDG1 %>" CssClass="FormLabel" ></asp:Label></td>
                  <td><asp:Label ID="lblICDData1" runat="server" CssClass="DataEntry"></asp:Label><asp:HiddenField ID="hfOldClaimID" runat="server" /></td>
                  <td colspan="2"><span class="FormLabel">Last Visit:</span> <asp:Label ID="lblLastVisit" runat="server" CssClass="DataEntry"></asp:Label> <asp:Label ID="lblLastDays" runat="server" CssClass="DataEntry"></asp:Label> <a id="lnkOldClaim" target="_blank">View Old Claim</a></td>
              </tr>
             <tr>
                  <td><asp:Label ID="lblVisitType" runat="server" Text="<%$ Resources:Resource,L_VISITTYPE %>" CssClass="FormLabel"></asp:Label></td>
                  <td><asp:Label ID="lblVisitTypeData" runat="server"  CssClass="DataEntry"></asp:Label></td>
                  <td><asp:Label ID="lblGuaranteeNo" runat="server" Text="<%$ Resources:Resource,L_GUARANTEE %>" CssClass="FormLabel"></asp:Label></td>
                  <td><asp:Label ID="lblGuaranteeData" runat="server"  CssClass="DataEntry"></asp:Label></td>
                  <td><a id="lnkViewDocumentOld" target="_blank">View Document Old</a></td>
                  <td><a id="lnkViewDocument" target="_blank">View Document New</a></td>
              </tr>         
         
          </table>
           
        </asp:Panel>
        
        <table  class="catlabel">
           <tr>
              <td>
                <asp:Label  ID="lblServiceDetails" runat="server" Text='<%$ Resources:Resource,L_SERVICES %>'></asp:Label>   
              </td>           
               <td  align="right" style="padding-left:10px; vertical-align:bottom">
                   <a href="#" id="RejectServices" style="color:white">Reject All Services</a></td>
               <td>
                   &nbsp;</td>
           </tr>
        </table>
        <asp:Panel ID="pnlServiceDetails" runat="server"  CssClass="panel" Height="140" ScrollBars ="Vertical">
            
            <asp:GridView ID="gvService" runat="server" AutoGenerateColumns="False" 
                            CssClass="mGrid" DataKeyNames="QtyApproved,PriceApproved,Justification,PriceAsked,ClaimServiceID,PriceValuated,ClaimServiceStatus" 
                            EmptyDataText='<%$ Resources:Resource, M_NOSERVICES %>' GridLines="None" 
                            PagerStyle-CssClass="pgr" ShowSelectButton="True">
                            <Columns>
                               <asp:BoundField DataField="servcode"  
                                    HeaderText="<%$ Resources:Resource, L_SERVICECODE %>" SortExpression="servcode" 
                                    ApplyFormatInEditMode="True" >
                               <HeaderStyle Width="125px" />                                
                               </asp:BoundField>
                               
                                <asp:BoundField DataField="QtyProvided" DataFormatString="{0:n2}"  HeaderText="<%$ Resources:Resource, L_QTY %> "   ItemStyle-HorizontalAlign="Right" SortExpression="QtyProvided" >
                                <HeaderStyle Width="25px" />                                
                               </asp:BoundField>
                               
                                <asp:BoundField DataField="PriceAsked"   HeaderText="<%$ Resources:Resource, L_PRICE %>" ItemStyle-HorizontalAlign="Right"  SortExpression="PriceAsked" >
                                <ItemStyle CssClass="PriceAsked" /> <HeaderStyle Width="50px" />                                
                               </asp:BoundField>                               
                               
                                <asp:BoundField DataField="Explanation"  ItemStyle-Width="215px" HeaderText="<%$ Resources:Resource, L_EXPLANATION %> " SortExpression="" >
                               </asp:BoundField>
                            
                                <asp:TemplateField ControlStyle-Width="45px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAppQty" runat="server" Width="100%" Text='<%# Bind("QtyApproved") %>' style="text-align:right" class="appQty numbersOnly"></asp:TextBox>
                                       </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="AppQtyHeader" runat="server"  Text='<%$ Resources:Resource, L_APPQTY %>' ></asp:Label>                                        
                                    </HeaderTemplate>
                                    <%--<ControlStyle Width="40px" />--%>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField ControlStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAPPVALUE" runat="server" Width="100%" Text='<%# Bind("PriceApproved") %>' style="text-align:right" class="appvalue numbersOnly" ></asp:TextBox>
                                        </ItemTemplate>
                                     <HeaderTemplate >
                                      <asp:Label ID="AppValueHeader" runat="server" Text='<%$ Resources:Resource, L_APPVALUE %>' ></asp:Label>
                                    </HeaderTemplate>
                                    <%-- <ControlStyle Width="40px" />--%>
                                </asp:TemplateField>
                                                                                                
                                <asp:TemplateField ControlStyle-Width="230px" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtJUSTIFICATION" runat="server" Width="100%" Text='<%# Bind("Justification") %>' ></asp:TextBox>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                      <asp:Label ID="JustificationHeader" runat="server" width="230px" Text='<%$ Resources:Resource, L_JUSTIFICATION %>'></asp:Label>
                                    </HeaderTemplate>                                    
                                </asp:TemplateField>
                                
                                 <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlSTATUS" runat="server" CssClass="cmb ClmStatus" ></asp:DropDownList>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                      <asp:Label ID="StatusHeader" runat="server" Text='<%$ Resources:Resource, L_STATUS %>'></asp:Label>
                                    </HeaderTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="PriceValuated" DataFormatString="{0:n2}" ItemStyle-Width="55px" HeaderText="<%$ Resources:Resource, L_PRICEVALUATED %>" ItemStyle-HorizontalAlign="Right"  SortExpression="" >
                                <HeaderStyle Width="55px" />
                                <ItemStyle HorizontalAlign="Right" Width="55px" />
                               </asp:BoundField>
                                <asp:BoundField DataField="RejectionReason"  ItemStyle-Width="8px" HeaderText="R" ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle Width="8px" />
                                <ItemStyle HorizontalAlign="Right" Width="8px" />
                               </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <SelectedRowStyle CssClass="srs" />
            </asp:GridView>
              
        </asp:Panel>
        
        <table  class="catlabel">
           <tr>
              <td>
                <asp:Label  ID="lblItems" runat="server" Text='<%$ Resources:Resource,L_ITEMS %>'></asp:Label>   
              </td>           
               <td  align="right" style="padding-left:10px; vertical-align:bottom">
                   <a href="#" id="RejectItems" style="color:white">Reject All Items</a></td>
               <td>
                   &nbsp;</td>
           </tr>
        </table>
        <asp:Panel ID="pnlItemsDetails" runat="server"  CssClass="panel"  Height="140" ScrollBars ="Vertical">
        
           
            <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False"  
                            CssClass="mGrid" DataKeyNames="QtyApproved,PriceApproved,Justification,PriceAsked,ClaimItemID,PriceValuated,ClaimItemStatus" 
                            EmptyDataText='<%$ Resources:Resource, M_NOITEMS %>' GridLines="None" 
                            PagerStyle-CssClass="pgr" ShowSelectButton="True">
                        <Columns>
                               <asp:BoundField DataField="ItemCode" HeaderText="<%$ Resources:Resource, R_ITEMCODE %>" SortExpression="ItemCode" >
                               <HeaderStyle Width="125px" />
                               </asp:BoundField>
                               
                                <asp:BoundField DataField="QtyProvided" DataFormatString="{0:n2}"  HeaderText="<%$ Resources:Resource, L_QTY %> "   ItemStyle-HorizontalAlign="Right" SortExpression="QtyProvided" >
                                <HeaderStyle Width="25px" /> 
                               </asp:BoundField>
                               
                                <asp:BoundField DataField="PriceAsked"   HeaderText="<%$ Resources:Resource, L_PRICE %> "   ItemStyle-HorizontalAlign="Right" SortExpression="PriceAsked" >
                                <ItemStyle CssClass="PriceAsked" /> <HeaderStyle Width="50px" />
                               </asp:BoundField>
                               
                                <asp:BoundField DataField="Explanation"  ItemStyle-Width="215px" HeaderText="<%$ Resources:Resource, L_EXPLANATION %> " SortExpression="Explanation" >
                               </asp:BoundField>
                              
                                <asp:TemplateField ControlStyle-Width="45px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAPPQTY" runat="server" Width="100%" Text='<%# Bind("QtyApproved") %>' style="text-align:right" class="appQty numbersOnly"></asp:TextBox>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                      <asp:Label ID="AppQtyHeaderI" runat="server"  Text='<%$ Resources:Resource, L_APPQTY %>' ></asp:Label>
                                    </HeaderTemplate>
                                    
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText='<%$ Resources:Resource, L_APPVALUE %>' ControlStyle-Width="60px"  >
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAPPVALUE" runat="server" Width="100%" Text='<%# Bind("PriceApproved") %>' style="text-align:right" class="appvalue numbersOnly"></asp:TextBox>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                       <asp:Label ID="AppValueHeaderI" runat="server"  width="50px" Text='<%$ Resources:Resource, L_APPVALUE %>'></asp:Label>
                                    </HeaderTemplate>
                                    
                                </asp:TemplateField>
                                                                                                 
                                <asp:TemplateField ControlStyle-Width="230px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtJUSTIFICATION" runat="server"  Width="100%" Text='<%# Bind("Justification") %>' ></asp:TextBox>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                      <asp:Label ID="JustificationHeaderI" runat="server"  width="230px" Text='<%$ Resources:Resource, L_JUSTIFICATION %>'></asp:Label>
                                    </HeaderTemplate>                                    
                                </asp:TemplateField>
                                
                                 <asp:TemplateField  HeaderText='<%$ Resources:Resource, L_STATUS %>' >
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlSTATUS" runat="server" CssClass="cmb ClmStatus"  ></asp:DropDownList>
                                    </ItemTemplate>
                                     <HeaderTemplate >
                                       <asp:Label ID="StatusHeaderI" runat="server"  Text='<%$ Resources:Resource, L_STATUS %>'></asp:Label>
                                    </HeaderTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="PriceValuated" DataFormatString="{0:n2}" ItemStyle-Width="55px" HeaderText="<%$ Resources:Resource, L_PRICEVALUATED %>" ItemStyle-HorizontalAlign="Right"  SortExpression="" >
                                <HeaderStyle Width="55px" />
                                <ItemStyle HorizontalAlign="Right" Width="55px" />
                               </asp:BoundField>
                               <asp:BoundField DataField="RejectionReason"  ItemStyle-Width="8px" HeaderText="R" ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle Width="8px" />
                                <ItemStyle HorizontalAlign="Right" Width="8px" />
                               </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <SelectedRowStyle CssClass="srs" />
            </asp:GridView>
           
        </asp:Panel>
       
        <table>
           
           <tr>
             <td class="FormLabel">
                   <asp:Label ID="lblEXPLANATION" runat="server" Text='<%$ Resources:Resource,L_EXPLANATION %>' ></asp:Label>
               </td>
               <td class ="DataEntry">
                   <asp:TextBox ID="txtEXPLANATION" runat="server" Width="450px"  Enabled ="false"></asp:TextBox>
               </td>
           
              
               <td class="FormLabel">
                   <asp:Label ID="lblCLAIMSTATUS" runat="server" Text='<%$ Resources:Resource,L_CLAIMSTATUS %>' ></asp:Label>
               </td>
               <td class ="DataEntry">
                   <%--<asp:DropDownList ID="ddlClaimStatus" runat="server"></asp:DropDownList>--%>
                   <asp:TextBox ID="txtClaimStatus" runat="server" enabled="false"></asp:TextBox>
               </td>
                 
           </tr>
           <tr>
            <td class="FormLabel">
                   <asp:Label ID="lblADJUSTMENT" runat="server"  Text='<%$ Resources:Resource,L_ADJUSTMENT %>' ></asp:Label>
               </td>
               <td class ="DataEntry">
                   <asp:TextBox ID="txtADJUSTMENTData" runat="server" Width="450px" Text=""></asp:TextBox>
               </td>
         
               
               <td class="FormLabel">
                   
               </td>
               <td class ="DataEntry">
                </td>
           </tr>
           </table>
          <asp:HiddenField ID="hfApprovedValue" runat="server" />
          <asp:HiddenField ID="hfClaimedValue" runat="server" />
          <asp:HiddenField ID="hfClaimAdminId" runat="server" />
        </div>
         </asp:Panel> 
        <asp:HiddenField ID="hfBatchID" runat="server" />
       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons"  >
        <table width="100%" cellpadding="10 10 10 10" align="center">
             <tr align="center">
                    
                    <td  align="left">
                       <asp:Button 
                        ID="B_SAVE" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_SAVE%>' ValidationGroup="check" />
                         
                    </td>
                    
                    <td  align="center">
                       <asp:Button 
                        ID="B_REVIEWED" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_REVIEWED%>' ValidationGroup="check" />
                         
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
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
</asp:Content>
