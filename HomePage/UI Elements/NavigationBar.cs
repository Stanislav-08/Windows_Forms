using App.Pages;
using App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace App.UI_Elements
{
    public partial class NavigationBar : UserControl
    {
        private static readonly SupabaseClient _supabase = new SupabaseClient();
        private string CurrentPage;

        public NavigationBar()
        {
            InitializeComponent();
            HomeButton.Image = Icons.Get("home",36);
            ProfileButton.Image = Icons.Get("profile",36);
            LogOutButton.Image = Icons.Get("log_out",36);
        }

        private void HomeButton_Click(object sender, EventArgs e) => NavigateTo("main");
        private void HomeLabel_Click(object sender, EventArgs e) => NavigateTo("main");

        private void ProfileButton_Click(object sender, EventArgs e) => NavigateTo("profile");
        private void ProfileLabel_Click(object sender, EventArgs e) => NavigateTo("profile");

        private async void LogOutLabel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Session.AccessToken != null)
                await _supabase.Logout(Session.AccessToken);

            if (result != DialogResult.Yes) return;
            Session.AccessToken = null;

            new HomePage().Show();
            FindForm()?.Hide();
        }

        private void NavigateTo(string page)
        {
            if (CurrentPage == page) return;
            CurrentPage = page;

            HomeButton.BackColor = SystemColors.Control;
            HomeLabel.BackColor = SystemColors.Control;
            ProfileButton.BackColor = SystemColors.Control;
            ProfileLabel.BackColor = SystemColors.Control;

            Form next;

            if (page == "main")
            {
                HomeButton.BackColor = SystemColors.ControlDark;
                HomeLabel.BackColor = SystemColors.ControlDark;
                next = new MainPage();
            }
            else
            {
                ProfileButton.BackColor = SystemColors.ControlDark;
                ProfileLabel.BackColor = SystemColors.ControlDark;
                next = new ProfilePage();
            }

            next.Show();
            FindForm()?.Hide();
        }
    }
}
