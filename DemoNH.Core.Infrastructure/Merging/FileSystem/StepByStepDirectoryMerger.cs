using DemoNH.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Merging.FileSystem
{
	/// <summary>
	/// Realiza as operações de merge entre diretórios
	/// </summary>
	public class StepByStepDirectoryMerger
	{
		/// <summary>
		/// Realiza merge entre uma lista de diretórios, enviando-o ao final para a melhor opção na lista de targets
		/// </summary>
		/// <param name="directoriesToMerge">Lista de diretórios que precisam ser analisados e possivelmente unificados</param>
		/// <param name="possibleTarget">Listas de diretórios de destino</param>
		public Directory Merge(IEnumerable<Directory> directoriesToMerge)
		{
			Directory targetDirectoryAfterProcess = null;

			IEnumerable<Directory> directoriesFound = directoriesToMerge.Where(it => it.Exists);
			if (directoriesFound.Count() == 0)
				throw new NotSupportedException("Não é possível maniular diretórios nulos");
			else
			{
				//Obrigatoriamente para que esse else seja executado, um ou mais diretórios precisam ter sido encontrados
				Queue<Directory> directoryQueue = new Queue<Directory>();

				var allTargetsQueue = directoriesToMerge									// Fila com Todos os Targets possíveis em ordem de prioridade
					.Where(directory => directory.Type == DirectoryType.Target)				// Somente Target
					.OrderByDescending(directory => directory.Priority)						// Ordenação correta de prioridade
					.ToQueue();																// Gerando o objeto queue

				if (directoriesFound.Count() == 1)											//Se Encontrar apenas 1 item
				{
					Directory uniqueDirectoryFound = directoriesFound.First();
					if (uniqueDirectoryFound.Type == DirectoryType.Source)					//Se o único encontrado é source, devemos adicionar um target válido
					{
						directoryQueue.Enqueue(uniqueDirectoryFound);						//Enfileirando a cópia
						directoryQueue.Enqueue(allTargetsQueue.Dequeue());					//Enfileirando a cópia
					}
					else if (uniqueDirectoryFound.Type == DirectoryType.Target)
					{
						if (uniqueDirectoryFound.Priority == 0)								//Somente se o target for de prioridade Zero, então temos de realizar o merge com um target válido
						{
							directoryQueue.Enqueue(uniqueDirectoryFound);					//Enfileirando a cópia
							directoryQueue.Enqueue(allTargetsQueue.Dequeue());				//Enfileirando a cópia
						}
						else
						{
							targetDirectoryAfterProcess = uniqueDirectoryFound;
						}
					}
				}
				else
				{
					directoriesFound
						.Where(directory => directory.Type == DirectoryType.Source)			// Somente Source
						.OrderBy(directory => directory.Priority)							// Do menos prioritário para o mais prioritário
						.ForEach(directory => directoryQueue.Enqueue(directory));			// Adiciona na fila de processamento

					directoriesFound
						.Where(directory => directory.Type == DirectoryType.Target)			// Somente Target
						.OrderBy(directory => directory.Priority)							// Do menos prioritário para o mais prioritário
						.ForEach(directory => directoryQueue.Enqueue(directory));			// Adiciona na fila de processamento

					//Para um target ser valido ele precisa ter prioridade maior que zero.
					//Caso nao haja nenhum target com prioridade maior que zero, entao adicionamos o target de maior prioridade
					if (directoryQueue.Where(it => it.Type == DirectoryType.Target && it.Priority > 0).IsEmpty())
						directoryQueue.Enqueue(allTargetsQueue.Dequeue());
				}

				if (directoryQueue.Count >= 2)
				{
					Directory sourceDirectory = directoryQueue.Dequeue();					//Ontem o primeiro item da fila, removendo-o para servir de source do merge
					Directory targetDirectory = directoryQueue.Dequeue();					//Ontem o primeiro item da fila, removendo-o para servir de target do merge
					while (true)
					{
						sourceDirectory.MoveTo(targetDirectory);                            //Realiza o merge entre as partes
						targetDirectoryAfterProcess = targetDirectory;                      //Marca o último diretório target para retorno
						if (directoryQueue.Any())											//Se ainda houver itens na fila, executar a rolagem (quem era target, agora é source... e escolhemos novos targets)
						{
							sourceDirectory = targetDirectory;
							targetDirectory = directoryQueue.Dequeue();
						}
						else
						{
							if (targetDirectory.Type == DirectoryType.Source)				//Nesse ponto, chegamos ao fim da fila de processamento. O resultado que temos é nesse ponto se o target, for um item
								// do tipo Source, então temos um problema grave no código.
								throw new MergeException(string.Format("Era esperado que o último Target do merge fosse um diretório do tipo Target, no entanto o último diretório ('{0}')", targetDirectory.FullPath));
							break;
						}
					}
				}
				else
				{
					//Nesse ponto não encontrou 2 diretórios e assim não necessita de merge
				}

				return targetDirectoryAfterProcess;
			}
		}
	}
}