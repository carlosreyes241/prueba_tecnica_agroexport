using Microsoft.Data.SqlClient;
using prueba_tecnica_agroexport.DbConnections;
using System.Windows;
using System.Windows.Controls;

namespace prueba_tecnica_agroexport.Pages.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Ingresa el correo y la contraseña.");
                return;
            }

            using var connection = await new DbConnection().Connection();
            string query = "SELECT Id, FullName, Role FROM Users WHERE Email = @Email AND Password = @Password";
            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas.");
            }
        }
    }
}
