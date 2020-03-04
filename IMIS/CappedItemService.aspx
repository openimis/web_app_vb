<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="CappedItemService.aspx.vb" Inherits="IMIS.CappedItemService"
    Title='<%$ Resources:Resource,L_INSUREE %>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <div class="divBody">
        <br />
        <h3>Capped Item/Service Utilized by Member</h3>
        <br />
        <asp:GridView ID="grdCappedDetails" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No Data Available" CssClass="mGrid" HeaderStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="CHFID" HeaderText="Membership No." />
                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                <asp:BoundField DataField="ClaimCode" HeaderText="Claim Code" />
                <asp:BoundField DataField="ServName" HeaderText="Item/Service Name"  />
                <asp:BoundField DataField="ServFrequency" HeaderText="Frequency"   />
                <asp:BoundField DataField="DateTo"  DataFormatString="{0:d}"  HeaderText="Utilized Date"  />
                <asp:BoundField DataField="HFName" HeaderText="Health Facility"   />

            </Columns>
        </asp:GridView>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" runat="Server">
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</asp:Content>
