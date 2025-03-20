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
            // Verifica se a descri��o est� vazia ou cont�m apenas espa�os em branco
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha a descri��o.", "OK");
                return; // Interrompe a execu��o se a descri��o estiver vazia
            }

            // Verifica se a quantidade � zero ou inv�lida
            double quantidade;
            if (!double.TryParse(txt_quantidade.Text, out quantidade) || quantidade == 0)
            {
                await DisplayAlert("Erro", "A quantidade n�o pode ser zero.", "OK");
                return; // Interrompe a execu��o se a quantidade for zero
            }

            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = quantidade, // Aqui a quantidade foi validada
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Verifica se o pre�o � 0 e pergunta ao usu�rio se deseja manter como gratuito
            if (p.Preco == 0)
            {
                bool isGratis = await DisplayAlert(
                    "Produto Gratuito?",
                    "O pre�o est� definido como R$ 0,00. Deseja manter como gratuito?",
                    "Sim",
                    "N�o");

                if (!isGratis)
                {
                    await DisplayAlert("Erro", "Por favor, defina um pre�o maior que zero.", "OK");
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
