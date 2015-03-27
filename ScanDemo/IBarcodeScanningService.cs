using System;
using System.Threading.Tasks;

namespace ScanDemo
{
	// ZXing
	public interface IBarcodeScanningService
	{
		Task<ScanResult> Scan();
	}
}

