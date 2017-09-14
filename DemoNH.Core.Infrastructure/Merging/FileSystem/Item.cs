namespace DemoNH.Core.Infrastructure.Merging.FileSystem
{
	public abstract class Item
	{
		public string RootPath { get; private set; }

		public string RelativePath { get; private set; }

		public abstract bool Exists { get; }

		public Item(string rootPath, string relativePath)
		{
			this.RootPath = rootPath;
			this.RelativePath = relativePath;
		}

		private string fullPath;

		public virtual string FullPath
		{
			get
			{
				if (this.fullPath == null)
					this.fullPath = System.IO.Path.Combine(this.RootPath, this.RelativePath);
				return this.fullPath;
			}
		}
	}
}