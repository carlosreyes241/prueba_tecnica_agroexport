using prueba_tecnica_agroexport.Models;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using prueba_tecnica_agroexport.DbConnections;

namespace prueba_tecnica_agroexport.Pages.Receptions
{
    /// <summary>
    /// Interaction logic for ReceptionViewControl.xaml
    /// </summary>
    public partial class ReceptionViewControl : UserControl
    {
        private DbConnection db = new();
        public ReceptionViewControl()
        {
            InitializeComponent();
        }

        private async void ReceptionViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            await this.LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            using SqlConnection connection = await db.Connection();

            string query = @"
                SELECT 
                    wr.Id,
                    wr.AverageWeight,
                    wr.PieceCount,
                    wr.TotalWeight,
                    wr.ReceptionDate,
                    s.Id AS SupplierId,
                    s.FullName,
                    s.SocialReason,
                    wt.Id AS WoodTypeId,
                    wt.Name AS WoodTypeName
                FROM WoodReception wr
                JOIN Suppliers s ON wr.SupplierId = s.Id
                JOIN WoodTypes wt ON wr.WoodTypeId = wt.Id";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();

            dataTable.Load(reader);

            ReceptionGrid.ItemsSource = dataTable.DefaultView;
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReceptionGrid.SelectedItem != null)
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
            if (ReceptionGrid.SelectedItem is DataRowView rowView)
            {
                ReceptionModel selectedReception = new()
                {
                    Id = Convert.ToInt32(rowView["Id"]),
                    SupplierId = Convert.ToInt32(rowView["SupplierId"]),
                    WoodTypeId = Convert.ToInt32(rowView["WoodTypeId"]),
                    AverageWeight = Convert.ToDecimal(rowView["AverageWeight"]),
                    PieceCount = Convert.ToInt32(rowView["PieceCount"]),
                    TotalWeight = Convert.ToDecimal(rowView["TotalWeight"]),
                    ReceptionDate = Convert.ToDateTime(rowView["ReceptionDate"])
                };

                AddOrUpdateReceptionForm form = new(this, $"Editar recepción #{selectedReception.Id}", true, selectedReception);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para editar.");
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceptionGrid.SelectedItem is DataRowView rowView)
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
            if (ReceptionGrid.ItemsSource is DataView dataView)
            {
                string search = SearchBox.Text.Trim().Replace("'", "''");

                dataView.RowFilter = $@"
                    SocialReason LIKE '%{search}%' 
                    OR FullName LIKE '%{search}%'
                    OR WoodTypeName LIKE '%{search}%'
                    OR CONVERT(ReceptionDate, 'System.String') LIKE '%{search}%' 
                    OR CONVERT(TotalWeight, 'System.String') LIKE '%{search}%' 
                    OR CONVERT(PieceCount, 'System.String') LIKE '%{search}%'";
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddOrUpdateReceptionForm addOrUpdateReceptionForm = new(this, "Agregar recepcion", false);
            addOrUpdateReceptionForm.ShowDialog();
        }

 
    }
}
