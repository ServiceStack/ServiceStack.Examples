<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Soap12.aspx.cs" Inherits="ServiceStack.Examples.Clients.Soap12" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ServiceStack Examples :: SOAP 1.2 Web Services</title>
    <link rel="stylesheet" type="text/css" href="default.css" />
</head>
<body class="soap-page">

    <div id="header-links">
        <a href="Default.htm">Ajax Client Examples</a>
        <a href="Soap11.aspx">Soap 1.1</a>
        <a href="Soap12.aspx">Soap 1.2</a>
        <a href="Silverlight.htm">Silverlight Examples</a>
        <a href="../ServiceStack.Examples.Host.Web/AjaxClient/MovieRestTest.htm">REST examples</a>
    </div>

    <a href="http://www.servicestack.net"><img src="img/demo-logo-servicestack.png" alt="Service Stack Demo" /></a>

    <h1>Trying ServiceStack's SOAP 1.2 Web Service Examples</h1>
    
    <div class="note">
        Testing ServiceStack's SOAP 1.2 Web services using 
        VS.NET 'Add Service Reference' code-generated proxy.<br />
        The source code for this ASP.NET page is <a href="https://github.com/mythz/ServiceStack.Examples/blob/master/src/ServiceStack.Examples.Clients/Soap12.aspx.cs">available here</a>.
    </div>
    
    <form id="form1" runat="server">
    
    <div class="soapservice">
        <h3>Get Factorial Services</h3>
        <div class="result">            
            <asp:Literal ID="litGetFactorialResult" runat="server" />
        </div>
        <div class="error">            
            <asp:Literal ID="litGetFactorialError" runat="server" />
        </div>
        <dl>
            <dt>For Number:</dt>
            <dd>
                <asp:TextBox ID="txtGetFactorial" Text="6" runat="server"></asp:TextBox>
                <asp:Button ID="btnGetFactorial" runat="server" Text="Go" 
                    onclick="btnGetFactorial_Click" />
            </dd>
        </dl>    
    </div>
    
    <div class="soapservice">
        <h3>Get Fibonacci Numbers Service</h3>
        <div class="result">
            <asp:Literal ID="litGetFibonacciResult" runat="server" />
        </div>
        <div class="error">            
            <asp:Literal ID="litGetFibonacciError" runat="server" />
        </div>
        <dl>
            <dt>Skip:</dt>
            <dd>
                <asp:TextBox ID="txtGetFibonacciSkip" Text="5" runat="server"></asp:TextBox>
            </dd>            
            <dt>Take:</dt>
            <dd>
                <asp:TextBox ID="txtGetFibonacciTake" Text="10" runat="server"></asp:TextBox>
                <asp:Button ID="btnGetFibonacci" runat="server" Text="Go" 
                    onclick="btnGetFibonacci_Click" />
            </dd>            
        </dl>    
    </div>
    
    <div class="soapservice">
        <h3>Store New User Service</h3>
        <div class="result">
            <asp:Literal ID="litStoreNewUserResult" runat="server" />
        </div>
        <div class="error">
            <asp:Literal ID="litStoreNewUserError" runat="server" />
        </div>
        <dl>
            <dt>User Name:</dt>
            <dd>
                <asp:TextBox ID="txtStoreNewUserUsername" Text="User 1" runat="server"></asp:TextBox>
            </dd>
            <dt>Password:</dt>
            <dd>
                <asp:TextBox ID="txtStoreNewUserPassword" Text="password" runat="server"></asp:TextBox>
            </dd>
            <dt>Email:</dt>
            <dd>
                <asp:TextBox ID="txtStoreNewUserEmail" Text="as@if.com" runat="server"></asp:TextBox>
            </dd>
            <dd>
                <asp:Button ID="btnStoreNewUser" runat="server" Text="Create New User" 
                    onclick="btnStoreNewUser_Click" />
            </dd>
            <dd>
                <asp:Button ID="btnDeleteAllUsers" runat="server" Text="Delete all Users" 
                    onclick="btnDeleteAllUsers_Click" />
            </dd>
        </dl>    
    </div>
    
    <div class="soapservice">
        <h3>Get Users Service</h3>
        <div class="result">
            <asp:Literal ID="litGetUsersResult" runat="server" />
        </div>
        <div class="error">
            <asp:Literal ID="litGetUsersError" runat="server" />
        </div>
        <dl>
            <dt>User Ids:</dt>
            <dd>
                <asp:TextBox ID="txtGetUsersUserIds" runat="server"></asp:TextBox>
                <asp:Button ID="btnGetUsers" runat="server" Text="Get Users" onclick="btnGetUsers_Click" 
                        />
            </dd>
        </dl>    
    </div>
    
    
    </form>


    <div id="footer-links">
        <a href="http://mono-project.com/">
            <img src="img/Mono-powered-big.png" alt="powered by mono" />
        </a>

        <a href="http://www.ajaxstack.com">Ajax Stack</a> |
        <a href="http://www.servicestack.net">Service Stack</a>

    </div>

</body>
</html>
