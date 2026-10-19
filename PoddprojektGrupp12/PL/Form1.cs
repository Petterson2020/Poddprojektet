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

        // Uppdaterar den stora poddlistan (listBox1)
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
            UpdatePodcastList();
        }

        // Spara Kategori
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

        // Spara Podd
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

                // Hämta det egna namnet från din nya "Namn"-ruta (troligen textBox4)
                string egetNamn = "";
                if (textBox4 != null)
                {
                    egetNamn = textBox4.Text;
                }

                // Skicka med "egetNamn" till Controllern
                await _podcastController.AddPodcastAsync(textBox1.Text, selectedCategory, egetNamn);

                MessageBox.Show("Podden har lagts till!");

                // Rensa båda fälten efter sparning
                textBox1.Clear();
                if (textBox4 != null) textBox4.Clear();

                UpdatePodcastList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fel vid sparning: " + ex.Message);
            }
        }

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

                        // FIX: Uppdaterar groupBox1 istället för label2 så antalet hamnar rätt
                        label2.Text = "Kategorier:";
                        groupBox1.Text = $"Avsnitt ({_currentEpisodes.Count} st)";

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

                    // Tvättar bort HTML-taggar
                    string cleanDescription = Regex.Replace(selectedEpisode.Description, "<.*?>", string.Empty);
                    cleanDescription = System.Net.WebUtility.HtmlDecode(cleanDescription);

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

        // Ta bort Kategori
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

        // Ändra Podd
        private async void button5_Click(object sender, EventArgs e)
        {
            // Kolla om en podd är vald
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ändra den.");
                return;
            }

            string gammalTitel = listBox1.SelectedItem.ToString();

            // Vi hämtar det nya namnet från vår nyskapade ruta textBox4!
            string nyTitel = "";
            if (textBox4 != null)
            {
                nyTitel = textBox4.Text;
            }

            string nyKategori = "";
            if (comboBox1.SelectedIndex != -1)
            {
                nyKategori = comboBox1.SelectedItem.ToString();
            }

            // Kolla så de fyllt i antingen nytt namn eller ny kategori
            if (string.IsNullOrWhiteSpace(nyTitel) && string.IsNullOrWhiteSpace(nyKategori))
            {
                MessageBox.Show("Skriv ett nytt namn i rutan eller välj en ny kategori för att ändra.");
                return;
            }

            try
            {
                await _podcastController.UpdatePodcastAsync(gammalTitel, nyTitel, nyKategori);
                MessageBox.Show("Podden har uppdaterats!");

                if (textBox4 != null) textBox4.Clear();
                comboBox1.SelectedIndex = -1;

                // Ladda om listan så det nya namnet syns!
                UpdatePodcastList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte ändra podden: " + ex.Message);
            }
        }

        // Ändra Kategori
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

        // Filtrera poddar på kategori
        private async void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                string valdKategori = listBox3.SelectedItem.ToString();
                textBox3.Text = valdKategori;

                try
                {
                    var allPodcasts = await _podcastController.GetAllPodcastsAsync();
                    listBox1.Items.Clear();

                    if (allPodcasts != null)
                    {
                        foreach (var podcast in allPodcasts)
                        {
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

        // Ta bort podd
        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ta bort den.");
                return;
            }

            string valdPodd = listBox1.SelectedItem.ToString();
            DialogResult dialogResult = MessageBox.Show(
                $"Är du säker på att du vill ta bort podden '{valdPodd}'?",
                "Bekräfta borttagning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    await _podcastController.DeletePodcastAsync(valdPodd);
                    UpdatePodcastList();

                    listBox2.Items.Clear();
                    textBox2.Clear();
                    groupBox1.Text = "Avsnitt";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunde inte ta bort podden: " + ex.Message);
                }
            }
        }

        // Tomma metoder (Behålls för att inte förstöra event-kopplingar i Designern)
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox1_Enter_1(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged_1(object sender, EventArgs e) { }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Anropa din befintliga metod för att ladda in ALLA poddar igen
                UpdatePodcastList();

                // 2. Avmarkera den valda kategorin i listan nere till höger, 
                // så att det är tydligt att inget filter är aktivt just nu.
                listBox3.SelectedIndex = -1;

                // 3. Töm textrutan för "Nytt namn" under kategorier
                textBox3.Clear();

                // 4. Städa upp avsnittslistan och beskrivningen (eftersom ingen podd är vald längre)
                listBox2.Items.Clear();
                textBox2.Clear();
                groupBox1.Text = "Avsnitt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod när poddarna skulle hämtas: " + ex.Message);
            }
        }
    }
}