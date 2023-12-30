using System;
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
        private Button RockSelectButton;
        private Button PaperSelectButton;
        private Button ScissorsSelectButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // get "GameSelectionView" ID
            GameSelectionView = FindViewById<ImageView>(Resource.Id.GameSelectionView);

            // get button IDs
            RockSelectButton = FindViewById<Button>(Resource.Id.RockSelectButton);
            RockSelectButton.Click += RockSelectClicked;
            PaperSelectButton = FindViewById<Button>(Resource.Id.PaperSelectButton);
            PaperSelectButton.Click += PaperSelectClicked;
            ScissorsSelectButton = FindViewById<Button>(Resource.Id.ScissorsSelectButton);
            ScissorsSelectButton.Click += ScissorsSelectClicked;
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

        private void RockSelectClicked(object sender, EventArgs e)
        {
            gameChoice = getGameChoice(sender, e);
            //DetermineWinner(userChoice, computerChoice);
        }

        private void PaperSelectClicked(object sender, EventArgs e)
        {
            gameChoice = getGameChoice(sender, e);
            //DetermineWinner(userChoice, computerChoice);
        }

        private void ScissorsSelectClicked(object sender, EventArgs e)
        {
            gameChoice = getGameChoice(sender, e);
            //DetermineWinner(userChoice, computerChoice);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}