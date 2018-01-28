<%@ Page Title="Home Page" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoCaquiCodersInteligenciaArtificial._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <div class="row text-center">
                    <div class="col-lg-6">
                         <asp:FileUpload ID="fuFoto" runat="server" CssClass="btn-file" /> 
                    </div>
                     <div class="col-lg-6">
                        <asp:Button ID="btnUpload" runat="server" Text="Upload"  OnClick="btnUpload_Click"/>
                     </div>
                </div>
               <br />
                <img src="" id="imgFoto" runat="server" style="width:100%; height:auto;" />
                <br /><br />
                <asp:Button ID="btnReconhecimentoFacial" Visible="false" runat="server" Text="Reconhecimento Facial" OnClick="btnReconhecimentoFacial_Click" />

                
                
            </div>
            <div class="col-lg-1"></div>
            <div class="col-lg-5 text-center" style="overflow:scroll; max-height:600px;" id="divCaracteristicas" runat="server">

                <h3>Características</h3>
                <br /><br />
                <asp:Repeater ID="rptFaces" runat="server" OnItemDataBound="rptFaces_ItemDataBound">
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-lg-6">
                                <img src='Content/Fotos/<%# Eval("FaceId").ToString() + ".jpeg" %>' style="width:100%; height:auto;" />
                            </div>
                            <div class="col-lg-6 text-left">
                                 
                                <b>Idade:</b> <asp:Label ID="lblIdade" runat="server" Text='<%# Eval("FaceAttributes.Age").ToString() %>'></asp:Label>
                                <br />
                                <b>Gênero:</b> <asp:Label ID="lblGenero" runat="server" Text='<%# Eval("FaceAttributes.Gender").ToString() %>'></asp:Label>
                                 <br />
                            </div>
                        </div>
                        <hr />
                        <div class="row text-left">
                            <div class="col-lg-6" >
                                
                                <b>Sentimento: </b> 
                                    <br />
                                    <div style="padding-left:10px;">
                                        <ul>
                                            <li>Raiva: <%# ( Double.Parse(Eval("FaceAttributes.Emotion.Anger").ToString())*100).ToString() %> %</li>
                                            <li>Desprezo: <%# (Double.Parse( Eval("FaceAttributes.Emotion.Contempt").ToString())*100).ToString()  %> %</li>
                                            <li>Desgosto: <%# (Double.Parse(Eval("FaceAttributes.Emotion.Disgust").ToString())*100).ToString() %> %</li>
                                            <li>Medo: <%# (Double.Parse(Eval("FaceAttributes.Emotion.Fear").ToString())*100).ToString() %> %</li>
                                            <li>Felicidade: <%# (Double.Parse(Eval("FaceAttributes.Emotion.Happiness").ToString())*100).ToString() %> %</li>
                                             <li>Neutro: <%# (Double.Parse(Eval("FaceAttributes.Emotion.Neutral").ToString())*100).ToString() %> %</li>
                                             <li>Tristeza: <%# (Double.Parse(Eval("FaceAttributes.Emotion.Sadness").ToString())*100).ToString() %> %</li>
                                            <li>Surpresa: <%#  (Double.Parse(Eval("FaceAttributes.Emotion.Surprise").ToString())*100).ToString() %></li>
                                        </ul>
                                    </div>

                                    <br />
                        

                        </div>
                            <div class="col-lg-6">
                                <b>Maquiagem: </b> 
                                    <br />
                                    <div style="padding-left:10px;">
                                        <ul>
                                            <li>Olhos: <%# (Eval("FaceAttributes.Makeup.EyeMakeup").ToString().ToLower() == "true"? "Sim":"Não") %></li>
                                            
                                        </ul>
                                    </div>
                                <br />
                                 <b>Careca:</b> <%# (Double.Parse(Eval("FaceAttributes.Hair.Bald").ToString())*100).ToString() %> % 
                                <br />
                                 <b>Cabelo encoberto:</b> <%# (Eval("FaceAttributes.Hair.Invisible").ToString().ToLower() == "true"? "Sim":"Não") %>
                                <br />
                                <b>Sorriso:</b> <%# (Double.Parse(Eval("FaceAttributes.Smile").ToString())*100).ToString() %> %
                                
                            </div>
                        </div>
                        <hr />
                   </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
        <br /><br />
        <div id="divEstatistica" runat="server">
            <div class="row">
            <div class="col-lg-1">
                <img src="Content/icone-pessoas.jpg" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblQuantidade" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-lg-1">
                <img src="Content/icon-man.png" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblHomens" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-lg-1">
                <img src="Content/icon-woman.jpg" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblMulheres" runat="server" Font-Bold="true"></asp:Label>
            </div>
                <div class="col-lg-1">
                <img src="Content/icone-aniversario.jpg" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblAniversario" runat="server" Font-Bold="true"></asp:Label>
            </div>
        </div>
            <br /><br />
            <div class="row">
            <div class="col-lg-1">
                <img src="Content/icone-feliz.png" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblFeliz" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-lg-1">
                <img src="Content/icone-triste.png" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblTriste" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-lg-1">
                <img src="Content/icone-oculos.jpg" style="max-width:50px; height:auto;" />
            </div>
            <div class="col-lg-2 text-left" >
                <br />
                <asp:Label ID="lblOculos" runat="server" Font-Bold="true"></asp:Label>
            </div>
        </div>
        </div>
    </div>
    

</asp:Content>
