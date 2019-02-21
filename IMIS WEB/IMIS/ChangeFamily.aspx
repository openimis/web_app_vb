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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="ChangeFamily.aspx.vb" Inherits="IMIS.ChangeFamily" 
 Title='<%$ Resources:Resource,L_CHANGEFAMILY %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<script type="text/javascript" language="javascript">
    function promptInsureeAdd(btn) {
        if (btn === "ok") {
            $("#<%=hfActivate.ClientID%>").val(1);
           //theForm.__EVENTTARGET.value = $("#<%=B_Move.ClientID%>");
           //theForm.submit();

       } else {
           //var familyId = '<%=HttpContext.Current.Request.QueryString("f") %>';
           //window.location = "OverviewFamily.aspx?f=" + familyId;
           $("#<%=hfActivate.ClientID%>").val(0);
       }
       $("#<%=hfCheckMaxInsureeCount.ClientID %>").val(0);
        $("#<%=B_Move.ClientID%>").click();
    }
    function msgOkay(btn) {
        if (btn == "ok") {
            $("#<%=hfOK.ClientID%>").val(0);
                $("#<%=B_Move.ClientID %>").click();
            }
        }
</script>

    <div class="divBody" style="height:550px;" >
    <asp:HiddenField ID="hfCheckMaxInsureeCount" Value="1" runat="server" />
    <asp:HiddenField ID="hfOK" Value="1" runat="server" />
      <asp:HiddenField ID="hfActivate" Value="0" runat="server" />

   <asp:Panel ID="L_FAMILYPANEL" runat="server"  height="100px" 
             CssClass="panel" >           
               <table >
                    <tr>                     
                         <td class="FormLabel">
                            <asp:Label ID="lblHeadCHFID" runat="server" Text='<%$ Resources:Resource,L_CHFID %>'></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadCHFID" runat="server"  />
                        </td>
                        <td class="FormLabel">
                            <asp:Label ID="lblRegion" runat="server" Text="<%$ Resources:Resource,L_Region %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtRegion" runat="server" />
                        </td>
                        <td class="FormLabel">
                            <asp:Label ID="lblPoverty3" runat="server" Text="<%$ Resources:Resource,L_POVERTY %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPoverty" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel" style="text-align:left; direction: ltr;">
                             <asp:Label ID="L_ADDRESS0" runat="server" Text="<%$ Resources:Resource, L_PARMANENTADDRESS %>"></asp:Label>
                         </td>
                    </tr>
                    
                    <tr>                     
                         <td class="FormLabel">
                            <asp:Label ID="lblHeadLastName" runat="server" Text='<%$ Resources:Resource,L_LASTNAME %>'></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                        <asp:label ID="txtHeadLastName" runat="server"  />
                        </td>
                        <td class="FormLabel">
                            <asp:Label ID="lblDistrict0" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtDistrict" runat="server" />
                        </td>
                          
                         <td class="FormLabel">
                             <asp:Label ID="lblConfirmation" runat="server" Text="<%$ Resources:Resource,L_CONFIRMATIONTYPE %>" style="direction: ltr"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtConfirmationType" runat="server" style="direction: ltr"></asp:Label>
                         </td>
                         <td class="ReadOnlyText" rowspan="3" style="vertical-align:top; direction: ltr;">
                             <asp:Label ID="txtPermanentAddress" runat="server"></asp:Label>
                         </td>
                    </tr>
                    <tr>                     
                        <td class="FormLabel">
                            <asp:Label ID="lblHeadOtherNames" runat="server" Text='<%$ Resources:Resource,L_OTHERNAMES %>'></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadOtherNames" runat="server"  />
                        </td>
                       <td class="FormLabel">
                            <asp:Label ID="lblWard0" runat="server" Text="<%$ Resources:Resource,L_WARD %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtWard" runat="server"></asp:Label>
                        </td>
                        
                         <td class="FormLabel">                          
                             <asp:Label ID="L_CONFIRMATIONNO0" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>" style="direction: ltr"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">                          
                            <asp:Label ID="txtConfirmationNo1" runat="server" style="direction: ltr" />
                        </td>
                    </tr>                   
                    <tr>
                        <td class="FormLabel">&nbsp;</td>
                        <td class="ReadOnlyText">&nbsp;</td>
                        <td class="FormLabel">
                            <asp:Label ID="lblVillage0" runat="server" Text="<%$ Resources:Resource,L_VILLAGE %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtVillage" runat="server"></asp:Label>
                        </td>
                        <td class="FormLabel">&nbsp;</td>
                        <td class="ReadOnlyText">&nbsp;</td>
                    </tr>
                </table> 
         </asp:Panel>
   <table class="catlabel" style="margin-top:15px">
                    <tr>
                        <td >
                            <asp:Label  ID="Label3" runat="server"  Text='<%$ Resources:Resource,L_CHANGEFAMILY %>'></asp:Label>   
                        </td>
                      
                    </tr>
                    </table>
               
         <asp:Panel ID="pnlChangeFamily" runat="server" height="210px" ScrollBars="Auto" style="margin-top:5px"
             CssClass="panel" GroupingText="Change Family/Group Details" 
        EnableTheming="True" >
        <asp:UpdatePanel ID="upDistrict" runat="server"  > 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>                               
        <ContentTemplate> 
            <table class="style15">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_REGION" runat="server" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                        </td>
                        <td class="DataEntry" >
                            <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRegion" runat="server" ControlToValidate="ddlRegion" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                   
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_DISTRICT" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" width="150px">
                                <%--<asp:ListItem Value="0">-- Select a District</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDistrict" runat="server" ControlToValidate="ddlDistrict" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                   
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_WARD" runat="server" Text='<%$ Resources:Resource,L_WARD %>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlWard" runat="server" Width="150px" AutoPostBack="true">
                                <%--<asp:ListItem Value="0">-- Select a Ward --</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorWard" runat="server" 
                                ControlToValidate="ddlWard" Text="*"
                                SetFocusOnError="True" ValidationGroup="check" InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     <tr>
                         <td class="FormLabel">
                            <asp:Label ID="L_VILLAGE" runat="server" Text='<%$ Resources:Resource,L_VILLAGE %>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlVillage" runat="server" Width="150px">
                                <%--<asp:ListItem Value="0">-- Select a Village --</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorVillage" runat="server" 
                                ControlToValidate="ddlVillage" Text="*" 
                                SetFocusOnError="True" ValidationGroup="check" InitialValue="0"></asp:RequiredFieldValidator>
                           
                        </td>
                       
                    </tr>
                    <tr id="trPoverty" runat="server">
                        <td class="FormLabel">
                            <asp:Label ID="lblPoverty" runat="server" Text='<%$ Resources:Resource,L_POVERTY %>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:DropDownList ID="ddlPoverty" runat="server" Width="150px" >
                                <%--<asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td >
                           
                            <asp:RequiredFieldValidator ID="rfPoverty" runat="server" ControlToValidate="ddlPoverty" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                        </tr>
                                   <tr id="tfConfirmationType" runat="server">
                                <td class="FormLabel">
                                    <asp:Label ID="lblPoverty4" runat="server" Text="<%$ Resources:Resource,L_CONFIRMATIONTYPE %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlConfirmationType" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                       <td>
                                           <asp:RequiredFieldValidator ID="rfConfirmationType" runat="server" ControlToValidate="ddlConfirmationType" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                       </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="ddlType" InitialValue="" SetFocusOnError="True" Text="*" 
                                        ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                <tr id="trConfirmationNo" runat="server">
                                <td class="FormLabel">
                                    <asp:Label ID="lblConfirmationNo" runat="server" style="direction: ltr" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtConfirmationNo" runat="server" AutoPostBack="false" MaxLength="12" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfConfirmationNo" runat="server" ControlToValidate="txtConfirmationNo" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                </td>
                    </tr>
                   
                            <tr id="trType" runat="server">
                                <td class="FormLabel">
                                    <asp:Label ID="L_TYPE" runat="server" Text="<%$ Resources:Resource, L_GROUPTYPE %>" ></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlType" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="ddlType" InitialValue="" SetFocusOnError="True" Text="*" 
                                        ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                                    <asp:RequiredFieldValidator ID="rfType" runat="server" ControlToValidate="ddlType" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                </td>
                    </tr>
                   
                            <tr style="display:none;">
                                <td class="FormLabel">
                                    <asp:Label ID="lblEthnicity" runat="server" Text="<%$ Resources:Resource, L_ETHNICITY %>" Visible="False"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlEthnicity" runat="server" Width="150px" Visible="False">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                    </tr>
                   
                            
                   
                            <tr id="trAddress" runat="server">
                                <td class="FormLabel">
                                    <asp:Label ID="L_ADDRESS" runat="server" 
                                        Text="<%$ Resources:Resource, L_PARMANENTADDRESS %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAddress" runat="server" Height="40px" MaxLength="25" 
                                        TextMode="MultiLine" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfAddress" runat="server" ControlToValidate="txtAddress" InitialValue="" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                </td>
                                
                            </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td align="right">
                                    <asp:Button ID="btnSave" runat="server" Text='<%$ Resources:Resource,B_SAVE %>' ValidationGroup="check" 
                                 /></td>
                </tr>
                </table> 
            </ContentTemplate>      
     </asp:UpdatePanel>                
                    
                    
         </asp:Panel>       
          <table class="catlabel" style="margin-top:15px">
                    <tr>
                        <td >
                            <asp:Label  ID="Label1" runat="server"  Text='<%$ Resources:Resource,L_CHANGEHEAD %>'></asp:Label>   
                        </td>                      
                    </tr>
                    </table>
         <asp:Panel ID="pnlChangeHeadOfFamily" runat="server" 
        ScrollBars="Auto" CssClass="panel" GroupingText='<%$ Resources:Resource, L_CHANGEHEAD %>' style="margin-top:5px">
                    <table class="style15">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_CHFID" runat="server" Text='<%$ Resources:Resource,L_ENTERNEWHEADCHFID %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtCHFIDToChange" runat="server"  Width="150px"  
                                MaxLength = "12" AutoPostBack="false"></asp:TextBox>
                            
                        </td>
                        
                        <td >
                          <asp:Button ID="B_CHECK" runat="server"  Text='<%$ Resources:Resource,B_CHECK %>' width="60px" 
                                 />
                                 </td>
                                 
                                 <td class="ReadOnlyText">
                        <asp:Label ID="lblCHFIDToChange" runat="server"></asp:Label> 
                        </td>
                                 
                                 
                                 <td align="right">
                                  <asp:RequiredFieldValidator 
                        ID="RequiredFieldCHFID" runat="server" 
                        ControlToValidate="txtCHFIDToChange" 
                        SetFocusOnError="True" 
                        ValidationGroup="check2"
                        Text="*"></asp:RequiredFieldValidator>
                        <asp:Button ID="B_Change" runat="server" enabled = "False" Text='<%$ Resources:Resource,B_CHANGE %>' 
                                ValidationGroup="check2" />
                       
                        </td>
                       
                    </tr>
                   
                </table>            
         </asp:Panel> 
         <table class="catlabel" style="margin-top:15px">
                    <tr>
                        <td >
                            <asp:Label  ID="Label2" runat="server"  Text='<%$ Resources:Resource,L_MOVEINSUREE %>'></asp:Label>   
                        </td>
                      
                    </tr>
                    </table>
         <asp:Panel ID="pnlMoveInsuree" runat="server" 
        ScrollBars="Auto" CssClass="panel" GroupingText="Move Insuree to Family/Group" style="margin-top:5px">
                    <table class="style15">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_CHFID2" runat="server" Text='<%$ Resources:Resource,L_ENTERCHFIDOFINSUREE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtCHFIDToMove" runat="server"  MaxLength = "12" 
                                Width="130px" ></asp:TextBox>
                        </td>
                        <td>
                         <asp:Button ID="B_CHECKMOVE" runat="server"  Text='<%$ Resources:Resource,B_CHECK %>' width="60px" 
                                 />
                                 
                                </td>
                                 
                                 <td class="ReadOnlyText">
                        <asp:Label ID="lblCHFIDToMove" runat="server"></asp:Label> 
                        </td>
                        <td align="right">
                         <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtCHFIDToMove" 
                        SetFocusOnError="True" 
                        ValidationGroup="check3"
                        Text="*"></asp:RequiredFieldValidator>
                        <asp:Button ID="B_Move" runat="server" Text='<%$ Resources:Resource,B_MOVE %>' ValidationGroup="check3"  Enabled="false" />
                       
                        </td>
                        
                    </tr>
                   
                </table>            
         </asp:Panel> 
         </div>
          <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                          <%--     <asp:Button 
                            
                            ID="B_SAVE" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_SAVE%>'
                            ValidationGroup="check1"  />--%>
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
            <asp:HiddenField ID="hfFamilyIDValue" runat="server" /> 
             <asp:HiddenField ID="hfInsureeIDValue" runat="server" />  
              <asp:HiddenField ID="hfInsureeIsOffline" runat="server" Value="" />
               <asp:HiddenField ID="hfFamilyIsOffline" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            font-family: Arial, Helvetica, sans-serif; /*min-width: 170px;*/;
            height: 27px;
            direction: ltr;
            width: 504px;
        }
        .auto-style2 {
            width: 504px;
        }
    </style>
</asp:Content>

