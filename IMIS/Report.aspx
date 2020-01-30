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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Report.aspx.vb" Inherits="IMIS.Report" 
    title='<%$ Resources:Resource,L_REPORTS %>'%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">

    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
    
<div align="center" class="divBody" style="overflow-x:auto;"> 
<h2><a href="#" runat="server" id="Back"><asp:label ID="lblGoBack" runat="server" Text='<%$ Resources:Resource,L_GOBACKTOSELECTOR%>'></asp:label></a></h2>
   
    
    <rsweb:ReportViewer ID="rptViewer" runat="server" class="reportViewer" SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        
    </rsweb:ReportViewer>


    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="NP_CENTRALDataSetTableAdapters.tblInsureeTableAdapter"></asp:ObjectDataSource>


    <%--<rsweb:ReportViewer ID="rptViewer" runat="server" Font-Names="Times New Roman"
        Font-Size="8pt" Height="530px" Width="100%" ShowExportControls="true" ShowPrintButton="true"
        ShowRefreshButton="false" ShowZoomControl="true" SkinID="" AsyncRendering="false"
        ShowBackButton="false">
    </rsweb:ReportViewer>--%>

  
</div>

     <script type="text/javascript">
         $(document).ready(function () {

             $("#" + $("#<%= rptViewer.ClientId %>").attr("id") + "_fixedTable").css({ "background": "white" });
        });
    </script>

<div id="divJsScript" style="display:none;" runat="server"></div>
</asp:Content>
