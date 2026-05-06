using App.Pages;
using App.Services;

namespace App.UI_Elements
{
    public partial class NavigationBar : UserControl
    {
        private static AuthenticationService AuthenticationService = new AuthenticationService();
        private string CurrentPage;

        public NavigationBar(string activePage)
        {
            InitializeComponent();

            HomeButton.Image = Icons.Get("home", 36);
            ProfileButton.Image = Icons.Get("profile", 36);
            LogOutButton.Image = Icons.Get("log_out", 36);
            CurrentPage = activePage;
            SetActive(activePage);

            // Hide logout for guests
            LogOutButton.Visible = Session.IsLoggedIn;
            LogOutLabel.Visible = Session.IsLoggedIn;

            // Hide profile for guests
            ProfileButton.Visible = Session.IsLoggedIn;
            ProfileLabel.Visible = Session.IsLoggedIn;
        }

        //-----------Navigation functions-----------

        private void HomeButton_Click(object sender, EventArgs e) => NavigateTo("main");
        private void HomeLabel_Click(object sender, EventArgs e) => NavigateTo("main");
        private void ProfileButton_Click(object sender, EventArgs e) => NavigateTo("profile");
        private void ProfileLabel_Click(object sender, EventArgs e) => NavigateTo("profile");

        //-----------Logout functions-----------

        private async void LogOutLabel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Log Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            if (Session.AccessToken != null)
            {
                await AuthenticationService.Logout(Session.AccessToken);
            }

            Session.AccessToken = null;
            new HomePage().Show();
            FindForm()?.Hide();
        }

        //-----------Set active page functions-----------

        private void SetActive(string page)
        {
            HomeButton.BackColor = Color.FromArgb(28, 30, 54);
            HomeLabel.BackColor = Color.FromArgb(28, 30, 54);
            ProfileButton.BackColor = Color.FromArgb(28, 30, 54);
            ProfileLabel.BackColor = Color.FromArgb(28, 30, 54);

            if (page == "main")
            {
                HomeButton.BackColor = SystemColors.ControlDark;
                HomeLabel.BackColor = SystemColors.ControlDark;
            }
            else
            {
                ProfileButton.BackColor = SystemColors.ControlDark;
                ProfileLabel.BackColor = SystemColors.ControlDark;
            }
        }

        private void NavigateTo(string page)
        {
            if (CurrentPage == page) return;
            CurrentPage = page;
            SetActive(page);
            Form next = page == "main" ? new MainPage() : new ProfilePage();
            next.Show();
            FindForm()?.Hide();
        }
    }
}