<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TreeView ID="TreeView1" runat="server" onload="TreeView1_Load" 
        onselectednodechanged="TreeView1_SelectedNodeChanged">
    </asp:TreeView>
    <asp:GridView ID="GridView1" runat="server" onrowcommand="GridView1_RowCommand" 
            AutoGenerateColumns="False">
        <Columns>
           <asp:BoundField DataField="PackageName" HeaderText="Name" />
           <asp:BoundField DataField="PackageFolder" HeaderText="Folder" />
           <asp:BoundField DataField="Status" HeaderText="Status" />
           <asp:ButtonField Text="Execute" CommandName="Execute"  ButtonType="Button"/>
        </Columns>    
    </asp:GridView>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Refresh" />
    </div>        
    </form>
</body>
</html>
