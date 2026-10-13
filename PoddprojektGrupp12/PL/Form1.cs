using System;
using System.Windows.Forms;
using BL;
using Models;

namespace PL
{
    public partial class Form1 : Form
    {
        private readonly IPodcastController _podcastController;
        private readonly ICategoryController _categoryController;

        public Form1()
        {
            InitializeComponent();
            _podcastController = new PodcastController();
            _categoryController = new CategoryController();
        }

        private async void UpdateCategoryLists()
        {
            try
            {
                // Rensa listBox3 (Kategorilistan) och comboBox1 (Rullgardinsmenyn)
                listBox3.Items.Clear();
                comboBox1.Items.Clear();

                var categories = await _categoryController.GetAllCategoriesAsync();

                foreach (var category in categories)
                {
                    listBox3.Items.Add(category.Name);
                    comboBox1.Items.Add(category.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod när kategorierna skulle laddas: " + ex.Message);
            }
        }

        // Ladda in listorna direkt när programmet startar
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateCategoryLists();
        }

        // Koden för att Spara Kategori
        private async void btnSaveCategory_Click(object sender, EventArgs e)
        {
            // textBox2 är antagligen din inmatningsruta för Kategorier
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Du måste skriva ett namn på kategorin.");
                return;
            }

            try
            {
                await _categoryController.CreateCategoryAsync(textBox2.Text);
                textBox2.Clear();
                UpdateCategoryLists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte spara: " + ex.Message);
            }
        }

        // Koden för att Spara Podd
        private async void btnSavePodcast_Click(object sender, EventArgs e)
        {
            // textBox1 är din URL-ruta och comboBox1 är din rullgardinsmeny
            if (string.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vänligen ange en URL och välj en kategori.");
                return;
            }

            try
            {
                string selectedCategory = comboBox1.SelectedItem.ToString();
                await _podcastController.AddPodcastAsync(textBox1.Text, selectedCategory);
                MessageBox.Show("Podden har lagts till!");
                textBox1.Clear();
                // Uppdatera listbox1 (Poddlistan) här senare
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid sparning: " + ex.Message);
            }
        }

        // --- Dina tomma metoder (Rör ej, behövs för designen) ---
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}