using System.Net;
using System.Text.RegularExpressions;
using BL;
using Models;

namespace PL
{
    public partial class Form1 : Form
    {
        private readonly IPodcastController _podcastController;
        private readonly ICategoryController _categoryController;

        private List<Podcast> _podcasts = new();
        private List<Category> _categories = new();

        public Form1()
        {
            InitializeComponent();
            _podcastController = new PodcastController();
            _categoryController = new CategoryController();

            lstPodcasts.DisplayMember = nameof(Podcast.Title);
            lstEpisodes.DisplayMember = nameof(Episode.Title);
            lstCategories.DisplayMember = nameof(Category.Name);
            cmbCategory.DisplayMember = nameof(Category.Name);
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadPodcastsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                _categories = (await _categoryController.GetAllCategoriesAsync())
                    .OrderBy(c => c.Name).ToList();

                lstCategories.Items.Clear();
                cmbCategory.Items.Clear();
                foreach (Category category in _categories)
                {
                    lstCategories.Items.Add(category);
                    cmbCategory.Items.Add(category);
                }
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ladda kategorierna.", ex);
            }
        }

        private async Task LoadPodcastsAsync()
        {
            try
            {
                _podcasts = await _podcastController.GetAllPodcastsAsync();
                lstCategories.ClearSelected();
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
            lstPodcasts.Items.Clear();
            foreach (Podcast podcast in podcasts.OrderBy(p => p.Title))
            {
                lstPodcasts.Items.Add(podcast);
            }
        }

        private void ShowEpisodes(Podcast podcast)
        {
            lstEpisodes.Items.Clear();
            txtEpisodeDescription.Clear();
            foreach (Episode episode in podcast.Episodes)
            {
                lstEpisodes.Items.Add(episode);
            }
            grpEpisodes.Text = $"Avsnitt ({podcast.Episodes.Count} st)";
        }

        private void ClearEpisodeView()
        {
            lstEpisodes.Items.Clear();
            txtEpisodeDescription.Clear();
            grpEpisodes.Text = "Avsnitt";
        }

        private static bool TryGetFeedUrl(string input, out string url)
        {
            url = input.Trim();
            bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (!isValid)
            {
                MessageBox.Show("Ange en giltig webbadress som börjar med http:// eller https://.");
            }

            return isValid;
        }

        private static bool SameUrl(string? a, string? b) =>
            string.Equals(a?.Trim().TrimEnd('/'), b?.Trim().TrimEnd('/'), StringComparison.OrdinalIgnoreCase);

        private static bool Confirm(string text) =>
            MessageBox.Show(text, "Bekräfta borttagning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes;

        private static void ShowError(string message, Exception ex)
        {
            string details = ex is PodcastAppException ? ex.Message : "Oväntat fel: " + ex.Message;
            MessageBox.Show(message + Environment.NewLine + details, "Fel",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void btnFetchPodcast_Click(object? sender, EventArgs e)
        {
            if (!TryGetFeedUrl(txtUrl.Text, out string url))
            {
                return;
            }

            btnFetchPodcast.Enabled = false;
            try
            {
                Podcast preview = await _podcastController.GetPodcastPreviewAsync(url);
                lstPodcasts.ClearSelected();
                ShowEpisodes(preview);
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte hämta podden.", ex);
            }
            finally
            {
                btnFetchPodcast.Enabled = true;
            }
        }

        private async void btnSavePodcast_Click(object? sender, EventArgs e)
        {
            if (!TryGetFeedUrl(txtUrl.Text, out string url))
            {
                return;
            }

            if (cmbCategory.SelectedItem is not Category category)
            {
                MessageBox.Show("Välj en kategori.");
                return;
            }

            if (_podcasts.Any(p => SameUrl(p.Url, url)))
            {
                MessageBox.Show("Du prenumererar redan på det här poddflödet.");
                return;
            }

            btnSavePodcast.Enabled = false;
            try
            {
                await _podcastController.AddPodcastAsync(url, category.Name, txtPodcastName.Text.Trim());
                txtUrl.Clear();
                txtPodcastName.Clear();
                cmbCategory.SelectedIndex = -1;
                await LoadPodcastsAsync();
                MessageBox.Show("Podden har lagts till!");
            }
            catch (Exception ex)
            {
                ShowError("Fel vid sparning.", ex);
            }
            finally
            {
                btnSavePodcast.Enabled = true;
            }
        }

        private void lstPodcasts_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstPodcasts.SelectedItem is Podcast podcast)
            {
                ShowEpisodes(podcast);
            }
        }

        private void lstEpisodes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstEpisodes.SelectedItem is not Episode episode)
            {
                return;
            }

            try
            {
                string description = Regex.Replace(episode.Description ?? "", "<.*?>", string.Empty);
                description = WebUtility.HtmlDecode(description);

                txtEpisodeDescription.Text = $"Publicerades: {episode.PublishDate}   |   Antal avsnitt i podden: {lstEpisodes.Items.Count} st"
                    + Environment.NewLine + Environment.NewLine + description;
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte visa beskrivningen.", ex);
            }
        }

        private async void btnUpdatePodcast_Click(object? sender, EventArgs e)
        {
            if (lstPodcasts.SelectedItem is not Podcast podcast)
            {
                MessageBox.Show("Du måste markera en podd i listan för att kunna ändra den.");
                return;
            }

            string newTitle = txtPodcastName.Text.Trim();
            string newCategory = (cmbCategory.SelectedItem as Category)?.Name ?? "";

            if (newTitle == "" && newCategory == "")
            {
                MessageBox.Show("Skriv ett nytt namn i rutan eller välj en ny kategori för att ändra.");
                return;
            }

            try
            {
                await _podcastController.UpdatePodcastAsync(podcast.Id, newTitle, newCategory);
                txtPodcastName.Clear();
                cmbCategory.SelectedIndex = -1;
                await LoadPodcastsAsync();
                MessageBox.Show("Podden har uppdaterats!");
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ändra podden.", ex);
            }
        }

        private async void btnDeletePodcast_Click(object? sender, EventArgs e)
        {
            if (lstPodcasts.SelectedItem is not Podcast podcast)
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

        private async void btnShowAllPodcasts_Click(object? sender, EventArgs e)
        {
            txtCategoryName.Clear();
            await LoadPodcastsAsync();
        }

        private async void btnSaveCategory_Click(object? sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();

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
                txtCategoryName.Clear();
                await LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte spara kategorin.", ex);
            }
        }

        private void lstCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstCategories.SelectedItem is not Category category)
            {
                return;
            }

            txtCategoryName.Text = category.Name;
            ShowPodcasts(_podcasts.Where(p => p.Category == category.Name));
            ClearEpisodeView();
        }

        private async void btnUpdateCategory_Click(object? sender, EventArgs e)
        {
            if (lstCategories.SelectedItem is not Category category)
            {
                MessageBox.Show("Välj en kategori i listan som du vill ändra.");
                return;
            }

            string newName = txtCategoryName.Text.Trim();

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
                txtCategoryName.Clear();
                await LoadCategoriesAsync();
                await LoadPodcastsAsync();
            }
            catch (Exception ex)
            {
                ShowError("Kunde inte ändra kategorin.", ex);
            }
        }

        private async void btnDeleteCategory_Click(object? sender, EventArgs e)
        {
            if (lstCategories.SelectedItem is not Category category)
            {
                MessageBox.Show("Du måste markera en kategori i listan för att kunna ta bort den.");
                return;
            }

            int podcastCount = _podcasts.Count(p => p.Category == category.Name);
            string question = $"Är du säker på att du vill ta bort kategorin '{category.Name}'?";
            if (podcastCount > 0)
            {
                question += $"{Environment.NewLine}{podcastCount} podd(ar) i kategorin blir okategoriserade.";
            }

            if (!Confirm(question))
            {
                return;
            }

            try
            {
                await _categoryController.DeleteCategoryAsync(category.Id);
                txtCategoryName.Clear();
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