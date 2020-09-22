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
<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/IMIS.Master" CodeBehind="Home.aspx.vb" Inherits="IMIS.Home" title="IMIS"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" >
     function pageLoadExtend() {
         showInsureePopupSearchResult();
         $('#btnCancel').click(function() {
             $('#SelectPic').hide();
         });
         GreetUser();
     }


     function GreetUser() {
         var hourOfTheDay = new Date().getHours();
         var morning = '<%=imisgen.getMessage("M_GOODMORNING",True )%>';
         var afternoon = '<%=imisgen.getMessage("M_GOODAFTERNOON", True)%>';
         var evening = '<%=imisgen.getMessage("M_GOODEVENING", True)%>';
         var greeting = '<%=imisgen.getMessage("L_WELCOME", True)%>';
         if (hourOfTheDay < 12)
             greeting = morning;
         else if (hourOfTheDay >= 12 && hourOfTheDay <= 17)
             greeting = afternoon;
         else if (hourOfTheDay > 17 && hourOfTheDay < 24)
             greeting = evening;

         $('#<%=L_CURRENTUSER.ClientID%>').html(greeting);

     }


 </script>
<style type="text/css">
    div.backentry{position:relative;}
    .footer{ bottom:2px;top:auto; }

  </style>




</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <span id="version">v1.4.1 (April 2020)</span>
    <div style="min-height:650px;">
    <div id="UserGreeting">
        <asp:Label ID="L_CURRENTUSER" runat="server" Text='<%$ Resources:Resource,L_WELCOME %>'></asp:Label>
        <asp:Label ID="txtCURRENTUSER" runat="server" Text=''></asp:Label>
    </div>

    <asp:GridView ID="gvRoles" runat="server"
        AutoGenerateColumns="False"
        EmptyDataText="No roles found" GridLines="None">

        <Columns>
            <asp:BoundField DataField="Role" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left"
                HeaderText="<%$ Resources:Resource,L_ROLES %>" SortExpression="Role">
                <HeaderStyle Width="200px" />
            </asp:BoundField>
        </Columns>

    </asp:GridView>
    <asp:GridView  ID="gvRegions" runat="server"
                    AutoGenerateColumns="False"
                    ShowSelectButton = "True"
                    GridLines="None"
                   EmptyDataText='No Region  found'
                    >

                    <Columns>
                    <asp:BoundField DataField="RegionName"  HeaderText='<%$ Resources:Resource,L_Region %>' SortExpression="RegionName" HeaderStyle-Width ="110px" HeaderStyle-HorizontalAlign="Left"> </asp:BoundField>
                    </Columns>
                </asp:GridView>
    <asp:GridView ID="gvDistrict" runat="server"
        AutoGenerateColumns="False"
        ShowSelectButton="True"
        GridLines="None"
        EmptyDataText='No District  found'>

        <Columns>
            <asp:BoundField DataField="Districtname" HeaderText='<%$ Resources:Resource,L_DISTRICT %>' SortExpression="Districtname" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
        </Columns>


    </asp:GridView>

        </div>
    <div style="bottom: 23px; left: 5px; font-size: 14px; margin-bottom: 6px;">
        <strong>&copy; Swiss Agency for Development and Cooperation</strong>
        <br />
        distributed under a royalty-free license
        <br />
        by courtesy of the copyright owner
    </div>
    <div style="position: absolute; bottom: 23px; right: 0px; margin-right:10px; margin-bottom:10px">
        <img src="Images/logo.png" alt="IMIS" style="max-width: 250px;" />
    </div>
</asp:Content>
