﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPG_Paper_Maker
{
    public partial class DialogDataBase : Form
    {
        protected DialogDataBaseControl Control;
        protected BindingSource ViewModelBindingSource = new BindingSource();
        public ListBox[] ListBoxesCanceling, ListBoxes;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public DialogDataBase()
        {
            InitializeComponent();
            Control = new DialogDataBaseControl(WANOK.LoadBinaryDatas<SystemDatas>(WANOK.SystemPath));
            ViewModelBindingSource.DataSource = Control;
            ListBoxesCanceling = new ListBox[] { textBoxLangGameName.GetTextBox(), listBoxColors.GetListBox(), textBoxGraphic.GetTextBox(), listBoxAutotiles.GetListBox(), listBoxRelief.GetListBox() };
            ListBoxes = new ListBox[] { listBoxTilesets.GetListBox() };

            // Tilesets
            listBoxTilesets.InitializeListParameters(ListBoxesCanceling, Control.ModelSystem.Tilesets.Cast<SuperListItem>().ToList(), null, typeof(Tileset), 1, Tileset.MAX_TILESETS);
            listBoxTilesets.GetListBox().SelectedIndexChanged += listBoxTilesets_SelectedIndexChanged;
            textBoxGraphic.GetTextBox().SelectedValueChanged += textBoxGraphic_SelectedValueChanged;
            collisionSettings.LoadTextures();
            listBoxAutotiles.GetButton().Text = "Choose autotiles";
            listBoxAutotiles.GetButton().Click += listBoxAutotiles_Click;
            listBoxRelief.GetButton().Text = "Choose reliefs";
            listBoxRelief.GetButton().Click += listBoxRelief_Click;

            // System
            ComboBoxResolution.SelectedIndex = Control.GetFullScreenIndex();
            toolTipSquareSize.SetToolTip(buttonSquareSize, "This option set the maps displaying, it is recommended to put multiple 8 numbers.\nNote that the pixel height addings are not modified.");
            textBoxLangGameName.GetTextBox().Items.Add(Control.GameName[Control.Langs[0]]);
            textBoxLangGameName.AllNames = Control.GameName;
            listBoxColors.InitializeListParameters(ListBoxesCanceling, Control.ModelSystem.Colors.Cast<SuperListItem>().ToList(), typeof(DialogSystemColors), typeof(SystemColor), 1, SystemColor.MAX_COLORS);

            // list event handlers
            for (int i = 0; i < ListBoxesCanceling.Length; i++)
            {
                ListBoxesCanceling[i].MouseClick += listBox_MouseClick;
            }
            for (int i = 0; i < ListBoxes.Length; i++)
            {
                ListBoxes[i].MouseClick += listBox_MouseClick;
            }

            MouseWheel += new MouseEventHandler(form_MouseWheel);
            tabControl1.KeyDown += new KeyEventHandler(form_KeyDown);

            UnselectAllLists();
            InitializeDataBindings();
        }

        // -------------------------------------------------------------------
        // InitializeDataBindings
        // -------------------------------------------------------------------

        private void InitializeDataBindings()
        {
            numericWidth.DataBindings.Add("Value", ViewModelBindingSource, "ScreenWidth", true);
            numericHeight.DataBindings.Add("Value", ViewModelBindingSource, "ScreenHeight", true);
            numericSquareSize.DataBindings.Add("Value", ViewModelBindingSource, "SquareSize", true);
        }

        // -------------------------------------------------------------------
        // UnselectAllCancelingLists
        // -------------------------------------------------------------------

        public void UnselectAllCancelingLists()
        {
            for (int i = 0; i < ListBoxesCanceling.Length; i++)
            {
                ListBoxesCanceling[i].ClearSelected();
            }
        }

        // -------------------------------------------------------------------
        // UnselectAllLists
        // -------------------------------------------------------------------

        public void UnselectAllLists()
        {
            UnselectAllCancelingLists();
            for (int i = 0; i < ListBoxes.Length; i++)
            {
                ListBoxes[i].SelectedIndex = 0;
            }
        }

        // -------------------------------------------------------------------
        // TILESETS
        // -------------------------------------------------------------------

        #region Tilesets

        // -------------------------------------------------------------------
        // SetCommonTilesetList
        // -------------------------------------------------------------------

        public void SetCommonTilesetList(Tileset tileset)
        {
            listBoxAutotiles.InitializeListParameters(Control.ModelSystem, ListBoxesCanceling, Control.ModelSystem.Autotiles.Cast<SuperListItem>().ToList(), tileset.Autotiles, typeof(DialogAddingAutotilesList), typeof(SystemAutotile), 1, SystemAutotile.MAX_AUTOTILES, Control.ModelSystem.GetAutotileById);
            listBoxRelief.InitializeListParameters(Control.ModelSystem, ListBoxesCanceling, Control.ModelSystem.Reliefs.Cast<SuperListItem>().ToList(), tileset.Reliefs, typeof(DialogAddingReliefsList), typeof(SystemRelief), 1, SystemRelief.MAX_RELIEFS, Control.ModelSystem.GetReliefById);
        }

        // -------------------------------------------------------------------
        // listBoxTilesets_SelectedIndexChanged
        // -------------------------------------------------------------------

        public void listBoxTilesets_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tileset tileset = (Tileset)listBoxTilesets.GetListBox().SelectedItem;
            if (tileset != null)
            {
                textBoxTilesetName.Text = tileset.Name;
                textBoxGraphic.InitializeParameters(tileset.Graphic);
                collisionSettings.InitializeParameters(tileset.Collision, tileset.Graphic);
                SetCommonTilesetList(tileset);
            }
        }

        // -------------------------------------------------------------------
        // textBoxGraphic_SelectedValueChanged
        // -------------------------------------------------------------------

        public void textBoxGraphic_SelectedValueChanged(object sender, EventArgs e)
        {
            Tileset tileset = (Tileset)listBoxTilesets.GetListBox().SelectedItem;
            collisionSettings.InitializeParameters(tileset.Collision, tileset.Graphic);
        }

        // -------------------------------------------------------------------
        // textBoxTilesetName_TextChanged
        // -------------------------------------------------------------------

        private void textBoxTilesetName_TextChanged(object sender, EventArgs e)
        {
            listBoxTilesets.SetName(textBoxTilesetName.Text);
        }

        // -------------------------------------------------------------------
        // listBoxAutotiles_Click
        // -------------------------------------------------------------------

        private void listBoxAutotiles_Click(object sender, EventArgs e)
        {
            Tileset tileset = (Tileset)listBoxTilesets.GetListBox().SelectedItem;
            DialogAddingAutotilesList dialog = new DialogAddingAutotilesList("Choose Autotile", Control.ModelSystem, tileset.Autotiles);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Control.ModelSystem.Autotiles = dialog.GetListAutotiles();
                tileset.Autotiles = dialog.GetListTileset();
                for (int i = 0; i < listBoxTilesets.GetListBox().Items.Count; i++)
                {
                    Tileset cpTileset = (Tileset)listBoxTilesets.GetListBox().Items[i];
                    List<int> list = new List<int>();
                    for (int j = 0; j < cpTileset.Autotiles.Count; j++)
                    {
                        list.Add(cpTileset.Autotiles[j]);
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] > Control.ModelSystem.Autotiles.Count) cpTileset.Autotiles.Remove(list[j]);
                    }
                }
                SetCommonTilesetList(tileset);
            }
        }

        // -------------------------------------------------------------------
        // listBoxRelief_Click
        // -------------------------------------------------------------------

        private void listBoxRelief_Click(object sender, EventArgs e)
        {
            Tileset tileset = (Tileset)listBoxTilesets.GetListBox().SelectedItem;
            DialogAddingReliefsList dialog = new DialogAddingReliefsList("Choose relief", Control.ModelSystem, tileset);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Control.ModelSystem.Reliefs = dialog.GetListReliefs();
                tileset.Reliefs = dialog.GetListTileset();
                for (int i = 0; i < listBoxTilesets.GetListBox().Items.Count; i++)
                {
                    Tileset cpTileset = (Tileset)listBoxTilesets.GetListBox().Items[i];
                    List<int> list = new List<int>();
                    for (int j = 0; j < cpTileset.Reliefs.Count; j++)
                    {
                        list.Add(cpTileset.Reliefs[j]);
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] > Control.ModelSystem.Reliefs.Count) cpTileset.Reliefs.Remove(list[j]);
                    }
                }
                SetCommonTilesetList(tileset);
            }
        }

        #endregion

        // -------------------------------------------------------------------
        // SYSTEM
        // -------------------------------------------------------------------

        #region System

        // -------------------------------------------------------------------
        // ComboBoxResolution_SelectedIndexChanged
        // -------------------------------------------------------------------

        public void ComboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control.SetFullScreen(ComboBoxResolution.SelectedIndex);
        }

        #endregion

        // -------------------------------------------------------------------
        // listBox_MouseClick
        // -------------------------------------------------------------------

        public void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = ((ListBox)sender).IndexFromPoint(e.X, e.Y); ;
            UnselectAllCancelingLists();
            ((ListBox)sender).SelectedIndex = index;
        }

        // -------------------------------------------------------------------
        // tabControl1_SelectedIndexChanged
        // -------------------------------------------------------------------

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnselectAllLists();
            if (tabControl1.SelectedTab == tabPageTilesets)
            {
                listBoxTilesets.GetListBox().Focus();
            }
        }

        // -------------------------------------------------------------------
        // form_MouseWheel
        // -------------------------------------------------------------------

        private void form_MouseWheel(object sender, MouseEventArgs e)
        {
            FocusList();
        }

        // -------------------------------------------------------------------
        // form_KeyDown
        // -------------------------------------------------------------------

        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) FocusList();
        }

        // -------------------------------------------------------------------
        // FocusList
        // -------------------------------------------------------------------

        public void FocusList()
        {
            if (tabControl1.SelectedTab == tabPageTilesets)
            {
                listBoxTilesets.GetListBox().Focus();
            }
        }

        // -------------------------------------------------------------------
        // ok_Click
        // -------------------------------------------------------------------

        private void ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Control.ModelSystem.Colors = listBoxColors.ModelList.Cast<SystemColor>().ToList();
            Control.ModelSystem.Tilesets = listBoxTilesets.ModelList.Cast<Tileset>().ToList();
            Control.Save();
            Close();
        }      
    }
}
