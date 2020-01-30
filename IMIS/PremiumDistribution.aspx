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
<%@ Page Title='<%$ Resources:Resource,R_PREMIUMDISTRIBUTION %>'  Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="PremiumDistribution.aspx.vb" Inherits="IMIS.PremiumDistribution" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
<div class="divBody" >  
   <asp:Panel ID="pnlBody" runat="server"  ScrollBars="Auto" CssClass="panelBody" GroupingText='<%$ Resources:Resource,L_SELECTCRITERIA %>' >
                    <table>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
           
                            &nbsp;</td>
                        
                        <td>
                            &nbsp;</td>
                
                    </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td width="300px">
                                &nbsp;</td>
                        </tr>
                        
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_Distict" runat="server" Text='<%$ Resources:Resource,L_DISTRICT %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                     <tr>
                        <td class="FormLabel">
                            <asp:Label ID="lblProduct" runat="server" 
                                Text='<%$ Resources:Resource, L_PRODUCT %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlProduct" runat="server">
                            </asp:DropDownList>

                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                   
                    <tr>
                        <td class="FormLabel">
                        <asp:Label 
                        ID="L_Month"
                        runat="server" 
                        Text='<%$ Resources:Resource, L_MONTH %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlMonth" runat="server">
                            </asp:DropDownList>


                          
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredMonth" runat="server" 
                                ControlToValidate="ddlMonth" SetFocusOnError="True" 
                                Text="Please select the month." ValidationGroup="check" 
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_Year" runat="server" Text='<%$ Resources:Resource, L_YEAR %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlYear" runat="server">
                            </asp:DropDownList>
                           
                           
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldYear" runat="server" 
                                ControlToValidate="ddlYear" SetFocusOnError="True" 
                                Text="Please select the year." ValidationGroup="check"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    
                        
                    
                </table>            
         </asp:Panel> 
     </div> 
  <asp:Panel ID="pnlButtons" runat="server"  CssClass="panel" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                               <asp:Button ID="btnPreview" runat="server" 
                                   Text="<%$ Resources:Resource,B_PREVIEW %>" ValidationGroup="check" />
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:label id="lblMsg" runat="server"></asp:label>
</asp:Content>
