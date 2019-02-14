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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="IMIS.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login</title>
      <noscript>
  <META http-equiv="refresh" content="1;url=nojs.htm" />
  </noscript>
    <link href="StyleSheets/Imis.css" rel="stylesheet" type="text/css" />
    <script src="Javascripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Javascripts/Exact.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function PreventBack(){window.history.forward();}
        setTimeout("PreventBack();",0);
        window.onunload = function() { null };
        var offlineHFMsg

        if ('<%= imisgen.offlineHF %>' == 'True') {
            offlineHFMsg = '<%= imisgen.getMessage("M_OFFLINEHFID", True)%>';
        } else {
            offlineHFMsg = '<%= imisgen.getMessage("M_OFFLINECHFID", True ) %>';
        }

        function SendOfflineHFID(popupBtnSource, evArgs) {
            if (popupBtnSource == "ok") {
                theForm.__EVENTARGUMENT.value = "SaveOfflineHFID";
                var objPopupFieldValues = evArgs[0]; //alert(objPopupFieldValues.txtOfflineHF);

                theForm.submit();
            } else if (popupBtnSource == "cancel")
                $("form").fadeIn();

        }

        $(document).ready(function() {
            var offlineHFFlag = parseInt($("#<%=hfOfflineHFIDFlag.ClientID %>").val());
            var args = new Array();
            var popupHTML = '<span style="color:#555;">' + offlineHFMsg + '</span>';

            if (offlineHFFlag == 1) {

                args.push({ txtOfflineHF: "" });

                //                popupHTML += '<br/><table align="left" style="width:100%;position:relative;margin-top:10px;" >';
                //                popupHTML += '<tr><td>';
                //                popupHTML += '<span style="color:#70A5DA;"><%= imisgen.getMessage("L_OFFLINEHFID", True)%></span>';
                //                popupHTML += '</td><td>';
                //                popupHTML += '<input type="text" name="txtOfflineHF" class="numbersOnly" style="border:1px solid #70A5DA;padding:3px;" />';
                //                popupHTML += '</td></tr></table>';


                popup.acceptBTN_Text = '<%= imisgen.getMessage("L_OK", True)%>'; //"SAVE";
                popup.rejectBTN_Text = '<%= imisgen.getMessage("L_CANCEL", True ) %>'; //"CANCEL";

                $("form").fadeOut();
                popup.promptTxtboxName = "txtOfflineHF";
                popup.prompt(popupHTML, SendOfflineHFID, args, true)
                $("#promptTxtbox").addClass("numbersOnly");
                bindAlphaNumber();
            }



        });



        (function closeChildWindowOnLogout() {
            if (window.opener != null || window.opener != undefined) {
                var pageUrl = window.location;
                if ((/Default.aspx/i).test(pageUrl)) {
                    window.close();
                    if (!(/Default.aspx/i).test(window.opener.location))
                        window.opener.location = pageUrl;
                }
            }
        })();


    </script>
    <style type="text/css">
        #SelectPic
        {
        	padding: 10px;
            width: 100%;
        	margin:auto;
         	text-align:center;
            vertical-align:bottom;
        	position:fixed;
	        top:0;
	        left:0;
	        height:100%;
	        z-index:1001;
	        background-color:Gray;
	        opacity:0.9;
	        filter:alpha(opacity=90);
	        display:none;
	        vertical-align:top;
	    }


   </style>
</head>
<body>
    <form id="form1" runat="server">
     <asp:HiddenField ID="hfOfflineHFIDFlag" runat="server" Value="0" />
     <div align="center" >
 		     <img src="Images/logo.png" alt="IMIS" style="max-width: 150px; padding-top:10%; padding-bottom:15px" />
 	   </div>
 	   <div align="center">
        <table class="Login">
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td  colspan="2" align="center" class="catlabel" style="border-top-left-radius:15px; border-top-right-radius:15px;">IMIS Login</td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="FormLabel" style="width:150px !important" ><%= imisgen.getMessage("L_USERNAME",False)%></td>
                <td><asp:TextBox ID="txtUserName" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtUserName" ErrorMessage="*" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="FormLabel" style="width:150px !important" ><%= imisgen.getMessage("L_PASSWORD" )%></td>
                <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="25" Width="120px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtPassword" ErrorMessage="*" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:Button ID="btnLogin" runat="server" Text="Login" /></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tdForgotPassword">
                    <a href="ForgotPassword.aspx">
                        <asp:Label ID="lblForgotPassword" runat="server" Text='<%$RESOURCES:Resource, L_FORGOTPASSWORD %>'></asp:Label></a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
