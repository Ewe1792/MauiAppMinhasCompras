using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica se a descrição está vazia ou contém apenas espaços em branco
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha a descrição.", "OK");
                return; // Interrompe a execução se a descrição estiver vazia
            }

            // Verifica se a quantidade é zero ou inválida
            double quantidade;
            if (!double.TryParse(txt_quantidade.Text, out quantidade) || quantidade == 0)
            {
                await DisplayAlert("Erro", "A quantidade não pode ser zero.", "OK");
                return; // Interrompe a execução se a quantidade for zero
            }

            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = quantidade, // Aqui a quantidade foi validada
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Verifica se o preço é 0 e pergunta ao usuário se deseja manter como gratuito
            if (p.Preco == 0)
            {
                bool isGratis = await DisplayAlert(
                    "Produto Gratuito?",
                    "O preço está definido como R$ 0,00. Deseja manter como gratuito?",
                    "Sim",
                    "Não");

                if (!isGratis)
                {
                    await DisplayAlert("Erro", "Por favor, defina um preço maior que zero.", "OK");
                    return;
                }
            }

            // Atualiza o produto no banco de dados
            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Produto atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
