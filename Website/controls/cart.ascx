<%@ Control Language="c#" AutoEventWireup="false" Codebehind="cart.ascx.cs" Inherits="WebFramework.Ascx.Cart" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Shop" Namespace="WebFramework.Shop" Assembly="Psantos3" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
		<Shop:CartPreview runat="server" />
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> » <a href="shop.aspx">Loja</a> » <a href="cart.aspx">Carrinho</a>
	</span>
</div>
<div class="wiki_topic">Carrinho</div>

<asp:Literal ID="message" Runat="server" />

<p>
	Eu posso aceitar trocas por determinados artigos, e posso ser flexível nos preços se 
	for convencido disso. Existe uma caixa de texto nesta página para que seja transmitida com a encomenda
	alguma informação relevante.
</p>
<p>
	Ver também <a href="wiki.aspx?topic=PRE.MeiosDePagamento">Meios de Pagamento</a>
	e <a href="wiki.aspx?topic=PRE.Encomendas">Como se Processa a Encomenda</a>.
</p>

<Shop:ShowCart runat="server" />

<p>Para realizar a encomenda, preencha e envie o seguinte formulário:</p>

<asp:RequiredFieldValidator 
	ControlToValidate="mail"
	Display="Dynamic"
	InitialValue=""
	runat="server">
		Tem de Introduzir o seu mail!
</asp:RequiredFieldValidator>

<div id="checkOutZone">
	O Seu Email: <asp:TextBox ID="mail" Runat="server" /><asp:Button OnClick="CheckOut" Runat="server" Text="Encomendar" /><br/>
	<asp:TextBox ID="text" TextMode="MultiLine" style="width: 95%; height: 100px" Runat="server">Informações adicionais aqui, nomeadamente a sua Morada, o seu nome, e outros contactos.
Será contactado por mim assim que eu receber a notificação de encomenda.</asp:TextBox>
</div>
