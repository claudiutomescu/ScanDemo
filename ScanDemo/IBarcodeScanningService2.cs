using System;
using System.Threading.Tasks;

namespace ScanDemo
{
	public interface IBarcodeScanningService2
	{
		void Configure(string appKey, string cancelText);
		Task<ScanResult> Scan();
	}

	public class ScanResult
	{
		public bool Success { get; set; }
		public string Code { get; set; }
		public string Symbology { get; set; }
	}
}

