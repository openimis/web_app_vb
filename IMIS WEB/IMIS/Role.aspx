<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Role.aspx.vb" Inherits="IMIS.Role"
 Title='<%$ Resources:Resource,P_ROLE %>' %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content runat="server" ContentPlaceHolderID="body">
    <script type="text/javascript">
      
        function fireCheckChanged() {
           //alert("test");
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var chk = document.getElementById('<%= chkIsSystem.ClientID %>');
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
             
            if(isChkBoxClick)
            {
                
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                //check if nxt sibling is not null & is an element node
                
                if(nxtSibling && nxtSibling.nodeType == 1)
                {
                //if node has children 
                   
                    if (nxtSibling.tagName.toLowerCase() == "div")
                    
                    {
                    //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                 }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

         function CheckUncheckChildren(childContainer, check)
         {
              
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for(var i=0;i<childChkBoxCount;i++)
            {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) 
             
            {
               
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if(parentNodeTable)
            {
                var checkUncheckSwitch;
                checkUncheckSwitch = true;
                //checkbox checked 
                if(!check)
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked) {
                        //alert("1");
                        checkUncheckSwitch = false;
                    }


                    else {
                         //alert("2");
                         checkUncheckSwitch = false;
                            return; //do not need to check parent if any(one or more) child not checked
                    }
                        
                }
                else //checkbox unchecked
                {
                    //alert("3");
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked) {
                        //alert("4");
                        checkUncheckSwitch = false;
                    }


                    else {
                         //alert("5");
                         checkUncheckSwitch = true;
                           /* return;*/ //do not need to check parent if any(one or more) child not checked
                    }
                }
   
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if(inpElemsInParentTable.length > 0)
                {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox)
        {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for(var i=0;i<childCount;i++)
            {
                if(parentDiv.childNodes[i].nodeType == 1)
                {
                //check if the child node is an element node
                    if(parentDiv.childNodes[i].tagName.toLowerCase() == "table")
                    {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if(prevChkBox.checked)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj)
        {
            var parent = childElementObj.parentNode;
            while(parent.tagName.toLowerCase() != parentTagName.toLowerCase())
            {
                parent = parent.parentNode;
            }
            return parent;
        }
        
    </script>


       
    <form id="Role" action="Role.aspx">
       <asp:Panel ID="pnlHeader"  runat="server" CssClass="panelTop" GroupingText='<%$ Resources:Resource,G_USER %>'> 
              <table width="100%" cellpadding="10 10 10 10" >
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblRole" runat="server" Text='<%$ Resources:Resource,L_ROLENAME %>'></asp:Label>
                    </td>
                     <td width="25%">
                         <asp:Textbox ID="txtRoles" size="10" runat="server" Width="135px"> </asp:Textbox> 
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldCode" runat="server" 
                        ControlToValidate="txtRoles" 
                        SetFocusOnError="False"
                        ValidationGroup="check"   ForeColor="Red" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        
                    </td>
                     <td width="25%">
                          <asp:Label ID="lblIsSystem" runat="server" Text='<%$ Resources:Resource,L_SYSTEM %>'  ></asp:Label> 
                         <asp:CheckBox ID="chkIsSystem" runat="server" Enabled="false" />
                    </td>
                    <td width="25%">
                          <asp:Label ID="Label1" runat="server" Text='<%$ Resources:Resource,L_BLOCKED %>' ></asp:Label> 
                         <asp:CheckBox ID="chkIsBlocked" runat="server" />
                    </td>                   
                </tr>
            </table>

        </asp:Panel  >
        <div>
            <table width="100%" cellpadding="10 10 10 10">
                <tr>
                    <td width="25%">
                        <asp:Panel ID="pnltreeview1"  runat="server" height="530" ScrollBars="Horizontal"> 
                          
                                    <asp:TreeView ID="tvRoleRights" runat="server" ImageSet="Arrows" ShowCheckBoxes="All" ShowLines="True" ExpandDepth="1" PopulateNodesFromClient="False" >
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <Nodes> 

                            <%--100000--%>
                            <asp:TreeNode Text="<%$ Resources:Resource,R_INSUREEANDPOLICIES %>" Value="100000" SelectAction="Expand" ImageUrl="~/Images/Renew.png" > 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_FAMILY %>" Value="101000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="101001" SelectAction="None">  </asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="101002" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="101003" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="101004" SelectAction="None"></asp:TreeNode>                           
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_INSUREE %>" Value="101100" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="101101" SelectAction="None"></asp:TreeNode>                      
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="101102" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="101103" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="101104" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_ENQUIRE %>" Value="101105" SelectAction="None"></asp:TreeNode> <%--NEW--%>
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_POLICY %>" Value="101200" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="101201" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="101202" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="101203" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="101204" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_RENEW %>" Value="101205" SelectAction="None"></asp:TreeNode>
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_CONTRIBUTION %>" Value="101300" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="101301" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="101302" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="101303" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="101304" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_PAYMENT %>" Value="101400" SelectAction="Expand" ImageUrl="~/Images/Renew.png" >  
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="101401" SelectAction="None">  </asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="101402" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="101403" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="101404" SelectAction="None"></asp:TreeNode>                                                            
                                </asp:TreeNode>

                            </asp:TreeNode>  
                        </Nodes>
                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                    </asp:Panel>
                    </td>
                     <td width="25%">
                         <asp:Panel ID="pnltreeview2"  runat="server" height="530" ScrollBars="Horizontal"> 
                          <asp:TreeView ID="tvRoleRights2" runat="server" ImageSet="Arrows" ShowCheckBoxes="All" ShowLines="True" ExpandDepth="1" PopulateNodesFromClient="False" >
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />

                <Nodes>
                     

                            <%--110000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMS %>" Value="110000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIM %>" Value="111000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="111001" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="111002" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="111003" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="111004" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_LOAD %>" Value="111005" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_PRINT %>" Value="111006" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SUBMIT %>" Value="111007" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMREVIEW %>" Value="111008" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_FEEDBACK %>" Value="111009" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPDATE %>" Value="111010" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_PROCESS %>" Value="111011" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_BATCH %>" Value="111100" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_PROCESS %>" Value="111101" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_FILTER %>" Value="111102" SelectAction="None"></asp:TreeNode> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_PREVIEW %>" Value="111103"  SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>
                                </asp:TreeNode> 
                </Nodes>

                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
                             </asp:Panel>
                    </td>
                     <td width="25%">
                          <asp:Panel ID="pnltreeview3"  runat="server" height="530" ScrollBars="Horizontal"> 
                          <asp:TreeView ID="tvRoleRights3" runat="server" ImageSet="Arrows" ShowCheckBoxes="All" ShowLines="True" ExpandDepth="1" PopulateNodesFromClient="False" >
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />

                <Nodes>
                     <%--120000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_ADMINISTRATION %>" Value="120000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PRODUCTS %>" Value="121000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121001" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121002" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121003" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121004" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DUPLICATE %>" Value="121005" SelectAction="None"></asp:TreeNode> 
                                    </asp:TreeNode>                                 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_HEALTHFACILITIES %>" Value="121100" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121101" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121102" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121103" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121104" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>   
                                <asp:TreeNode Text="<%$ Resources:Resource,R_PRICELISTMS %>" Value="121200" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121201" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121202" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121203" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121204" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DUPLICATE %>" Value="121205" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_PRICELISTMI %>" Value="121300" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121301" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121302" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121303" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121304" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DUPLICATE %>" Value="121305" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_MEDICALSERVICES %>" Value="121400" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121401" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121402" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121403" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121404" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_MEDICALITEMS %>" Value="122100" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="122101" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="122102" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="122103" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="122104" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_ENROLMENTOFFICER %>" Value="121500" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121501" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121502" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121503" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121504" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>   
                                <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMADMINISTRATOR %>" Value="121600" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121601" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121602" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121603" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121604" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_USERS %>" Value="121700" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121701" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121702" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121703" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121704" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_PAYERS %>" Value="121800" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121801" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121802" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121803" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121804" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_LOCATIONS %>" Value="121900" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="121901" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_ADD %>" Value="121902" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="121903" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="121904" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_MOVE %>" Value="121905" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode>  
                                <asp:TreeNode Text="<%$ Resources:Resource,R_USERPROFILES %>" Value="122000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SEARCH %>" Value="122001" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="122002" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_EDIT %>" Value="122003" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DELETE %>" Value="122004" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DUPLICATE %>" Value="122005" SelectAction="None"></asp:TreeNode>                            
                                </asp:TreeNode>
                            </asp:TreeNode> 
                </Nodes>

                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
                              </asp:Panel>
                    </td>
                     <td width="25%">
                         <asp:Panel ID="pnltreeview4"  runat="server" height="500" ScrollBars="Horizontal"> 
                          <asp:TreeView ID="tvRoleRights4" runat="server" ImageSet="Arrows" ShowCheckBoxes="All" ShowLines="True" ExpandDepth="1" PopulateNodesFromClient="False" >
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />

                <Nodes>                    
                            <%--130000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_TOOLS %>" Value="130000" SelectAction="None" ImageUrl="~/Images/Renew.png"></asp:TreeNode> 
                            
                            <%--140000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_REGISTERS %>" Value="140000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">    
                                <asp:TreeNode Text="<%$ Resources:Resource,R_DIAGNOSES %>" Value="141000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="141001" SelectAction="None"> </asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DOWNLOAD %>" Value="141002" SelectAction="None"> </asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_HEALTHFACILITIES %>" Value="141100" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="141101" SelectAction="None"></asp:TreeNode>
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DOWNLOAD %>" Value="141102" SelectAction="None"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_LOCATIONS %>" Value="141200" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="141201" SelectAction="None"></asp:TreeNode> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DOWNLOAD %>" Value="141202" SelectAction="None"></asp:TreeNode>
                                </asp:TreeNode>
                                </asp:TreeNode> 
                            
                            <%--150000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_EXTRACTS %>" Value="150000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">                                        
                                <asp:TreeNode Text="<%$ Resources:Resource,R_MASTERDATA %>" Value="151000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_DOWNLOAD %>" Value="151001" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_PHONEEXTRACTS %>" Value="151100" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_CREATE %>" Value="151101" SelectAction="None"></asp:TreeNode>
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_OFFLINEEXTRACTS %>" Value="151200" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_CREATE %>" Value="151201" SelectAction="None"></asp:TreeNode>
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMS %>" Value="151300" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="151301" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_ENROLMENTS %>" Value="151400" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="151401" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                <asp:TreeNode Text="<%$ Resources:Resource,R_FEEDBACK %>" Value="151500" SelectAction="Expand" ImageUrl="~/Images/Renew.png">  
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_UPLOAD %>" Value="151501" SelectAction="None"></asp:TreeNode> 
                                </asp:TreeNode> 
                                </asp:TreeNode>  

                                         <%--160000--%>
                                <asp:TreeNode ShowCheckBox="True" Text="<%$ Resources:Resource,R_REPORTS %>" Value="160000" SelectAction="Expand" ImageUrl="~/Images/Renew.png"> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PRIMARYOPERATIONALIP %>" Value="160001" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PRIMARYOPERATIONALIC %>" Value="160002" SelectAction="None" ></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_DERIVEDOPERATIONALI %>" Value="160003" SelectAction="None" ></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CONTRIBUTIONCOLLECTION %>" Value="160004" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PRODUCTSALES %>" Value="160005" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CONTRIBUTIONDISTRIBUTION %>" Value="160006" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_USERACTIVITYREPORT %>" Value="160007" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_ENROLMENTPERFORMANCEI %>" Value="160008" SelectAction="None"></asp:TreeNode>                                                       
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_STATUSOFREGISTERS %>" Value="160009" SelectAction="None"></asp:TreeNode>                         
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_INSUREESWITHOUTPHOTOS %>" Value="160010" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PAYMENTCATEGORYOVERVIEW %>" Value="160011" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_MATCHINGFUNDS %>" Value="160012" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMOVERVIEW %>" Value="160013" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PERCENTAGEOFREFERRALS %>" Value="160014" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_FAMILIESANDINSUREESOVERVIEW %>" Value="160015" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_PENDINGINSUREES %>" Value="160016" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_RENEWALS %>" Value="160017" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CAPITATIONPAYMENT %>" Value="160018" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_REJECTEDPHOTOS %>" Value="160019" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CONTRIBUTIONPAYMENT %>" Value="160020" SelectAction="None"></asp:TreeNode>                                   
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CONTROLNUMBERASSIGNMENT %>" Value="160021" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_OVERVIEWOFCOMMISSIONS %>" Value="160022" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_CLAIMHISTORYREPORT %>" Value="160022" SelectAction="None"></asp:TreeNode>
                                    
                                </asp:TreeNode>

                                    <%--170000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_UTILITIES %>" Value="170000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_BACKUP %>" Value="170001" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_RESTORE %>" Value="170002" SelectAction="None"></asp:TreeNode>
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_EXCUTESCRIPT %>" Value="170003" SelectAction="None"></asp:TreeNode> 
                                    <asp:TreeNode Text="<%$ Resources:Resource,R_EMAILSETTING %>" Value="170004" SelectAction="None"> 
                                </asp:TreeNode> 
                                </asp:TreeNode>

                                    <%--180000--%>
                                <asp:TreeNode Text="<%$ Resources:Resource,R_FUNDING %>" Value="180000" SelectAction="Expand" ImageUrl="~/Images/Renew.png">                       
                                        <asp:TreeNode Text="<%$ Resources:Resource,R_SAVE %>" Value="181001" SelectAction="None"> </asp:TreeNode> 
                                </asp:TreeNode> 
                </Nodes>

                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
                              </asp:Panel>
                    </td>
                </tr>
            </table> 
        </div>
        </form>    
     <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <table width="100%" cellpadding="10 10 10 10">
            <tr>

                <td align="left">

                    <asp:Button
                        ID="B_SAVE"
                        runat="server"
                        Text='<%$ Resources:Resource,B_SAVE%>'
                        ValidationGroup="check" />
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
    <asp:label id="lblMsg"  runat="server"></asp:label><asp:ValidationSummary ID="validationSummary" runat="server"  HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>
