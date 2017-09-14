using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DemoNH.Core.Infrastructure.Merging.FileSystem
{
	public class Directory : Item
	{
		public int Priority { get; protected set; }

		public DirectoryType Type { get; protected set; }

		public Directory(string rootPath, string relativePath, DirectoryType type, int priority)
			: base(rootPath: rootPath, relativePath: relativePath)
		{
			this.Priority = priority;
			this.Type = type;
			this.exists = null;
		}

		private bool? exists;

		public override bool Exists
		{
			get
			{
				if (this.exists == null)
					this.exists = System.IO.Directory.Exists(this.FullPath);
				return this.exists.Value;
			}

		}

		public void Create()
		{
			try
			{
				this.exists = null;
				System.IO.Directory.CreateDirectory(this.FullPath);
			}
			catch (Exception)
			{
				DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.Directory.CreateDirectory('{0}')", this.FullPath));
				throw;
			}
		}

		public void Delete()
		{
			try
			{
				this.exists = null;
				System.IO.Directory.Delete(this.FullPath, true);
			}
			catch (Exception)
			{
				DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.Directory.Delete('{0}', true)", this.FullPath));
				throw;
			}
		}

		public CopyResult MoveTo(Directory targetDirectory)
		{
			CopyResult copyResult = this.CopyTo(targetDirectory);
			if (copyResult == CopyResult.Ok)
				this.Delete();
			return copyResult;
		}

		public CopyResult CopyTo(Directory targetDirectory)
		{
			CopyResult copyResult;
			if (targetDirectory.FullPath != this.FullPath)
			{
				bool targetExists = targetDirectory.Exists;
				if (targetExists == false)
					targetDirectory.Create();

				System.IO.DirectoryInfo sourceDirInfo = new System.IO.DirectoryInfo(this.FullPath);
				System.IO.DirectoryInfo[] sourceDirectories = sourceDirInfo.GetDirectories("*", System.IO.SearchOption.AllDirectories);
				System.IO.FileInfo[] sourceFiles = sourceDirInfo.GetFiles("*", System.IO.SearchOption.AllDirectories);
				foreach (System.IO.DirectoryInfo directory in sourceDirectories)
				{
					string targetRelativePath = directory.FullName.Substring(this.RootPath.Length);
					while (targetRelativePath.StartsWith(@"\"))
						targetRelativePath = targetRelativePath.Substring(1);
					string targetFullPath = System.IO.Path.Combine(targetDirectory.RootPath, targetRelativePath);
					if (System.IO.Directory.Exists(targetFullPath) == false)
					{
						try
						{
							System.IO.Directory.CreateDirectory(targetFullPath);
						}
						catch (Exception)
						{
							DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.Directory.CreateDirectory('{0}')", targetFullPath));
							throw;
						}
					}
				}
				List<System.IO.FileInfo> filesToDelete = new List<System.IO.FileInfo>();
				foreach (var sourceFileInfo in sourceFiles)
				{
					string targetRelativePath = sourceFileInfo.FullName.Substring(this.RootPath.Length);
					while (targetRelativePath.StartsWith(@"\"))
						targetRelativePath = targetRelativePath.Substring(1);
					string targetFullPath = System.IO.Path.Combine(targetDirectory.RootPath, targetRelativePath);
					System.IO.FileInfo targetFileInfo = new System.IO.FileInfo(targetFullPath);
					if (System.IO.File.Exists(targetFullPath) == false)
					{
						try
						{
							System.IO.File.Copy(sourceFileInfo.FullName, targetFullPath);
							filesToDelete.Add(sourceFileInfo);
						}
						catch (Exception)
						{
							DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.File.Copy('{0}', '{1}')", sourceFileInfo.FullName, targetFullPath));
							throw;
						}
					}
					else
					{
						string sourceMD5 = null;
						string targetMD5 = null;
						using (System.IO.FileStream sourceStream = sourceFileInfo.Open(System.IO.FileMode.Open))
						{
							using (System.IO.FileStream targetStream = targetFileInfo.Open(System.IO.FileMode.Open))
							{
								sourceMD5 = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(sourceStream));
								targetMD5 = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(targetStream));
							}
						}
						if
							(
								(sourceMD5 != targetMD5)
								&&
								(sourceFileInfo.Length >= targetFileInfo.Length)
							)
						{
							try
							{
								System.IO.File.Copy(sourceFileInfo.FullName, targetFullPath, true);
							}
							catch (Exception)
							{
								DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.File.Copy('{0}', '{1}')", sourceFileInfo.FullName, targetFullPath));
								throw;
							}
						}
						filesToDelete.Add(sourceFileInfo);
					}
				}
				foreach (var file in filesToDelete)
				{
					try
					{
						file.Delete();
					}
					catch (Exception)
					{
						DemoNH.Core.Infrastructure.AOP.ExceptionHandling.LogContext.Current.SetValue("Action", string.Format("System.IO.File.Delete('{0}')", file.FullName));
						throw;
					}
				}
				sourceDirInfo.Refresh();
				if (sourceDirInfo.GetFiles("*", System.IO.SearchOption.AllDirectories).Any())
					throw new InvalidOperationException(String.Format("Houve uma falha no merge, um ou mais arquivos foram deixados na origem ({0}).", sourceDirInfo.FullName));

				copyResult = CopyResult.Ok;
			}
			else
			{
				copyResult = CopyResult.SourceAndTargetEquals;
			}
			return copyResult;
		}
	}
}