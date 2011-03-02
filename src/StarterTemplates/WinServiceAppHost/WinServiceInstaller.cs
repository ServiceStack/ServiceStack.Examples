using System.ComponentModel;
using System.Configuration.Install;

namespace WinServiceAppHost
{
	[RunInstaller(true)]
	public partial class WinServiceInstaller : Installer
	{
		public WinServiceInstaller()
		{
			InitializeComponent();
		}		
	}
}
