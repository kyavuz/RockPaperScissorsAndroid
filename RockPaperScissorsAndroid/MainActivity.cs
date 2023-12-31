using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace RockPaperScissorsAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string[] images = { "rock", "paper", "scissors" };
        private int randomIndex;
        private string image;
        private string gameChoice;
        private ImageView GameSelectionView;
        private ImageView WinLoseStatusView;
        private Button RockSelectButton;
        private Button PaperSelectButton;
        private Button ScissorsSelectButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

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
            await rollingDice();
            gameChoice = getGameChoice(sender, e);
            DetermineWinner("rock", gameChoice);
        }

        private async void PaperSelectClicked(object sender, EventArgs e)
        {
            await rollingDice();
            gameChoice = getGameChoice(sender, e);
            DetermineWinner("paper", gameChoice);
        }

        private async void ScissorsSelectClicked(object sender, EventArgs e)
        {
            await rollingDice();
            gameChoice = getGameChoice(sender, e);
            DetermineWinner("scissors", gameChoice);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}