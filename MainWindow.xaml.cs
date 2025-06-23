using System.Windows;
using prueba_tecnica_agroexport.Pages.Login;
using prueba_tecnica_agroexport.Pages.Orders;
using prueba_tecnica_agroexport.Pages.Receptions;



namespace prueba_tecnica_agroexport;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new OrderViewControl();
    }

    private void Orders_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new OrderViewControl();
    }

    private void Recepction_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ReceptionViewControl();
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        LoginWindow login = new();
        login.Show();
        this.Close();
    }
}
