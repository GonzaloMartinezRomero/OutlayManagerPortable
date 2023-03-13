using OutlayManagerPortableMaui.Models.TransactionModelView;
using OutlayManagerPortableMaui.ViewModels;

namespace OutlayManagerPortableMaui;

public partial class TransactionPage : ContentPage
{
    private readonly TransactionViewModel transactionViewModel = new TransactionViewModel();
    private readonly TransactionOutlayModelView transactionOutlayView = new TransactionOutlayModelView() { Date = DateTime.Today.Date };

    public TransactionPage()
	{
		InitializeComponent();

        this.DeleteButton.IsEnabled = false;

        this.MainContentPage.Appearing += LoadMasterDataInViewAsync;
    }

    public TransactionPage(TransactionOutlayModelView transactionOutlay) : this()
    {
        transactionOutlayView = transactionOutlay;

        this.DeleteButton.IsEnabled = true;
    }

    private async void LoadMasterDataInViewAsync(object sender, EventArgs e)
    {
        try
        {
            ShowLoadingView();

            this.TransactionTypeSelector.ItemsSource = transactionViewModel.TransactionTypesViewModel();
            this.TransactionCodeSelector.ItemsSource = transactionViewModel.TransactionCodesViewModel();

            BindingContext = transactionOutlayView;
        }
        catch (Exception except)
        {
            await DisplayAlert("Transaction", except.Message, "Ok");
        }
        finally
        {
            HideLoadingView();
        }
    }  

    private async void SaveTransactionEvent(object sender, EventArgs e)
    {
        try
        {
            ShowLoadingView();
            await transactionViewModel.SaveTransactionAsync(transactionOutlayView);

        }
        catch (Exception except)
        {
            await DisplayAlert("Transaction", except.Message, "Ok");
        }
        finally
        {
            _ = await this.Navigation.PopAsync(animated: true);
        }
    }

    private async void DeleteTransactionEvent(object sender, EventArgs e)
    {
        try
        {
            ShowLoadingView();

            await transactionViewModel.DeleteTransaction(transactionOutlayView.Id);
        }
        catch (Exception except)
        {
            await DisplayAlert("Transaction", except.Message, "Ok");
        }
        finally
        {
            _ = await this.Navigation.PopAsync(animated: true);
        }
    }

    private void ShowLoadingView()
    {
        mainStackLayout.IsVisible = false;
        loadingScreen.IsVisible = true;
        loadingIndicador.IsRunning = true;
        loadingIndicador.IsVisible = true;
    }

    private void HideLoadingView()
    {
        mainStackLayout.IsVisible = true;
        loadingScreen.IsVisible = false;
        loadingIndicador.IsRunning = false;
        loadingIndicador.IsVisible = false;
    }
}