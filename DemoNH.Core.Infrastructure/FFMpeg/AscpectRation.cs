namespace DemoNH.Core.Infrastructure.FFMpeg
{
	public class AscpectRation
	{
		public string Value { get; private set; }

		private AscpectRation(string value)
		{
			this.Value = value;
		}

		public static AscpectRation Ratio4x3 = new AscpectRation("4x3");
		public static AscpectRation Ratio16x9 = new AscpectRation("4x3");
		public static AscpectRation Ratio13333 = new AscpectRation("1.3333");
		public static AscpectRation Ratio17777 = new AscpectRation("1.7777");
	}
}