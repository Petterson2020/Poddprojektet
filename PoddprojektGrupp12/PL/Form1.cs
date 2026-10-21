using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Models;

namespace PL
{
    public partial class Form1 : Form
    {
        private readonly IPodcastController _podcastController;
        private readonly ICategoryController _categoryController;

        // Cache så att vi inte hämtar allt från databasen vid varje klick
        private List<Podcast> _podcasts = new();
        private List<Category> _categories = new();

        public Form1()
        {
            InitializeComponent();
            _podcastController = new PodcastController();
            _categoryController = new CategoryController();

            // Listorna innehåller riktiga objekt (med Id) men visar bara titel/namn
            listBox1.DisplayMember = nameof(Podcast.Title);
            listBox2.DisplayMember = nameof(Episode.Title);
            listBox3.DisplayMember = nameof(Category.Name);
            comboBox1.DisplayMember = nameof(Category.Name);
        }

        // ---------- Start ----------

        private async void Form1_Load(object? sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadPodcastsAsync();
        }

        // ---------- Hjälpmetoder: laddning och visning ----------

        private async Task LoadCategoriesAsync()
        {
            try
            {
                _categories = (await _categoryController.GetAllCategoriesAsync())
                    .OrderBy(c => c.Name).ToList();

                listBox3.Items.Clear();
                comboBox1.Items.Clear();
                foreach (Category category in _categories)
                {
                    listBox3.Items.Add(category);
                    comboBox1.Items.Add(category);
                }
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ladda kategorierna.", ex);
            }
        }

        // Laddar om ALLA poddar från databasen och nollställer filter/avsnittsvy
        private async Task LoadPodcastsAsync()
        {
            try
            {
                _podcasts = await _podcastController.GetAllPodcastsAsync();
                listBox3.ClearSelected();
                ShowPodcasts(_podcasts);
                ClearEpisodeView();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ladda poddarna.", ex);
            }
        }

        private void ShowPodcasts(IEnumerable<Podcast> podcasts)
        {
            listBox1.Items.Clear();
            foreach (Podcast podcast in podcasts.OrderBy(p => p.Title))
            {
                listBox1.Items.Add(podcast);
            }
        }

        private void ShowEpisodes(Podcast podcast)
        {
            listBox2.Items.Clear();
            textBox2.Clear();
            foreach (Episode episode in podcast.Episodes)
            {
                listBox2.Items.Add(episode);
            }
            groupBox1.Text = $"Avsnitt ({podcast.Episodes.Count} st)";
        }

        private void ClearEpisodeView()
        {
            listBox2.Items.Clear();
            textBox2.Clear();
            groupBox1.Text = "Avsnitt";
        }

        // ---------- Hjälpmetoder: validering och meddelanden ----------

        private static bool TryGetFeedUrl(string input, out string url)
        {
            url = input.Trim();
            return Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private static bool SameUrl(string? a, string? b) =>
            string.Equals(a?.Trim().TrimEnd('/'), b?.Trim().TrimEnd('/'), StringComparison.OrdinalIgnoreCase);

        private static bool Confirm(string text) =>
            MessageBox.Show(text, "Bekräfta borttagning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes;

        private static void ShowError(string message, Exception ex)
        {
            // Våra egna fel har användarvänliga meddelanden, övriga är oväntade
            string details = ex is PodcastAppException ? ex.Message : "Oväntat fel: " + ex.Message;
            MessageBox.Show(message + Environment.NewLine + details, "Fel",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ---------- Poddar ----------

        // Hämta-knappen: förhandsgranska flödet utan att spara
        private async void btnHamta_Click(object? sender, EventArgs e)
        {
            if (!TryGetFeedUrl(textBox1.Text, out string url))
            {
                MessageBox.Show("Ange en giltig webbadress som börjar med http:// eller https://.");
                return;
            }

            button7.Enabled = false;
            try
            {
                Podcast preview = await _podcastController.GetPodcastPreviewAsync(url);
                listBox1.ClearSelected();
                ShowEpisodes(preview);
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte hämta podden.", ex);
            }
            finally
            {
                button7.Enabled = true;
            }
        }

        // Spara podd
        private async void btnSavePodcast_Click(object? sender, EventArgs e)
        {
            if (!TryGetFeedUrl(textBox1.Text, out string url))
            {
                MessageBox.Show("Ange en giltig webbadress som börjar med http:// eller https://.");
                return;
            }

            if (comboBox1.SelectedItem is not Category category)
            {
                MessageBox.Show("Välj en kategori.");
                return;
            }

            if (_podcasts.Any(p => SameUrl(p.Url, url)))
            {
                MessageBox.Show("Du prenumererar redan på det här poddflödet.");
                return;
            }

            button1.Enabled = false;
            try
            {
                await _podcastController.AddPodcastAsync(url, category.Name, textBox4.Text.Trim());
                textBox1.Clear();
                textBox4.Clear();
                comboBox1.SelectedIndex = -1;
                await LoadPodcastsAsync();
                MessageBox.Show("Podden har lagts till!");
            }
            catch (Exception ex)
            {
                ShowError("Fel vid sparning.", ex);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        // Välj podd i listan -> visa dess avsnitt
        private void listBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Podcast podcast)
            {
                ShowEpisodes(podcast);
            }
        }

        // Välj avsnitt -> visa publiceringsdatum och beskrivning
        private void listBox2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBox2.SelectedItem is not Episode episode)
            {
                return;
            }

            try
            {
                // Tvätta bort HTML-taggar och avkoda t.ex. &amp;
                string description = Regex.Replace(episode.Description ?? "", "<.*?>", string.Empty);
                description = WebUtility.HtmlDecode(description);

                textBox2.Text = $"Publicerades: {episode.PublishDate}   |   Antal avsnitt i podden: {listBox2.Items.Count} st"
                    + Environment.NewLine + Environment.NewLine + description;
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte visa beskrivningen.", ex);
            }
        }

        // Ändra namn/kategori på vald podd
        private async void button5_Click(object? sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not Podcast podcast)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ändra den.");
                return;
            }

            string newTitle = textBox4.Text.Trim();
            string newCategory = (comboBox1.SelectedItem as Category)?.Name ?? "";

            if (newTitle == "" && newCategory == "")
            {
                MessageBox.Show("Skriv ett nytt namn i rutan eller välj en ny kategori för att ändra.");
                return;
            }

            try
            {
                await _podcastController.UpdatePodcastAsync(podcast.Id, newTitle, newCategory);
                textBox4.Clear();
                comboBox1.SelectedIndex = -1;
                await LoadPodcastsAsync();
                MessageBox.Show("Podden har uppdaterats!");
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ändra podden.", ex);
            }
        }

        // Ta bort podd
        private async void button2_Click(object? sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not Podcast podcast)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ta bort den.");
                return;
            }

            if (!Confirm($"Är du säker på att du vill ta bort podden '{podcast.Title}'?"))
            {
                return;
            }

            try
            {
                await _podcastController.DeletePodcastAsync(podcast.Id);
                await LoadPodcastsAsync();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ta bort podden.", ex);
            }
        }

        // Visa alla poddar (tar bort kategorifiltret)
        private async void button8_Click(object? sender, EventArgs e)
        {
            textBox3.Clear();
            await LoadPodcastsAsync();
        }

        // ---------- Kategorier ----------

        // Spara ny kategori
        private async void btnSaveCategory_Click(object? sender, EventArgs e)
        {
            string name = textBox3.Text.Trim();

            if (name == "")
            {
                MessageBox.Show("Du måste skriva ett namn på kategorin.");
                return;
            }

            if (_categories.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Det finns redan en kategori med det namnet.");
                return;
            }

            try
            {
                await _categoryController.CreateCategoryAsync(name);
                textBox3.Clear();
                await LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte spara kategorin.", ex);
            }
        }

        // Välj kategori -> fyll i namnrutan och filtrera poddlistan
        private void listBox3_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBox3.SelectedItem is not Category category)
            {
                return;
            }

            textBox3.Text = category.Name;
            ShowPodcasts(_podcasts.Where(p => p.Category == category.Name));
            ClearEpisodeView();
        }

        // Ändra kategorinamn
        private async void button3_Click(object? sender, EventArgs e)
        {
            if (listBox3.SelectedItem is not Category category)
            {
                MessageBox.Show("Välj en kategori i listan som du vill ändra.");
                return;
            }

            string newName = textBox3.Text.Trim();

            if (newName == "")
            {
                MessageBox.Show("Skriv in det nya namnet i textrutan (Namn:).");
                return;
            }

            if (_categories.Any(c => c.Id != category.Id
                && string.Equals(c.Name, newName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Det finns redan en kategori med det namnet.");
                return;
            }

            try
            {
                await _categoryController.UpdateCategoryAsync(category.Id, newName);
                textBox3.Clear();
                await LoadCategoriesAsync();
                await LoadPodcastsAsync(); // poddarnas kategorinamn har också ändrats
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ändra kategorin.", ex);
            }
        }

        // Ta bort kategori
        private async void button6_Click(object? sender, EventArgs e)
        {
            if (listBox3.SelectedItem is not Category category)
            {
                MessageBox.Show("Du måste markera en kategori i listan för att kunna ta bort den.");
                return;
            }

            if (!Confirm($"Är du säker på att du vill ta bort kategorin '{category.Name}'?"))
            {
                return;
            }

            try
            {
                await _categoryController.DeleteCategoryAsync(category.Id);
                textBox3.Clear();
                await LoadCategoriesAsync();
                await LoadPodcastsAsync();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ta bort kategorin.", ex);
            }
        }
    }
}