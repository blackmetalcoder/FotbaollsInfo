using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FotbaollsInfo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();


        public MainPage()
        {
            this.InitializeComponent();
            //Start_timer();
            loadData();
        }
        public void Start_timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(00, 0, 60);
            bool enabled = timer.IsEnabled;
            timer.Start();
        }
        private async void loadData()
        {
            try
            {
                pring1.IsActive = true;
                DateTime date = new DateTime();
                date = DateTime.Now;
                GlobalDatum.SomeDatum = date.ToString("yyyy-MM-dd");
                string s = await DataSource.GetLive(date.ToString("yyyy-MM-dd"));
                var j = JsonConvert.DeserializeObject<List<Matcher>>(s);

                listView.ItemsSource = j;
                //MyGroupedResults = j.ToGroups(x => x.HomeTeam, x => x.League);
                /*this.defaultViewModel["Matcher"] = j;
                
                jumplist.ReleaseItemsSource();
                itemListLive.ItemsSource = MyGroupedResults;
                jumplist.ApplyItemsSource();
                string sLigor = await DataSource.GetLigor();
                var L = JsonConvert.DeserializeObject<List<Ligor>>(sLigor);
                var MyGroupedLigor = L.ToGroups(x => x.Name, x => x.Country);
                jumplistLigor.ReleaseItemsSource();
                listLiga.ItemsSource = MyGroupedLigor;
                jumplistLigor.ApplyItemsSource();*/
                pring1.IsActive = false;

            }
            catch
            {
                pring1.IsActive = false;
            }
        }
        private async void timer_Tick(object sender, object e)
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            if (cboDatum.Date.ToString("yyyy-MM-dd") == GlobalDatum.SomeDatum)
            {
                try
                {
                    pring1.IsActive = true;
                    string s = await DataSource.GetLive(date.ToString("yyyy-MM-dd"));
                    var j = JsonConvert.DeserializeObject<List<Matcher>>(s);
                    /*this.defaultViewModel["Matcher"] = j;
                    var MyGroupedResults = j.ToGroups(x => x.HomeTeam, x => x.League);
                    jumplist.ReleaseItemsSource();
                    itemListLive.ItemsSource = MyGroupedResults;
                    jumplist.ApplyItemsSource();
                    itemListLive.SelectedIndex = ListIndex.Position;
                    itemListLive.UpdateLayout();
                    itemListLive.ScrollIntoView(itemListLive.SelectedItem);*/
                    pring1.IsActive = false;
                }
                catch
                {
                    pring1.IsActive = false;
                }
            }

        }
        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {

        }
    }
}
