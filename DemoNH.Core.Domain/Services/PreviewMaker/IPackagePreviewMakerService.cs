using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoNH.Core.Services.Messages;

namespace DemoNH.Core.Services.PreviewMaker
{
	public interface IPackagePreviewMakerService
	{
		#region Public Methods

		void MakePreview(PackageMessage message);

		#endregion Public Methods
	}
}