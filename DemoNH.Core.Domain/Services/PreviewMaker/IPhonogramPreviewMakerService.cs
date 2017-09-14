using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoNH.Core.Services.Messages;

namespace DemoNH.Core.Services.PreviewMaker
{
	public interface IPhonogramPreviewMakerService
	{
		#region Public Methods

		void MakePreview(PhonogramMessage message);

		#endregion Public Methods
	}
}