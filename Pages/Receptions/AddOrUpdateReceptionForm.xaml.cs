using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.SqlClient;
using prueba_tecnica_agroexport.DbConnections;
using prueba_tecnica_agroexport.Models;
using prueba_tecnica_agroexport.Pages.Receptions;

namespace prueba_tecnica_agroexport.Pages.Receptions
{
    /// <summary>
    /// Interaction logic for AddOrUpdateReceptionForm.xaml
    /// </summary>
    public partial class AddOrUpdateReceptionForm : Window
    {
        private readonly ReceptionViewControl _receptionView;
        private readonly ReceptionModel _receptionToEdit;
        private readonly bool _isEditMode;
        private readonly DbConnection db = new();
        public AddOrUpdateReceptionForm(ReceptionViewControl receptionView, string title, bool isEditMode, ReceptionModel receptionToEdit = null)
        {    
            InitializeComponent();
            TitleModal.Content = title;
            _receptionView = receptionView;
            _receptionToEdit = receptionToEdit;
            _isEditMode = isEditMode;
        }

        private async void AddOrUpdateOrder_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDropdownsAsync();
            if (_receptionToEdit != null)
                await LoadDataToEdit();
        }

        private async Task LoadDropdownsAsync()
        {
            using SqlConnection connection = await db.Connection();

            string supplierQuery = "SELECT Id, FullName FROM Suppliers";
            string woodTypeQuery = "SELECT Id, Name FROM WoodTypes";

            List<Supplier> suppliers = [];
            List<WoodType> woodTypes = [];

            using (var cmd = new SqlCommand(supplierQuery, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    suppliers.Add(new Supplier
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    });
                }
            }

            using (var cmd = new SqlCommand(woodTypeQuery, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    woodTypes.Add(new WoodType
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            ComboBoxSupplier.ItemsSource = suppliers;
            ComboBoxSupplier.DisplayMemberPath = "FullName";
            ComboBoxSupplier.SelectedValuePath = "Id";

            ComboBoxWoodType.ItemsSource = woodTypes;
            ComboBoxWoodType.DisplayMemberPath = "Name";
            ComboBoxWoodType.SelectedValuePath = "Id";
        }

        private Task LoadDataToEdit()
        {
            ComboBoxSupplier.SelectedValue = _receptionToEdit.SupplierId;
            ComboBoxWoodType.SelectedValue = _receptionToEdit.WoodTypeId;
            QuantityTextBox.Text = _receptionToEdit.AverageWeight.ToString("F2");
            PieceCountTextBox.Text = _receptionToEdit.PieceCount.ToString();
            TotalWeightTexbox.Text = _receptionToEdit.TotalWeight.ToString("F2");
            ReceptionDatePicker.SelectedDate = _receptionToEdit.ReceptionDate;

            return Task.CompletedTask;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxSupplier.SelectedIndex = -1;
            ComboBoxWoodType.SelectedIndex = -1;
            QuantityTextBox.Text = "";
            PieceCountTextBox.Text = "";
            TotalWeightTexbox.Text = "";
            ReceptionDatePicker.SelectedDate = null;
            this.Close();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones
            if (ComboBoxSupplier.SelectedValue == null || ComboBoxWoodType.SelectedValue == null ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(PieceCountTextBox.Text) ||
                string.IsNullOrWhiteSpace(TotalWeightTexbox.Text) ||
                ReceptionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            if (!decimal.TryParse(QuantityTextBox.Text, out decimal avgWeight) ||
                !int.TryParse(PieceCountTextBox.Text, out int pieceCount) ||
                !decimal.TryParse(TotalWeightTexbox.Text, out decimal totalWeight))
            {
                MessageBox.Show("Verifica los campos numéricos.");
                return;
            }

            int supplierId = (int)ComboBoxSupplier.SelectedValue;
            int woodTypeId = (int)ComboBoxWoodType.SelectedValue;
            DateTime receptionDate = ReceptionDatePicker.SelectedDate ?? DateTime.Now;

            using SqlConnection connection = await db.Connection();

            if (_isEditMode)
            {
                string updateQuery = @"
                    UPDATE WoodReception
                    SET SupplierId=@SupplierId, AverageWeight=@AverageWeight, PieceCount=@PieceCount, 
                        WoodTypeId=@WoodTypeId, TotalWeight=@TotalWeight, ReceptionDate=@ReceptionDate
                    WHERE Id=@Id";

                using var cmd = new SqlCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("@Id", _receptionToEdit.Id);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                cmd.Parameters.AddWithValue("@AverageWeight", avgWeight);
                cmd.Parameters.AddWithValue("@PieceCount", pieceCount);
                cmd.Parameters.AddWithValue("@WoodTypeId", woodTypeId);
                cmd.Parameters.AddWithValue("@TotalWeight", totalWeight);
                cmd.Parameters.AddWithValue("@ReceptionDate", receptionDate);
                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                string insertQuery = @"
                    INSERT INTO WoodReception 
                    (SupplierId, AverageWeight, PieceCount, WoodTypeId, TotalWeight, ReceptionDate)
                    VALUES (@SupplierId, @AverageWeight, @PieceCount, @WoodTypeId, @TotalWeight, @ReceptionDate)";

                using var cmd = new SqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                cmd.Parameters.AddWithValue("@AverageWeight", avgWeight);
                cmd.Parameters.AddWithValue("@PieceCount", pieceCount);
                cmd.Parameters.AddWithValue("@WoodTypeId", woodTypeId);
                cmd.Parameters.AddWithValue("@TotalWeight", totalWeight);
                cmd.Parameters.AddWithValue("@ReceptionDate", receptionDate);
                await cmd.ExecuteNonQueryAsync();
            }

            await _receptionView.LoadDataAsync();
            this.DialogResult = true;
            this.Close();
        }
    }
}

