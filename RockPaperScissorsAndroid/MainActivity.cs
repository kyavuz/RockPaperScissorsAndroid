using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace RockPaperScissorsAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "activity_main" layout resource
            SetContentView(Resource.Layout.about_page);

            Thread.Sleep(500);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            FinishAffinity(); // close Splash screen
        }
    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, NoHistory = false)]
    public class MainActivity : AppCompatActivity
    {
        private string[] images = { "rock", "paper", "scissors" };
        private int randomIndex;
        private string image;
        private string gameChoice;
        private string pageName = "activity_main";
        private bool isButtonClicked = false;
        private ImageView GameSelectionView;
        private ImageView WinLoseStatusView;
        private Button RockSelectButton;
        private Button PaperSelectButton;
        private Button ScissorsSelectButton;
        private Button AboutButton;
        private TextView VersionTextView;
        private TextView emailTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Thread.Sleep(1000);
            //StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            //Finish();


            //SetContentView(Resource.Drawable.splash_screen);
            //Thread.Sleep(1000);
            //StartActivity(typeof(AppCompatActivity));


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "activity_main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Prevent switching to horizontal mode (force to portrait mode)
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            // get ImageView IDs
            GameSelectionView = FindViewById<ImageView>(Resource.Id.GameSelectionView);
            WinLoseStatusView = FindViewById<ImageView>(Resource.Id.WinLoseStatusView);

            // get button IDs
            RockSelectButton = FindViewById<Button>(Resource.Id.RockSelectButton);
            RockSelectButton.Click += RockSelectClicked;
            PaperSelectButton = FindViewById<Button>(Resource.Id.PaperSelectButton);
            PaperSelectButton.Click += PaperSelectClicked;
            ScissorsSelectButton = FindViewById<Button>(Resource.Id.ScissorsSelectButton);
            ScissorsSelectButton.Click += ScissorsSelectClicked;
            AboutButton = FindViewById<Button>(Resource.Id.AboutButton);
            AboutButton.Click += NavigateToAboutPage;
        }

        public override void OnBackPressed()
        {
            // if back button pressed, go back to main page (activity_main.xml)
            if (pageName!="activity_main") {
                // If it's not on the main page
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                pageName = "activity_main";
            }
            else { // If on activity_main, finish all activities and exit the application
                // Add a check to prevent multiple calls to FinishAffinity()
                if (!IsDestroyed)
                {
                    FinishAffinity();
                }
            }
        }

        static async Task WaitForDelayAsync(int milliseconds)
        {
            await Task.Run(() => Task.Delay(milliseconds));
        }

        private async Task rollingDice()
        {
            WinLoseStatusView.SetImageResource(Resource.Drawable.empty);
            for (int i = 0; i<2; i++)
            {
                GameSelectionView.SetImageResource(Resource.Drawable.rock);
                await WaitForDelayAsync(200);
                GameSelectionView.SetImageResource(Resource.Drawable.paper);
                await WaitForDelayAsync(200);
                GameSelectionView.SetImageResource(Resource.Drawable.scissors);
                await WaitForDelayAsync(200);
            }
        }
        private string getGameChoice(object sender, EventArgs e)
        {
            Random random = new Random();
            randomIndex = random.Next(images.Length);
            image = images[randomIndex];
            randomIndex=99;

            int resourceId;

            switch (image)
            {
                case "rock":
                    resourceId = Resource.Drawable.rock;
                    break;
                case "paper":
                    resourceId = Resource.Drawable.paper;
                    break;
                case "scissors":
                    resourceId = Resource.Drawable.scissors;
                    break;
                default:
                    resourceId = Resource.Drawable.homePage;
                    break;
            }

            GameSelectionView.SetImageResource(resourceId);
            return image;
        }

        private void DetermineWinner(string userChoice, string gameChoice)
        {
            if (userChoice == gameChoice)
            {
                // "It's a tie!"
                WinLoseStatusView.SetImageResource(Resource.Drawable.tie);
            }
            else if ((userChoice == "rock" && gameChoice == "scissors") ||
                     (userChoice == "paper" && gameChoice == "rock") ||
                     (userChoice == "scissors" && gameChoice == "paper"))
            {
                // "The user wins!"
                WinLoseStatusView.SetImageResource(Resource.Drawable.win);
            }
            else
            {
                // "The user loses!"
                WinLoseStatusView.SetImageResource(Resource.Drawable.lose);
            }
        }

        private async void RockSelectClicked(object sender, EventArgs e)
        {
            if (!isButtonClicked)
            {
                isButtonClicked = true;
                await rollingDice();
                gameChoice = getGameChoice(sender, e);
                DetermineWinner("rock", gameChoice);
                isButtonClicked = false;
            }
        }

        private async void PaperSelectClicked(object sender, EventArgs e)
        {
            if (!isButtonClicked)
            {
                isButtonClicked = true;
                await rollingDice();
                gameChoice = getGameChoice(sender, e);
                DetermineWinner("paper", gameChoice);
                isButtonClicked = false;
            }
        }

        private async void ScissorsSelectClicked(object sender, EventArgs e)
        {
            if (!isButtonClicked)
            {
                isButtonClicked = true;
                await rollingDice();
                gameChoice = getGameChoice(sender, e);
                DetermineWinner("scissors", gameChoice);
                isButtonClicked = false;
            }
        }

        private void ComposeEmail(string[] addresses, string subject, string body)
        {
            var emailIntent = new Intent(Intent.ActionSendto);
            emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));
            emailIntent.PutExtra(Intent.ExtraEmail, addresses);
            emailIntent.PutExtra(Intent.ExtraSubject, subject);
            emailIntent.PutExtra(Intent.ExtraText, body);

            if (emailIntent.ResolveActivity(PackageManager) != null)
            {
                StartActivity(emailIntent);
            }
        }

        private void NavigateToAboutPage(object sender, EventArgs e)
        {
            // Set our view from the "about" layout resource
            SetContentView(Resource.Layout.about_page);
            pageName = "about_page";

            // get TextView IDs
            VersionTextView = FindViewById<TextView>(Resource.Id.VersionTextView);
            emailTextView = FindViewById<TextView>(Resource.Id.emailTextView);

            // display version number
            var packageInfo = PackageManager.GetPackageInfo(PackageName, 0);
            var versionName = packageInfo.VersionName;
            VersionTextView.Text = "Version: " + versionName;


            // contact us
            emailTextView.Click += (sender, e) =>
            {
                string emailAddress = emailTextView.Text;
                ComposeEmail(new[] { emailAddress }, "Subject", "Body");
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}