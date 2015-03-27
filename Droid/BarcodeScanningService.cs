using System;
using ZXing;
using ZXing.Mobile;
using Xamarin.Forms;
using ScanDemo.Droid;

[assembly: Dependency(typeof(BarcodeScanningService))]
namespace ScanDemo.Droid
{
	public class BarcodeScanningService : IBarcodeScanningService
	{
		#region IBarcodeScanningService implementation
		public async System.Threading.Tasks.Task<ScanResult> Scan ()
		{
			var scanner = new ZXing.Mobile.MobileBarcodeScanner(Forms.Context);
			var scanResults = await scanner.Scan();

			return new ScanResult { Success = true, Code = scanResults.Text, Symbology = scanResults.BarcodeFormat.ToString() };
		}
		#endregion

	}
}

