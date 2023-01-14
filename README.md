# Desafio2

## Requisição
 A comunicação com a API é feita através de uma requisição GET para o seguinte endereço, e com os seguintes parâmetros
 
```ruby 
(GET) https://api.exchangerate.host/convert?from=$1&to=$2&amount=$3&places=2
```

Onde $1, $2 e $3 são as entradas do usuário

## Interface Exemplo

```
Moeda origem:
eur
Moeda destino:
usd
Valor:
190,09
Executando.........
--------------------------------------------------
EUR 190,09 => USD 206,08
Taxa: 1,080000
--------------------------------------------------
Moeda origem:
brl
Moeda destino:
eur
Valor:
2065,70
Executando........
--------------------------------------------------
BRL 2065,70 => EUR 373,74
Taxa: 0,180000
--------------------------------------------------
Moeda origem:
aaa
Moeda destino:
bbb
Valor:
1,00
Executando.....Falha ao converter 1,00 de AAA para BBB
Resultado da operação é nulo
```

## Arquivos

* __Conversao.cs__: Classe que implementa as regras de negócio
* __Request.cs__: Classe que implementa as requisições à API
* __UserInterface.cs__: Classe que implementa a interação como usuário
* __ReadMenu.cs__: Classe implementa a entrada e validação de dados do usuário
* __Program.cs__: Entrada do Projeto
