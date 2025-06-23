
using System.Windows;
using Microsoft.Data.SqlClient;
using prueba_tecnica_agroexport.DbConnections;
using prueba_tecnica_agroexport.Models;
using prueba_tecnica_agroexport.Pages.Orders;

namespace prueba_tecnica_agroexport
{
    /// <summary>
    /// Interaction logic for AddOrUpdateOrder.xaml
    /// </summary>
    public partial class AddOrUpdateOrder : Window
    {
        private readonly OrderViewControl _orderView;
        private readonly OrderModel _orderToEdit;
        private DbConnection db = new();
        private bool _isEditMode;
        public AddOrUpdateOrder(OrderViewControl orderView, string title, bool isEditMode, OrderModel orderToEdit = null)
        {
            InitializeComponent();
            TitleModal.Content = title;
            _orderView = orderView;
            _orderToEdit = orderToEdit;
            _isEditMode = isEditMode;
        }

        private async void AddOrUpdateOrder_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDropdownsAsync();
            if (_orderToEdit != null)
                await LoadDataToEdit();
        }

        private Task LoadDataToEdit()
        {
            QuantityTextBox.Text = _orderToEdit.ItemQuantity.ToString();
            ComboBoxClientes.SelectedValue = _orderToEdit.CustomerId;
            ComboBoxItems.SelectedValue = _orderToEdit.ItemId;
            return Task.CompletedTask;
        }

        private async Task LoadDropdownsAsync()
        {
            using SqlConnection connection = await db.Connection();
            string itemsQuery = "SELECT Id , Name  FROM Items";
            string customersQuery = "SELECT Id , FullName  FROM Customers";
            List<Item> itemsList = [];
            List<Customer> customersList = [];

            using var commandItems = new SqlCommand(itemsQuery, connection);
            using (var reader = await commandItems.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    itemsList.Add(new Item
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }


            using (var commandCustomers = new SqlCommand(customersQuery, connection))
            using (var reader = await commandCustomers.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    customersList.Add(new Customer
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    });
                }
            }

            ComboBoxItems.ItemsSource = itemsList;
            ComboBoxItems.DisplayMemberPath = "Name";
            ComboBoxItems.SelectedValuePath = "Id";

            ComboBoxClientes.ItemsSource = customersList;
            ComboBoxClientes.DisplayMemberPath = "FullName";
            ComboBoxClientes.SelectedValuePath = "Id";

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            QuantityTextBox.Text = "";
            ComboBoxItems.SelectedIndex = -1;
            ComboBoxClientes.SelectedIndex = -1;
            this.Close();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxItems.SelectedValue == null || ComboBoxClientes.SelectedValue == null || string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }
            int? id = _orderToEdit?.Id;
            int productoId = (int)ComboBoxItems.SelectedValue;
            int clienteId = (int)ComboBoxClientes.SelectedValue;
            string quantity = QuantityTextBox.Text;

            if (!int.TryParse(quantity, out int quantityValue
                ))
            {
                MessageBox.Show("Cantidad inválida. Debe ser un número.");
                return;
            }

            //guardar o editar en bd
            using SqlConnection connection = await db.Connection();
            if (_isEditMode)
            {
                string updateQuery = @"
                    UPDATE 
                        Orders 
                    SET ItemQuantity=@ItemQuantity, CustomerId=@CustomerId, ItemId=@ItemId
                    WHERE Id =@Id";
                using var command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@ItemQuantity", quantityValue);
                command.Parameters.AddWithValue("@ItemId", productoId);
                command.Parameters.AddWithValue("@CustomerId", clienteId);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                string insertQuery = @"
                INSERT INTO Orders (ItemQuantity, CreatedAt, Status, ItemId, CustomerId)
                VALUES (@ItemQuantity, @CreatedAt, @Status, @ItemId, @CustomerId)";
                using var command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@ItemQuantity", quantityValue);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@Status", "Pendiente");
                command.Parameters.AddWithValue("@ItemId", productoId);
                command.Parameters.AddWithValue("@CustomerId", clienteId);
                await command.ExecuteNonQueryAsync();
            }
            MainWindow mainWindow = new MainWindow();
            await _orderView.LoadDataAsync();
            this.DialogResult = true;
            this.Close();
        }


    }
}
