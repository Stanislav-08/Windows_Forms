using App.UI_Elements;

namespace App.Pages
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();

            //Title bar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);

            //Search bar
            SearchBar searchBar = new SearchBar(new Point(300, 0), 650);
            searchBar.Location = new Point(175, 300);
            Controls.Add(searchBar);
        }

        //----------Login button----------

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            Hide();
        }

        //----------Register button----------

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            Hide();
        }
    }
}