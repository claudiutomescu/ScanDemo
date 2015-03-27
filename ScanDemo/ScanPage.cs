using System;

using Xamarin.Forms;

namespace ScanDemo
{
	public class ScanPage : ContentPage
	{
		public ScanPage ()
		{
			var lblBarcode = new Label ();

			var btnScan = new Button () { Text = "Scan with ZXing" };
			btnScan.Clicked += async (sender, e) => {

				// Obtain a reference to the an implementation of IBarcodeScanningService (platform specific)
				var scanningService = DependencyService.Get<IBarcodeScanningService>();
				var result = await scanningService.Scan();

				if (result.Success) {
					lblBarcode.Text = string.Format("{0}: {1}", result.Symbology, result.Code);
				}
				else {
					lblBarcode.Text = "Scanning error!";
				}
			};


			var btnScanWithScandit = new Button (){ Text = "Scan with Scandit" };
			btnScanWithScandit.Clicked += async (sender, e) => {

				var scanningService = DependencyService.Get<IBarcodeScanningService2>();
				//scanningService.Configure("", "");
				var result = await scanningService.Scan();

				if (result.Success) {
					lblBarcode.Text = string.Format("{0}: {1}", result.Symbology, result.Code);
				}
				else {
					lblBarcode.Text = "Scanning error!";
				}

			};
				
			Content = new StackLayout { 
				Children = {
					btnScan,
					btnScanWithScandit,
					lblBarcode
				}
			};
		}
	}
}


