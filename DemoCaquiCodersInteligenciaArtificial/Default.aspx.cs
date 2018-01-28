using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoCaquiCodersInteligenciaArtificial
{
    public partial class _Default : Page
    {
        //Chave da Face API que pode ser obtida na sua conta no Portal do Azure
        private static string minha_chave = "SUA_CHAVE";

        //URL do serviço da API que pode ser obtida na sua conta no Portal do Azure
        private static string url_servico = "https://australiaeast.api.cognitive.microsoft.com/face/v1.0/detect";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                divCaracteristicas.Visible = false;
                divEstatistica.Visible = false;
            }
        }


        //Ação de upload da imagem
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            divCaracteristicas.Visible = false;
            divEstatistica.Visible = false;
            rptFaces.DataSource = null;
            rptFaces.DataBind();
            btnReconhecimentoFacial.Visible = true;

            if (fuFoto.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(fuFoto.FileName);
                    fuFoto.SaveAs(Server.MapPath("~/Content/Fotos/") + filename);

                    imgFoto.Src = "Content/Fotos/" + filename;
                    
                }
                catch (Exception ex)
                {
                   
                }
            }
        }

        protected  void btnReconhecimentoFacial_Click(object sender, EventArgs e)
        {
            divCaracteristicas.Visible = true;
            divEstatistica.Visible = true;
            MakeAnalysisRequest(Server.MapPath(imgFoto.Src), rptFaces, Server.MapPath("~/Content/Fotos"), lblQuantidade, lblHomens, lblMulheres, lblAniversario, lblOculos, lblFeliz, lblTriste);
        }

        static async void MakeAnalysisRequest(string caminho_fisico_arquivo, Repeater rpt, string diretorio, Label lblQuantidade, Label lblHomens, Label lblMulheres, Label lblAniversario, Label lblOculos, Label lblFeliz, Label lblTriste)
        {
            HttpClient client = new HttpClient();

            //Configuração das inforamções de Cabeçalho 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", minha_chave);

            //Especificações de informações adicionais ao cabeçalho referente à API.
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Especificação completo da URL do serviço com respectivos parâmetros
            string uri = url_servico + "?" + requestParameters;

            HttpResponseMessage response;

            // Transformando o arquivo físico em bytes para ser enviado ao serviço.

            byte[] bytes_imagem = GetArrayDaImagem(caminho_fisico_arquivo);

            using (ByteArrayContent content = new ByteArrayContent(bytes_imagem))
            {
                //Informações adicionais ao cabeçalho sobre o tipo de informações que trafegará.
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Fazendo chamada ao serviço
                response = await client.PostAsync(uri, content);

                // JSON retornado pela API.
                string contentString = await response.Content.ReadAsStringAsync();

                //Desserializando o JSON, populando um array do tipo, Face. Este array contém todos os rostos identificados 
                //serviço do Azure
                Face [] faces = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Face[]>(contentString);


                //Populando o controle do tipo Repeater do Asp.Net Webforms
                rpt.DataSource = faces;
                rpt.DataBind();
                              

                //Criando novas imagens, uma para da cada rosto identificado através das coordenadas
                //presentes no retorno do serviço.

                #region                 RECORTE DE IMAGENS

                Bitmap imagem_rosto = null;


                //Percorrendo o array/lista de rostos identificados
                foreach (Face face in faces)
                {
                    //Especificando um nome para a nova imagem através do identificador único do rosto
                    //retornado pelo serviço
                    var nome_imagem_rosto = face.FaceId + ".jpeg" as string;
                    
                    //Especificando o caminho completo para salvar a nova imagem "recortada"
                    var caminho_completo_nova_imagem = diretorio + '/' + nome_imagem_rosto as string;

                    //Posição do retângulo referente ao rosto identificado na imagem original
                    Rectangle rect = new Rectangle(face.FaceRectangle.Left, 
                                                   face.FaceRectangle.Top,
                                                   face.FaceRectangle.Width,
                                                   face.FaceRectangle.Height);


                    //Gerando a nova imagem com o rosto recortado a parti da posição do retângulo
                    imagem_rosto = ((Bitmap)System.Drawing.Image.FromFile(caminho_fisico_arquivo))
                                .Clone(rect, ((Bitmap)System.Drawing.Image.FromFile(caminho_fisico_arquivo)).PixelFormat);

                    //Salvando a imagem no diretório
                    imagem_rosto.Save(caminho_completo_nova_imagem, ImageFormat.Jpeg);

                    if (imagem_rosto != null)
                        ((IDisposable)imagem_rosto).Dispose();
                }

                #endregion

                #region ESTATISTICAS

                
                //Quantidade de homens identificados na imagem pelo serviço 
                int quantidade_homens = faces.Count(f => f.FaceAttributes.Gender == "male");

                //Quantidade de mulheres identificadas na imagem pelo serviço
                int quantidade_mulheres = faces.Count(f => f.FaceAttributes.Gender == "female");

                //Quantidade de pessoas na imagem identificadas como aprensentando sentimento feliz
                //O parâmetro para a apresentação foi o fator de 60
                int quantidade_feliz = faces.Count(f => f.FaceAttributes.Emotion.Happiness*100  > 60);

                //Quantidade de pessoas na imagem identificadas como apresentando sentimento triste
                //O parâmetro para a apresentação foi o fator de 60
                int quantidade_triste = faces.Count(f => f.FaceAttributes.Emotion.Sadness*100 > 60);

                //Calculando a média de idade de todas as pessoas identificadas na imagem
                double media_aniversarios = faces.Average(f => f.FaceAttributes.Age);
                
                //Número de pessoas identificadas na imagem que estavam usando óculos
                int quantidade_oculos = faces.Count(f => f.FaceAttributes.Glasses.ToString().ToLower() != "noglasses");

                lblQuantidade.Text = faces.Length.ToString() + " pessoa(s)";
               


                if (quantidade_homens == 0)
                {
                    lblHomens.Text = "nenhum homem";
                }
                else
                {
                    if (quantidade_homens == 1)
                    {
                        lblHomens.Text = "1 homem";
                    }
                    else
                    {
                        lblHomens.Text = quantidade_homens.ToString() +  " homens";
                    }
                }

                if (quantidade_mulheres == 0)
                {
                    lblMulheres.Text = "nenhuma mulher";
                }
                else
                {
                    if (quantidade_mulheres == 1)
                    {
                        lblMulheres.Text = "1 mulher";
                    }
                    else
                    {
                        lblMulheres.Text = quantidade_mulheres.ToString() + " mulheres";
                    }
                }

                lblAniversario.Text = "A média é de " + media_aniversarios.ToString() + " anos" ;

                //Formatação das informações para melhor exibição

                if (quantidade_feliz == 0)
                {
                    lblFeliz.Text = "ninguém feliz";
                }
                else
                {
                    if (quantidade_feliz == 1)
                    {
                        lblFeliz.Text = "1 feliz";
                    }
                    else
                    {
                        lblFeliz.Text = quantidade_feliz.ToString() + " felizes";
                    }
                }


                if (quantidade_triste == 0)
                {
                    lblTriste.Text = "ninguém triste";
                }
                else
                {
                    if (quantidade_triste == 1)
                    {
                        lblTriste.Text = "1 triste";
                    }
                    else
                    {
                        lblTriste.Text = quantidade_triste.ToString() + " tristes";
                    }
                }

                if (quantidade_oculos == 0)
                {
                    lblOculos.Text = "ninguém de óculos ";
                }
                else
                {
                    if (quantidade_oculos == 1)
                    {
                        lblOculos.Text = "1 de óculos";
                    }
                    else
                    {
                        lblOculos.Text = quantidade_oculos.ToString() + " de óculos";
                    }
                }


                #endregion

            }
        }

        static byte[] GetArrayDaImagem(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        protected void rptFaces_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblGenero = (Label) e.Item.FindControl("lblGenero");

            if (lblGenero.Text.Trim() == "male")
            {
                lblGenero.Text = "Masculino";
            }
            else
            {
                lblGenero.Text = "Feminino";
            }
        }
 
    }
}
