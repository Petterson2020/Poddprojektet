using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions; // För att tvätta bort HTML-taggar
using BL;
using Models;

namespace PL
{
    public partial class Form1 : Form
    {
        private readonly IPodcastController _podcastController;
        private readonly ICategoryController _categoryController;

        // Vårt arbetsminne för att komma ihåg avsnitten som visas just nu
        private List<Episode> _currentEpisodes = new List<Episode>();

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

        // NY METOD: Uppdaterar den stora poddlistan (listBox1)
        private async void UpdatePodcastList()
        {
            try
            {
                listBox1.Items.Clear();
                var podcasts = await _podcastController.GetAllPodcastsAsync();

                if (podcasts != null)
                {
                    foreach (var podcast in podcasts)
                    {
                        listBox1.Items.Add(podcast.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte ladda poddar: " + ex.Message);
            }
        }

        // Ladda in listorna direkt när programmet startar
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateCategoryLists();
            UpdatePodcastList(); // Laddar in dina sparade poddar direkt!
        }

        // Koden för att Spara Kategori
        private async void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Du måste skriva ett namn på kategorin.");
                return;
            }

            try
            {
                await _categoryController.CreateCategoryAsync(textBox3.Text);
                textBox3.Clear();
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

                // Uppdaterar listan direkt så den nya podden dyker upp!
                UpdatePodcastList();
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
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        // När man klickar på en podd i listan
        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    string selectedTitle = listBox1.SelectedItem.ToString();
                    var allPodcasts = await _podcastController.GetAllPodcastsAsync();

                    var selectedPodcast = System.Linq.Enumerable.FirstOrDefault(allPodcasts, p => p.Title == selectedTitle);

                    listBox2.Items.Clear();
                    textBox2.Clear();

                    if (selectedPodcast != null && selectedPodcast.Episodes != null)
                    {
                        _currentEpisodes = selectedPodcast.Episodes;

                        // NY KOD: Uppdaterar label2 så den visar antal avsnitt!
                        label2.Text = $"Avsnitt ({_currentEpisodes.Count} st)";

                        foreach (var episode in _currentEpisodes)
                        {
                            listBox2.Items.Add(episode.Title);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte ladda avsnitt: " + ex.Message);
                }
            }
        }

        // När man klickar på ett avsnitt
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && _currentEpisodes != null)
            {
                try
                {
                    var selectedEpisode = _currentEpisodes[listBox2.SelectedIndex];

                    // 1. Tvättar bort HTML-taggar (som <div> och <p>)
                    string cleanDescription = Regex.Replace(selectedEpisode.Description, "<.*?>", string.Empty);

                    // 2. Översätter HTML-koder (som &amp; till & och &nbsp; till mellanslag)
                    cleanDescription = System.Net.WebUtility.HtmlDecode(cleanDescription);

                    // NY KOD: Visar publiceringsdatumet tillsammans med den rena texten i beskrivningsrutan
                    textBox2.Text = $"Publicerades: {selectedEpisode.PublishDate}   |   Antal avsnitt i podden: {_currentEpisodes.Count} st\r\n\r\n{cleanDescription}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte ladda beskrivningen: " + ex.Message);
                }
            }
        }

        // Hämta-knappen
        private async void btnHamta_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Du måste skriva in en länk först!");
                return;
            }

            try
            {
                var podcastPreview = await _podcastController.GetPodcastPreviewAsync(url);

                listBox2.Items.Clear();
                textBox2.Clear();

                if (podcastPreview.Episodes != null)
                {
                    _currentEpisodes = podcastPreview.Episodes;

                    foreach (var episode in _currentEpisodes)
                    {
                        listBox2.Items.Add(episode.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte hämta podden: " + ex.Message);
            }

        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Du måste markera en kategori i listan för att kunna ta bort den.");
                return;
            }

            string valdKategori = listBox3.SelectedItem.ToString();

            DialogResult dialogResult = MessageBox.Show(
                $"Är du säker på att du vill ta bort kategorin '{valdKategori}'?",
                "Bekräfta borttagning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    await _categoryController.DeleteCategoryAsync(valdKategori);
                    UpdateCategoryLists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte ta bort kategorin: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Välj en kategori i listan som du vill ändra.");
                return;
            }

            string oldName = listBox3.SelectedItem.ToString();
            string newName = textBox3.Text;

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Skriv in det nya namnet i textrutan (Namn:).");
                return;
            }

            try
            {
                await _categoryController.UpdateCategoryAsync(oldName, newName);
                textBox3.Clear();
                UpdateCategoryLists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte ändra kategorin: " + ex.Message);
            }
        }

        // Metoden för filtrering
        private async void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                string valdKategori = listBox3.SelectedItem.ToString();

                // Fyller i namnet i textrutan så det är redo om användaren vill trycka "Ändra"
                textBox3.Text = valdKategori;

                try
                {
                    // Hämta alla poddar
                    var allPodcasts = await _podcastController.GetAllPodcastsAsync();

                    // Rensa den stora poddlistan till vänster
                    listBox1.Items.Clear();

                    // Lägg BARA till de poddar som matchar den valda kategorin
                    if (allPodcasts != null)
                    {
                        foreach (var podcast in allPodcasts)
                        {
                            // Om din databasmodell använder ett annat namn än "Category" (t.ex. CategoryName), ändra det här.
                            if (podcast.Category == valdKategori)
                            {
                                listBox1.Items.Add(podcast.Title);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte filtrera poddar: " + ex.Message);
                }
            }
        }

        // Ta bort podd-knappen
        private async void button2_Click(object sender, EventArgs e)
        {
            // 1. Kolla att användaren har markerat en podd i den stora listan
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ta bort den.");
                return;
            }

            // 2. Hämta titeln på den markerade podden
            string valdPodd = listBox1.SelectedItem.ToString();

            // 3. Fråga om användaren är säker
            DialogResult dialogResult = MessageBox.Show(
                $"Är du säker på att du vill ta bort podden '{valdPodd}'?",
                "Bekräfta borttagning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    // 4. Säg åt logiken att ta bort podden
                    await _podcastController.DeletePodcastAsync(valdPodd);

                    // 5. Uppdatera listan så att podden försvinner från skärmen
                    UpdatePodcastList();

                    // 6. Rensa avsnittslistan och beskrivningen (eftersom podden inte finns längre)
                    listBox2.Items.Clear();
                    textBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte ta bort podden: " + ex.Message);
                }
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e) { }
        private void groupBox1_Enter_1(object sender, EventArgs e) { }
    }
}