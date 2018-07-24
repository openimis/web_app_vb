<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="IMISExtracts.aspx.vb" Inherits="IMIS.IMISExtracts"
    Title='<%$ Resources:Resource,L_EXTRACTS%>' Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

    </script>
    <style type="text/css">
        table.tblPopupMsg
        {
            margin: auto;
            width: 350px;
        }

            table.tblPopupMsg td
            {
                padding: 2px 10px;
            }

                table.tblPopupMsg td.no
                {
                    text-align: right;
                }

                table.tblPopupMsg td.str
                {
                    text-align: left;
                }

            table.tblPopupMsg th
            {
                border-bottom: 1px solid #000000;
            }

        .auto-style1
        {
            height: 33px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ("<%=IMIS_Gen.OfflineCHF %>" == "True") {
            $("#<%=btnUploadExtract.ClientID %>").click(function () {
                 popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                 popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True)%>';
                 popup.confirm('<%=imisgen.getMessage("M_UPLOADENROLALERT", True ) %>', UploadCancelled);
                 return false;
             });
         }
        $("#<%=btnOffLineExtract.ClientID %>").click(function () {
            popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
            popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True)%>';
            if ($("#<%=ddlRegionOffLine.ClientID %>").val() == 0) {
                popup.confirmTitle = "!Alert"
                popup.confirm('<%=imisgen.getMessage("M_CREATEOFFLINEEXTRACTPROMPT", True ) %>', OfflineExtractPrompted);
                return false;
            }
        });
    });

    function UploadCancelled(btn, Args) {
        if (btn == "ok") {
            theForm.__EVENTTARGET.value = "<%=btnUploadExtract.ClientID %>";
            theForm.submit();
        }
    }
    function OfflineExtractPrompted(btn, Args) {
        if (btn == "ok") {
            theForm.__EVENTTARGET.value = "<%=btnOffLineExtract.ClientID %>";
            theForm.submit();
        }
    }
    </script>

    <div class="divBody">
        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div id="SelectPic"></div>
                <div id="Progress">
                    <div style="width:30px; float:left;"><img height="30px" width="30px" src="Images/progress.gif" /></div>
                    Please wait...
                </div>
            </ProgressTemplate>
       </asp:UpdateProgress>--%>
        <%--  <ContentTemplate>
        <div id="DivMsg" runat="server" style="display:none;"></div>--%>
        <asp:HiddenField ID="hfExtractFound" runat="server" Value="0" />
        <asp:Panel ID="pnlOnline" runat="server">
            <asp:Panel ID="pnlOnlineExtracts" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="LBLPhoneExtract" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_PHONEEXTRACT %>'></asp:Label>

                        </td>


                    </tr>
                </table>
                <asp:Panel ID="pnlPhoneExtract" runat="server" CssClass="panel" GroupingText=""
                    Height="60px" Width="976px">
                    <table width="100%">
                        <tr>
                            <td class="auto-style1">
                                <table>
                                    <tr>
                                        <td class="FormLabel ExtractTd">

                                            <asp:Label ID="LBLREGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>" Width="50px"></asp:Label>

                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel runat="server" ID="UPRegionPhone">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlRegionPhone" runat="server" class="Month" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldRegion" runat="server"
                                                ControlToValidate="ddlRegionPhone" Text="*" ErrorMessage='<%$ Resources:Resource,M_PLEASESELECTAREGION%>'
                                                ValidationGroup="checkPhone" InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel ExtractTd">
                                            <asp:Label ID="LBLDISTRICT" runat="server" Text='<%$ Resources:Resource,L_DISTRICT%>' Width="50px"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel runat="server" ID="UpDistrictPhone">
                                                <ContentTemplate>
                                                    <asp:DropDownList class="Month" ID="ddlDistrictsPhone"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldLastName" runat="server"
                                                ControlToValidate="ddlDistrictsPhone" Text="*" ErrorMessage='<%$ Resources:Resource,V_DISTRICT%>'
                                                ValidationGroup="checkPhone" InitialValue="0"></asp:RequiredFieldValidator>

                                        </td>

                                        <td class="DataEntry" style="width: 40px !important">
                                            <asp:LinkButton ID="PhoneExtractLink" ValidationGroup="checkPhone"
                                                NavigateUrl="" runat="server" Visible="false">DOWNLOAD</asp:LinkButton>
                                        </td>
                                        <%--<td class ="DataEntry">  </td> --%>

                                        <td class="FormLabel">
                                            <asp:CheckBox ID="chkWithInsuree" runat="server" Checked="True" Style="direction: ltr" Text="<%$ Resources:Resource, L_WITHINSUREE %>" />
                                        </td>
                                        <td class="FormLabel">
                                            <asp:CheckBox ID="chkInBackground" runat="server" Text="<%$ Resources:Resource, L_INBACKGROUND %>" />
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnPhoneExtract" runat="server" ValidationGroup="checkPhone"
                                                Text='<%$ Resources:Resource,B_CREATE %>' />
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="LblOfflineExtract" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_OFFLINEEXTRACT %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlOffLineExtract" runat="server" CssClass="panel"
                    Height="60px" GroupingText="" Width="976px">
                    <table width="100%">
                        <tr>
                            <td class="auto-style1">
                                <table>

                                    <tr>
                                        <td class="FormLabel ExtractTd">

                                            <asp:Label ID="L_REGION2" runat="server" Text="<%$ Resources:Resource,L_REGION %>" Width="50px"></asp:Label>

                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel runat="server" ID="UpRegionOffline">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlRegionOffLine" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel ExtractTd">
                                            <asp:Label ID="LBLDISTRICT2" runat="server"
                                                Text='<%$ Resources:Resource,L_DISTRICT %>' Width="50px"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel runat="server" ID="UpDistrictOffline">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDistrictsOffLine" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="ddlDistrictsOffLine" Text="*" ErrorMessage ='<%$ Resources:Resource,V_DISTRICT%>'
                                    ValidationGroup="checkOffline" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                        </td>

                                        <td class="DataEntry">
                                            <asp:UpdatePanel runat="server" ID="UpExtracts">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlExtracts" runat="server">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>



                                        <td class="FormLabel">
                                            <asp:CheckBox ID="chkWithInsureeExport" runat="server" Checked="false" Style="direction: ltr" Text="<%$ Resources:Resource, L_WITHINSUREE %>" />  
                                        </td>

                                        <td class="FormLabel">
                                         <asp:CheckBox ID="chkInBackgroundExport" runat="server" Text="<%$ Resources:Resource, L_INBACKGROUND %>" />   
                                        </td>

                                        <td class="FormLabel">
                                            <asp:CheckBox ID="chkFULL" runat="server" Text='<%$ Resources:Resource,L_FULLEXTRACT %>' />
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnOffLineExtract" runat="server" ValidationGroup="checkOffline"
                                                Text='<%$ Resources:Resource,B_CREATE %>' />



                                        </td>
                                    </tr>

                                    <tr>

                                        <td class="FormLabel ExtractTd">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>

                                        <td class="ExtractTd">&nbsp;</td>
                                        <td class="DataEntry" width="10">
                                            <asp:LinkButton ID="OffLinePhotoLinkD" runat="server" NavigateUrl=""
                                                ValidationGroup="checkOffline">DOWNLOAD PHOTOS [D]</asp:LinkButton>
                                        </td>

                                        <td class="GridHeader">
                                            <asp:LinkButton ID="OffLinePhotoLinkF" runat="server" NavigateUrl=""
                                                ValidationGroup="checkOffline">DOWNLOAD PHOTOS [F]</asp:LinkButton>
                                        </td>

                                        <td align="right">
                                             <asp:LinkButton ID="OffLineExtractLinkD" runat="server" NavigateUrl=""
                                                ValidationGroup="checkOffline">DOWNLOAD [D]</asp:LinkButton>
                                         
                                        </td>
                                        <td align="right">
                                            <asp:LinkButton ID="OffLineExtractLinkE" runat="server" CssClass="DataEntry" NavigateUrl=""  ValidationGroup="checkOffline">DOWNLOAD [E]</asp:LinkButton>
                                        </td>
                                        <td align="right">
                                            <asp:LinkButton ID="OffLineExtractLinkF" runat="server" CssClass="DataEntry" NavigateUrl="" ValidationGroup="checkOffline">DOWNLOAD [F]</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlOnlineClaims" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_UPLOADCLAIMS %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlUploadClaims" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <table>

                                    <tr>

                                        <td class="FormLabel">
                                            <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                                        <td class="DataEntry">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="FileUpload1" ErrorMessage="*" ValidationGroup="uClaims">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnUpload" runat="server"
                                                Text='<%$ Resources:Resource,B_UPLOAD %>' ValidationGroup="uClaims" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlUploadEnrolments" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_UPLOADENROLMENT %>'></asp:Label>

                        </td>


                    </tr>
                </table>
                <asp:Panel ID="pnlUploadEnrolmentXML" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <table>

                                    <tr>

                                        <td class="FormLabel">
                                            <asp:FileUpload ID="fuEnrolments" runat="server" /></td>
                                        <td class="DataEntry">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="fuEnrolments" ErrorMessage="*" ValidationGroup="uEnrolments">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnUploadEnrolments" runat="server"
                                                Text='<%$ Resources:Resource,B_UPLOAD %>' ValidationGroup="uEnrolments" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnlOffline" runat="server">
            <asp:Panel ID="pnlOfflineExtracts" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="LblImportExtract" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_IMPORTEXTRACT %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlImportExtract" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <table>

                                    <tr>

                                        <td class="FormLabel">
                                            <asp:FileUpload ID="txtFileUpload" runat="server" Width="212px" />
                                        </td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnUploadExtract" runat="server"
                                                Text='<%$ Resources:Resource,B_UPLOAD %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="LblImportPhotos" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_IMPORTPHOTOS %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PnlImportPhotos" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <table>

                                    <tr>

                                        <td class="FormLabel">
                                            <asp:FileUpload ID="txtFileUploadPhotos" runat="server" Width="212px" />
                                        </td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnUploadPhotos" runat="server"
                                                Text='<%$ Resources:Resource,B_UPLOAD %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlOfflineClaims" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_DOWNLOADCLAIMS %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlDownloadClaims" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <table>

                                    <tr>

                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnDownloadClaim" runat="server"
                                                Text='<%$ Resources:Resource,B_DOWNLOAD %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>

            <asp:Panel ID="pnlExtractEntrolment" runat="server">
                <table class="catlabel">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server"
                                Text='<%$ Resources:Resource,L_EXTR_OFFLINENROLMENT %>'></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlDownloadEntrolment" runat="server" CssClass="panel"
                    Height="60px" GroupingText="">
                    <table>
                        <tr>
                            <td>
                                <iframe id="iFrame" runat="server" src="" width="0px" height="0px"></iframe>
                                <table>
                                    <tr>

                                        <td class="FormLabel"></td>

                                        <td class="DataEntry">&nbsp;</td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td class="FormLabel"></td>
                                        <td class="DataEntry"></td>
                                        <td align="right">
                                            <asp:Button ID="btnDownloadEnrolment" runat="server"
                                                Text='<%$ Resources:Resource,B_DOWNLOAD %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <%--</ContentTemplate>
           
           <Triggers>
            <asp:PostBackTrigger ControlID="PhoneExtractLink"  />
           </Triggers>
        --%>
        <%--    </asp:UpdatePanel>--%>
        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div id="SelectPic"></div>
                <div id="Progress">
                    <div style="width:30px; float:left;"><img height="30px" width="30px" src="Images/progress.gif" /></div>
                    Please wait...
                </div>
            </ProgressTemplate>
       </asp:UpdateProgress>--%>
    </div>
    <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <asp:HiddenField ID="hfExtract" runat="server" Value="No message" />
        <table width="100%" cellpadding="10 10 10 10" align="center">
            <tr align="center">

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
    <asp:Label ID="lblmsg" runat="server"></asp:Label>
    <asp:ValidationSummary ID="validationSummary1" runat="server" ValidationGroup="checkPhone" />
    <asp:ValidationSummary ID="validationSummary2" runat="server" ValidationGroup="checkOffline" />
</asp:Content>
