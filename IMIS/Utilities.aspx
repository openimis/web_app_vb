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
<%@ Page Title='<%$ Resources:Resource, L_IMISUTILITIES%>' Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Utilities.aspx.vb" Inherits="IMIS.Utilities" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">


<script type="text/javascript" language="javascript">




    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest)


    //Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(
    function InitializeRequest() {

        if (document.getElementById) {
            var progress = document.getElementById('Progress');
            var blur = document.getElementById('SelectPic');

            progress.style.width = '150px';
            progress.style.height = '40px';

            blur.style.height = document.documentElement.clientHeight;
            blur.style.display = 'block';

            progress.style.top = document.documentElement.clientHeight / 3 - Progress.style.height.replace('px', '') / 2 + 'px';
            progress.style.left = document.body.offsetWidth / 2 + 'px'; // / 2 - progress.style.width.replace('px', '') / 2 + 'px';

        }
    }



    //)


   

</script>

<div class="divBody">



<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
        

    <table width="100%">
        <tr>
            <td class="MainMenu">
                <asp:Label ID="lblBackEnd" runat="server" Text='<%$ Resources:Resource,L_BACKENDVERSION %>'></asp:Label>
                  <asp:Literal ID="ltlBackendVersion" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_Extract1"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_BACKUP %>'>
                   </asp:label>   
                    
                </td>
            </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td valign="middle" class="panel" style="height:50px">
                <asp:TextBox ID="txtPath" runat="server" width="200px" MaxLength="255"></asp:TextBox>
                <asp:CheckBox ID="chkSavePath" runat="server" Text='<%$ Resources:Resource,L_SAVEPATH %>' TextAlign="Right" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage='<%$ Resources:Resource,M_ENTERBACKUPPATH %>' ControlToValidate="txtPath" Text='<%$ Resources:Resource,M_ENTERBACKUPPATH %>' 
                    ValidationGroup="Backup" SetFocusOnError="True"></asp:RequiredFieldValidator>
                
                <asp:Button ID="btnBackup" runat="server" Text='<%$ Resources:Resource,L_BACKUP %>' style="float:right" ValidationGroup="Backup" ToolTip="Click on the button to backup a database to the displayed path on the server." />
            </td>
        </tr>
        
        <tr>
            <td>
                <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="Label1"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_RESTORE %>'>
                   </asp:label>   
                    
                </td>
            </tr>
        </table>
            </td>
        </tr>
        
        <tr>
            <td valign="middle" class="panel" style="height:50px">
                <asp:TextBox ID="txtRestore" runat="server" MaxLength="255" width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtRestore" ErrorMessage='<%$ Resources:Resource,M_ENTERRESTOREPATH %>' 
                    SetFocusOnError="True" Text='<%$ Resources:Resource,M_ENTERRESTOREPATH %>' ValidationGroup="Restore"></asp:RequiredFieldValidator>
                <asp:Button ID="btnRestore" runat="server" style="float:right" Text='<%$ Resources:Resource,L_RESTORE %>' 
                    ToolTip="Click on the button to restore a database from the give path." 
                    ValidationGroup="Restore" />
            </td>
        </tr>
        
        <tr>
            <td valign="middle">
                 <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="Label2"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_EXECUTESCRIPT %>'>
                   </asp:label>   
                    
                </td>
            </tr>
        </table>
           
            </td>
        </tr>
        
         <tr>
            <td valign="middle" class="panel" style="height:50px">
            
            
                <asp:FileUpload ID="FileUpload1" runat="server" />
                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="FileUpload1" ErrorMessage='<%$ Resources:Resource,M_PLEASESELECTTHEFILE %>' 
                    SetFocusOnError="True" Text='<%$ Resources:Resource,M_PLEASESELECTTHEFILE %>' ValidationGroup="Execute"></asp:RequiredFieldValidator>
                
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ErrorMessage='<%$ Resources:Resource,M_SELECTONLYISFFILE %>' ControlToValidate="FileUpload1" Text='<%$ Resources:Resource,M_SELECTONLYISFFILE %>'
                    SetFocusOnError="true" ValidationGroup="Execute" 
                    ValidationExpression="^.+\.(isf)$" ></asp:RegularExpressionValidator>
                
                <asp:Button ID="btnExecute" runat="server" style="float:right" Text='<%$ Resources:Resource,B_EXECUTE %>' 
                    ToolTip="Click on the button to to execute the script." 
                    ValidationGroup="Execute" />
               
            </td>
        </tr>
        
    </table>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="SelectPic"></div>
            <div id="Progress">
                <div style="width:30px; float:left;"><img height="30px" width="30px" src="Images/progress.gif" /></div>
                Please wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExecute" />
    </Triggers>
</asp:UpdatePanel>
<asp:Panel ID="Panel1" runat="server">
</asp:Panel>



</div>
    <asp:UpdatePanel ID="upButtons" runat="server">
        <ContentTemplate>

        
<asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
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
    </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
