# Hangout-NH-EF-DAPPER--demo-nh

Pessoal, perdão, mas esse projeto tem pelo menos 5 anos, portanto tem coisas que não estão da forma como eu gostaria. Uma delas (que já melhorei) é o aspecto de utilização do repositório local [nuget], pois na época meu servidor de build não tinha acesso à internet.

Essa demo conta com outros elementos muito interessantes, no entanto esse é um projeto bem antigo.

Mas entre as features interessantes esta a infraestrutura de aspecto e logs que dão suporte a toda a arquitetura.

Esse projeto conta com:
* AOP para abstração de conectividade com NHibernate, MongoDB e Redis, permitindo diversas sessões simultaneamente.
* AOP para exception handling, diferenciando Exceptions de sistema de exceptions de negócio.
* AOP para Log, utilizando RabbitMQ como endpoint para envio de logs. Saiba mais sobre a infaestrutura de logs em:
   * [Enterprise Log - Post](http://luizcarlosfaria.net/blog/docker-de-z-15-rabbitmq-logstash-elsticseach-e-kibana-com-docker-compose/)
   * [Enterprise Log - Projeto](https://github.com/docker-gallery/EnterpriseApplicationLog)
* Spring .NET
* WCF Self Hosting
* TopShelf Windows Service Hosting

Muitos recursos foram desativados para essa demo, mas esses foram as features que sobraram.

Esses elementos são parte da arquitetura que levei 10 anos para conceber, chamada Oragon Architecture. De 2004 a 2014 fui enriquecendo um projeto de arquitetura chamado Oragon Architecture. Esse projeto endereçava aspectos técnicos para o desenvolvimento de serviços. Hoje poderíamos chamar de uma arquitetura de microserviços, no entanto na época esses buzzword sequer existia.
A arquitetura destinava-se à simplificação de aspectos complexos de requisitos arquiteturais, como computação distribuída, host de serviços, automação de deploy e outros elementos que são complexos de se implementar e exigem cuidados específicos. O mindset sob o desenvolvimento de serviços é: Crie um serviço de uma única forma, sem se preocupar com peculiaridades da tecnologia que pretende expor, e do outro lado, a configuração é responsável por elencar quem é executado, quem não é, como os pontos são conectados. 

O resultado dessa arquitetura é um total desacoplamento dos serviços, permitindo a escolha, via configuração, da estratégia de deploy e configuração de ligação entre serviços, permitindo usar serviços memória ou remotamente, podendo usar os mais variados protocolos, com WCF ou não. Suporta WCF, .NET Remoting, Web Services, REST e Enterprise Services sem necessidade de mudança em código, apenas configuração. Praticamente todas essas tecnologias cairam em desuso ao longo dos anos, por sua complexidade na gestão de ambientes produtivos e necessidades específicas que interferiam na codificação, tratamento de erros e gestão de código de cada uma delas. Isso tudo não ajudou muito na sua perpetuação.

Nesse projeto vemos uma utilização máxima de IoC, DI como meio de favorecer o reaproveitamento. A escolha do Spring .NET e a utilização de XML não é ocasional. Esses projetos foram concebidos para oferecer um único core de funcionalidades, exposto das mais variadas formas e hospedado nos mais variados formatos, desde WCF, como em memória, SOAP, REST... Conta com um grade suporte e muitas features ligadas à processamento distribuído com AMQP (RabbitMQ) e oferece recursos avançados como Pipeline de Filas, RPC over AMQP e muito mais.

O projeto nunca agradou a todos, mas foi fundamental para as principais entregas que fiz nos últimos anos, pois abstrai a complexidade inerente às tecnologias que estão ao redor, oferecendo a possibilidade de programar efetivamente negócio, ignorando aspectos arquiteturais, como tratamento específico de exceções, gestão de conexões, acesso a dados, mapeamento Objeto Relacional.

Faz parte da arquitetura o gerador de código apresentado no [hangout](https://youtu.be/W2WUrvSYJhE?t=3331)  no entanto esse não é open source, mas é um dos elementos de produtividade que foram concebidos para dar suporte à aquitetura.

Esse desenho de solução me perimitiu criar projetos extremamente complexos, atender aos mais variados requisitos, eliminando a necessidade de reprogramar algo em função de decisões arquiteturais. Uma unificação do desenho de acesso a dados com MongoDB, Redis e NHibernate, forneceu um modelo consistente para que os times pudessem começar a usar tais tecnologias com baixa fricção e baixa curva de aprendizado.

O host wcf também é um recurso interessante, visto que na época era o método padrão para exposição de serviços. Toda a configuração é injetava via IoC / DI, possibilitando a criação de processos complexos com hospedagem de serviços remotamente da mesma forma como os mesmos elementos são usados para uma tarefa agendada, por exemplo. Todos esses recursos são característidos dessa abordagem de desacoplamento. Quem conhece WCF sabe que há cuidados específicos no tratamento de exceções como o lançamento de faltas em vez de exceptions cruas, e isso é uma das características resolvidas pelo framework.

Como disse, esse é mais um projeto que exemplifique um design do que uma implementação final. A versão que me basei para criar essa demo tem o projeto de arquitetura como algo monolítico, e suporta exclusivamente Spring .NET. As versões subsequentes já suportavam Ninject e SimpleInjector, elementos que abordo no [post](http://luizcarlosfaria.net/blog/oragon-architecture-application-hosting-suporte-para-nijnect-e-simpleinjector-e-qualquer-outro-container/). Essa foi uma das últimas modificações que fiz no projeto e desde então, com o .NET Core, ainda estou revendo se cabe ou não uma migração.



Esse projeto usa docker para subir RabbitMQ e Redis.

```
version: '3'

services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq/mnesia
    ports:
      - "15672:15672"
      - "5672:5672"    
    networks:
      - enterprise-app
    environment:
      RABBITMQ_DEFAULT_USER: DemoNHCoreApp
      RABBITMQ_DEFAULT_PASS: 7NZ4U5st6vtcw0DB73k3d8iqwqHA2Mni79ZEJEmCmDa1G6Wnm2
      RABBITMQ_DEFAULT_VHOST: LogEngine

  redis:
    image: redis:alpine
    ports: 
      - "6379:6379"
    networks:
      - enterprise-app

volumes:
  mysql_data:
  rabbitmq_data:

networks: 
  enterprise-app: 
    driver: bridge
```    
    
