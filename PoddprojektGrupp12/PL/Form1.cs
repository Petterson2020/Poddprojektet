using System;
using System.Windows.Forms;
using BL;
using Models;

namespace PL
{
    public partial class Form1 : Form
    {
        // Skapa variabler för dina controllers
        private readonly IPodcastController _podcastController;
        private readonly ICategoryController _categoryController;

        public Form1()
        {
            InitializeComponent();

            // Initiera dem när programmet startar
            _podcastController = new PodcastController();
            _categoryController = new CategoryController();
        }

        // Metoden måste ligga HÄR INNE, inuti klassen Form1
        private async void UpdateCategoryLists()
        {
            try
            {
                // Rensa listorna så vi inte får dubbletter
                lstCategories.Items.Clear();
                cbCategoryDropdown.Items.Clear();

                // Hämta listan från BL
                var categories = await _categoryController.GetAllCategoriesAsync();

                // Lägg in dem i gränssnittet
                foreach (var category in categories)
                {
                    lstCategories.Items.Add(category.Name);
                    cbCategoryDropdown.Items.Add(category.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod när kategorierna skulle laddas: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    } // HÄR stängs Form1-klassen
} // HÄR stängs namespace PL