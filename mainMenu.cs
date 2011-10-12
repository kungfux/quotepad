using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    /// <summary>
    /// Main menu of the QuotePad
    /// </summary>
    public class mainMenu : menuStripPrototype
    {
        private TabControlPrototype tabcontrol;

        public mainMenu(TabControlPrototype tabControl)
        {
            tabcontrol = tabControl;
            ToolStripMenuItem quote = new ToolStripMenuItem("Цитата");
            ToolStripMenuItem quote_view = new ToolStripMenuItem("Просмотр", Resources._1315511677_old_edit_find);
            quote_view.Click += new EventHandler(quote_view_Click);
            ToolStripMenuItem quote_add = new ToolStripMenuItem("Добавить", Resources._1315515285_Add);
            quote_add.Click += new EventHandler(quote_add_Click);
            quote_add.Tag = this.adminTag;
            ToolStripMenuItem quote_quit = new ToolStripMenuItem("Выход", Resources._1315515375_quit);
            quote_quit.Click += new EventHandler(quote_quit_Click);
            quote.DropDownItems.Add(quote_view);
            quote.DropDownItems.Add(quote_add);
            quote.DropDownItems.Add(quote_quit);
            ToolStripMenuItem edit = new ToolStripMenuItem("Правка");
            edit.Tag = this.adminTag;
            ToolStripMenuItem edit_theme = new ToolStripMenuItem("Темы");
            edit_theme.Click += new EventHandler(edit_theme_Click);
            ToolStripMenuItem edit_author = new ToolStripMenuItem("Автора");
            edit_author.Click += new EventHandler(edit_author_Click);
            ToolStripMenuItem edit_quote = new ToolStripMenuItem("Цитаты");
            edit.DropDownItems.Add(edit_theme);
            edit.DropDownItems.Add(edit_author);
            edit.DropDownItems.Add(edit_quote);
            ToolStripMenuItem search = new ToolStripMenuItem("Поиск");
            ToolStripMenuItem search_author = new ToolStripMenuItem("По автору");
            ToolStripMenuItem search_theme = new ToolStripMenuItem("По теме");
            ToolStripMenuItem search_text = new ToolStripMenuItem("По тексту");
            ToolStripMenuItem search_date = new ToolStripMenuItem("По дате");
            search.DropDownItems.Add(search_author);
            search.DropDownItems.Add(search_theme);
            search.DropDownItems.Add(search_text);
            search.DropDownItems.Add(search_date);
            ToolStripMenuItem settings = new ToolStripMenuItem("Параметры");
            ToolStripMenuItem settings_config = new ToolStripMenuItem("Настройка");
            settings_config.Tag = this.adminTag;
            ToolStripMenuItem settings_export = new ToolStripMenuItem("Экспорт");
            ToolStripMenuItem settings_login = new ToolStripMenuItem("Авторизоваться");
            settings.DropDownItems.Add(settings_config);
            settings.DropDownItems.Add(settings_export);
            settings.DropDownItems.Add(settings_login);
            ToolStripMenuItem help = new ToolStripMenuItem("Справка");
            ToolStripMenuItem help_about = new ToolStripMenuItem("О программе");
            help.DropDownItems.Add(help_about);

            this.Items.Add(quote);
            this.Items.Add(edit);
            this.Items.Add(search);
            this.Items.Add(settings);
            this.Items.Add(help);
        }

        void edit_author_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new AuthorManager());
        }

        void edit_theme_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new ThemeManager());
        }

        void quote_quit_Click(object sender, EventArgs e)
        {
            // Needs to send manually close all page before (to save job before exit)
            Application.Exit();
        }

        void quote_add_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new pageQuoteEditor());
        }

        private void quote_view_Click(object sender, EventArgs e)
        {
            tabcontrol.AddPage(new pageQuoteView());
        }
    }
}
