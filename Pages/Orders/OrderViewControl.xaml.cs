using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using prueba_tecnica_agroexport.DbConnections;
using prueba_tecnica_agroexport.Models;


namespace prueba_tecnica_agroexport.Pages.Orders
{
    /// <summary>
    /// Interaction logic for OrderViewControl.xaml
    /// </summary>
    public partial class OrderViewControl : UserControl
    {
        private DbConnection db = new();
        public OrderViewControl()
        {
            InitializeComponent();
        }


        private async void OrderViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            await this.LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            using SqlConnection connection = await db.Connection();

            string query = @"
            SELECT 
                o.Id,
                o.ItemQuantity,
                o.CreatedAt,
                o.DeliveryDate,
                o.Status,
                i.Id AS ItemId,
                i.Name AS ItemName,
                c.Id AS CustomerId,
                c.Rfc,
                c.FullName AS CustomerName,
                c.Address AS CustomerAddress
            FROM Orders o
            JOIN Customers c ON o.CustomerId = c.Id
            JOIN Items i ON o.ItemId = i.Id";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();

            dataTable.Load(reader);

            ProductosGrid.ItemsSource = dataTable.DefaultView;
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductosGrid.SelectedItem != null)
            {
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosGrid.SelectedItem is DataRowView rowView)
            {
                OrderModel selectedOrder = new()
                {
                    Id = Convert.ToInt32(rowView["Id"]),
                    ItemQuantity = Convert.ToInt32(rowView["ItemQuantity"]),
                    ItemId = Convert.ToInt32(rowView["ItemId"]),
                    CustomerId = Convert.ToInt32(rowView["CustomerId"])
                };
                AddOrUpdateOrder addOrUpdateOrder = new(this, $"Editar orden {selectedOrder.Id}", true, selectedOrder);
                addOrUpdateOrder.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para editar.");
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosGrid.SelectedItem is DataRowView rowView)
            {
                var result = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este registro?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    int orderId = Convert.ToInt32(rowView["Id"]);
                    using SqlConnection connection = await db.Connection();
                    var deleteQuery = "DELETE FROM Orders Where Id=@OrderId";
                    using var command = new SqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    await command.ExecuteNonQueryAsync();
                    await this.LoadDataAsync();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para editar.");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProductosGrid.ItemsSource is DataView dataView)
            {
                string search = SearchBox.Text.Trim().Replace("'", "''");
                dataView.RowFilter = $"ItemName LIKE '%{search}%' OR CustomerName LIKE '%{search}%' OR Rfc LIKE '%{search}%'";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddOrUpdateOrder addOrUpdateOrder = new(this, "Agregar orden nueva", false);
            addOrUpdateOrder.ShowDialog();
        }
    }
}
