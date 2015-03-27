using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Xamarin.Forms;
using ScanDemo;
using Scandit;
using Scandit.Interfaces;
using Android.OS;
using Android.Views;
using ScanDemo.Droid;

[assembly: Dependency(typeof(BarcodeScanningService2))]
namespace ScanDemo.Droid
{
	public class BarcodeScanningService2 : IBarcodeScanningService2
	{
		private string licenseKey = "nTrNMOrX5s8KG/BYtCu/fUZq98FOmTvbEVGPwezdn/s";
		private string cancelText = "Cancel";

		ScanResult scanResult = null;
		ManualResetEvent waitScanResetEvent = null;

		public BarcodeScanningService2 ()
		{
		}

		#region IBarcodeScanningService2 implementation

		public System.Threading.Tasks.Task<ScanResult> Scan ()
		{
			return Task<ScanResult>.Run (() => {
				waitScanResetEvent = new ManualResetEvent (false);

				//Run ScanditActivity
				var intent = new Intent (Forms.Context, typeof(ScanditActivity));
				intent.PutExtra ("licenseKey", licenseKey);

				ScanditActivity.OnCanceled += OnCanceled;
				ScanditActivity.OnScanCompleted += OnScanCompleted;

				Forms.Context.StartActivity (intent);

				waitScanResetEvent.WaitOne ();

				ScanditActivity.OnCanceled -= OnCanceled;
				ScanditActivity.OnScanCompleted -= OnScanCompleted;

				return scanResult;
			});
		}

		public void Configure(string licenseKey, string cancelText)
		{
			this.licenseKey = licenseKey;
			this.cancelText = cancelText;
		}

		#endregion

		private void OnCanceled(){
			scanResult = new ScanResult () {
				Success = false
			};
			waitScanResetEvent.Set ();
		}

		private void OnScanCompleted(ScanResult result){
			scanResult = result;
			waitScanResetEvent.Set ();
		}
	}

	[Activity(Label = "ScanditActivity", ConfigurationChanges=ConfigChanges.Orientation|ConfigChanges.KeyboardHidden|ConfigChanges.ScreenLayout)]
	public class ScanditActivity: FragmentActivity, IScanditSDKListener {

		private ScanditSDKBarcodePicker picker;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature(WindowFeatures.NoTitle);
			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			string licenseKey = Intent.GetStringExtra ("licenseKey");

			picker = new ScanditSDKBarcodePicker (this, licenseKey, ScanditSDK.CameraFacingBack);

			picker.OverlayView.AddListener (this);

			picker.StartScanning ();

			SetContentView (picker);
		}

		public void DidCancel ()
		{
			this.Finish ();
			if (OnCanceled != null) {
				OnCanceled ();
			}
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
			DidCancel ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			picker.StartScanning ();
		}

		protected override void OnPause ()
		{
			picker.StopScanning ();
			base.OnPause ();
		}

		public void DidManualSearch (string text)
		{
			//Do nothing
		}

		public void DidScanBarcode (string barcode, string type)
		{
			this.Finish ();
			if (OnScanCompleted != null) {
				OnScanCompleted (new ScanResult () { Success = true, Code = barcode, Symbology = type });
			}
		}

		//
		// Static Events
		//
		public static event Action OnCanceled;

		public static event Action<ScanResult> OnScanCompleted;
	}
}

