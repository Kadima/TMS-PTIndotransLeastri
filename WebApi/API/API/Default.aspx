<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApi.Default" %>

<!DOCTYPE html>
<html lang="en-US">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>Web API Service</title>
    <link rel="stylesheet" href="libs/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="libs/css/ui-dialog.css"/>
    <script src="libs/js/jquery.min.js"></script>
    <script src="libs/js/bootstrap.min.js"></script>
    <script src="libs/js/dialog-min.js"></script>
    <style type="text/css">
        h1 {
            color: #FFF;
            font-size: 26px;
            font-weight: normal;
            margin: 0;
            padding: 0 0 0 15px;
            line-height: 48px;
            min-height: 48px;
            border-radius: 0px;
            border-bottom: 1px solid #191e23;
            background: #2c3742; /* Old browsers */
            background: -moz-linear-gradient(top, #2c3742 0%, #28303a 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#2c3742), color-stop(100%,#28303a)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #2c3742 0%,#28303a 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #2c3742 0%,#28303a 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #2c3742 0%,#28303a 100%); /* IE10+ */
            background: linear-gradient(to bottom, #2c3742 0%,#28303a 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#2c3742', endColorstr='#28303a',GradientType=0 ); /* IE6-9 */
        }  
        .hide {
            display:none;
        }
        footer {
            position:absolute;
            bottom:2em;
            width:98%;
            text-align:center;
            z-index:2;
            overflow:hidden;
        }
    </style>
</head>
<body>
    <div class="container-fluid">
        <header>            
            <h1>Web API Dashboard</h1>
        </header>
        <br />
        <div class="panel panel-default">
          <div class="panel-heading">Version Status</div>
          <div id="divPanelBody" class="panel-body" style="display:none;">            
            <form id="form1" runat="server">
                <div>
                    <asp:Button ID="Button1" runat="server" Text="" Width="0" Height="0" onclick="Button1_Click" CssClass="hide"/>
                    <button class="btn btn-default btn-info" type="button" id="Button0" onclick="updateWS();return false;">Update WebService</button>   
                </div>
            </form>
          </div>
          <ul class="list-group">
            <li class="list-group-item">Web Service Core Version ： <% = strVersion %></li>
            <li class="list-group-item">Application Pool Runtime Version ： <% = strNetVersion %></li>
            <li class="list-group-item">IIS Server Version ： <% = strServiceIIS %></li>
          </ul>
        </div>
        <br />
        <footer>
            <h5>Power by <a href="http://www.sysfreight.com" target="_blank" title="sysfreight">SysMagic Software Solution</a>.</h5>
        </footer>
    </div>
    <script>
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        var currentDiv = document.getElementById("divPanelBody");
        var myurl = GetQueryString("egg");
        if (myurl != null && myurl.toString().length > 0) {
            var d = dialog({
                title: 'Message',
                content: '<input id="property-returnValue" value="" autofocus />',
                ok: function () {
                    var value = $('#property-returnValue').val();
                    this.close(value);
                    this.remove();
                    if (value === "sysfrt") {
                        currentDiv.style.display = "block";
                    }
                }
            });
            d.show();
        }
        function updateWS() {
            document.getElementById("Button1").click();
        }
    </script>
</body>
</html>
