using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verificação da descrição antes de tentar criar o produto
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                throw new Exception("Por favor, preencha a descrição.");
            }

            // Criação do objeto Produto
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Verifica se o preço é 0 e pergunta ao usuário
            if (p.Preco == 0)
            {
                bool isGratis = await DisplayAlert(
                    "Produto Gratuito?",
                    "O preço está definido como R$ 0,00. Deseja manter como gratuito?",
                    "Sim",
                    "Não");

                // Se o usuário não quiser manter o produto gratuito, lança uma exceção
                if (!isGratis)
                {
                    throw new Exception("Por favor, defina um preço maior que zero.");
                }
            }

            // Insere o produto no banco de dados
            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
