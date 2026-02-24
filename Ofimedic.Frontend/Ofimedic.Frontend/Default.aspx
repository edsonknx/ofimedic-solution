<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Ofimedic.Frontend._Default" Async="true"%>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Álbumes</title>
    <style>
        body { font-family: Arial; margin: 20px; }
        .filtro { margin: 20px 0; padding: 10px; background: #f0f0f0; }
        table { width: 100%; border-collapse: collapse; }
        th { background: #333; color: white; padding: 8px; text-align: left; }
        td { padding: 8px; border-bottom: 1px solid #ddd; }
        tr:hover { background: #f5f5f5; }
        .btn { padding: 5px 15px; background: #007bff; color: white; border: none; cursor: pointer; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Álbumes</h1>
        
        <div class="filtro">
            Título: <asp:TextBox ID="txtFiltro" runat="server" />
            <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn" runat="server" />
            <asp:Button ID="btnSync" Text="Sincronizar" CssClass="btn" runat="server" />
        </div>
        
        <asp:Literal ID="litMensaje" runat="server" />
        
        <asp:GridView ID="gvAlbumes" runat="server" AutoGenerateColumns="false" 
            DataKeyNames="Id" OnRowCommand="gvAlbumes_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Title" HeaderText="Título" />
                <asp:BoundField DataField="UserId" HeaderText="Usuario" />
                <asp:ButtonField Text="Ver Fotos" CommandName="VerFotos" ButtonType="Button" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>