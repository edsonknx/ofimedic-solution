<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Fotos.aspx.vb" Inherits="Ofimedic.Frontend.Fotos" Async="true" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Fotos</title>
    <style>
        body { font-family: Arial; margin: 20px; }
        .foto { display: inline-block; margin: 10px; text-align: center; }
        img { width: 150px; height: 150px; object-fit: cover; border: 1px solid #ddd; }
        .volver { margin: 20px 0; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="volver">
            <a href="Default.aspx">← Volver</a>
        </div>
        
        <h2>Fotos del Álbum</h2>
        
        <asp:Repeater ID="rptFotos" runat="server">
            <ItemTemplate>
                <div style="display: inline-block; margin: 10px; text-align: center; border: 1px solid #ccc; padding: 10px; width: 150px;">
                    <img src='<%# Eval("ThumbnailUrl") %>' style="width: 100px; height: 100px;" />
                    <br />
                    <span><%# Eval("Title") %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        
       <asp:Label ID="lblNoFotos" runat="server" Text="No hay fotos en este álbum" Visible="false" />
       <asp:Label ID="litMensaje" runat="server" />
    </form>
</body>
</html>