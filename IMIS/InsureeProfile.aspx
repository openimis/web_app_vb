<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="InsureeProfile.aspx.vb" Inherits="IMIS.InsureeProfile"
    Title='<%$ Resources:Resource,L_INSUREE %>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <div class="divBody">
        <table class="catlabel">
            <tr>
                <td>Profile</td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Repeater ID="rptInsuree" runat="server">
                        <ItemTemplate>
                            <table align="left" cellpadding="0" cellspacing="0">                               
                                <tr>
                                    <td valign="top" width="150px">
                                        <div style="border: 1px solid #AAAAAA; width: 150px; height: 150px; overflow: visible;">
                                            <img id="img1" width="150px" height="175px" alt="" src='<%#Eval("PhotoPath") %>' onerror="NoImage(this);" />
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" style="width: 500px;">
                                            <thead>
                                                <tr class="SearchHeader">
                                                    <td colspan="2"><%#Eval("CHFID") %></td>
                                                </tr>
                                                <tr style="height: 35px; font-family: Times New Roman; font-size: 20px; font-weight: bold; text-align: center; vertical-align: middle; color: Maroon;">
                                                    <td colspan="2"><%#Eval("OtherNames") %> &nbsp;<%#Eval("LastName") %></td>
                                                </tr>
                                                <tr style="height: 35px; font-family: Times New Roman; font-size: 20px; font-weight: bold; text-align: center; vertical-align: middle; color: Maroon;">
                                                    <td colspan="2"><%# Eval("DOB", "{0:d}") %><asp:Label ID="lblInsureeAge" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 35px; font-family: Times New Roman; font-size: 20px; font-weight: bold; text-align: center; vertical-align: middle; color: Maroon;">
                                                    <td colspan="2"><%#Eval("Gender") %></td>
                                                </tr>
                                            </thead>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:GridView ID="grdFamilyDetail" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="No Data Available" CssClass="mGrid" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="CHFID" HeaderText="NSHI" />
                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" />

                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table class="catlabel">
            <tr>
                <td>Policy</td>
            </tr>
        </table>
        <asp:GridView ID="gvPolicy" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvPolicy_RowDataBound"
            Width="100%" EmptyDataText="<%$ Resources:Resource,M_NOPOLICYENROLLEDFORADHERENT %>" CssClass="mGrid" HeaderStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:Resource,L_PRODUCTCODE %>" />
                <asp:BoundField DataField="ProductName" HeaderText="<%$ Resources:Resource,L_FSP %>" />
                <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Resource,L_EXPIREDATE %>" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Resource,L_STATUS %>" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Medium" />
                <asp:BoundField DataField="Ceiling1" HeaderText="Balance" ItemStyle-HorizontalAlign="Right" />

            </Columns>
        </asp:GridView>


        <table class="catlabel">
            <tr>
                <td>Claims                  
                </td>
                <td>
                    <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true">
                     </asp:DropDownList>
                </td>
                <td><asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true">
                    </asp:DropDownList></td>
                <td>
                    <asp:DropDownList ID="ddlHFCode" runat="server" AutoPostBack="true">
                </asp:DropDownList>
                                    </td>
                <td><asp:DropDownList ID="ddlClaimAdmin" runat="server">
                  </asp:DropDownList></td>
                <td style="float:right;"><asp:Button 
                        ID="B_ADD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_ADD%>'
                          />
                    </td>
            </tr>
        </table>

        <asp:GridView ID="grdClaimDetail" runat="server" AutoGenerateColumns="False"
            EmptyDataText="No Data Available" CssClass="mGrid" HeaderStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="ClaimCode" HeaderText="Claim Code" />
                <asp:BoundField DataField="DateClaimed" HeaderText="Claim Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="HFName" HeaderText="HF Name" />
                <asp:BoundField DataField="DateFrom" HeaderText="Visit From" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DateTo" HeaderText="Visit To" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Claimed" HeaderText="Amount" />
                <asp:BoundField DataField="ClaimStatus" HeaderText="Status" />
                <%--<asp:HyperLinkField Text="View" DataNavigateUrlFields = "ClaimID" DataNavigateUrlFormatString = "Claim.aspx?c={0}"  HeaderText='' SortExpression="ClaimCode" HeaderStyle-Width="30px">  
                    
                    </asp:HyperLinkField>--%>

            </Columns>
        </asp:GridView>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" runat="Server">
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</asp:Content>

