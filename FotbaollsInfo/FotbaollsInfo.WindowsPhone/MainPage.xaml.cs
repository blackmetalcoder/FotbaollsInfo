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
using QKit.JumpList;
using FotbaollsInfo.Common;
using System.Collections;
using Windows.ApplicationModel.Email;

namespace FotbaollsInfo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Start_timer();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            loadData();
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public void Start_timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(00, 0, 60);
            bool enabled = timer.IsEnabled;
            timer.Start();
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
                    /*Kollar tidszon för att omvandla till lokaltid*/
                    foreach (var item in j)
                    {
                        Matcher m = new Matcher();
                        string startTid = GetLocalDateTime(DateTime.Parse(item.Date.ToString()));
                        item.Date = startTid; //.Substring(10, 10);
                    }
                    /*Slut tidszon*/
                    this.defaultViewModel["Matcher"] = j;
                    var MyGroupedResults = j.ToGroups(x => x.HomeTeam, x => x.League);
                    jumplist.ReleaseItemsSource();
                    itemListLive.ItemsSource = MyGroupedResults;
                    jumplist.ApplyItemsSource();
                    itemListLive.SelectedIndex = ListIndex.Position;
                    itemListLive.UpdateLayout();
                    itemListLive.ScrollIntoView(itemListLive.SelectedItem);
                    pring1.IsActive = false;
                }
                catch
               {
                    pring1.IsActive = false;
                }
            }

        }
        string GetLocalDateTime(DateTime targetDateTime)
        {
            int fromTimezone = -3;
            int localTimezone;
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            if (TimeZoneInfo.Local.BaseUtcOffset.Minutes != 0)
                localTimezone = Convert.ToInt16(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString() + (TimeZoneInfo.Local.BaseUtcOffset.Minutes / 60).ToString());
            else
                localTimezone = TimeZoneInfo.Local.BaseUtcOffset.Hours;

            DateTime Sdt = targetDateTime;
            DateTime UTCDateTime = targetDateTime.AddHours(-(fromTimezone));
            DateTime localDateTime = Sdt.AddHours(+(localTimezone + 1));
            //DateTime localDateTime = TimeZoneInfo.ConvertTime(targetDateTime, localZone);//ConvertTimeFromUtc(nu, tzi);
            return localDateTime.ToString("H:mm") ;//yyyy-MM-dd
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
                /*Kollar tidszon för att omvandla till lokaltid*/
                foreach (var item in j)
                {
                    Matcher m = new Matcher();
                    string startTid = GetLocalDateTime(DateTime.Parse(item.Date.ToString()));
                    item.Date = startTid; //.Substring(10,10);
                }
                /*Slut tidszon*/
                this.defaultViewModel["Matcher"] = j;
                var MyGroupedResults = j.ToGroups(x => x.HomeTeam, x => x.League);
                jumplist.ReleaseItemsSource();
                itemListLive.ItemsSource = MyGroupedResults;
                jumplist.ApplyItemsSource();
                string sLigor = await DataSource.GetLigor();
                var L = JsonConvert.DeserializeObject<List<Ligor>>(sLigor);
                var MyGroupedLigor = L.ToGroups(x => x.Name, x => x.Country);
                jumplistLigor.ReleaseItemsSource();
                listLiga.ItemsSource = MyGroupedLigor;
                jumplistLigor.ApplyItemsSource();
                pring1.IsActive = false;

            }
            catch (Exception x)
            {
                string s2 = x.Message.ToString();
                pring1.IsActive = false;
            }
        }

        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            try
            {
                string s = await DataSource.GetLive(cboDatum.Date.ToString("yyyy-MM-dd"));
                pring1.IsActive = true;
                var j = JsonConvert.DeserializeObject<List<Matcher>>(s);
                /*Kollar tidszon för att omvandla till lokaltid*/
                foreach (var item in j)
                {
                    Matcher m = new Matcher();
                    string startTid = GetLocalDateTime(DateTime.Parse(item.Date.ToString()));
                    item.Date = startTid; //.Substring(10, 10);
                }
                /*Slut tidszon*/
                var MyGroupedResults = j.ToGroups(x => x.HomeTeam, x => x.League);
                jumplist.ReleaseItemsSource();
                itemListLive.ItemsSource = MyGroupedResults;
                jumplist.ApplyItemsSource();
                itemListLive.SelectedIndex = ListIndex.Position;
                itemListLive.UpdateLayout();
                itemListLive.ScrollIntoView(itemListLive.SelectedItem);
                pring1.IsActive = false;
            }
            catch
            {
                pring1.IsActive = false;
            }
        }
        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            /*FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);*/
        }
        private void itemListLive_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void itemListLive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (itemListLive.SelectedIndex != -1)
            {
                ListIndex.Position = itemListLive.SelectedIndex;
            }*/
           
        }

        private void itemListLive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListIndex.Position = itemListLive.SelectedIndex;
            //FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void Om_Click(object sender, RoutedEventArgs e)
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Name = "Blackmetalcoder",
                Address = "peter@blackmetalcoder.se"
            };

            // Create email object
            EmailMessage mail = new EmailMessage();
            mail.Subject = " ";
            mail.Body = " ";

            // Add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            // Open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void NormalFlyoutBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            var context = button.DataContext;
            Matcher M = context as Matcher;
            /*Vi hamtar info om hemmalaget*/
            int HomeID = int.Parse(M.HomeTeam_Id);
            string s = await DataSource.getTeam(HomeID);
            var j = JsonConvert.DeserializeObject<List<Teams>>(s);
            cvsHometeam.Source = j;
            /*Vi hamtar info om Bortalaget*/
            int AwayID = int.Parse(M.AwayTeam_Id);
            string s2 = await DataSource.getTeam(AwayID);
            var j2 = JsonConvert.DeserializeObject<List<Teams>>(s2);
            cvsAwayteam.Source = j2;
        }
    }
}
