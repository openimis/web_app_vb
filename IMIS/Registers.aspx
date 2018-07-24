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

<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Registers.aspx.vb" Inherits="IMIS.UploadICD" Title='<%$ Resources:Resource,T_UPLOADDOWNLOADREGISTERS %>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contenthead" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $("#<%=btnUpload.ClientID %>").click(function () {
                htmlMsgUpload = '<%= imisgen.getMessage("M_AREYOUSUREUPLOADICDLIST", True ) %>';

                popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
            popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';
        popup.confirm(htmlMsgUpload, UploadIDCFn);
        return false;
            });

            $("#<%=btnUploadLocations.ClientID %>").click(function () {
                htmlMsgUpload = '<%= imisgen.getMessage("M_AREYOUSUREUPLOADLOCATIONS", True ) %>';
            popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
            popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';
            popup.confirm(htmlMsgUpload, UploadLocations);
            return false;
            });

             $("#<%=btnUploadHF.ClientID %>").click(function () {
                htmlMsgUpload = '<%= imisgen.getMessage("M_AREYOUSUREUPLOADHF", True ) %>';
            popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';
                popup.confirm(htmlMsgUpload, UploadHF);
                return false;
         });

        });

        

        function UploadIDCFn(btn) {
            if (btn == "ok") {
                __doPostBack("<%=btnUpload.ClientID%>", "");
            } else if (btn == "cancel") {
                return false;
            }
        }

        function UploadLocations(btn) {
            if (btn == "ok") {
                __doPostBack("<%=btnUploadLocations.ClientID %>", "");
            } else if (btn == "cancel") {
                return false;
            }
        }

        function UploadHF(btn) {
            if (btn == "ok") {
                __doPostBack("<%=btnUploadHF.ClientID %>", "");
             } else if (btn == "cancel") {
                 return false;
             }
         }

    </script>
    <style type="text/css">
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
        .auto-style3 {
            font-family: Arial, Helvetica, sans-serif; /*min-width: 170px;*/;
            height: 27px;
            direction: ltr;
            width: 51px;
        }
        .auto-style4 {
            height: 23px;
            width: 51px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }

        
        </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <div class="divBody">
    <asp:Panel ID="pnlUploads" runat="server" CssClass="panel">
        <asp:Panel ID="PnlUploadLog" runat="server" >
            
            <asp:Panel ID="PnlUploadLogDetails" runat="server"  Height="80px" GroupingText="">
                            <table class="mGrid">
                                <tr>
                                    <th class="DataEntry"><asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, L_LASTUPLOADEDFILENAME %>"  ></asp:Label></th>
                                    <th class="DataEntry"><asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, L_REGISTER %>"></asp:Label></th>
                                    <th class="DataEntry"><asp:Label ID="Label9" runat="server" Text='<%$ Resources:Resource,L_ICDUPLOADSTRATEGY %>' ></asp:Label></th>
                                    <th class="DataEntry"><asp:Label ID="Label10" runat="server" Text='<%$ Resources:Resource,L_DATE %>' ></asp:Label></th>
                                    <th class="DataEntry"></th>
                                </tr>
                                <tr>
                                    <td class="alt"><asp:Label ID="LblFileName" runat="server" Text="" Height="20px" ></asp:Label></td>
                                    <td class="alt"><asp:Label ID="LblRegister" runat="server" Text="" Height="20px"></asp:Label></td>
                                    <td class="alt"><asp:Label ID="LblStrategy" runat="server" Text='' Height="20px" ></asp:Label></td>
                                    <td class="alt"><asp:Label ID="LblDate" runat="server" Text='' Height="20px" ></asp:Label></td>
                                    <td class="alt"><asp:button ID="btnDownloadLog" runat="server" Text='<%$ Resources:Resource,B_DOWNLOADLOG %>' Height="20px" Visible="false" style="border:none; box-shadow:none;background:none;color:green;cursor:pointer;width:auto" />
                                        <asp:HiddenField ID="hfDownloadLog" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        
            </asp:Panel>
        </asp:Panel>

        <asp:Panel ID="pnlUploadDiagnoses" runat="server">
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server"
                            Text='<%$ Resources:Resource,L_UPLOADICD %>'></asp:Label>

                    </td>


                </tr>
            </table>
            <asp:Panel ID="pnlUploadDiagnosesFile" runat="server" CssClass="panel"  Height="60px" GroupingText="">
                
                            <table>

                                <tr>

                                    <td class="FormLabel ExtractTd">
                                        <asp:FileUpload ID="FileUploadDiagnosis" runat="server" />
                                    </td>
                                    <td class="DataEntry"></td>
                                    <td class="FormLabel ExtractTd"><asp:Label ID="lblSelectStrategy" runat="server" Text='<%$ Resources:Resource,L_ICDUPLOADSTRATEGY %>' Width="50px"></asp:Label></td>
                                    <td class="DataEntry"></td>
                                    <td class="DataEntry"> <asp:DropDownList ID="ddlUploadStrategy" runat="server" ></asp:DropDownList></td>
                                    <td class="DataEntry"></td>
                                    <td class="DataEntry">                                        
                                        &nbsp;<asp:CheckBox ID="chkDryRun" runat="server" Text="<%$ Resources:Resource, L_DRYRUN %>" CssClass="checkbox" />
                                    </td>
                                    <td class="DataEntry"></td>
                                    <td align="right" class="auto-style2">
                                        <asp:Button ID="btnUpload" runat="server"
                                            Text='<%$ Resources:Resource,B_UPLOAD%>' ValidationGroup="Upload" />
                                    </td>
                                   
                                </tr>
                                <tr>
                                    
                                    <td class="ExtractTd">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="FileUploadDiagnosis" ErrorMessage="<%$ Resources:Resource, M_PLEASESELECTTHEFILE %>"
                                            SetFocusOnError="True" Text='' ValidationGroup="Upload" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="Select only .isf File" ControlToValidate="FileUploadDiagnosis" Text='<%$ Resources:Resource, M_SELECTONLYXMLFILE %>'
                                            SetFocusOnError="true" ValidationGroup="Upload"
                                            ValidationExpression="^.+\.([Xx][Mm][Ll])$" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="ExtractTd" colspan="9">
                                         <asp:RequiredFieldValidator ID="requiredStrategyID" runat="server" 
                                    ControlToValidate="ddlUploadStrategy" Text="" ErrorMessage ='<%$ Resources:Resource, M_SELECTSTRATEGY %>' Display="Dynamic"
                                    ValidationGroup="Upload" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                     
                                </tr>
                            </table>
                        
            </asp:Panel>

        </asp:Panel>

        <asp:Panel ID="pnlUploadLocations" runat="server">
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server"
                            Text='<%$ Resources:Resource,L_UPLOADLOCATIONS %>'></asp:Label>

                    </td>


                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" CssClass="panel"    Height="60px" GroupingText="">
                
                            <table>

                                <tr>

                                    <td class="FormLabel ExtractTd">
                                        <asp:FileUpload ID="FileUploadLocations" runat="server" />
                                    </td>
                                    <td class="DataEntry"></td>
                                    <td class="FormLabel ExtractTd"><asp:Label ID="lblUploadStrategyLocation" runat="server" Text='<%$ Resources:Resource,L_ICDUPLOADSTRATEGY %>' Width="50px"></asp:Label></td>
                                     <td class="DataEntry"></td>
                                    <td class="DataEntry"> <asp:DropDownList ID="ddlUploadStrategyLocation" runat="server" ></asp:DropDownList></td>
                                    <td class="DataEntry"></td>
                                    <td class="DataEntry">                                        
                                        &nbsp;<asp:CheckBox ID="chkDryRunLocations" runat="server" Text="<%$ Resources:Resource, L_DRYRUN %>" CssClass="checkbox" />
                                    </td>
                                    <td class="DataEntry"></td>
                                    <td align="right" class="auto-style2">
                                        <asp:Button ID="btnUploadLocations" runat="server"
                                            Text='<%$ Resources:Resource,B_UPLOAD%>' ValidationGroup="UploadLocations"  />
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td class="ExtractTd">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="FileUploadLocations" ErrorMessage="M_PLEASESELECTTHEFILE"
                                            SetFocusOnError="True" Text='Please select the file' ValidationGroup="UploadLocations" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="Select only .isf File" ControlToValidate="FileUploadLocations" Text='<%$ Resources:Resource, M_SELECTONLYXMLFILE %>'
                                            SetFocusOnError="true" ValidationGroup="UploadLocations"
                                            ValidationExpression="^.+\.([Xx][Mm][Ll])$" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </td>
                                   <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="ddlUploadStrategyLocation" ErrorMessage="<%$ Resources:Resource, M_SELECTSTRATEGY %>" ValidationGroup="UploadLocations"
                                            SetFocusOnError="True" Display="Dynamic" InitialValue="0" ForeColor="Red" ></asp:RequiredFieldValidator>
                                    </td>
                                     
                                </tr>
                            </table>
                        
            </asp:Panel>

        </asp:Panel>

        <asp:Panel ID="PnlUploadHF" runat="server">
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server"
                            Text='<%$ Resources:Resource,L_UPLOADHEALTHFACILITIES %>'></asp:Label>

                    </td>


                </tr>
            </table>
            <asp:Panel ID="pnlHealthFacilities" runat="server" CssClass="panel"  Height="60px" GroupingText="">
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="FormLabel ExtractTd">
                                        <asp:FileUpload ID="FileUploadHF" runat="server" />
                                    </td>
                                     <td class="DataEntry"></td>
                                    <td class="FormLabel ExtractTd"><asp:Label ID="Label7" runat="server" Text='<%$ Resources:Resource,L_ICDUPLOADSTRATEGY %>' Width="50px"></asp:Label></td>
                                    <td class="DataEntry"></td>
                                    <td class="DataEntry"> <asp:DropDownList ID="ddlUploadStrategyHF" runat="server"  ></asp:DropDownList></td>
                                    <td class="DataEntry"></td>
                                    <td class="DataEntry">                                        
                                        &nbsp;<asp:CheckBox ID="chkDryRunHF" runat="server" Text="<%$ Resources:Resource, L_DRYRUN %>" CssClass="checkbox" />
                                    </td>
                                    <td class="DataEntry"></td>
                                    <td align="right" class="auto-style2">
                                        <asp:Button ID="btnUploadHF" runat="server"
                                            Text='<%$ Resources:Resource,B_UPLOAD%>' ValidationGroup="UploadHF"    />
                                    </td>
                                </tr>
                                <tr>

                                    
                                    <td class="ExtractTd">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="FileUploadHF" ErrorMessage="M_PLEASESELECTTHEFILE"
                                            SetFocusOnError="True" Text='<%$ Resources:Resource, M_PLEASESELECTTHEFILE %>' ValidationGroup="UploadHF" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="Select only .xml File" ControlToValidate="FileUploadHF" Text='<%$ Resources:Resource, M_SELECTONLYXMLFILE %>'
                                            SetFocusOnError="true" ValidationGroup="UploadHF"
                                            ValidationExpression="^.+\.([Xx][Mm][Ll])$" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="DataEntry">&nbsp;</td>
                                    <td class="FormLabel">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="ddlUploadStrategyHF" ErrorMessage="<%$ Resources:Resource, M_SELECTSTRATEGY %>" ValidationGroup="UploadHF"
                                            SetFocusOnError="True" Display="Dynamic" InitialValue="0" ForeColor="Red" ></asp:RequiredFieldValidator>
                                    </td>
                                  



                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlDownloadDiagnosis" runat="server" CssClass="panel">
        <asp:Panel ID="pnlDownLoadICD" runat="server">
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server"
                            Text='<%$ Resources:Resource,L_DOWNLOADICD %>'></asp:Label>

                    </td>


                </tr>
            </table>
           
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnDownLoadICD" runat="server" Text='<%$ Resources:Resource,B_DOWNLOAD%>' OnClick="btnDownLoadICD_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </asp:Panel>
         <asp:Panel ID="pnlDownLoadLocations" runat="server" CssClass="panel">
        <asp:Panel ID="Panel3" runat="server">
             
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server"
                            Text='<%$ Resources:Resource,L_DOWNLOADLOCATIONS %>'></asp:Label>

                    </td>


                </tr>
            </table>
           
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnDownLoadLocation" runat="server" Text='<%$ Resources:Resource,B_DOWNLOAD%>'  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
  </asp:Panel>

       

        <asp:Panel ID="pnlDownLoadHF" runat="server" CssClass="panel">
           <asp:Panel ID="Panel4" runat="server">
            <table class="catlabel">
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server"
                            Text='<%$ Resources:Resource,L_DOWNLOADHEALTHFACILITIES %>'></asp:Label>

                    </td>
                   
                </tr>
            </table>
             <table>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnDownloadHF" runat="server" Text='<%$ Resources:Resource,B_DOWNLOAD%>' />
                    </td>
                </tr>
            </table>

        </asp:Panel>
    </asp:Panel>


  </div>

    <asp:Panel ID="Panel1" runat="server" CssClass="panelbuttons">
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

<asp:Content ID="footer" ContentPlaceHolderID="Footer" runat="server">
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
