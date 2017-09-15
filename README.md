# Hangout-NH-EF-DAPPER--demo-nh

Pessoal, perdão, mas esse projeto tem pelo menos 5 anos, portanto tem coisas que não estão da forma como eu gostaria. Uma delas (que já melhorei) é o aspecto de utilização do repositório local [nuget], pois na época meu servidor de build não tinha acesso à internet.

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
    
