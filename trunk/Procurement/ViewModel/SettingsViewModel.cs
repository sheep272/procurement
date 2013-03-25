﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;
using Procurement.View;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Procurement.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsView view;

        private string currentCharacter;
        private string currentLeague;
        private bool isBusy;
        public string CurrentLeague
        {
            get { return currentLeague; }
            set
            {
                if (currentLeague == value)
                    return;

                currentLeague = value;
                Settings.UserSettings["FavoriteLeague"] = value;
                Settings.Save();
                refreshCharacters();
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public string CurrentCharacter
        {
            get { return currentCharacter; }
            set 
            {
                if (currentCharacter == value)
                    return;
                currentCharacter = value;
                Settings.UserSettings["FavoriteCharacter"] = value;
                Settings.Save();
            }
        }

        public List<string> Characters { get; set; }

        public List<string> Leagues { get; set; }

        public List<CurrencyRatio> CurrencyRatios
        {
            get { return Settings.CurrencyRatios.Values.ToList(); }
        }

        public ICommand UpdateRates { get; private set; }

        public SettingsViewModel(SettingsView view)
        {
            this.view = view;
            this.Leagues = ApplicationState.Leagues;
            this.CurrentLeague = Settings.UserSettings["FavoriteLeague"];
            refreshCharacters();
            this.CurrentCharacter = Settings.UserSettings["FavoriteCharacter"];            
            isBusy = false;
        }

        private void refreshCharacters()
        {
            this.Characters = ApplicationState.Model.GetCharacters().Where(c => c.League == CurrentLeague).Select(c => c.Name).ToList();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Characters"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
