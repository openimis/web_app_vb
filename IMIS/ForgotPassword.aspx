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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForgotPassword.aspx.vb" Inherits="IMIS.ForgotPassword" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= imisgen.getMessage("T_FORGOTPASSWORD", True ) %></title>
    <link href="StyleSheets/Imis.css" rel="stylesheet" />
    <script src="Javascripts/jquery-1.8.2.min.js"></script>
</head>
<body>

    

    <form id="form1" runat="server">
    <div style="height:500px;top:10%;">
        <div id="ForgotPassword">
            <div class="Heading">
                <asp:Literal ID="heading" runat="server" Text='<%$RESOURCES:Resource, L_FOROGOTPASSWORDHEADING %>'></asp:Literal>
            </div>
            <div class="Summary">
                <asp:ValidationSummary ID="vs" runat="server" DisplayMode="List" ValidationGroup="submit" />
            </div>
            <div class="line">
                <div class="lbl">Email:</div>
                <div class="cnt"><asp:TextBox ID="txtEmailId" runat="server" Width="80%" required></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmailId" Display="none" SetFocusOnError="True" ValidationGroup="submit" ErrorMessage='<%$RESOURCES:Resource, L_EMAILREQUIRED %>'></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmailId" Display="none" SetFocusOnError="True" ValidationGroup="submit" ErrorMessage='<%$RESOURCES:Resource, L_INVALIDEMAIL %>' style="direction: ltr" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </div>
                    
            </div>
            <div class="btn">
                <a href="Default.aspx"><asp:Literal ID="Login" runat="server" Text='<%$RESOURCES:Resource, L_BACKTOLOGIN %>'></asp:Literal></a>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="submit" ClientIDMode="Static" />
            </div>
            <div class="msg">
                <asp:Literal ID="msg" runat="server"></asp:Literal>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
